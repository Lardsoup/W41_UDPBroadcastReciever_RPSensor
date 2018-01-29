using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace W41WCFSoapSensor
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string ConnectionString =
            "Server=tcp:nfskole.database.windows.net,1433;Initial Catalog=NFDB;Persist Security Info=False;User ID=aopIn;Password=Swaz1212;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public int AddSensorData(int LightData, int TempData, int PotData, int AnalogData, DateTime Time)
        {
            const string insertData =
                "INSERT INTO W41_RPSensorData (LightData, TempData, PotData, AnalogData, Time) values (@LightData, @TempData, @PotData, @AnalogData, @Time)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertData, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@LightData", LightData);
                    insertCommand.Parameters.AddWithValue("@TempData", TempData);
                    insertCommand.Parameters.AddWithValue("@PotData", PotData);
                    insertCommand.Parameters.AddWithValue("@AnalogData", AnalogData);
                    insertCommand.Parameters.AddWithValue("@Time", Time);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        //public int AddStudent(string name, byte semester)
        //{
        //    const string insertStudent = "insert into student (name, semester) values (@name, @semester)";
        //    using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
        //    {
        //        databaseConnection.Open();
        //        using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
        //        {
        //            insertCommand.Parameters.AddWithValue("@name", name);
        //            insertCommand.Parameters.AddWithValue("@semester", semester);
        //            int rowsAffected = insertCommand.ExecuteNonQuery();
        //            return rowsAffected;
        //        }
        //    }
        //}
    }
}
