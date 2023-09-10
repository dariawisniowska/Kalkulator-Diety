using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    public class JednostkaDAO
    {
        //public static void Insert(string miasto)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    DataTable dtProdukty = DataSet.Tables["Jednostka"];
        //    DataRow drProdukty = dtProdukty.NewRow();
        //    drProdukty["Miasto"] = miasto;
        //    dtProdukty.Rows.Add(drProdukty);
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void InsertSQL(string miasto)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Jednostka(Miasto) VALUES ('{miasto}');";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //public static void Update(Jednostka jednostka, string miasto)
        //{
        //    Delete(jednostka);
        //    Insert(miasto);
        //}

        public static void UpdateSQL(Jednostka jednostka, string miasto)
        {
            DeleteSQL(jednostka);
            InsertSQL(miasto);
        }

        //public static void Delete(Jednostka jednostka)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Diety.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Jednostka.Rows.Count; i++)
        //        {
        //            if (DataSet.Jednostka.Rows[i]["Miasto"].ToString() == jednostka.miasto)
        //                DataSet.Jednostka.Rows[i].Delete();
        //        }
        //    }
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void DeleteSQL(Jednostka jednostka)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Jednostka WHERE Miasto = '{jednostka.miasto}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //public static List<Jednostka> SelectAll()
        //{
        //    List<Jednostka> listaJednostek = new List<Jednostka>();
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Jednostka.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Jednostka.Rows.Count; i++)
        //        {
        //            listaJednostek.Add(new Jednostka(DataSet.Jednostka.Rows[i]["Miasto"].ToString()));
        //        }
        //    }

        //    return listaJednostek;
        //}

        public static List<Jednostka> SelectAllSQL()
        {
            List<Jednostka> listaJednostek = new List<Jednostka>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Jednostka;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaJednostek.Add(new Jednostka(reader["Miasto"].ToString()));
                        }
                    }
                }
            }
            return listaJednostek;
        }
    }
    public class Jednostka
    {
        public string miasto;
        public Jednostka(string miasto)
        {
            this.miasto = miasto;
        }
    }
}
