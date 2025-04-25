using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RandomWordApi_Vom_Hias.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using RandomWordApi_Vom_Hias;

namespace RandomWordApi_Vom_Hias.Data
{
    public class DataManager : IDataManager
    {
        DataManager(IConfiguration pConfig) 
        {
            _config = pConfig;
            connectionString = _config.GetConnectionString("Data:ConnectionStrings:randomWordDB");
        }

        public async Task<Word> GetWords(string pTable, int pNumber)
        {
            List<string> words = new List<string>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Verbindung zur Datenbank erfolgreich!");


                        string query = $"SELECT * FROM {pTable} ORDER BY RAND() LIMIT {pNumber};";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                words.Add(reader.GetString(0));
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

        private readonly string connectionString;
        private readonly IConfiguration _config;
    }
}
