using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    class DekadowkaDAO
    {
        public static void Insert(string nazwa, string miasto, int dni, string dzienStart, List<Jadlospis> listaJadlospisow)
        {

            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dataTable = DataSet.Tables["Dekadowka"];
            DataRow dataRow = dataTable.NewRow();
            dataRow["Nazwa"] = nazwa;
            dataRow["Miasto"] = miasto;
            dataRow["DzienStart"] = dzienStart;
            dataRow["Dni"] = dni;
            dataTable.Rows.Add(dataRow);
            DataSet.WriteXml(XML_Location);

            int identyfikatorDekadowki = SelectId(new Dekadowka(null, nazwa, miasto, dni, dzienStart, null));

            if(listaJadlospisow!=null)
                foreach (Jadlospis jadlospis in listaJadlospisow)
                    DAO.JadlospisDekadowkiDAO.Insert(identyfikatorDekadowki, jadlospis.dzien, jadlospis.dieta, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja);
        }

        public static int SelectId(Dekadowka dekadowka)
        {
            int identyfikatorDekadowki = 0;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString() == dekadowka.nazwa && DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString() == dekadowka.miasto && DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString() == dekadowka.dzienStart && DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString() == dekadowka.dni.ToString())
                {
                    identyfikatorDekadowki = Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]);
                }
            }
            return identyfikatorDekadowki;
        }

        public static Dekadowka SelectFromId(int id)
        {
            Dekadowka dekadowka = null;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString() == id.ToString())
                {
                    dekadowka = new Dekadowka(Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]), DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString(), DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString(), Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString()), DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString(), null);
        }
            }
            return dekadowka;
        }

        public static void Update(Dekadowka dekadowka, string nazwa, string miasto, int dni, string dzienStart, List<Jadlospis> listaJadlospisow)
        {
            Delete(dekadowka);
            Insert(nazwa, miasto, dni, dzienStart, listaJadlospisow);
        }

        public static void Delete(Dekadowka dekadowka)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Dekadowka.Rows.Count; i++)
            {
                if (DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString() == dekadowka.id.ToString())
                {
                    DataSet.Tables["Dekadowka"].Rows[i].Delete();
                }
            }
            DataRowCollection collection = DataSet.JadlsopisDekadowki.Rows;
            for (int i = collection.Count-1; i >=0; i--)
            {
                if (DataSet.Tables["JadlsopisDekadowki"].Rows[i]["IdentyfikatorDekadowki"].ToString() == dekadowka.id.ToString())
                {
                    DataSet.Tables["JadlsopisDekadowki"].Rows[i].Delete();
                }
            }
            DataSet.WriteXml(XML_Location);
        }
        public static List<Dekadowka> SelectAll()
        {
            List<Dekadowka> listaDekadowek = new List<Dekadowka>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);

            
            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Dekadowka.Rows.Count; i++)
                {
                    List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
                    for (int j = 0; j < DataSet.JadlsopisDekadowki.Rows.Count; j++)
                    {
                        if (DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorDekadowki"].ToString() == DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString())
                        {
                            listaIdentyfikatorowJadlospisowDekadowki.Add(i);
                        }
                    }
                    List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                    for (int k = 0; k < DataSet.Jadlospis.Rows.Count; k++)
                    {
                        if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"].ToString())))
                        {
                            listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"]), SelectDzien(Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString()), Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"])), DietaDAO.Select(DataSet.Tables["Jadlospis"].Rows[k]["Dieta"].ToString(), DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString()), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Kolacja"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Kolacja"].ToString()));
                        }
                    }

                    listaDekadowek.Add(new Dekadowka(Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]), DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString(), DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString(), Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString()), DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString(), listaJadlospisow));
                }
            }

            return listaDekadowek;
        }

        public static List<Dekadowka> Select(string miasto)
        {
            List<Dekadowka> listaDekadowek = new List<Dekadowka>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);

            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Dekadowka.Rows.Count; i++)
                {
                    if (DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString() == miasto)
                    {
                        List<int> listaIdentyfikatorowJadlospisowDekadowki = new List<int>();
                        for (int j = 0; j < DataSet.JadlsopisDekadowki.Rows.Count; j++)
                        {
                            if (DataSet.Tables["JadlsopisDekadowki"].Rows[j]["IdentyfikatorDekadowki"].ToString() == DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString())
                            {
                                listaIdentyfikatorowJadlospisowDekadowki.Add(i);
                            }
                        }
                        List<Jadlospis> listaJadlospisow = new List<Jadlospis>();
                        for (int k = 0; k < DataSet.Jadlospis.Rows.Count; k++)
                        {
                            if (listaIdentyfikatorowJadlospisowDekadowki.Contains(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"].ToString())))
                            {
                                listaJadlospisow.Add(new Jadlospis(Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"]), SelectDzien(Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"].ToString()),Convert.ToInt32(DataSet.Tables["Jadlospis"].Rows[k]["Identyfikator"])), DietaDAO.Select(DataSet.Tables["Jadlospis"].Rows[k]["Dieta"].ToString(), DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString()), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Nazwa-Kolacja"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Śniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-IIŚniadanie"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Obiad"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Podwieczorek"].ToString(), DataSet.Tables["Jadlospis"].Rows[k]["Skład-Kolacja"].ToString()));
                            }
                        }

                        listaDekadowek.Add(new Dekadowka(Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Identyfikator"]), DataSet.Tables["Dekadowka"].Rows[i]["Nazwa"].ToString(), DataSet.Tables["Dekadowka"].Rows[i]["Miasto"].ToString(), Convert.ToInt32(DataSet.Tables["Dekadowka"].Rows[i]["Dni"].ToString()), DataSet.Tables["Dekadowka"].Rows[i]["DzienStart"].ToString(), listaJadlospisow));
                    }
                }
            }

            return listaDekadowek;
        }

        public static int SelectDzien(int idDekadowki, int id)
        {
            int dzien = 0;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);

            if (DataSet.Diety.Rows.Count > 0)
            {
                
                        for (int k = 0; k < DataSet.JadlsopisDekadowki.Rows.Count; k++)
                        {
                            if (Convert.ToInt32(DataSet.Tables["JadlsopisDekadowki"].Rows[k]["IdentyfikatorJadlospisu"].ToString())==id && Convert.ToInt32(DataSet.Tables["JadlsopisDekadowki"].Rows[k]["IdentyfikatorDekadowki"].ToString()) == idDekadowki)
                            {
                                dzien = Convert.ToInt32(DataSet.Tables["JadlospisDekadowki"].Rows[k]["Dzien"]);
                            }
                        }
            }

            return dzien;
        }
    }
}
