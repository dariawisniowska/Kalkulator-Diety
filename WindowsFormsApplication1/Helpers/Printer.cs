namespace KalkulatorDiety
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Xceed.Words.NET;

    public class Printer
    {
        public static string disclaimer = "\r\n• substancje lub produkty powodujące alergie lub reakcje nietolerancji zaznaczono pogrubionym drukiem w odniesieniu do załącznika " +
            "\r\n• możliwe odchylenia +/- 10 % " +
            "\r\n• kuchnia zastrzega sobie prawo do możliwości wymiany potrawy, składnika lub produktu z powodu przyczyn niezależnych od niej - sytuacje losowe";
        
        public static string[] laczoneSniadanie = new string[]{ "Dieta podstawowa" , "Dieta łatwostrawna",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 5 posiłkowa","Dieta bogatobiałkowa" ,"Dieta łatwostrawna z ograniczeniem tłuszczu" , "Dieta bezmleczna",
            "Dieta bezglutenowa", "Dieta wegetariańska"};

        public static string[] laczonyObiad = new string[] { "Dieta podstawowa" , "Dieta łatwostrawna",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 5 posiłkowa","Dieta bogatobiałkowa" ,"Dieta łatwostrawna z ograniczeniem tłuszczu" , "Dieta bezmleczna",
            "Dieta bezglutenowa", "Dieta wegetariańska", "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów", "Dieta łatwostrawna z ograniczeniem łatwo przyswajalnych węglowodanów" };

        public static string[] laczonaKolacja = new string[]{ "Dieta podstawowa" , "Dieta podstawowa dzieci", "Dieta podstawowa dzieci 50%","Dieta podstawowa dzieci 70%",
            "Dieta łatwostrawna dzieci 50%", "Dieta łatwostrawna dzieci 70%", "Dieta łatwostrawna", "Dieta łatwostrawna dzieci" ,
            "Dieta niskobiałkowa" ,"Dieta ubogoenergetyczna 1200kcal" ,
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 3 posiłkowa", "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 5 posiłkowa","Dieta bogatobiałkowa" ,
            "Dieta łatwostrawna z ograniczeniem tłuszczu" , "Dieta bezmleczna", "Dieta bezglutenowa", "Dieta wegetariańska", "Dieta łatwostrawna dla osób starszych", "Dieta łatwostrawna osób starszych",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów", "Dieta łatwostrawna z ograniczeniem łatwo przyswajalnych węglowodanów",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów i nasyconych kwasów tłuszczowych" };

        public static void Receptura(Receptura receptura)
        {
            try
            {
                System.IO.Directory.CreateDirectory("Receptury");
                string path = @"Receptury/" + receptura.nazwa + ".docx";

                using (DocX document = DocX.Create(path))
                {
                    Paragraph p = document.InsertParagraph();
                    p.Alignment = Alignment.center;
                    p.Append("Receptura \r\n\r\n")
                    .Font("Times New Roman")
                    .FontSize(16)
                    .Color(Color.Black)
                    .Bold();

                    Paragraph p2 = document.InsertParagraph();
                    p2.Alignment = Alignment.left;
                    p2.Append("Nazwa: " + receptura.nazwa + "\r\n")
                    .Font("Times New Roman")
                    .FontSize(14)
                    .Color(Color.Black);

                    string[] produkty = receptura.sklad.Split('$');
                    int rows = produkty.Length;
                    int columns = produkty[0].Split('|').Length;
                    double[] suma = new double[columns - 1];
                    string[] naglowki;
                    if (columns == 10)
                        naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                    else
                        naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                    Table t = document.AddTable(rows, columns);
                    t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.Alignment = Alignment.center;
                    t.SetColumnWidth(0, 1400);
                    for (int i = 1; i < columns; i++)
                        t.SetColumnWidth(i, 900);

                    for (int i = 0; i < columns; i++)
                    {
                        t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                            .Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);
                    }

                    for (int r = 0; r < rows - 1; r++)
                    {
                        string[] dane = produkty[r].Split('|');
                        for (int c = 0; c < columns; c++)
                        {
                            if (c == 0)
                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                    .Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            else if (c == columns)
                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                    .Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            else
                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                    .Font("Times New Roman")
                                 .FontSize(12)
                                .Color(Color.Black);
                            if (c >= 2 && c < columns)
                                suma[c - 2] += Convert.ToDouble(dane[c]);
                            if (c == columns)
                                suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                        }

                    }

                    Paragraph p3 = document.InsertParagraph();
                    p3.InsertTableAfterSelf(t);
                    Paragraph p4 = document.InsertParagraph();
                    p4.Alignment = Alignment.left;

                    Table t2 = document.AddTable(2, columns - 2);
                    t2.Alignment = Alignment.center;
                    t2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));

                    for (int i = 0; i < columns - 2; i++)
                    {
                        t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 2])
                            .Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);

                        t2.Rows[1].Cells[i].Paragraphs[0].Append(suma[i].ToString())
                                .Font("Times New Roman")
                                .FontSize(12)
                                .Color(Color.Black);
                    }
                    p4.Append("\r\nWartości odżywcze:\r\n");
                    p4.InsertTableAfterSelf(t2);

                    document.Save();
                    BoldTextInBrackets(document);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można wydrukować dokumentu. \r\n {ex.Message}", "Błąd");
            }
        }

        public static void Produkt(Produkt produkt)
        {
            try
            {
                System.IO.Directory.CreateDirectory("Produkty");
                string path = @"Produkty/" + produkt.nazwa + ".docx";

                using (DocX document = DocX.Create(path))
                {
                    Paragraph p = document.InsertParagraph();
                    p.Alignment = Alignment.center;
                    p.Append("Produkt \r\n\r\n")
                    .Font("Times New Roman")
                    .FontSize(16)
                    .Color(Color.Black)
                    .Bold();

                    Paragraph p2 = document.InsertParagraph();
                    p2.Alignment = Alignment.left;
                    p2.Append("Nazwa: " + produkt.nazwa + "\r\nEnergia [kcal]: " + produkt.wartosciOdzywcze.energia + "\r\nBiałko [g]: " + produkt.wartosciOdzywcze.bialko + "\r\nTłuszcze [g]: " + produkt.wartosciOdzywcze.tluszcze + "\r\nKwasy tłuszczowe nasycone [g]: " + produkt.wartosciOdzywcze.tluszcze_nn + "\r\nWęglowodany ogółem [g]: " + produkt.wartosciOdzywcze.weglowodany + "\r\nWęglowodany przyswajalne [g]: " + produkt.wartosciOdzywcze.weglowodany_przyswajalne + "\r\nCukry [g]: " + produkt.wartosciOdzywcze.cukry + "\r\nBłonnik pokarmowy [g]: " + produkt.wartosciOdzywcze.blonnik + "\r\nSód [mg]: " + produkt.wartosciOdzywcze.sod + "\r\nSól [g]: " + Math.Round(produkt.wartosciOdzywcze.sod * 0.0025, 2))
                    .Font("Times New Roman")
                    .FontSize(12)
                    .Color(Color.Black);

                    var image = document.AddImage("pieczatka.png");
                    var picture = image.CreatePicture(39, 125);
                    Paragraph p5 = document.InsertParagraph();
                    p5.AppendPicture(picture);
                    p5.AppendPicture(picture);

                    document.Save();
                    BoldTextInBrackets(document);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można wydrukować dokumentu. \r\n {ex.Message}", "Błąd");
            }
        }

        public static void Jadlospis(Jadlospis jadlospis, string miasto)
        {
            try
            {
                if (jadlospis != null)
                {
                    DateTime data = Convert.ToDateTime(jadlospis.data);
                    System.IO.Directory.CreateDirectory("Jadłospisy/" + miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day);
                    string path = @"Jadłospisy/" + miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day + "/" + jadlospis.data + ", " + jadlospis.dieta.nazwa + ".docx";
                    if (miasto.Contains("pudełko"))
                    {
                        path = @"Jadłospisy/" + miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day + "/" + jadlospis.data + ", " + jadlospis.dieta.nazwa + " 2000.docx";
                    }
                    using (DocX document = DocX.Create(path))
                    {
                        string opis = opis = $"{jadlospis.data}, {GetDayOfWeek(Convert.ToDateTime(jadlospis.data).DayOfWeek.ToString()).ToLower()}\r\n{jadlospis.dieta.nazwa}\r\n{miasto}";
                        if (jadlospis.miasto.Contains("pudełko"))
                        {
                            opis = $"{jadlospis.data}, {GetDayOfWeek(Convert.ToDateTime(jadlospis.data).DayOfWeek.ToString()).ToLower()}\r\n{jadlospis.dieta.nazwa} 2000\r\n{miasto}";
                        }
                        if (jadlospis.dieta.kod != null && jadlospis.dieta.kod != "" && !miasto.Contains("pudełko"))
                            opis = $"{jadlospis.data}, {GetDayOfWeek(Convert.ToDateTime(jadlospis.data).DayOfWeek.ToString()).ToLower()}\r\n{jadlospis.dieta.nazwa} ({jadlospis.dieta.kod})\r\n{miasto}";

                        Paragraph p0 = document.InsertParagraph();
                        p0.Alignment = Alignment.left;
                        Table t0 = document.AddTable(1, 3);
                        t0.Alignment = Alignment.center;
                        t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                        t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                        t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                        t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                        t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                        t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                        var image = document.AddImage("pieczatka.png");
                        var picture = image.CreatePicture(39, 125);

                        //var image_lesko = document.AddImage("pieczatka2_lesko.jpg");
                        //var picture_lesko = image_lesko.CreatePicture(39, 125);

                        //var image_ustrzyki = document.AddImage("pieczatka2_ustrzyki.png");
                        //var picture_ustrzyki = image_ustrzyki.CreatePicture(55, 175);
                        if (!miasto.Contains("pudełko"))
                        {
                            t0.SetColumnWidth(0, 2000);
                            t0.SetColumnWidth(1, 5000);
                            t0.SetColumnWidth(2, 3500);
                        }
                        else
                        {
                            t0.SetColumnWidth(0, 2750);
                            t0.SetColumnWidth(1, 5000);
                            t0.SetColumnWidth(2, 2750);
                        }
                        if (!miasto.Contains("pudełko"))
                            t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                        t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                        t0.Rows[0].Cells[1].Paragraphs[0].Append(opis)
                            .Font("Times New Roman")
                            .FontSize(12)
                            .Color(Color.Black)
                            .Bold();
                        t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                        if (!miasto.Contains("pudełko"))
                        {
                            if (miasto == "Lesko")
                            {
                                //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_lesko);
                                t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                            }
                            else
                            {
                                //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_ustrzyki);
                                t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                            }
                        }
                        p0.InsertTableAfterSelf(t0);


                        double[] suma_kalorie = new double[5];
                        double[] suma_masa = new double[5];
                        string[] pr = jadlospis.sklad_sniadanie.Split('$');
                        int cl = pr[0].Split('|').Length;
                        double[] sum = new double[cl - 1];

                        //ŚNIADANIE
                        if (jadlospis.sklad_sniadanie != "")
                        {
                            int rows = pr.Length + 1;
                            string[] naglowki;
                            if (cl == 10)
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;
                            Table t = document.AddTable(rows, cl + 1);
                            t.Alignment = Alignment.center;
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetColumnWidth(0, 1400);
                            for (int i = 1; i < cl + 1; i++)
                                t.SetColumnWidth(i, 900);

                            string sniadanie_label = "Śniadanie: ";
                            if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa))
                                sniadanie_label = "Śniadanie/II śniadanie: ";
                            p2.Append($"\r\n{sniadanie_label}{jadlospis.nazwa_sniadanie}")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black);
                            for (int i = 0; i < cl + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                 .Font("Times New Roman")
                                 .FontSize(8)
                                 .Color(Color.Black);
                            }

                            double masa = 0;
                            for (int r = 0; r < rows - 2; r++)
                            {
                                string[] dane = pr[r].Split('|');
                                if (dane[0] != "")
                                {
                                    for (int c = 0; c < cl + 1; c++)
                                    {
                                        if (c == 0)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                                .FontSize(8)
                                                .Color(Color.Black);
                                        else if (c == cl)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                                        if (c == 1)
                                            masa += Convert.ToDouble(dane[c]);
                                        if (c >= 2 && c < cl)
                                            sum[c - 2] += Convert.ToDouble(dane[c]);
                                        if (c == cl)
                                            sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                    }
                                }
                            }
                            suma_kalorie[0] = sum[0];
                            suma_masa[0] = masa;
                            t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                               .Font("Times New Roman")
                                            .FontSize(8)
                                           .Color(Color.Black);
                            for (int i = 0; i < cl - 1; i++)
                                t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(sum[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);

                            //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                            //                  .Font("Times New Roman")
                            //               .FontSize(8)
                            //              .Color(Color.Black);
                            // for (int i = 0; i < cl - 1; i++)
                            //   t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * sum[i] / suma_masa[0],2).ToString())
                            //                 .Font("Times New Roman")
                            //            .FontSize(8)
                            //         .Color(Color.Black);

                            if (sum[6] == 0)
                            {
                                for (int i = 0; i < t.Rows.Count; i++)
                                {
                                    t.Rows[i].Cells.RemoveAt(8);
                                }
                            }
                            p2.InsertTableAfterSelf(t);
                        }

                        //II ŚNIADANIE
                        if (jadlospis.sklad_IIsniadanie != "")
                        {
                            string[] produkty = jadlospis.sklad_IIsniadanie.Split('$');
                            int rows = produkty.Length + 1;
                            int columns = produkty[0].Split('|').Length;

                            string[] naglowki;
                            if (columns == 10)
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            double[] suma2 = new double[columns - 1];

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;
                            Table t = document.AddTable(rows, columns + 1);
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.Alignment = Alignment.center;
                            t.SetColumnWidth(0, 1400);
                            for (int i = 1; i < columns + 1; i++)
                                t.SetColumnWidth(i, 900);

                            p2.Append("\r\nII śniadanie: " + jadlospis.nazwa_IIsniadanie)
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                            for (int i = 0; i < columns + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                            }
                            double masa = 0;
                            for (int r = 0; r < rows - 2; r++)
                            {
                                string[] dane = produkty[r].Split('|');
                                if (dane[0] != "")
                                {
                                    for (int c = 0; c < columns + 1; c++)
                                    {
                                        if (c == 0)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                                .FontSize(8)
                                                .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                                        if (c == 1)
                                            masa += Convert.ToDouble(dane[c]);
                                        if (c >= 2 && c < columns)
                                        {
                                            suma2[c - 2] += Convert.ToDouble(dane[c]);
                                            sum[c - 2] += Convert.ToDouble(dane[c]);
                                        }
                                        if (c == columns)
                                        {
                                            suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                            sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                        }
                                    }
                                }
                            }
                            suma_kalorie[1] = suma2[0];
                            suma_masa[1] = masa;
                            t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                  .Font("Times New Roman")
                                               .FontSize(8)
                                              .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);


                            //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                            //                   .Font("Times New Roman")
                            //               .FontSize(8)
                            //              .Color(Color.Black);
                            //  for (int i = 0; i < columns - 1; i++)
                            //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[1], 2).ToString())
                            //                   .Font("Times New Roman")
                            //                .FontSize(8)
                            //             .Color(Color.Black);
                            if (suma2[6] == 0)
                            {
                                for (int i = 0; i < t.Rows.Count; i++)
                                {
                                    t.Rows[i].Cells.RemoveAt(8);
                                }
                            }
                            p2.InsertTableAfterSelf(t);
                        }

                        //OBIAD
                        if (jadlospis.sklad_obiad != "")
                        {
                            string[] produkty = jadlospis.sklad_obiad.Split('$');
                            int rows = produkty.Length + 1;
                            int columns = produkty[0].Split('|').Length;

                            string[] naglowki;
                            if (columns == 10)
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                            double[] suma2 = new double[columns - 1];

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;
                            Table t = document.AddTable(rows, columns + 1);
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.Alignment = Alignment.center;
                            t.SetColumnWidth(0, 1400);
                            for (int i = 1; i < columns + 1; i++)
                                t.SetColumnWidth(i, 900);

                            string obiad_label = "Obiad: ";
                            if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                obiad_label = "Obiad/Podwieczorek: ";
                            p2.Append($"\r\n{obiad_label}" + jadlospis.nazwa_obiad)
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                            for (int i = 0; i < columns + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                            }

                            double masa = 0;
                            for (int r = 0; r < rows - 2; r++)
                            {
                                string[] dane = produkty[r].Split('|');
                                if (dane[0] != "")
                                {
                                    for (int c = 0; c < columns + 1; c++)
                                    {
                                        if (c == 0)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                                .FontSize(8)
                                                .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                                        if (c == 1)
                                            masa += Convert.ToDouble(dane[c]);
                                        if (c >= 2 && c < columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(dane[c]);
                                            suma2[c - 2] += Convert.ToDouble(dane[c]);
                                        }
                                        if (c == columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                            suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                        }

                                    }
                                }

                            }
                            suma_kalorie[2] = suma2[0];
                            suma_masa[2] = masa;
                            t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                  .Font("Times New Roman")
                                               .FontSize(8)
                                              .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                            // t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                            //.Font("Times New Roman")
                            // .FontSize(8)
                            // .Color(Color.Black);
                            // for (int i = 0; i < columns - 1; i++)
                            //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[2], 2).ToString())
                            //                  .Font("Times New Roman")
                            //             .FontSize(8)
                            //          .Color(Color.Black);
                            if (suma2[6] == 0)
                            {
                                for (int i = 0; i < t.Rows.Count; i++)
                                {
                                    t.Rows[i].Cells.RemoveAt(8);
                                }
                            }
                            p2.InsertTableAfterSelf(t);

                        }

                        //PODWIECZOREAK
                        if (jadlospis.sklad_podwieczorek != "")
                        {
                            string[] produkty = jadlospis.sklad_podwieczorek.Split('$');
                            int rows = produkty.Length + 1;
                            int columns = produkty[0].Split('|').Length;

                            string[] naglowki;
                            if (columns == 10)
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                            double[] suma2 = new double[columns - 1];

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;
                            Table t = document.AddTable(rows, columns + 1);
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.Alignment = Alignment.center;
                            t.SetColumnWidth(0, 1400);
                            for (int i = 1; i < columns + 1; i++)
                                t.SetColumnWidth(i, 900);

                            p2.Append("\r\nPodwieczorek: " + jadlospis.nazwa_podwieczorek)
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                            for (int i = 0; i < columns + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                            }

                            double masa = 0;
                            for (int r = 0; r < rows - 2; r++)
                            {
                                string[] dane = produkty[r].Split('|');
                                if (dane[0] != "")
                                {
                                    for (int c = 0; c < columns + 1; c++)
                                    {
                                        if (c == 0)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                                .FontSize(8)
                                                .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                                        if (c == 1)
                                            masa += Convert.ToDouble(dane[c]);
                                        if (c >= 2 && c < columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(dane[c]);
                                            suma2[c - 2] += Convert.ToDouble(dane[c]);
                                        }
                                        if (c == columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                            suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                        }
                                    }
                                }
                            }
                            suma_kalorie[3] = suma2[0];
                            suma_masa[3] = masa;
                            t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                  .Font("Times New Roman")
                                               .FontSize(8)
                                              .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                            //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                            //.Font("Times New Roman")
                            //.FontSize(8)
                            //.Color(Color.Black);
                            //for (int i = 0; i < columns - 1; i++)
                            //     t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[3], 2).ToString())
                            ///                      .Font("Times New Roman")
                            //                  .FontSize(8)
                            //                  .Color(Color.Black);
                            if (suma2[6] == 0)
                            {
                                for (int i = 0; i < t.Rows.Count; i++)
                                {
                                    t.Rows[i].Cells.RemoveAt(8);
                                }
                            }
                            p2.InsertTableAfterSelf(t);
                        }

                        //KOLACJA
                        if (jadlospis.sklad_kolacja != "")
                        {
                            string[] produkty = jadlospis.sklad_kolacja.Split('$');
                            int rows = produkty.Length + 1;
                            int columns = produkty[0].Split('|').Length;

                            string[] naglowki;
                            if (columns == 10)
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                            double[] suma2 = new double[columns - 1];

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;
                            Table t = document.AddTable(rows, columns + 1);
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.Alignment = Alignment.center;
                            t.SetColumnWidth(0, 1400);
                            for (int i = 1; i < columns + 1; i++)
                                t.SetColumnWidth(i, 900);

                            string kolacja_label = "Kolacja: ";
                            if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                kolacja_label = "Kolacja/Posiłek nocny: ";
                            p2.Append($"\r\n{kolacja_label}" + jadlospis.nazwa_kolacja)
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black);

                            for (int i = 0; i < columns + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                    .Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            }

                            double masa = 0;
                            for (int r = 0; r < rows - 2; r++)
                            {
                                string[] dane = produkty[r].Split('|');
                                if (dane[0] != "")
                                {
                                    for (int c = 0; c < columns + 1; c++)
                                    {
                                        if (c == 0)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                                .FontSize(8)
                                                .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);
                                        if (c == 1)
                                            masa += Convert.ToDouble(dane[c]);
                                        if (c >= 2 && c < columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(dane[c]);
                                            suma2[c - 2] += Convert.ToDouble(dane[c]);
                                        }
                                        if (c == columns)
                                        {
                                            sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                            suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                        }
                                    }
                                }
                            }
                            suma_kalorie[4] = suma2[0];
                            suma_masa[4] = masa;

                            t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                  .Font("Times New Roman")
                                               .FontSize(8)
                                              .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(8)
                                            .Color(Color.Black);

                            //  t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                            //.Font("Times New Roman")
                            // .FontSize(8)
                            // .Color(Color.Black);
                            //for (int i = 0; i < columns - 1; i++)
                            //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[4], 2).ToString())
                            //                    .Font("Times New Roman")
                            //                 .FontSize(8)
                            //                .Color(Color.Black);
                            p2.InsertTableAfterSelf(t);
                        }

                        string[] produkty2 = jadlospis.sklad_sniadanie.Split('$');
                        int columns2 = produkty2[0].Split('|').Length;
                        string[] naglowki2;
                        if (columns2 == 10)
                            naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                        else
                            naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                        Paragraph p3 = document.InsertParagraph();
                        p3.Alignment = Alignment.left;
                        Table t2 = document.AddTable(2, columns2);
                        t2.Alignment = Alignment.center;
                        t2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        for (int i = 0; i < columns2; i++)
                            t2.SetColumnWidth(i, 1000);

                        t2.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                        //t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                        //           .Font("Times New Roman")
                        //            .FontSize(8)
                        //           .Color(Color.Black);

                        for (int i = 1; i < columns2; i++)
                        {
                            t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki2[i + 1])
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);

                            string dodatek = "";
                            if (i == 2)
                            {
                                dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Bialko * 100.0) / sum[0], 2)).ToString();
                                dodatek = "\r\n(" + dodatek + " %)";
                            }
                            if (i == 3)
                            {
                                dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Tluszcze * 100.0) / sum[0], 2)).ToString();
                                dodatek = "\r\n(" + dodatek + " %)";
                            }
                            if (i == 5)
                            {
                                dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Weglowodany * 100.0) / sum[0], 2)).ToString();
                                dodatek = "\r\n(" + dodatek + " %)";
                            }
                            t2.Rows[1].Cells[i].Paragraphs[0].Append(Math.Round(sum[i - 1], 2).ToString() + dodatek)
                                        .Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                            //   t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(sum[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                            //                       .Font("Times New Roman")
                            //                      .FontSize(8)
                            //                      .Color(Color.Black);
                        }
                        p3.Append("\r\nWartości odżywcze:").Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                        p3.InsertTableAfterSelf(t2);



                        Paragraph p4 = document.InsertParagraph();
                        p4.Alignment = Alignment.left;
                        int col;
                        string[] nag;
                        if (suma_kalorie[1] != 0 && suma_kalorie[3] != 0)
                        {
                            col = 5;

                            if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                nag = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                            else
                                nag = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                        }
                        else if (suma_kalorie[1] == 0 && suma_kalorie[3] != 0)
                        {
                            col = 4;
                            if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                nag = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                            else
                                nag = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                        }
                        else
                        {
                            col = 3;
                            if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa) && laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                nag = new string[3] { "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                            else if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                nag = new string[3] { "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                            else if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                nag = new string[3] { "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                            else
                                nag = new string[3] { "Śniadanie", "Obiad", "Kolacja" };
                        }
                        Table t3 = document.AddTable(2, col);
                        t3.Alignment = Alignment.center;
                        t3.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t3.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t3.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t3.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t3.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t3.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));

                        for (int i = 0; i < col; i++)
                        {
                            t3.Rows[0].Cells[i].Paragraphs[0].Append(nag[i])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black);

                            double procent = 0;
                            if (col == 5)
                            {
                                switch (i)
                                {
                                    case 0:
                                        procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                        break;
                                    case 1:
                                        procent = Math.Round(((suma_kalorie[1] * 100.0) / sum[0]), 2);
                                        break;
                                    case 2:
                                        procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                        break;
                                    case 3:
                                        procent = Math.Round(((suma_kalorie[3] * 100.0) / sum[0]), 2);
                                        break;
                                    case 4:
                                        procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                        break;

                                }
                            }
                            if (col == 4)
                            {
                                switch (i)
                                {
                                    case 0:
                                        procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                        break;
                                    case 1:
                                        procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                        break;
                                    case 2:
                                        procent = Math.Round(((suma_kalorie[3] * 100.0) / sum[0]), 2);
                                        break;
                                    case 3:
                                        procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                        break;

                                }
                            }
                            if (col == 3)
                            {
                                switch (i)
                                {
                                    case 0:
                                        procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                        break;
                                    case 1:
                                        procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                        break;
                                    case 2:
                                        procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                        break;

                                }
                            }
                            t3.Rows[1].Cells[i].Paragraphs[0].Append(procent.ToString() + " %")
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                        }
                        p4.InsertTableAfterSelf(t3);

                        document.Save(); 
                        BoldTextInBrackets(document);
                    }
                    
                    if (miasto.Contains("pudełko"))
                    {
                        data = Convert.ToDateTime(jadlospis.data);
                        System.IO.Directory.CreateDirectory("Jadłospisy/" + miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day);
                        path = @"Jadłospisy/" + miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day + "/" + jadlospis.data + ", " + jadlospis.dieta.nazwa + " 1500" + ".docx";

                        using (DocX document = DocX.Create(path))
                        {
                            string opis = opis = $"{jadlospis.data}, {GetDayOfWeek(Convert.ToDateTime(jadlospis.data).DayOfWeek.ToString()).ToLower()}\r\n{jadlospis.dieta.nazwa} 1500\r\n{miasto}";
                            Paragraph p0 = document.InsertParagraph();
                            p0.Alignment = Alignment.left;
                            Table t0 = document.AddTable(1, 3);
                            t0.Alignment = Alignment.center;
                            t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            if (!miasto.Contains("pudełko"))
                            {
                                t0.SetColumnWidth(0, 2000);
                                t0.SetColumnWidth(1, 5000);
                                t0.SetColumnWidth(2, 3500);
                            }
                            else
                            {
                                t0.SetColumnWidth(0, 2750);
                                t0.SetColumnWidth(1, 5000);
                                t0.SetColumnWidth(2, 2750);
                            }
                            t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                            t0.Rows[0].Cells[1].Paragraphs[0].Append(opis)
                                .Font("Times New Roman")
                                .FontSize(12)
                                .Color(Color.Black)
                                .Bold();
                            t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                            p0.InsertTableAfterSelf(t0);

                            double[] suma_kalorie = new double[5];
                            double[] suma_masa = new double[5];
                            string[] pr = jadlospis.sklad_sniadanie.Split('$');
                            int cl = pr[0].Split('|').Length;
                            double[] sum = new double[cl - 1];


                            string sniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_sniadanie, 0.75);
                            string IIsniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_IIsniadanie, 0.75);
                            string obiad7 = Printer.ZamienGramature(jadlospis.nazwa_obiad, 0.75);
                            string podwieczorek7 = Printer.ZamienGramature(jadlospis.nazwa_podwieczorek, 0.75);
                            string kolacja7 = Printer.ZamienGramature(jadlospis.nazwa_kolacja, 0.75);

                            //ŚNIADANIE
                            if (jadlospis.sklad_sniadanie != "")
                            {
                                int rows = pr.Length + 1;
                                string[] naglowki;
                                if (cl == 10)
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                else
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;
                                Table t = document.AddTable(rows, cl + 1);
                                t.Alignment = Alignment.center;
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetColumnWidth(0, 1400);
                                for (int i = 1; i < cl + 1; i++)
                                    t.SetColumnWidth(i, 900);

                                string sniadanie_label = "Śniadanie: ";
                                if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa))
                                    sniadanie_label = "Śniadanie/II śniadanie: ";
                                p2.Append($"\r\n{sniadanie_label}{sniadanie7}")
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);
                                for (int i = 0; i < cl + 1; i++)
                                {
                                    t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                     .Font("Times New Roman")
                                     .FontSize(8)
                                     .Color(Color.Black);
                                }

                                double masa = 0;
                                for (int r = 0; r < rows - 2; r++)
                                {
                                    string[] dane = pr[r].Split('|');
                                    if (dane[0] != "")
                                    {
                                        for (int c = 0; c < cl + 1; c++)
                                        {
                                            if (c == 0)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                    .FontSize(8)
                                                    .Color(Color.Black);
                                            else if (c == cl)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(9)
                                                .Color(Color.Black);
                                            else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                            if (c == 1)
                                                masa += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c >= 2 && c < cl)
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c == cl)
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                        }
                                    }
                                }
                                suma_kalorie[0] = sum[0];
                                suma_masa[0] = masa;
                                t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                   .Font("Times New Roman")
                                                .FontSize(8)
                                               .Color(Color.Black);
                                for (int i = 0; i < cl - 1; i++)
                                    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(sum[i].ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);

                                //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                //                  .Font("Times New Roman")
                                //               .FontSize(8)
                                //              .Color(Color.Black);
                                // for (int i = 0; i < cl - 1; i++)
                                //   t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * sum[i] / suma_masa[0],2).ToString())
                                //                 .Font("Times New Roman")
                                //            .FontSize(8)
                                //         .Color(Color.Black);

                                if (sum[6] == 0)
                                {
                                    for (int i = 0; i < t.Rows.Count; i++)
                                    {
                                        t.Rows[i].Cells.RemoveAt(8);
                                    }
                                }
                                p2.InsertTableAfterSelf(t);
                            }

                            //II ŚNIADANIE
                            if (jadlospis.sklad_IIsniadanie != "")
                            {
                                string[] produkty = jadlospis.sklad_IIsniadanie.Split('$');
                                int rows = produkty.Length + 1;
                                int columns = produkty[0].Split('|').Length;

                                string[] naglowki;
                                if (columns == 10)
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                else
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                double[] suma2 = new double[columns - 1];

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;
                                Table t = document.AddTable(rows, columns + 1);
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.Alignment = Alignment.center;
                                t.SetColumnWidth(0, 1400);
                                for (int i = 1; i < columns + 1; i++)
                                    t.SetColumnWidth(i, 900);

                                p2.Append("\r\nII śniadanie: " + IIsniadanie7)
                                   .Font("Times New Roman")
                                   .FontSize(9)
                                   .Color(Color.Black);

                                for (int i = 0; i < columns + 1; i++)
                                {
                                    t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                        .Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                                }
                                double masa = 0;
                                for (int r = 0; r < rows - 2; r++)
                                {
                                    string[] dane = produkty[r].Split('|');
                                    if (dane[0] != "")
                                    {
                                        for (int c = 0; c < columns + 1; c++)
                                        {
                                            if (c == 0)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                    .FontSize(8)
                                                    .Color(Color.Black);
                                            else if (c == columns)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(9)
                                                .Color(Color.Black);
                                            else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                            if (c == 1)
                                                masa += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c >= 2 && c < columns)
                                            {
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            }
                                            if (c == columns)
                                            {
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                            }
                                        }
                                    }
                                }
                                suma_kalorie[1] = suma2[0];
                                suma_masa[1] = masa;
                                t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                      .Font("Times New Roman")
                                                   .FontSize(8)
                                                  .Color(Color.Black);
                                for (int i = 0; i < columns - 1; i++)
                                    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);


                                //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                //                   .Font("Times New Roman")
                                //               .FontSize(8)
                                //              .Color(Color.Black);
                                //  for (int i = 0; i < columns - 1; i++)
                                //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[1], 2).ToString())
                                //                   .Font("Times New Roman")
                                //                .FontSize(8)
                                //             .Color(Color.Black);
                                if (suma2[6] == 0)
                                {
                                    for (int i = 0; i < t.Rows.Count; i++)
                                    {
                                        t.Rows[i].Cells.RemoveAt(8);
                                    }
                                }
                                p2.InsertTableAfterSelf(t);
                            }

                            //OBIAD
                            if (jadlospis.sklad_obiad != "")
                            {
                                string[] produkty = jadlospis.sklad_obiad.Split('$');
                                int rows = produkty.Length + 1;
                                int columns = produkty[0].Split('|').Length;

                                string[] naglowki;
                                if (columns == 10)
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                else
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                                double[] suma2 = new double[columns - 1];

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;
                                Table t = document.AddTable(rows, columns + 1);
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.Alignment = Alignment.center;
                                t.SetColumnWidth(0, 1400);
                                for (int i = 1; i < columns + 1; i++)
                                    t.SetColumnWidth(i, 900);

                                string obiad_label = "Obiad: ";
                                if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                    obiad_label = "Obiad/Podwieczorek: ";
                                p2.Append($"\r\n{obiad_label}" + obiad7)
                                   .Font("Times New Roman")
                                   .FontSize(9)
                                   .Color(Color.Black);

                                for (int i = 0; i < columns + 1; i++)
                                {
                                    t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                        .Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                                }

                                double masa = 0;
                                for (int r = 0; r < rows - 2; r++)
                                {
                                    string[] dane = produkty[r].Split('|');
                                    if (dane[0] != "")
                                    {
                                        for (int c = 0; c < columns + 1; c++)
                                        {
                                            if (c == 0)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                    .FontSize(8)
                                                    .Color(Color.Black);
                                            else if (c == columns)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(9)
                                                .Color(Color.Black);
                                            else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                            if (c == 1)
                                                masa += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c >= 2 && c < columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            }
                                            if (c == columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                            }

                                        }
                                    }

                                }
                                suma_kalorie[2] = suma2[0];
                                suma_masa[2] = masa;
                                t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                      .Font("Times New Roman")
                                                   .FontSize(8)
                                                  .Color(Color.Black);
                                for (int i = 0; i < columns - 1; i++)
                                    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                // t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                //.Font("Times New Roman")
                                // .FontSize(8)
                                // .Color(Color.Black);
                                // for (int i = 0; i < columns - 1; i++)
                                //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[2], 2).ToString())
                                //                  .Font("Times New Roman")
                                //             .FontSize(8)
                                //          .Color(Color.Black);
                                if (suma2[6] == 0)
                                {
                                    for (int i = 0; i < t.Rows.Count; i++)
                                    {
                                        t.Rows[i].Cells.RemoveAt(8);
                                    }
                                }
                                p2.InsertTableAfterSelf(t);

                            }

                            //PODWIECZOREAK
                            if (jadlospis.sklad_podwieczorek != "")
                            {
                                string[] produkty = jadlospis.sklad_podwieczorek.Split('$');
                                int rows = produkty.Length + 1;
                                int columns = produkty[0].Split('|').Length;

                                string[] naglowki;
                                if (columns == 10)
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                else
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                                double[] suma2 = new double[columns - 1];

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;
                                Table t = document.AddTable(rows, columns + 1);
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.Alignment = Alignment.center;
                                t.SetColumnWidth(0, 1400);
                                for (int i = 1; i < columns + 1; i++)
                                    t.SetColumnWidth(i, 900);

                                p2.Append("\r\nPodwieczorek: " + podwieczorek7)
                                   .Font("Times New Roman")
                                   .FontSize(9)
                                   .Color(Color.Black);

                                for (int i = 0; i < columns + 1; i++)
                                {
                                    t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                        .Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                                }

                                double masa = 0;
                                for (int r = 0; r < rows - 2; r++)
                                {
                                    string[] dane = produkty[r].Split('|');
                                    if (dane[0] != "")
                                    {
                                        for (int c = 0; c < columns + 1; c++)
                                        {
                                            if (c == 0)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                    .FontSize(8)
                                                    .Color(Color.Black);
                                            else if (c == columns)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(9)
                                                .Color(Color.Black);
                                            else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                            if (c == 1)
                                                masa += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c >= 2 && c < columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            }
                                            if (c == columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                            }
                                        }
                                    }
                                }
                                suma_kalorie[3] = suma2[0];
                                suma_masa[3] = masa;
                                t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                      .Font("Times New Roman")
                                                   .FontSize(8)
                                                  .Color(Color.Black);
                                for (int i = 0; i < columns - 1; i++)
                                    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                //t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                //.Font("Times New Roman")
                                //.FontSize(8)
                                //.Color(Color.Black);
                                //for (int i = 0; i < columns - 1; i++)
                                //     t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[3], 2).ToString())
                                ///                      .Font("Times New Roman")
                                //                  .FontSize(8)
                                //                  .Color(Color.Black);
                                if (suma2[6] == 0)
                                {
                                    for (int i = 0; i < t.Rows.Count; i++)
                                    {
                                        t.Rows[i].Cells.RemoveAt(8);
                                    }
                                }
                                p2.InsertTableAfterSelf(t);
                            }

                            //KOLACJA
                            if (jadlospis.sklad_kolacja != "")
                            {
                                string[] produkty = jadlospis.sklad_kolacja.Split('$');
                                int rows = produkty.Length + 1;
                                int columns = produkty[0].Split('|').Length;

                                string[] naglowki;
                                if (columns == 10)
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                                else
                                    naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                                double[] suma2 = new double[columns - 1];

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;
                                Table t = document.AddTable(rows, columns + 1);
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.Alignment = Alignment.center;
                                t.SetColumnWidth(0, 1400);
                                for (int i = 1; i < columns + 1; i++)
                                    t.SetColumnWidth(i, 900);

                                string kolacja_label = "Kolacja: ";
                                if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                    kolacja_label = "Kolacja/Posiłek nocny: ";
                                p2.Append($"\r\n{kolacja_label}" + kolacja7)
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                                for (int i = 0; i < columns + 1; i++)
                                {
                                    t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                        .Font("Times New Roman")
                                        .FontSize(9)
                                        .Color(Color.Black);
                                }

                                double masa = 0;
                                for (int r = 0; r < rows - 2; r++)
                                {
                                    string[] dane = produkty[r].Split('|');
                                    if (dane[0] != "")
                                    {

                                        for (int c = 0; c < columns + 1; c++)
                                        {
                                            if (c == 0)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                    .FontSize(8)
                                                    .Color(Color.Black);
                                            else if (c == columns)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(9)
                                                .Color(Color.Black);
                                            else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);
                                            if (c == 1)
                                                masa += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            if (c >= 2 && c < columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c]) * 0.75, 2).ToString());
                                            }
                                            if (c == columns)
                                            {
                                                sum[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                                suma2[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.75 * 0.0025, 2));
                                            }
                                        }
                                    }
                                }
                                suma_kalorie[4] = suma2[0];
                                suma_masa[4] = masa;

                                t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("Suma")
                                                      .Font("Times New Roman")
                                                   .FontSize(8)
                                                  .Color(Color.Black);
                                for (int i = 0; i < columns - 1; i++)
                                    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                    .Font("Times New Roman")
                                                 .FontSize(8)
                                                .Color(Color.Black);

                                //  t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                //.Font("Times New Roman")
                                // .FontSize(8)
                                // .Color(Color.Black);
                                //for (int i = 0; i < columns - 1; i++)
                                //    t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[4], 2).ToString())
                                //                    .Font("Times New Roman")
                                //                 .FontSize(8)
                                //                .Color(Color.Black);
                                p2.InsertTableAfterSelf(t);
                            }

                            string[] produkty2 = jadlospis.sklad_sniadanie.Split('$');
                            int columns2 = produkty2[0].Split('|').Length;
                            string[] naglowki2;
                            if (columns2 == 10)
                                naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                            else
                                naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                            Paragraph p3 = document.InsertParagraph();
                            p3.Alignment = Alignment.left;
                            Table t2 = document.AddTable(2, columns2);
                            t2.Alignment = Alignment.center;
                            t2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            for (int i = 0; i < columns2; i++)
                                t2.SetColumnWidth(i, 1000);

                            t2.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                                        .Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                            //t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                            //           .Font("Times New Roman")
                            //            .FontSize(8)
                            //           .Color(Color.Black);

                            for (int i = 1; i < columns2; i++)
                            {
                                t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki2[i + 1])
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);

                                string dodatek = "";
                                if (i == 2)
                                {
                                    dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Bialko * 100.0) / sum[0], 2)).ToString();
                                    dodatek = "\r\n(" + dodatek + " %)";
                                }
                                if (i == 3)
                                {
                                    dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Tluszcze * 100.0) / sum[0], 2)).ToString();
                                    dodatek = "\r\n(" + dodatek + " %)";
                                }
                                if (i == 5)
                                {
                                    dodatek = (Math.Round((sum[i - 1] * Form1.przelicznik_Weglowodany * 100.0) / sum[0], 2)).ToString();
                                    dodatek = "\r\n(" + dodatek + " %)";
                                }
                                t2.Rows[1].Cells[i].Paragraphs[0].Append(Math.Round(sum[i - 1], 2).ToString() + dodatek)
                                            .Font("Times New Roman")
                                            .FontSize(8)
                                            .Color(Color.Black);
                                //   t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(sum[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                                //                       .Font("Times New Roman")
                                //                      .FontSize(8)
                                //                      .Color(Color.Black);
                            }
                            p3.Append("\r\nWartości odżywcze:").Font("Times New Roman")
                                        .FontSize(8)
                                        .Color(Color.Black);
                            p3.InsertTableAfterSelf(t2);



                            Paragraph p4 = document.InsertParagraph();
                            p4.Alignment = Alignment.left;
                            int col;
                            string[] nag;
                            if (suma_kalorie[1] != 0 && suma_kalorie[3] != 0)
                            {
                                col = 5;

                                if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                    nag = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                else
                                    nag = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            }
                            else if (suma_kalorie[1] == 0 && suma_kalorie[3] != 0)
                            {
                                col = 4;
                                if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                    nag = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                else
                                    nag = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            }
                            else
                            {
                                col = 3;
                                if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa) && laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                    nag = new string[3] { "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                else if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                    nag = new string[3] { "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                else if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                    nag = new string[3] { "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                                else
                                    nag = new string[3] { "Śniadanie", "Obiad", "Kolacja" };
                            }
                            Table t3 = document.AddTable(2, col);
                            t3.Alignment = Alignment.center;
                            t3.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t3.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t3.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t3.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t3.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t3.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));

                            for (int i = 0; i < col; i++)
                            {
                                t3.Rows[0].Cells[i].Paragraphs[0].Append(nag[i])
                                    .Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);

                                double procent = 0;
                                if (col == 5)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                            break;
                                        case 1:
                                            procent = Math.Round(((suma_kalorie[1] * 100.0) / sum[0]), 2);
                                            break;
                                        case 2:
                                            procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                            break;
                                        case 3:
                                            procent = Math.Round(((suma_kalorie[3] * 100.0) / sum[0]), 2);
                                            break;
                                        case 4:
                                            procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                            break;

                                    }
                                }
                                if (col == 4)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                            break;
                                        case 1:
                                            procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                            break;
                                        case 2:
                                            procent = Math.Round(((suma_kalorie[3] * 100.0) / sum[0]), 2);
                                            break;
                                        case 3:
                                            procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                            break;

                                    }
                                }
                                if (col == 3)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            procent = Math.Round(((suma_kalorie[0] * 100.0) / sum[0]), 2);
                                            break;
                                        case 1:
                                            procent = Math.Round(((suma_kalorie[2] * 100.0) / sum[0]), 2);
                                            break;
                                        case 2:
                                            procent = Math.Round(((suma_kalorie[4] * 100.0) / sum[0]), 2);
                                            break;

                                    }
                                }
                                t3.Rows[1].Cells[i].Paragraphs[0].Append(procent.ToString() + " %")
                                   .Font("Times New Roman")
                                   .FontSize(9)
                                   .Color(Color.Black);

                            }
                            p4.InsertTableAfterSelf(t3);

                            document.Save();
                            BoldTextInBrackets(document);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Brak jadłospisu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można wydrukować dokumentu. \r\n {ex.Message}", "Błąd");
            }
        }

        public static void JadlospisDzienny(List<Jadlospis> listaJadlospisow)
        {
            try
            {
                if (listaJadlospisow.Count > 0)
                {
                    if (listaJadlospisow[0].miasto == "Szpital")
                    {
                        JadlospisDzienny(listaJadlospisow, "Lesko");
                        JadlospisDzienny(listaJadlospisow, "Ustrzyki Dolne");
                    }
                    else
                    {
                        JadlospisDzienny(listaJadlospisow, listaJadlospisow[0].miasto);
                    }
                }
                else
                {
                    MessageBox.Show("Brak jadłospisów we wskazanym dniu", "Błąd");
                }
            }
            catch
            {
                MessageBox.Show("Nie można wydrukować dokumentu", "Błąd");
            }
        }
        public static void JadlospisDzienny(List<Jadlospis> listaJadlospisow, string miasto)
        {
            System.IO.Directory.CreateDirectory("Jadłospisy dzienne/" + miasto);
            string path = @"Jadłospisy dzienne/" + miasto + "/" + listaJadlospisow[0].data + ".docx";

            using (DocX document = DocX.Create(path))
            {
                Paragraph p0 = document.InsertParagraph();
                p0.Alignment = Alignment.left;
                Table t0 = document.AddTable(1, 3);
                t0.Alignment = Alignment.center;
                t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                var image = document.AddImage("pieczatka.png");
                var picture = image.CreatePicture(39, 125);

                //var image_lesko = document.AddImage("pieczatka2_lesko.jpg");
                //var picture_lesko = image_lesko.CreatePicture(39, 125);

                //var image_ustrzyki = document.AddImage("pieczatka2_ustrzyki.png");
                //var picture_ustrzyki = image_ustrzyki.CreatePicture(55, 175);

                t0.SetColumnWidth(0, 2000);
                t0.SetColumnWidth(1, 5000);
                t0.SetColumnWidth(2, 3500);
                if (!miasto.Contains("pudełko"))
                {
                    t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                }
                else
                {
                    t0.SetColumnWidth(0, 2750);
                    t0.SetColumnWidth(1, 5000);
                    t0.SetColumnWidth(2, 2750);
                }
                t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                t0.Rows[0].Cells[1].Paragraphs[0].Append($"JADŁOSPIS\r\n{listaJadlospisow[0].data}\r\n{GetDayOfWeek(Convert.ToDateTime(listaJadlospisow[0].data).DayOfWeek.ToString()).ToLower()}\r\n{miasto}")
                    .Font("Times New Roman")
                    .FontSize(12)
                    .Color(Color.Black)
                    .Bold();

                t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                if (!miasto.Contains("pudełko"))
                {
                    if (miasto == "Lesko")
                    {
                        //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_lesko);
                        t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    }
                    else
                    {
                        //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_ustrzyki);
                        t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    }
                }
                p0.InsertTableAfterSelf(t0);

                foreach (Jadlospis jadlospis in listaJadlospisow)
                {
                    Paragraph p2 = document.InsertParagraph();
                    p2.Alignment = Alignment.left;

                    string opis = $"\r\n{jadlospis.dieta.nazwa}";
                    if (jadlospis.dieta.kod != null && jadlospis.dieta.kod != "" && !miasto.Contains("pudełko"))
                        opis = $"\r\n{jadlospis.dieta.nazwa} ({jadlospis.dieta.kod})";


                    if (miasto.Contains("pudełko"))
                    {
                        p2.Append(opis + " 2000")
                            .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();
                    }
                    else
                    {
                        p2.Append(opis)
                            .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();

                    }

                    int rows = 2;
                    int columns = 3;
                    if (jadlospis.sklad_IIsniadanie != "" && jadlospis.sklad_podwieczorek != "")
                        columns = 5;
                    if (jadlospis.sklad_IIsniadanie == "" && jadlospis.sklad_podwieczorek != "")
                        columns = 4;
                    string[] naglowki = null;

                    if (columns == 3)
                    {
                        if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa) && laczonyObiad.Contains(jadlospis.dieta.nazwa))
                            naglowki = new string[3] { "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                        else if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                            naglowki = new string[3] { "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                        else if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                            naglowki = new string[3] { "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                        else
                            naglowki = new string[3] { "Śniadanie", "Obiad", "Kolacja" };
                    }
                    if (columns == 4)
                    {
                        if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                            naglowki = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                        else
                            naglowki = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                    }
                    if (columns == 5)
                    {
                        if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                            naglowki = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                        else
                            naglowki = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                    }


                    Table t = document.AddTable(rows, columns);
                    t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t.Alignment = Alignment.center;
                    for (int i = 0; i < columns; i++)
                    {
                        if (columns == 5)
                            t.SetColumnWidth(i, 2100);
                        else if (columns == 4)
                            t.SetColumnWidth(i, 2600);
                        else
                            t.SetColumnWidth(i, 3500);
                    }

                    for (int i = 0; i < columns; i++)
                    {
                        t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                            .Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black).Bold();
                    }
                    if (naglowki.Length == 3)
                    {
                        t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                    }
                    else if (naglowki.Length == 4)
                    {
                        t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                    }
                    else
                    {
                        t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_IIsniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t.Rows[1].Cells[4].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                    }
                    p2.InsertTableAfterSelf(t);

                    if (jadlospis.dieta.nazwa.Contains("dzieci"))
                    {
                        Paragraph p3 = document.InsertParagraph();
                        p3.Alignment = Alignment.left;
                        p3.Append(opis + " 70%")
                        .Font("Times New Roman")
                       .FontSize(9)
                       .Color(Color.Black)
                       .Bold();
                        string sniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_sniadanie, 0.7);
                        string IIsniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_IIsniadanie, 0.7);
                        string obiad7 = Printer.ZamienGramature(jadlospis.nazwa_obiad, 0.7);
                        string podwieczorek7 = Printer.ZamienGramature(jadlospis.nazwa_podwieczorek, 0.7);
                        string kolacja7 = Printer.ZamienGramature(jadlospis.nazwa_kolacja, 0.7);

                        Table t7 = document.AddTable(rows, columns);
                        t7.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.Alignment = Alignment.center;
                        for (int i = 0; i < columns; i++)
                        {
                            if (columns == 5)
                                t7.SetColumnWidth(i, 2100);
                            else if (columns == 4)
                                t7.SetColumnWidth(i, 2600);
                            else
                                t7.SetColumnWidth(i, 3500);
                        }

                        for (int i = 0; i < columns; i++)
                        {
                            t7.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black).Bold();
                        }
                        if (naglowki.Length == 3)
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else if (naglowki.Length == 4)
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(podwieczorek7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[3].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(IIsniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[3].Paragraphs[0].Append(podwieczorek7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[4].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        p3.InsertTableAfterSelf(t7);

                        Paragraph p4 = document.InsertParagraph();
                        p4.Alignment = Alignment.left;
                        p4.Append(opis + " 50%")
                        .Font("Times New Roman")
                       .FontSize(9)
                       .Color(Color.Black)
                       .Bold();
                        string sniadanie5 = Printer.ZamienGramature(jadlospis.nazwa_sniadanie, 0.5);
                        string IIsniadanie5 = Printer.ZamienGramature(jadlospis.nazwa_IIsniadanie, 0.5);
                        string obiad5 = Printer.ZamienGramature(jadlospis.nazwa_obiad, 0.5);
                        string podwieczorek5 = Printer.ZamienGramature(jadlospis.nazwa_podwieczorek, 0.5);
                        string kolacja5 = Printer.ZamienGramature(jadlospis.nazwa_kolacja, 0.5);
                        Table t5 = document.AddTable(rows, columns);
                        t5.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t5.Alignment = Alignment.center;
                        for (int i = 0; i < columns; i++)
                        {
                            if (columns == 5)
                                t5.SetColumnWidth(i, 2100);
                            else if (columns == 4)
                                t5.SetColumnWidth(i, 2600);
                            else
                                t5.SetColumnWidth(i, 3500);
                        }

                        for (int i = 0; i < columns; i++)
                        {
                            t5.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black).Bold();
                        }
                        if (naglowki.Length == 3)
                        {
                            t5.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[1].Paragraphs[0].Append(obiad5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[2].Paragraphs[0].Append(kolacja5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else if (naglowki.Length == 4)
                        {
                            t5.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[1].Paragraphs[0].Append(obiad5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[2].Paragraphs[0].Append(podwieczorek5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[3].Paragraphs[0].Append(kolacja5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else
                        {
                            t5.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[1].Paragraphs[0].Append(IIsniadanie5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[2].Paragraphs[0].Append(obiad5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[3].Paragraphs[0].Append(podwieczorek5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t5.Rows[1].Cells[4].Paragraphs[0].Append(kolacja5).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        p4.InsertTableAfterSelf(t5);
                    }
                    if (jadlospis.miasto.Contains("pudełko"))
                    {
                        Paragraph p3 = document.InsertParagraph();
                        p3.Alignment = Alignment.left;
                        p3.Append(opis + " 1500")
                        .Font("Times New Roman")
                       .FontSize(9)
                       .Color(Color.Black)
                       .Bold();
                        string sniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_sniadanie, 0.75);
                        string IIsniadanie7 = Printer.ZamienGramature(jadlospis.nazwa_IIsniadanie, 0.75);
                        string obiad7 = Printer.ZamienGramature(jadlospis.nazwa_obiad, 0.75);
                        string podwieczorek7 = Printer.ZamienGramature(jadlospis.nazwa_podwieczorek, 0.75);
                        string kolacja7 = Printer.ZamienGramature(jadlospis.nazwa_kolacja, 0.75);

                        Table t7 = document.AddTable(rows, columns);
                        t7.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t7.Alignment = Alignment.center;
                        for (int i = 0; i < columns; i++)
                        {
                            if (columns == 5)
                                t7.SetColumnWidth(i, 2100);
                            else if (columns == 4)
                                t7.SetColumnWidth(i, 2600);
                            else
                                t7.SetColumnWidth(i, 3500);
                        }

                        for (int i = 0; i < columns; i++)
                        {
                            t7.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black).Bold();
                        }
                        if (naglowki.Length == 3)
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else if (naglowki.Length == 4)
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(podwieczorek7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[3].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else
                        {
                            t7.Rows[1].Cells[0].Paragraphs[0].Append(sniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[1].Paragraphs[0].Append(IIsniadanie7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[2].Paragraphs[0].Append(obiad7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[3].Paragraphs[0].Append(podwieczorek7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t7.Rows[1].Cells[4].Paragraphs[0].Append(kolacja7).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        p3.InsertTableAfterSelf(t7);
                    }
                }
                Paragraph pWartosci = document.InsertParagraph();
                pWartosci.Alignment = Alignment.left;
                pWartosci.Append("\r\n" + "Wartości odżywcze: ")
                   .Font("Times New Roman")
                   .FontSize(12)
                   .Color(Color.Black)
                   .Bold();

                foreach (Jadlospis jadlospis in listaJadlospisow)
                {
                    Paragraph p2 = document.InsertParagraph();
                    p2.Alignment = Alignment.left;
                    if (miasto.Contains("pudełko"))
                    {
                        p2.Append("\r\n" + jadlospis.dieta.nazwa + " 2000")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();
                    }
                    else
                    {
                        p2.Append("\r\n" + jadlospis.dieta.nazwa)
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black)
                               .Bold();
                    }

                    string[] naglowki;
                    string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                    int columns2 = produkty[0].Split('|').Length;

                    if (columns2 == 10)
                        naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                    else
                        naglowki = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                    double[] suma = new double[columns2 - 1];
                    double[] suma_masa = new double[5];

                    foreach (string sklad in jadlospis.sklad_sniadanie.Split('$'))
                    {
                        if (sklad != "")
                        {
                            string[] dane = sklad.Split('|');
                            for (int c = 0; c < columns2 + 1; c++)
                            {
                                if (c == 1)
                                    suma_masa[0] += Convert.ToDouble(dane[c]);
                                if (c >= 2 && c < columns2)
                                    suma[c - 2] += Convert.ToDouble(dane[c]);
                                if (c == columns2)
                                    suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                            }
                        }
                    }
                    foreach (string sklad in jadlospis.sklad_IIsniadanie.Split('$'))
                    {
                        if (sklad != "")
                        {
                            string[] dane = sklad.Split('|');
                            for (int c = 0; c < columns2 + 1; c++)
                            {
                                if (c == 1)
                                    suma_masa[1] += Convert.ToDouble(dane[c]);
                                if (c >= 2 && c < columns2)
                                    suma[c - 2] += Convert.ToDouble(dane[c]);
                                if (c == columns2)
                                    suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                            }
                        }
                    }
                    foreach (string sklad in jadlospis.sklad_obiad.Split('$'))
                    {
                        if (sklad != "")
                        {
                            string[] dane = sklad.Split('|');
                            for (int c = 0; c < columns2 + 1; c++)
                            {
                                if (c == 1)
                                    suma_masa[2] += Convert.ToDouble(dane[c]);
                                if (c >= 2 && c < columns2)
                                    suma[c - 2] += Convert.ToDouble(dane[c]);
                                if (c == columns2)
                                    suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                            }
                        }
                    }
                    foreach (string sklad in jadlospis.sklad_podwieczorek.Split('$'))
                    {
                        if (sklad != "")
                        {
                            string[] dane = sklad.Split('|');
                            for (int c = 0; c < columns2 + 1; c++)
                            {
                                if (c == 1)
                                    suma_masa[3] += Convert.ToDouble(dane[c]);
                                if (c >= 2 && c < columns2)
                                    suma[c - 2] += Convert.ToDouble(dane[c]);
                                if (c == columns2)
                                    suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                            }
                        }
                    }
                    foreach (string sklad in jadlospis.sklad_kolacja.Split('$'))
                    {
                        if (sklad != "")
                        {
                            string[] dane = sklad.Split('|');
                            for (int c = 0; c < columns2 + 1; c++)
                            {
                                if (c == 1)
                                    suma_masa[4] += Convert.ToDouble(dane[c]);
                                if (c >= 2 && c < columns2)
                                    suma[c - 2] += Convert.ToDouble(dane[c]);
                                if (c == columns2)
                                    suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                            }
                        }
                    }

                    Table t2 = document.AddTable(2, columns2);
                    t2.Alignment = Alignment.center;
                    t2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    t2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                    for (int i = 0; i < columns2; i++)
                    {
                        t2.SetColumnWidth(i, 1000);
                    }

                    t2.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                        .Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                    //t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                    //     .Font("Times New Roman")
                    //     .FontSize(8)
                    //     .Color(Color.Black);
                    for (int i = 1; i < columns2; i++)
                    {
                        t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 1])
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        t2.Rows[1].Cells[i].Paragraphs[0].Append(suma[i - 1].ToString())
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        // t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                        //        .Font("Times New Roman")
                        //         .FontSize(8)
                        //         .Color(Color.Black);
                    }
                    p2.InsertTableAfterSelf(t2);



                    if (jadlospis.dieta.nazwa.Contains("dzieci"))
                    {
                        Paragraph p2_d1 = document.InsertParagraph();
                        p2_d1.Alignment = Alignment.left;
                        p2_d1.Append("\r\n" + jadlospis.dieta.nazwa + " 70%")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();

                        Table t2_d1 = document.AddTable(2, columns2);
                        t2_d1.Alignment = Alignment.center;
                        t2_d1.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        for (int i = 0; i < columns2; i++)
                        {
                            t2_d1.SetColumnWidth(i, 1000);
                        }

                        t2_d1.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        for (int i = 1; i < columns2; i++)
                        {
                            t2_d1.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 1])
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t2_d1.Rows[1].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 0.7, 2).ToString())
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                        }
                        p2_d1.InsertTableAfterSelf(t2_d1);


                        Paragraph p2_d2 = document.InsertParagraph();
                        p2_d2.Alignment = Alignment.left;
                        p2_d2.Append("\r\n" + jadlospis.dieta.nazwa + " 50%")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();


                        Table t2_d2 = document.AddTable(2, columns2);
                        t2_d2.Alignment = Alignment.center;
                        t2_d2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        for (int i = 0; i < columns2; i++)
                        {
                            t2_d2.SetColumnWidth(i, 1000);
                        }

                        t2_d2.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        for (int i = 1; i < columns2; i++)
                        {
                            t2_d2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 1])
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t2_d2.Rows[1].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 0.5, 2).ToString())
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                        }
                        p2_d2.InsertTableAfterSelf(t2_d2);
                    }
                    if (miasto.Contains("pudełko"))
                    {
                        Paragraph p2_d1 = document.InsertParagraph();
                        p2_d1.Alignment = Alignment.left;
                        p2_d1.Append("\r\n" + jadlospis.dieta.nazwa + " 1500")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();

                        Table t2_d1 = document.AddTable(2, columns2);
                        t2_d1.Alignment = Alignment.center;
                        t2_d1.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2_d1.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        for (int i = 0; i < columns2; i++)
                        {
                            t2_d1.SetColumnWidth(i, 1000);
                        }

                        t2_d1.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        for (int i = 1; i < columns2; i++)
                        {
                            t2_d1.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 1])
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t2_d1.Rows[1].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 0.75, 2).ToString())
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                        }
                        p2_d1.InsertTableAfterSelf(t2_d1);
                    }
                }

                Paragraph p5 = document.InsertParagraph();
                p5.Alignment = Alignment.left;
                p5.Append(disclaimer)
                   .Font("Times New Roman")
                   .FontSize(8)
                   .Color(Color.Black);

                document.Save();

                BoldTextInBrackets(document);
            }
        }

        public static void JadlospisNaStrone(List<Jadlospis> listaJadlospisow)
        {
            try
            {
                if (listaJadlospisow.Count > 0)
                {
                    if (listaJadlospisow[0].miasto == "Szpital")
                    {
                        JadlospisNaStrone(listaJadlospisow, "Lesko");
                        JadlospisNaStrone(listaJadlospisow, "Ustrzyki Dolne");
                    }
                    else
                    {
                        JadlospisNaStrone(listaJadlospisow, listaJadlospisow[0].miasto);
                    }
                }
                else
                {
                    MessageBox.Show("Brak jadłospisów we wskazanym dniu", "Błąd");
                }
            }
            catch
            {
                MessageBox.Show("Nie można wydrukować dokumentu", "Błąd");
            }
        }
        public static void JadlospisNaStrone(List<Jadlospis> listaJadlospisow, string miasto)
        {
            System.IO.Directory.CreateDirectory("Jadłospisy na stronę/" + miasto);
            string path = @"Jadłospisy na stronę/" + miasto + "/" + listaJadlospisow[0].data + ".docx";

            using (DocX document = DocX.Create(path))
            {
                Paragraph p0 = document.InsertParagraph();
                p0.Alignment = Alignment.left;
                Table t0 = document.AddTable(1, 3);
                t0.Alignment = Alignment.center;
                t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                var image = document.AddImage("pieczatka.png");
                var picture = image.CreatePicture(39, 125);
                t0.SetColumnWidth(0, 2000);
                t0.SetColumnWidth(1, 5000);
                t0.SetColumnWidth(2, 2000);
                //t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                t0.Rows[0].Cells[1].Paragraphs[0].Append($"JADŁOSPIS\r\n{listaJadlospisow[0].data}\r\n{GetDayOfWeek(Convert.ToDateTime(listaJadlospisow[0].data).DayOfWeek.ToString()).ToLower()}\r\n{miasto}")
                    .Font("Times New Roman")
                    .FontSize(12)
                    .Color(Color.Black)
                    .Bold();
                t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;

                p0.InsertTableAfterSelf(t0);

                foreach (Jadlospis jadlospis in listaJadlospisow)
                {
                    if ((miasto == "Ustrzyki Dolne" && (jadlospis.dieta.nazwa == "Dieta podstawowa" || jadlospis.dieta.nazwa == "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 3 posiłkowa")) ||
                        (miasto == "Lesko" && (jadlospis.dieta.nazwa == "Dieta podstawowa" || jadlospis.dieta.nazwa == "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów")))
                    {
                        Paragraph p1 = document.InsertParagraph();
                        p1.Alignment = Alignment.left;

                        string opis = $"\r\n{jadlospis.dieta.nazwa}";
                        if (jadlospis.dieta.kod != null && jadlospis.dieta.kod != "" && !miasto.Contains("pudełko"))
                            opis = $"\r\n{jadlospis.dieta.nazwa} ({jadlospis.dieta.kod})";

                        p1.Append(opis)
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black)
                           .Bold();

                        int rows = 2;
                        int columns = 3;
                        if (jadlospis.sklad_IIsniadanie != "" && jadlospis.sklad_podwieczorek != "")
                            columns = 5;
                        if (jadlospis.sklad_IIsniadanie == "" && jadlospis.sklad_podwieczorek != "")
                            columns = 4;
                        string[] naglowki = null;

                        if (columns == 3)
                        {
                            if (laczoneSniadanie.Contains(jadlospis.dieta.nazwa) && laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                naglowki = new string[3] { "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                            else if (laczonyObiad.Contains(jadlospis.dieta.nazwa))
                                naglowki = new string[3] { "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                            else if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                naglowki = new string[3] { "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                            else
                                naglowki = new string[3] { "Śniadanie", "Obiad", "Kolacja" };
                        }
                        if (columns == 4)
                        {
                            if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                naglowki = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                            else
                                naglowki = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                        }
                        if (columns == 5)
                        {
                            if (laczonaKolacja.Contains(jadlospis.dieta.nazwa))
                                naglowki = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                            else
                                naglowki = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                        }



                        Table t = document.AddTable(rows, columns);
                        t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t.Alignment = Alignment.center;
                        for (int i = 0; i < columns; i++)
                        {
                            if (columns == 5)
                                t.SetColumnWidth(i, 2100);
                            else if (columns == 4)
                                t.SetColumnWidth(i, 2600);
                            else
                                t.SetColumnWidth(i, 3500);
                        }

                        for (int i = 0; i < columns; i++)
                        {
                            t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black).Bold();
                        }
                        if (naglowki.Length == 3)
                        {
                            t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else if (naglowki.Length == 4)
                        {
                            t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        else
                        {
                            t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_IIsniadanie).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t.Rows[1].Cells[4].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                        }
                        p1.InsertTableAfterSelf(t);


                        string[] naglowki2;
                        string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                        int columns2 = produkty[0].Split('|').Length;

                        if (columns2 == 10)
                            naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };
                        else
                            naglowki2 = new string[] { "Nazwa produktu", "Masa [g]", "Energia [kcal]", "Białko [g]", "Tłuszcze [g]", "Kwasy tłuszczowe nasycone [g]", "Węglowodany ogółem [g]", "Węglowodany przyswajalne [g]", "Cukry [g]", "Błonnik pokarmowy [g]", "Sód [mg]", "Sól [g]" };

                        double[] suma = new double[columns2 - 1];
                        double[] suma_masa = new double[5];

                        foreach (string sklad in jadlospis.sklad_sniadanie.Split('$'))
                        {
                            if (sklad != "")
                            {
                                string[] dane = sklad.Split('|');
                                for (int c = 0; c < columns2 + 1; c++)
                                {
                                    if (c == 1)
                                        suma_masa[0] += Convert.ToDouble(dane[c]);
                                    if (c >= 2 && c < columns2)
                                        suma[c - 2] += Convert.ToDouble(dane[c]);
                                    if (c == columns2)
                                        suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                }
                            }
                        }
                        foreach (string sklad in jadlospis.sklad_IIsniadanie.Split('$'))
                        {
                            if (sklad != "")
                            {
                                string[] dane = sklad.Split('|');
                                for (int c = 0; c < columns2 + 1; c++)
                                {
                                    if (c == 1)
                                        suma_masa[1] += Convert.ToDouble(dane[c]);
                                    if (c >= 2 && c < columns2)
                                        suma[c - 2] += Convert.ToDouble(dane[c]);
                                    if (c == columns2)
                                        suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                }
                            }
                        }
                        foreach (string sklad in jadlospis.sklad_obiad.Split('$'))
                        {
                            if (sklad != "")
                            {
                                string[] dane = sklad.Split('|');
                                for (int c = 0; c < columns2 + 1; c++)
                                {
                                    if (c == 1)
                                        suma_masa[2] += Convert.ToDouble(dane[c]);
                                    if (c >= 2 && c < columns2)
                                        suma[c - 2] += Convert.ToDouble(dane[c]);
                                    if (c == columns2)
                                        suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                }
                            }
                        }
                        foreach (string sklad in jadlospis.sklad_podwieczorek.Split('$'))
                        {
                            if (sklad != "")
                            {
                                string[] dane = sklad.Split('|');
                                for (int c = 0; c < columns2 + 1; c++)
                                {
                                    if (c == 1)
                                        suma_masa[3] += Convert.ToDouble(dane[c]);
                                    if (c >= 2 && c < columns2)
                                        suma[c - 2] += Convert.ToDouble(dane[c]);
                                    if (c == columns2)
                                        suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                }
                            }
                        }
                        foreach (string sklad in jadlospis.sklad_kolacja.Split('$'))
                        {
                            if (sklad != "")
                            {
                                string[] dane = sklad.Split('|');
                                for (int c = 0; c < columns2 + 1; c++)
                                {
                                    if (c == 1)
                                        suma_masa[4] += Convert.ToDouble(dane[c]);
                                    if (c >= 2 && c < columns2)
                                        suma[c - 2] += Convert.ToDouble(dane[c]);
                                    if (c == columns2)
                                        suma[c - 2] += Convert.ToDouble(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2));
                                }
                            }
                        }

                        Table t2 = document.AddTable(2, columns2);
                        t2.Alignment = Alignment.center;
                        t2.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        t2.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                        for (int i = 0; i < columns2; i++)
                        {
                            t2.SetColumnWidth(i, 1000);
                        }

                        t2.Rows[1].Cells[0].Paragraphs[0].Append("Suma")
                            .Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                        //t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                        //     .Font("Times New Roman")
                        //     .FontSize(8)
                        //     .Color(Color.Black);
                        for (int i = 1; i < columns2; i++)
                        {
                            t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki2[i + 1])
                                .Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                            t2.Rows[1].Cells[i].Paragraphs[0].Append(suma[i - 1].ToString())
                                    .Font("Times New Roman")
                                    .FontSize(8)
                                    .Color(Color.Black);
                            // t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                            //        .Font("Times New Roman")
                            //        .FontSize(8)
                            //        .Color(Color.Black);
                        }
                        Paragraph p2 = document.InsertParagraph();
                        p2.Alignment = Alignment.left;
                        p2.InsertTableAfterSelf(t2);
                    }
                }
                Paragraph p5 = document.InsertParagraph();
                p5.Alignment = Alignment.left;
                p5.Append(disclaimer)
                   .Font("Times New Roman")
                   .FontSize(8)
                   .Color(Color.Black);

                document.Save();

                BoldTextInBrackets(document);
            }
        }

        public static bool Dekadowka(string jednostka, string miasto, string dataOd, string dataDo, List<Jadlospis> listaJadlospisow)
        {
            try
            {
                System.IO.Directory.CreateDirectory("Dekadówki/" + miasto);
                List<Dieta> listaDiet = DAO.DietaDAO.SelectAll(jednostka);
                DateTime dateFrom = Convert.ToDateTime(dataOd);
                DateTime dateTo = Convert.ToDateTime(dataDo);
                foreach (Dieta d in listaDiet)
                {
                    string path = @"Dekadówki/" + miasto + "/" + dataOd + "-" + dataDo + ", " + d.nazwa + ".docx";
                    List<Jadlospis> listaJadlospisowDlaDiety = listaJadlospisow.Where(x => x.dieta.nazwa == d.nazwa && x.dieta.miasto == d.miasto).Cast<Jadlospis>().ToList();
                    if (listaJadlospisowDlaDiety != null && listaJadlospisowDlaDiety.Count > 0)
                    {
                        using (DocX document = DocX.Create(path))
                        {
                            document.PageLayout.Orientation = Xceed.Words.NET.Orientation.Landscape;
                            document.MarginTop = 10;
                            document.MarginHeader = 0;
                            document.MarginBottom = 10;
                            document.MarginFooter = 0;

                            Paragraph p0 = document.InsertParagraph();
                            p0.Alignment = Alignment.left;
                            Table t0 = document.AddTable(1, 3);
                            t0.Alignment = Alignment.center;
                            t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                            t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                            var image = document.AddImage("pieczatka.png");
                            var picture = image.CreatePicture(39, 125);

                            //var image_lesko = document.AddImage("pieczatka2_lesko.jpg");
                            //var picture_lesko = image_lesko.CreatePicture(39, 125);

                            //var image_ustrzyki = document.AddImage("pieczatka2_ustrzyki.png");
                            //var picture_ustrzyki = image_ustrzyki.CreatePicture(55, 175);

                            t0.SetColumnWidth(0, 2000);
                            t0.SetColumnWidth(1, 5000);
                            t0.SetColumnWidth(2, 3500);
                            if (!listaJadlospisow[0].miasto.Contains("pudełko"))
                                t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                            t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                            t0.Rows[0].Cells[1].Paragraphs[0].Append("Od " + dataOd + " do " + dataDo + ", " + d.nazwa + ", " + miasto)
                                .Font("Times New Roman")
                                .FontSize(12)
                                .Color(Color.Black)
                                .Bold();
                            t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                            if (!listaJadlospisow[0].miasto.Contains("pudełko"))
                            {
                                if (miasto == "Lesko")
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_lesko);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                                else
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_ustrzyki);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                            }
                            p0.InsertTableAfterSelf(t0);

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;

                            int rows = 4;
                            if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie != "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                rows = 6;
                            if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie == "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                rows = 5;
                            int columns = (dateTo - dateFrom).Days + 2;
                            string[] naglowki = null;

                            if (rows == 4)
                            {
                                if (laczoneSniadanie.Contains(d.nazwa) && laczonyObiad.Contains(d.nazwa))
                                    naglowki = new string[4] { "Dzień", "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                else if (laczoneSniadanie.Contains(d.nazwa))
                                    naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                else if (laczonaKolacja.Contains(d.nazwa))
                                    naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                                else
                                    naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja" };
                            }
                            if (rows == 5)
                            {
                                if (laczonaKolacja.Contains(d.nazwa))
                                    naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                else
                                    naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            }
                            if (rows == 6)
                            {
                                if (laczonaKolacja.Contains(d.nazwa))
                                    naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                else
                                    naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            }


                            Table t = document.AddTable(rows, columns);
                            t.Alignment = Alignment.center;
                            t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                            t.SetColumnWidth(0, 1500);
                            for (int i = 1; i < columns; i++)
                            {
                                if (columns <= 8)
                                    t.SetColumnWidth(i, 2000);
                                else
                                {
                                    t.SetColumnWidth(0, 1150);
                                    t.SetColumnWidth(i, 1550);
                                }
                            }
                            int licz = 0;
                            foreach (string s in naglowki)
                            {
                                t.Rows[licz].Cells[0].Paragraphs[0].Append(s)
                                        .FontSize(8).Color(Color.Black).Bold().Font("Times New Roman");
                                licz++;
                            }
                            int licznik = 1;
                            for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                            {
                                string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                                Jadlospis j = DAO.JadlospisDAO.SelectAll(dt, jednostka, d.nazwa);
                                string dzien = data.DayOfWeek.ToString();
                                t.Rows[0].Cells[licznik].Paragraphs[0].Append(dt + "\r\n" + GetDayOfWeek(dzien)).FontSize(8).Color(Color.Black).Bold().Font("Times New Roman");
                                if (j != null)
                                {
                                    if (rows == 6)
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                                            .FontSize(8)
                                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_IIsniadanie).Font("Times New Roman")
                                            .FontSize(8)
                                            .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                                            .FontSize(8)
                                            .Color(Color.Black);
                                        t.Rows[4].Cells[licznik].Paragraphs[0].Append(j.nazwa_podwieczorek).Font("Times New Roman")
                                            .FontSize(8)
                                            .Color(Color.Black);
                                        t.Rows[5].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                    }
                                    else if (rows == 5)
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_podwieczorek).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                        t.Rows[4].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                    }
                                    else
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(8)
                        .Color(Color.Black);
                                    }
                                }
                                licznik++;
                            }
                            p2.InsertTableAfterSelf(t);

                            Paragraph p5 = document.InsertParagraph();
                            p5.Alignment = Alignment.left;
                            p5.Append(disclaimer)
                               .Font("Times New Roman")
                               .FontSize(8)
                               .Color(Color.Black);

                            document.Save();
                            BoldTextInBrackets(document);

                        }
                        if (d.nazwa.Contains("dzieci"))
                        {
                            string path_dzieci_5 = @"Dekadówki/" + miasto + "/" + dataOd + "-" + dataDo + ", " + d.nazwa + " 50%" + ".docx";

                            using (DocX document = DocX.Create(path_dzieci_5))
                            {
                                document.PageLayout.Orientation = Xceed.Words.NET.Orientation.Landscape;
                                document.MarginTop = 10;
                                document.MarginHeader = 0;
                                document.MarginBottom = 10;
                                document.MarginFooter = 0;

                                Paragraph p0 = document.InsertParagraph();
                                p0.Alignment = Alignment.left;
                                Table t0 = document.AddTable(1, 3);
                                t0.Alignment = Alignment.center;
                                t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                                var image = document.AddImage("pieczatka.png");
                                var picture = image.CreatePicture(39, 125);

                                //var image_lesko = document.AddImage("pieczatka2_lesko.jpg");
                                //var picture_lesko = image_lesko.CreatePicture(39, 125);

                                //var image_ustrzyki = document.AddImage("pieczatka2_ustrzyki.png");
                                //var picture_ustrzyki = image_ustrzyki.CreatePicture(55, 175);

                                t0.SetColumnWidth(0, 2000);
                                t0.SetColumnWidth(1, 5000);
                                t0.SetColumnWidth(2, 3500);
                                t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                                t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                                t0.Rows[0].Cells[1].Paragraphs[0].Append("Od " + dataOd + " do " + dataDo + ", " + d.nazwa)
                                    .Font("Times New Roman")
                                    .FontSize(12)
                                    .Color(Color.Black)
                                    .Bold();
                                t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                                if (miasto == "Lesko")
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_lesko);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                                else
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_ustrzyki);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                                p0.InsertTableAfterSelf(t0);

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;

                                int rows = 4;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie != "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 6;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie == "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 5;
                                int columns = (dateTo - dateFrom).Days + 2;
                                string[] naglowki = null;

                                if (rows == 4)
                                {
                                    if (laczoneSniadanie.Contains(d.nazwa) && laczonyObiad.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczoneSniadanie.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja" };
                                }
                                if (rows == 5)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }
                                if (rows == 6)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }


                                Table t = document.AddTable(rows, columns);
                                t.Alignment = Alignment.center;
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetColumnWidth(0, 1500);
                                for (int i = 1; i < columns; i++)
                                {
                                    if (columns <= 8)
                                        t.SetColumnWidth(i, 2000);
                                    else
                                    {
                                        t.SetColumnWidth(0, 1150);
                                        t.SetColumnWidth(i, 1550);
                                    }
                                }
                                int licz = 0;
                                foreach (string s in naglowki)
                                {
                                    t.Rows[licz].Cells[0].Paragraphs[0].Append(s)
                                            .FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    licz++;
                                }
                                int licznik = 1;
                                for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                                {
                                    string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                                    Jadlospis j = DAO.JadlospisDAO.SelectAll(dt, jednostka, d.nazwa);
                                    string dzien = data.DayOfWeek.ToString();
                                    t.Rows[0].Cells[licznik].Paragraphs[0].Append(dt + "\r\n" + GetDayOfWeek(dzien)).FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    if (j != null)
                                    {
                                        if (rows == 6)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.5)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_IIsniadanie, 0.5)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[5].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else if (rows == 5)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.5)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.5)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.5)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                    }
                                    licznik++;
                                }
                                p2.InsertTableAfterSelf(t);

                                Paragraph p5 = document.InsertParagraph();
                                p5.Alignment = Alignment.left;
                                p5.Append(disclaimer)
                                   .Font("Times New Roman")
                                   .FontSize(8)
                                   .Color(Color.Black);
                                document.Save();
                                BoldTextInBrackets(document);
                            }
                            string path_dzieci_7 = @"Dekadówki/" + miasto + "/" + dataOd + "-" + dataDo + ", " + d.nazwa + " 70%" + ".docx";

                            using (DocX document = DocX.Create(path_dzieci_7))
                            {
                                document.PageLayout.Orientation = Xceed.Words.NET.Orientation.Landscape;
                                document.MarginTop = 10;
                                document.MarginHeader = 0;
                                document.MarginBottom = 10;
                                document.MarginFooter = 0;

                                Paragraph p0 = document.InsertParagraph();
                                p0.Alignment = Alignment.left;
                                Table t0 = document.AddTable(1, 3);
                                t0.Alignment = Alignment.center;
                                t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                                var image = document.AddImage("pieczatka.png");
                                var picture = image.CreatePicture(39, 125);

                                //var image_lesko = document.AddImage("pieczatka2_lesko.jpg");
                                //var picture_lesko = image_lesko.CreatePicture(39, 125);

                                //var image_ustrzyki = document.AddImage("pieczatka2_ustrzyki.png");
                                //var picture_ustrzyki = image_ustrzyki.CreatePicture(55, 175);

                                t0.SetColumnWidth(0, 2000);
                                t0.SetColumnWidth(1, 5000);
                                t0.SetColumnWidth(2, 3500);
                                t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                                t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                                t0.Rows[0].Cells[1].Paragraphs[0].Append("Od " + dataOd + " do " + dataDo + ", " + d.nazwa)
                                    .Font("Times New Roman")
                                    .FontSize(12)
                                    .Color(Color.Black)
                                    .Bold();
                                t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                                if (miasto == "Lesko")
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_lesko);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                                else
                                {
                                    //t0.Rows[0].Cells[2].Paragraphs[0].AppendPicture(picture_ustrzyki);
                                    t0.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                                }
                                p0.InsertTableAfterSelf(t0);

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;

                                int rows = 4;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie != "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 6;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie == "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 5;
                                int columns = (dateTo - dateFrom).Days + 2;
                                string[] naglowki = null;

                                if (rows == 4)
                                {
                                    if (laczoneSniadanie.Contains(d.nazwa) && laczonyObiad.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczoneSniadanie.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja" };
                                }
                                if (rows == 5)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }
                                if (rows == 6)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }


                                Table t = document.AddTable(rows, columns);
                                t.Alignment = Alignment.center;
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetColumnWidth(0, 1500);
                                for (int i = 1; i < columns; i++)
                                {
                                    if (columns <= 8)
                                        t.SetColumnWidth(i, 2000);
                                    else
                                    {
                                        t.SetColumnWidth(0, 1150);
                                        t.SetColumnWidth(i, 1550);
                                    }
                                }
                                int licz = 0;
                                foreach (string s in naglowki)
                                {
                                    t.Rows[licz].Cells[0].Paragraphs[0].Append(s)
                                            .FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    licz++;
                                }
                                int licznik = 1;
                                for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                                {
                                    string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                                    Jadlospis j = DAO.JadlospisDAO.SelectAll(dt, jednostka, d.nazwa);
                                    string dzien = data.DayOfWeek.ToString();
                                    t.Rows[0].Cells[licznik].Paragraphs[0].Append(dt + "\r\n" + GetDayOfWeek(dzien)).FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    if (j != null)
                                    {
                                        if (rows == 6)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.7)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_IIsniadanie, 0.7)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[5].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else if (rows == 5)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.7)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.7)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.7)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                    }
                                    licznik++;
                                }
                                p2.InsertTableAfterSelf(t);

                                Paragraph p5 = document.InsertParagraph();
                                p5.Alignment = Alignment.left;
                                p5.Append(disclaimer)
                                   .Font("Times New Roman")
                                   .FontSize(8)
                                   .Color(Color.Black);

                                document.Save();
                                BoldTextInBrackets(document);
                            }
                        }
                        if (d.miasto.Contains("pudełko"))
                        {
                            string path_dzieci_5 = @"Dekadówki/" + miasto + "/" + dataOd + "-" + dataDo + ", " + d.nazwa + " 1500" + ".docx";

                            using (DocX document = DocX.Create(path_dzieci_5))
                            {
                                document.PageLayout.Orientation = Xceed.Words.NET.Orientation.Landscape;
                                document.MarginTop = 10;
                                document.MarginHeader = 0;
                                document.MarginBottom = 10;
                                document.MarginFooter = 0;

                                Paragraph p0 = document.InsertParagraph();
                                p0.Alignment = Alignment.left;
                                Table t0 = document.AddTable(1, 3);
                                t0.Alignment = Alignment.center;
                                t0.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));
                                t0.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Black));

                                t0.SetColumnWidth(0, 2000);
                                t0.SetColumnWidth(1, 5000);
                                t0.SetColumnWidth(2, 3500);
                                t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                                t0.Rows[0].Cells[1].Paragraphs[0].Append("Od " + dataOd + " do " + dataDo + ", " + d.nazwa)
                                    .Font("Times New Roman")
                                    .FontSize(12)
                                    .Color(Color.Black)
                                    .Bold();
                                t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                                p0.InsertTableAfterSelf(t0);

                                Paragraph p2 = document.InsertParagraph();
                                p2.Alignment = Alignment.left;

                                int rows = 4;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie != "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 6;
                                if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie == "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                    rows = 5;
                                int columns = (dateTo - dateFrom).Days + 2;
                                string[] naglowki = null;

                                if (rows == 4)
                                {
                                    if (laczoneSniadanie.Contains(d.nazwa) && laczonyObiad.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie/II śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczoneSniadanie.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad/Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja" };
                                }
                                if (rows == 5)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }
                                if (rows == 6)
                                {
                                    if (laczonaKolacja.Contains(d.nazwa))
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja/Posiłek nocny" };
                                    else
                                        naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                                }


                                Table t = document.AddTable(rows, columns);
                                t.Alignment = Alignment.center;
                                t.SetBorder(TableBorderType.Bottom, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideH, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.InsideV, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Left, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Right, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetBorder(TableBorderType.Top, new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));
                                t.SetColumnWidth(0, 1500);
                                for (int i = 1; i < columns; i++)
                                {
                                    if (columns <= 8)
                                        t.SetColumnWidth(i, 2000);
                                    else
                                    {
                                        t.SetColumnWidth(0, 1150);
                                        t.SetColumnWidth(i, 1550);
                                    }
                                }
                                int licz = 0;
                                foreach (string s in naglowki)
                                {
                                    t.Rows[licz].Cells[0].Paragraphs[0].Append(s)
                                            .FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    licz++;
                                }
                                int licznik = 1;
                                for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                                {
                                    string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                                    Jadlospis j = DAO.JadlospisDAO.SelectAll(dt, jednostka, d.nazwa);
                                    string dzien = data.DayOfWeek.ToString();
                                    t.Rows[0].Cells[licznik].Paragraphs[0].Append(dt + "\r\n" + GetDayOfWeek(dzien)).FontSize(8)
                                .Color(Color.Black).Bold().Font("Times New Roman");
                                    if (j != null)
                                    {
                                        if (rows == 6)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.75)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_IIsniadanie, 0.75)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[5].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else if (rows == 5)
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.75)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_podwieczorek, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[4].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                        else
                                        {
                                            t.Rows[1].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_sniadanie, 0.75)).Font("Times New Roman")
                                .FontSize(8)
                                .Color(Color.Black);
                                            t.Rows[2].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_obiad, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                            t.Rows[3].Cells[licznik].Paragraphs[0].Append(ZamienGramature(j.nazwa_kolacja, 0.75)).Font("Times New Roman")
                            .FontSize(8)
                            .Color(Color.Black);
                                        }
                                    }
                                    licznik++;
                                }
                                p2.InsertTableAfterSelf(t);

                                Paragraph p5 = document.InsertParagraph();
                                p5.Alignment = Alignment.left;
                                p5.Append(disclaimer)
                                   .Font("Times New Roman")
                                   .FontSize(8)
                                   .Color(Color.Black);
                                document.Save();
                                BoldTextInBrackets(document);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie można wydrukować dokumentu", "Błąd");
                return false;
            }

            return true;
        }

        #region Private methods

        public static void BoldTextInBrackets(DocX document)
        {
            // Step 1: bold anything inside parentheses like "(mąka pszenna)"
            var boldFormatting = new Formatting { Bold = true };

            document.ReplaceText(
                searchValue: @"\(.[^0-9]+?\)",
                newValue: "$0",                // keep the matched text unchanged
                trackChanges: false,
                options: RegexOptions.None,
                newFormatting: boldFormatting,
                matchFormatting: null,
                fo: MatchFormattingOptions.SubsetMatch,
                escapeRegEx: false,            // treat oldValue as a real regex
                useRegExSubstitutions: true    // needed so "$0" works
            );

            // Step 2: un-bold the category words, wherever they occur
            var normalFormatting = new Formatting { Bold = false };

            document.ReplaceText(
                searchValue: @"zboża|zboza|zawierające|zawierajace|mąka|maka",
                newValue: "$0",
                trackChanges: false,
                options: RegexOptions.IgnoreCase,
                newFormatting: normalFormatting,
                matchFormatting: null,
                fo: MatchFormattingOptions.SubsetMatch,
                escapeRegEx: false,
                useRegExSubstitutions: true
            );

            document.Save();
        }

        private static string GetMonthForDate(int month)
        {
            switch (month)
            {
                case 1:
                    return "stycznia";
                case 2:
                    return "lutego";
                case 3:
                    return "marca";
                case 4:
                    return "kwietnia";
                case 5:
                    return "maja";
                case 6:
                    return "czerwca";
                case 7:
                    return "lipca";
                case 8:
                    return "sierpnia";
                case 9:
                    return "września";
                case 10:
                    return "października";
                case 11:
                    return "listopada";
                case 12:
                    return "grudnia";
            }
            return "";
        }

        private static string GetDayOfWeek(string day)
        {
            switch (day)
            {
                case "Monday":
                    return "Poniedziałek";
                case "Tuesday":
                    return "Wtorek";
                case "Wednesday":
                    return "Środa";
                case "Thursday":
                    return "Czwartek";
                case "Friday":
                    return "Piątek";
                case "Saturday":
                    return "Sobota";
                case "Sunday":
                    return "Niedziela";
            }
            return "";
        }

        public static string ZamienGramature(string inputString, double percent)
        {
            string resultString = Regex.Replace(inputString, @"\d+(\.\d+)?", m =>
            {
                // Parse the matched numeric value
                if (double.TryParse(m.Value, out double number))
                {
                    // Decrease the number by 70%
                    double decreasedNumber = number * percent;

                    // Format the result back to the original number format
                    return decreasedNumber.ToString(m.Groups[1].Success ? "F1" : ""); // F1 to keep one decimal place if it's a floating-point number
                }

                return m.Value;
            });
            return resultString;
        }

        #endregion
    }
}
