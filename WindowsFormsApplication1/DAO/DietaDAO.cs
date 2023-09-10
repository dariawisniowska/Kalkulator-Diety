using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApplication1.DAO
{
    public class DietaDAO
    {
        //public static void Insert(string nazwa, string miasto, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double weglowodany_przyswajalne, double blonnik)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    DataTable dtProdukty = DataSet.Tables["Diety"];
        //    DataRow drProdukty = dtProdukty.NewRow();
        //    drProdukty["Nazwa diety"] = nazwa;
        //    drProdukty["Miasto"] = miasto;
        //    drProdukty["Energia"] = energia;
        //    drProdukty["Białko"] = bialko;
        //    drProdukty["Tłuszcze"] = tluszcze;
        //    drProdukty["Węglowodany"] = weglowodany;
        //    drProdukty["Węglowodany przyswajalne"] = weglowodany_przyswajalne;
        //    drProdukty["Błonnik"] = blonnik;
        //    drProdukty["Sód"] = sod;
        //    drProdukty["Kwasy tłuszczowe nasycone"] = tluszcze_nn;
        //    dtProdukty.Rows.Add(drProdukty);
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void InsertSQL(string nazwa, string miasto, string kod, string plec, WartosciOdzywcze wartosciOdzywcze)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT Dieta(Nazwa, Miasto, Kod, Plec, EnergiaOd, EnergiaDo, " +
                    $"BialkoOd, BialkoDo, TluszczeOd, TluszczeDo, Kwasy, " +
                    $"WeglowodanyOd, WeglowodanyDo, Cukry, Blonnik, " +
                    $"Sod, WitA, WitB1, WitB2, WitB6, WitB12, Niacyna, " +
                    $"WitC, WitD, WitE, WitK, "+
                            $"Foliany, Fosfor, Magnez, Zelazo, " +
                            $"Cynk, Jod, Selen, Miedz, Cholina, KwasPantotenowy, " +
                            $"Biotyna, Mangan, Fluor, Potas) " +
                            $"VALUES ('{nazwa}', '{miasto}', '{kod}', '{plec}', {wartosciOdzywcze.energiaOd}, {wartosciOdzywcze.energiaDo}, "+
                            $"{wartosciOdzywcze.bialkoOd}, {wartosciOdzywcze.bialkoDo}, {wartosciOdzywcze.tluszczeOd}, {wartosciOdzywcze.tluszczeDo}, {wartosciOdzywcze.tluszcze_nn}, " +
                            $"{wartosciOdzywcze.weglowodanyOd}, {wartosciOdzywcze.weglowodanyDo}, {wartosciOdzywcze.cukry}, {wartosciOdzywcze.blonnik}, " +
                            $"{wartosciOdzywcze.sod}, {wartosciOdzywcze.witA}, {wartosciOdzywcze.witB1}, {wartosciOdzywcze.witB2}, {wartosciOdzywcze.witB6}, {wartosciOdzywcze.witB12}, {wartosciOdzywcze.niacyna}," +
                            $" {wartosciOdzywcze.witC}, {wartosciOdzywcze.witD}, {wartosciOdzywcze.witE}, {wartosciOdzywcze.witK}, " +
                            $"{wartosciOdzywcze.foliany}, {wartosciOdzywcze.fosfor}, {wartosciOdzywcze.magnez}, {wartosciOdzywcze.zelazo}, " +
                            $"{wartosciOdzywcze.cynk}, { wartosciOdzywcze.jod}, { wartosciOdzywcze.selen}, {wartosciOdzywcze.miedz}, {wartosciOdzywcze.cholina}, {wartosciOdzywcze.kwasPantotenowy}, " +
                            $"{wartosciOdzywcze.biotyna}, {wartosciOdzywcze.mangan}, {wartosciOdzywcze.fluor}, {wartosciOdzywcze.potas});";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //public static void Update(Dieta dieta, string nazwa, string miasto, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double weglowodany_przyswajalne, double blonnik)
        //{
        //    Delete(dieta);
        //    Insert(nazwa, miasto, energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn, weglowodany_przyswajalne,blonnik);
        //}

        public static void UpdateSQL(Dieta dieta, string nazwa, string miasto, string kod, string plec, WartosciOdzywcze wartosciOdzywcze)
        {
            DeleteSQL(dieta);
            InsertSQL(nazwa, miasto, kod, plec, wartosciOdzywcze);
        }


        //public static void Delete(Dieta dieta)
        //{
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Diety.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
        //        {
        //            if (DataSet.Diety.Rows[i]["Nazwa diety"].ToString() == dieta.nazwa && DataSet.Diety.Rows[i]["Miasto"].ToString() == dieta.miasto)
        //                DataSet.Diety.Rows[i].Delete();
        //        }
        //    }
        //    DataSet.WriteXml(XML_Location);
        //}

        public static void DeleteSQL(Dieta dieta)
        {
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE Dieta WHERE Nazwa = '{dieta.nazwa}' AND Miasto = '{dieta.miasto}' AND Plec = '{dieta.plec}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //public static List<Dieta> SelectAll(string miasto)
        //{
        //    List<Dieta> listaDiet = new List<Dieta>();
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Diety.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
        //        {
        //            if (DataSet.Diety.Rows[i]["Miasto"].ToString()==miasto)
        //                listaDiet.Add(new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), new WartosciOdzywcze(Convert.ToDouble(DataSet.Diety.Rows[i]["Energia"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Białko"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Tłuszcze"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Węglowodany"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Sód"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Kwasy tłuszczowe nasycone"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Węglowodany przyswajalne"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Błonnik"]))));
        //        }
        //    }

        //    return listaDiet;
        //}

        public static List<Dieta> SelectAllSQL(string miasto)
        {
            List<Dieta> listaDiet = new List<Dieta>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Dieta WHERE Miasto = '{miasto}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaDiet.Add(new Dieta(reader["Nazwa"].ToString(), reader["Miasto"].ToString(), reader["Kod"].ToString(), reader["Plec"].ToString(), new WartosciOdzywcze(Convert.ToDouble(reader["EnergiaOd"]), Convert.ToDouble(reader["EnergiaDo"]),
                                Convert.ToDouble(reader["BialkoOd"]), Convert.ToDouble(reader["BialkoDo"]), Convert.ToDouble(reader["TluszczeOd"]), Convert.ToDouble(reader["TluszczeDo"]), Convert.ToDouble(reader["Kwasy"]), Convert.ToDouble(reader["WeglowodanyOd"]), Convert.ToDouble(reader["WeglowodanyDo"]),
                                Convert.ToDouble(reader["Cukry"]), Convert.ToDouble(reader["Blonnik"]), Convert.ToDouble(reader["Sod"]), Convert.ToDouble(reader["WitA"]),
                                Convert.ToDouble(reader["WitB1"]), Convert.ToDouble(reader["WitB2"]), Convert.ToDouble(reader["WitB6"]), Convert.ToDouble(reader["WitB12"]),
                                Convert.ToDouble(reader["Niacyna"]), Convert.ToDouble(reader["WitC"]), Convert.ToDouble(reader["WitD"]), Convert.ToDouble(reader["WitE"]),
                                Convert.ToDouble(reader["WitK"]), Convert.ToDouble(reader["Foliany"]), Convert.ToDouble(reader["Fosfor"]), Convert.ToDouble(reader["Magnez"]),
                                Convert.ToDouble(reader["Zelazo"]), Convert.ToDouble(reader["Cynk"]), Convert.ToDouble(reader["Jod"]), Convert.ToDouble(reader["Selen"]), Convert.ToDouble(reader["Miedz"]),
                                Convert.ToDouble(reader["Cholina"]), Convert.ToDouble(reader["KwasPantotenowy"]), Convert.ToDouble(reader["Biotyna"]),
                                Convert.ToDouble(reader["Mangan"]), Convert.ToDouble(reader["Fluor"]), Convert.ToDouble(reader["Potas"]))));
                        }
                    }
                }
            }
            return listaDiet;
        }

        public static List<Dieta> SelectAllSQL()
        {
            List<Dieta> listaDiet = new List<Dieta>();
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Dieta;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaDiet.Add(new Dieta(reader["Nazwa"].ToString(), reader["Miasto"].ToString(), reader["Kod"].ToString(), reader["Plec"].ToString(), new WartosciOdzywcze(Convert.ToDouble(reader["EnergiaOd"]), Convert.ToDouble(reader["EnergiaDo"]),
                                Convert.ToDouble(reader["BialkoOd"]), Convert.ToDouble(reader["BialkoDo"]), Convert.ToDouble(reader["TluszczeOd"]), Convert.ToDouble(reader["TluszczeDo"]), Convert.ToDouble(reader["Kwasy"]), Convert.ToDouble(reader["WeglowodanyOd"]), Convert.ToDouble(reader["WeglowodanyDo"]),
                                Convert.ToDouble(reader["Cukry"]), Convert.ToDouble(reader["Blonnik"]), Convert.ToDouble(reader["Sod"]), Convert.ToDouble(reader["WitA"]),
                                Convert.ToDouble(reader["WitB1"]), Convert.ToDouble(reader["WitB2"]), Convert.ToDouble(reader["WitB6"]), Convert.ToDouble(reader["WitB12"]),
                                Convert.ToDouble(reader["Niacyna"]), Convert.ToDouble(reader["WitC"]), Convert.ToDouble(reader["WitD"]), Convert.ToDouble(reader["WitE"]),
                                Convert.ToDouble(reader["WitK"]), Convert.ToDouble(reader["Foliany"]), Convert.ToDouble(reader["Fosfor"]), Convert.ToDouble(reader["Magnez"]),
                                Convert.ToDouble(reader["Zelazo"]), Convert.ToDouble(reader["Cynk"]), Convert.ToDouble(reader["Jod"]), Convert.ToDouble(reader["Selen"]), Convert.ToDouble(reader["Miedz"]),
                                Convert.ToDouble(reader["Cholina"]), Convert.ToDouble(reader["KwasPantotenowy"]), Convert.ToDouble(reader["Biotyna"]),
                                Convert.ToDouble(reader["Mangan"]), Convert.ToDouble(reader["Fluor"]), Convert.ToDouble(reader["Potas"]))));
                        }
                    }
                }
            }
            return listaDiet;
        }

        //public static Dieta Select(string nazwa, string miasto)
        //{
        //    Dieta dieta = null;
        //    KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        //    String XML_Location = @"DataBase.xml";
        //    DataSet.ReadXml(XML_Location);
        //    if (DataSet.Diety.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
        //        {
        //            if (DataSet.Diety.Rows[i]["Nazwa diety"].ToString() == nazwa && DataSet.Diety.Rows[i]["Miasto"].ToString() == miasto)
        //            {
        //                try
        //                {
        //                    dieta = new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), new WartosciOdzywcze(Convert.ToDouble(DataSet.Diety.Rows[i]["Energia"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Białko"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Tłuszcze"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Węglowodany"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Sód"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Kwasy tłuszczowe nasycone"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Węglowodany przyswajalne"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Błonnik"])));
        //                }
        //                catch
        //                {
        //                    dieta = new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), new WartosciOdzywcze(Convert.ToDouble(DataSet.Diety.Rows[i]["Energia"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Białko"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Tłuszcze"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Węglowodany"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Sód"]), Convert.ToDouble(DataSet.Diety.Rows[i]["Kwasy tłuszczowe nasycone"]),0,0));
        //                }
        //                }
        //        }
        //    }

        //    return dieta;
        //}

        public static Dieta SelectSQL(string nazwa, string miasto, string plec)
        {
            Dieta dieta = null;
            using (SqlConnection connection = new SqlConnection(DAO.ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM Dieta WHERE Nazwa = '{nazwa}' AND Miasto = '{miasto}' AND Plec = '{plec}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dieta = new Dieta(reader["Nazwa"].ToString(), reader["Miasto"].ToString(), reader["Kod"].ToString(), reader["Plec"].ToString(), new WartosciOdzywcze(Convert.ToDouble(reader["EnergiaOd"]), Convert.ToDouble(reader["EnergiaDo"]),
                                Convert.ToDouble(reader["BialkoOd"]), Convert.ToDouble(reader["BialkoDo"]), Convert.ToDouble(reader["TluszczeOd"]), Convert.ToDouble(reader["TluszczeDo"]), Convert.ToDouble(reader["Kwasy"]), Convert.ToDouble(reader["WeglowodanyOd"]), Convert.ToDouble(reader["WeglowodanyDo"]),
                                Convert.ToDouble(reader["Cukry"]), Convert.ToDouble(reader["Blonnik"]), Convert.ToDouble(reader["Sod"]), Convert.ToDouble(reader["WitA"]),
                                Convert.ToDouble(reader["WitB1"]), Convert.ToDouble(reader["WitB2"]), Convert.ToDouble(reader["WitB6"]), Convert.ToDouble(reader["WitB12"]),
                                Convert.ToDouble(reader["Niacyna"]), Convert.ToDouble(reader["WitC"]), Convert.ToDouble(reader["WitD"]), Convert.ToDouble(reader["WitE"]),
                                Convert.ToDouble(reader["WitK"]), Convert.ToDouble(reader["Foliany"]), Convert.ToDouble(reader["Fosfor"]), Convert.ToDouble(reader["Magnez"]),
                                Convert.ToDouble(reader["Zelazo"]), Convert.ToDouble(reader["Cynk"]), Convert.ToDouble(reader["Jod"]), Convert.ToDouble(reader["Selen"]), Convert.ToDouble(reader["Miedz"]),
                                Convert.ToDouble(reader["Cholina"]), Convert.ToDouble(reader["KwasPantotenowy"]), Convert.ToDouble(reader["Biotyna"]),
                                Convert.ToDouble(reader["Mangan"]), Convert.ToDouble(reader["Fluor"]), Convert.ToDouble(reader["Potas"])));
                        }
                    }
                }
            }
            return dieta;
        }
    }
}
