namespace KalkulatorDiety.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class DietaDAO
    {
        public static void Insert(string nazwa, string miasto, string kod,
            double energiaOd, double energiaDo, double energiaOdNaTysiąc, double energiaDoNaTysiąc, double energiaProcentOd, double energiaProcentDo,
            double bialkoOd, double bialkoDo, double bialkoOdNaTysiąc, double bialkoDoNaTysiąc, double bialkoProcentOd, double bialkoProcentDo,
            double tluszczeOd, double tluszczeDo, double tluszczeOdNaTysiąc, double tluszczeDoNaTysiąc, double tluszczeProcentOd, double tluszczeProcentDo,
            double kwasyOd, double kwasyDo, double kwasyOdNaTysiąc, double kwasyDoNaTysiąc, double kwasyProcentOd, double kwasyProcentDo,
            double wegleOd, double wegleDo, double wegleOdNaTysiąc, double wegleDoNaTysiąc, double wegleProcentOd, double wegleProcentDo,
            double przyswajalneOd, double przyswajalneDo, double przyswajalneOdNaTysiąc, double przyswajalneDoNaTysiąc, double przyswajalneProcentOd, double przyswajalneProcentDo,
            double cukryOd, double cukryDo, double cukryOdNaTysiąc, double cukryDoNaTysiąc, double cukryProcentOd, double cukryProcentDo,
            double blonnikOd, double blonnikDo, double blonnikOdNaTysiąc, double blonnikDoNaTysiąc, double blonnikProcentOd, double blonnikProcentDo,
            double sodOd, double sodDo, double sodOdNaTysiąc, double sodDoNaTysiąc, double sodProcentOd, double sodProcentDo,
            double solOd, double solDo, double solOdNaTysiąc, double solDoNaTysiąc, double solProcentOd, double solProcentDo)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            DataTable dtProdukty = DataSet.Tables["Diety"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Nazwa diety"] = nazwa;
            drProdukty["Miasto"] = miasto;
            drProdukty["Kod"] = kod;

            drProdukty["EnergiaOd"] = energiaOd;
            drProdukty["EnergiaDo"] = energiaDo;
            drProdukty["EnergiaOdNaTysiac"] = energiaOdNaTysiąc;
            drProdukty["EnergiaDoNaTysiac"] = energiaDoNaTysiąc;
            drProdukty["EnergiaOdProcent"] = energiaProcentOd;
            drProdukty["EnergiaDoProcent"] = energiaProcentDo;

            drProdukty["BialkoOd"] = bialkoOd;
            drProdukty["BialkoDo"] = bialkoDo;
            drProdukty["BialkoOdNaTysiac"] = bialkoOdNaTysiąc;
            drProdukty["BialkoDoNaTysiac"] = bialkoDoNaTysiąc;
            drProdukty["BialkoOdProcent"] = bialkoProcentOd;
            drProdukty["BialkoDoProcent"] = bialkoProcentDo;

            drProdukty["TluszczeOd"] = tluszczeOd;
            drProdukty["TluszczeDo"] = tluszczeDo;
            drProdukty["TluszczeOdNaTysiac"] = tluszczeOdNaTysiąc;
            drProdukty["TluszczeDoNaTysiac"] = tluszczeDoNaTysiąc;
            drProdukty["TluszczeOdProcent"] = tluszczeProcentOd;
            drProdukty["TluszczeDoProcent"] = tluszczeProcentDo;

            drProdukty["KwasyOd"] = kwasyOd;
            drProdukty["KwasyDo"] = kwasyDo;
            drProdukty["KwasyOdNaTysiac"] = kwasyOdNaTysiąc;
            drProdukty["KwasyDoNaTysiac"] = kwasyDoNaTysiąc;
            drProdukty["KwasyOdProcent"] = kwasyProcentOd;
            drProdukty["KwasyDoProcent"] = kwasyProcentDo;

            drProdukty["WeglowodanyOd"] = wegleOd;
            drProdukty["WeglowodanyDo"] = wegleDo;
            drProdukty["WeglowodanyOdNaTysiac"] = wegleOdNaTysiąc;
            drProdukty["WeglowodanyDoNaTysiac"] = wegleDoNaTysiąc;
            drProdukty["WeglowodanyOdProcent"] = wegleProcentOd;
            drProdukty["WeglowodanyDoProcent"] = wegleProcentDo;

            drProdukty["PrzyswajalneOd"] = przyswajalneOd;
            drProdukty["PrzyswajalneDo"] = przyswajalneDo;
            drProdukty["PrzyswajalneOdNaTysiac"] = przyswajalneOdNaTysiąc;
            drProdukty["PrzyswajalneDoNaTysiac"] = przyswajalneDoNaTysiąc;
            drProdukty["PrzyswajalneOdProcent"] = przyswajalneProcentOd;
            drProdukty["PrzyswajalneDoProcent"] = przyswajalneProcentDo;

            drProdukty["CukryOd"] = cukryOd;
            drProdukty["CukryDo"] = cukryDo;
            drProdukty["CukryOdNaTysiac"] = cukryOdNaTysiąc;
            drProdukty["CukryDoNaTysiac"] = cukryDoNaTysiąc;
            drProdukty["CukryOdProcent"] = cukryProcentOd;
            drProdukty["CukryDoProcent"] = cukryProcentDo;

            drProdukty["SodOd"] = sodOd;
            drProdukty["SodDo"] = sodDo;
            drProdukty["SodOdNaTysiac"] = sodOdNaTysiąc;
            drProdukty["SodDoNaTysiac"] = sodDoNaTysiąc;
            drProdukty["SodOdProcent"] = sodProcentOd;
            drProdukty["SodDoProcent"] = sodProcentDo;

            drProdukty["SolOd"] = solOd;
            drProdukty["SolDo"] = solDo;
            drProdukty["SolOdNaTysiac"] = solOdNaTysiąc;
            drProdukty["SolDoNaTysiac"] = solDoNaTysiąc;
            drProdukty["SolOdProcent"] = solProcentOd;
            drProdukty["SolDoProcent"] = solProcentDo;

            drProdukty["BlonnikOd"] = blonnikOd;
            drProdukty["BlonnikDo"] = blonnikDo;
            drProdukty["BlonnikOdNaTysiac"] = blonnikOdNaTysiąc;
            drProdukty["BlonnikDoNaTysiac"] = blonnikDoNaTysiąc;
            drProdukty["BlonnikOdProcent"] = blonnikProcentOd;
            drProdukty["BlonnikDoProcent"] = blonnikProcentDo;

            dtProdukty.Rows.Add(drProdukty);
            DataSet.WriteXml(XML_Location);
        }

        public static void Update(Dieta dieta, string nazwa, string miasto, string kod,
            double energiaOd, double energiaDo, double energiaOdNaTysiąc, double energiaDoNaTysiąc, double energiaProcentOd, double energiaProcentDo,
            double bialkoOd, double bialkoDo, double bialkoOdNaTysiąc, double bialkoDoNaTysiąc, double bialkoProcentOd, double bialkoProcentDo,
            double tluszczeOd, double tluszczeDo, double tluszczeOdNaTysiąc, double tluszczeDoNaTysiąc, double tluszczeProcentOd, double tluszczeProcentDo,
            double kwasyOd, double kwasyDo, double kwasyOdNaTysiąc, double kwasyDoNaTysiąc, double kwasyProcentOd, double kwasyProcentDo,
            double wegleOd, double wegleDo, double wegleOdNaTysiąc, double wegleDoNaTysiąc, double wegleProcentOd, double wegleProcentDo,
            double przyswajalneOd, double przyswajalneDo, double przyswajalneOdNaTysiąc, double przyswajalneDoNaTysiąc, double przyswajalneProcentOd, double przyswajalneProcentDo,
            double cukryOd, double cukryDo, double cukryOdNaTysiąc, double cukryDoNaTysiąc, double cukryProcentOd, double cukryProcentDo,
            double blonnikOd, double blonnikDo, double blonnikOdNaTysiąc, double blonnikDoNaTysiąc, double blonnikProcentOd, double blonnikProcentDo,
            double sodOd, double sodDo, double sodOdNaTysiąc, double sodDoNaTysiąc, double sodProcentOd, double sodProcentDo,
            double solOd, double solDo, double solOdNaTysiąc, double solDoNaTysiąc, double solProcentOd, double solProcentDo)
        {
            Delete(dieta);
            Insert(nazwa, miasto, kod,
            energiaOd, energiaDo, energiaOdNaTysiąc, energiaDoNaTysiąc, energiaProcentOd, energiaProcentDo,
            bialkoOd, bialkoDo, bialkoOdNaTysiąc, bialkoDoNaTysiąc, bialkoProcentOd, bialkoProcentDo,
            tluszczeOd, tluszczeDo, tluszczeOdNaTysiąc, tluszczeDoNaTysiąc, tluszczeProcentOd, tluszczeProcentDo,
            kwasyOd, kwasyDo, kwasyOdNaTysiąc, kwasyDoNaTysiąc, kwasyProcentOd, kwasyProcentDo,
            wegleOd, wegleDo, wegleOdNaTysiąc, wegleDoNaTysiąc, wegleProcentOd, wegleProcentDo,
            przyswajalneOd, przyswajalneDo, przyswajalneOdNaTysiąc, przyswajalneDoNaTysiąc, przyswajalneProcentOd, przyswajalneProcentDo,
            cukryOd, cukryDo, cukryOdNaTysiąc, cukryDoNaTysiąc, cukryProcentOd, cukryProcentDo,
            blonnikOd, blonnikDo, blonnikOdNaTysiąc, blonnikDoNaTysiąc, blonnikProcentOd, blonnikProcentDo,
            sodOd, sodDo, sodOdNaTysiąc, sodDoNaTysiąc, sodProcentOd, sodProcentDo,
            solOd, solDo, solOdNaTysiąc, solDoNaTysiąc, solProcentOd, solProcentDo);
        }

        public static void Delete(Dieta dieta)
        {
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
                {
                    if (DataSet.Diety.Rows[i]["Nazwa diety"].ToString() == dieta.nazwa && DataSet.Diety.Rows[i]["Miasto"].ToString() == dieta.miasto)
                        DataSet.Diety.Rows[i].Delete();
                }
            }
            DataSet.WriteXml(XML_Location);
        }

        public static List<Dieta> SelectAll(string miasto)
        {
            List<Dieta> listaDiet = new List<Dieta>();
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
                {
                    if (DataSet.Diety.Rows[i]["Miasto"].ToString() == miasto)
                    {
                        try
                        {
                            listaDiet.Add(new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), DataSet.Diety.Rows[i]["Kod"].ToString(),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["SodOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["SolOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDoProcent"])));
                        }
                        catch
                        {
                            listaDiet.Add(new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), "", 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
                        }
                    }
                }
            }

            return listaDiet;
        }

        public static Dieta Select(string nazwa, string miasto)
        {
            Dieta dieta = null;
            KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
            String XML_Location = @"DataBase.xml";
            DataSet.ReadXml(XML_Location);
            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
                {
                    if (DataSet.Diety.Rows[i]["Nazwa diety"].ToString() == nazwa && DataSet.Diety.Rows[i]["Miasto"].ToString() == miasto)
                    {
                        try
                        {
                            dieta = new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(), DataSet.Diety.Rows[i]["Kod"].ToString(),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["EnergiaDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BialkoDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["TluszczeDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["KwasyDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["WeglowodanyDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["PrzyswajalneDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["CukryDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["BlonnikDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["SodOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SodDoProcent"]),
                                Convert.ToDouble(DataSet.Diety.Rows[i]["SolOd"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDo"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolOdNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDoNaTysiac"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolOdProcent"]), Convert.ToDouble(DataSet.Diety.Rows[i]["SolDoProcent"]));
                        }
                        catch
                        {
                            dieta = new Dieta(DataSet.Diety.Rows[i]["Nazwa diety"].ToString(), DataSet.Diety.Rows[i]["Miasto"].ToString(),"", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        }
                    }
                }
            }

            return dieta;
        }
    }
}
