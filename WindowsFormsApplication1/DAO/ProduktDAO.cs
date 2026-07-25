namespace KalkulatorDiety.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    class ProduktDAO
    {
        public static void Insert(string nazwa, char kategoria, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double przyswajalne, double blonnik, double cukry)
        {
            DataTable dtProdukty = DAO.DataSet.Tables["Produkt"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Nazwa produktu"] = nazwa;
            drProdukty["Kategoria"] = kategoria;
            drProdukty["Energia"] = energia;
            drProdukty["Białko"] = bialko;
            drProdukty["Tłuszcze"] = tluszcze;
            drProdukty["Węglowodany"] = weglowodany;
            drProdukty["Węglowodany przyswajalne"] = przyswajalne;
            drProdukty["Cukry"] = cukry;
            drProdukty["Błonnik"] = blonnik;
            drProdukty["Sód"] = sod;
            drProdukty["Kwasy tłuszczowe nasycone"] = tluszcze_nn;
            dtProdukty.Rows.Add(drProdukty);
            DAO.WriteXml();
        }

        public static void Update(Produkt produkt, string nazwa, char kategoria, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double przyswajalne, double blonnik, double cukry)
        {
            Delete(produkt);
            Insert(nazwa, kategoria, energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn, przyswajalne, blonnik, cukry);
        }

        public static void Delete(Produkt produkt)
        {
            for (int i = 0; i < DAO.DataSet.Produkt.Rows.Count; i++)
            {
                if (DAO.DataSet.Tables["Produkt"].Rows[i]["Nazwa produktu"].ToString() == produkt.nazwa && DAO.DataSet.Tables["Produkt"].Rows[i]["Energia"].ToString()==produkt.wartosciOdzywcze.energia.ToString())
                {
                    DAO.DataSet.Tables["Produkt"].Rows[i].Delete();
                }

            }
            DAO.WriteXml();
        }

        public static List<Produkt> SelectAll()
        {
            List<Produkt> listaProduktow = new List<Produkt>();

            if (DAO.DataSet.Produkt.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Produkt.Rows.Count; i++)
                {
                    try
                    {
                        listaProduktow.Add(new Produkt(Convert.ToChar(DAO.DataSet.Produkt.Rows[i]["Kategoria"]), DAO.DataSet.Produkt.Rows[i]["Nazwa produktu"].ToString(), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Energia"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Białko"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Tłuszcze"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Węglowodany"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Sód"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Kwasy tłuszczowe nasycone"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Węglowodany przyswajalne"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Błonnik"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Cukry"])));
                    }
                    catch
                    {
                        Produkt p = new Produkt(Convert.ToChar(DAO.DataSet.Produkt.Rows[i]["Kategoria"]), DAO.DataSet.Produkt.Rows[i]["Nazwa produktu"].ToString(), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Energia"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Białko"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Tłuszcze"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Węglowodany"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Sód"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Kwasy tłuszczowe nasycone"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Węglowodany przyswajalne"]), Convert.ToDouble(DAO.DataSet.Produkt.Rows[i]["Błonnik"]),0);
                        listaProduktow.Add(p);
                    }
                }
            }

            return listaProduktow;
        }
    }
}
