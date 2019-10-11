using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using IOTLabo3.Models;

namespace IOTLabo3
{
    public static class AddRegistration
    {
        [FunctionName("AddRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/registration")] HttpRequest req,
            ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

                string json = await new StreamReader(req.Body).ReadToEndAsync();
                Registration newRegistration = JsonConvert.DeserializeObject<Registration>(json);
                newRegistration.RegistrationId = Guid.NewGuid().ToString();

                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "insert into Registrations values(@regid,@lastname,@firstname,@email" +
                            ",@zipcode,@age,@isFirstTimer)";

                        command.Parameters.AddWithValue("regid", newRegistration.RegistrationId);
                        command.Parameters.AddWithValue("lastname", newRegistration.LastName);
                        command.Parameters.AddWithValue("firstname", newRegistration.FirstName);
                        command.Parameters.AddWithValue("email", newRegistration.Email);
                        command.Parameters.AddWithValue("zipcode", newRegistration.Zipcode);
                        command.Parameters.AddWithValue("age", newRegistration.Age);
                        command.Parameters.AddWithValue("isFirstTimer", newRegistration.IsFirstTimer);

                        await command.ExecuteNonQueryAsync();

                    }
                    return new OkObjectResult("");
                }
            }catch (Exception ex)
            {
                log.LogError(ex, "AddRegistration");
                return new StatusCodeResult(500);
            }

        }
    }
}
