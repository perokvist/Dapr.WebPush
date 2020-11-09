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

        public static string Template(ProductInfo info) =>
@$"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset = ""UTF-8"" >
  < meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Product</title>
  <link rel = ""stylesheet"" href=""product.css"" />
</head>
<body>
  
  <section id = ""product-page"" class=""product-wrapper"" itemscope="""" itemtype=""http://schema.org/Product"" data-ref-page=""productpage"">
    <div class=""product"">
      <img src = ""monitor.png"" class=""product-image""/>
      <div class=""product-info"">
        <h2 class=""title"">
          {info.Title}
        </h2>
        <div class=""price"">{info.Price}</div>
        <button class=""buy-btn"">BUY</button>
      </div>
    </div>
  </section>
</body>
</html>";
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
    }
}
