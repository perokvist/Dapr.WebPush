import dapr from 'dapr-client';
import grpc from "grpc";
import express from "express";
import bodyParser from "body-parser";

const daprGrpcPort = process.env.DAPR_GRPC_PORT || 50001;
const stateStoreName = `statestore`;

const port = 3000;
const app = express();

app.use(bodyParser.json());


const client = new dapr.dapr_grpc.DaprClient(
    `localhost:${daprGrpcPort}`, grpc.credentials.createInsecure());
const messages = dapr.dapr_pb;

app.post('/in', (req, res) => {
    const data = req.body.data;
    console.dir(req.body.data);
    console.dir(req.body);
    console.log("Got a new product: " + data);
    const binding = new messages.InvokeBindingRequest();
    binding.setName('azurestorage');
    binding.setData(data);
    binding.setOperation('create')
    const metaMap = binding.getMetadataMap();
    metaMap.set("blobName", "product-"+ data.Id );
    metaMap.set("ContentType", "text/html");

    client.invokeBinding(binding, (err, response) => {
        if (err) {
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