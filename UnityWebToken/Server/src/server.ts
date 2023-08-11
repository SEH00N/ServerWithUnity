import express, { Application, Request, Response } from 'express';
import nunjucks from 'nunjucks';

//middleware
import { tokenChecker } from './MyJWT';

// router
import { lunchRouter } from './LunchRouter';
import { userRouter } from './UserRouter';
import { invenRouter } from './inventoryRouter';

// 익스프레스 어플리케이션 <- 이건 웹 서버
let app:Application = express();

app.set('view engine', 'njk');
nunjucks.configure('views', { express: app, watch: true });

app.use(express.json()); // post 데이터를 json형태로 파싱한다
app.use(express.urlencoded({extended:true}));

app.use(tokenChecker);

// Get, Post, Put, Delete => Method
// CRUD -> Create, Read, Update, Delete
// Application 내에서 구현한 CRUD == API
// URI Get(Read), Post(Create), Put(Update), Delete(Delete)
// RESTful API

app.use(lunchRouter);
app.use(userRouter);
app.use(invenRouter);

app.listen(8081, () => {
    console.log(
    `
    #########################################
    #   Server is starting on 8181 port     #
    #   http://localhost:8081/              #
    #########################################
    `);
    
}); 