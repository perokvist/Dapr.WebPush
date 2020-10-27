using Dapr;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace push
{
    [ApiController]
    public class DaprController : ControllerBase
    {
        [Topic("pubsub", "in")]
        [HttpPost("in")]
        public Task InboxAsync(ProductInfo productInfo, [FromServices]DaprClient daprClient)
         => daprClient.InvokeBindingAsync(
             "azurestorage",
             "create",
             Template(productInfo),
             metadata: new Dictionary<string, string>
                {
                    { "blobName", "index.html" },
                    { "ContentType", "text/html" }
            });

        public static string Template(ProductInfo info)
            => $"<html><body><h1>{info.Title}</h1><div>{info.Price}</div></body></html>";
    }

    public class ProductInfo
    {
        public string Title { get; set; }

        public int Price { get; set; }
    }
}
