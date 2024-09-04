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
        //aiueo




        //���
        //����ɂ���


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
    }
}






//���J
//mochimochi


//�����܂�


