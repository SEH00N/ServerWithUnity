import { Router, Request, Response, NextFunction } from "express";
import { Pool } from "./DB";
import { InventoryVO, MessageType, ResponseMSG } from "./Types";
import { ResultSetHeader, RowDataPacket } from "mysql2";

export const invenRouter = Router();

invenRouter.get('/inven', async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:'권한이 없습니다.'});
        return;   
    }

    const user = req.user;
    let sql = 'SELECT json FROM inventories WHERE user_id = ?';
    let[rows, col] : [RowDataPacket[], any] = await Pool.query(sql, [user.id]);

    if(rows.length == 0)
    {
        res.json({type:MessageType.EMPTY, message:''});
    } else {
        let json = rows[0].json as string;
        res.json({type:MessageType.SUCCESS, message:json});
    }
});

invenRouter.post('/inven', async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:'권한이 없습니다.'});
        return;   
    }

    const user = req.user;

    let json = req.body.json;
    let vo:InventoryVO = JSON.parse(json);

    console.log(vo);
    let [rows, cols]:[RowDataPacket[], any] = await Pool.query("SELECT * FROM newinventories WHERE user_id = ?", [user.id]);

    if(rows.length == 0)
    {
        let insertSQL = `INSERT INTO newinventories (user_id, slot_number, item_code, count) VALUES `;
        let bindDataArr = [];
        for(let i = 0; i < vo.count; i++)
        {
            insertSQL += (i == vo.count - 1) ? `(?, ?, 0, 0)` : `(?, ?, 0, 0), `;
            bindDataArr.push(user.id);
            bindDataArr.push(i);
        }

        let [result, info] : [ResultSetHeader, any] = await Pool.execute(insertSQL, bindDataArr);
    }
    else {
        let updateSQL = "UPDATE newinventories SET item_code = 0, count = 0 WHERE user_id = ?";
        await Pool.execute(updateSQL, [user.id]);
    }

    let updateSQL = "UPDATE newinventories SET item_code = ?, count = ? WHERE slot_number = ? AND user_id = ?";

    for(let i = 0; i < vo.list.length; i++)
    {
        const item = vo.list[i];
        await Pool.execute(updateSQL, [item.itemCode, item.count, item.slotNumber, user.id]);
    }

    const sql = `INSERT INTO inventories (user_id, json) VALUES (?, ?)
                    ON DUPLICATE KEY UPDATE json = VALUES(json)`;

    let [result, info]:[ResultSetHeader, any] = await Pool.execute(sql, [user.id, JSON.stringify(vo)]);
    let msg:ResponseMSG = {type:MessageType.SUCCESS, message:'인벤토리 저장 완료'};
    res.json(msg);
});