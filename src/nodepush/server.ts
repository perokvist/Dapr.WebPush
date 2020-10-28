import { Any } from 'google-protobuf/google/protobuf/any_pb';

import dapr from 'dapr-client';
import grpc from "grpc";
import express from "express";
import bodyParser from "body-parser";

const daprGrpcPort = process.env.DAPR_GRPC_PORT || 50001;

const port = 3000;
const app = express();
const client = new dapr.dapr_grpc.DaprClient(
    `localhost:${daprGrpcPort}`, grpc.credentials.createInsecure());
const messages = dapr.dapr_pb;

app.use(bodyParser.json({ type: 'application/*+json' }));

app.post('/in', (req, res) => {
    const data = req.body.data;
    console.log("Got a new product");
    console.dir(data);
    const binding = new messages.InvokeBindingRequest();
    binding.setName("azurestorage");
    binding.setData(Buffer.from(JSON.stringify(data)));
    binding.setOperation("create");
    const metaMap = binding.getMetadataMap();
    //metaMap.set("key", "val");
    metaMap.set("blobName", "producttest-"+ data.Id + ".html" );
    metaMap.set("ContentType", "text/html");
    client.invokeBinding(binding, (err, response) => {
        if (err) {
            console.dir(binding);
            console.log(`Error binding: ${err}`);
        } else {
            console.log('File uploaded!');
        }
    });

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