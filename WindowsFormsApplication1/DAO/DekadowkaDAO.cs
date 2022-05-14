using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    public static class DekadowkaDAO
    {
        public static void InsertSQL(string nazwa, string miasto, int dni, string dzienStart, List<Jadlospis> listaJadlospisow)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Dekadowka(Nazwa, Miasto, DzienStart, Dni) VALUES ({nazwa}, {miasto}, {dzienStart}, {dni});";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            int identyfikatorDekadowki = SelectIdSQL(new Dekadowka(null, nazwa, miasto, dni, dzienStart, null));

            if (listaJadlospisow != null)
                foreach (Jadlospis jadlospis in listaJadlospisow)
                    JadlospisDekadowkiDAO.InsertSQL(identyfikatorDekadowki, jadlospis.dzien, jadlospis.dieta, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja);

        }

        public static void Insert(string nazwa, string miasto, int dni, string dzienStart, List<Jadlospis> listaJadlospisow)
        {
            DataTable dataTable = DAO.DataSet.Tables["Dekadowka"];
            DataRow dataRow = dataTable.NewRow();
            dataRow["Nazwa"] = nazwa;
            dataRow["Miasto"] = miasto;
            dataRow["DzienStart"] = dzienStart;
            dataRow["Dni"] = dni;
            dataTable.Rows.Add(dataRow);
            DAO.WriteXml();

            int identyfikatorDekadowki = SelectId(new Dekadowka(null, nazwa, miasto, dni, dzienStart, null));

            if(listaJadlospisow!=null)
                foreach (Jadlospis jadlospis in listaJadlospisow)
                    JadlospisDekadowkiDAO.Insert(identyfikatorDekadowki, jadlospis.dzien, jadlospis.dieta, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja);
        }

        public static int SelectIdSQL(Dekadowka dekadowka)
        {
            int identyfikatorDekadowki = 0;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT Id FROM Dekadowka WHERE Nazwa = '{dekadowka.nazwa}' AND Miasto = '{dekadowka.miasto}' AND DzienStart = {dekadowka.dzienStart} AND Dni = {dekadowka.dni};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            identyfikatorDekadowki = Convert.ToInt32(reader["Id"]);
                        }
                    }
                }
            }
            return identyfikatorDekadowki;
        }

        public static int SelectId(Dekadowka dekadowka)
        {
            int identyfikatorDekadowki = 0;
            for (int i = 0; i < DAO.DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString() == dekadowka.nazwa && DAO.DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString() == dekadowka.miasto && DAO.DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString() == dekadowka.dzienStart && DAO.DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString() == dekadowka.dni.ToString())
                {
                    identyfikatorDekadowki = Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]);
                }
            }
            return identyfikatorDekadowki;
        }

        public static Dekadowka SelectFromIdSQL(int id)
        {
            Dekadowka dekadowka = null;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT Nazwa, Miasto, DzienStart, Dni FROM Dekadowka WHERE Id = {id};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dekadowka = new Dekadowka(Convert.ToInt32(reader["Id"]), reader["Nazwa"].ToString(), reader["Miasto"].ToString(), Convert.ToInt32(reader["Dni"].ToString()), reader["DzienStart"].ToString(), null);
                        }
                    }
                }
            }
            return dekadowka;
        }

        public static Dekadowka SelectFromId(int id)
        {
            Dekadowka dekadowka = null;
            for (int i = 0; i < DAO.DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString() == id.ToString())
                {
                    dekadowka = new Dekadowka(Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]), DAO.DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString(), DAO.DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString(), Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString()), DAO.DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString(), null);
                }
            }
            return dekadowka;
        }

        public static void DeleteSQL(Dekadowka dekadowka)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Dekadowka WHERE Id = {dekadowka.id};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE JadlospisDekadowki WHERE DekadowkaId = {dekadowka.id};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(Dekadowka dekadowka)
        {
            for (int i = 0; i < DAO.DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString() == dekadowka.id.ToString())
                {
                    DAO.DataSet.Tables["Dekadowka"].Rows[i].Delete();
                }
            }
            DataRowCollection collection = DAO.DataSet.JadlsopisDekadowki.Rows;
            for (int i = collection.Count-1; i >=0; i--)
            {
                if (DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorDekadowki"].ToString() == dekadowka.id.ToString())
                {
                    DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i].Delete();
                }
            }
            DAO.WriteXml();
        }

        public static List<Dekadowka> SelectSQL(string miasto)
        {
            List<Dekadowka> listaDekadowek = new List<Dekadowka>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string selectDekadowki = $"SELECT * FROM Dekadowka WHERE Miasto = '{miasto}'";
                using (SqlCommand command = new SqlCommand(selectDekadowki, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int dekadowkaId = Convert.ToInt32(reader["Id"]);

                            string selectJadlospisyId = $"SELECT Id FROM SzablonDekadowki WHERE DekadowkaId = '{dekadowkaId}'";
                            List<int> listaIdJadlospisow = new List<int>();
                            using (SqlCommand command2 = new SqlCommand(selectJadlospisyId, connection))
                            {
                                using (SqlDataReader reader2 = command2.ExecuteReader())
                                {
                                    while (reader2.Read())
                                    {
                                        listaIdJadlospisow.Add(Convert.ToInt32(reader2["Id"]));
                                    }
                                }
                            }

                            List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                            foreach (int jadlospisId in listaIdJadlospisow)
                            {
                                string selectJadlospisy = $"SELECT * FROM Szablon WHERE Id = '{jadlospisId}'";
                                using (SqlCommand command3 = new SqlCommand(selectJadlospisy, connection))
                                {
                                    using (SqlDataReader reader3 = command3.ExecuteReader())
                                    {
                                        while (reader3.Read())
                                        {
                                            listaJadlospisow.Add(new Jadlospis(jadlospisId, SelectDzien(dekadowkaId, jadlospisId), DietaDAO.SelectSQL(reader3["Dieta"].ToString(), miasto), reader3["NazwaSniadanie"].ToString(), reader3["NazwaSniadanieII"].ToString(), reader3["NazwaObiad"].ToString(), reader3["NazwaPodwieczorek"].ToString(), reader3["NazwaKolacja"].ToString(), reader3["SkladSniadanie"].ToString(), reader3["SkladSniadanieII"].ToString(), reader3["SkladObiad"].ToString(), reader["SkladPodwieczorek"].ToString(), reader3["SkladKolacja"].ToString()));
                                        }
                                    }
                                }
                            }
                            listaDekadowek.Add(new Dekadowka(dekadowkaId, reader["Nazwa"].ToString(), reader["Miasto"].ToString(), Convert.ToInt32(reader["Dni"].ToString()), reader["DzienStart"].ToString(), listaJadlospisow));
                        }
                    }
                }
            }
            return listaDekadowek;
        }

        public static List<Dekadowka> Select(string miasto)
        {
            List<Dekadowka> listaDekadowek = new List<Dekadowka>();
            if (DAO.DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Dekadowka.Rows.Count; i++)
                {
                    if (DAO.DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString() == miasto)
                    {
                        List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
                        for (int j = 0; j < DAO.DataSet.JadlsopisDekadowki.Rows.Count; j++)
                        {
                            if (DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorDekadowki"].ToString() == DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString())
                            {
                                listaIdentyfikatorowJadlospisowDekadowki.Add(i);
                            }
                        }
                        List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                        for (int k = 0; k < DAO.DataSet.Jadlospis.Rows.Count; k++)
                        {
                            if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"].ToString())))
                            {
                                listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"]), SelectDzien(Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString()),Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"])), DietaDAO.Select(DAO.DataSet.Tables["Jadlospis"].Rows[k]["Dieta"].ToString(), DAO.DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString()), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Skład-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Skład-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[k]["Skład-Kolacja"].ToString()));
                            }
                        }

                        listaDekadowek.Add(new Dekadowka(Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]), DAO.DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString(), DAO.DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString(), Convert.ToInt32(DAO.DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString()), DAO.DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString(), listaJadlospisow));
                    }
                }
            }

            return listaDekadowek;
        }

        public static int SelectDzienSQL(int idDekadowki, int id)
        {
            int dzien = 0;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT Dzien FROM JadlsopisDekadowki WHERE JadlospisId = {id} AND DekadowkaId = {idDekadowki};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dzien = Convert.ToInt32(reader["Dzien"]);
                        }
                    }
                }
            }
            return dzien;
        }

        public static int SelectDzien(int idDekadowki, int id)
        {
            int dzien = 0;
            if (DAO.DataSet.Diety.Rows.Count > 0)
            {
                
                        for (int k = 0; k < DAO.DataSet.JadlsopisDekadowki.Rows.Count; k++)
                        {
                            if (Convert.ToInt32(DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[k]["IdentyfikatorJadlospisu"].ToString())==id && Convert.ToInt32(DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[k]["IdentyfikatorDekadowki"].ToString()) == idDekadowki)
                            {
                                dzien = Convert.ToInt32(DAO.DataSet.Tables["JadlospisDekadowki"].Rows[k]["Dzien"]);
                            }
                        }
            }
            return dzien;
        }
    }
}
