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
        //����ɂ���


        //��
        //����������



        //���J
        //mochimochi


        //�����܂�

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

                        //using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Task_NewTableList resultList = new Task_NewTableList();

                            //while (reader.Read())
                            //{
                                //resultList.List.Add(new Task_NewTableRow
                                //{
                                    //TaskID = reader.GetInt32(0),  // "id" �J�����̃C���f�b�N�X
                                    //Title = reader.GetString(1),  // "title" �J�����̃C���f�b�N�X
                                    //Status = reader.GetString(2),  // "status" �J�����̃C���f�b�N�X
                                    //DueDate = reader.GetDateTime(3)  // "due_date" �J�����̃C���f�b�N�X
                                //});
                            //}

                            //responseMessage = JsonConvert.SerializeObject(resultList);
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
                responseMessage = "�p�����[�^�[���ݒ肳��Ă��܂���";
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

                            responseMessage = result > 0 ? "�^�X�N���X�V����܂����B" : "�^�X�N��������܂���B";
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
                responseMessage = "�L���ȃp�����[�^�[���񋟂���Ă��܂���B";
            }

            return new OkObjectResult(responseMessage);
        }

    }
}

