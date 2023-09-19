using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using ConsoleTableExt;
using System.Threading.Tasks;
using CodingTracker.Mo3ses.Models;
using System.Globalization;

namespace CodingTracker.Mo3ses.Data
{
    public class DbConnect
    {
        string? connectionString;

        public DbConnect()
        {
            connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");
            DbCreate();
        }

        public void DbCreate(){
           using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS CODINGTRACKER(
							Id INTEGER PRIMARY KEY AUTOINCREMENT,
							STARTTIME TEXT NOT NULL,
							ENDTIME TEXT NOT NULL,
						    DURATION TEXT)";

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void Create(CodingSession session){
            session.Duration = Math.Round((session.EndTime - session.StartTime).TotalDays).ToString();

            using (var conn = new SQLiteConnection(connectionString)){
                conn.Open();
                var cmd = conn.CreateCommand();
               
                cmd.CommandText = "INSERT INTO CODINGTRACKER (STARTTIME, ENDTIME, DURATION) VALUES (@StartTime, @EndTime, @Duration)";
                cmd.Parameters.AddWithValue("@StartTime", session.StartTime.ToString());
                cmd.Parameters.AddWithValue("@EndTime", session.EndTime.ToString());
                cmd.Parameters.AddWithValue("@Duration", session.Duration);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void Update(CodingSession session){
            session.Duration = Math.Round((session.EndTime - session.StartTime).TotalDays).ToString();

            using (var conn = new SQLiteConnection(connectionString)){
                conn.Open();
                var cmd = conn.CreateCommand();
               
                cmd.CommandText = "UPDATE CODINGTRACKER SET STARTTIME = @StartTime, ENDTIME = @EndTime, DURATION = @Duration WHERE ID = @Id";
                cmd.Parameters.AddWithValue("@Id", session.Id);
                cmd.Parameters.AddWithValue("@StartTime", session.StartTime.ToString());
                cmd.Parameters.AddWithValue("@EndTime", session.EndTime.ToString());
                cmd.Parameters.AddWithValue("@Duration", session.Duration);
                cmd.ExecuteNonQuery();

                
                conn.Close();

            }
        }

        public void Delete(int id){
            using (var conn = new SQLiteConnection(connectionString)){
                conn.Open();
                var cmd = conn.CreateCommand();
               
                cmd.CommandText = "DELETE FROM CODINGTRACKER WHERE ID = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();

            }
        }
         public CodingSession GetById(int id){

            using(var conn = new SQLiteConnection(connectionString)){
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM CODINGTRACKER WHERE ID = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                
                CodingSession codingSession = new CodingSession();

                using (var reader = cmd.ExecuteReader()){
                    if (reader.Read()){
                    codingSession = new CodingSession
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            StartTime = Convert.ToDateTime(reader["STARTTIME"]),
                            EndTime = Convert.ToDateTime(reader["ENDTIME"]),
                            Duration = reader["DURATION"].ToString()
                        };
                    }else{
                        return null;
                    }
                    return codingSession;
                }
            } 
            
            
        }
        public List<CodingSession> GetAll(){
            
            List<CodingSession> result = new List<CodingSession>();

            using(var conn = new SQLiteConnection(connectionString)){
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM CODINGTRACKER";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CodingSession session = new CodingSession
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StartTime = Convert.ToDateTime(reader["StartTime"]),
                            EndTime = Convert.ToDateTime(reader["EndTime"]),
                            Duration = reader["DURATION"].ToString()
                        };

                        result.Add(session);
                    }
                }

                conn.Close();

                if (result.Count > 0)
                {
                    ConsoleTableBuilder.From(result)
                   //.WithFormat(ConsoleTableBuilderFormat.MarkDown)
                   .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 0, TextAligntment.Left },
                    { 1, TextAligntment.Left },
                    { 3, TextAligntment.Left },
                    { 100, TextAligntment.Left }
                   })
                   .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
                   })
                   .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                   .WithTitle("LIST", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
                   .WithFormatter(1, (text) =>
                   {
                       return text.ToString().ToUpper().Replace(" ", "-") + " �";
                   })
                   .ExportAndWriteLine(TableAligntment.Left);
                }
                
            }

            return result;
        }


    }
}