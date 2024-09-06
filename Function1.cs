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
using System.Collections.Generic;
using recipeApp2;

namespace FunctionAPIApp
{
    public static class Function1
    {
        //���{
        [FunctionName("USERLOGIN")]
        public static async Task<IActionResult> UserLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTP���X�|���X�ŕԂ���������`
            string responseMessage = "SQL RESULT:";

            try
            {
                string user_name = req.Query["user_name"];
                string user_password = req.Query["user_password"];

                if (string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(user_password))
                {
                    return new BadRequestObjectResult("Please pass a user_name and user_password in the query string");
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "m3hkouhei2010.database.windows.net";
                builder.UserID = "kouhei0726";
                builder.Password = "Battlefield341610";
                builder.InitialCatalog = "m3h-kouhei-0726";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    // SQL�N�G���̒�`�i�p�����[�^�����ꂽ�N�G���j
                    string sql = "SELECT user_name FROM user_table WHERE user_name = @user_name AND user_password = @user_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // �p�����[�^�̒ǉ�
                        command.Parameters.AddWithValue("@user_name", user_name);
                        command.Parameters.AddWithValue("@user_password", user_password);

                        // �ڑ����J��
                        connection.Open();

                        // SQL�N�G�������s���A���ʂ��擾
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // �F�ؐ���
                                responseMessage = "Login successful";
                            }
                            else
                            {
                                // �F�؎��s
                                responseMessage = "Invalid user_name or user_password";
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                log.LogError($"SQL error: {e.ToString()}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                log.LogError($"General error: {ex.ToString()}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("EMPLOYEELOGIN")]
        public static async Task<IActionResult> EmployeeLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTP���X�|���X�ŕԂ���������`
            string responseMessage = "SQL RESULT:";

            try
            {
                string employee_id = req.Query["employee_id"];
                string employee_password = req.Query["employee_password"];

                if (string.IsNullOrEmpty(employee_id) || string.IsNullOrEmpty(employee_password))
                {
                    return new BadRequestObjectResult("Please pass a employee_id and employee_password in the query string");
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "m3hkouhei2010.database.windows.net";
                builder.UserID = "kouhei0726";
                builder.Password = "Battlefield341610";
                builder.InitialCatalog = "m3h-kouhei-0726";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    // SQL�N�G���̒�`�i�p�����[�^�����ꂽ�N�G���j
                    string sql = "SELECT employee_id FROM employee_table WHERE employee_id = @employee_id AND employee_password = @employee_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // �p�����[�^�̒ǉ�
                        command.Parameters.AddWithValue("@employee_id", employee_id);
                        command.Parameters.AddWithValue("@employee_password", employee_password);

                        // �ڑ����J��
                        connection.Open();

                        // SQL�N�G�������s���A���ʂ��擾
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // �F�ؐ���
                                responseMessage = "Login successful";
                            }
                            else
                            {
                                // �F�؎��s
                                responseMessage = "Invalid employee_id or employee_password";
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                log.LogError($"SQL error: {e.ToString()}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                log.LogError($"General error: {ex.ToString()}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(responseMessage);
        }

        //���
        [FunctionName("SEARCH")]
        public static async Task<IActionResult> Search(
     [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
     ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a search request.");

            string responseMessage = "SQL RESULT:";
            string recipeCategory = req.Query["recipe_category"];
            string recipeTime = req.Query["recipe_time"];
            string recipeScene = req.Query["recipe_scene"];


            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "m3hkouhei2010.database.windows.net";
                builder.UserID = "kouhei0726";
                builder.Password = "Battlefield341610";
                builder.InitialCatalog = "m3h-kouhei-0726";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT recipe_id, recipe_name, recipe_category1, recipe_time, recipe_scene1 " +
                                 "FROM recipe_table WHERE 1=1";

                    if (!string.IsNullOrEmpty(recipeCategory))
                    {
                        sql += " AND (recipe_category1 = @recipeCategory OR recipe_category2 = @recipeCategory OR recipe_category3 = @recipeCategory)";
                        log.LogInformation("Added recipeCategory to query: {sql}", sql);
                    }

                    if (!string.IsNullOrEmpty(recipeTime))
                    {
                        sql += " AND recipe_time = @recipeTime";
                        log.LogInformation("Added recipeTime to query: {sql}", sql);
                    }

                    if (!string.IsNullOrEmpty(recipeScene))
                    {
                        sql += " AND (recipe_scene1 = @recipeScene OR recipe_scene2 = @recipeScene OR recipe_scene3 = @recipeScene)";
                        log.LogInformation("Added recipeScene to query: {sql}", sql);
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(recipeCategory))
                        {
                            command.Parameters.AddWithValue("@recipeCategory", recipeCategory);
                            log.LogInformation("Set recipeCategory parameter: {recipeCategory}", recipeCategory);
                        }

                        if (!string.IsNullOrEmpty(recipeTime))
                        {
                            command.Parameters.AddWithValue("@recipeTime", recipeTime);
                            log.LogInformation("Set recipeTime parameter: {recipeTime}", recipeTime);
                        }

                        if (!string.IsNullOrEmpty(recipeScene))
                        {
                            command.Parameters.AddWithValue("@recipeScene", recipeScene);
                            log.LogInformation("Set recipeScene parameter: {recipeScene}", recipeScene);
                        }

                        connection.Open();
                        log.LogInformation("Database connection opened.");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var resultList = new List<object>();

                            while (reader.Read())
                            {
                                var recipe = new
                                {
                                    RecipeID = reader.GetInt32(0),
                                    RecipeName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    RecipeCategory1 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    RecipeTime = reader.IsDBNull(3) ? null : reader.GetInt32(3).ToString(),
                                    RecipeScene1 = reader.IsDBNull(4) ? null : reader.GetString(4)
                                };

                                resultList.Add(recipe);
                            }

                            responseMessage = JsonConvert.SerializeObject(resultList);
                            log.LogInformation("Query executed successfully. Number of records: {count}", resultList.Count);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //log.LogError("SQL Exception: {message}", e.Message);  // �G���[���O�̏o��
                responseMessage = "�G���[���������܂����B";
            }

            return new OkObjectResult(responseMessage);
        }


        //��
        [FunctionName("FAVORITESELECT")]
        public static async Task<IActionResult> FavoriteSelect(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            string responseMessage = "SQL RESULT:";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = "m3hkouhei2010.database.windows.net",
                    UserID = "kouhei0726",
                    Password = "Battlefield341610",
                    InitialCatalog = "m3h-kouhei-0726"
                };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT favorite_id, user_id, recipe_id FROM favorite_table";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            favorite_tableList resultList = new favorite_tableList();

                            while (reader.Read())
                            {
                                resultList.List.Add(new favorite_tableRow
                                {
                                    favorite_id = reader.GetInt32(0), //("favorite_id"),
                                    user_id = reader.GetInt32(1), //("user_id"),
                                    recipe_id = reader.GetInt32(2), //("recipe_id"),
                                });
                            }

                            responseMessage = JsonConvert.SerializeObject(resultList);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "�f�[�^�x�[�X�ڑ��G���[���������܂����B";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "�\�����Ȃ��G���[���������܂����B";
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RECIPESELECT")]
        public static async Task<IActionResult> RecipeSelect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = "SQL RESULT:";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = "m3hkouhei2010.database.windows.net",
                    UserID = "kouhei0726",
                    Password = "Battlefield341610",
                    InitialCatalog = "m3h-kouhei-0726"
                };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT recipe_name, recipe_category1, recipe_category2, recipe_category3, " +
                        "recipe_time, recipe_scene1, recipe_scene2, recipe_scene3, " +
                        "recipe_item1, recipe_item2, recipe_item3 FROM recipe_table";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            recipe_tableList resultList = new recipe_tableList();

                            while (reader.Read())
                            {
                                resultList.List.Add(new recipe_tableRow
                                {
                                    recipe_name = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    recipe_category1 = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    recipe_category2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    recipe_category3 = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    recipe_time = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                    recipe_scene1 = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    recipe_scene2 = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    recipe_scene3 = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    recipe_item1 = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    recipe_item2 = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    recipe_item3 = reader.IsDBNull(10) ? null : reader.GetString(10)
                                });
                            }

                            responseMessage = JsonConvert.SerializeObject(resultList);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "�f�[�^�x�[�X�ڑ��G���[���������܂����B";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "�\�����Ȃ��G���[���������܂����B";
            }

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("FAVORITEINSERT")]
        public static async Task<IActionResult> FavoriteInsert(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "FAVORITE RESULT: ";
            string favorite_id = req.Query["favorite_id"];
            string user_id = req.Query["user_id"];
            string recipe_id = req.Query["recipe_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            favorite_id = favorite_id ?? data?.favorite_id;
            user_id = user_id ?? data?.user_id;
            recipe_id = recipe_id ?? data?.recipe_id;

            if (!string.IsNullOrWhiteSpace(user_id) && !string.IsNullOrWhiteSpace(recipe_id))
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                    {
                        DataSource = "m3hkouhei2010.database.windows.net",
                        UserID = "kouhei0726",
                        Password = "Battlefield341610",
                        InitialCatalog = "m3h-kouhei-0726"
                    };

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        string sql = "INSERT INTO favorite_table (favorite_id, user_id, recipe_id) VALUES (@favorite_id, @user_id, @recipe_id)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            // �����񂩂琮���ւ̕ϊ�
                            int favoriteIdValue = int.Parse(favorite_id);
                            int userIdValue = int.Parse(user_id);
                            int recipeIdValue = int.Parse(recipe_id);

                            command.Parameters.AddWithValue("@favorite_id", favoriteIdValue);
                            command.Parameters.AddWithValue("@user_id", userIdValue);
                            command.Parameters.AddWithValue("@recipe_id", recipeIdValue);

                            connection.Open();
                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            responseMessage = rowsAffected > 0
                                ? "���V�s�����C�ɓ���ɓo�^����܂����B"
                                : "���C�ɓ���o�^�Ɏ��s���܂����B";
                        }
                    }
                }
                catch (SqlException e)
                {
                    log.LogError(e.ToString());
                    responseMessage = "�G���[���������܂����B";
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                    responseMessage = "�\�����Ȃ��G���[���������܂����B";
                }
            }
            else
            {
                responseMessage = "�p�����[�^�[���ݒ肳��Ă��܂���";
            }

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("RECIPEJOIN")]
        public static async Task<IActionResult> RecipeJoin(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
            ILogger log)
        {

            string connectionString = "Server=tcp:m3hkouhei2010.database.windows.net,1433;Initial Catalog=m3h-kouhei-0726;Persist Security Info=False;User ID=kouhei0726;Password=Battlefield341610;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            List<YourDataModel> result = new List<YourDataModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                 SELECT
                    recipe_table.recipe_name,
                    recipe_table.recipe_time
                FROM 
                    recipe_table
                JOIN 
                    favorite_table
                ON recipe_table.recipe_id=favorite_table.recipe_id;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var data = new YourDataModel
                            {
                                recipe_name = reader.GetString(0),
                                recipe_time = reader.GetInt32(1)
                            };
                            result.Add(data);
                        }
                    }
                }
            }

            return new OkObjectResult(result);
        }

        public class YourDataModel
        {
            public string recipe_name { get; set; }
            public int recipe_time { get; set; }
        }


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

