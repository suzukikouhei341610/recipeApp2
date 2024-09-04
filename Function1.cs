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

namespace FunctionAPIApp
{
    public static class Function1
    {
        //松本
        //aiueo




        //鈴木
        //こんにちは


        //碇
        //あいうえお



        //水谷
        //mochimochi


        //ここまで

        

        [FunctionName("INSERT")]
        public static async Task<IActionResult> RunInsert(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "INSERT RESULT:";
            string title = req.Query["title"];
            string status = req.Query["status"];
            string dueDate = req.Query["due_date"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            title = title ?? data?.title;
            status = status ?? data?.status;
            dueDate = dueDate ?? data?.due_date;

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(status) && !string.IsNullOrWhiteSpace(dueDate))
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
                        String sql = "INSERT INTO Task_NewTable(title, status, due_date) Values(@title, @status, @due_date)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@title", title);
                            command.Parameters.AddWithValue("@status", status);
                            command.Parameters.AddWithValue("@due_date", DateTime.Parse(dueDate));

                            connection.Open();

                            int result = command.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                responseMessage = "パラメーターが設定されていません";
            }

            return new OkObjectResult(responseMessage);
        }
        [FunctionName("DELETE")]
        public static async Task<IActionResult> RunDelete(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
       ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "DELETE RESULT:";
            string taskId = req.Query["taskID"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            taskId = taskId ?? data?.taskID;

            if (int.TryParse(taskId, out int id))
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
                        string sql = "DELETE FROM Task_NewTable WHERE id = @taskID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@taskID", id);

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
        [FunctionName("UPDATE")]
        public static async Task<IActionResult> RunUpdate(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed an update request.");

            string responseMessage = "UPDATE RESULT:";
            string taskId = req.Query["taskID"];
            string title = req.Query["title"];
            string status = req.Query["status"];
            string dueDate = req.Query["due_date"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            taskId = taskId ?? data?.taskID;
            title = title ?? data?.title;
            status = status ?? data?.status;
            dueDate = dueDate ?? data?.due_date;

            if (int.TryParse(taskId, out int id) && !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(status) && !string.IsNullOrWhiteSpace(dueDate))
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
                        string sql = "UPDATE Task_NewTable SET title = @title, status = @status, due_date = @due_date WHERE id = @taskID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@taskID", id);
                            command.Parameters.AddWithValue("@title", title);
                            command.Parameters.AddWithValue("@status", status);
                            command.Parameters.AddWithValue("@due_date", DateTime.Parse(dueDate));

                            connection.Open();

                            int result = command.ExecuteNonQuery();

                            responseMessage = result > 0 ? "タスクが更新されました。" : "タスクが見つかりません。";
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
                responseMessage = "有効なパラメーターが提供されていません。";
            }

            return new OkObjectResult(responseMessage);
        }
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

            // 1. 受け取ったパラメータをログに記録
           // log.LogInformation("Received parameters - recipeCategory: {recipeCategory}, recipeTime: {recipeTime}, recipeScene: {recipeScene}",
                              // recipeCategory, recipeTime, recipeScene);

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

    }
}
  
