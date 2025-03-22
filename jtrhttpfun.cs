using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZ_204
{
    public class jtrhttpfun
    {
        private readonly ILogger<jtrhttpfun> _logger;

        public jtrhttpfun(ILogger<jtrhttpfun> logger)
        {
            _logger = logger;
        }

        [Function("jtrhttpfun")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (req == null)
            {
                _logger.LogError("Request object is null");
                return new BadRequestObjectResult("Request object cannot be null");
            }

            string requestBody;
            try
            {
                requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                if (string.IsNullOrEmpty(requestBody))
                {
                    _logger.LogWarning("Request body is empty");
                    return new BadRequestObjectResult("Request body cannot be empty");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading request body");
                return new BadRequestObjectResult("Error reading request body");
            }

            RequestModel data;
            try
            {
                data = System.Text.Json.JsonSerializer.Deserialize<RequestModel>(requestBody);
                if (data == null)
                {
                    _logger.LogWarning("Could not deserialize request body");
                    return new BadRequestObjectResult("Invalid request body format");
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error");
                return new BadRequestObjectResult("Invalid JSON format");
            }

            if (string.IsNullOrEmpty(data.Description))
            {
                _logger.LogWarning("Description is required");
                return new BadRequestObjectResult("Description is required");
            }

            if (data.CategoryId <= 0)
            {
                _logger.LogWarning("Invalid CategoryId");
                return new BadRequestObjectResult("CategoryId must be greater than 0");
            }

            return new OkObjectResult($"Received description: {data.Description}, categoryId: {data.CategoryId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing request");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        }

        private class RequestModel
        {
            public string Description { get; set; }
            public int CategoryId { get; set; }
        }
    }
}
