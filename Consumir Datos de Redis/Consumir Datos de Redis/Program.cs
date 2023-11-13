using StackExchange.Redis;
using Npgsql;
using System;
using System.Data;

namespace ConsumirDatosDeRedis
{
    class Program
    {
        static void Main()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            var db = redis.GetDatabase();
            string usuario1 = "Hailey";
            string usuario2 = "Jordyn";
            string claveRedis = $"distancia:{usuario1}:{usuario2}";
            string distanciaRedis = db.StringGet(claveRedis);

            var connectionString = "Host=localhost;Username=postgres;Password=hermana1;Database=usuarios";

            NpgsqlConnection connection = null;

            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Conexion exitosa a postgres");
                if (!string.IsNullOrEmpty(distanciaRedis))
                {
                    double distancia = Convert.ToDouble(distanciaRedis);

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "INSERT INTO usuarios_calificaciones (nombre, banda, calificacion) VALUES (@nombre, @banda, @calificacion)";
                        cmd.Parameters.AddWithValue("@nombre", usuario1);
                        cmd.Parameters.AddWithValue("@banda", usuario2);
                        cmd.Parameters.AddWithValue("@calificacion", distancia);

                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine($"La distancia de Manhattan entre {usuario1} y {usuario2} es {distancia}. Datos insertados en PostgreSQL.");
                }
                else
                {
                    Console.WriteLine($"La distancia de Manhattan NO SE ENCONTRÓ en Redis. No se realizaron inserciones en PostgreSQL.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            redis.Close();
        }
    }
}
