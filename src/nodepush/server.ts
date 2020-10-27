import dapr from 'dapr-client';
import grpc from "grpc";
import express from "express";
import bodyParser from "body-parser";

const daprGrpcPort = process.env.DAPR_GRPC_PORT || 50001;
const stateStoreName = `statestore`;

const port = 3000;
const app = express();

app.use(bodyParser.json());

var client = new dapr.dapr_grpc.DaprClient(
    `localhost:${daprGrpcPort}`, grpc.credentials.createInsecure());

app.post('/in', (req, res) => {
    const data = req.body.data;
    console.log("Got a new product: " + data);
    res.status(200).send();
});

app.get('/dapr/subscribe', (req, res) => {
    res.json([
        {
            pubsubname: "pubsub",
            topic: "in",
            route: "in"
        }
    ]);
})

app.get("/in", (req, res) => {
    res.json({
        message: 'Hello Web Push!'
    });
});

app.listen(port, () => {
    // tslint:disable-next-line:no-console
    console.log(`server started at http://localhost:${port}`);
});