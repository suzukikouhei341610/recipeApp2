using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;//DB接続用ライブラリ
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.Generic;
using recipeApp2;


namespace FunctionAPIApp
{
    public static class Function1
    {
        //松本
        [FunctionName("USERLOGIN")]
        public static async Task<IActionResult> UserLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req,
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTPレスポンスで返す文字列を定義
            string responseMessage = "SQL RESULT:";

            try
            {
                // リクエストボディからJSONデータを読み取る
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                //リクエストボディから受け取る
                string user_name = data?.user_name;
                string user_password = data?.user_password;

                //クエリから受け取る
                //string user_name = req.Query["user_name"];
                //string user_password = req.Query["user_password"];

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
                    // SQLクエリの定義（パラメータ化されたクエリ）
                    string sql = "SELECT COUNT(*) FROM user_table WHERE user_name = @user_name AND user_password = @user_password";
                    //string sql = "SELECT user_name FROM user_table WHERE user_name = @user_name AND user_password = @user_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // パラメータの追加
                        command.Parameters.AddWithValue("@user_name", user_name);
                        command.Parameters.AddWithValue("@user_password", user_password);

                        // 接続を開く
                        connection.Open();

                        // SQLクエリを実行し、結果を取得
                        //using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int userCount = (int)command.ExecuteScalar();
                            if (userCount > 0)
                                //if (reader.HasRows)
                            {
                                // 認証成功時にトークンを生成
                                //var tokenService = new TokenService();
                                //string token = tokenService.GenerateToken(user_name);
                                //log.LogInformation($"Generated token: {token}");
                                //return new OkObjectResult(new { token = token });


                                //string token = GenerateToken(user_name); // トークン生成のロジックを実装
                                //return new OkObjectResult(new { token = token });

                                // 認証成功
                                responseMessage = "Login successful";
                            }
                            else
                            {
                                // 認証失敗
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
                //log.LogError($"General error: {ex.ToString()}");
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                log.LogError($"General error: {ex.ToString()}"); // エラー詳細をログに記録
                return new ObjectResult($"An error occurred: {ex.Message}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("EMPLOYEELOGIN")]
        public static async Task<IActionResult> EmployeeLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTPレスポンスで返す文字列を定義
            string responseMessage = "SQL RESULT:";

            try
            {
                // リクエストボディからJSONデータを読み取る
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                //リクエストボディから受け取る
                string employee_id = data?.employee_id;
                string employee_password = data?.employee_password;

                //クエリから受け取る
                //string user_name = req.Query["user_name"];
                //string user_password = req.Query["user_password"];

                if (string.IsNullOrEmpty(employee_id) || string.IsNullOrEmpty(employee_password))
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
                    // SQLクエリの定義（パラメータ化されたクエリ）
                    string sql = "SELECT COUNT(*) FROM employee_table WHERE employee_id = @employee_id AND employee_password = @employee_password";
                    //string sql = "SELECT user_name FROM user_table WHERE user_name = @user_name AND user_password = @user_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // パラメータの追加
                        command.Parameters.AddWithValue("@employee_id", employee_id);
                        command.Parameters.AddWithValue("@employee_password", employee_password);

                        // 接続を開く
                        connection.Open();

                        // SQLクエリを実行し、結果を取得
                        //using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int userCount = (int)command.ExecuteScalar();
                            if (userCount > 0)
                            //if (reader.HasRows)
                            {
                                // 認証成功時にトークンを生成
                                //string token = GenerateToken(user_name); // トークン生成のロジックを実装
                                //return new OkObjectResult(new { token = token });

                                // 認証成功
                                responseMessage = "Login successful";
                            }
                            else
                            {
                                // 認証失敗
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

        [FunctionName("LOGINIDSELECT")]
        public static async Task<IActionResult> LoginIdSelect(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)

        {
            string responseMessage = "SQL RESULT:";
            string user_name = null;

            // クエリパラメータからuser_nameを取得
            if (req.Query.ContainsKey("user_name"))
            {
                user_name = req.Query["user_name"];
            }
            else
            {
                // リクエストボディからuser_nameを取得（POSTメソッドの場合）
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                user_name = data?.user_name;
            }

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
                    string sql = "SELECT user_id FROM user_table WHERE user_name = @user_name;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user_name", user_name);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var resultList = new List<int>();

                            while (reader.Read())
                            {
                                resultList.Add(reader.GetInt32(0));
                            }

                            responseMessage = JsonConvert.SerializeObject(resultList);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "データベース接続エラーが発生しました。";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "予期しないエラーが発生しました。";
            }

            return new OkObjectResult(responseMessage);
        }


        //鈴木
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
                //log.LogError("SQL Exception: {message}", e.Message);  // エラーログの出力
                responseMessage = "エラーが発生しました。";
            }

            return new OkObjectResult(responseMessage);
        }


        //碇
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
                responseMessage = "データベース接続エラーが発生しました。";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "予期しないエラーが発生しました。";
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
                    string sql = "SELECT recipe_id, recipe_name, recipe_category1, recipe_category2, recipe_category3, " +
                        "recipe_time, recipe_scene1, recipe_scene2, recipe_scene3, " +
                        "recipe_item1, recipe_item2, recipe_item3, recipe_photo FROM recipe_table";

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
                                    recipe_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    recipe_name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    recipe_category1 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    recipe_category2 = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    recipe_category3 = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    recipe_time = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    recipe_scene1 = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    recipe_scene2 = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    recipe_scene3 = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    recipe_item1 = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    recipe_item2 = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    recipe_item3 = reader.IsDBNull(11) ? null : reader.GetString(11),
                                    recipe_photo = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                responseMessage = "データベース接続エラーが発生しました。";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "予期しないエラーが発生しました。";
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
                            // 文字列から整数への変換
                            int favoriteIdValue = int.Parse(favorite_id);
                            int userIdValue = int.Parse(user_id);
                            int recipeIdValue = int.Parse(recipe_id);

                            command.Parameters.AddWithValue("@favorite_id", favoriteIdValue);
                            command.Parameters.AddWithValue("@user_id", userIdValue);
                            command.Parameters.AddWithValue("@recipe_id", recipeIdValue);

                            connection.Open();
                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            responseMessage = rowsAffected > 0
                                ? "レシピがお気に入りに登録されました。"
                                : "お気に入り登録に失敗しました。";
                        }
                    }
                }
                catch (SqlException e)
                {
                    log.LogError(e.ToString());
                    responseMessage = "エラーが発生しました。";
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                    responseMessage = "予期しないエラーが発生しました。";
                }
            }
            else
            {
                responseMessage = "パラメーターが設定されていません";
            }

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("RECIPEJOIN")]
        public static async Task<IActionResult> RecipeJoin(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
            ILogger log)
        {

            string connectionString = "Server=tcp:m3hkouhei2010.database.windows.net,1433;Initial Catalog=m3h-kouhei-0726;Persist Security Info=False;User ID=kouhei0726;Password=Battlefield341610;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=100;";

            List<YourDataModel> result = new List<YourDataModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                 SELECT
                    recipe_table.recipe_name,
                    recipe_table.recipe_time,
                    recipe_table.recipe_photo
                        
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
                                recipe_time = reader.GetInt32(1),
                                recipe_photo = reader.GetString(2)
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
            public string recipe_photo { get; set; }
        }



        //水谷
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
                                responseMessage += "ユーザー情報が登録されました";
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
                responseMessage = "パラメーターが設定されていません";
            }

            return new OkObjectResult(responseMessage);
        }



        [FunctionName("USERSELECT")]
        public static async Task<IActionResult> UserSelect(
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
                    string sql = "SELECT user_id, user_name, user_password FROM user_table";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            user_tableList resultList = new user_tableList();

                            while (reader.Read())
                            {
                                resultList.List.Add(new user_tableRow
                                {
                                    user_id = reader.GetInt32(0), 
                                    user_name = reader.GetString(1), 
                                    user_password = reader.GetString(2), 
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
                responseMessage = "データベース接続エラーが発生しました。";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseMessage = "予期しないエラーが発生しました。";
            }

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("USERCHECK")]
        public static async Task<IActionResult> UserCheck(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            string responseMessage = "SQL RESULT:";
            bool userExists = false;

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
                    // クエリパラメータから user_name を取得
                    string userName = req.Query["user_name"];

                    if (string.IsNullOrEmpty(userName))
                    {
                        return new BadRequestObjectResult("ユーザー名が入力されていません。");
                    }

                    string sql = "SELECT COUNT(*) FROM user_table WHERE user_name = @user_name";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // SQLパラメータの設定
                        command.Parameters.AddWithValue("@user_name", userName);
                        connection.Open();

                        int count = (int)await command.ExecuteScalarAsync();
                        userExists = count > 0;
                    }
                }
            }
            catch (SqlException e)
            {
                log.LogError(e.ToString());
                responseMessage = "データベース接続エラーが発生しました。";
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                responseMessage = "予期しないエラーが発生しました。";
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            // ユーザー名が既に存在するかどうかの結果をJSONで返す
            return new OkObjectResult(new { exists = userExists });
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

                            responseMessage = result > 0 ? "タスクが削除されました。" : "タスクが見つかりません。";
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    responseMessage = "エラーが発生しました。";
                }
            }
            else
            {
                responseMessage = "有効なタスクIDが提供されていません。";
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

                            responseMessage = result > 0 ? "お気に入りが解除されました。" : "該当のお気に入りが見つかりません。";
                        }
                    }
                }
                catch (SqlException e)
                {
                    log.LogError($"SQL Exception: {e.Message}");
                    responseMessage = $"エラーが発生しました: {e.Message}";
                }
                catch (Exception e)
                {
                    log.LogError($"Exception: {e.Message}");
                    responseMessage = $"予期しないエラーが発生しました: {e.Message}";
                }
            }
            else
            {
                responseMessage = "有効なユーザーIDまたはレシピIDが提供されていません。";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
