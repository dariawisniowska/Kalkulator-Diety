using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    class ProduktDAO
    {
        public static void Insert(string nazwa, char kategoria, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double przyswajalne, double blonnik)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dtProdukty = DataSet.Tables["Produkt"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Nazwa produktu"] = nazwa;
            drProdukty["Kategoria"] = kategoria;
            drProdukty["Energia"] = energia;
            drProdukty["Białko"] = bialko;
            drProdukty["Tłuszcze"] = tluszcze;
            drProdukty["Węglowodany"] = weglowodany;
            drProdukty["Węglowodany przyswajalne"] = przyswajalne;
            drProdukty["Błonnik"] = blonnik;
            drProdukty["Sód"] = sod;
            drProdukty["Kwasy tłuszczowe nasycone"] = tluszcze_nn;
            dtProdukty.Rows.Add(drProdukty);
            DataSet.WriteXml(XML_Location);
        }

        public static void Update(Produkt produkt, string nazwa, char kategoria, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double przyswajalne, double blonnik)
        {
            Delete(produkt);
            Insert(nazwa, kategoria, energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn, przyswajalne, blonnik);
        }

        public static void Delete(Produkt produkt)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            for (int i = 0; i < DataSet.Produkt.Rows.Count; i++)
            {

                if (DataSet.Tables["Produkt"].Rows[i]["Nazwa produktu"].ToString() == produkt.nazwa && DataSet.Tables["Produkt"].Rows[i]["Energia"].ToString()==produkt.wartosciOdzywcze.energia.ToString())
                {
                    DataSet.Tables["Produkt"].Rows[i].Delete();
                }

            }
            DataSet.WriteXml(XML_Location);
        }

        public static List<Produkt> SelectAll()
        {
            List<Produkt> listaProduktow = new List<Produkt>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Produkt.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Produkt.Rows.Count; i++)
                {
                    try
                    {
                        listaProduktow.Add(new Produkt(Convert.ToChar(DataSet.Produkt.Rows[i]["Kategoria"]), DataSet.Produkt.Rows[i]["Nazwa produktu"].ToString(), Convert.ToDouble(DataSet.Produkt.Rows[i]["Energia"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Białko"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Tłuszcze"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Węglowodany"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Sód"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Kwasy tłuszczowe nasycone"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Węglowodany przyswajalne"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Błonnik"])));
                    }
                    catch
                    {
                        Produkt p = new Produkt(Convert.ToChar(DataSet.Produkt.Rows[i]["Kategoria"]), DataSet.Produkt.Rows[i]["Nazwa produktu"].ToString(), Convert.ToDouble(DataSet.Produkt.Rows[i]["Energia"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Białko"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Tłuszcze"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Węglowodany"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Sód"]), Convert.ToDouble(DataSet.Produkt.Rows[i]["Kwasy tłuszczowe nasycone"]),0,0);
                        listaProduktow.Add(p);
                    }
                }
            }

            return listaProduktow;
        }

    }
}
