import express, { Application, Request, Response } from 'express';
import nunjucks from 'nunjucks';

// router
import { lunchRouter } from './LunchRouter';

// 익스프레스 어플리케이션 <- 이건 웹 서버
let app:Application = express();

app.set('view engine', 'njk');
nunjucks.configure('views', { express: app, watch: true });

// Get, Post, Put, Delete => Method
// CRUD -> Create, Read, Update, Delete
// Application 내에서 구현한 CRUD == API
// URI Get(Read), Post(Create), Put(Update), Delete(Delete)
// RESTful API

app.use(lunchRouter);

app.listen(8081, () => {
    console.log(
    `
    #########################################
    #   Server is starting on 8181 port     #
    #   http://localhost:8081/              #
    #########################################
    `);
    
}); 