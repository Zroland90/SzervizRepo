using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace SzervizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UgyfelekController : ControllerBase
    {
        private readonly string connectionString = "Server=localhost;Database=szerviz;Uid=root;Pwd=;";

        [HttpGet]
        public IActionResult GetUgyfelek()
        {
            var ugyfelek = new List<object>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM ugyfelek";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ugyfelek.Add(new
                            {
                                Id = reader["id"],
                                Nev = reader["nev"],
                                Telefon = reader["telefon"],
                                Email = reader["email"]
                            });
                        }
                    }
                }
            }

            return Ok(ugyfelek);
        }

        [HttpPost]
        public IActionResult PostUgyfel([FromBody] UgyfelModel ugyfel)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO ugyfelek (nev, telefon, email) VALUES (@nev, @telefon, @email)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nev", ugyfel.Nev);
                    command.Parameters.AddWithValue("@telefon", ugyfel.Telefon);
                    command.Parameters.AddWithValue("@email", ugyfel.Email);
                    command.ExecuteNonQuery();
                }
            }

            return Ok(new { message = "Sikeres ügyfél felvétel!" });
        }

        [HttpPut("{id}")]
        public IActionResult PutUgyfel(int id, [FromBody] UgyfelModel ugyfel)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE ugyfelek SET nev = @nev, telefon = @telefon, email = @email WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@nev", ugyfel.Nev);
                    command.Parameters.AddWithValue("@telefon", ugyfel.Telefon);
                    command.Parameters.AddWithValue("@email", ugyfel.Email);
                    command.ExecuteNonQuery();
                }
            }

            return Ok(new { message = "Sikeres módosítás!" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUgyfel(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM ugyfelek WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            return Ok(new { message = "Sikeres törlés!" });
        }
    }

    public class UgyfelModel
    {
        public string Nev { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
    }
}