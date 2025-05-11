//intern:
using RandomWordApi_Vom_Hias.Data.Interfaces;

//extern:
using Npgsql;
using Newtonsoft.Json.Linq;

namespace RandomWordApi_Vom_Hias.Data
{
    public class DataManager : IDataManager
    {
        public DataManager(IConfiguration pConfig) 
        {
            var secretsPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");
            var json = File.ReadAllText(secretsPath);
            var jsonObject = JObject.Parse(json);
            connectionString = (string)jsonObject["Data"]["ConnectionStrings"]["randomWordDB"];
        }

        public async Task<Word> GetWords(string pTable, int pNumber, int minLength, int maxLength)
        {
            List<string> words = new List<string>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Verbindung zur Datenbank erfolgreich!");

                    string query = $"SELECT wort FROM {pTable} WHERE LENGTH(wort) >= {minLength} AND LENGTH(wort) <= {maxLength} ORDER BY RANDOM() LIMIT {pNumber};";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                words.Add(reader.GetString(reader.GetOrdinal("wort")));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine("Verbindung geschlossen.");
                }
            }
            return new Word { words = words };
        }

        public async Task PutScore(string pDifficulty, int pScore)
        {
            string query = $@"INSERT INTO {pDifficulty}_scores_db (Score) VALUES({pScore}";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine("Score hinzugefügt.");
                }
            }
        }

       

        private readonly string connectionString;
    }
}
