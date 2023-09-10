using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApplication1.DAO
{
    public static class DAO
    {
        //public static string ConnectionString = "Server=DESKTOP-2PUB462;Database=KalkulatorDiety;Trusted_Connection=True;";
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        //public static KalkulatorDietyDatabase DataSet { get; set; }
        //private static String XML_Location = @"DataBase.xml";

        //public static void ReloadDatabase()
        //{
        //    DataSet = new KalkulatorDietyDatabase();
        //    DataSet.ReadXml(XML_Location);
        //}

        //public static void WriteXml()
        //{
        //    DataSet.WriteXml(XML_Location);
        //}
    }
}
