using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;//DB�ڑ��p���C�u����
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace FunctionAPIApp
{
    public static class Function1
    {
        //���{
        




        //���
        


        //��
        



        //���J
        [FunctionName("USERINSERT")]
        public static async Task<IActionResult> UserInsert(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "INSERT RESULT: ";
            string user_id = req.Query["user_id"];
            string user_name = req.Query["user_name"];
            string user_password = req.Query["user_password"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            user_id = user_id ?? data?.user_id;
            user_name = user_name ?? data?.user_name;
            user_password = user_password ?? data?.user_password;

            if (!string.IsNullOrWhiteSpace(user_id) && !string.IsNullOrWhiteSpace(user_name) && !string.IsNullOrWhiteSpace(user_password))
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "m3hkouhei2010.database.windows.net";
                    builder.UserID = "kouhei0726";
                    builder.Password = "Battlefield341610";
                    builder.InitialCatalog = "m3h-kouhei-0726";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        String sql = "INSERT INTO user_table(user_id, user_name, user_password) Values(@user_id, @user_name, @user_password)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@user_id", int.Parse(user_id));
                            command.Parameters.AddWithValue("@user_name", user_name);
                            command.Parameters.AddWithValue("@user_password", user_password);

                            connection.Open();

                            int result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                responseMessage += "���[�U�[��񂪓o�^����܂���";
                            }
                            else
                            {
                                responseMessage += "No rows were inserted.";
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    log.LogError($"SQL Exception: {e.Message}");
                    responseMessage += $"SQL Error: {e.Message}";
                }
                catch (Exception e)
                {
                    log.LogError($"Exception: {e.Message}");
                    responseMessage += $"Error: {e.Message}";
                }
            }
            else
            {
                responseMessage = "�p�����[�^�[���ݒ肳��Ă��܂���";
            }

            return new OkObjectResult(responseMessage);
        }



        [FunctionName("RECIPEDELETE")]
        public static async Task<IActionResult> RecipeDelete(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
       ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "DELETE RESULT:";
            string recipe_id = req.Query["recipe_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            recipe_id = recipe_id ?? data?.recipe_id;

            if (int.TryParse(recipe_id, out int id))
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "m3hkouhei2010.database.windows.net";
                    builder.UserID = "kouhei0726";
                    builder.Password = "Battlefield341610";
                    builder.InitialCatalog = "m3h-kouhei-0726";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        string sql = "DELETE FROM recipe_table WHERE recipe_id = @recipe_id";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@recipe_id", id);

                            connection.Open();

                            int result = command.ExecuteNonQuery();

                            responseMessage = result > 0 ? "�^�X�N���폜����܂����B" : "�^�X�N��������܂���B";
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    responseMessage = "�G���[���������܂����B";
                }
            }
            else
            {
                responseMessage = "�L���ȃ^�X�NID���񋟂���Ă��܂���B";
            }

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("FAVORITEDELETE")]
        public static async Task<IActionResult> FavoriteDelete(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "DELETE RESULT:";
            string user_id = req.Query["user_id"];
            string recipe_id = req.Query["recipe_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            user_id = user_id ?? data?.user_id;
            recipe_id = recipe_id ?? data?.recipe_id;

            if (int.TryParse(user_id, out int userId) && int.TryParse(recipe_id, out int recipeId))
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "m3hkouhei2010.database.windows.net";
                    builder.UserID = "kouhei0726";
                    builder.Password = "Battlefield341610";
                    builder.InitialCatalog = "m3h-kouhei-0726";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        string sql = "DELETE FROM favorite_table WHERE user_id = @user_id AND recipe_id = @recipe_id";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@user_id", userId);
                            command.Parameters.AddWithValue("@recipe_id", recipeId);

                            connection.Open();

                            int result = command.ExecuteNonQuery();

                            responseMessage = result > 0 ? "���C�ɓ��肪��������܂����B" : "�Y���̂��C�ɓ��肪������܂���B";
                        }
                    }
                }
                catch (SqlException e)
                {
                    log.LogError($"SQL Exception: {e.Message}");
                    responseMessage = $"�G���[���������܂���: {e.Message}";
                }
                catch (Exception e)
                {
                    log.LogError($"Exception: {e.Message}");
                    responseMessage = $"�\�����Ȃ��G���[���������܂���: {e.Message}";
                }
            }
            else
            {
                responseMessage = "�L���ȃ��[�U�[ID�܂��̓��V�sID���񋟂���Ă��܂���B";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}

