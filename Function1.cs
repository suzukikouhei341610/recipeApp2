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

namespace FunctionAPIApp
{
    public static class Function1
    {
        //松本
        //aiueo




        //鈴木



        //碇
        //あいうえお



        //水谷
        [FunctionName("USERINSERT")]
        public static async Task<IActionResult> UserInsert(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "INSERT RESULT:";
            string id = req.Query["id"];
            string user_name = req.Query["user_name"];
            string user_password = req.Query["user_password"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.id;
            user_name = user_name ?? data?.user_name;
            user_password = user_password ?? data?.user_password;

            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(user_name) && !string.IsNullOrWhiteSpace(user_password))
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
                        String sql = "INSERT INTO user_table(id, user_name, user_password) Values(@id, @user_name, @user_password)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@user_name", user_name);
                            command.Parameters.AddWithValue("@user_password", DateTime.Parse(user_password));

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


        //ここまで

        [FunctionName("SELECT")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            string responseMessage = "SQL RESULT:";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "m3hkouhei2010.database.windows.net";
                builder.UserID = "kouhei0726";
                builder.Password = "Battlefield341610";
                builder.InitialCatalog = "m3h-kouhei-0726";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = "SELECT id, title, status, due_date FROM Task_NewTable";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Task_NewTableList resultList = new Task_NewTableList();

                            while (reader.Read())
                            {
                                resultList.List.Add(new Task_NewTableRow
                                {
                                    TaskID = reader.GetInt32(0),  // "id" カラムのインデックス
                                    Title = reader.GetString(1),  // "title" カラムのインデックス
                                    Status = reader.GetString(2),  // "status" カラムのインデックス
                                    DueDate = reader.GetDateTime(3)  // "due_date" カラムのインデックス
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
            }

            return new OkObjectResult(responseMessage);
        }

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

    }
}

