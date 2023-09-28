namespace KalkulatorDiety
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Xceed.Words.NET;

    public class Printer
    {
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
                            .FontSize(10)
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
                                    .FontSize(10)
                                    .Color(Color.Black);
                            else if (c == columns)
                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                    .Font("Times New Roman")
                                    .FontSize(10)
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
                            .FontSize(10)
                            .Color(Color.Black);

                        t2.Rows[1].Cells[i].Paragraphs[0].Append(suma[i].ToString())
                                .Font("Times New Roman")
                                .FontSize(12)
                                .Color(Color.Black);
                    }
                    p4.Append("\r\nWartości odżywcze:\r\n");
                    p4.InsertTableAfterSelf(t2);

                    document.Save();
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można wydrukować dokumentu. \r\n {ex.Message}", "Błąd");
            }
        }

        public static void Jadlospis(Jadlospis jadlospis)
        {
            //try
            //{
                if (jadlospis != null)
                {
                    DateTime data = Convert.ToDateTime(jadlospis.data);
                    System.IO.Directory.CreateDirectory("Jadłospisy/" + jadlospis.miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day);
                    string path = @"Jadłospisy/" + jadlospis.miasto + "/" + data.Year + "/" + data.Month + "/" + data.Day + "/" + jadlospis.data + ", " + jadlospis.dieta.nazwa + ".docx";

                    using (DocX document = DocX.Create(path))
                    {
                        Paragraph p = document.InsertParagraph();
                        p.Alignment = Alignment.center;
                        p.Append($"{jadlospis.data}, {GetDayOfWeek(Convert.ToDateTime(jadlospis.data).DayOfWeek.ToString()).ToLower()}\r\n{jadlospis.dieta.nazwa}\r\n{jadlospis.miasto}")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Color(Color.Black)
                        .Bold();

                        double[] suma_kalorie = new double[5];
                        double[] suma_masa = new double[5];
                        string[] pr = jadlospis.sklad_sniadanie.Split('$');
                        int cl = pr[0].Split('|').Length;
                        double[] sum = new double[cl - 1];

                    //ŚNIADANIE
                    if (jadlospis.sklad_sniadanie != "")
                        {
                            int rows = pr.Length + 2;
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

                            p2.Append("\r\nŚniadanie: " + jadlospis.nazwa_sniadanie)
                           .Font("Times New Roman")
                           .FontSize(10)
                           .Color(Color.Black);
                            for (int i = 0; i < cl + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                 .Font("Times New Roman")
                                 .FontSize(9)
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
                                                .FontSize(9)
                                                .Color(Color.Black);
                                        else if (c == cl)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(10)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);
                                        if(c == 1)
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
                            t.Rows[rows - 2].Cells[1].Paragraphs[0].Append("Suma")
                                               .Font("Times New Roman")
                                            .FontSize(9)
                                           .Color(Color.Black);
                            for (int i = 0; i < cl - 1; i++)
                                t.Rows[rows - 2].Cells[i + 2].Paragraphs[0].Append(sum[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);

                        t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                          .Font("Times New Roman")
                                       .FontSize(9)
                                      .Color(Color.Black);
                        for (int i = 0; i < cl - 1; i++)
                            t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * sum[i] / suma_masa[0],2).ToString())
                                            .Font("Times New Roman")
                                         .FontSize(9)
                                        .Color(Color.Black);

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
                            int rows = produkty.Length + 2;
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
                               .FontSize(10)
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
                                                    .FontSize(9)
                                                    .Color(Color.Black);
                                        else if (c == columns)
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                                .Font("Times New Roman")
                                                .FontSize(10)
                                                .Color(Color.Black);
                                        else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                 .FontSize(9)
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
                        t.Rows[rows - 2].Cells[1].Paragraphs[0].Append("Suma")
                                              .Font("Times New Roman")
                                           .FontSize(9)
                                          .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 2].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);
                            

                        t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
                                          .Font("Times New Roman")
                                       .FontSize(9)
                                      .Color(Color.Black);
                        for (int i = 0; i < columns - 1; i++)
                            t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[1], 2).ToString())
                                            .Font("Times New Roman")
                                         .FontSize(9)
                                        .Color(Color.Black);
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
                            int rows = produkty.Length + 2;
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

                            p2.Append("\r\nObiad: " + jadlospis.nazwa_obiad)
                               .Font("Times New Roman")
                               .FontSize(10)
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
                                                    .FontSize(9)
                                                    .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(10)
                                            .Color(Color.Black);
                                        else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                 .FontSize(9)
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
                        t.Rows[rows - 2].Cells[1].Paragraphs[0].Append("Suma")
                                              .Font("Times New Roman")
                                           .FontSize(9)
                                          .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 2].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);
                        t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
              .Font("Times New Roman")
           .FontSize(9)
          .Color(Color.Black);
                        for (int i = 0; i < columns - 1; i++)
                            t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[2], 2).ToString())
                                            .Font("Times New Roman")
                                         .FontSize(9)
                                        .Color(Color.Black);
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
                            int rows = produkty.Length + 2;
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
                               .FontSize(10)
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
                                                .FontSize(9)
                                                .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(10)
                                            .Color(Color.Black);
                                        else
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                .Font("Times New Roman")
                                             .FontSize(9)
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
                        t.Rows[rows - 2].Cells[1].Paragraphs[0].Append("Suma")
                                              .Font("Times New Roman")
                                           .FontSize(9)
                                          .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 2].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);
                        t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
              .Font("Times New Roman")
           .FontSize(9)
          .Color(Color.Black);
                        for (int i = 0; i < columns - 1; i++)
                            t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[3], 2).ToString())
                                            .Font("Times New Roman")
                                         .FontSize(9)
                                        .Color(Color.Black);
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
                            int rows = produkty.Length + 2;
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

                            p2.Append("\r\nKolacja: " + jadlospis.nazwa_kolacja)
                           .Font("Times New Roman")
                           .FontSize(10)
                           .Color(Color.Black);

                            for (int i = 0; i < columns + 1; i++)
                            {
                                t.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i])
                                    .Font("Times New Roman")
                                    .FontSize(10)
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
                                                    .FontSize(9)
                                                    .Color(Color.Black);
                                        else if (c == columns)
                                            t.Rows[r + 1].Cells[c].Paragraphs[0].Append(Math.Round(Double.Parse(dane[c - 1]) * 0.0025, 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(10)
                                            .Color(Color.Black);
                                        else
                                                t.Rows[r + 1].Cells[c].Paragraphs[0].Append(dane[c])
                                                    .Font("Times New Roman")
                                                 .FontSize(9)
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

                        t.Rows[rows - 2].Cells[1].Paragraphs[0].Append("Suma")
                                              .Font("Times New Roman")
                                           .FontSize(9)
                                          .Color(Color.Black);
                            for (int i = 0; i < columns - 1; i++)
                                t.Rows[rows - 2].Cells[i + 2].Paragraphs[0].Append(suma2[i].ToString())
                                                .Font("Times New Roman")
                                             .FontSize(9)
                                            .Color(Color.Black);

                        t.Rows[rows - 1].Cells[1].Paragraphs[0].Append("na 100g")
              .Font("Times New Roman")
           .FontSize(9)
          .Color(Color.Black);
                        for (int i = 0; i < columns - 1; i++)
                            t.Rows[rows - 1].Cells[i + 2].Paragraphs[0].Append(Math.Round(100 * suma2[i] / suma_masa[4], 2).ToString())
                                            .Font("Times New Roman")
                                         .FontSize(9)
                                        .Color(Color.Black);
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
                        Table t2 = document.AddTable(3, columns2 );
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
                                .FontSize(9)
                                .Color(Color.Black);
                    t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                               .Font("Times New Roman")
                               .FontSize(9)
                               .Color(Color.Black);

                    for (int i = 1; i < columns2 ; i++)
                        {
                                t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki2[i + 1])
                                .Font("Times New Roman")
                                .FontSize(9)
                                .Color(Color.Black);

                                string dodatek = "";
                                    if (i == 2) 
                                    {
                                        dodatek = (Math.Round((sum[i-1] * Form1.przelicznik_Bialko * 100.0) / sum[0], 2)).ToString();
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
                                            .FontSize(9)
                                            .Color(Color.Black);
                                t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(sum[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                                                   .Font("Times New Roman")
                                                   .FontSize(9)
                                                   .Color(Color.Black);
                    }
                        p3.Append("\r\nWartości odżywcze:").Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                    p3.InsertTableAfterSelf(t2);



                        Paragraph p4 = document.InsertParagraph();
                        p4.Alignment = Alignment.left;
                        int col;
                        string[] nag;
                        if (suma_kalorie[1] != 0 && suma_kalorie[3] != 0) 
                        { 
                            col = 5; 
                            nag = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" }; 
                        }
                        else if (suma_kalorie[1] == 0 && suma_kalorie[3] != 0) 
                        { 
                            col = 4; 
                            nag = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" }; 
                        }
                        else 
                        { 
                            col = 3;
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
                                .FontSize(10)
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
                               .FontSize(10)
                               .Color(Color.Black);

                        }
                        p4.InsertTableAfterSelf(t3);

                        var image = document.AddImage("pieczatka.png");
                        var picture = image.CreatePicture(39, 125);
                        Paragraph p5 = document.InsertParagraph().Append("\r\n");
                        p5.AppendPicture(picture);

                        document.Save();
                    }
                }
                else
                {
                    MessageBox.Show("Brak jadłospisu");
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Nie można wydrukować dokumentu. \r\n {ex.Message}", "Błąd");
            //}
        }

        public static void JadlospisDzienny(List<Jadlospis> listaJadlospisow)
        {
            //try
            //{
                if (listaJadlospisow.Count > 0)
                {
                    System.IO.Directory.CreateDirectory("Jadłospisy dzienne/" + listaJadlospisow[0].miasto);
                    string path = @"Jadłospisy dzienne/" + listaJadlospisow[0].miasto + "/" + listaJadlospisow[0].data + ".docx";

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
                        t0.Rows[0].Cells[0].Paragraphs[0].AppendPicture(picture);
                        t0.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                        t0.Rows[0].Cells[1].Paragraphs[0].Append($"JADŁOSPIS\r\n{listaJadlospisow[0].data}\r\n{GetDayOfWeek(Convert.ToDateTime(listaJadlospisow[0].data).DayOfWeek.ToString()).ToLower()}\r\n{listaJadlospisow[0].miasto}")
                            .Font("Times New Roman")
                            .FontSize(12)
                            .Color(Color.Black)
                            .Bold();
                        t0.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;

                        p0.InsertTableAfterSelf(t0);

                        foreach (Jadlospis jadlospis in listaJadlospisow)
                        {
                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;

                            p2.Append("\r\n" + jadlospis.dieta.nazwa)
                               .Font("Times New Roman")
                               .FontSize(10)
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
                                naglowki = new string[3] { "Śniadanie", "Obiad", "Kolacja" };
                            if (columns == 4)
                                naglowki = new string[4] { "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            if (columns == 5)
                                naglowki = new string[5] { "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };



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
                                    .FontSize(10)
                                    .Color(Color.Black).Bold();
                            }
                            if (naglowki.Length == 3)
                            {
                                t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            }
                            else if (naglowki.Length == 4)
                            {
                                t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            }
                            else
                            {
                                t.Rows[1].Cells[0].Paragraphs[0].Append(jadlospis.nazwa_sniadanie).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[1].Paragraphs[0].Append(jadlospis.nazwa_IIsniadanie).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[2].Paragraphs[0].Append(jadlospis.nazwa_obiad).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[3].Paragraphs[0].Append(jadlospis.nazwa_podwieczorek).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                                t.Rows[1].Cells[4].Paragraphs[0].Append(jadlospis.nazwa_kolacja).Font("Times New Roman")
                                    .FontSize(9)
                                    .Color(Color.Black);
                            }
                            p2.InsertTableAfterSelf(t);
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
                            p2.Append("\r\n" + jadlospis.dieta.nazwa)
                               .Font("Times New Roman")
                               .FontSize(10)
                               .Color(Color.Black)
                               .Bold();

                        string[] naglowki;
                        string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                        int columns2 = produkty[0].Split('|').Length;

                        if(columns2 == 10)
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

                            Table t2 = document.AddTable(3, columns2);
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
                                .FontSize(9)
                                .Color(Color.Black);
                            t2.Rows[2].Cells[0].Paragraphs[0].Append("na 100g")
                                 .Font("Times New Roman")
                                 .FontSize(9)
                                 .Color(Color.Black);
                            for (int i = 1; i < columns2; i++)
                            {
                                    t2.Rows[0].Cells[i].Paragraphs[0].Append(naglowki[i + 1])
                                        .Font("Times New Roman")
                                        .FontSize(9)
                                        .Color(Color.Black);
                                    t2.Rows[1].Cells[i].Paragraphs[0].Append(suma[i - 1].ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                                    t2.Rows[2].Cells[i].Paragraphs[0].Append(Math.Round(suma[i - 1] * 100 / (suma_masa[0] + suma_masa[1] + suma_masa[2] + suma_masa[3] + suma_masa[4]), 2).ToString())
                                            .Font("Times New Roman")
                                            .FontSize(9)
                                            .Color(Color.Black);
                            }
                            p2.InsertTableAfterSelf(t2);
                        }

                        Paragraph p5 = document.InsertParagraph();
                        p5.Alignment = Alignment.left;
                        p5.Append("\r\n* substancje lub produkty powodujące alergie lub reakcje nietolerancji zaznaczono pogrubionym drukiem w odniesieniu do załącznika \r\n* możliwe odchylenia +/- 10 %")
                           .Font("Times New Roman")
                           .FontSize(9)
                           .Color(Color.Black);

                        document.Save();
                    }
                }
                else
                {
                    MessageBox.Show("Brak jadłospisów we wskazanym dniu", "Błąd");
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Nie można wydrukować dokumentu", "Błąd");
            //}
        }

        public static void Dekadowka(string miasto, string dataOd, string dataDo, List<Jadlospis> listaJadlospisow)
        {
            //try
            //{
                System.IO.Directory.CreateDirectory("Dekadówki/" + miasto);
                List<Dieta> listaDiet = DAO.DietaDAO.SelectAll(miasto);
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
                            Paragraph p = document.InsertParagraph();
                            p.Alignment = Alignment.center;
                            p.Append("Od " + dataOd + " do " + dataDo + ", " + d.nazwa)
                            .Font("Times New Roman")
                            .FontSize(12)
                            .Color(Color.Black)
                            .Bold();

                            Paragraph p2 = document.InsertParagraph();
                            p2.Alignment = Alignment.left;

                            int rows = 4;
                            if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie != "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                rows = 6;
                            if (listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_IIsniadanie == "" && listaJadlospisowDlaDiety[listaJadlospisowDlaDiety.Count - 1].sklad_podwieczorek != "")
                                rows = 5;
                            int columns = (dateTo - dateFrom).Days + 2;
                            string[] naglowki = null;
                            if (rows == 6)
                                naglowki = new string[6] { "Dzień", "Śniadanie", "II śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            else if (rows == 5)
                                naglowki = new string[5] { "Dzień", "Śniadanie", "Obiad", "Podwieczorek", "Kolacja" };
                            else
                                naglowki = new string[4] { "Dzień", "Śniadanie", "Obiad", "Kolacja" };


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
                                        .FontSize(9)
                            .Color(Color.Black).Bold().Font("Times New Roman");
                                licz++;
                            }
                            int licznik = 1;
                            for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                            {
                                string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                                Jadlospis j = DAO.JadlospisDAO.SelectAll(dt, miasto, d.nazwa);
                                string dzien = data.DayOfWeek.ToString();
                                t.Rows[0].Cells[licznik].Paragraphs[0].Append(dt + "\r\n" + GetDayOfWeek(dzien)).FontSize(9)
                            .Color(Color.Black).Bold().Font("Times New Roman");
                                if (j != null)
                                {
                                    if (rows == 6)
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_IIsniadanie).Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                        t.Rows[4].Cells[licznik].Paragraphs[0].Append(j.nazwa_podwieczorek).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                        t.Rows[5].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                    }
                                    else if (rows == 5)
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_podwieczorek).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                        t.Rows[4].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                    }
                                    else
                                    {
                                        t.Rows[1].Cells[licznik].Paragraphs[0].Append(j.nazwa_sniadanie).Font("Times New Roman")
                            .FontSize(9)
                            .Color(Color.Black);
                                        t.Rows[2].Cells[licznik].Paragraphs[0].Append(j.nazwa_obiad).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                        t.Rows[3].Cells[licznik].Paragraphs[0].Append(j.nazwa_kolacja).Font("Times New Roman")
                        .FontSize(9)
                        .Color(Color.Black);
                                    }
                                    licznik++;
                                }
                            }
                            p2.InsertTableAfterSelf(t);
                            document.Save();

                            //  MessageBox.Show("Zapisano dokument");
                        }
                    }
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Nie można wydrukować dokumentu", "Błąd");
            //}
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

    }
}
