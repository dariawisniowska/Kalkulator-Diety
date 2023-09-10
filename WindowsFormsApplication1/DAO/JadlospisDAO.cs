using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    public class JadlospisDAO
    {
        //public static void Insert(string data, string dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    DataTable dataTable = DataSet.Tables["Jadłospisy"];
        //    DataRow dataRow = dataTable.NewRow();
        //    dataRow["Data"] = data;
        //    dataRow["Dieta"] = dieta;
        //    dataRow["Miasto"] = miasto;
        //    dataRow["Nazwa-Śniadanie"] = nazwa_sniadanie;
        //    dataRow["Skład-Śniadanie"] = sklad_sniadanie;
        //    dataRow["Nazwa-IIŚniadanie"] = nazwa_IIsniadanie;
        //    dataRow["Skład-IIŚniadanie"] = sklad_IIsniadanie;
        //    dataRow["Nazwa-Obiad"] = nazwa_obiad;
        //    dataRow["Skład-Obiad"] = sklad_obiad;
        //    dataRow["Nazwa-Podwieczorek"] = nazwa_podwieczorek;
        //    dataRow["Skład-Podwieczorek"] = sklad_podwieczorek;
        //    dataRow["Nazwa-Kolacja"] = nazwa_kolacja;
        //    dataRow["Skład-Kolacja"] = sklad_kolacja;
        //    dataTable.Rows.Add(dataRow);
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void InsertSQL(string data, string dieta, string miasto, string plec, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Jadlospis(Data, Plec, NazwaSniadanie, SkladSniadanie, NazwaSniadanieII, SkladSniadanieII, NazwaObiad, SkladObiad, NazwaPodwieczorek, SkladPodwieczorek, NazwaKolacja, SkladKolacja, Dieta, Miasto) VALUES (" +
                            $"'{data}', '{plec}', '{nazwa_sniadanie}', '{sklad_sniadanie}', '{nazwa_IIsniadanie}', '{sklad_IIsniadanie}', '{nazwa_obiad}', '{sklad_obiad}', '{nazwa_podwieczorek}', '{sklad_podwieczorek}', '{nazwa_kolacja}', '{sklad_kolacja}', '{dieta}', '{miasto}');";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //public static void Update(Jadlospis jadlospis, string data, string dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        //{
        //    Delete(data, dieta, miasto);
        //    Insert(data, dieta, miasto, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);
        //}

        public static void UpdateSQL(Jadlospis jadlospis, string data, string dieta, string miasto, string plec, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            DeleteSQL(data, dieta, miasto, plec);
            InsertSQL(data, dieta, miasto, plec, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);
        }

        //public static void Delete(string data, string miasto, string dieta)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
        //    {

        //        if (DataSet.Tables["Jadłospisy"].Rows[i]["Data"].ToString() == data && DataSet.Tables["Jadłospisy"].Rows[i]["Dieta"].ToString() == dieta && DataSet.Tables["Jadłospisy"].Rows[i]["Miasto"].ToString() == miasto)
        //        {
        //            DataSet.Tables["Jadłospisy"].Rows[i].Delete();
        //        }

        //    }
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void DeleteSQL(string data, string miasto, string dieta, string plec)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Jadlospis WHERE Data = '{data}' AND Miasto = '{miasto}' AND dieta = '{dieta}' AND Plec = '{plec}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static Jadlospis SelectAllSQL(string data, string miasto, string dieta, string plec)
        {
            Jadlospis jadlospis = null;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Jadlospis WHERE Data = '{data}' AND Miasto = '{miasto}' AND dieta = '{dieta}'AND Plec = '{plec}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            jadlospis = new Jadlospis(reader["Data"].ToString(), DietaDAO.SelectSQL(reader["Dieta"].ToString(), reader["Miasto"].ToString(), reader["Plec"].ToString()), reader["Miasto"].ToString(), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladSniadanie"].ToString(), reader["SkladSniadanieII"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString());
                        }
                    }
                }
            }
            return jadlospis;
        }

        //public static Jadlospis SelectAll(string data, string miasto, string dieta)
        //{
        //    Jadlospis jadlospis = null;
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Jadłospisy.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
        //        {
        //            if(DataSet.Jadłospisy.Rows[i]["Data"].ToString()==data&& DataSet.Jadłospisy.Rows[i]["Dieta"].ToString()==dieta && DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()==miasto )
        //                jadlospis = new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
        //        }
        //    }

        //    return jadlospis;
        //}

        public static List<Jadlospis> SelectAllSQL(string dataOd, string dataDo)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Jadlospis WHERE Data >= '{dataOd}' AND Data <= '{dataDo}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            jadlospis.Add(new Jadlospis(reader["Data"].ToString(), DietaDAO.SelectSQL(reader["Dieta"].ToString(), reader["Miasto"].ToString(), reader["Plec"].ToString()), reader["Miasto"].ToString(), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladSniadanie"].ToString(), reader["SkladSniadanieII"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString()));
                        }
                    }
                }
            }
            return jadlospis;
        }

        public static int CountSQL()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT Count(*) as Count FROM Jadlospis;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["Count"]);
                        }
                    }
                }
            }
            return count;
        }

        //public static List<Jadlospis> SelectAll(string dataOd, string dataDo)
        //{
        //    List<Jadlospis> jadlospis = new List<Jadlospis>();
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Jadłospisy.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
        //        {
        //            if (Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString())>=Convert.ToDateTime(dataOd)&& Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString())<=Convert.ToDateTime(dataDo))
        //                jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
        //        }
        //    }

        //    return jadlospis;
        //}

        //public static List<Jadlospis> Select(string data, string miasto)
        //  {
        //      List<Jadlospis> jadlospis = new List<Jadlospis>();
        //      KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //      String XML_Location = @"DataBase.xml";
        //      DataSet.ReadXml(XML_Location);
        //      if (DataSet.Jadłospisy.Rows.Count > 0)
        //      {
        //          for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
        //          {
        //              if (DataSet.Jadłospisy.Rows[i]["Data"].ToString() == data && DataSet.Jadłospisy.Rows[i]["Miasto"].ToString() == miasto)
        //              {
        //                  int? j = Check(jadlospis, DataSet.Jadłospisy.Rows[i]["Dieta"].ToString());
        //                  if (j != null) {
        //                      jadlospis.RemoveAt(Convert.ToInt32(j));
        //                  }
        //                  jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
        //              }
        //          }
        //      }

        //      return jadlospis;
        //  }


        public static List<Jadlospis> SelectSQL(string data, string miasto)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Jadlospis WHERE Miasto = '{miasto}' AND Data = '{data}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? j = Check(jadlospis, reader["Dieta"].ToString());
                            if (j != null)
                            {
                                jadlospis.RemoveAt(Convert.ToInt32(j));
                            }
                            jadlospis.Add(new Jadlospis(reader["Data"].ToString(), DietaDAO.SelectSQL(reader["Dieta"].ToString(), reader["Miasto"].ToString(), reader["Plec"].ToString()), reader["Miasto"].ToString(), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladSniadanie"].ToString(), reader["SkladSniadanieII"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString()));
                        }
                    }
                }
            }
            return jadlospis;
        }

        public static Jadlospis SelectSQL(string data, string dieta, string miasto, string plec)
        {
            Jadlospis jadlospis = null;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Jadlospis WHERE Miasto = '{miasto}' AND Data = '{data}' AND Dieta = '{dieta}' AND Plec = '{plec}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            jadlospis = new Jadlospis(reader["Data"].ToString(), DietaDAO.SelectSQL(reader["Dieta"].ToString(), reader["Miasto"].ToString(), reader["Plec"].ToString()), reader["Miasto"].ToString(), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladSniadanie"].ToString(), reader["SkladSniadanieII"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString());
                        }
                    }
                }
            }
            return jadlospis;
        }

        public static int? Check(List<Jadlospis> lista, string dieta)
        {
            int i = 0;
            foreach(Jadlospis j in lista)
            {
                if (j.dieta.nazwa == dieta)
                {
                    //Delete(j.data, j.miasto, dieta);
                    return i;
                }
                i++;
            }
            return null;
        }

    }
}
