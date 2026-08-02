namespace KalkulatorDiety.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class JadlospisDekadowkiDAO
    {
        public static void Insert(int identyfikatorDekadowki, int dzien, Dieta dieta, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            DataTable dataTable = DAO.DataSet.Tables["Jadlospis"];
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
            DAO.WriteXml();

            int identyfikatorJadlospisu = SelectId(new Jadlospis(null, dzien, dieta, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja));

            Check(identyfikatorDekadowki, dzien, dieta.nazwa);

            DataTable dataTable2 = DAO.DataSet.Tables["JadlsopisDekadowki"];
            DataRow dataRow2 = dataTable2.NewRow();
            dataRow2["IdentyfikatorDekadowki"] = identyfikatorDekadowki;
            dataRow2["IdentyfikatorJadlospisu"] = identyfikatorJadlospisu;
            dataRow2["Dzien"] = dzien;
            dataTable2.Rows.Add(dataRow2);
            DAO.WriteXml();
        }

        public static void Check(int identyfikatorDekadowki, int dzien, string dieta)
        {
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();


            for (int i = 0; i < DAO.DataSet.JadlsopisDekadowki.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorDekadowki"].ToString() == identyfikatorDekadowki.ToString() && DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i]["Dzien"].ToString() == dzien.ToString())
                {
                    listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorJadlospisu"]));
                }
            }


            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())))
                {
                    if (DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString() == dieta)
                    {

                        for (int j = 0; j < DAO.DataSet.JadlsopisDekadowki.Rows.Count; j++)
                        {
                            if (DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorDekadowki"].ToString() == identyfikatorDekadowki.ToString() && DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorJadlospisu"].ToString() == DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())
                            {
                                DAO.DataSet.JadlsopisDekadowki.Rows.RemoveAt(j);
                                Delete(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString()));
                            }
                        }
                    }
                }
            }

            DAO.WriteXml();

        }

        public static void Delete(int identyfikatorJadlospisu)
        {
            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString() == identyfikatorJadlospisu.ToString())
                {
                    DAO.DataSet.Tables["Jadlospis"].Rows[i].Delete();
                }
            }

            for (int i = 0; i < DAO.DataSet.JadlsopisDekadowki.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorJadlospisu"].ToString() == identyfikatorJadlospisu.ToString())
                {
                    DAO.DataSet.Tables["JadlsopisDekadowki"].Rows[i].Delete();
                }
            }

            DAO.WriteXml();
        }

        public static int SelectId(Jadlospis jadlospis)
        {
            int identyfikatorJadlospisu = 0;
            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString() == jadlospis.dieta.nazwa && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString() == jadlospis.nazwa_sniadanie && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString() == jadlospis.nazwa_IIsniadanie && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString() == jadlospis.nazwa_obiad && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString() == jadlospis.nazwa_podwieczorek && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString() == jadlospis.nazwa_kolacja && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString() == jadlospis.sklad_sniadanie && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString() == jadlospis.sklad_IIsniadanie && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString() == jadlospis.sklad_obiad && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString() == jadlospis.sklad_podwieczorek && DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString() == jadlospis.sklad_kolacja)
                {
                    identyfikatorJadlospisu = Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]);
                }
            }
            return identyfikatorJadlospisu;
        }

        public static Jadlospis SelectFromId(int id)
        {
            Jadlospis jadlospis = null;
            DataTable orders = DAO.DataSet.Tables["Jadlospis"];
            EnumerableRowCollection<DataRow> query = from order in orders.AsEnumerable()
                                                     where order.Field<String>("Identyfikator") == id.ToString()
                                                     select order;

            foreach (DataRow prod in query)
            {
                jadlospis = new Jadlospis(prod.Field<String>("Data").ToString(), DietaDAO.Select(prod.Field<String>("Dieta").ToString(), prod.Field<String>("Miasto").ToString()), prod.Field<String>("Miasto").ToString(), prod.Field<String>("Nazwa-Śniadanie").ToString(), prod.Field<String>("Nazwa-IIŚniadanie").ToString(), prod.Field<String>("Nazwa-Obiad").ToString(), prod.Field<String>("Nazwa-Podwieczorek").ToString(), prod.Field<String>("Nazwa-Kolacja").ToString(), prod.Field<String>("Skład-Śniadanie").ToString(), prod.Field<String>("Skład-IIŚniadanie").ToString(), prod.Field<String>("Skład-Obiad").ToString(), prod.Field<String>("Skład-Podwieczorek").ToString(), prod.Field<String>("Skład-Kolacja").ToString());
            }

            return jadlospis;
        }

        public static List<Jadlospis> SelectForDay(int identyfikatorDekadowki, string miasto, int dzien)
        {
            List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
            List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();

            EnumerableRowCollection<DataRow> listaIdentyfikatorow = from order in DAO.DataSet.Tables["JadlsopisDekadowki"].AsEnumerable()
                                                                    where order.Field<String>("IdentyfikatorDekadowki") == identyfikatorDekadowki.ToString()
                                                                     && order.Field<String>("Dzien") == dzien.ToString()
                                                                    select order;
            foreach (DataRow jadlospis in listaIdentyfikatorow)
            {
                listaIdentyfikatorowJadlospisowDekadowki.Add(Convert.ToInt32(jadlospis.Field<String>("IdentyfikatorJadlospisu")));
            }

            int end = 0;
            for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            {
                if (end == listaIdentyfikatorowJadlospisowDekadowki.Count)
                    return listaJadlospisow;
                if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())))
                {
                    listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]), dzien, DietaDAO.Select(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString(), miasto), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString()));
                    end++;
                }
            }

            return listaJadlospisow;
        }

        public static Dekadowka[] SelectForAllDays(Dekadowka dekadowka)
        {
            Dekadowka[] allDaysJadlospisy = new Dekadowka[dekadowka.dni];
            List<String> allIds = new List<string>();

            for (int j = 0; j < dekadowka.dni; j++)
            {
                List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                EnumerableRowCollection<String> listaIdentyfikatorowJadlospisowDekadowki = from order in DAO.DataSet.Tables["JadlsopisDekadowki"].AsEnumerable()
                                                                                           where order.Field<String>("IdentyfikatorDekadowki") == dekadowka.id.ToString()
                                                                                            && order.Field<String>("Dzien") == (j + 1).ToString()
                                                                                           select order.Field<String>("IdentyfikatorJadlospisu");
                allDaysJadlospisy[j] = new Dekadowka(listaIdentyfikatorowJadlospisowDekadowki.ToList(), new List<Jadlospis>());
                allIds.AddRange(listaIdentyfikatorowJadlospisowDekadowki.ToList());
            }

            EnumerableRowCollection<DataRow> jadlospisy = from order in DAO.DataSet.Tables["Jadlospis"].AsEnumerable()
                                                          where allIds.Contains(order.Field<Int32>("Identyfikator").ToString())
                                                          select order;
            DataRow[] rows = jadlospisy.ToArray();
            for (int i = 0; i < jadlospisy.Count(); i++)
            {
                string id = rows[i]["Identyfikator"].ToString();
                for (int j = 0; j < dekadowka.dni; j++)
                {
                    if (allDaysJadlospisy[j].listaIdentyfikatorówJadlospisow.Contains(id))
                    {
                        allDaysJadlospisy[j].listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(rows[i]["Identyfikator"]), j, DietaDAO.Select(rows[i]["Dieta"].ToString(), dekadowka.miasto), rows[i]["Nazwa-Śniadanie"].ToString(), rows[i]["Nazwa-IIŚniadanie"].ToString(), rows[i]["Nazwa-Obiad"].ToString(), rows[i]["Nazwa-Podwieczorek"].ToString(), rows[i]["Nazwa-Kolacja"].ToString(), rows[i]["Skład-Śniadanie"].ToString(), rows[i]["Skład-IIŚniadanie"].ToString(), rows[i]["Skład-Obiad"].ToString(), rows[i]["Skład-Podwieczorek"].ToString(), rows[i]["Skład-Kolacja"].ToString()));
                    }
                }
            }

            return allDaysJadlospisy;
        }
    }
}
