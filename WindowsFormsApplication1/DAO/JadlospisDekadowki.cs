using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    public class JadlospisDekadowkiDAO
    {
        public static void Insert(int identyfikatorDekadowki, int dzien, Dieta dieta, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dataTable = DataSet.Tables["Jadlospis"];
            DataRow dataRow = dataTable.NewRow();
            dataRow["Dieta"] = dieta.nazwa;
            dataRow["Nazwa-Śniadanie"] = nazwa_sniadanie;
            dataRow["Skład-Śniadanie"] = sklad_sniadanie;
            dataRow["Nazwa-IIŚniadanie"] = nazwa_IIsniadanie;
            dataRow["Skład-IIŚniadanie"] = sklad_IIsniadanie;
            dataRow["Nazwa-Obiad"] = nazwa_obiad;
            dataRow["Skład-Obiad"] = sklad_obiad;
            dataRow["Nazwa-Podwieczorek"] = nazwa_podwieczorek;
            dataRow["Skład-Podwieczorek"] = sklad_podwieczorek;
            dataRow["Nazwa-Kolacja"] = nazwa_kolacja;
            dataRow["Skład-Kolacja"] = sklad_kolacja;
            dataTable.Rows.Add(dataRow);
            DataSet.WriteXml(XML_Location);

            int identyfikatorJadlospisu = SelectId(new Jadlospis(null, dzien, dieta, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja));

            Check(identyfikatorDekadowki, dzien, dieta.nazwa);

            DataSet = new KalkulatorDietyDatabase();
            DataSet.ReadXml(XML_Location);
            dataTable = DataSet.Tables["JadlsopisDekadowki"];
            dataRow = dataTable.NewRow();
            dataRow["IdentyfikatorDekadowki"] = identyfikatorDekadowki;
            dataRow["IdentyfikatorJadlospisu"] = identyfikatorJadlospisu;
            dataRow["Dzien"] = dzien;
            dataTable.Rows.Add(dataRow);
            DataSet.WriteXml(XML_Location);
        }

        public static void InsertSQL(int identyfikatorDekadowki, int dzien, Dieta dieta, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Szablon(NazwaSniadanie, SkladSniadanie, NazwaSniadanieII, SkladSniadanieII, NazwaObiad, SkladObiad, NazwaPodwieczorek, SkladPodwieczorek, NazwaKolacja, SkladKolacja, Dieta) VALUES (" +
                            $"{nazwa_sniadanie}, {sklad_sniadanie}, {nazwa_IIsniadanie}, {sklad_IIsniadanie}, {nazwa_obiad}, {sklad_obiad}, {nazwa_podwieczorek}, {sklad_podwieczorek}, {nazwa_kolacja}, {sklad_kolacja}, {dieta});";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            int identyfikatorJadlospisu = SelectIdSQL(new Jadlospis(null, dzien, dieta, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja));
            CheckSQL(identyfikatorDekadowki, dzien, dieta.nazwa);
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT SzablonDekadowki(IdDekadowka, IdSzablon, Dzien) VALUES (" +
                            $"{identyfikatorDekadowki}, {identyfikatorJadlospisu}, {dzien});";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void CheckSQL(int identyfikatorDekadowki, int dzien, string dieta)
        {
            Jadlospis listaJadlospisow = null;
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();

            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM SzablonDekadowki WHERE IdDekadowka = {identyfikatorDekadowki} AND Dzien = {dzien};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(reader["IdSzablon"]));
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                foreach (int jadlospisId in listaIdentyfikatorowJadlospisowDekadowki)
                {
                    string sql = $"SELECT * FROM Szablon WHERE IdSzablon = {jadlospisId} AND Dieta = {dieta};";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string sql2 = $"SELECT * FROM SzablonDekadowki WHERE IdDekadowka = {identyfikatorDekadowki} AND IdSzablon = {reader["Id"]};";
                                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                                {
                                    using (SqlDataReader reader2 = command2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            DeleteSQL(Convert.ToInt32(reader["Identyfikator"].ToString()));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Check(int identyfikatorDekadowki, int dzien, string dieta)
        {
            Jadlospis listaJadlospisow = null;
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);

            for (int i = 0; i < DataSet.JadlsopisDekadowki.Rows.Count; i++)
            {
                if (DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorDekadowki"].ToString() == identyfikatorDekadowki.ToString() && DataSet.Tables["JadlsopisDekadowki"].Rows[i]["Dzien"].ToString() == dzien.ToString())
                {
                    listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorJadlospisu"]));
                }
            }


            for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            {
                if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())))
                {
                    if (DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString() == dieta)
                    {

                        for (int j = 0; j < DataSet.JadlsopisDekadowki.Rows.Count; j++)
                        {
                            if (DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorDekadowki"].ToString() == identyfikatorDekadowki.ToString() && DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorJadlospisu"].ToString() == DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())
                            {
                                DataSet.JadlsopisDekadowki.Rows.RemoveAt(j);
                                Delete(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString()));
                            }
                        }
                    }
                }
            }

            DataSet.WriteXml(XML_Location);

        }

        public static void Delete(int identyfikatorJadlospisu)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            {
                if (DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString() == identyfikatorJadlospisu.ToString())
                {
                    DataSet.Tables["Jadlospis"].Rows[i].Delete();
                }
            }

            for (int i = 0; i < DataSet.JadlsopisDekadowki.Rows.Count; i++)
            {
                if (DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorJadlospisu"].ToString() == identyfikatorJadlospisu.ToString())
                {
                    DataSet.Tables["JadlsopisDekadowki"].Rows[i].Delete();
                }
            }
            DataSet.WriteXml(XML_Location);
        }

        public static void DeleteSQL(int identyfikatorJadlospisu)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Szablon WHERE Id = {identyfikatorJadlospisu};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE SzablonDekadowki WHERE IdSzablon = {identyfikatorJadlospisu};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int SelectId(Jadlospis jadlospis)
        {
            int identyfikatorJadlospisu = 0;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            {
                if (DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString() == jadlospis.dieta.nazwa && DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString() == jadlospis.nazwa_sniadanie && DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString() == jadlospis.nazwa_IIsniadanie && DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString() == jadlospis.nazwa_obiad && DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString() == jadlospis.nazwa_podwieczorek && DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString() == jadlospis.nazwa_kolacja && DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString() == jadlospis.sklad_sniadanie && DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString() == jadlospis.sklad_IIsniadanie && DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString() == jadlospis.sklad_obiad && DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString() == jadlospis.sklad_podwieczorek && DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString() == jadlospis.sklad_kolacja)
                {
                    identyfikatorJadlospisu = Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]);
                }
            }
            return identyfikatorJadlospisu;
        }

        public static int SelectIdSQL(Jadlospis jadlospis)
        {
            int identyfikatorJadlospisu = 0;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Szablon WHERE Dieta = '{jadlospis.dieta}' AND NazwaSniadanie = '{jadlospis.nazwa_sniadanie} AND NazwaSniadanieII = '{jadlospis.nazwa_IIsniadanie} AND NazwaObiad = '{jadlospis.nazwa_obiad} AND NazwaPodiweczorek = '{jadlospis.nazwa_podwieczorek} AND NazwaKolacja = '{jadlospis.nazwa_kolacja} AND SkladSniadanie = '{jadlospis.sklad_sniadanie} AND SkladSniadanieII = '{jadlospis.sklad_IIsniadanie} AND SkladObiad = '{jadlospis.sklad_obiad} AND SkladPodiweczorek = '{jadlospis.sklad_podwieczorek} AND SkladKolacja = '{jadlospis.sklad_kolacja};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            identyfikatorJadlospisu = Convert.ToInt32(reader["Id"]);
                        }
                    }
                }
            }
            return identyfikatorJadlospisu;
        }

        public static List<Jadlospis> SelectForDay(int identyfikatorDekadowki, int dzien)
        {
            List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
            //for (int i = 0; i < DataSet.JadlsopisDekadowki.Rows.Count; i++)
            //{
            //    if (DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorDekadowki"].ToString() == identyfikatorDekadowki.ToString() && DataSet.Tables["JadlsopisDekadowki"].Rows[i]["Dzien"].ToString() == dzien.ToString())
            //    {
            //        listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorJadlospisu"]));
            //    }
            //}
            EnumerableRowCollection<DataRow> listaIdentyfikatorow = from order in DAO.DataSet.Tables["JadlsopisDekadowki"].AsEnumerable()
                                                                    where order.Field<String>("IdentyfikatorDekadowki") == identyfikatorDekadowki.ToString()
                                                                     && order.Field<String>("Dzien") == dzien.ToString()
                                                                    select order;
            foreach (DataRow jadlospis in listaIdentyfikatorow)
            {
                listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(jadlospis.Field<String>("IdentyfikatorJadlospisu")));
            }
            Dekadowka dekadowka = DekadowkaDAO.SelectFromId(identyfikatorDekadowki);

            //EnumerableRowCollection<DataRow> listaJadlospisow2 = from order in DAO.DataSet.Tables["Jadlospis"].AsEnumerable()
            //                                         where listaIdentyfikatorowJadlospisowDekadowki.Contains(order.Field<Int32>("Identyfikator"))
            //                                         select order;
            //foreach (DataRow row in listaJadlospisow2)
            //{
            //    listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(row["Identyfikator"]), dzien, DietaDAO.Select(row["Dieta"].ToString(), dekadowka.miasto), row["Nazwa-Śniadanie"].ToString(), row["Nazwa-IIŚniadanie"].ToString(), row["Nazwa-Obiad"].ToString(), row["Nazwa-Podwieczorek"].ToString(), row["Nazwa-Kolacja"].ToString(), row["Skład-Śniadanie"].ToString(), row["Skład-IIŚniadanie"].ToString(), row["Skład-Obiad"].ToString(), row["Skład-Podwieczorek"].ToString(), row["Skład-Kolacja"].ToString()));
            //}


            int end = 0;
            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                if (end == listaIdentyfikatorowJadlospisowDekadowki.Count)
                    return listaJadlospisow;
                if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())))
                {
                    listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]), dzien, DietaDAO.Select(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString(), dekadowka.miasto), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString()));
                    end++;
                }
            }
            //DataTable dekad = DataSet.Tables["Jadlospis"];
            //EnumerableRowCollection<DataRow> query2 = from order in orders.AsEnumerable()
            //                                         where order.Field<String>("Identyfikator")
            //                                          && order.Field<String>("Dzien") == dzien.ToString()
            //                                         select order;
            //foreach (DataRow dek in query2)
            //{
            //    listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(dek.Field<String>("IdentyfikatorJadlospisu")));
            //}

            return listaJadlospisow;
        }
        public static List<Jadlospis> SelectForDaySQL(int identyfikatorDekadowki, int dzien)
        {
            List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM SzablonDekadowki WHERE IdDekadowka = {identyfikatorDekadowki} AND Dzien = {dzien};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(reader["IdSzablon"]));
                        }
                    }
                }
            }

            Dekadowka dekadowka = DekadowkaDAO.SelectFromIdSQL(identyfikatorDekadowki);
            foreach (int idJadlospisu in listaIdentyfikatorowJadlospisowDekadowki)
            {
                using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Szablon WHERE Id = {idJadlospisu};";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(reader["Id"]), dzien, DietaDAO.SelectSQL(reader["Dieta"].ToString(), dekadowka.miasto), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladSniadanie"].ToString(), reader["SkladSniadanieII"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString()));

                            }
                        }
                    }
                }
            }
            return listaJadlospisow;
        }

        public static Dekadowka[] SelectForAllDays(Dekadowka dekadowka)
        {
            Dekadowka[] allDaysJadlospisy = new Dekadowka[dekadowka.dni];

            for (int j = 0; j < dekadowka.dni; j++)
            {
                List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                EnumerableRowCollection<String> listaIdentyfikatorowJadlospisowDekadowki = from order in DAO.DataSet.Tables["JadlsopisDekadowki"].AsEnumerable()
                                                                                           where order.Field<String>("IdentyfikatorDekadowki") == dekadowka.id.ToString()
                                                                                            && order.Field<String>("Dzien") == (j + 1).ToString()
                                                                                           select order.Field<String>("IdentyfikatorJadlospisu");
                allDaysJadlospisy[j] = new Dekadowka(listaIdentyfikatorowJadlospisowDekadowki.ToList(), new List<Jadlospis>());
            }

            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                string id = DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString();
                for (int j = 0; j < dekadowka.dni; j++)
                {
                    if (allDaysJadlospisy[j].listaIdentyfikatorówJadlospisow.Contains(id))
                    {
                        allDaysJadlospisy[j].listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]), j, DietaDAO.Select(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString(), dekadowka.miasto), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString()));
                    }
                }
            }

            return allDaysJadlospisy;
        }

        public static Dekadowka[] SelectForAllDaysSQL(Dekadowka dekadowka)
        {
            Dekadowka[] allDaysJadlospisy = new Dekadowka[dekadowka.dni];
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                for (int j = 0; j < dekadowka.dni; j++)
                {
                    string sql = $"SELECT * FROM SzablonDekadowki WHERE IdDekadowka = {dekadowka.id} AND Dzien = {j + 1};";
                    List<string> listaIdentyfikatorowJadlospisowDekadowki = new List<string>();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaIdentyfikatorowJadlospisowDekadowki.Add(reader["IdSzablon"].ToString());
                            }
                        }
                    }
                    allDaysJadlospisy[j] = new Dekadowka(listaIdentyfikatorowJadlospisowDekadowki.ToList(), new List<Jadlospis>());
                }
            }

            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Szablon;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["Id"].ToString();
                            for (int j = 0; j < dekadowka.dni; j++)
                            {
                                if (allDaysJadlospisy[j].listaIdentyfikatorówJadlospisow.Contains(id))
                                {
                                    allDaysJadlospisy[j].listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(reader["Id"]), j, DietaDAO.SelectSQL(reader["Dieta"].ToString(), dekadowka.miasto), reader["NazwaSniadanie"].ToString(), reader["NazwaSniadanieII"].ToString(), reader["NazwaObiad"].ToString(), reader["NazwaPodwieczorek"].ToString(), reader["NazwaKolacja"].ToString(), reader["SkladŚniadanie"].ToString(), reader["SkladIIŚniadanie"].ToString(), reader["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader["SkladKolacja"].ToString()));
                                }
                            }
                        }
                    }
                }
            }
            return allDaysJadlospisy;
        }
    }
}
