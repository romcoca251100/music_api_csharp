using Microsoft.AspNetCore.Mvc;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {

        public void ShowDataTable(DataTable table)
        {
            Console.WriteLine($"Tên bảng: {table.TableName}");
            foreach (DataColumn col in table.Columns)
            {
                Console.Write($"{col.ColumnName,15}");
            }
            Console.WriteLine();
            foreach (DataRow row in table.Rows)
            {
                for(int i = 0; i < table.Columns.Count; i++)
                {
                    Console.Write($"{row[i],15}");
                }
                Console.WriteLine();
            }
        }
        // GET: api/<MusicController>
        [HttpGet]
        public object Get()
        {
            var result = new Result();

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "DESKTOP-RL88JED\\ROMCOCA";
            sqlStringBuilder["Database"] = "music";
            sqlStringBuilder["Trusted_Connection"] = "True";
            
            //string connection = "Server=DESKTOP-RL88JED\\ROMCOCA; Database=music; Trusted_Connection=True;";
            string connection = sqlStringBuilder.ToString();
            using var sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();
            using DbCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM musics";

            using var sqlDataReader = cmd.ExecuteReader();

            if(sqlDataReader.HasRows)
            {
                List<Music> list = new List<Music>();
                while (sqlDataReader.Read())
                {
                    //Console.WriteLine($"{sqlDataReader["id"]} - {sqlDataReader["song"]}");
                    list.Add(new Music()
                    {
                        Id = (int)sqlDataReader["id"],
                        Song = (string)sqlDataReader["song"],
                        Link = (string)sqlDataReader["link"],
                        Singer = (string)sqlDataReader["singer"],
                        Author = (string)sqlDataReader["author"],
                    });
                }
                result.Data = list;
                result.Message = "Thành công";
                result.TotalResult = list.Count;
 
            }
           
            sqlConnection.Close();
            //return Data.MusicData.Values.ToList();
            //return Data.MusicData.Select(music => music.Value).ToList();
            return result;
        }

        [HttpGet("v2")]
        public object GetV2()
        {
            var result = new Result();

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "DESKTOP-RL88JED\\ROMCOCA";
            sqlStringBuilder["Database"] = "music";
            sqlStringBuilder["Trusted_Connection"] = "True";

            //string connection = "Server=DESKTOP-RL88JED\\ROMCOCA; Database=music; Trusted_Connection=True;";
            string connection = sqlStringBuilder.ToString();
            using var sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            //var dataset = new DataSet();
            //var table = new DataTable("MusciTable");
            //dataset.Tables.Add(table);

            //table.Columns.Add()

            var adapter = new SqlDataAdapter();
            adapter.TableMappings.Add("Table", "Music");

            //Thiết lập SelectCommand
            adapter.SelectCommand = new SqlCommand("SELECT * FROM musics", sqlConnection);
            
            //Thiết lập InsertCommand
            adapter.InsertCommand = new SqlCommand("INSERT INTO musics (song, link, singer, author) VALUES (@Song, @Link, @Singer, @Author)", sqlConnection);
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@Song", SqlDbType.NVarChar, 50 , "song"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@Link", SqlDbType.VarChar, 50, "link"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@Singer", SqlDbType.NVarChar, 50, "singer"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@Author", SqlDbType.NVarChar, 50, "author"));

            //Thiết lập DeleteCommand
            adapter.DeleteCommand = new SqlCommand("DELETE FROM musics WHERE id = @Id", sqlConnection);
            var paramDelete = adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
            paramDelete.SourceColumn = "id";
            paramDelete.SourceVersion = DataRowVersion.Original;

            //Thiết lập UpdateCommand
            adapter.UpdateCommand = new SqlCommand("UPDATE musics SET song = @Song, link = @Link, singer = @Singer, author = @Author WHERE id = @Id", sqlConnection);
            var paramUpdate = adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
            paramUpdate.SourceColumn = "id";
            paramUpdate.SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Song", SqlDbType.NVarChar, 50, "song"));
            adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Link", SqlDbType.VarChar, 50, "link"));
            adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Singer", SqlDbType.NVarChar, 50, "singer"));
            adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Author", SqlDbType.NVarChar, 50, "author"));

            var dataset = new DataSet();
            adapter.Fill(dataset);

            DataTable table = dataset.Tables["Music"];
            ShowDataTable(table);

            var rowInsert = table.Rows.Add();
            rowInsert["song"] = "Song 1";
            rowInsert["link"] = "Link 1";
            rowInsert["singer"] = "Singer 1";
            rowInsert["author"] = "Nhac Si 1";

            //table.Rows.Add(new object[] {(table.Rows.Count + 1), "Bai hat 4", "https://google.com.vn", "Ca si 4", "Nhac si 4" });
            //table.Rows.Add(new object[] {(table.Rows.Count + 1), "Bai hat 4", "https://google.com.vn", "Ca si 4", "Nhac si 4" });
            //table.Rows.Add(new object[] {(table.Rows.Count + 1), "Bai hat 4", "https://google.com.vn", "Ca si 4", "Nhac si 4" });

            //table.Rows[9].Delete();

            //var rowUpdate = table.Rows[0];
            //rowUpdate[1] = "Chinh sua bai hat 1";

            adapter.Update(dataset);
            adapter.Fill(dataset);
            ShowDataTable(table);

            sqlConnection.Close();
            return result;
        }

        // GET api/<MusicController>/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            var result = new Result();
            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "DESKTOP-RL88JED\\ROMCOCA";
            sqlStringBuilder["Database"] = "music";
            sqlStringBuilder["Trusted_Connection"] = "True";

            string connection = sqlStringBuilder.ToString();
            using var sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();
            using DbCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM musics WHERE id = @MusicId";
            var musicId = new SqlParameter("@MusicId", id);
            cmd.Parameters.Add(musicId);

            using var sqlDataReader = cmd.ExecuteReader(); // Lấy nhiều dòng dữ liệu
            //var sqlDataReader = cmd.ExecuteScalar(); //Trả về dữ liệu hàng 1 cột 1 (Count)
            //var sqlDataReader = cmd.ExecuteNonQuery(); //Dùng để thêm, sửa xoá  

            //var dataTable = new DataTable();
            //dataTable.Load(sqlDataReader);
            if (sqlDataReader.HasRows)
            {
                result.Message = "Thành công";
                sqlDataReader.Read();
                result.Data = new List<Music>()
                {
                    new Music()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Song = (string)sqlDataReader["song"],
                        Link = (string)sqlDataReader["link"],
                        Singer = (string)sqlDataReader["singer"],
                        Author = (string)sqlDataReader["author"],
                    }
                };
                result.TotalResult = result.Data.ToList().Count;
            }
            else
            {
                result.Message = "Không có dữ liệu.";
            }
            sqlConnection.Close();
            return result;
        }

        // POST api/<MusicController>
        [HttpPost]
        public object Post([FromBody] Music music)
        {
            var result = new Result();

            if(music == null)
            {
                result.Status = 400;
                result.Message = "Hãy nhập đủ các trường";
                return result;
            }

            if (music.CheckNull())
            {
                return music.GetResult();
            }

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "DESKTOP-RL88JED\\ROMCOCA";
            sqlStringBuilder["Database"] = "music";
            sqlStringBuilder["Trusted_Connection"] = "True";

            string connection = sqlStringBuilder.ToString();
            using var sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();
            using DbCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "INSERT INTO musics (song, link, singer, author) VALUES (@Song, @Link, @Singer, @Author); SELECT SCOPE_IDENTITY();";
            cmd.Parameters.Add(new SqlParameter("@Song", music.Song));
            cmd.Parameters.Add(new SqlParameter("@Link", music.Link));
            cmd.Parameters.Add(new SqlParameter("@Singer", music.Singer));
            cmd.Parameters.Add(new SqlParameter("@Author", music.Author));


            //using var sqlDataReader = cmd.ExecuteReader(); // Lấy nhiều dòng dữ liệu
            var sqlDataReader = cmd.ExecuteScalar(); //Trả về dữ liệu hàng 1 cột 1 (Count)
            //var sqlDataReader = cmd.ExecuteNonQuery(); //Dùng để thêm, sửa xoá  

            //var dataTable = new DataTable();
            //dataTable.Load(sqlDataReader);
            int id = Int32.Parse(sqlDataReader.ToString());
            if (id != 0)
            {
                music.Id = id;
                result.Message = "Thành công";
                result.Data = new List<Music>()
                {
                    music
                };
                result.TotalResult = result.Data.ToList().Count;
            }
            else
            {
                result.Message = "Thêm mới thất bại.";
            }
            sqlConnection.Close();
            return result;
        }

        // PUT api/<MusicController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Music music)
        {
           
        }

        // DELETE api/<MusicController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
