using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WindowsFormsApplication1.DAO
{
    public class JadlospisDAO
    {
        public static void Insert(string data, string dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dataTable = DataSet.Tables["Jadłospisy"];
            DataRow dataRow = dataTable.NewRow();
            dataRow["Data"] = data;
            dataRow["Dieta"] = dieta;
            dataRow["Miasto"] = miasto;
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

        }

        public static void Update(Jadlospis jadlospis, string data, string dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            Delete(data, dieta, miasto);
            Insert(data, dieta, miasto, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);
        }

        public static void Delete(string data, string miasto, string dieta)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
            {

                if (DataSet.Tables["Jadłospisy"].Rows[i]["Data"].ToString() == data && DataSet.Tables["Jadłospisy"].Rows[i]["Dieta"].ToString() == dieta && DataSet.Tables["Jadłospisy"].Rows[i]["Miasto"].ToString() == miasto)
                {
                    DataSet.Tables["Jadłospisy"].Rows[i].Delete();
                }

            }
            DataSet.WriteXml(XML_Location);
        }

        public static Jadlospis SelectAll(string data, string miasto, string dieta)
        {
            Jadlospis jadlospis = null;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if(DataSet.Jadłospisy.Rows[i]["Data"].ToString()==data&& DataSet.Jadłospisy.Rows[i]["Dieta"].ToString()==dieta && DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()==miasto )
                        jadlospis = new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> SelectAll(string dataOd, string dataDo)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString())>=Convert.ToDateTime(dataOd)&& Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString())<=Convert.ToDateTime(dataDo))
                        jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> SelectAll(string dataOd, string dataDo, string miasto, string dieta)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString()) >= Convert.ToDateTime(dataOd) && Convert.ToDateTime(DataSet.Jadłospisy.Rows[i]["Data"].ToString()) <= Convert.ToDateTime(dataDo) && DataSet.Jadłospisy.Rows[i]["Dieta"].ToString() == dieta && DataSet.Jadłospisy.Rows[i]["Miasto"].ToString() == miasto)
                        jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> Select(string data, string miasto)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (DataSet.Jadłospisy.Rows[i]["Data"].ToString() == data && DataSet.Jadłospisy.Rows[i]["Miasto"].ToString() == miasto)
                    {
                        int? j = Check(jadlospis, DataSet.Jadłospisy.Rows[i]["Dieta"].ToString());
                        if (j != null) {
                            jadlospis.RemoveAt(Convert.ToInt32(j));
                        }
                        jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
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

            int identyfikatorJadlospisu = SelectId(new Jadlospis(null, dzien, dieta, nazwa_sniadanie, nazwa_IIsniadanie, nazwa_obiad, nazwa_podwieczorek, nazwa_kolacja, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad,sklad_podwieczorek, sklad_kolacja));

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

        public static Jadlospis SelectFromId(int id)
        {
            Jadlospis jadlospis = null;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            //for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            //{
            //    if (DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString() == id.ToString())
            //    {
            //        jadlospis = new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DAO.DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
            //    }
            //}

            DataTable orders = DataSet.Tables["Jadlospis"];
            EnumerableRowCollection<DataRow> query = from order in orders.AsEnumerable()
                                                     where order.Field<String>("Identyfikator") == id.ToString()
                                                     select order;

            foreach (DataRow prod in query)
            {
                jadlospis = new Jadlospis(prod.Field<String>("Data").ToString(), DietaDAO.Select(prod.Field<String>("Dieta").ToString(), prod.Field<String>("Miasto").ToString()), prod.Field<String>("Miasto").ToString(), prod.Field<String>("Nazwa-Śniadanie").ToString(), prod.Field<String>("Nazwa-IIŚniadanie").ToString(), prod.Field<String>("Nazwa-Obiad").ToString(), prod.Field<String>("Nazwa-Podwieczorek").ToString(), prod.Field<String>("Nazwa-Kolacja").ToString(), prod.Field<String>("Skład-Śniadanie").ToString(), prod.Field<String>("Skład-IIŚniadanie").ToString(), prod.Field<String>("Skład-Obiad").ToString(), prod.Field<String>("Skład-Podwieczorek").ToString(), prod.Field<String>("Skład-Kolacja").ToString());
            }



            return jadlospis;
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

        public static Dekadowka[] SelectForAllDays(Dekadowka dekadowka)
        {
            Dekadowka[] allDaysJadlospisy = new Dekadowka[dekadowka.dni];
            List<String> allIds = new List<string>();

            for (int j = 0; j < dekadowka.dni; j++)
            {
                List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                EnumerableRowCollection<String> listaIdentyfikatorowJadlospisowDekadowki = from order in DAO.DataSet.Tables["JadlsopisDekadowki"].AsEnumerable()
                                                                                           where order.Field<String>("IdentyfikatorDekadowki") == dekadowka.id.ToString()
                                                                                            && order.Field<String>("Dzien") == (j+1).ToString()
                                                                                           select order.Field<String>("IdentyfikatorJadlospisu");
                allDaysJadlospisy[j] = new Dekadowka(listaIdentyfikatorowJadlospisowDekadowki.ToList(), new List<Jadlospis>());
                allIds.AddRange(listaIdentyfikatorowJadlospisowDekadowki.ToList());
            }

            //for (int i = 0; i < DAO.DataSet.Jadlospis.Rows.Count; i++)
            //{
            //    string id = DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString();
            //    for (int j = 0; j < dekadowka.dni; j++)
            //     {
            //        if (allDaysJadlospisy[j].listaIdentyfikatorówJadlospisow.Contains(id))
            //        {
            //            allDaysJadlospisy[j].listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]), j, DietaDAO.Select(DAO.DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString(), dekadowka.miasto), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString()));
            //        }
            //    }
            //}  
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
