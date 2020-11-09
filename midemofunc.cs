using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;

namespace midemoapp
{
    public static class midemofunc
    {
        [FunctionName("midemofunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var token = await new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/");
            var connectionstring = Environment.GetEnvironmentVariable("dbconnection");
            using (var connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.AccessToken = token;
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT GETDATE()";
                    using (var sr = cmd.ExecuteReader())
                    {
                        sr.Read();
                        string responseMessage = sr.GetDateTime(0).ToString();

                        return new OkObjectResult(responseMessage);

                    }
                }
                catch(Exception ex)
                {
                    log.LogError(ex, ex.Message);
                    return new BadRequestObjectResult(ex);
                }
            }
        }
    }
}
