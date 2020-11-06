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
        public async Task InboxAsync(
            [FromBody] ProductInfo productInfo,
            [FromServices] DaprClient daprClient,
            [FromServices] ILogger<DaprController> logger)
        {
            try
            {
                await daprClient.InvokeBindingAsync(
                        "azurestorage",
                        "create",
                        Template(productInfo),
                           metadata: new Dictionary<string, string>
                           {
                                { "blobName", $"product-{productInfo.Id}.html" },
                                { "ContentType", "text/html" }
                           });
            }
            catch (System.Exception ex)
            {
                logger.LogInformation("Error updating product {productId}-{productTitle}", productInfo.Id, productInfo.Title);
                logger.LogError(ex, "Error updating product {productId}-{productTitle}", productInfo.Id, productInfo.Title);
                throw;
            }

        }

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
