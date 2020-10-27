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
        private readonly DaprClient daprClient;

        public DaprController(DaprClient daprClient)
        {
            this.daprClient = daprClient;
        }

        [Topic("pubsub", "in")]
        [HttpPost("in")]
        public async Task<IActionResult> InboxAsync(ProductInfo productInfo)
        {
            var d = new Dictionary<string, string>
            {
                { "blobName", "index.html" },
                { "ContentType", "text/html" }
            };
            await daprClient.InvokeBindingAsync("azurestorage", "create", Template(productInfo), metadata: d);
            return Ok();
        }

        public static string Template(ProductInfo info)
            => $"<html><body><h1>{info.Title}</h1><div>{info.Price}</div></body></html>";
    }

    public class ProductInfo
    {
        public string Title { get; set; }

        public int Price { get; set; }
    }
}
