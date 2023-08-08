import { Router, Request, Response } from 'express';
import axios from 'axios';
import { load, CheerioAPI } from 'cheerio';
import iconv from 'iconv-lite';
import { Pool } from './DB';
import { RowDataPacket } from 'mysql2/promise';

export const lunchRouter = Router();

lunchRouter.get('/lunch', async (req:Request, res:Response) => {

    let arr:number[] = process.hrtime();
    
    const date:string = '20230703';
    const url:string = `https://ggm.hs.kr/lunch.view?date=${date}`;

    let html = await axios({ url: url, method: 'GET', responseType:'arraybuffer' });
    
    // 데이터 통신은 byte 스트림으로 통신함
    let data:Buffer = html.data;
    let decoded = iconv.decode(data, 'euc-kr');

    // HTML 문자열을 Cheerio를 통해 파싱
    const $:CheerioAPI = load(decoded);
    let text:string = $('.menuName > span').text();
    let menus:string[] = text.split('\n').map(x => x.replace(/[0-9]+\./g, '')).filter(x => x.length > 0);

    GetDataFromDB('20230723');

    let arr2:number[] = process.hrtime();
    let secDelta = arr2[0] - arr[0];
    let nanoDelta = arr2[1] - arr[1] + secDelta * 1000000000;
    res.render('lunch', {menus: menus, date: date, time: nanoDelta});
});

async function GetDataFromDB(date:string)
{
    const sql:string = 'SELECT * FROM lunch WHERE date = ?';
    let [row, col] = await Pool.query(sql, [date]);
    row = row as RowDataPacket[];
    
    return row.length > 0 ? row : null;
}