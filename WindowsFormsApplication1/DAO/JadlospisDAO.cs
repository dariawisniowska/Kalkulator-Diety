using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        jadlospis = new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DAO.DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
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
                        jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DAO.DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
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
                        jadlospis.Add(new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DAO.DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
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
            for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            {
                if (DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString() == id.ToString())
                {
                    jadlospis = new Jadlospis(DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DAO.DietaDAO.Select(DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
                }
            }
            return jadlospis;
        }

        public static List<Jadlospis> SelectForDay(int identyfikatorDekadowki, int dzien)
        {
            List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
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

            Dekadowka dekadowka = DekadowkaDAO.SelectFromId(identyfikatorDekadowki);
            for (int i = 0; i < DataSet.Jadlospis.Rows.Count; i++)
            {
                if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"].ToString())))
                {
                    listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[i]["Identyfikator"]), dzien,DietaDAO.Select(DataSet.Tables["Jadlospis"].Rows[i]["Dieta"].ToString(), dekadowka.miasto), DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Nazwa-Kolacja"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Skład-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Skład-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Skład-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Skład-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[i]["Skład-Kolacja"].ToString()));
                }
            }

            return listaJadlospisow;
        }

    }
}
