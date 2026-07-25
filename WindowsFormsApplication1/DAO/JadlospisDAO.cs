namespace KalkulatorDiety.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class JadlospisDAO
    {
        public static void Insert(string data, string dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja, bool reload = true)
        {
            DataTable dataTable = DAO.DataSet.Tables["Jadłospisy"];
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
            DAO.WriteXml();
        }

        public static void Delete(string data, string miasto, string dieta)
        {
            for (int i = 0; i < DAO.DataSet.Jadłospisy.Rows.Count; i++)
            {

                if (DAO.DataSet.Tables["Jadłospisy"].Rows[i]["Data"].ToString() == data && DAO.DataSet.Tables["Jadłospisy"].Rows[i]["Dieta"].ToString() == dieta && DAO.DataSet.Tables["Jadłospisy"].Rows[i]["Miasto"].ToString() == miasto)
                {
                    DAO.DataSet.Tables["Jadłospisy"].Rows[i].Delete();
                }

            }
            DAO.WriteXml();
        }

        public static Jadlospis SelectAll(string data, string miasto, string dieta)
        {
            Jadlospis jadlospis = null;
            if (DAO.DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString()==data&& DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString()==dieta && DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()==miasto )
                        jadlospis = new Jadlospis(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString());
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> SelectAll(string dataOd, string dataDo)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();
            if (DAO.DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString())>=Convert.ToDateTime(dataOd)&& Convert.ToDateTime(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString())<=Convert.ToDateTime(dataDo))
                        jadlospis.Add(new Jadlospis(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> SelectAll(string dataOd, string dataDo, string miasto, string dieta)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();

            if (DAO.DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString()) >= Convert.ToDateTime(dataOd) && Convert.ToDateTime(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString()) <= Convert.ToDateTime(dataDo) && DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString() == dieta && DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString() == miasto)
                        jadlospis.Add(new Jadlospis(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
                }
            }

            return jadlospis;
        }

        public static List<Jadlospis> Select(string data, string miasto)
        {
            List<Jadlospis> jadlospis = new List<Jadlospis>();

            if (DAO.DataSet.Jadłospisy.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jadłospisy.Rows.Count; i++)
                {
                    if (DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString() == data && DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString() == miasto)
                    {
                        int? j = Check(jadlospis, DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString());
                        if (j != null) {
                            jadlospis.RemoveAt(Convert.ToInt32(j));
                        }
                        jadlospis.Add(new Jadlospis(DAO.DataSet.Jadłospisy.Rows[i]["Data"].ToString(), DietaDAO.Select(DAO.DataSet.Jadłospisy.Rows[i]["Dieta"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString()), DAO.DataSet.Jadłospisy.Rows[i]["Miasto"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Nazwa-Kolacja"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Śniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-IIŚniadanie"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Obiad"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Podwieczorek"].ToString(), DAO.DataSet.Jadłospisy.Rows[i]["Skład-Kolacja"].ToString()));
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