using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        public static void InsertSQL(string nazwa, char kategoria, WartosciOdzywcze wartosciOdzywcze)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Produkt(Nazwa, Kategoria, Energia, Bialko, Tluszcze, Kwasy, Weglowodany, Cukry, Blonnik, Sod, WitA, WitB1, WitB2, WitB6, WitB12, Niacyna, WitC, WitD, WitE, WitK, " +
                            $"Foliany, Fosfor, Magnez, Zelazo, Cynk, Selen, Cholina, KwasPantotenowy, Biotyna, Mangan, Fluor, Potas) VALUES ({nazwa}, {kategoria}, {wartosciOdzywcze.energia}, " +
                            $"{wartosciOdzywcze.bialko}, {wartosciOdzywcze.tluszcze}, {wartosciOdzywcze.tluszcze_nn}, {wartosciOdzywcze.weglowodany}, {wartosciOdzywcze.cukry}, {wartosciOdzywcze.blonnik}, {wartosciOdzywcze.sod}, {wartosciOdzywcze.witA}, {wartosciOdzywcze.witB1}, {wartosciOdzywcze.witB2}, {wartosciOdzywcze.witB6}, {wartosciOdzywcze.witB12}, {wartosciOdzywcze.niacyna}, {wartosciOdzywcze.witC}, {wartosciOdzywcze.witD}, {wartosciOdzywcze.witE}, {wartosciOdzywcze.witK}, {wartosciOdzywcze.foliany}, {wartosciOdzywcze.fosfor}, {wartosciOdzywcze.magnez}, {wartosciOdzywcze.zelazo}, {wartosciOdzywcze.cynk}, {wartosciOdzywcze.selen}, {wartosciOdzywcze.miedz}, {wartosciOdzywcze.cholina}, {wartosciOdzywcze.kwasPantotenowy}, {wartosciOdzywcze.biotyna}, {wartosciOdzywcze.mangan}, {wartosciOdzywcze.fluor}, {wartosciOdzywcze.potas});";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Update(Produkt produkt, string nazwa, char kategoria, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double przyswajalne, double blonnik)
        {
            Delete(produkt);
            Insert(nazwa, kategoria, energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn, przyswajalne, blonnik);
        }

        public static void UpdateSQL(Produkt produkt, string nazwa, char kategoria, WartosciOdzywcze wartosciOdzywcze)
        {
            DeleteSQL(produkt);
            InsertSQL(nazwa, kategoria, wartosciOdzywcze);
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

        public static void DeleteSQL(Produkt produkt)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Produkt WHERE Nazwa = '{produkt.nazwa}' AND Energia = '{produkt.wartosciOdzywcze.energia}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
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

        public static List<Produkt> SelectAllSQL()
        {
            List<Produkt> listaProduktow = new List<Produkt>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Produkt;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaProduktow.Add(new Produkt(reader["Kategoria"].ToString()[0], reader["Nazwa"].ToString(), new WartosciOdzywcze(Convert.ToDouble(reader["Energia"]),
                                Convert.ToDouble(reader["Bialko"]), Convert.ToDouble(reader["Tluszcze"]), Convert.ToDouble(reader["Kwasy"]), Convert.ToDouble(reader["Weglowodany"]),
                                Convert.ToDouble(reader["Cukry"]), Convert.ToDouble(reader["Blonnik"]), Convert.ToDouble(reader["Sod"]), Convert.ToDouble(reader["WitA"]),
                                Convert.ToDouble(reader["WitB1"]), Convert.ToDouble(reader["WitB2"]), Convert.ToDouble(reader["WitB6"]), Convert.ToDouble(reader["WitB12"]),
                                Convert.ToDouble(reader["Niacyna"]), Convert.ToDouble(reader["WitC"]), Convert.ToDouble(reader["WitD"]), Convert.ToDouble(reader["WitE"]),
                                Convert.ToDouble(reader["WitK"]), Convert.ToDouble(reader["Foliany"]), Convert.ToDouble(reader["Fosfor"]), Convert.ToDouble(reader["Magnez"]),
                                Convert.ToDouble(reader["Zelazo"]), Convert.ToDouble(reader["Cynk"]), Convert.ToDouble(reader["Selen"]), Convert.ToDouble(reader["Miedx"]),
                                Convert.ToDouble(reader["Cholina"]), Convert.ToDouble(reader["KwasPantotenowy"]), Convert.ToDouble(reader["Biotyna"]),
                                Convert.ToDouble(reader["Mangan"]), Convert.ToDouble(reader["Fluor"]), Convert.ToDouble(reader["Potas"]))));
                        }
                    }
                }
            }
            return listaProduktow;
        }
    }
}
