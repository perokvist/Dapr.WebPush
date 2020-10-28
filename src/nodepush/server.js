"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const dapr_client_1 = __importDefault(require("dapr-client"));
const grpc_1 = __importDefault(require("grpc"));
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const daprGrpcPort = process.env.DAPR_GRPC_PORT || 50001;
const port = 3000;
const app = express_1.default();
const client = new dapr_client_1.default.dapr_grpc.DaprClient(`localhost:${daprGrpcPort}`, grpc_1.default.credentials.createInsecure());
const messages = dapr_client_1.default.dapr_pb;
app.use(body_parser_1.default.json({ type: 'application/*+json' }));
app.post('/in', (req, res) => {
    const data = req.body.data;
    console.log("Got a new product");
    console.dir(data);
    const binding = new messages.InvokeBindingRequest();
    binding.setName('azurestorage');
    binding.setData(Buffer.from(JSON.stringify(data)));
    binding.setOperation('create');
    const metaMap = binding.getMetadataMap();
    metaMap.set("blobName", "product-test.html"); //-"+ data.Id );
    metaMap.set("ContentType", "text/html");
    client.invokeBinding(binding, (err, response) => {
        if (err) {
            console.dir(binding);
            console.log(`Error binding: ${err}`);
        }
        else {
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
});
app.get("/in", (req, res) => {
    res.json({
        message: 'Hello Web Push!'
    });
});
app.listen(port, () => {
    // tslint:disable-next-line:no-console
    console.log(`server started at http://localhost:${port}`);
});
//# sourceMappingURL=server.js.map