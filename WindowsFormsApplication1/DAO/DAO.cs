using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WindowsFormsApplication1.DAO
{
    public static class DAO
    {
        public static SqlConnectionStringBuilder Builder()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";  
            builder.UserID = "DESKTOP-2PUB462\\daria";            
            builder.Password = "Darianna.1";     
            builder.InitialCatalog = "master";
            return builder;
        }
        public static string ConnectionString = "Server=DESKTOP-2PUB462;Database=KalkulatorDiety;Trusted_Connection=True;";

        public static KalkulatorDietyDatabase DataSet { get; set; }
        private static String XML_Location = @"DataBase.xml";

        public static void ReloadDatabase()
        {
            DataSet = new KalkulatorDietyDatabase();
            DataSet.ReadXml(XML_Location);
        }

        public static void WriteXml()
        {
            DataSet.WriteXml(XML_Location);
        }
    }
}
