namespace KalkulatorDiety.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class JednostkaDAO
    {
        public static void Insert(string miasto)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dtProdukty = DataSet.Tables["Jednostka"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Miasto"] = miasto;
            dtProdukty.Rows.Add(drProdukty);
            DataSet.WriteXml(XML_Location);
        }

        public static void Update(Jednostka jednostka, string miasto)
        {
            Delete(jednostka);
            Insert(miasto);
        }

        public static void Delete(Jednostka jednostka)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jednostka.Rows.Count; i++)
                {
                    if (DataSet.Jednostka.Rows[i]["Miasto"].ToString() == jednostka.miasto)
                        DataSet.Jednostka.Rows[i].Delete();
                }
            }
            DataSet.WriteXml(XML_Location);
        }
        public static List<Jednostka> SelectAll()
        {
            List<Jednostka> listaJednostek = new List<Jednostka>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Jednostka.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Jednostka.Rows.Count; i++)
                {
                    listaJednostek.Add(new Jednostka(DataSet.Jednostka.Rows[i]["Miasto"].ToString()));
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
