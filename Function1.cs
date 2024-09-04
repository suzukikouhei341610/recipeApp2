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
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTPレスポンスで返す文字列を定義
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
                    // SQLクエリの定義（パラメータ化されたクエリ）
                    string sql = "SELECT user_name FROM user_table WHERE user_name = @user_name AND user_password = @user_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // パラメータの追加
                        command.Parameters.AddWithValue("@user_name", user_name);
                        command.Parameters.AddWithValue("@user_password", user_password);

                        // 接続を開く
                        connection.Open();

                        // SQLクエリを実行し、結果を取得
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
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

        [FunctionName("EMPLOYEELOGIN")]
        public static async Task<IActionResult> EmployeeLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            //HTTPレスポンスで返す文字列を定義
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
                    // SQLクエリの定義（パラメータ化されたクエリ）
                    string sql = "SELECT employee_id FROM employee_table WHERE employee_id = @employee_id AND employee_password = @employee_password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // パラメータの追加
                        command.Parameters.AddWithValue("@employee_id", employee_id);
                        command.Parameters.AddWithValue("@employee_password", employee_password);

                        // 接続を開く
                        connection.Open();

                        // SQLクエリを実行し、結果を取得
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // 認証成功
                                responseMessage = "Login successful";
                            }
                            else
                            {
                                // 認証失敗
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
        //あいうえお



//水谷
//mochimochi


//ここまで

    }
}
  
