using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace push
{
    [ApiController]
    public class DaprController : ControllerBase
    {
        [Topic("azurepubsub", "in")]
        [HttpPost("in")]
        public Task InboxAsync([FromBody] ProductInfo productInfo, [FromServices] DaprClient daprClient)
            => daprClient.InvokeBindingAsync(
               "azurestorage",
               "create",
               Template(productInfo),
                  metadata: new Dictionary<string, string>
                  {
                    { "blobName", $"product-{productInfo.Id}.html" },
                    { "ContentType", "text/html" }
                  });

        public static string Template(ProductInfo info)
            => $"<html><body><h1>{info.Title}</h1><div>{info.Price}</div></body></html>";
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
    }
}
