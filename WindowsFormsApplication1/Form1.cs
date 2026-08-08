namespace KalkulatorDiety
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Windows.Forms;
    using KalkulatorDiety.DAO;
    using KalkulatorDiety.Models;

    public partial class Form1 : Form
    {
        #region Zmienne 

        private int wybranaDieta;
        private int wybraneMiasto;
        private string kategoria;

        public static double przelicznik_Bialko = 4; //kcal na 1g
        public static double przelicznik_Weglowodany = 4; //kcal na 1g
        public static double przelicznik_Tluszcze = 9; //kcal na 1g

        private static readonly Color highlightColor = Color.FromArgb(91, 146, 121); 
        private static readonly Color primaryColor = Color.FromArgb(31, 61, 46);
        private static readonly Color sandColor = Color.FromArgb(247, 243, 233);
        private static readonly Font DietLabelFont = new Font("Segoe UI", 12);
        private static readonly Font MealLabelFont = new Font("Segoe UI", 10);
        public static int borderRadius = 20;

        private double[,] suma;
        private double[,] procent;

        private List<Jednostka> listaJednostek;
        private List<Receptura> listaReceptur;
        private List<Dekadowka> listaDekadowekDoWczytania;
        private Dekadowka wybranaDekadowkaDoWczytania;
        private Jadlospis jadlospisDekadowkiDoWczytania;
        private List<Dekadowka> listaDekadowekDoZapisania;
        private Dekadowka wybranaDekadowkaDoZapisania;
        private List<Produkt> Lista;
        private List<Dieta> Diety = new List<Dieta>();
        private List<Produkt> Bakalie = new List<Produkt>();
        private List<Produkt> Warzywa = new List<Produkt>();
        private List<Produkt> Owoce = new List<Produkt>();
        private List<Produkt> Zboza = new List<Produkt>();
        private List<Produkt> Mieso = new List<Produkt>();
        private List<Produkt> Ryby = new List<Produkt>();
        private List<Produkt> Przyprawy = new List<Produkt>();
        private List<Produkt> Napoje = new List<Produkt>();
        private List<Produkt> Nabial = new List<Produkt>();
        private List<Produkt> Tluszcze = new List<Produkt>();
        private List<Produkt> Slodycze = new List<Produkt>();

        private List<Dekadowka> listaDekadowek;
        private Dekadowka wybranaDekadowka;

        private readonly int[] dekadowkaSize = new int[] { 900, 475 };
        private readonly int[] dzienSize = new int[] { 300, 450 };
        private readonly int[] dietaSize = new int[] { 295, 200 };

        private readonly KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        private readonly String XML_Location = @"DataBase.xml";
        public static readonly string[] DietaPriority = new[]
        {
            "Dieta podstawowa",
            "Dieta łatwostrawna",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 3 posiłkowa",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 4 posiłkowa",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 5 posiłkowa",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów 6 posiłkowa",
            "Dieta łatwostrawna z ograniczeniem łatwo przyswajalnych węglowodanów",
            "Dieta z ograniczeniem łatwo przyswajalnych węglowodanów i nasyconych kwasów tłuszczowych",
            "Dieta bogatobiałkowa",
            "Dieta łatwostrawna z ograniczeniem tłuszczu",
            "Dieta bezmleczna",
            "Dieta papkowata",
            "Dieta łatwostrawna o zmienionej konsystencji - płynna wzmocniona",
            "Dieta bezglutenowa",
            "Dieta niskobiałkowa",
            "Dieta wegetariańska",
            "Dieta uberopurynowa",
            "Dieta ubogoenergetyczna 1200kcal",
            "Dieta redukcyjna 1200kcal",
            "Dieta ubogoenergetyczna 1400kcal",
            "Dieta redukcyjna 1400kcal",
            "Dieta ubogoresztkowa",
            "Dieta niskobiałkowa",
            "Dieta łatwostrawna z ograniczeniem ubstancji pobudzających wydzielanie soku żołądkowego",
            "Dieta podstawowa dzieci",
            "Dieta łatwostrawna dzieci",
            "Dieta bezmleczna dzieci",
            "Dieta bezglutenowa dzieci",
            "Dieta łatwostrawna osób starszych",
        };

        #endregion

        #region Obsługa aplikacji

        public Form1()
        {
            InitializeComponent();
            DAO.DAO.ReloadDatabase();
            this.WindowState = FormWindowState.Maximized;
            suma = new double[6, 10];
            procent = new double[6, 10];

            glownaClick();

            cb_kategorie.SelectedItem = "Wszystkie kategorie";

            panel_produkty.Dock = DockStyle.Fill;
            panel_dekadowka.Dock = DockStyle.Fill;
            panel_dekadowka_zapisz.Dock = DockStyle.Fill;
            panel_dekadowka_wczytaj.Dock = DockStyle.Fill;
            panel_glowny.Dock = DockStyle.Fill;
            panel_dieta.Dock = DockStyle.Fill;
            panel_jednostka.Dock = DockStyle.Fill;
            panel_jadlospis.Dock = DockStyle.Fill;
            panel_receptura.Dock = DockStyle.Fill;
            panel_drukuj.Dock = DockStyle.Fill;
            panel_kontrola.Dock = DockStyle.Fill;

            dekadowka_panel.AutoScroll = true;
            dekadowka_panel.FlowDirection = FlowDirection.LeftToRight;
            dekadowka_panel.VerticalScroll.Visible = false;
            dekadowka_panel.HorizontalScroll.Visible = false;
            dekadowka_panel.WrapContents = false;
            dekadowka_panel.BackColor = Color.White;
            dekadowka_panel.Size = new Size(dekadowkaSize[0], dekadowkaSize[1]);

            LiczSrednia();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            switch (MessageBox.Show(this, "Na pewno chcesz zamknąć program?", "Zakończ", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private void Masa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    Dodaj_Click(sender, e);
                    e.Handled = true;

                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Produkt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Potwierdź", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Usun_Click(sender, e);
                }
            }
        }

        #endregion

        #region Strona główna

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (lb_produkty.SelectedIndex != -1)
            {
                if (tb_masa.Text != "")
                {
                    try
                    {
                        double masa = Math.Round(double.Parse(tb_masa.Text), 2);
                        int posilek = Int32.Parse(tc_posilki.SelectedIndex.ToString());
                        int ktory = lb_produkty.SelectedIndex;
                        string[] arr = new string[11];
                        List<Produkt> Kategoria = new List<Produkt>();
                        switch (kategoria)
                        {
                            case "Wszystkie":
                                Kategoria = Lista;
                                break;
                            case "M":
                                Kategoria = Mieso;
                                break;
                            case "W":
                                Kategoria = Warzywa;
                                break;
                            case "O":
                                Kategoria = Owoce;
                                break;
                            case "S":
                                Kategoria = Slodycze;
                                break;
                            case "R":
                                Kategoria = Ryby;
                                break;
                            case "D":
                                Kategoria = Napoje;
                                break;
                            case "Z":
                                Kategoria = Zboza;
                                break;
                            case "P":
                                Kategoria = Przyprawy;
                                break;
                            case "N":
                                Kategoria = Nabial;
                                break;
                            case "B":
                                Kategoria = Bakalie;
                                break;
                            case "T":
                                Kategoria = Tluszcze;
                                break;
                        }
                        arr[0] = Kategoria[ktory].nazwa;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(Kategoria[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                        arr[3] = Math.Round(Kategoria[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                        arr[6] = Math.Round(Kategoria[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                        arr[4] = Math.Round(Kategoria[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                        arr[5] = Math.Round(Kategoria[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                        arr[7] = Math.Round(Kategoria[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                        arr[8] = Math.Round(Kategoria[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                        arr[9] = Math.Round(Kategoria[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                        arr[10] = Math.Round(Kategoria[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                        ListViewItem itm = new ListViewItem(arr);
                        switch (posilek)
                        {
                            case 0:
                                lv_sniadanie.Items.Add(itm); 
                                break;
                            case 1:
                                lv_IIsniadanie.Items.Add(itm);
                                break;
                            case 2:
                                lv_obiad.Items.Add(itm);
                                break;
                            case 3:
                                lv_podwieczorek.Items.Add(itm);
                                break;
                            case 4:
                                lv_kolacja.Items.Add(itm);
                                break;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Nieprawidłowa wartość.", "Błąd");
                    }
                    LiczSrednia();
                }
                else
                {
                    MessageBox.Show("Nie wpisano masy produktu.", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu.", "Błąd");
            }

        }

        private void Usun_Click(object sender, EventArgs e)
        {
            int posilek = tc_posilki.SelectedIndex;
            string produkt = "";
            List<int> ktory;
            switch (posilek)
            {
                case 0:
                    ktory = new List<int>();
                    for (int k = 0; k < lv_sniadanie.SelectedIndices.Count; k++)
                        ktory.Add(Int32.Parse(lv_sniadanie.SelectedIndices[k].ToString()));
                    if (ktory.Count > 0)
                        produkt = lv_sniadanie.Items[ktory[0]].Text;
                    break;
                case 1:
                    ktory = new List<int>();
                    for (int k = 0; k < lv_IIsniadanie.SelectedIndices.Count; k++)
                        ktory.Add(Int32.Parse(lv_IIsniadanie.SelectedIndices[k].ToString()));
                    if (ktory.Count > 0)
                        produkt = lv_IIsniadanie.Items[ktory[0]].Text;
                    break;

                case 2:
                    ktory = new List<int>();
                    for (int k = 0; k < lv_obiad.SelectedIndices.Count; k++)
                        ktory.Add(Int32.Parse(lv_obiad.SelectedIndices[k].ToString()));
                    if (ktory.Count > 0)
                        produkt = lv_obiad.Items[ktory[0]].Text;
                    break;
                case 3:
                    ktory = new List<int>();
                    for (int k = 0; k < lv_podwieczorek.SelectedIndices.Count; k++)
                        ktory.Add(Int32.Parse(lv_podwieczorek.SelectedIndices[k].ToString()));
                    if (ktory.Count > 0)
                        produkt = lv_podwieczorek.Items[ktory[0]].Text;
                    break;

                case 4:
                    ktory = new List<int>();
                    for (int k = 0; k < lv_kolacja.SelectedIndices.Count; k++)
                        ktory.Add(Int32.Parse(lv_kolacja.SelectedIndices[k].ToString()));
                    if (ktory.Count > 0)
                        produkt = lv_kolacja.Items[ktory[0]].Text;
                    break;
            }
            if (produkt != "")
            {
                switch (posilek)
                {
                    case 0:
                        ktory = new List<int>();
                        for (int k = 0; k < lv_sniadanie.SelectedIndices.Count; k++)
                            ktory.Add(Int32.Parse(lv_sniadanie.SelectedIndices[k].ToString()));
                        lv_sniadanie.Items.RemoveAt(ktory[0]);
                        break;
                    case 1:
                        ktory = new List<int>();
                        for (int k = 0; k < lv_IIsniadanie.SelectedIndices.Count; k++)
                            ktory.Add(Int32.Parse(lv_IIsniadanie.SelectedIndices[k].ToString()));
                        lv_IIsniadanie.Items.RemoveAt(ktory[0]);
                        break;
                    case 2:
                        ktory = new List<int>();
                        for (int k = 0; k < lv_obiad.SelectedIndices.Count; k++)
                            ktory.Add(Int32.Parse(lv_obiad.SelectedIndices[k].ToString()));
                        lv_obiad.Items.RemoveAt(ktory[0]);
                        break;
                    case 3:
                        ktory = new List<int>();
                        for (int k = 0; k < lv_podwieczorek.SelectedIndices.Count; k++)
                            ktory.Add(Int32.Parse(lv_podwieczorek.SelectedIndices[k].ToString()));
                        lv_podwieczorek.Items.RemoveAt(ktory[0]);
                        break;
                    case 4:
                        ktory = new List<int>();
                        for (int k = 0; k < lv_kolacja.SelectedIndices.Count; k++)
                            ktory.Add(Int32.Parse(lv_kolacja.SelectedIndices[k].ToString()));
                        lv_kolacja.Items.RemoveAt(ktory[0]);
                        break;
                }
                LiczSrednia();
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu.", "Błąd");
            }
        }

        public void LiczSrednia()
        {
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma[i, k] = 0;
                    procent[i, k] = 0;
                }

            }

            string[] arr = new string[10];
            for (int i = 0; i < 10; i++)
            {
                arr[i] = "0";
            }
            ListViewItem itm = new ListViewItem(arr)
            {
                UseItemStyleForSubItems = false
            };

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < lv_sniadanie.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(lv_sniadanie.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma[0, k] += a;
                }
            }

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(lv_IIsniadanie.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma[1, k] += a;
                }
            }

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < lv_obiad.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(lv_obiad.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma[2, k] += a;
                }
            }
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(lv_podwieczorek.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma[3, k] += a;
                }
            }


            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < lv_kolacja.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(lv_kolacja.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma[4, k] += a;
                }
            }

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    suma[5, k] += suma[i, k];
                }
            }
            if (suma[5, 0] != 0)
            {
                if (lv_podwieczorek.Items.Count != 0 || lv_IIsniadanie.Items.Count != 0)
                {
                    procent_sniadanie.Text = Math.Round(((suma[0, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_sniadanie.Text) >= 25.0 && Convert.ToDouble(procent_sniadanie.Text) <= 30.0)
                        procent_sniadanie.ForeColor = Color.Green;
                    else
                        procent_sniadanie.ForeColor = Color.Red;
                    procent_sniadanie.Text += " %";

                    procent_IIsniadanie.Text = Math.Round(((suma[1, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_IIsniadanie.Text) >= 5.0 && Convert.ToDouble(procent_IIsniadanie.Text) <= 10.0)
                        procent_IIsniadanie.ForeColor = Color.Green;
                    else
                        procent_IIsniadanie.ForeColor = Color.Red;
                    procent_IIsniadanie.Text += " %";

                    procent_obiad.Text = Math.Round(((suma[2, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_obiad.Text) >= 35.0 && Convert.ToDouble(procent_obiad.Text) <= 40.0)
                        procent_obiad.ForeColor = Color.Green;
                    else
                        procent_obiad.ForeColor = Color.Red;
                    procent_obiad.Text += " %";

                    procent_podwieczorek.Text = Math.Round(((suma[3, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_podwieczorek.Text) >= 5.0 && Convert.ToDouble(procent_podwieczorek.Text) <= 10.0)
                        procent_podwieczorek.ForeColor = Color.Green;
                    else
                        procent_podwieczorek.ForeColor = Color.Red;
                    procent_podwieczorek.Text += " %";

                    procent_kolacja.Text = Math.Round(((suma[4, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_kolacja.Text) >= 15.0 && Convert.ToDouble(procent_kolacja.Text) <= 20.0)
                        procent_kolacja.ForeColor = Color.Green;
                    else
                        procent_kolacja.ForeColor = Color.Red;
                    procent_kolacja.Text += " %";
                }
                else
                {
                    procent_sniadanie.Text = Math.Round(((suma[0, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_sniadanie.Text) >= 30.0 && Convert.ToDouble(procent_sniadanie.Text) <= 35.0)
                        procent_sniadanie.ForeColor = Color.Green;
                    else
                        procent_sniadanie.ForeColor = Color.Red;
                    procent_sniadanie.Text += " %";

                    procent_IIsniadanie.Text = Math.Round(((suma[1, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    procent_IIsniadanie.Text += " %";

                    procent_obiad.Text = Math.Round(((suma[2, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_obiad.Text) >= 35.0 && Convert.ToDouble(procent_obiad.Text) <= 40.0)
                        procent_obiad.ForeColor = Color.Green;
                    else
                        procent_obiad.ForeColor = Color.Red;
                    procent_obiad.Text += " %";

                    procent_podwieczorek.Text = Math.Round(((suma[3, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    procent_podwieczorek.Text += " %";

                    procent_kolacja.Text = Math.Round(((suma[4, 0] * 100.0) / suma[5, 0]), 2).ToString();
                    if (Convert.ToDouble(procent_kolacja.Text) >= 25.0 && Convert.ToDouble(procent_kolacja.Text) <= 30.0)
                        procent_kolacja.ForeColor = Color.Green;
                    else
                        procent_kolacja.ForeColor = Color.Red;
                    procent_kolacja.Text += " %";
                }
            }
            else
            {
                procent_sniadanie.Text = "0 %";
                procent_IIsniadanie.Text = "0 %";
                procent_obiad.Text = "0 %";
                procent_podwieczorek.Text = "0 %";
                procent_kolacja.Text = "0 %";
            }


            //WARTOŚCI
            e_text.Text = Math.Round(suma[5, 0], 2).ToString();
            b_text.Text = Math.Round(suma[5, 1], 2).ToString();
            t_text.Text = Math.Round(suma[5, 2], 2).ToString();
            k_text.Text = Math.Round(suma[5, 3], 2).ToString();
            w_text.Text = Math.Round(suma[5, 4], 2).ToString();
            wp_text.Text = Math.Round(suma[5, 5], 2).ToString();
            c_text.Text = Math.Round(suma[5, 6], 2).ToString();
            bp_text.Text = Math.Round(suma[5, 7], 2).ToString();
            s_text.Text = Math.Round(suma[5, 8], 2).ToString();
            sol_text.Text = (Math.Round(suma[5, 8] * 0.0025, 2)).ToString();


            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (suma[i, 0] != 0)
                    {
                        double wartosc_odzywcza = suma[i, k];
                        double przelicznik = 0;
                        if (k == 1)
                            przelicznik = przelicznik_Bialko;
                        if (k == 2 || k == 3)
                            przelicznik = przelicznik_Tluszcze;
                        if (k == 4 || k == 5 || k == 6 || k == 7)
                            przelicznik = przelicznik_Weglowodany;

                        procent[i, k] = (wartosc_odzywcza * przelicznik * 100.0) / suma[i, 0];
                    }
                }
            }


            //PROCENTY
            double bialkoProcent = Math.Round(procent[5, 1], 2);
            bialko_procent.Text = bialkoProcent.ToString();
            bialko_procent.ForeColor = Color.DarkGray;

            double tluszczeProcent = Math.Round(procent[5, 2], 2);
            tluszcze_procent.Text = tluszczeProcent.ToString();
            tluszcze_procent.ForeColor = Color.DarkGray;

            double kwasyProcent = Math.Round(procent[5, 3], 2);
            kwasy_procent.Text = kwasyProcent.ToString();
            kwasy_procent.ForeColor = Color.DarkGray;

            double wegleProcent = Math.Round(procent[5, 4], 2);
            wegle_procent.Text = wegleProcent.ToString();
            wegle_procent.ForeColor = Color.DarkGray;

            double przyswajalneProcent = Math.Round(procent[5, 5], 2);
            przyswajalne_procent.Text = przyswajalneProcent.ToString();
            przyswajalne_procent.ForeColor = Color.DarkGray;

            double cukryProcent = Math.Round(procent[5, 6], 2);
            cukry_procent.Text = cukryProcent.ToString();
            cukry_procent.ForeColor = Color.DarkGray;

            double blonnikProcent = Math.Round(procent[5, 7], 2);
            blonnik_procent.Text = blonnikProcent.ToString();
            blonnik_procent.ForeColor = Color.DarkGray;


            //NA TYSIAC
            double bialkoNaTysiac = Math.Round(suma[5, 1] * 1000.0 / suma[5, 0], 2);
            bialko_tysiac.Text = bialkoNaTysiac.ToString();
            bialko_tysiac.ForeColor = Color.DarkGray;

            double tluszczeNaTysiac = Math.Round(suma[5, 2] * 1000.0 / suma[5, 0], 2);
            tluszcze_tysiac.Text = tluszczeNaTysiac.ToString();
            tluszcze_tysiac.ForeColor = Color.DarkGray;

            double kwasyNaTysiac = Math.Round(suma[5, 3] * 1000.0 / suma[5, 0], 2);
            kwasy_tysiac.Text = kwasyNaTysiac.ToString();
            kwasy_tysiac.ForeColor = Color.DarkGray;

            double wegleNaTysiac = Math.Round(suma[5, 4] * 1000.0 / suma[5, 0], 2);
            wegle_tysiac.Text = wegleNaTysiac.ToString();
            wegle_tysiac.ForeColor = Color.DarkGray;

            double przyswajalneNaTysiac = Math.Round(suma[5, 5] * 1000.0 / suma[5, 0], 2);
            przyswajalne_tysiac.Text = przyswajalneNaTysiac.ToString();
            przyswajalne_tysiac.ForeColor = Color.DarkGray;

            double cukryNaTysiac = Math.Round(suma[5, 6] * 1000.0 / suma[5, 0], 2);
            cukry_tysiac.Text = cukryNaTysiac.ToString();
            cukry_tysiac.ForeColor = Color.DarkGray;

            double blonnikNaTysiac = Math.Round(suma[5, 7] * 1000.0 / suma[5, 0], 2);
            blonnik_tysiac.Text = blonnikNaTysiac.ToString();
            blonnik_tysiac.ForeColor = Color.DarkGray;

            //ZAWARTOSC
            try
            {
                if (cb_dieta.SelectedIndex != -1)
                {
                    if (Diety[cb_dieta.SelectedIndex].energiaDo != 0)
                    {
                        energia_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].energiaOd.ToString()} - {Diety[cb_dieta.SelectedIndex].energiaDo.ToString()}";
                        if (suma[5, 0] > Diety[cb_dieta.SelectedIndex].energiaDo)
                        {
                            plus_energia.Text = "+ " + Math.Round(suma[5, 0] - Diety[cb_dieta.SelectedIndex].energiaDo, 2);
                            if(suma[5, 0] > Diety[cb_dieta.SelectedIndex].energiaDo * 1.1)
                                plus_energia.ForeColor = Color.Red;
                            else
                                plus_energia.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 0] < Diety[cb_dieta.SelectedIndex].energiaOd)
                        {
                            plus_energia.Text =  Math.Round(suma[5, 0] - Diety[cb_dieta.SelectedIndex].energiaOd, 2).ToString();
                            if (suma[5, 0] < Diety[cb_dieta.SelectedIndex].energiaOd * 0.9)
                                plus_energia.ForeColor = Color.Red;
                            else
                                plus_energia.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_energia.Text = "OK";
                            plus_energia.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_energia.Text = "";
                        energia_zakres.Text = "";
                        plus_energia.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].bialkoDo != 0)
                    {
                        bialko_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].bialkoOd.ToString()} - {Diety[cb_dieta.SelectedIndex].bialkoDo.ToString()}";
                        if (suma[5, 1] > Diety[cb_dieta.SelectedIndex].bialkoDo)
                        {
                            plus_bialko.Text = "+ " + Math.Round(suma[5, 1] - Diety[cb_dieta.SelectedIndex].bialkoDo, 2);
                            if (suma[5, 1] > Diety[cb_dieta.SelectedIndex].bialkoDo * 1.1)
                                plus_bialko.ForeColor = Color.Red;
                            else
                                plus_bialko.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 1] < Diety[cb_dieta.SelectedIndex].bialkoOd)
                        {
                            plus_bialko.Text =  Math.Round(suma[5, 1] - Diety[cb_dieta.SelectedIndex].bialkoOd, 2).ToString();
                            if (suma[5, 1] < Diety[cb_dieta.SelectedIndex].bialkoOd * 0.9)
                                plus_bialko.ForeColor = Color.Red;
                            else
                                plus_bialko.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_bialko.Text = "OK";
                            plus_bialko.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_bialko.Text = "";
                        bialko_zakres.Text = "";
                        plus_bialko.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].bialkoDoNaTysiąc != 0)
                    {
                        bialko_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].bialkoOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].bialkoDoNaTysiąc}";
                        bialko_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        bialko_tysiac.Text = "";
                        bialko_tysiac_zakres.Text = "";
                        bialko_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].bialkoProcentDo != 0)
                    {
                        bialko_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].bialkoProcentOd} - {Diety[cb_dieta.SelectedIndex].bialkoProcentDo} % kcal";
                        if (bialkoProcent > Diety[cb_dieta.SelectedIndex].bialkoProcentDo * 1.1)
                        {
                            bialko_procent.ForeColor = Color.Red;
                        }
                        else if (bialkoProcent > Diety[cb_dieta.SelectedIndex].bialkoProcentDo)
                        {
                            bialko_procent.ForeColor = Color.Orange;
                        }
                        else if (bialkoProcent < Diety[cb_dieta.SelectedIndex].bialkoProcentOd * 0.9)
                        {
                            bialko_procent.ForeColor = Color.Red;
                        }
                        else if (bialkoProcent < Diety[cb_dieta.SelectedIndex].bialkoProcentOd)
                        {
                            bialko_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            bialko_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        bialko_procent.Text = "";
                        bialko_procent_zakres.Text = "";
                        bialko_procent_zakres.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].tluszczeDo != 0)
                    {
                        tluszcze_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].tluszczeOd.ToString()} - {Diety[cb_dieta.SelectedIndex].tluszczeDo.ToString()}";
                        if (suma[5, 2] > Diety[cb_dieta.SelectedIndex].tluszczeDo)
                        {
                            plus_tluszcze.Text = "+ " + Math.Round(suma[5, 2] - Diety[cb_dieta.SelectedIndex].tluszczeDo, 2);
                            if (suma[5, 2] > Diety[cb_dieta.SelectedIndex].tluszczeDo * 1.1)
                                plus_tluszcze.ForeColor = Color.Red;
                            else
                                plus_tluszcze.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 2] < Diety[cb_dieta.SelectedIndex].tluszczeOd)
                        {
                            plus_tluszcze.Text =  Math.Round(suma[5, 2] - Diety[cb_dieta.SelectedIndex].tluszczeOd, 2).ToString();
                            if (suma[5, 2] < Diety[cb_dieta.SelectedIndex].tluszczeOd * 0.9)
                                plus_tluszcze.ForeColor = Color.Red;
                            else
                                plus_tluszcze.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_tluszcze.Text = "OK";
                            plus_tluszcze.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_tluszcze.Text = "";
                        tluszcze_zakres.Text = "";
                        plus_tluszcze.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].tluszczeDoNaTysiąc != 0)
                    {
                        tluszcze_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].tluszczeOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].tluszczeDoNaTysiąc}";
                        t_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        tluszcze_tysiac.Text = "";
                        tluszcze_tysiac_zakres.Text = "";
                        t_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].tluszczeProcentDo != 0)
                    {
                        tluszcze_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].tluszczeProcentOd} - {Diety[cb_dieta.SelectedIndex].tluszczeProcentDo} % kcal";
                        if (tluszczeProcent > Diety[cb_dieta.SelectedIndex].tluszczeProcentDo * 1.1)
                        {
                            tluszcze_procent.ForeColor = Color.Red;
                        }
                        else if (tluszczeProcent > Diety[cb_dieta.SelectedIndex].tluszczeProcentDo)
                        {
                            tluszcze_procent.ForeColor = Color.Orange;
                        }
                        else if (tluszczeProcent < Diety[cb_dieta.SelectedIndex].tluszczeProcentOd * 0.9)
                        {
                            tluszcze_procent.ForeColor = Color.Red;
                        }
                        else if (tluszczeProcent < Diety[cb_dieta.SelectedIndex].tluszczeProcentOd)
                        {
                            tluszcze_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            tluszcze_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        tluszcze_procent.Text = "";
                        tluszcze_procent_zakres.Text = "";
                        tluszcze_procent.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].kwasyDo != 0)
                    {
                        kwasy_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].kwasyOd.ToString()} - {Diety[cb_dieta.SelectedIndex].kwasyDo.ToString()}";
                        if (suma[5, 3] > Diety[cb_dieta.SelectedIndex].kwasyDo)
                        {
                            plus_kwasy.Text = "+ " + Math.Round(suma[5, 3] - Diety[cb_dieta.SelectedIndex].kwasyDo, 2);
                            if (suma[5, 3] > Diety[cb_dieta.SelectedIndex].kwasyDo * 1.1)
                                plus_kwasy.ForeColor = Color.Red;
                            else
                                plus_kwasy.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 3] < Diety[cb_dieta.SelectedIndex].kwasyOd)
                        {
                            plus_kwasy.Text =  Math.Round(suma[5, 3] - Diety[cb_dieta.SelectedIndex].kwasyOd, 2).ToString();
                            if (suma[5, 3] < Diety[cb_dieta.SelectedIndex].kwasyOd * 0.9)
                                plus_kwasy.ForeColor = Color.Red;
                            else
                                plus_kwasy.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_kwasy.Text = "OK";
                            plus_kwasy.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_kwasy.Text = "";
                        kwasy_zakres.Text = "";
                        plus_kwasy.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].kwasyDoNaTysiąc != 0)
                    {
                        kwasy_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].kwasyOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].kwasyDoNaTysiąc}";
                        k_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        kwasy_tysiac.Text = "";
                        kwasy_tysiac_zakres.Text = "";
                        k_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].kwasyProcentDo != 0)
                    {
                        kwasy_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].kwasyProcentOd} - {Diety[cb_dieta.SelectedIndex].kwasyProcentDo} % kcal";
                        if (kwasyProcent > Diety[cb_dieta.SelectedIndex].kwasyProcentDo * 1.1)
                        {
                            kwasy_procent.ForeColor = Color.Red;
                        }
                        else if (kwasyProcent > Diety[cb_dieta.SelectedIndex].kwasyProcentDo)
                        {
                            kwasy_procent.ForeColor = Color.Orange;
                        }
                        else if (kwasyProcent < Diety[cb_dieta.SelectedIndex].kwasyProcentOd * 0.9)
                        {
                            kwasy_procent.ForeColor = Color.Red;
                        }
                        else if (kwasyProcent < Diety[cb_dieta.SelectedIndex].kwasyProcentOd)
                        {
                            kwasy_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            kwasy_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        kwasy_procent.Text = "";
                        kwasy_procent_zakres.Text = "";
                        kwasy_procent.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].wegleDo != 0)
                    {
                        wegle_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].wegleOd.ToString()} - {Diety[cb_dieta.SelectedIndex].wegleDo.ToString()}";
                        if (suma[5, 4] > Diety[cb_dieta.SelectedIndex].wegleDo)
                        {
                            plus_wegle.Text = "+ " + Math.Round(suma[5, 4] - Diety[cb_dieta.SelectedIndex].wegleDo, 2);
                            if (suma[5, 4] > Diety[cb_dieta.SelectedIndex].wegleDo * 1.1)
                                plus_wegle.ForeColor = Color.Red;
                            else
                                plus_wegle.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 4] < Diety[cb_dieta.SelectedIndex].wegleOd)
                        {
                            plus_wegle.Text =  Math.Round(suma[5, 4] - Diety[cb_dieta.SelectedIndex].wegleOd, 2).ToString();
                            if (suma[5, 4] < Diety[cb_dieta.SelectedIndex].wegleOd * 0.9)
                                plus_wegle.ForeColor = Color.Red;
                            else
                                plus_wegle.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_wegle.Text = "OK";
                            plus_wegle.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_wegle.Text = "";
                        wegle_zakres.Text = "";
                        plus_wegle.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].wegleDoNaTysiąc != 0)
                    {
                        wegle_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].wegleOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].wegleDoNaTysiąc}";
                        w_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        wegle_tysiac.Text = "";
                        wegle_tysiac_zakres.Text = "";
                        w_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].wegleProcentDo != 0)
                    {
                        wegle_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].wegleProcentOd} - {Diety[cb_dieta.SelectedIndex].wegleProcentDo} % kcal";
                        if (wegleProcent > Diety[cb_dieta.SelectedIndex].wegleProcentDo * 1.1)
                        {
                            wegle_procent.ForeColor = Color.Red;
                        }
                        else if (wegleProcent > Diety[cb_dieta.SelectedIndex].wegleProcentDo)
                        {
                            wegle_procent.ForeColor = Color.Orange;
                        }
                        else if (wegleProcent < Diety[cb_dieta.SelectedIndex].wegleProcentOd * 0.9)
                        {
                            wegle_procent.ForeColor = Color.Red;
                        }
                        else if (wegleProcent < Diety[cb_dieta.SelectedIndex].wegleProcentOd)
                        {
                            wegle_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            wegle_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        wegle_procent.Text = "";
                        wegle_procent_zakres.Text = "";
                        wegle_procent.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].przyswajalneDo != 0)
                    {
                        przyswajalne_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].przyswajalneOd.ToString()} - {Diety[cb_dieta.SelectedIndex].przyswajalneDo.ToString()}";
                        if (suma[5, 5] > Diety[cb_dieta.SelectedIndex].przyswajalneDo)
                        {
                            plus_przyswajalne.Text = "+ " + Math.Round(suma[5, 5] - Diety[cb_dieta.SelectedIndex].przyswajalneDo, 2);
                            if (suma[5, 5] > Diety[cb_dieta.SelectedIndex].przyswajalneDo * 1.1)
                                plus_przyswajalne.ForeColor = Color.Red;
                            else
                                plus_przyswajalne.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 5] < Diety[cb_dieta.SelectedIndex].przyswajalneOd)
                        {
                            plus_przyswajalne.Text =  Math.Round(suma[5, 5] - Diety[cb_dieta.SelectedIndex].przyswajalneOd, 2).ToString();
                            if (suma[5, 5] < Diety[cb_dieta.SelectedIndex].przyswajalneOd * 0.9)
                                plus_przyswajalne.ForeColor = Color.Red;
                            else
                                plus_przyswajalne.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_przyswajalne.Text = "OK";
                            plus_przyswajalne.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_przyswajalne.Text = "";
                        przyswajalne_zakres.Text = "";
                        plus_przyswajalne.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].przyswajalneDoNaTysiąc != 0)
                    {
                        przyswajalne_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].przyswajalneOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].przyswajalneDoNaTysiąc}";
                        p_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        przyswajalne_tysiac.Text = "";
                        przyswajalne_tysiac_zakres.Text = "";
                        p_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].przyswajalneProcentDo != 0)
                    {
                        przyswajalne_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].przyswajalneProcentOd} - {Diety[cb_dieta.SelectedIndex].przyswajalneProcentDo} % kcal";
                        if (przyswajalneProcent > Diety[cb_dieta.SelectedIndex].przyswajalneProcentDo * 1.1)
                        {
                            przyswajalne_procent.ForeColor = Color.Red;
                        }
                        else if (przyswajalneProcent > Diety[cb_dieta.SelectedIndex].przyswajalneProcentDo)
                        {
                            przyswajalne_procent.ForeColor = Color.Orange;
                        }
                        else if (przyswajalneProcent < Diety[cb_dieta.SelectedIndex].przyswajalneProcentOd * 0.9)
                        {
                            przyswajalne_procent.ForeColor = Color.Red;
                        }
                        else if (przyswajalneProcent < Diety[cb_dieta.SelectedIndex].przyswajalneProcentOd)
                        {
                            przyswajalne_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            przyswajalne_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        przyswajalne_procent.Text = "";
                        przyswajalne_procent_zakres.Text = "";
                        przyswajalne_procent.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].cukryDo != 0)
                    {
                        cukry_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].cukryOd.ToString()} - {Diety[cb_dieta.SelectedIndex].cukryDo.ToString()}";
                        if (suma[5, 6] > Diety[cb_dieta.SelectedIndex].cukryDo)
                        {
                            plus_cukry.Text = "+ " + Math.Round(suma[5, 6] - Diety[cb_dieta.SelectedIndex].cukryDo, 2);
                            if (suma[5, 6] > Diety[cb_dieta.SelectedIndex].cukryDo * 1.1)
                                plus_cukry.ForeColor = Color.Red;
                            else
                                plus_cukry.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 6] < Diety[cb_dieta.SelectedIndex].cukryOd)
                        {
                            plus_cukry.Text =  Math.Round(suma[5, 6] - Diety[cb_dieta.SelectedIndex].cukryOd, 2).ToString();
                            if (suma[5, 6] < Diety[cb_dieta.SelectedIndex].cukryOd * 0.9)
                                plus_cukry.ForeColor = Color.Red;
                            else
                                plus_cukry.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_cukry.Text = "OK";
                            plus_cukry.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_cukry.Text = "";
                        cukry_zakres.Text = "";
                        plus_cukry.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].cukryDoNaTysiąc != 0)
                    {
                        cukry_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].cukryOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].cukryDoNaTysiąc}";
                        c_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        cukry_tysiac.Text = "";
                        cukry_tysiac_zakres.Text = "";
                        c_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].cukryProcentDo != 0)
                    {
                        cukry_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].cukryProcentOd} - {Diety[cb_dieta.SelectedIndex].cukryProcentDo} % kcal";
                        if (cukryProcent > Diety[cb_dieta.SelectedIndex].cukryProcentDo * 1.1)
                        {
                            cukry_procent.ForeColor = Color.Red;
                        }
                        else if (cukryProcent > Diety[cb_dieta.SelectedIndex].cukryProcentDo)
                        {
                            cukry_procent.ForeColor = Color.Orange;
                        }
                        else if (cukryProcent < Diety[cb_dieta.SelectedIndex].cukryProcentOd * 0.9)
                        {
                            cukry_procent.ForeColor = Color.Red;
                        }
                        else if (cukryProcent < Diety[cb_dieta.SelectedIndex].cukryProcentOd)
                        {
                            cukry_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            cukry_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        cukry_procent.Text = "";
                        cukry_procent_zakres.Text = "";
                        cukry_procent.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].blonnikDo != 0)
                    {
                        blonnik_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].blonnikOd.ToString()} - {Diety[cb_dieta.SelectedIndex].blonnikDo.ToString()}";
                        if (suma[5, 7] > Diety[cb_dieta.SelectedIndex].blonnikDo)
                        {
                            plus_blonnik.Text = "+ " + Math.Round(suma[5, 7] - Diety[cb_dieta.SelectedIndex].blonnikDo, 2);
                            if (suma[5, 7] > Diety[cb_dieta.SelectedIndex].blonnikDo * 1.1)
                                plus_blonnik.ForeColor = Color.Red;
                            else
                                plus_blonnik.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 7] < Diety[cb_dieta.SelectedIndex].blonnikOd)
                        {
                            plus_blonnik.Text =  Math.Round(suma[5, 7] - Diety[cb_dieta.SelectedIndex].blonnikOd, 2).ToString();
                            if (suma[5, 7] < Diety[cb_dieta.SelectedIndex].blonnikOd * 0.9)
                                plus_blonnik.ForeColor = Color.Red;
                            else
                                plus_blonnik.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_blonnik.Text = "OK";
                            plus_blonnik.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_blonnik.Text = "";
                        blonnik_zakres.Text = "";
                        plus_blonnik.ForeColor = Color.DarkGray;
                    }

                    if (Diety[cb_dieta.SelectedIndex].blonnikDoNaTysiąc != 0)
                    {
                        blonnik_tysiac_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].blonnikOdNaTysiąc} - {Diety[cb_dieta.SelectedIndex].blonnikDoNaTysiąc}";
                        blonnik_label.Text = "na 1000 kcal";
                    }
                    else
                    {
                        blonnik_tysiac.Text = "";
                        blonnik_tysiac_zakres.Text = "";
                        blonnik_label.Text = "";
                    }

                    if (Diety[cb_dieta.SelectedIndex].blonnikProcentDo != 0)
                    {
                        blonnik_procent_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].blonnikProcentOd} - {Diety[cb_dieta.SelectedIndex].blonnikProcentDo} % kcal";
                        if (blonnikProcent > Diety[cb_dieta.SelectedIndex].blonnikProcentDo * 1.1)
                        {
                            blonnik_procent.ForeColor = Color.Red;
                        }
                        else if (blonnikProcent > Diety[cb_dieta.SelectedIndex].blonnikProcentDo)
                        {
                            blonnik_procent.ForeColor = Color.Orange;
                        }
                        else if (blonnikProcent < Diety[cb_dieta.SelectedIndex].blonnikProcentOd * 0.9)
                        {
                            blonnik_procent.ForeColor = Color.Red;
                        }
                        else if (blonnikProcent < Diety[cb_dieta.SelectedIndex].blonnikProcentOd)
                        {
                            blonnik_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            blonnik_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        blonnik_procent.Text = "";
                        blonnik_procent_zakres.Text = "";
                        blonnik_procent.ForeColor = Color.DarkGray;
                    }


                    if (Diety[cb_dieta.SelectedIndex].sodDo != 0)
                    {
                        sod_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].sodOd.ToString()} - {Diety[cb_dieta.SelectedIndex].sodDo.ToString()}";
                        if (suma[5, 8] > Diety[cb_dieta.SelectedIndex].sodDo)
                        {
                            plus_sod.Text = "+ " + Math.Round(suma[5, 8] - Diety[cb_dieta.SelectedIndex].sodDo, 2);
                            if (suma[5, 8] > Diety[cb_dieta.SelectedIndex].sodDo * 1.1)
                                plus_sod.ForeColor = Color.Red;
                            else
                                plus_sod.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 8] < Diety[cb_dieta.SelectedIndex].sodOd)
                        {
                            plus_sod.Text =  Math.Round(suma[5, 8] - Diety[cb_dieta.SelectedIndex].sodOd, 2).ToString();
                            if (suma[5, 8] < Diety[cb_dieta.SelectedIndex].sodOd * 0.9)
                                plus_sod.ForeColor = Color.Red;
                            else
                                plus_sod.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_sod.Text = "OK";
                            plus_sod.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_sod.Text = "";
                        sod_zakres.Text = "";
                        plus_sod.ForeColor = Color.DarkGray;
                    }


                    if (Diety[cb_dieta.SelectedIndex].solDo != 0)
                    {
                        sol_zakres.Text = $"{Diety[cb_dieta.SelectedIndex].solOd.ToString()} - {Diety[cb_dieta.SelectedIndex].solDo.ToString()}";
                        if (Math.Round(suma[5, 8] * 0.0025, 2) > Diety[cb_dieta.SelectedIndex].solDo)
                        {
                            plus_sol.Text = "+ " + Math.Round(suma[5, 8] * 0.0025 - Diety[cb_dieta.SelectedIndex].solDo, 2);
                            if (Math.Round(suma[5, 8] * 0.0025, 2) > Diety[cb_dieta.SelectedIndex].solDo * 1.1)
                                plus_sol.ForeColor = Color.Red;
                            else
                                plus_sol.ForeColor = Color.Orange;
                        }
                        else if (Math.Round(suma[5, 8] * 0.0025, 2) < Diety[cb_dieta.SelectedIndex].solOd)
                        {
                            plus_sol.Text =  Math.Round(suma[5, 8] * 0.0025 - Diety[cb_dieta.SelectedIndex].solOd, 2).ToString();
                            if (Math.Round(suma[5, 8] * 0.0025, 2) < Diety[cb_dieta.SelectedIndex].solDo * 0.9)
                                plus_sol.ForeColor = Color.Red;
                            else
                                plus_sol.ForeColor = Color.Orange;
                        }
                        else
                        {
                            plus_sol.Text = "OK";
                            plus_sol.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        plus_sol.Text = "";
                        sol_zakres.Text = "";
                        plus_sol.ForeColor = Color.DarkGray;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można przeliczyć wartości, o które przekroczono limity diety.\r\n{ex.Message}", "Błąd");
            }
        }

        private void Kategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int wybor = cb_kategorie.SelectedIndex;

            lb_produkty.BeginUpdate();
            lb_produkty.Items.Clear();
            switch (wybor)
            {
                case 0:
                    Lista.OrderBy(x => x.nazwa);
                    foreach (var v in Lista)
                        lb_produkty.Items.Add(v.nazwa);

                    kategoria = "Wszystkie";
                    break;
                case 1:
                    Bakalie = Lista.Where(x => x.kategoria == 'B').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Bakalie)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "B";
                    break;
                case 2:
                    Mieso = Lista.Where(x => x.kategoria == 'M').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Mieso)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "M";
                    break;
                case 3:
                    Przyprawy = Lista.Where(x => x.kategoria == 'P').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Przyprawy)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "P";
                    break;
                case 4:
                    Nabial = Lista.Where(x => x.kategoria == 'N').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Nabial)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "N";
                    break;
                case 5:
                    Owoce = Lista.Where(x => x.kategoria == 'O').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Owoce)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "O";
                    break;
                case 6:
                    Warzywa = Lista.Where(x => x.kategoria == 'W').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Warzywa)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "W";
                    break;
                case 7:
                    Ryby = Lista.Where(x => x.kategoria == 'R').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Ryby)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "R";
                    break;
                case 8:
                    Tluszcze = Lista.Where(x => x.kategoria == 'T').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Tluszcze)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "T";
                    break;
                case 9:
                    Slodycze = Lista.Where(x => x.kategoria == 'S').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Slodycze)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "S";
                    break;
                case 10:
                    Napoje = Lista.Where(x => x.kategoria == 'D').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Napoje)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "D";
                    break;
                case 11:
                    Zboza = Lista.Where(x => x.kategoria == 'Z').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Zboza)
                    {
                        lb_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "Z";
                    break;
            }
            lb_produkty.EndUpdate();
        }

        private string GetMonthForDate(int month)
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

        private void Edytuj_Click(object sender, EventArgs e)
        {
            try
            {
                int tab = tc_posilki.SelectedIndex;
                switch (tab)
                {
                    case 0:
                        int wybrany = lv_sniadanie.SelectedIndices[0];
                        string[] arr = new string[11];
                        double masa = double.Parse(tb_masa.Text);
                        arr[0] = lv_sniadanie.Items[wybrany].SubItems[0].Text;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[2].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[3] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[3].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[4] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[4].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[5] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[5].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[6] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[6].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[7] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[7].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[8].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[9].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[10] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[10].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        ListViewItem itm = new ListViewItem(arr);

                        lv_sniadanie.Items.Remove(lv_sniadanie.Items[wybrany]);
                        lv_sniadanie.Items.Insert(wybrany, itm);
                        LiczSrednia();
                        break;
                    case 1:
                        wybrany = lv_IIsniadanie.SelectedIndices[0];
                        arr = new string[11];
                        masa = double.Parse(tb_masa.Text);
                        arr[0] = lv_IIsniadanie.Items[wybrany].SubItems[0].Text;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[2].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[3] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[3].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[4] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[4].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[5] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[5].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[6] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[6].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[7] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[7].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[8].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[9].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[10] = Math.Round(masa * double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[10].Text) / double.Parse(lv_IIsniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        itm = new ListViewItem(arr);

                        lv_IIsniadanie.Items.Remove(lv_IIsniadanie.Items[wybrany]);
                        lv_IIsniadanie.Items.Insert(wybrany, itm);
                        LiczSrednia();
                        break;
                    case 2:
                        wybrany = lv_obiad.SelectedIndices[0];
                        arr = new string[11];
                        masa = double.Parse(tb_masa.Text);
                        arr[0] = lv_obiad.Items[wybrany].SubItems[0].Text;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[2].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[3] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[3].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[4] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[4].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[5] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[5].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[6] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[6].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[7] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[7].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[8].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[9].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[10] = Math.Round(masa * double.Parse(lv_obiad.Items[wybrany].SubItems[10].Text) / double.Parse(lv_obiad.Items[wybrany].SubItems[1].Text), 2).ToString();
                        itm = new ListViewItem(arr);

                        lv_obiad.Items.Remove(lv_obiad.Items[wybrany]);
                        lv_obiad.Items.Insert(wybrany, itm);
                        LiczSrednia();
                        break;
                    case 3:
                        wybrany = lv_podwieczorek.SelectedIndices[0];
                        arr = new string[11];
                        masa = double.Parse(tb_masa.Text);
                        arr[0] = lv_podwieczorek.Items[wybrany].SubItems[0].Text;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[2].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[3] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[3].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[4] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[4].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[5] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[5].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[6] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[6].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[7] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[7].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[8].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[9].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[10] = Math.Round(masa * double.Parse(lv_podwieczorek.Items[wybrany].SubItems[10].Text) / double.Parse(lv_podwieczorek.Items[wybrany].SubItems[1].Text), 2).ToString();
                        itm = new ListViewItem(arr);

                        lv_podwieczorek.Items.Remove(lv_podwieczorek.Items[wybrany]);
                        lv_podwieczorek.Items.Insert(wybrany, itm);
                        LiczSrednia();
                        break;
                    case 4:
                        wybrany = lv_kolacja.SelectedIndices[0];
                        arr = new string[11];
                        masa = double.Parse(tb_masa.Text);
                        arr[0] = lv_kolacja.Items[wybrany].SubItems[0].Text;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[2].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[3] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[3].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[4] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[4].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[5] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[5].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[6] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[6].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[7] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[7].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[8].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[9].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[10] = Math.Round(masa * double.Parse(lv_kolacja.Items[wybrany].SubItems[10].Text) / double.Parse(lv_kolacja.Items[wybrany].SubItems[1].Text), 2).ToString();
                        itm = new ListViewItem(arr);

                        lv_kolacja.Items.Remove(lv_kolacja.Items[wybrany]);
                        lv_kolacja.Items.Insert(wybrany, itm);
                        LiczSrednia();
                        break;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można edytować.{ex.Message}.", "Błąd");
            }
        }

        private void Gora_Click(object sender, EventArgs e)
        {
            try
            {
                int tab = tc_posilki.SelectedIndex;

                switch (tab)
                {
                    case 0:
                        int liczba = lv_sniadanie.Items.Count;
                        int wybrany = lv_sniadanie.SelectedIndices[0];
                        if (wybrany > 0)
                        {
                            ListViewItem itm = lv_sniadanie.Items[wybrany];
                            lv_sniadanie.Items.Remove(itm);
                            lv_sniadanie.Items.Insert(wybrany - 1, itm);
                        }
                        break;
                    case 1:
                        int liczba2 = lv_IIsniadanie.Items.Count;
                        int wybrany2 = lv_IIsniadanie.SelectedIndices[0];
                        if (wybrany2 > 0)
                        {
                            ListViewItem itm = lv_IIsniadanie.Items[wybrany2];
                            lv_IIsniadanie.Items.Remove(itm);
                            lv_IIsniadanie.Items.Insert(wybrany2 - 1, itm);
                        }
                        break;
                    case 2:
                        int liczba3 = lv_obiad.Items.Count;
                        int wybrany3 = lv_obiad.SelectedIndices[0];
                        if (wybrany3 > 0)
                        {
                            ListViewItem itm = lv_obiad.Items[wybrany3];
                            lv_obiad.Items.Remove(itm);
                            lv_obiad.Items.Insert(wybrany3 - 1, itm);
                        }
                        break;
                    case 3:
                        int liczba4 = lv_podwieczorek.Items.Count;
                        int wybrany4 = lv_podwieczorek.SelectedIndices[0];
                        if (wybrany4 > 0)
                        {
                            ListViewItem itm = lv_podwieczorek.Items[wybrany4];
                            lv_podwieczorek.Items.Remove(itm);
                            lv_podwieczorek.Items.Insert(wybrany4 - 1, itm);
                        }
                        break;
                    case 4:
                        int liczba5 = lv_kolacja.Items.Count;
                        int wybrany5 = lv_kolacja.SelectedIndices[0];
                        if (wybrany5 > 0)
                        {
                            ListViewItem itm = lv_kolacja.Items[wybrany5];
                            lv_kolacja.Items.Remove(itm);
                            lv_kolacja.Items.Insert(wybrany5 - 1, itm);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można przesunąć.\r\n{ex.Message}.", "Błąd");
            }
        }

        private void Dol_Click(object sender, EventArgs e)
        {
            try
            {
                int tab = tc_posilki.SelectedIndex;

                switch (tab)
                {
                    case 0:
                        int liczba = lv_sniadanie.Items.Count;
                        int wybrany = lv_sniadanie.SelectedIndices[0];
                        if (wybrany < liczba - 1)
                        {
                            ListViewItem itm = lv_sniadanie.Items[wybrany];
                            lv_sniadanie.Items.Remove(itm);
                            lv_sniadanie.Items.Insert(wybrany + 1, itm);
                        }
                        break;
                    case 1:
                        int liczba2 = lv_IIsniadanie.Items.Count;
                        int wybrany2 = lv_IIsniadanie.SelectedIndices[0];
                        if (wybrany2 < liczba2 - 1)
                        {
                            ListViewItem itm = lv_IIsniadanie.Items[wybrany2];
                            lv_IIsniadanie.Items.Remove(itm);
                            lv_IIsniadanie.Items.Insert(wybrany2 + 1, itm);
                        }
                        break;
                    case 2:
                        int liczba3 = lv_obiad.Items.Count;
                        int wybrany3 = lv_obiad.SelectedIndices[0];
                        if (wybrany3 < liczba3 - 1)
                        {
                            ListViewItem itm = lv_obiad.Items[wybrany3];
                            lv_obiad.Items.Remove(itm);
                            lv_obiad.Items.Insert(wybrany3 + 1, itm);
                        }
                        break;
                    case 3:
                        int liczba4 = lv_podwieczorek.Items.Count;
                        int wybrany4 = lv_podwieczorek.SelectedIndices[0];
                        if (wybrany4 < liczba4 - 1)
                        {
                            ListViewItem itm = lv_podwieczorek.Items[wybrany4];
                            lv_podwieczorek.Items.Remove(itm);
                            lv_podwieczorek.Items.Insert(wybrany4 + 1, itm);
                        }
                        break;
                    case 4:
                        int liczba5 = lv_kolacja.Items.Count;
                        int wybrany5 = lv_kolacja.SelectedIndices[0];
                        if (wybrany5 < liczba5 - 1)
                        {
                            ListViewItem itm = lv_kolacja.Items[wybrany5];
                            lv_kolacja.Items.Remove(itm);
                            lv_kolacja.Items.Insert(wybrany5 + 1, itm);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można przesunąć.\r\n{ex.Message}.", "Błąd");
            }
        }

        #endregion

        #region Menu

        private void posiłekToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            switch (tc_posilki.SelectedIndex)
            {
                case 0:
                    textBox1.Text = "";
                    lv_sniadanie.Items.Clear();
                    break;
                case 1:
                    textBox2.Text = "";
                    lv_IIsniadanie.Items.Clear();
                    break;
                case 2:
                    textBox3.Text = "";
                    lv_obiad.Items.Clear();
                    break;
                case 3:
                    textBox4.Text = "";
                    lv_podwieczorek.Items.Clear();
                    break;
                case 4:
                    textBox5.Text = "";
                    lv_kolacja.Items.Clear();
                    break;
            }
            LiczSrednia();
            MessageBox.Show($"Wyczyszczono recepturę: {tc_posilki.SelectedTab.Text}.", "Sukces");
        }

        private void dzieńToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            lv_sniadanie.Items.Clear();
            textBox2.Text = "";
            lv_IIsniadanie.Items.Clear();
            textBox3.Text = "";
            lv_obiad.Items.Clear();
            textBox4.Text = "";
            lv_podwieczorek.Items.Clear();
            textBox5.Text = "";
            lv_kolacja.Items.Clear();
            LiczSrednia();
            MessageBox.Show($"Wyczyszczono dzień: {dateTimePicker1.Text} ({cb_dieta.SelectedItem}).", "Sukces");
        }

        private void dzieńToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show($"Czy na pewno chcesz zapisać dzień: \n{dateTimePicker1.Text}\n{cb_dieta.Text}\n{cb_miasto.Text}", "Potwierdź", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {


                string sklad_sniadanie = "";
                for (int i = 0; i < lv_sniadanie.Items.Count; i++)
                    sklad_sniadanie += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "|" + lv_sniadanie.Items[i].SubItems[8].Text + "|" + lv_sniadanie.Items[i].SubItems[9].Text + "|" + lv_sniadanie.Items[i].SubItems[10].Text + "$";

                string sklad_IIsniadanie = "";
                for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                    sklad_IIsniadanie += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "$";

                string sklad_obiad = "";
                for (int i = 0; i < lv_obiad.Items.Count; i++)
                    sklad_obiad += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "$";

                string sklad_podwieczorek = "";
                for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                    sklad_podwieczorek += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "$";

                string sklad_kolacja = "";
                for (int i = 0; i < lv_kolacja.Items.Count; i++)
                    sklad_kolacja += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "$";


                DAO.JadlospisDAO.Insert(dateTimePicker1.Text, cb_dieta.Text, cb_miasto.SelectedItem.ToString(), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);

                MessageBox.Show($"Zapisano dzień:\n{dateTimePicker1.Text}\n{cb_dieta.Text}\n{cb_miasto.Text}.", "Sukces");

            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void posiłekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            recepturaClick();
            label10.Text = "Receptury -> Wczytaj";
            pictureBox14.Visible = true;
            pictureBox16.Visible = true;
            pictureBox17.Visible = false;
            pictureBox18.Visible = false;
            pictureBox15.Visible = false;
            receptura_posilek.Visible = true;
            receptura_posilek.SelectedIndex = 0;
            label53.Visible = true;
        }

        private void dzieńToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            label10.Text = "Jadłospisy -> Wczytaj";

            pictureBox23.Visible = true;
            pictureBox24.Visible = true;
            pictureBox25.Visible = false;

            panel_jadlospis.Visible = true;
            panel_jadlospis.BringToFront();

            wczytajJadlospis();
        }

        private void posiłekToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            string tekst = "";
            int posilek = tc_posilki.SelectedIndex;
            switch (posilek)
            {
                case 0:
                    tekst = textBox1.Text;
                    break;
                case 1:
                    tekst = textBox2.Text;
                    break;
                case 2:
                    tekst = textBox3.Text;
                    break;
                case 3:
                    tekst = textBox4.Text;
                    break;
                case 4:
                    tekst = textBox5.Text;
                    break;

            }
            DialogResult dialogResult = MessageBox.Show($"Czy na pewno chcesz zapisać: \n{tc_posilki.SelectedTab.ToString().Remove(tc_posilki.SelectedTab.ToString().Length - 1).Remove(0, 10)}\n{tekst}.", "Potwierdź", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DataTable dtProdukty = DataSet.Tables["Receptury"];
                DataRow drProdukty = dtProdukty.NewRow();

                //drProdukty["Nazwa produktu"] = "Piersi z kurczaka";
                //drProdukty["Kategoria"] = "M";
                //drProdukty["Energia"] = 20.0;
                //drProdukty["Białko"] = 20.0;
                //drProdukty["Tłuszcze"] = 20.0;
                //drProdukty["Węglowodany"] = 20.0;
                //drProdukty["Sód"] = 20.0;
                //drProdukty["Kwasy tłuszczowe nasycone"] = 20.0;


                switch (posilek)
                {
                    case 0:
                        drProdukty["Nazwa receptury"] = textBox1.Text;
                        for (int i = 0; i < lv_sniadanie.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "|" + lv_sniadanie.Items[i].SubItems[8].Text + "|" + lv_sniadanie.Items[i].SubItems[9].Text + "|" + lv_sniadanie.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show($"Zapisano recepturę: {textBox1.Text}.", "Sukces");
                        break;
                    case 1:
                        drProdukty["Nazwa receptury"] = textBox2.Text;
                        for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show($"Zapisano recepturę: {textBox2.Text}.", "Sukces");
                        break;
                    case 2:
                        drProdukty["Nazwa receptury"] = textBox3.Text;
                        for (int i = 0; i < lv_obiad.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show($"Zapisano recepturę: {textBox3.Text}.", "Sukces");
                        break;
                    case 3:
                        drProdukty["Nazwa receptury"] = textBox4.Text;
                        for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show($"Zapisano recepturę: {textBox4.Text}.", "Sukces");
                        break;
                    case 4:
                        drProdukty["Nazwa receptury"] = textBox5.Text;
                        for (int i = 0; i < lv_kolacja.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show($"Zapisano recepturę: {textBox5.Text}.", "Sukces");
                        break;
                }

                dtProdukty.Rows.Add(drProdukty);

                DataSet.WriteXml(XML_Location);

            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void KontrolaClick()
        {
            p_k.BackColor = highlightColor;
            p_p.BackColor = primaryColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            label10.Text = "Kontrola";
            panel_kontrola.Visible = true;
            panel_kontrola.BringToFront();

            if (k_miasto.Items.Count == 0)
            {
                listaJednostek = JednostkaDAO.SelectAll();
                k_miasto.BeginUpdate();
                foreach (Jednostka r in listaJednostek)
                    k_miasto.Items.Add(r.miasto);
                k_miasto.EndUpdate();
            }
            k_miasto.SelectedIndex = 0;

            if (k_dieta.Items.Count == 0)
            {
                k_dieta.BeginUpdate();
                Diety = DAO.DietaDAO.SelectAll(k_miasto.SelectedItem.ToString());
                var sortedDiety = Diety
                .OrderBy(d =>
                {
                    int index = Array.IndexOf(DietaPriority, d.nazwa);
                    return index == -1 ? int.MaxValue : index;
                }).ThenBy(d => d.nazwa).ToList();

                foreach (Dieta d in sortedDiety)
                    k_dieta.Items.Add(d.nazwa);
                k_dieta.EndUpdate();
            }
            k_dieta.SelectedIndex = 0;
        }

        private void produktClick()
        {
            p_p.BackColor = highlightColor;
            i_p.BackColor = highlightColor;
            t_p.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            panel_produkty.Visible = true;
            panel_produkty.BringToFront();

            produkt_wstecz_Click(null, null);
        }

        private void recepturaClick()
        {
            p_r.BackColor = highlightColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            panel_receptura.Visible = true;
            panel_receptura.BringToFront();

            pictureBox14_Click(null, null);

        }

        private void jadlospisClick()
        {
            p_j.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            label10.Text = "Jadłospisy";
            panel_jadlospis.Visible = true;
            pictureBox23.Visible = false;
            pictureBox24.Visible = false;
            pictureBox25.Visible = true;
            panel_jadlospis.BringToFront();

            wczytajJadlospis();

            if (jadlospis_miasto.Items.Count > 0)
                jadlospis_miasto.SelectedIndex = 0;

        }

        private void dekadowkaClick()
        {
            p_de.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            p_k.BackColor = primaryColor;

            this.Update();

            label10.Text = "Szablony";
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = true;
            panel_dekadowka.BringToFront();

            dekadowka_miasto.BeginUpdate();
            dekadowka_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                dekadowka_miasto.Items.Add(j.miasto);
            dekadowka_miasto.EndUpdate();

            if (dekadowka_miasto.Items.Count > 0)
                dekadowka_miasto.SelectedIndex = 0;

                dekadowka_panel.SuspendLayout();
                dekadowka_panel.Controls.Clear();

                Label loading = new Label
                {
                    Text = "Wybierz dekadówkę...",
                    AutoSize = true,
                    Font = DietLabelFont
                };
                dekadowka_panel.Controls.Add(loading);
                dekadowka_panel.ResumeLayout();

            dekadowka_nope_Click(null, null);
        }
        private void dietaClick()
        {
            p_d.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            panel_dieta.Visible = true; 
            panel_dieta.BringToFront();

            dieta_wstecz_Click(null, null);

        }

        private void glownaClick()
        {
            p_g.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            label10.Text = "Strona główna";
            panel_glowny.Visible = true;
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = false;
            panel_kontrola.Visible = false;
            panel_dekadowka_zapisz.Visible = false;
            panel_dieta.Visible = false;
            panel_glowny.BringToFront();

            lb_produkty.BeginUpdate();
            lb_produkty.Items.Clear();
            Lista = DAO.ProduktDAO.SelectAll();
            Lista = Lista.OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
            foreach (Produkt p in Lista)
                lb_produkty.Items.Add(p.nazwa);
            lb_produkty.EndUpdate();
            cb_kategorie.SelectedIndex = 0;

            cb_miasto.BeginUpdate();
            cb_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                cb_miasto.Items.Add(j.miasto);
            cb_miasto.EndUpdate();
            if (cb_miasto.Items.Count > 0)
                cb_miasto.SelectedIndex = 0;

            try { cb_miasto.SelectedIndex = wybraneMiasto; }
            catch
            {
                if (cb_miasto.Items.Count > 0)
                    cb_miasto.SelectedIndex = 0;
            }
            LiczSrednia();
        }
        private void jednostkaClick()
        {
            p_h.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_pr.BackColor = primaryColor;
            i_p.BackColor = primaryColor;
            t_p.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            this.Update();

            label10.Text = "Jednostki";
            panel_jednostka.Visible = true;
            panel_jednostka.BringToFront();

            jednostka_wstecz_Click(null, null);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            produktClick();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            produktClick();
        }
        private void panel10_Click(object sender, EventArgs e)
        {
            glownaClick();
        }


        private void pictureBox6_Click(object sender, EventArgs e)
        {
            produktClick();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            recepturaClick();
        }
        private void panel5_Click(object sender, EventArgs e)
        {
            recepturaClick();
        }
        private void panel6_Click(object sender, EventArgs e)
        {
            jadlospisClick();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            dekadowkaClick();
        }
        private void panel8_Click(object sender, EventArgs e)
        {
            dietaClick();
        }
        private void panel9_Click(object sender, EventArgs e)
        {
            jednostkaClick();
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            recepturaClick();
        }
        private void label13_Click(object sender, EventArgs e)
        {
            jadlospisClick();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            jadlospisClick();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            dekadowkaClick();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            dekadowkaClick();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            dietaClick();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            dietaClick();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            jednostkaClick();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            jednostkaClick();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            glownaClick();
        }


        private void panel_produkty_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Produkt
        private void produkt_przelicz_Click(object sender, EventArgs e)
        {
            try
            {
                if (produkt_sol.Text != "")
                    produkt_sod.Text = (Double.Parse(produkt_sol.Text) / 0.0025).ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Błąd przeliczania.\r\n{ex.Message}", "Błąd");
            }
        }

        private void produkt_sol_TextChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void produkt_edytuj_Click(object sender, EventArgs e)
        {
            produkt_nazwa.Enabled = true;
            produkt_nazwa.BackColor = Color.White;
            produkt_kategoria.Enabled = true;
            produkt_energia.Enabled = true;
            produkt_energia.BackColor = Color.White;
            produkt_bialko.Enabled = true;
            produkt_bialko.BackColor = Color.White;
            produkt_tluszcze.Enabled = true;
            produkt_tluszcze.BackColor = Color.White;
            produkt_weglowodany.Enabled = true;
            produkt_weglowodany.BackColor = Color.White;
            produkty_cukry.Enabled = true;
            produkty_cukry.BackColor = Color.White;
            produkt_tluszcze_nn.Enabled = true;
            produkt_tluszcze_nn.BackColor = Color.White;
            produkt_sod.Enabled = true;
            produkt_sod.BackColor = Color.White;
            lbl_sol.Visible = true;
            produkty_przyswajalne.Enabled = true;
            produkty_przyswajalne.BackColor = Color.White;
            produkty_blonnik.Enabled = true;
            produkty_blonnik.BackColor = Color.White;
            produkt_sol.Visible = true;
            produkt_przelicz.Visible = true;
            produkt_sol.Text = "";

            produkt_edytuj.Visible = false;
            produkt_usun.Visible = false;
            produkt_dodaj.Visible = false;
            produkt_wstecz.Visible = true;
            produkt_zapisz.Visible = true;

            label10.Text = "Produkty -> Edytuj";
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć ten produkt?", "Potwierdź", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.ProduktDAO.Delete(Lista[produkt_wczytaj.SelectedIndex]);
                    MessageBox.Show($"Usunięto: {Lista[produkt_wczytaj.SelectedIndex].nazwa}.", "Sukces");
                    produktClick();
                    break;
                default:
                    break;
            }

        }

        private void produkt_wstecz_Click(object sender, EventArgs e)
        {
            produkt_nazwa.Enabled = false;
            produkt_nazwa.BackColor = Color.FromName("ControlLight");
            produkty_przyswajalne.Enabled = false;
            produkty_przyswajalne.BackColor = Color.FromName("ControlLight");
            produkty_cukry.Enabled = false;
            produkty_cukry.BackColor = Color.FromName("ControlLight");
            produkty_blonnik.Enabled = false;
            produkty_blonnik.BackColor = Color.FromName("ControlLight");
            produkt_kategoria.Enabled = false;
            produkt_energia.Enabled = false;
            produkt_energia.BackColor = Color.FromName("ControlLight");
            produkt_bialko.Enabled = false;
            produkt_bialko.BackColor = Color.FromName("ControlLight");
            produkt_tluszcze.Enabled = false;
            produkt_tluszcze.BackColor = Color.FromName("ControlLight");
            produkt_weglowodany.Enabled = false;
            produkt_weglowodany.BackColor = Color.FromName("ControlLight");
            produkt_tluszcze_nn.Enabled = false;
            produkt_tluszcze_nn.BackColor = Color.FromName("ControlLight");
            produkt_sod.Enabled = false;
            produkt_sod.BackColor = Color.FromName("ControlLight");
            lbl_sol.Visible = false;
            produkt_sol.Visible = false;
            produkt_przelicz.Visible = false;

            produkt_edytuj.Visible = true;
            produkt_usun.Visible = true;
            produkt_dodaj.Visible = true;
            produkt_wstecz.Visible = false;
            produkt_zapisz.Visible = false;

            label27.Visible = true;
            produkt_wczytaj.Visible = true;

            Lista = DAO.ProduktDAO.SelectAll();

            produkt_wczytaj.BeginUpdate();
            produkt_kategoria.BeginUpdate();
            produkt_wczytaj.Items.Clear();
            produkt_kategoria.Items.Clear();

            Lista = Lista.OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
            for (int i = 0; i < Lista.Count; i++)
            {
                produkt_wczytaj.Items.Add(Lista[i].nazwa);
            }
            produkt_wczytaj.EndUpdate();

            produkt_kategoria.Items.Add("Bakalie, orzechy, ziarna");
            produkt_kategoria.Items.Add("Mięso");
            produkt_kategoria.Items.Add("Przyprawy, zioła");
            produkt_kategoria.Items.Add("Nabiał, jaja");
            produkt_kategoria.Items.Add("Owoce");
            produkt_kategoria.Items.Add("Warzywa");
            produkt_kategoria.Items.Add("Ryby, owoce morza");
            produkt_kategoria.Items.Add("Tłuszcze");
            produkt_kategoria.Items.Add("Słodycze");
            produkt_kategoria.Items.Add("Napoje");
            produkt_kategoria.Items.Add("Zbożowe");
            produkt_kategoria.EndUpdate();

            produkt_wczytaj.SelectedIndex = 0;
            produkt_wczytaj_SelectedIndexChanged(sender, e);

            label10.Text = "Produkty";
        }

        private void produkt_dodaj_Click(object sender, EventArgs e)
        {
            label27.Visible = false;
            produkt_wczytaj.Visible = false;

            produkt_nazwa.Enabled = true;
            produkt_nazwa.BackColor = Color.White;
            produkt_kategoria.Enabled = true;
            produkt_energia.Enabled = true;
            produkt_energia.BackColor = Color.White;
            produkt_bialko.Enabled = true;
            produkt_bialko.BackColor = Color.White;
            produkt_tluszcze.Enabled = true;
            produkt_tluszcze.BackColor = Color.White;
            produkt_weglowodany.Enabled = true;
            produkt_weglowodany.BackColor = Color.White;
            produkty_cukry.Enabled = true;
            produkty_cukry.BackColor = Color.White;
            produkty_przyswajalne.Enabled = true;
            produkty_przyswajalne.BackColor = Color.White;
            produkty_blonnik.Enabled = true;
            produkty_blonnik.BackColor = Color.White;
            produkt_tluszcze_nn.Enabled = true;
            produkt_tluszcze_nn.BackColor = Color.White;
            produkt_sod.Enabled = true;
            produkt_sod.BackColor = Color.White;
            lbl_sol.Visible = true;
            produkt_sol.Visible = true;
            produkt_przelicz.Visible = true;

            produkt_edytuj.Visible = false;
            produkt_usun.Visible = false;
            produkt_dodaj.Visible = false;
            produkt_wstecz.Visible = true;
            produkt_zapisz.Visible = true;

            produkt_nazwa.Text = "";
            produkt_kategoria.SelectedIndex = 0;
            produkt_energia.Text = "";
            produkt_bialko.Text = "";
            produkt_weglowodany.Text = "";
            produkt_tluszcze.Text = "";
            produkt_sod.Text = "";
            produkt_tluszcze_nn.Text = "";
            produkt_sol.Text = "";
            produkty_blonnik.Text = "";
            produkty_przyswajalne.Text = "";
            produkty_cukry.Text = "";

            label10.Text = "Produkty -> Dodaj";
        }

        private void produkt_zapisz_Click(object sender, EventArgs e)
        {
            char kategoria;
            switch (label10.Text)
            {
                case "Produkty -> Dodaj":
                    try
                    {
                        if (produkt_kategoria.SelectedIndex != -1 && produkt_nazwa.Text != "" && produkt_energia.Text != "" && produkt_bialko.Text != "" && produkt_tluszcze_nn.Text != "" && produkt_tluszcze.Text != "" && produkt_weglowodany.Text != "" && produkty_przyswajalne.Text != "" && produkty_blonnik.Text != "" && produkt_sod.Text != "" && produkty_cukry.Text != "")
                        {
                            kategoria = 'A';
                            switch (produkt_kategoria.SelectedIndex)
                            {
                                case 0:
                                    kategoria = 'B';
                                    break;
                                case 1:
                                    kategoria = 'M';
                                    break;
                                case 2:
                                    kategoria = 'P';
                                    break;
                                case 3:
                                    kategoria = 'N';
                                    break;
                                case 4:
                                    kategoria = 'O';
                                    break;
                                case 5:
                                    kategoria = 'W';
                                    break;
                                case 6:
                                    kategoria = 'R';
                                    break;
                                case 7:
                                    kategoria = 'T';
                                    break;
                                case 8:
                                    kategoria = 'S';
                                    break;
                                case 9:
                                    kategoria = 'D';
                                    break;
                                case 10:
                                    kategoria = 'Z';
                                    break;
                            }
                            DAO.ProduktDAO.Insert(produkt_nazwa.Text, kategoria, Convert.ToDouble(produkt_energia.Text), Convert.ToDouble(produkt_bialko.Text), Convert.ToDouble(produkt_tluszcze.Text), Convert.ToDouble(produkt_weglowodany.Text), Convert.ToDouble(produkt_sod.Text), Convert.ToDouble(produkt_tluszcze_nn.Text), Convert.ToDouble(produkty_przyswajalne.Text), Convert.ToDouble(produkty_blonnik.Text), Convert.ToDouble(produkty_cukry.Text));
                            MessageBox.Show($"Dodano: {produkt_nazwa.Text}.", "Sukces");
                            produktClick();
                        }
                        else
                        {
                            MessageBox.Show("Nie uzupełniono wszystkich danych.", "Błąd");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Błąd dodawania produktu.\r\n{ex.Message}.", "Błąd");
                    }
                    break;
                case "Produkty -> Edytuj":
                    try
                    {
                        if (produkt_kategoria.SelectedIndex != -1 && produkt_nazwa.Text != "" && produkt_energia.Text != "" && produkt_bialko.Text != "" && produkt_tluszcze_nn.Text != "" && produkt_tluszcze.Text != "" && produkt_weglowodany.Text != "" && produkty_przyswajalne.Text != "" && produkty_blonnik.Text != "" && produkt_sod.Text != "")
                        {
                            kategoria = 'A';
                            switch (produkt_kategoria.SelectedIndex)
                            {
                                case 0:
                                    kategoria = 'B';
                                    break;
                                case 1:
                                    kategoria = 'M';
                                    break;
                                case 2:
                                    kategoria = 'P';
                                    break;
                                case 3:
                                    kategoria = 'N';
                                    break;
                                case 4:
                                    kategoria = 'O';
                                    break;
                                case 5:
                                    kategoria = 'W';
                                    break;
                                case 6:
                                    kategoria = 'R';
                                    break;
                                case 7:
                                    kategoria = 'T';
                                    break;
                                case 8:
                                    kategoria = 'S';
                                    break;
                                case 9:
                                    kategoria = 'D';
                                    break;
                                case 10:
                                    kategoria = 'Z';
                                    break;
                            }
                            DAO.ProduktDAO.Update(Lista[produkt_wczytaj.SelectedIndex], produkt_nazwa.Text, kategoria, Convert.ToDouble(produkt_energia.Text), Convert.ToDouble(produkt_bialko.Text), Convert.ToDouble(produkt_tluszcze.Text), Convert.ToDouble(produkt_weglowodany.Text), Convert.ToDouble(produkt_sod.Text), Convert.ToDouble(produkt_tluszcze_nn.Text), Convert.ToDouble(produkty_przyswajalne.Text), Convert.ToDouble(produkty_blonnik.Text), Convert.ToDouble(produkty_cukry.Text));
                            MessageBox.Show($"Edytowano: {produkt_nazwa.Text}.", "Sukces");
                            produktClick();
                        }
                        else
                        {
                            MessageBox.Show("Nie uzupełniono wszystkich danych.", "Błąd");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Błąd dodawania produktu.\r\n{ex.Message}.", "Błąd");
                    }
                    break;
            }


        }

        private void produkt_wczytaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            produkt_nazwa.Text = Lista[produkt_wczytaj.SelectedIndex].nazwa;
            produkt_energia.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.energia.ToString();
            produkt_bialko.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.bialko.ToString();
            produkt_weglowodany.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.weglowodany.ToString();
            produkt_tluszcze.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.tluszcze.ToString();
            produkt_sod.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.sod.ToString();
            produkty_przyswajalne.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne.ToString();
            produkty_cukry.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.cukry.ToString();
            produkt_tluszcze_nn.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.tluszcze_nn.ToString();
            produkty_blonnik.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.blonnik.ToString();
            switch (Lista[produkt_wczytaj.SelectedIndex].kategoria)
            {
                case 'B':
                    produkt_kategoria.SelectedIndex = 0;
                    break;
                case 'M':
                    produkt_kategoria.SelectedIndex = 1;
                    break;
                case 'P':
                    produkt_kategoria.SelectedIndex = 2;
                    break;
                case 'N':
                    produkt_kategoria.SelectedIndex = 3;
                    break;
                case 'O':
                    produkt_kategoria.SelectedIndex = 4;
                    break;
                case 'W':
                    produkt_kategoria.SelectedIndex = 5;
                    break;
                case 'R':
                    produkt_kategoria.SelectedIndex = 6;
                    break;
                case 'T':
                    produkt_kategoria.SelectedIndex = 7;
                    break;
                case 'S':
                    produkt_kategoria.SelectedIndex = 8;
                    break;
                case 'D':
                    produkt_kategoria.SelectedIndex = 9;
                    break;
                case 'Z':
                    produkt_kategoria.SelectedIndex = 10;
                    break;

            }
        }

        private void cb_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybraneMiasto = cb_miasto.SelectedIndex;
            if (wybraneMiasto != -1)
            {
                cb_dieta.BeginUpdate();
                cb_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAll(cb_miasto.SelectedItem.ToString()); 
                var sortedDiety = Diety
                .OrderBy(d =>
                {
                    int index = Array.IndexOf(DietaPriority, d.nazwa);
                    return index == -1 ? int.MaxValue : index;
                }).ThenBy(d => d.nazwa).ToList();

                foreach (Dieta d in sortedDiety)
                    cb_dieta.Items.Add(d.nazwa);
                cb_dieta.EndUpdate();
                try { cb_dieta.SelectedIndex = wybranaDieta; } catch { if (cb_dieta.Items.Count > 0) cb_dieta.SelectedIndex = 0; }
            }
        }

        private void cb_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybranaDieta = cb_dieta.SelectedIndex;
            LiczSrednia();
        }

        private void cb_dieta_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (e.Index < 0) return;

            string text = cb.Items[e.Index].ToString();

            // Measure how tall the wrapped text will be at the combo's current width
            Size proposedSize = new Size(cb.Width - SystemInformation.VerticalScrollBarWidth - 4, int.MaxValue);
            SizeF textSize = e.Graphics.MeasureString(text, cb.Font, proposedSize.Width);

            e.ItemHeight = (int)Math.Ceiling(textSize.Height);
            e.ItemWidth = cb.Width;
        }


        private void cb_dieta_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox cb = (ComboBox)sender;
            string text = cb.Items[e.Index].ToString();

            e.DrawBackground();

            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                Rectangle rect = e.Bounds;
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(text, e.Font, textBrush, rect, sf);
            }

            e.DrawFocusRectangle();

        }

        #endregion

        #region Dekadowka

        public void GenerateCards()
        {
            Dekadowka dekadowkaDoWyswietlenia = wybranaDekadowka;

            //Pokaż pusty panel 
            Cursor.Current = Cursors.WaitCursor; 
            dekadowka_dekadowka.Enabled = false;
            dekadowka_panel.SuspendLayout();
            dekadowka_panel.Controls.Clear();
            Label loading = new Label
            {
                Text = "Wczytywanie...",
                AutoSize = true,
                Font = DietLabelFont
            };
            dekadowka_panel.Controls.Add(loading);
            dekadowka_panel.ResumeLayout();

            //Wczytaj dane
            Dekadowka[] jadlospisyDanejDekadowki = JadlospisDekadowkiDAO.SelectForAllDays(dekadowkaDoWyswietlenia);
            Cursor.Current = Cursors.Default;
            if (dekadowkaDoWyswietlenia != wybranaDekadowka) return;

            dekadowka_panel.SuspendLayout();
            dekadowka_panel.Controls.Clear();
            dekadowka_panel.VerticalScroll.Visible = true;
            dekadowka_panel.HorizontalScroll.Visible = false;

            for (int j = 0; j < dekadowkaDoWyswietlenia.dni; j++)
            {
                FlowLayoutPanel dayOfWeek = new FlowLayoutPanel
                {
                    BackColor = Color.White,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Size = new Size(dzienSize[0], dzienSize[1])
                };
                dayOfWeek.VerticalScroll.Visible = false;
                dayOfWeek.HorizontalScroll.Visible = false;
                dayOfWeek.SuspendLayout();

                string day = GetDay(dekadowkaDoWyswietlenia.dzienStart, j + 1);
                Label myDay = new Label
                {
                    Text = day,
                    MaximumSize = new Size(dzienSize[0], 0),
                    AutoSize = true
                };
                dayOfWeek.Controls.Add(myDay);

                foreach (Jadlospis jadlospis in jadlospisyDanejDekadowki[j].listaJadlospisow)
                {
                    if (jadlospis.dieta != null)
                    {
                        FlowLayoutPanel myPanel = new FlowLayoutPanel();
                        myPanel.SuspendLayout();
                        myPanel.BackColor = sandColor;
                        myPanel.AutoScroll = true;
                        myPanel.VerticalScroll.Visible = false;
                        myPanel.HorizontalScroll.Enabled = false;
                        myPanel.FlowDirection = FlowDirection.TopDown;
                        myPanel.WrapContents = false;
                        myPanel.AutoSize = true;

                        Panel divider = new Panel();
                        divider.BackColor = highlightColor;
                        divider.Size = new Size(dietaSize[0] - 25, 5);
                        myPanel.Controls.Add(divider);

                        Label diet = new Label();
                        diet.Text = jadlospis.dieta.nazwa;
                        diet.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        diet.Font = DietLabelFont;
                        diet.Margin = new Padding(0, 0, 0, 10);
                        diet.AutoSize = true;
                        myPanel.Controls.Add(diet);

                        Label meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        Label meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Margin = new Padding(10, 0, 0, 5);

                        meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        meal.Text = "Śniadanie:";
                        myPanel.Controls.Add(meal);

                        meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Text = jadlospis.nazwa_sniadanie != "" ? jadlospis.nazwa_sniadanie : "-";
                        meal_content.Margin = new Padding(10, 0, 0, 5);
                        myPanel.Controls.Add(meal_content);

                        meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        meal.Text = "II śniadanie:";
                        myPanel.Controls.Add(meal);

                        meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Text = jadlospis.nazwa_IIsniadanie != "" ? jadlospis.nazwa_IIsniadanie : "-";
                        meal_content.Margin = new Padding(10, 0, 0, 5);
                        myPanel.Controls.Add(meal_content);

                        meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        meal.Text = "Obiad:";
                        myPanel.Controls.Add(meal);

                        meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Text = jadlospis.nazwa_obiad != "" ? jadlospis.nazwa_obiad : "-";
                        meal_content.Margin = new Padding(10, 0, 0, 5);
                        myPanel.Controls.Add(meal_content);

                        meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        meal.Text = "Podwieczorek:";
                        myPanel.Controls.Add(meal);

                        meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Text = jadlospis.nazwa_podwieczorek != "" ? jadlospis.nazwa_podwieczorek : "-";
                        meal_content.Margin = new Padding(10, 0, 0, 5);
                        myPanel.Controls.Add(meal_content);

                        meal = new Label();
                        meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal.AutoSize = true;
                        meal.Font = MealLabelFont;
                        meal.Text = "Kolacja:";
                        myPanel.Controls.Add(meal);

                        meal_content = new Label();
                        meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                        meal_content.Font = MealLabelFont;
                        meal_content.AutoSize = true;
                        meal_content.Text = jadlospis.nazwa_kolacja != "" ? jadlospis.nazwa_kolacja : "-";
                        meal_content.Margin = new Padding(10, 0, 0, 5);
                        myPanel.Controls.Add(meal_content);

                        myPanel.ResumeLayout();
                        dayOfWeek.Controls.Add(myPanel);
                    }
                }

                dayOfWeek.ResumeLayout();
                dekadowka_panel.Controls.Add(dayOfWeek);
            }

            dekadowka_panel.ResumeLayout(); 
            dekadowka_dekadowka.Enabled = true;
        }

        public string GetDay(string dzien, int licznik)
        {
            if (licznik > 1)
            {
                string[] dni = new string[7] { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };
                int licz = 0;
                for (int i = 0; i < dni.Length; i++)
                {
                    if (dni[i] == dzien)
                        licz = i;
                }

                int j = 0;
                int odliczanie = 1;
                for (int i = 0; i < dni.Length; i++)
                {
                    if (j > licz)
                    {
                        string kolejnyDzien = dni[i];

                        if (odliczanie == licznik - 1)
                            return kolejnyDzien;

                        odliczanie++;
                    }
                    j++;

                    if (i == 6)
                        i = -1;
                }

                return "";
            }
            else
                return dzien;
        }
        
        private void dekadowka_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dekadowka_dekadowka.BeginUpdate();
            dekadowka_dekadowka.Items.Clear();
            listaDekadowek = DekadowkaDAO.Select(dekadowka_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowek)
                dekadowka_dekadowka.Items.Add(d.nazwa);
            dekadowka_dekadowka.EndUpdate();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dekadowka_usun_Click_1(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć tę szablon?", "Potwierdź", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DekadowkaDAO.Delete(listaDekadowek[dekadowka_dekadowka.SelectedIndex]);
                    MessageBox.Show($"Usunięto szablon: {listaDekadowek[dekadowka_dekadowka.SelectedIndex].nazwa} z: {listaDekadowek[dekadowka_dekadowka.SelectedIndex].miasto}.", "Sukces");
                    dekadowkaClick();
                    break;
                default:
                    break;
            }
        }

        private void dekadowka_ok_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Szablony -> Dodaj":
                    if (dekadowka_dodaj_nazwa.Text != "" && dekadowka_dodaj_dni.Text != "")
                    {
                        try
                        {
                            DekadowkaDAO.Insert(dekadowka_dodaj_nazwa.Text, dekadowka_dodaj_miasto.Text, Convert.ToInt32(dekadowka_dodaj_dni.Text), dekadowka_dodaj_dzienStart.SelectedItem.ToString(), null);
                            MessageBox.Show($"Dodano szablon: {dekadowka_dodaj_nazwa.Text} w: {dekadowka_dodaj_miasto.Text}.", "Sukces");
                            dekadowkaClick();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show($"Błąd dodawania szablonu.\r\n{ex.Message}.", "Błąd");

                        }

                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych.", "Błąd");
                    }
                    break;
                case "Szablony -> Generuj jadłospisy":
                    List<string> daty = new List<string>();
                    int dni = (Convert.ToDateTime(dekadowka_generuj_data2.Text) - Convert.ToDateTime(dekadowka_generuj_data1.Text)).Days + 1;
                    if (dni == wybranaDekadowka.dni)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        generowanie_status.Visible = true;
                        generowanie_status.Text = $"";
                        DateTime data = Convert.ToDateTime(dekadowka_generuj_data1.Text);
                        for (int i = 0; i < dni; i++)
                        {
                            string aktualna_data = $"{data.Day} {GetMonthForDate(data.Month)} {data.Year}";
                            List<Jadlospis> jadlospisyDanegoDnia = JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowka.id), wybranaDekadowka.miasto, i + 1);
                            int j = 0;
                            foreach (Jadlospis jadlospis in jadlospisyDanegoDnia)
                            {
                                generowanie_status.Text = $"Generowanie... \r\ndzień: {i + 1}/{dni} \r\njadłospis: {j + 1}/{jadlospisyDanegoDnia.Count}";
                                generowanie_status.Refresh();
                                JadlospisDAO.Insert(aktualna_data, jadlospis.dieta.nazwa, wybranaDekadowka.miasto, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja,
                                    reload: i == dni - 1 && j == jadlospisyDanegoDnia.Count - 1);
                                j++;
                            }
                            data = data.AddDays(1);
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Dodano jadłospisy według szablonu.", "Sukces");
                        generowanie_status.Text = $"";
                        generowanie_status.Visible = false;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Wpisano inną ilość dni niż wybranego szablonu.", "Błąd");
                    }
                    break;
            }
        }

        private void dekadowka_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybranaDekadowka = listaDekadowek[dekadowka_dekadowka.SelectedIndex];
            GenerateCards();
        }

        private void dekadowka_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dekadowka_usun_Click(object sender, EventArgs e)
        {
            //dodaj
            label10.Text = "Szablony -> Dodaj";

            dekadowka_miasto.Visible = false;
            dekadowka_panel.Visible = false;
            dekadowka_usun.Visible = false;
            dekadowka_dodaj.Visible = false;
            dekadowka_generuj.Visible = false;
            dekadowka_dekadowka.Visible = false;
            dekadowka_ok.Visible = true;
            dekadowka_nope.Visible = true;
            label33.Visible = false;
            label32.Visible = false;

            dekadowka_dodaj_dni.Visible = true;
            dekadowka_dodaj_label_dzienStart.Visible = true;
            dekadowka_dodaj_label_dekadowka.Visible = true;
            dekadowka_dodaj_label_dni.Visible = true;
            dekadowka_dodaj_label_miasto.Visible = true;
            dekadowka_dodaj_miasto.Visible = true;
            dekadowka_dodaj_nazwa.Visible = true;
            dekadowka_dodaj_dzienStart.Visible = true;

            dekadowka_dodaj_miasto.BeginUpdate();
            dekadowka_dodaj_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                dekadowka_dodaj_miasto.Items.Add(j.miasto);
            dekadowka_dodaj_miasto.EndUpdate();

            dekadowka_dodaj_dni.Text = "7";
            dekadowka_dodaj_miasto.SelectedIndex = 0;
            dekadowka_dodaj_dzienStart.SelectedIndex = 0;
            dekadowka_dodaj_nazwa.Text = "";
        }

        private void dekadowka_nope_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablony";
            dekadowka_miasto.Visible = true;
            dekadowka_panel.Visible = true;
            dekadowka_usun.Visible = true;
            dekadowka_generuj.Visible = true;
            dekadowka_dodaj.Visible = true;
            dekadowka_dekadowka.Visible = true;
            dekadowka_ok.Visible = false;
            dekadowka_nope.Visible = false;
            label33.Visible = true;
            label32.Visible = true;

            dekadowka_generuj_label1.Visible = false;
            dekadowka_generuj_label2.Visible = false;
            dekadowka_generuj_data1.Visible = false;
            dekadowka_generuj_data2.Visible = false;
            generowanie_status.Visible = false;

            dekadowka_dodaj_dni.Visible = false;
            dekadowka_dodaj_label_dzienStart.Visible = false;
            dekadowka_dodaj_label_dekadowka.Visible = false;
            dekadowka_dodaj_label_dni.Visible = false;
            dekadowka_dodaj_label_miasto.Visible = false;
            dekadowka_dodaj_miasto.Visible = false;
            dekadowka_dodaj_nazwa.Visible = false;
            dekadowka_dodaj_dzienStart.Visible = false;
        }

        private void zapiszJadłospisDekadówkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablon -> Dodaj jadłospis";
            panel_dekadowka_zapisz.Visible = true;
            panel_dekadowka_zapisz.BringToFront();

            dekadowka_zapisz_miasto.BeginUpdate();
            dekadowka_zapisz_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                dekadowka_zapisz_miasto.Items.Add(j.miasto);
            dekadowka_zapisz_miasto.EndUpdate();
            dekadowka_zapisz_miasto.SelectedIndex = 0;
        }

        private void dekadowka_zapisz_wstec_Click(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void dekadowka_zapisz_ok_Click(object sender, EventArgs e)
        {
            string sklad_sniadanie = "";
            for (int i = 0; i < lv_sniadanie.Items.Count; i++)
                sklad_sniadanie += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "|" + lv_sniadanie.Items[i].SubItems[8].Text + "|" + lv_sniadanie.Items[i].SubItems[9].Text + "|" + lv_sniadanie.Items[i].SubItems[10].Text + "$";

            string sklad_IIsniadanie = "";
            for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                sklad_IIsniadanie += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "$";

            string sklad_obiad = "";
            for (int i = 0; i < lv_obiad.Items.Count; i++)
                sklad_obiad += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "$";

            string sklad_podwieczorek = "";
            for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                sklad_podwieczorek += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "$";

            string sklad_kolacja = "";
            for (int i = 0; i < lv_kolacja.Items.Count; i++)
                sklad_kolacja += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "$";

            DAO.JadlospisDekadowkiDAO.Insert(Convert.ToInt32(wybranaDekadowkaDoZapisania.id), dekadowka_zapisz_dzien.SelectedIndex + 1, DAO.DietaDAO.Select(dekadowka_zapisz_dieta.SelectedItem.ToString(), dekadowka_zapisz_miasto.Text), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);

            MessageBox.Show("Zapisano jadłospis szablonu.", "Sukces");
            dekadowka_zapisz_wstec_Click(null, null);
        }

        private void dekadowka_zapisz_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dekadowka_zapisz_dieta.BeginUpdate();
            dekadowka_zapisz_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAll(dekadowka_zapisz_miasto.Text);
            var sortedDiety = Diety
            .OrderBy(d =>
            {
                int index = Array.IndexOf(DietaPriority, d.nazwa);
                return index == -1 ? int.MaxValue : index;
            }).ThenBy(d => d.nazwa).ToList();

            foreach (Dieta d in sortedDiety)
                dekadowka_zapisz_dieta.Items.Add(d.nazwa);
            dekadowka_zapisz_dieta.EndUpdate();
            if (dekadowka_zapisz_dieta.Items.Count > 0)
                dekadowka_zapisz_dieta.SelectedIndex = 0;

            dekadowka_zapisz_dekadowka.BeginUpdate();
            dekadowka_zapisz_dekadowka.Items.Clear();
            listaDekadowekDoZapisania = DekadowkaDAO.Select(dekadowka_zapisz_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoZapisania)
                dekadowka_zapisz_dekadowka.Items.Add(d.nazwa);
            dekadowka_zapisz_dekadowka.EndUpdate();
            if (dekadowka_zapisz_dekadowka.Items.Count > 0)
                dekadowka_zapisz_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_zapisz_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybranaDekadowkaDoZapisania = listaDekadowekDoZapisania[dekadowka_zapisz_dekadowka.SelectedIndex];
            dekadowka_zapisz_dzien.BeginUpdate();
            dekadowka_zapisz_dzien.Items.Clear();
            for (int j = 0; j < wybranaDekadowkaDoZapisania.dni; j++)
            {
                dekadowka_zapisz_dzien.Items.Add(GetDay(wybranaDekadowkaDoZapisania.dzienStart, j + 1));
            }
            dekadowka_zapisz_dzien.EndUpdate();
            if (dekadowka_zapisz_dzien.Items.Count > 0)
                dekadowka_zapisz_dzien.SelectedIndex = 0;
        }
        
        private void wczytajJadłospisDekadówkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablony -> Wczytaj";
            panel_dekadowka_wczytaj.Visible = true;
            panel_dekadowka_wczytaj.BringToFront();

            dekadowka_wczytaj_miasto.BeginUpdate();
            dekadowka_wczytaj_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                dekadowka_wczytaj_miasto.Items.Add(j.miasto);
            dekadowka_wczytaj_miasto.EndUpdate();
            dekadowka_wczytaj_miasto.SelectedIndex = 0;

            dekadowka_wczytaj_dekadowka.BeginUpdate();
            dekadowka_wczytaj_dekadowka.Items.Clear();
            listaDekadowekDoWczytania = DekadowkaDAO.Select(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            dekadowka_wczytaj_dekadowka.EndUpdate();
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_wczytaj_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                if (wybranaDekadowkaDoWczytania.nazwa != listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex].nazwa || wybranaDekadowkaDoWczytania.miasto != listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex].miasto)
                {
                    dekadowka_wczytaj_dzien.BeginUpdate();
                    dekadowka_wczytaj_dzien.Items.Clear();
                    wybranaDekadowkaDoWczytania = listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex];
                    for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
                    {
                        dekadowka_wczytaj_dzien.Items.Add(GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1));
                    }
                    dekadowka_wczytaj_dzien.EndUpdate();
                    if (dekadowka_wczytaj_dzien.Items.Count > 0)
                        dekadowka_wczytaj_dzien.SelectedIndex = 0;
                }
            }
            else
            {
                dekadowka_wczytaj_dzien.BeginUpdate();
                dekadowka_wczytaj_dzien.Items.Clear();
                wybranaDekadowkaDoWczytania = listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex];
                for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
                {
                    dekadowka_wczytaj_dzien.Items.Add(GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1));
                }
                dekadowka_wczytaj_dzien.EndUpdate();
                if (dekadowka_wczytaj_dzien.Items.Count > 0)
                    dekadowka_wczytaj_dzien.SelectedIndex = 0;
            }
        }

        private void dekadowka_wczytaj_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dekadowka_wczytaj_dekadowka.BeginUpdate();
            dekadowka_wczytaj_dekadowka.Items.Clear();
            listaDekadowekDoWczytania = DekadowkaDAO.Select(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            dekadowka_wczytaj_dekadowka.EndUpdate();
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_wczytaj_dzien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                dekadowka_wczytaj_dieta.BeginUpdate();
                dekadowka_wczytaj_dieta.Items.Clear();
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), wybranaDekadowkaDoWczytania.miasto, dekadowka_wczytaj_dzien.SelectedIndex + 1);
                foreach (Jadlospis d in jadlospisyDanegoDnia)
                {
                    if (d.dzien - 1 == dekadowka_wczytaj_dzien.SelectedIndex)
                    {
                        dekadowka_wczytaj_dieta.Items.Add(d.dieta.nazwa);
                    }
                }
                dekadowka_wczytaj_dieta.EndUpdate();

                if (dekadowka_wczytaj_dieta.Items.Count > 0)
                    dekadowka_wczytaj_dieta.SelectedIndex = 0;
            }
        }

        #endregion

        #region Dieta

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label10.Text != "Diety -> Dodaj")
            {
                dieta_dieta.BeginUpdate();
                dieta_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAll(dieta_miasto.SelectedItem.ToString());
                var sortedDiety = Diety
                .OrderBy(d =>
                {
                    int index = Array.IndexOf(DietaPriority, d.nazwa);
                    return index == -1 ? int.MaxValue : index;
                }).ThenBy(d => d.nazwa).ToList();

                foreach (Dieta d in sortedDiety)
                    dieta_dieta.Items.Add(d.nazwa);
                dieta_dieta.EndUpdate();
                if (dieta_dieta.Items.Count > 0)
                    dieta_dieta.SelectedIndex = 0;
                else
                {
                    dieta_nazwa.Text = "";
                    dieta_kod.Text = "";
                    energiaOd.Text = "";
                    energiaDo.Text = "";

                    bialkoOd.Text = "";
                    bialkoDo.Text = "";
                    bialkoOdTysiac.Text = "";
                    bialkoDoTysiac.Text = "";
                    bialkoOdProcent.Text = "";
                    bialkoDoProcent.Text = "";

                    tluszczeOd.Text = "";
                    tluszczeDo.Text = "";
                    tluszczeOdTysiac.Text = "";
                    tluszczeDoTys.Text = "";
                    TluszczeOdProc.Text = "";
                    tluszczeDoProc.Text = "";

                    kwasyOd.Text = "";
                    kwasyDo.Text = "";
                    KwasyOdTys.Text = "";
                    KwasyDoTys.Text = "";
                    kwasyOdProc.Text = "";
                    kwasyDoProc.Text = "";

                    wegleod.Text = "";
                    wegleDo.Text = "";
                    wegleOdTys.Text = "";
                    wedgleDoTys.Text = "";
                    wegleOdProc.Text = "";
                    wegleDoProc.Text = "";

                    przyswajalneOd.Text = "";
                    przyswajalneDo.Text = "";
                    przyswajalneOdTys.Text = "";
                    przyswajalneDotys.Text = "";
                    przyswajalneodProc.Text = "";
                    przyswajalneDoProc.Text = "";

                    cukryOd.Text = "";
                    cukryDo.Text = "";
                    cukryOdTys.Text = "";
                    cukryDoTys.Text = "";
                    cukryOdProc.Text = "";
                    cukryDoProc.Text = "";

                    blonnikOd.Text = "";
                    blonnikDo.Text = "";
                    blonnikOdTys.Text = "";
                    blonnikDoTys.Text = "";
                    blonnikOdProc.Text = "";
                    blonnikDoProc.Text = "";

                    sodOd.Text = "";
                    sodDo.Text = "";

                    SolOd.Text = "";
                    SolDo.Text = "";
                }
            }
        }

        private void dieta_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            dieta_nazwa.Text = Diety[dieta_dieta.SelectedIndex].nazwa;
            dieta_kod.Text = Diety[dieta_dieta.SelectedIndex].kod;

            energiaOd.Text = Diety[dieta_dieta.SelectedIndex].energiaOd.ToString();
            energiaDo.Text = Diety[dieta_dieta.SelectedIndex].energiaDo.ToString();

            bialkoOd.Text = Diety[dieta_dieta.SelectedIndex].bialkoOd.ToString();
            bialkoDo.Text = Diety[dieta_dieta.SelectedIndex].bialkoDo.ToString();
            bialkoOdTysiac.Text = Diety[dieta_dieta.SelectedIndex].bialkoOdNaTysiąc.ToString();
            bialkoDoTysiac.Text = Diety[dieta_dieta.SelectedIndex].bialkoDoNaTysiąc.ToString();
            bialkoOdProcent.Text = Diety[dieta_dieta.SelectedIndex].bialkoProcentOd.ToString();
            bialkoDoProcent.Text = Diety[dieta_dieta.SelectedIndex].bialkoProcentDo.ToString();

            tluszczeOd.Text = Diety[dieta_dieta.SelectedIndex].tluszczeOd.ToString();
            tluszczeDo.Text = Diety[dieta_dieta.SelectedIndex].tluszczeDo.ToString();
            tluszczeOdTysiac.Text = Diety[dieta_dieta.SelectedIndex].tluszczeOdNaTysiąc.ToString();
            tluszczeDoTys.Text = Diety[dieta_dieta.SelectedIndex].tluszczeDoNaTysiąc.ToString();
            TluszczeOdProc.Text = Diety[dieta_dieta.SelectedIndex].tluszczeProcentOd.ToString();
            tluszczeDoProc.Text = Diety[dieta_dieta.SelectedIndex].tluszczeProcentDo.ToString();

            kwasyOd.Text = Diety[dieta_dieta.SelectedIndex].kwasyOd.ToString();
            kwasyDo.Text = Diety[dieta_dieta.SelectedIndex].kwasyDo.ToString();
            KwasyOdTys.Text = Diety[dieta_dieta.SelectedIndex].kwasyOdNaTysiąc.ToString();
            KwasyDoTys.Text = Diety[dieta_dieta.SelectedIndex].kwasyDoNaTysiąc.ToString();
            kwasyOdProc.Text = Diety[dieta_dieta.SelectedIndex].kwasyProcentOd.ToString();
            kwasyDoProc.Text = Diety[dieta_dieta.SelectedIndex].kwasyProcentDo.ToString();

            wegleod.Text = Diety[dieta_dieta.SelectedIndex].wegleOd.ToString();
            wegleDo.Text = Diety[dieta_dieta.SelectedIndex].wegleDo.ToString();
            wegleOdTys.Text = Diety[dieta_dieta.SelectedIndex].wegleOdNaTysiąc.ToString();
            wedgleDoTys.Text = Diety[dieta_dieta.SelectedIndex].wegleDoNaTysiąc.ToString();
            wegleOdProc.Text = Diety[dieta_dieta.SelectedIndex].wegleProcentOd.ToString();
            wegleDoProc.Text = Diety[dieta_dieta.SelectedIndex].wegleProcentDo.ToString();

            przyswajalneOd.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneOd.ToString();
            przyswajalneDo.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneDo.ToString();
            przyswajalneOdTys.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneOdNaTysiąc.ToString();
            przyswajalneDotys.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneDoNaTysiąc.ToString();
            przyswajalneodProc.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneProcentOd.ToString();
            przyswajalneDoProc.Text = Diety[dieta_dieta.SelectedIndex].przyswajalneProcentDo.ToString();

            cukryOd.Text = Diety[dieta_dieta.SelectedIndex].cukryOd.ToString();
            cukryDo.Text = Diety[dieta_dieta.SelectedIndex].cukryDo.ToString();
            cukryOdTys.Text = Diety[dieta_dieta.SelectedIndex].cukryOdNaTysiąc.ToString();
            cukryDoTys.Text = Diety[dieta_dieta.SelectedIndex].cukryDoNaTysiąc.ToString();
            cukryOdProc.Text = Diety[dieta_dieta.SelectedIndex].cukryProcentOd.ToString();
            cukryDoProc.Text = Diety[dieta_dieta.SelectedIndex].cukryProcentDo.ToString();

            blonnikOd.Text = Diety[dieta_dieta.SelectedIndex].blonnikOd.ToString();
            blonnikDo.Text = Diety[dieta_dieta.SelectedIndex].blonnikDo.ToString();
            blonnikOdTys.Text = Diety[dieta_dieta.SelectedIndex].blonnikOdNaTysiąc.ToString();
            blonnikDoTys.Text = Diety[dieta_dieta.SelectedIndex].blonnikDoNaTysiąc.ToString();
            blonnikOdProc.Text = Diety[dieta_dieta.SelectedIndex].blonnikProcentOd.ToString();
            blonnikDoProc.Text = Diety[dieta_dieta.SelectedIndex].blonnikProcentDo.ToString();

            sodOd.Text = Diety[dieta_dieta.SelectedIndex].sodOd.ToString();
            sodDo.Text = Diety[dieta_dieta.SelectedIndex].sodDo.ToString();

            SolOd.Text = Diety[dieta_dieta.SelectedIndex].solOd.ToString();
            SolDo.Text = Diety[dieta_dieta.SelectedIndex].solDo.ToString();
        }

        private void dieta_ok_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Diety -> Dodaj":
                    if (dieta_nazwa.Text != "" && dieta_miasto.Text != "" && dieta_kod.Text != "" &&
     energiaOd.Text != "" && energiaDo.Text != "" &&
     bialkoOd.Text != "" && bialkoDo.Text != "" && bialkoOdTysiac.Text != "" && bialkoOdTysiac.Text != "" && bialkoOdProcent.Text != "" && bialkoOdProcent.Text != "" &&
     tluszczeOd.Text != "" && tluszczeDo.Text != "" && tluszczeOdTysiac.Text != "" && tluszczeDoTys.Text != "" && TluszczeOdProc.Text != "" && tluszczeDoProc.Text != "" &&
     wegleod.Text != "" && wegleDo.Text != "" &&
     sodOd.Text != "" && sodDo.Text != "" &&
     SolOd.Text != "" && SolDo.Text != "" &&
     kwasyOd.Text != "" && kwasyDo.Text != "" && KwasyOdTys.Text != "" && KwasyDoTys.Text != "" && kwasyOdProc.Text != "" && kwasyDoProc.Text != "" &&
     przyswajalneOd.Text != "" && przyswajalneDo.Text != "" && przyswajalneOdTys.Text != "" && przyswajalneDotys.Text != "" && przyswajalneodProc.Text != "" && przyswajalneDoProc.Text != "" &&
     blonnikOd.Text != "" && blonnikDo.Text != "" && blonnikOdTys.Text != "" && blonnikDoTys.Text != "" && blonnikOdProc.Text != "" && blonnikDoProc.Text != "" &&
     cukryOd.Text != "" && cukryDo.Text != "" && cukryOdTys.Text != "" && cukryDoTys.Text != "" && cukryOdProc.Text != "" && cukryDoProc.Text != "")
                    {
                        try
                        {
                            DietaDAO.Insert(dieta_nazwa.Text, dieta_miasto.Text, dieta_kod.Text,
                                Convert.ToDouble(energiaOd.Text), Convert.ToDouble(energiaDo.Text),0,0,0,0,
                                Convert.ToDouble(bialkoOd.Text), Convert.ToDouble(bialkoDo.Text), Convert.ToDouble(bialkoOdTysiac.Text), Convert.ToDouble(bialkoDoTysiac.Text), Convert.ToDouble(bialkoOdProcent.Text), Convert.ToDouble(bialkoDoProcent.Text),
                                Convert.ToDouble(tluszczeOd.Text), Convert.ToDouble(tluszczeDo.Text), Convert.ToDouble(tluszczeOdTysiac.Text), Convert.ToDouble(tluszczeDoTys.Text), Convert.ToDouble(TluszczeOdProc.Text), Convert.ToDouble(tluszczeDoProc.Text),
                                Convert.ToDouble(kwasyOd.Text), Convert.ToDouble(kwasyDo.Text), Convert.ToDouble(KwasyOdTys.Text), Convert.ToDouble(KwasyDoTys.Text), Convert.ToDouble(kwasyOdProc.Text), Convert.ToDouble(kwasyDoProc.Text),
                                Convert.ToDouble(wegleod.Text), Convert.ToDouble(wegleDo.Text), 0, 0, 0, 0,
                                Convert.ToDouble(przyswajalneOd.Text), Convert.ToDouble(przyswajalneDo.Text), Convert.ToDouble(przyswajalneOdTys.Text), Convert.ToDouble(przyswajalneDotys.Text), Convert.ToDouble(przyswajalneodProc.Text), Convert.ToDouble(przyswajalneDoProc.Text),
                                Convert.ToDouble(cukryOd.Text), Convert.ToDouble(cukryDo.Text), Convert.ToDouble(cukryOdTys.Text), Convert.ToDouble(cukryDoTys.Text), Convert.ToDouble(cukryOdProc.Text), Convert.ToDouble(cukryDoProc.Text),
                                Convert.ToDouble(blonnikOd.Text), Convert.ToDouble(blonnikDo.Text), Convert.ToDouble(blonnikOdTys.Text), Convert.ToDouble(blonnikDoTys.Text), Convert.ToDouble(blonnikOdProc.Text), Convert.ToDouble(blonnikDoProc.Text),
                                Convert.ToDouble(sodOd.Text), Convert.ToDouble(sodDo.Text), 0, 0, 0, 0,
                                Convert.ToDouble(SolOd.Text), Convert.ToDouble(SolDo.Text),0, 0, 0, 0
                                );
                            MessageBox.Show($"Dodano: {dieta_nazwa.Text}.", "Sukces");
                            dietaClick();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show($"Błąd dodawania diety. \r\n{ex.Message}.", "Błąd");

                        }
                    }
                    else
                        MessageBox.Show("Nie uzupełniono wszystkich danych.", "Błąd");
                    break;
                case "Diety -> Edytuj":
                    if (dieta_nazwa.Text != "" && dieta_miasto.Text != "" && dieta_kod.Text != "" &&
                        energiaOd.Text != "" && energiaDo.Text != "" &&
                        bialkoOd.Text != "" && bialkoDo.Text != "" && bialkoOdTysiac.Text != "" && bialkoDoTysiac.Text != "" && bialkoOdProcent.Text != "" && bialkoDoProcent.Text != "" &&
                        tluszczeOd.Text != "" && tluszczeDo.Text != "" && tluszczeOdTysiac.Text != "" && tluszczeDoTys.Text != "" && TluszczeOdProc.Text != "" && tluszczeDoProc.Text != "" &&
                        wegleod.Text != "" && wegleDo.Text != "" && wegleOdTys.Text != "" && wedgleDoTys.Text != "" && wegleOdProc.Text != "" && wegleDoProc.Text != "" &&
                        sodOd.Text != "" && sodDo.Text != "" &&
                        SolOd.Text != "" && SolDo.Text != "" &&
                        kwasyOd.Text != "" && kwasyDo.Text != "" && KwasyOdTys.Text != "" && KwasyDoTys.Text != "" && kwasyOdProc.Text != "" && kwasyDoProc.Text != "" &&
                        przyswajalneOd.Text != "" && przyswajalneDo.Text != "" && przyswajalneOdTys.Text != "" && przyswajalneDotys.Text != "" && przyswajalneodProc.Text != "" && przyswajalneDoProc.Text != "" &&
                        blonnikOd.Text != "" && blonnikDo.Text != "" && blonnikOdTys.Text != "" && blonnikDoTys.Text != "" && blonnikOdProc.Text != "" && blonnikDoProc.Text != "" &&
                        cukryOd.Text != "" && cukryDo.Text != "" && cukryOdTys.Text != "" && cukryDoTys.Text != "" && cukryOdProc.Text != "" && cukryDoProc.Text != "")
                    {
                        try
                        {
                            DAO.DietaDAO.Update(Diety[dieta_dieta.SelectedIndex], dieta_nazwa.Text, dieta_miasto.Text, dieta_kod.Text,
                                Convert.ToDouble(energiaOd.Text), Convert.ToDouble(energiaDo.Text), 0, 0, 0, 0,
                                Convert.ToDouble(bialkoOd.Text), Convert.ToDouble(bialkoDo.Text), Convert.ToDouble(bialkoOdTysiac.Text), Convert.ToDouble(bialkoDoTysiac.Text), Convert.ToDouble(bialkoOdProcent.Text), Convert.ToDouble(bialkoDoProcent.Text),
                                Convert.ToDouble(tluszczeOd.Text), Convert.ToDouble(tluszczeDo.Text), Convert.ToDouble(tluszczeOdTysiac.Text), Convert.ToDouble(tluszczeDoTys.Text), Convert.ToDouble(TluszczeOdProc.Text), Convert.ToDouble(tluszczeDoProc.Text),
                                Convert.ToDouble(kwasyOd.Text), Convert.ToDouble(kwasyDo.Text), Convert.ToDouble(KwasyOdTys.Text), Convert.ToDouble(KwasyDoTys.Text), Convert.ToDouble(kwasyOdProc.Text), Convert.ToDouble(kwasyDoProc.Text),
                                Convert.ToDouble(wegleod.Text), Convert.ToDouble(wegleDo.Text), Convert.ToDouble(wegleOdTys.Text), Convert.ToDouble(wedgleDoTys.Text), Convert.ToDouble(wegleOdProc.Text), Convert.ToDouble(wegleDoProc.Text),
                                Convert.ToDouble(przyswajalneOd.Text), Convert.ToDouble(przyswajalneDo.Text), Convert.ToDouble(przyswajalneOdTys.Text), Convert.ToDouble(przyswajalneDotys.Text), Convert.ToDouble(przyswajalneodProc.Text), Convert.ToDouble(przyswajalneDoProc.Text),
                                Convert.ToDouble(cukryOd.Text), Convert.ToDouble(cukryDo.Text), Convert.ToDouble(cukryOdTys.Text), Convert.ToDouble(cukryDoTys.Text), Convert.ToDouble(cukryOdProc.Text), Convert.ToDouble(cukryDoProc.Text),
                                Convert.ToDouble(blonnikOd.Text), Convert.ToDouble(blonnikDo.Text), Convert.ToDouble(blonnikOdTys.Text), Convert.ToDouble(blonnikDoTys.Text), Convert.ToDouble(blonnikOdProc.Text), Convert.ToDouble(blonnikDoProc.Text),
                                Convert.ToDouble(sodOd.Text), Convert.ToDouble(sodDo.Text), 0, 0, 0, 0,
                                Convert.ToDouble(SolOd.Text), Convert.ToDouble(SolDo.Text), 0, 0, 0, 0
                                );
                            MessageBox.Show($"Edytowano: {dieta_nazwa.Text}.", "Sukces");
                            dietaClick();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show($"Błąd edytowania diety.\r\n{ex.Message}.", "Błąd");

                        }
                    }
                    else
                        MessageBox.Show("Nie uzupełniono wszystkich danych.", "Błąd");
                    break;
            }


        }

        private void dieta_wstecz_Click(object sender, EventArgs e)
        {
            dieta_wstecz.Visible = false;
            dieta_ok.Visible = false;
            dieta_dodaj.Visible = true;
            dieta_edytuj.Visible = true;
            dieta_usun.Visible = true;
            dieta_dieta.Visible = true;
            label52.Visible = true;

            dieta_nazwa.Enabled = false;
            dieta_nazwa.BackColor = Color.FromName("ControlLight");

            dieta_kod.Enabled = false;
            dieta_kod.BackColor = Color.FromName("ControlLight");

            energiaOd.Enabled = false;
            energiaOd.BackColor = Color.FromName("ControlLight");
            energiaDo.Enabled = false;
            energiaDo.BackColor = Color.FromName("ControlLight");

            bialkoOd.BackColor = Color.FromName("ControlLight");
            bialkoDo.BackColor = Color.FromName("ControlLight");
            bialkoOdTysiac.BackColor = Color.FromName("ControlLight");
            bialkoDoTysiac.BackColor = Color.FromName("ControlLight");
            bialkoOdProcent.BackColor = Color.FromName("ControlLight");
            bialkoDoProcent.BackColor = Color.FromName("ControlLight");

            tluszczeOd.BackColor = Color.FromName("ControlLight");
            tluszczeDo.BackColor = Color.FromName("ControlLight");
            tluszczeOdTysiac.BackColor = Color.FromName("ControlLight");
            tluszczeDoTys.BackColor = Color.FromName("ControlLight");
            TluszczeOdProc.BackColor = Color.FromName("ControlLight");
            tluszczeDoProc.BackColor = Color.FromName("ControlLight");

            kwasyOd.BackColor = Color.FromName("ControlLight");
            kwasyDo.BackColor = Color.FromName("ControlLight");
            KwasyOdTys.BackColor = Color.FromName("ControlLight");
            KwasyDoTys.BackColor = Color.FromName("ControlLight");
            kwasyOdProc.BackColor = Color.FromName("ControlLight");
            kwasyDoProc.BackColor = Color.FromName("ControlLight");

            wegleod.BackColor = Color.FromName("ControlLight");
            wegleDo.BackColor = Color.FromName("ControlLight");
            wegleOdTys.BackColor = Color.FromName("ControlLight");
            wedgleDoTys.BackColor = Color.FromName("ControlLight");
            wegleOdProc.BackColor = Color.FromName("ControlLight");
            wegleDoProc.BackColor = Color.FromName("ControlLight");

            przyswajalneOd.BackColor = Color.FromName("ControlLight");
            przyswajalneDo.BackColor = Color.FromName("ControlLight");
            przyswajalneOdTys.BackColor = Color.FromName("ControlLight");
            przyswajalneDotys.BackColor = Color.FromName("ControlLight");
            przyswajalneodProc.BackColor = Color.FromName("ControlLight");
            przyswajalneDoProc.BackColor = Color.FromName("ControlLight");

            cukryOd.BackColor = Color.FromName("ControlLight");
            cukryDo.BackColor = Color.FromName("ControlLight");
            cukryOdTys.BackColor = Color.FromName("ControlLight");
            cukryDoTys.BackColor = Color.FromName("ControlLight");
            cukryOdProc.BackColor = Color.FromName("ControlLight");
            cukryDoProc.BackColor = Color.FromName("ControlLight");

            blonnikOd.BackColor = Color.FromName("ControlLight");
            blonnikDo.BackColor = Color.FromName("ControlLight");
            blonnikOdTys.BackColor = Color.FromName("ControlLight");
            blonnikDoTys.BackColor = Color.FromName("ControlLight");
            blonnikOdProc.BackColor = Color.FromName("ControlLight");
            blonnikDoProc.BackColor = Color.FromName("ControlLight");

            sodOd.BackColor = Color.FromName("ControlLight");
            sodDo.BackColor = Color.FromName("ControlLight");

            SolOd.BackColor = Color.FromName("ControlLight");
            SolDo.BackColor = Color.FromName("ControlLight");

            bialkoOd.Enabled = false;
            bialkoDo.Enabled = false;
            bialkoOdTysiac.Enabled = false;
            bialkoDoTysiac.Enabled = false;
            bialkoOdProcent.Enabled = false;
            bialkoDoProcent.Enabled = false;

            tluszczeOd.Enabled = false;
            tluszczeDo.Enabled = false;
            tluszczeOdTysiac.Enabled = false;
            tluszczeDoTys.Enabled = false;
            TluszczeOdProc.Enabled = false;
            tluszczeDoProc.Enabled = false;

            kwasyOd.Enabled = false;
            kwasyDo.Enabled = false; 
            KwasyOdTys.Enabled = false;
            KwasyDoTys.Enabled = false; 
            kwasyOdProc.Enabled = false; 
            kwasyDoProc.Enabled = false; 

            wegleod.Enabled = false; 
            wegleDo.Enabled = false;
            wegleOdTys.Enabled = false; 
            wedgleDoTys.Enabled = false; 
            wegleOdProc.Enabled = false; 
            wegleDoProc.Enabled = false; 

            przyswajalneOd.Enabled = false;
            przyswajalneDo.Enabled = false; 
            przyswajalneOdTys.Enabled = false; 
            przyswajalneDotys.Enabled = false; 
            przyswajalneodProc.Enabled = false; 
            przyswajalneDoProc.Enabled = false; 

            cukryOd.Enabled = false;
            cukryDo.Enabled = false;
            cukryOdTys.Enabled = false;
            cukryDoTys.Enabled = false; 
            cukryOdProc.Enabled = false; 
            cukryDoProc.Enabled = false; 

            blonnikOd.Enabled = false;
            blonnikDo.Enabled = false;
            blonnikOdTys.Enabled = false;
            blonnikDoTys.Enabled = false; 
            blonnikOdProc.Enabled = false; 
            blonnikDoProc.Enabled = false; 

            sodOd.Enabled = false;
            sodDo.Enabled = false; 

            SolOd.Enabled = false; 
            SolDo.Enabled = false; 


            label10.Text = "Diety";

            dieta_miasto.BeginUpdate();
            dieta_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka d in listaJednostek)
                dieta_miasto.Items.Add(d.miasto);
            dieta_miasto.EndUpdate();
            if (dieta_miasto.Items.Count > 0) dieta_miasto.SelectedIndex = 0;


        }

        private void dieta_usun_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, $"Na pewno chcesz usunąć {Diety[dieta_dieta.SelectedIndex].nazwa}?", "Potwierdź", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DietaDAO.Delete(Diety[dieta_dieta.SelectedIndex]);
                    MessageBox.Show($"Usunięto: {Diety[dieta_dieta.SelectedIndex].nazwa}.", "Sukces");
                    dietaClick();
                    break;
                default:
                    break;
            }
        }

        private void dieta_edytuj_Click(object sender, EventArgs e)
        {
            label10.Text = "Diety -> Edytuj";
            dieta_usun.Visible = false;
            dieta_dodaj.Visible = false;
            dieta_edytuj.Visible = false;

            dieta_ok.Visible = true;
            dieta_wstecz.Visible = true;

            dieta_nazwa.Enabled = true;
            dieta_nazwa.BackColor = Color.White;
            dieta_kod.Enabled = true;
            dieta_kod.BackColor = Color.White;
            energiaOd.Enabled = true;
            energiaOd.BackColor = Color.White;
            energiaDo.Enabled = true;
            energiaDo.BackColor = Color.White;

            bialkoOd.BackColor = Color.White;
            bialkoDo.BackColor = Color.White;
            bialkoOdTysiac.BackColor = Color.White;
            bialkoDoTysiac.BackColor = Color.White;
            bialkoOdProcent.BackColor = Color.White;
            bialkoDoProcent.BackColor = Color.White;

            tluszczeOd.BackColor = Color.White;
            tluszczeDo.BackColor = Color.White;
            tluszczeOdTysiac.BackColor = Color.White;
            tluszczeDoTys.BackColor = Color.White;
            TluszczeOdProc.BackColor = Color.White;
            tluszczeDoProc.BackColor = Color.White;

            kwasyOd.BackColor = Color.White;
            kwasyDo.BackColor = Color.White;
            KwasyOdTys.BackColor = Color.White;
            KwasyDoTys.BackColor = Color.White;
            kwasyOdProc.BackColor = Color.White;
            kwasyDoProc.BackColor = Color.White;

            wegleod.BackColor = Color.White;
            wegleDo.BackColor = Color.White;
            wegleOdTys.BackColor = Color.White;
            wedgleDoTys.BackColor = Color.White;
            wegleOdProc.BackColor = Color.White;
            wegleDoProc.BackColor = Color.White;

            przyswajalneOd.BackColor = Color.White;
            przyswajalneDo.BackColor = Color.White;
            przyswajalneOdTys.BackColor = Color.White;
            przyswajalneDotys.BackColor = Color.White;
            przyswajalneodProc.BackColor = Color.White;
            przyswajalneDoProc.BackColor = Color.White;

            cukryOd.BackColor = Color.White;
            cukryDo.BackColor = Color.White;
            cukryOdTys.BackColor = Color.White;
            cukryDoTys.BackColor = Color.White;
            cukryOdProc.BackColor = Color.White;
            cukryDoProc.BackColor = Color.White;

            blonnikOd.BackColor = Color.White;
            blonnikDo.BackColor = Color.White;
            blonnikOdTys.BackColor = Color.White;
            blonnikDoTys.BackColor = Color.White;
            blonnikOdProc.BackColor = Color.White;
            blonnikDoProc.BackColor = Color.White;

            sodOd.BackColor = Color.White;
            sodDo.BackColor = Color.White;

            SolOd.BackColor = Color.White;
            SolDo.BackColor = Color.White;

            bialkoOd.Enabled = true;
            bialkoDo.Enabled = true;
            bialkoOdTysiac.Enabled = true;
            bialkoDoTysiac.Enabled = true;
            bialkoOdProcent.Enabled = true;
            bialkoDoProcent.Enabled = true;

            tluszczeOd.Enabled = true;
            tluszczeDo.Enabled = true;
            tluszczeOdTysiac.Enabled = true;
            tluszczeDoTys.Enabled = true;
            TluszczeOdProc.Enabled = true;
            tluszczeDoProc.Enabled = true;

            kwasyOd.Enabled = true;
            kwasyDo.Enabled = true;
            KwasyOdTys.Enabled = true;
            KwasyDoTys.Enabled = true;
            kwasyOdProc.Enabled = true;
            kwasyDoProc.Enabled = true;

            wegleod.Enabled = true;
            wegleDo.Enabled = true;
            wegleOdTys.Enabled = true;
            wedgleDoTys.Enabled = true;
            wegleOdProc.Enabled = true;
            wegleDoProc.Enabled = true;

            przyswajalneOd.Enabled = true;
            przyswajalneDo.Enabled = true;
            przyswajalneOdTys.Enabled = true;
            przyswajalneDotys.Enabled = true;
            przyswajalneodProc.Enabled = true;
            przyswajalneDoProc.Enabled = true;

            cukryOd.Enabled = true;
            cukryDo.Enabled = true;
            cukryOdTys.Enabled = true;
            cukryDoTys.Enabled = true;
            cukryOdProc.Enabled = true;
            cukryDoProc.Enabled = true;

            blonnikOd.Enabled = true;
            blonnikDo.Enabled = true;
            blonnikOdTys.Enabled = true;
            blonnikDoTys.Enabled = true;
            blonnikOdProc.Enabled = true;
            blonnikDoProc.Enabled = true;

            sodOd.Enabled = true;
            sodDo.Enabled = true;

            SolOd.Enabled = true;
            SolDo.Enabled = true;
        }

        private void dieta_dodaj_Click(object sender, EventArgs e)
        {
            label52.Visible = false;
            label10.Text = "Diety -> Dodaj";
            dieta_usun.Visible = false;
            dieta_dodaj.Visible = false;
            dieta_edytuj.Visible = false;
            dieta_dieta.Visible = false;

            dieta_ok.Visible = true;
            dieta_wstecz.Visible = true;

            dieta_nazwa.Enabled = true;
            dieta_nazwa.BackColor = Color.White;
            dieta_kod.Enabled = true;
            dieta_kod.BackColor = Color.White;
            energiaOd.Enabled = true;
            energiaOd.BackColor = Color.White;
            energiaDo.Enabled = true;
            energiaDo.BackColor = Color.White;

            bialkoOd.BackColor = Color.White;
            bialkoDo.BackColor = Color.White;
            bialkoOdTysiac.BackColor = Color.White;
            bialkoDoTysiac.BackColor = Color.White;
            bialkoOdProcent.BackColor = Color.White;
            bialkoDoProcent.BackColor = Color.White;

            tluszczeOd.BackColor = Color.White;
            tluszczeDo.BackColor = Color.White;
            tluszczeOdTysiac.BackColor = Color.White;
            tluszczeDoTys.BackColor = Color.White;
            TluszczeOdProc.BackColor = Color.White;
            tluszczeDoProc.BackColor = Color.White;

            kwasyOd.BackColor = Color.White;
            kwasyDo.BackColor = Color.White;
            KwasyOdTys.BackColor = Color.White;
            KwasyDoTys.BackColor = Color.White;
            kwasyOdProc.BackColor = Color.White;
            kwasyDoProc.BackColor = Color.White;

            wegleod.BackColor = Color.White;
            wegleDo.BackColor = Color.White;
            wegleOdTys.BackColor = Color.White;
            wedgleDoTys.BackColor = Color.White;
            wegleOdProc.BackColor = Color.White;
            wegleDoProc.BackColor = Color.White;

            przyswajalneOd.BackColor = Color.White;
            przyswajalneDo.BackColor = Color.White;
            przyswajalneOdTys.BackColor = Color.White;
            przyswajalneDotys.BackColor = Color.White;
            przyswajalneodProc.BackColor = Color.White;
            przyswajalneDoProc.BackColor = Color.White;

            cukryOd.BackColor = Color.White;
            cukryDo.BackColor = Color.White;
            cukryOdTys.BackColor = Color.White;
            cukryDoTys.BackColor = Color.White;
            cukryOdProc.BackColor = Color.White;
            cukryDoProc.BackColor = Color.White;

            blonnikOd.BackColor = Color.White;
            blonnikDo.BackColor = Color.White;
            blonnikOdTys.BackColor = Color.White;
            blonnikDoTys.BackColor = Color.White;
            blonnikOdProc.BackColor = Color.White;
            blonnikDoProc.BackColor = Color.White;

            sodOd.BackColor = Color.White;
            sodDo.BackColor = Color.White;

            SolOd.BackColor = Color.White;
            SolDo.BackColor = Color.White;

            bialkoOd.Enabled = true;
            bialkoDo.Enabled = true;
            bialkoOdTysiac.Enabled = true;
            bialkoDoTysiac.Enabled = true;
            bialkoOdProcent.Enabled = true;
            bialkoDoProcent.Enabled = true;

            tluszczeOd.Enabled = true;
            tluszczeDo.Enabled = true;
            tluszczeOdTysiac.Enabled = true;
            tluszczeDoTys.Enabled = true;
            TluszczeOdProc.Enabled = true;
            tluszczeDoProc.Enabled = true;

            kwasyOd.Enabled = true;
            kwasyDo.Enabled = true;
            KwasyOdTys.Enabled = true;
            KwasyDoTys.Enabled = true;
            kwasyOdProc.Enabled = true;
            kwasyDoProc.Enabled = true;

            wegleod.Enabled = true;
            wegleDo.Enabled = true;
            wegleOdTys.Enabled = true;
            wedgleDoTys.Enabled = true;
            wegleOdProc.Enabled = true;
            wegleDoProc.Enabled = true;

            przyswajalneOd.Enabled = true;
            przyswajalneDo.Enabled = true;
            przyswajalneOdTys.Enabled = true;
            przyswajalneDotys.Enabled = true;
            przyswajalneodProc.Enabled = true;
            przyswajalneDoProc.Enabled = true;

            cukryOd.Enabled = true;
            cukryDo.Enabled = true;
            cukryOdTys.Enabled = true;
            cukryDoTys.Enabled = true;
            cukryOdProc.Enabled = true;
            cukryDoProc.Enabled = true;

            blonnikOd.Enabled = true;
            blonnikDo.Enabled = true;
            blonnikOdTys.Enabled = true;
            blonnikDoTys.Enabled = true;
            blonnikOdProc.Enabled = true;
            blonnikDoProc.Enabled = true;

            sodOd.Enabled = true;
            sodDo.Enabled = true;

            SolOd.Enabled = true;
            SolDo.Enabled = true;

            dieta_nazwa.Text = "";
            dieta_kod.Text = "";
            energiaOd.Text = "";
            energiaDo.Text = "";

            bialkoOd.Text = "";
            bialkoDo.Text = "";
            bialkoOdTysiac.Text = "";
            bialkoDoTysiac.Text = "";
            bialkoOdProcent.Text = "";
            bialkoDoProcent.Text = "";

            tluszczeOd.Text = "";
            tluszczeDo.Text = "";
            tluszczeOdTysiac.Text = "";
            tluszczeDoTys.Text = "";
            TluszczeOdProc.Text = "";
            tluszczeDoProc.Text = "";

            kwasyOd.Text = "";
            kwasyDo.Text = "";
            KwasyOdTys.Text = "";
            KwasyDoTys.Text = "";
            kwasyOdProc.Text = "";
            kwasyDoProc.Text = "";

            wegleod.Text = "";
            wegleDo.Text = "";
            wegleOdTys.Text = "";
            wedgleDoTys.Text = "";
            wegleOdProc.Text = "";
            wegleDoProc.Text = "";

            przyswajalneOd.Text = "";
            przyswajalneDo.Text = "";
            przyswajalneOdTys.Text = "";
            przyswajalneDotys.Text = "";
            przyswajalneodProc.Text = "";
            przyswajalneDoProc.Text = "";

            cukryOd.Text = "";
            cukryDo.Text = "";
            cukryOdTys.Text = "";
            cukryDoTys.Text = "";
            cukryOdProc.Text = "";
            cukryDoProc.Text = "";

            blonnikOd.Text = "";
            blonnikDo.Text = "";
            blonnikOdTys.Text = "";
            blonnikDoTys.Text = "";
            blonnikOdProc.Text = "";
            blonnikDoProc.Text = "";

            sodOd.Text = "";
            sodDo.Text = "";

            SolOd.Text = "";
            SolDo.Text = "";
        }

        #endregion

        #region Jednostka

        private void jednostka_wstecz_Click(object sender, EventArgs e)
        {
            label10.Text = "Jednostki";
            jednostka_wstecz.Visible = false;
            jednostka_ok.Visible = false;
            jednostka_dodaj.Visible = true;
            jednostka_edytuj.Visible = true;
            jednostka_usun.Visible = true;
            jednostka_jednostka.Visible = true;
            jednostka_label.Visible = true;

            jednostka_miasto.Enabled = false;
            jednostka_miasto.BackColor = Color.FromName("ControlLight");

            jednostka_jednostka.BeginUpdate();
            jednostka_jednostka.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                jednostka_jednostka.Items.Add(j.miasto);
            jednostka_jednostka.EndUpdate();
            if (jednostka_jednostka.Items.Count > 0)
                jednostka_jednostka.SelectedIndex = 0;


        }

        private void jednostka_edytuj_Click(object sender, EventArgs e)
        {

            label10.Text = "Jednostki -> Edytuj";
            jednostka_usun.Visible = false;
            jednostka_dodaj.Visible = false;
            jednostka_edytuj.Visible = false;

            jednostka_ok.Visible = true;
            jednostka_wstecz.Visible = true;

            jednostka_miasto.Enabled = true;
            jednostka_miasto.BackColor = Color.White;
        }

        private void jednostka_dodaj_Click(object sender, EventArgs e)
        {
            jednostka_label.Visible = false;
            label10.Text = "Jednostki -> Dodaj";
            jednostka_usun.Visible = false;
            jednostka_dodaj.Visible = false;
            jednostka_edytuj.Visible = false;
            jednostka_jednostka.Visible = false;

            jednostka_ok.Visible = true;
            jednostka_wstecz.Visible = true;

            jednostka_miasto.Enabled = true;
            jednostka_miasto.BackColor = Color.White;

            jednostka_miasto.Text = "";
        }

        private void jednostka_usun_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, $"Na pewno chcesz usunąć {listaJednostek[jednostka_jednostka.SelectedIndex].miasto}?", "Potwierdź", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    JednostkaDAO.Delete(listaJednostek[jednostka_jednostka.SelectedIndex]);
                    MessageBox.Show($"Usunięto: {listaJednostek[jednostka_jednostka.SelectedIndex].miasto}.", "Sukces");
                    jednostkaClick();
                    break;
                default:
                    break;
            }
        }

        private void jednostka_ok_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Jednostki -> Dodaj":
                    if (jednostka_miasto.Text != "")
                    {
                        JednostkaDAO.Insert(jednostka_miasto.Text);
                        MessageBox.Show($"Dodano: {jednostka_miasto.Text}.", "Sukces");

                        jednostkaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych.", "Błąd");
                    }
                    break;
                case "Jednostki -> Edytuj":
                    if (jednostka_miasto.Text != "")
                    {
                        JednostkaDAO.Update(listaJednostek[jednostka_jednostka.SelectedIndex], jednostka_miasto.Text);
                        MessageBox.Show($"Edytowano: {jednostka_miasto.Text}.", "Sukces");

                        jednostkaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych.", "Błąd");
                    }
                    break;
            }

        }

        private void jednostka_jednostka_SelectedIndexChanged(object sender, EventArgs e)
        {
            jednostka_miasto.Text = listaJednostek[jednostka_jednostka.SelectedIndex].miasto;
        }

        #endregion Jednostka

        #region Receptury

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                default:
                    label10.Text = "Receptury";

                    receptura_wczytaj.BeginUpdate();
                    receptura_wczytaj.Items.Clear();
                    listaReceptur = DAO.RecepturaDAO.SelectAll();
                    foreach (Receptura r in listaReceptur)
                        receptura_wczytaj.Items.Add(r.nazwa);
                    receptura_wczytaj.EndUpdate();

                    if (receptura_wczytaj.Items.Count > 0)
                        receptura_wczytaj.SelectedIndex = 0;

                    receptura_kategoria.Visible = false;
                    receptura_produkty.Visible = false;
                    pictureBox6.Visible = false;
                    receptura_masa.Visible = false;
                    pictureBox7.Visible = false;
                    receptura_up.Visible = false;
                    receptura_down.Visible = false;
                    receptura_del.Visible = false;

                    label61.Visible = true;
                    receptura_wczytaj.Visible = true;
                    pictureBox14.Visible = false;
                    pictureBox16.Visible = false;
                    pictureBox15.Visible = true;
                    pictureBox17.Visible = true;
                    pictureBox18.Visible = true;
                    receptura_posilek.Visible = false;
                    label53.Visible = false;
                    receptura_nazwa.Enabled = false;
                    receptura_nazwa.BackColor = Color.FromName("ControlLight");

                    label50.Location = new Point(40, 170);
                    receptura_nazwa.Location = new Point(210, 170);
                    receptura_sklad.Location = new Point(45, 212);
                    label61.Location = new Point(40, 125);
                    receptura_wczytaj.Location = new Point(210, 120);

                    break;
                case "Receptury -> Wczytaj":
                    glownaClick();
                    break;
            }
        }

        private void receptura_wczytaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            receptura_nazwa.Text = listaReceptur[receptura_wczytaj.SelectedIndex].nazwa;

            receptura_sklad.BeginUpdate();
            receptura_sklad.Items.Clear();
            string[] produkty = listaReceptur[receptura_wczytaj.SelectedIndex].sklad.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                receptura_sklad.Items.Add(itm);
            }
            receptura_sklad.EndUpdate();

            LiczSredniaDlaReceptur();
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, $"Na pewno chcesz usunąć {listaReceptur[receptura_wczytaj.SelectedIndex].nazwa}?", "Potwierdź", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.RecepturaDAO.Delete(listaReceptur[receptura_wczytaj.SelectedIndex]);
                    MessageBox.Show($"Usunięto: {listaReceptur[receptura_wczytaj.SelectedIndex].nazwa}.", "Sukces");
                    recepturaClick();
                    break;
                default:
                    break;
            }
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            label10.Text = "Receptury -> Edytuj";
            receptura_kategoria.SelectedIndex = 0;
            receptura_masa.Text = "";

            label50.Location = new Point(40 + 185, label50.Location.Y);
            receptura_nazwa.Location = new Point(210 + 185, receptura_nazwa.Location.Y);
            receptura_sklad.Location = new Point(45 + 185, receptura_sklad.Location.Y);
            label61.Location = new Point(40 + 185, label61.Location.Y);
            receptura_wczytaj.Location = new Point(210 + 185, receptura_wczytaj.Location.Y);

            receptura_kategoria.Visible = true;
            receptura_produkty.Visible = true;
            pictureBox6.Visible = true;
            receptura_masa.Visible = true;
            pictureBox7.Visible = true;
            receptura_up.Visible = true;
            receptura_down.Visible = true;
            receptura_del.Visible = true;

            pictureBox14.Visible = true;
            pictureBox16.Visible = true;
            pictureBox15.Visible = false;
            pictureBox17.Visible = false;
            pictureBox18.Visible = false;

            receptura_nazwa.Enabled = true;
            receptura_nazwa.BackColor = Color.White;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            label10.Text = "Receptury -> Dodaj";
            receptura_kategoria.SelectedIndex = 0;
            receptura_masa.Text = "";
            label50.Location = new Point(40 + 185, label50.Location.Y);
            receptura_nazwa.Location = new Point(210 + 185, receptura_nazwa.Location.Y);
            receptura_sklad.Location = new Point(45 + 185, receptura_sklad.Location.Y);

            receptura_kategoria.Visible = true;
            receptura_produkty.Visible = true;
            pictureBox6.Visible = true;
            receptura_masa.Visible = true;
            pictureBox7.Visible = true;
            receptura_up.Visible = true;
            receptura_down.Visible = true;
            receptura_del.Visible = true;

            label61.Visible = false;
            receptura_wczytaj.Visible = false;
            pictureBox14.Visible = true;
            pictureBox16.Visible = true;
            pictureBox15.Visible = false;
            pictureBox17.Visible = false;
            pictureBox18.Visible = false;

            receptura_nazwa.Enabled = true;
            receptura_nazwa.BackColor = Color.White;

            double[] suma_receptura = new double[6];
            receptura_nazwa.Text = "";
            receptura_sklad.Items.Clear();
            LiczSredniaDlaReceptur();

        }

        public void LiczSredniaDlaReceptur()
        {
            double[] suma_receptura = new double[9];

            for (int i = 0; i < 9; i++)
            {
                suma_receptura[i] = 0;
            }

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < receptura_sklad.Items.Count; i++)
                {
                    double a = 0;
                    try
                    {
                        a = double.Parse(receptura_sklad.Items[i].SubItems[k + 2].Text);
                    }
                    catch { }
                    suma_receptura[k] += a;
                }
            }

            receptura_energia.Text = Math.Round(suma_receptura[0], 2).ToString() + " kcal";
            receptura_bialko.Text = Math.Round(suma_receptura[1], 2).ToString() + " g";
            receptura_tluszcze.Text = Math.Round(suma_receptura[2], 2).ToString() + " g";
            receptura_weglowodany.Text = Math.Round(suma_receptura[4], 2).ToString() + " g";
            receptura_sod.Text = Math.Round(suma_receptura[8], 2).ToString() + " mg";
            receptura_przyswajalne.Text = Math.Round(suma_receptura[5], 2).ToString() + " g";
            receptura_cukry.Text = Math.Round(suma_receptura[6], 2).ToString() + " g";
            receptura_blonnik.Text = Math.Round(suma_receptura[7], 2).ToString() + " mg";
            receptura_ktn.Text = Math.Round(suma_receptura[3], 2).ToString() + " g";
            receptura_sol.Text = Math.Round(suma_receptura[8] * 0.0025, 2).ToString() + " g";
        }

        private void receptura_kategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int wybor = receptura_kategoria.SelectedIndex;

            receptura_produkty.BeginUpdate();
            receptura_produkty.Items.Clear();
            switch (wybor)
            {
                case 0:
                    Lista.OrderBy(x => x.nazwa);
                    foreach (var v in Lista)
                        receptura_produkty.Items.Add(v.nazwa);

                    kategoria = "Wszystkie";
                    break;
                case 1:
                    Bakalie = Lista.Where(x => x.kategoria == 'B').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Bakalie)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "B";
                    break;
                case 2:
                    Mieso = Lista.Where(x => x.kategoria == 'M').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Mieso)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "M";
                    break;
                case 3:
                    Przyprawy = Lista.Where(x => x.kategoria == 'P').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Przyprawy)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "P";
                    break;
                case 4:
                    Nabial = Lista.Where(x => x.kategoria == 'N').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Nabial)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "N";
                    break;
                case 5:
                    Owoce = Lista.Where(x => x.kategoria == 'O').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Owoce)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "O";
                    break;
                case 6:
                    Warzywa = Lista.Where(x => x.kategoria == 'W').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Warzywa)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "W";
                    break;
                case 7:
                    Ryby = Lista.Where(x => x.kategoria == 'R').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Ryby)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "R";
                    break;
                case 8:
                    Tluszcze = Lista.Where(x => x.kategoria == 'T').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Tluszcze)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "T";
                    break;
                case 9:
                    Slodycze = Lista.Where(x => x.kategoria == 'S').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Slodycze)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "S";
                    break;
                case 10:
                    Napoje = Lista.Where(x => x.kategoria == 'D').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Napoje)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "D";
                    break;
                case 11:
                    Zboza = Lista.Where(x => x.kategoria == 'Z').OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
                    foreach (var v in Zboza)
                    {
                        receptura_produkty.Items.Add(v.nazwa);
                    }
                    kategoria = "Z";
                    break;
            }
            receptura_produkty.EndUpdate();
        }

        private void receptura_produkt_dodaj_Click(object sender, EventArgs e)
        {
            if (receptura_produkty.SelectedIndex != -1)
            {
                if (receptura_masa.Text != "")
                {
                    try
                    {
                        double masa = Math.Round(double.Parse(receptura_masa.Text), 2);
                        int ktory = receptura_produkty.SelectedIndex;
                        string[] arr = new string[11];
                        ListViewItem itm;
                        List<Produkt> Kategoria = new List<Produkt>();
                        switch (kategoria)
                        {
                            case "Wszystkie":
                                Kategoria = Lista;
                                break;
                            case "M":
                                Kategoria = Mieso;
                                break;
                            case "W":
                                Kategoria = Warzywa;
                                break;
                            case "O":
                                Kategoria = Owoce;
                                break;
                            case "S":
                                Kategoria = Slodycze;
                                break;
                            case "R":
                                Kategoria = Ryby;
                                break;
                            case "D":
                                Kategoria = Napoje;
                                break;
                            case "Z":
                                Kategoria = Zboza;
                                break;
                            case "P":
                                Kategoria = Przyprawy;
                                break;
                            case "N":
                                Kategoria = Nabial;
                                break;
                            case "B":
                                Kategoria = Bakalie;
                                break;
                            case "T":
                                Kategoria = Tluszcze;
                                break;
                        }

                        arr[0] = Lista[ktory].nazwa;
                        arr[1] = masa.ToString();
                        arr[2] = Math.Round(Lista[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                        arr[3] = Math.Round(Lista[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                        arr[4] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                        arr[5] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                        arr[6] = Math.Round(Lista[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                        arr[7] = Math.Round(Lista[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                        arr[8] = Math.Round(Lista[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                        arr[9] = Math.Round(Lista[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                        arr[10] = Math.Round(Lista[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();

                        itm = new ListViewItem(arr);

                        receptura_sklad.Items.Add(itm);

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Nieprawidłowa wartość.\r\n{ex.Message}.", "Błąd");
                    }
                    LiczSredniaDlaReceptur();
                }
                else
                {
                    MessageBox.Show("Nie wpisano masy produktu.", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu.", "Błąd");
            }

        }

        private void receptura_del_Click(object sender, EventArgs e)
        {
            string produkt = "";
            List<int> ktory = new List<int>();

            ktory = new List<int>();
            for (int k = 0; k < receptura_sklad.SelectedIndices.Count; k++)
                ktory.Add(Int32.Parse(receptura_sklad.SelectedIndices[k].ToString()));
            if (ktory.Count > 0)
                produkt = receptura_sklad.Items[ktory[0]].Text;

            DialogResult dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć {produkt}?", "Potwierdź", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                ktory = new List<int>();
                for (int k = 0; k < receptura_sklad.SelectedIndices.Count; k++)
                    ktory.Add(Int32.Parse(receptura_sklad.SelectedIndices[k].ToString()));
                receptura_sklad.Items.RemoveAt(ktory[0]);
                LiczSredniaDlaReceptur();
            }
            else
            {

            }
        }

        private void receptura_dodaj_edytuj_Click(object sender, EventArgs e)
        {
            try
            {
                int liczba = receptura_sklad.Items.Count;
                int wybrany = receptura_sklad.SelectedIndices[0];
                string[] arr = new string[11];
                double masa = double.Parse(receptura_masa.Text);
                arr[0] = receptura_sklad.Items[wybrany].SubItems[0].Text;
                arr[1] = masa.ToString();
                arr[2] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[2].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[3] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[3].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[4] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[4].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[5] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[5].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[6] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[6].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[7] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[7].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[8] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[8].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[9] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[9].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                arr[10] = Math.Round(masa * double.Parse(receptura_sklad.Items[wybrany].SubItems[10].Text) / double.Parse(receptura_sklad.Items[wybrany].SubItems[1].Text), 2).ToString();
                ListViewItem itm = new ListViewItem(arr);

                receptura_sklad.Items.Remove(receptura_sklad.Items[wybrany]);
                receptura_sklad.Items.Insert(wybrany, itm);
                LiczSredniaDlaReceptur();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można edytować.\r\n{ex.Message}.", "Błąd");
            }
        }

        private void receptura_up_Click(object sender, EventArgs e)
        {
            try
            {

                int liczba = receptura_sklad.Items.Count;
                int wybrany = receptura_sklad.SelectedIndices[0];
                if (wybrany > 0)
                {
                    ListViewItem itm = receptura_sklad.Items[wybrany];
                    receptura_sklad.Items.Remove(itm);
                    receptura_sklad.Items.Insert(wybrany - 1, itm);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można przesunąć.\r\n{ex.Message}", "Błąd");
            }
        }

        private void receptura_down_Click(object sender, EventArgs e)
        {
            try
            {

                int liczba = receptura_sklad.Items.Count;
                int wybrany = receptura_sklad.SelectedIndices[0];
                if (wybrany < liczba - 1)
                {
                    ListViewItem itm = receptura_sklad.Items[wybrany];
                    receptura_sklad.Items.Remove(itm);
                    receptura_sklad.Items.Insert(wybrany + 1, itm);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można przesunąć.\r\n{ex.Message}", "Błąd");
            }
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            string sklad = "";
            for (int i = 0; i < receptura_sklad.Items.Count; i++)
                sklad += receptura_sklad.Items[i].SubItems[0].Text + "|" + receptura_sklad.Items[i].SubItems[1].Text + "|" + receptura_sklad.Items[i].SubItems[2].Text + "|" + receptura_sklad.Items[i].SubItems[3].Text + "|" + receptura_sklad.Items[i].SubItems[4].Text + "|" + receptura_sklad.Items[i].SubItems[5].Text + "|" + receptura_sklad.Items[i].SubItems[6].Text + "|" + receptura_sklad.Items[i].SubItems[7].Text + "|" + receptura_sklad.Items[i].SubItems[8].Text + "|" + receptura_sklad.Items[i].SubItems[9].Text + "|" + receptura_sklad.Items[i].SubItems[10].Text + "$";

            switch (label10.Text)
            {
                case "Receptury -> Dodaj":
                    if (sklad != "" && receptura_nazwa.Text != "")
                    {
                        DAO.RecepturaDAO.Insert(receptura_nazwa.Text, sklad);
                        MessageBox.Show($"Dodano: {receptura_nazwa.Text}.", "Sukces");
                        recepturaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych.", "Błąd");
                    }
                    break;
                case "Receptury -> Edytuj":
                    if (sklad != "" && receptura_nazwa.Text != "")
                    {
                        DAO.RecepturaDAO.Update(listaReceptur[receptura_wczytaj.SelectedIndex], receptura_nazwa.Text, sklad);
                        MessageBox.Show($"Edytowano: {receptura_nazwa.Text}.", "Sukces");
                        recepturaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych.", "Błąd");
                    }
                    break;
                case "Receptury -> Wczytaj":
                    glownaClick();
                    for (int i = 0; i < receptura_sklad.Items.Count; i++)
                    {
                        string[] arr = new string[11];
                        ListViewItem itm = null;
                        if (receptura_sklad.Items[i].SubItems.Count != 11)
                        {
                            arr[0] = receptura_sklad.Items[i].SubItems[0].Text;
                            arr[1] = receptura_sklad.Items[i].SubItems[1].Text;
                            arr[2] = receptura_sklad.Items[i].SubItems[2].Text;
                            arr[3] = receptura_sklad.Items[i].SubItems[3].Text;
                            arr[4] = receptura_sklad.Items[i].SubItems[4].Text;
                            arr[5] = receptura_sklad.Items[i].SubItems[5].Text;
                            arr[6] = receptura_sklad.Items[i].SubItems[6].Text;
                            arr[7] = receptura_sklad.Items[i].SubItems[7].Text;
                            arr[8] = "0";
                            arr[9] = receptura_sklad.Items[i].SubItems[9].Text;
                            arr[10] = receptura_sklad.Items[i].SubItems[10].Text;
                        }
                        else
                        {
                            arr[0] = receptura_sklad.Items[i].SubItems[0].Text;
                            arr[1] = receptura_sklad.Items[i].SubItems[1].Text;
                            arr[2] = receptura_sklad.Items[i].SubItems[2].Text;
                            arr[3] = receptura_sklad.Items[i].SubItems[3].Text;
                            arr[4] = receptura_sklad.Items[i].SubItems[4].Text;
                            arr[5] = receptura_sklad.Items[i].SubItems[5].Text;
                            arr[6] = receptura_sklad.Items[i].SubItems[6].Text;
                            arr[7] = receptura_sklad.Items[i].SubItems[7].Text;
                            arr[8] = receptura_sklad.Items[i].SubItems[8].Text;
                            arr[9] = receptura_sklad.Items[i].SubItems[9].Text;
                            arr[10] = receptura_sklad.Items[i].SubItems[10].Text;
                        }
                        itm = new ListViewItem(arr);
                        switch (receptura_posilek.SelectedItem)
                        {
                            case "Śniadanie":
                                lv_sniadanie.Items.Add(itm);
                                break;
                            case "II Śniadanie":
                                lv_IIsniadanie.Items.Add(itm);
                                break;
                            case "Obiad":
                                lv_obiad.Items.Add(itm);
                                break;
                            case "Podwieczorek":
                                lv_podwieczorek.Items.Add(itm);
                                break;
                            case "Kolacja":
                                lv_kolacja.Items.Add(itm);
                                break;
                        }
                    }
                    switch (receptura_posilek.SelectedItem)
                    {
                        case "Śniadanie":
                            textBox1.Text += " " + receptura_nazwa.Text;
                            break;
                        case "II Śniadanie":
                            textBox2.Text += " " + receptura_nazwa.Text;
                            break;
                        case "Obiad":
                            textBox3.Text += " " + receptura_nazwa.Text;
                            break;
                        case "Podwieczorek":
                            textBox4.Text += " " + receptura_nazwa.Text;
                            break;
                        case "Kolacja":
                            textBox5.Text += " " + receptura_nazwa.Text;
                            break;
                    }
                    LiczSrednia();
                    break;
            }
        }

        private void panel_receptura_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion Receptury

        private void pictureBox13_Click_1(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            glownaClick();
            //wczytania
            textBox1.Text = jadlospisDekadowkiDoWczytania.nazwa_sniadanie;
            textBox2.Text = jadlospisDekadowkiDoWczytania.nazwa_IIsniadanie;
            textBox3.Text = jadlospisDekadowkiDoWczytania.nazwa_obiad;
            textBox4.Text = jadlospisDekadowkiDoWczytania.nazwa_podwieczorek;
            textBox5.Text = jadlospisDekadowkiDoWczytania.nazwa_kolacja;
            lv_sniadanie.Items.Clear();
            lv_IIsniadanie.Items.Clear();
            lv_obiad.Items.Clear();
            lv_podwieczorek.Items.Clear();
            lv_kolacja.Items.Clear();
            string[] produkty = jadlospisDekadowkiDoWczytania.sklad_sniadanie.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                lv_sniadanie.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_IIsniadanie.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                lv_IIsniadanie.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_obiad.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                lv_obiad.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_podwieczorek.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                lv_podwieczorek.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_kolacja.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[11];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 11)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[5];
                    arg[6] = arr[6];
                    arg[7] = arr[7];
                    arg[8] = "0";
                    arg[9] = arr[8];
                    arg[10] = arr[9];
                    itm = new ListViewItem(arg);
                }
                else
                {
                    itm = new ListViewItem(arr);
                }
                lv_kolacja.Items.Add(itm);
            }

            cb_miasto.SelectedItem = jadlospisDekadowkiDoWczytania.dieta.miasto;
            cb_dieta.SelectedItem = jadlospisDekadowkiDoWczytania.dieta.nazwa;

            LiczSrednia();

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            SetRoundedRegion(p_j, borderRadius);
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            glownaClick();
            string miasto = jadlospis_miasto.SelectedItem.ToString();
            string dieta = jadlospis_dieta.SelectedItem.ToString();
            string data = ja.Text;

            Jadlospis jadlospis = DAO.JadlospisDAO.SelectAll(data, miasto, dieta);
            if (jadlospis != null)
            {
                textBox1.Text = jadlospis.nazwa_sniadanie;
                textBox2.Text = jadlospis.nazwa_IIsniadanie;
                textBox3.Text = jadlospis.nazwa_obiad;
                textBox4.Text = jadlospis.nazwa_podwieczorek;
                textBox5.Text = jadlospis.nazwa_kolacja;
                lv_sniadanie.Items.Clear();
                lv_IIsniadanie.Items.Clear();
                lv_obiad.Items.Clear();
                lv_podwieczorek.Items.Clear();
                lv_kolacja.Items.Clear();
                string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }
                    lv_sniadanie.Items.Add(itm);
                }
                produkty = jadlospis.sklad_IIsniadanie.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }
                    lv_IIsniadanie.Items.Add(itm);
                }
                produkty = jadlospis.sklad_obiad.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }
                    lv_obiad.Items.Add(itm);
                }
                produkty = jadlospis.sklad_podwieczorek.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }
                    lv_podwieczorek.Items.Add(itm);
                }
                produkty = jadlospis.sklad_kolacja.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }
                    lv_kolacja.Items.Add(itm);
                }

                cb_miasto.SelectedItem = jadlospis.miasto;
                cb_dieta.SelectedItem = jadlospis.dieta.nazwa;
            }
            LiczSrednia();
        }

        #region Jadlospis

        public void wczytajJadlospis()
        {
            jadlospis_miasto.BeginUpdate();
            jadlospis_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka d in listaJednostek)
                jadlospis_miasto.Items.Add(d.miasto);
            jadlospis_miasto.EndUpdate();

            if (jadlospis_miasto.Items.Count > 0)
                jadlospis_miasto.SelectedIndex = 0;


            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
        }

        public void wpiszJadlospis()
        {

            if (jadlospis_miasto.SelectedIndex != -1 && jadlospis_dieta.SelectedIndex != -1)
            {
                string miasto = jadlospis_miasto.SelectedItem.ToString();
                string dieta = jadlospis_dieta.SelectedItem.ToString();
                string data = ja.Text;

                Jadlospis jadlospis = DAO.JadlospisDAO.SelectAll(data, miasto, dieta);

                if (jadlospis != null)
                {
                    textBox7.Text = jadlospis.nazwa_sniadanie;
                    textBox8.Text = jadlospis.nazwa_IIsniadanie;
                    textBox9.Text = jadlospis.nazwa_obiad;
                    textBox10.Text = jadlospis.nazwa_podwieczorek;
                    textBox11.Text = jadlospis.nazwa_kolacja;
                    listView1.Items.Clear();
                    listView2.Items.Clear();
                    listView3.Items.Clear();
                    listView4.Items.Clear();
                    listView5.Items.Clear();
                    string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[11];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                        if (arr.Length != 11)
                        {
                            arg[0] = arr[0];
                            arg[1] = arr[1];
                            arg[2] = arr[2];
                            arg[3] = arr[3];
                            arg[4] = arr[4];
                            arg[5] = arr[5];
                            arg[6] = arr[6];
                            arg[7] = arr[7];
                            arg[8] = "0";
                            arg[9] = arr[8];
                            arg[10] = arr[9];
                            itm = new ListViewItem(arg);
                        }
                        else
                        {
                            itm = new ListViewItem(arr);
                        }
                        listView1.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_IIsniadanie.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[11];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                        if (arr.Length != 11)
                        {
                            arg[0] = arr[0];
                            arg[1] = arr[1];
                            arg[2] = arr[2];
                            arg[3] = arr[3];
                            arg[4] = arr[4];
                            arg[5] = arr[5];
                            arg[6] = arr[6];
                            arg[7] = arr[7];
                            arg[8] = "0";
                            arg[9] = arr[8];
                            arg[10] = arr[9];
                            itm = new ListViewItem(arg);
                        }
                        else
                        {
                            itm = new ListViewItem(arr);
                        }
                        listView2.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_obiad.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[11];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                        if (arr.Length != 11)
                        {
                            arg[0] = arr[0];
                            arg[1] = arr[1];
                            arg[2] = arr[2];
                            arg[3] = arr[3];
                            arg[4] = arr[4];
                            arg[5] = arr[5];
                            arg[6] = arr[6];
                            arg[7] = arr[7];
                            arg[8] = "0";
                            arg[9] = arr[8];
                            arg[10] = arr[9];
                            itm = new ListViewItem(arg);
                        }
                        else
                        {
                            itm = new ListViewItem(arr);
                        }
                        listView3.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_podwieczorek.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[11];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                        if (arr.Length != 11)
                        {
                            arg[0] = arr[0];
                            arg[1] = arr[1];
                            arg[2] = arr[2];
                            arg[3] = arr[3];
                            arg[4] = arr[4];
                            arg[5] = arr[5];
                            arg[6] = arr[6];
                            arg[7] = arr[7];
                            arg[8] = "0";
                            arg[9] = arr[8];
                            arg[10] = arr[9];
                            itm = new ListViewItem(arg);
                        }
                        else
                        {
                            itm = new ListViewItem(arr);
                        }
                        listView4.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_kolacja.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[11];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm;
                        if (arr.Length != 11)
                        {
                            arg[0] = arr[0];
                            arg[1] = arr[1];
                            arg[2] = arr[2];
                            arg[3] = arr[3];
                            arg[4] = arr[4];
                            arg[5] = arr[5];
                            arg[6] = arr[6];
                            arg[7] = arr[7];
                            arg[8] = "0";
                            arg[9] = arr[8];
                            arg[10] = arr[9];
                            itm = new ListViewItem(arg);
                        }
                        else
                        {
                            itm = new ListViewItem(arr);
                        }
                        listView5.Items.Add(itm);
                    }

                }
                else
                {
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    textBox10.Text = "";
                    textBox11.Text = "";
                    listView1.Items.Clear();
                    listView2.Items.Clear();
                    listView3.Items.Clear();
                    listView4.Items.Clear();
                    listView5.Items.Clear();
                }
            }
        }

        private void ja_ValueChanged(object sender, EventArgs e)
        {
            wpiszJadlospis();
        }

        private void jadlospis_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            jadlospis_dieta.BeginUpdate();
            jadlospis_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAll(jadlospis_miasto.Text);
            var sortedDiety = Diety
            .OrderBy(d =>
            {
                int index = Array.IndexOf(DietaPriority, d.nazwa);
                return index == -1 ? int.MaxValue : index;
            }).ThenBy(d => d.nazwa).ToList();

            foreach (Dieta d in sortedDiety)
                jadlospis_dieta.Items.Add(d.nazwa);
            jadlospis_dieta.EndUpdate();

            if (jadlospis_dieta.Items.Count > 0)
                jadlospis_dieta.SelectedIndex = 0;

            wpiszJadlospis();
        }

        private void jadlospis_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            wpiszJadlospis();
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            if (jadlospis_miasto.SelectedIndex != -1 && jadlospis_dieta.SelectedIndex != -1)
            {
                string miasto = jadlospis_miasto.SelectedItem.ToString();
                string dieta = jadlospis_dieta.SelectedItem.ToString();
                string data = ja.Text;

                DAO.JadlospisDAO.Delete(data, miasto, dieta);
                MessageBox.Show("Usunięto wybrany jadłospis.", "Sukces");
            }
        }

        #endregion Jadlospis
        
        private void dekadowka_generuj_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablony -> Generuj jadłospisy";
            dekadowka_generuj_label1.Visible = true;
            dekadowka_generuj_label2.Visible = true;
            dekadowka_generuj_data1.Visible = true;
            dekadowka_generuj_data2.Visible = true;
            dekadowka_generuj.Visible = false;

            dekadowka_miasto.Visible = true;
            dekadowka_panel.Visible = false;
            dekadowka_usun.Visible = false;
            dekadowka_dodaj.Visible = false;
            dekadowka_dekadowka.Visible = true;
            dekadowka_ok.Visible = true;
            dekadowka_nope.Visible = true;
            label33.Visible = false;
            label32.Visible = false;

            dekadowka_dodaj_dni.Visible = false;
            dekadowka_dodaj_label_dzienStart.Visible = false;
            dekadowka_dodaj_label_dekadowka.Visible = false;
            dekadowka_dodaj_label_dni.Visible = false;
            dekadowka_dodaj_label_miasto.Visible = false;
            dekadowka_dodaj_miasto.Visible = false;
            dekadowka_dodaj_nazwa.Visible = false;
            dekadowka_dodaj_dzienStart.Visible = false;

        }

        #region Drukuj

        private void panel11_Click(object sender, EventArgs e)
        {
            drukujClick();
        }

        private void panel11_Paint_1(object sender, PaintEventArgs e)
        {

            SetRoundedRegion(p_pr, borderRadius);
        }

        public void drukujClick()
        {
            label10.Text = "Drukowanie";
            p_pr.BackColor = highlightColor;
            p_r.BackColor = primaryColor;
            p_j.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            p_d.BackColor = primaryColor;
            p_h.BackColor = primaryColor;
            p_g.BackColor = primaryColor;
            p_p.BackColor = primaryColor;
            p_k.BackColor = primaryColor;
            p_de.BackColor = primaryColor;

            panel_drukuj.Visible = true;
            panel_drukuj.BringToFront();

            drukuj_combo.Visible = false;
            drukuj_combo_label.Visible = false;
            drukuj_data.Visible = false;
            drukuj_data_label.Visible = false;
            drukuj_do.Visible = false;
            drukuj_do_label.Visible = false;
            drukuj_od.Visible = false;
            drukuj_od_label.Visible = false;
            drukuj_rodzaj.Visible = true;
            drukuj_rodzaj_label.Visible = true;

            drukuj_rodzaj.SelectedIndex = 0;
            drukuj_rodzaj_SelectedIndexChanged(null, null);
        }

        private void label96_Click(object sender, EventArgs e)
        {
            drukujClick();
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            drukujClick();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            string miasto = drukuj_combo.SelectedItem.ToString();
            switch (drukuj_rodzaj.SelectedItem.ToString())
            {
                case "Szablon":
                    if (miasto == "Szpital")
                    { 
                        bool success = Printer.Dekadowka(miasto, "Lesko", drukuj_od.Text, drukuj_do.Text, DAO.JadlospisDAO.SelectAll(drukuj_od.Text, drukuj_do.Text));
                        bool success2 = Printer.Dekadowka(miasto, "Ustrzyki Dolne", drukuj_od.Text, drukuj_do.Text, DAO.JadlospisDAO.SelectAll(drukuj_od.Text, drukuj_do.Text));
                        if (success && success2) MessageBox.Show("Wygenerowano szablon.", "Sukces");
                    }
                    else
                    {
                        bool success = Printer.Dekadowka(miasto, miasto, drukuj_od.Text, drukuj_do.Text, DAO.JadlospisDAO.SelectAll(drukuj_od.Text, drukuj_do.Text));
                        if (success) MessageBox.Show("Wygenerowano szablon.", "Sukces");
                    }
                    break;
                case "Jadłospis":
                    Jadlospis jadlospis = DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString());

                    if (miasto == "Szpital")
                    {
                        Printer.Jadlospis(jadlospis, "Lesko");
                        Printer.Jadlospis(jadlospis, "Ustrzyki Dolne");
                        if (jadlospis.dieta.nazwa.Contains("dzieci"))
                        {
                            Jadlospis jadlospis2 = DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString());
                            DuplicateJadlospis(jadlospis2.DeepCopy(), 0.5, "Lesko");
                            DuplicateJadlospis(jadlospis2.DeepCopy(), 0.5, "Ustrzyki Dolne");
                            Jadlospis jadlospis3 = DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString());
                            DuplicateJadlospis(jadlospis3.DeepCopy(), 0.7, "Lesko");
                            DuplicateJadlospis(jadlospis3.DeepCopy(), 0.7, "Ustrzyki Dolne");
                        }
                    }
                    else
                    {
                        Printer.Jadlospis(jadlospis, miasto);
                        if (jadlospis.dieta.nazwa.Contains("dzieci"))
                        {
                            Jadlospis jadlospis2 = DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString());
                            DuplicateJadlospis(jadlospis2.DeepCopy(), 0.5, miasto);
                            Jadlospis jadlospis3 = DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString());
                            DuplicateJadlospis(jadlospis3.DeepCopy(), 0.7, miasto);
                        }
                    }
                    MessageBox.Show("Wygenerowano jadłospis.", "Sukces");
                    break;
                case "Jadłospisy w danym okresie":
                    DateTime dateFrom = Convert.ToDateTime(drukuj_od.Text);
                    DateTime dateTo = Convert.ToDateTime(drukuj_do.Text);
                    if (miasto == "Szpital")
                    {
                        for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                        {
                            string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                            List<Jadlospis> jad = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                            for (int i = 0; i < jad.Count; i++)
                            {
                                Printer.Jadlospis(jad[i], "Lesko");
                                Printer.Jadlospis(jad[i], "Ustrzyki Dolne");
                                if (jad[i].dieta.nazwa.Contains("dzieci"))
                                {
                                    List<Jadlospis> jad2 = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                                    DuplicateJadlospis(jad2[i].DeepCopy(), 0.5, "Lesko");
                                    DuplicateJadlospis(jad2[i].DeepCopy(), 0.5, "Ustrzyki Dolne");
                                    List<Jadlospis> jad3 = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                                    DuplicateJadlospis(jad3[i].DeepCopy(), 0.7, "Lesko");
                                    DuplicateJadlospis(jad2[i].DeepCopy(), 0.7, "Ustrzyki Dolne");
                                }
                            }
                        }
                    }
                    else
                    {
                        for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                        {
                            string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                            List<Jadlospis> jad = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                            for (int i = 0; i < jad.Count; i++)
                            {
                                Printer.Jadlospis(jad[i], miasto);
                                if (jad[i].dieta.nazwa.Contains("dzieci"))
                                {
                                    List<Jadlospis> jad2 = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                                    DuplicateJadlospis(jad2[i].DeepCopy(), 0.5, miasto);
                                    List<Jadlospis> jad3 = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                                    DuplicateJadlospis(jad3[i].DeepCopy(), 0.7, miasto);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Wygenerowano jadłospisy w wybranym okresie.", "Sukces");
                    break;
                case "Jadłospis dzienny":
                    DateTime dateFrom2 = Convert.ToDateTime(drukuj_od.Text);
                    DateTime dateTo2 = Convert.ToDateTime(drukuj_do.Text);
                    for (DateTime data = dateFrom2; data <= dateTo2; data = data.AddDays(1))
                    {
                        string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                        Printer.JadlospisDzienny(DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString()));
                        Printer.JadlospisNaStrone(DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString()));
                    }
                    MessageBox.Show("Wygenerowano jadłospisy dzienne i na stronę w wybranym okresie.", "Sukces");
                    break;
                case "Receptura":
                    Printer.Receptura(listaReceptur[drukuj_combo.SelectedIndex]);
                    MessageBox.Show("Wygernerowano recepturę.", "Sukces");
                    break;
                case "Produkt":
                    Printer.Produkt(Lista[drukuj_combo.SelectedIndex]);
                    MessageBox.Show("Wygernerowano produkt.", "Sukces");
                    break;
            }
        }

        private void DuplicateJadlospis(Jadlospis j3, double percentage, string miasto)
        {
            j3.dieta.nazwa = j3.dieta.nazwa + $" {percentage*100}%";

            j3.nazwa_sniadanie = Printer.ZamienGramature(j3.nazwa_sniadanie, percentage);
            string[] pr = j3.sklad_sniadanie.Split('$');
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i] != "")
                {
                    string[] wartosci = pr[i].Split('|');
                    for (int k = 1; k < wartosci.Length; k++)
                    {
                        wartosci[k] = Math.Round(Convert.ToDouble(wartosci[k]) * percentage, 3).ToString();
                    }
                    pr[i] = String.Join("|", wartosci);
                }
            }
            j3.sklad_sniadanie = String.Join("$", pr);

            j3.nazwa_IIsniadanie = Printer.ZamienGramature(j3.nazwa_IIsniadanie, percentage);
            pr = j3.sklad_IIsniadanie.Split('$');
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i] != "")
                {
                    string[] wartosci = pr[i].Split('|');
                    for (int k = 1; k < wartosci.Length; k++)
                    {
                        wartosci[k] = Math.Round(Convert.ToDouble(wartosci[k]) * percentage, 3).ToString();
                    }
                    pr[i] = String.Join("|", wartosci);
                }
            }
            j3.sklad_IIsniadanie = String.Join("$", pr);

            j3.nazwa_obiad = Printer.ZamienGramature(j3.nazwa_obiad, percentage);
            pr = j3.sklad_obiad.Split('$');
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i] != "")
                {
                    string[] wartosci = pr[i].Split('|');
                    for (int k = 1; k < wartosci.Length; k++)
                    {
                        wartosci[k] = Math.Round(Convert.ToDouble(wartosci[k]) * percentage, 3).ToString();
                    }
                    pr[i] = String.Join("|", wartosci);
                }
            }
            j3.sklad_obiad = String.Join("$", pr);

            j3.nazwa_podwieczorek = Printer.ZamienGramature(j3.nazwa_podwieczorek, percentage);
            pr = j3.sklad_podwieczorek.Split('$');
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i] != "")
                {
                    string[] wartosci = pr[i].Split('|');
                    for (int k = 1; k < wartosci.Length; k++)
                    {
                        wartosci[k] = Math.Round(Convert.ToDouble(wartosci[k]) * percentage, 3).ToString();
                    }
                    pr[i] = String.Join("|", wartosci);
                }
            }
            j3.sklad_podwieczorek = String.Join("$", pr);

            j3.nazwa_kolacja = Printer.ZamienGramature(j3.nazwa_kolacja, percentage);
            pr = j3.sklad_kolacja.Split('$');
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i] != "")
                {
                    string[] wartosci = pr[i].Split('|');
                    for (int k = 1; k < wartosci.Length; k++)
                    {
                        wartosci[k] = Math.Round(Convert.ToDouble(wartosci[k]) * percentage, 3).ToString();
                    }
                    pr[i] = String.Join("|", wartosci);
                }
            }
            j3.sklad_kolacja = String.Join("$", pr);

            Printer.Jadlospis(j3, miasto);
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void drukuj_rodzaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (drukuj_rodzaj.SelectedItem.ToString())
            {
                case "Szablon":
                    label10.Text = "Drukowanie -> Szablon";
                    drukuj_do.Visible = true;
                    drukuj_do_label.Visible = true;
                    drukuj_od.Visible = true;
                    drukuj_od_label.Visible = true;
                    drukuj_data.Visible = false;
                    drukuj_data_label.Visible = false;
                    drukuj_combo.Visible = true;
                    drukuj_combo_label.Visible = true;
                    drukuj_dieta.Visible = false;
                    drukuj_dieta_label.Visible = false;
                    drukuj_combo_label.Text = "Miasto:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    listaJednostek = JednostkaDAO.SelectAll();
                    foreach (Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    break;
                case "Jadłospis":
                    label10.Text = "Drukowanie -> Jadłospis";
                    drukuj_do.Visible = false;
                    drukuj_do_label.Visible = false;
                    drukuj_od.Visible = false;
                    drukuj_od_label.Visible = false;
                    drukuj_data.Visible = true;
                    drukuj_data_label.Visible = true;
                    drukuj_combo.Visible = true;
                    drukuj_combo_label.Visible = true;
                    drukuj_dieta.Visible = true;
                    drukuj_dieta_label.Visible = true;
                    drukuj_combo_label.Text = "Miasto:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    listaJednostek = JednostkaDAO.SelectAll();
                    foreach (Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    break;
                case "Jadłospisy w danym okresie":
                    label10.Text = "Drukowanie -> Jadłospisy w danym okresie";
                    drukuj_do.Visible = true;
                    drukuj_do_label.Visible = true;
                    drukuj_od.Visible = true;
                    drukuj_od_label.Visible = true;
                    drukuj_data.Visible = false;
                    drukuj_data_label.Visible = false;
                    drukuj_dieta.Visible = false;
                    drukuj_dieta_label.Visible = false;
                    drukuj_combo_label.Text = "Miasto:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    listaJednostek = JednostkaDAO.SelectAll();
                    foreach (Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    break;
                case "Jadłospis dzienny":
                    label10.Text = "Drukowanie -> Jadłospis dzienny";
                    drukuj_data.Visible = false;
                    drukuj_data_label.Visible = false;
                    drukuj_do.Visible = true;
                    drukuj_do_label.Visible = true;
                    drukuj_od.Visible = true;
                    drukuj_od_label.Visible = true;
                    drukuj_combo.Visible = true;
                    drukuj_combo_label.Visible = true;
                    drukuj_combo_label.Text = "Miasto:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    listaJednostek = JednostkaDAO.SelectAll();
                    foreach (Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    drukuj_dieta.Visible = false;
                    drukuj_dieta_label.Visible = false;
                    break;
                case "Receptura":
                    label10.Text = "Drukowanie -> Receptura";
                    drukuj_combo.Visible = true;
                    drukuj_combo_label.Visible = true;
                    drukuj_do.Visible = false;
                    drukuj_do_label.Visible = false;
                    drukuj_od.Visible = false;
                    drukuj_od_label.Visible = false;
                    drukuj_data.Visible = false;
                    drukuj_data_label.Visible = false;
                    drukuj_combo_label.Text = "Receptura:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    listaReceptur = DAO.RecepturaDAO.SelectAll();
                    foreach (Receptura r in listaReceptur)
                        drukuj_combo.Items.Add(r.nazwa);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    drukuj_dieta.Visible = false;
                    drukuj_dieta_label.Visible = false;
                    break;
                case "Produkt":
                    label10.Text = "Drukowanie -> Produkt";
                    drukuj_combo.Visible = true;
                    drukuj_do.Visible = false;
                    drukuj_do_label.Visible = false;
                    drukuj_od.Visible = false;
                    drukuj_od_label.Visible = false;
                    drukuj_data.Visible = false;
                    drukuj_data_label.Visible = false;
                    drukuj_combo_label.Visible = true;
                    drukuj_combo_label.Text = "Produkt:";
                    drukuj_combo.BeginUpdate();
                    drukuj_combo.Items.Clear();
                    foreach (Produkt r in Lista)
                        drukuj_combo.Items.Add(r.nazwa);
                    drukuj_combo.EndUpdate();
                    if (drukuj_combo.Items.Count > 0)
                        drukuj_combo.SelectedIndex = 0;
                    drukuj_dieta.Visible = false;
                    drukuj_dieta_label.Visible = false;
                    break;
            }
        }

        private void drukuj_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Drukowanie -> Szablon":
                    break;
                case "Drukowanie -> Jadłospis":
                    drukuj_dieta.BeginUpdate();
                    drukuj_dieta.Items.Clear();
                    Diety = DAO.DietaDAO.SelectAll(drukuj_combo.SelectedItem.ToString());
                    foreach (Dieta r in Diety)
                        drukuj_dieta.Items.Add(r.nazwa);
                    drukuj_dieta.EndUpdate();
                    if (drukuj_dieta.Items.Count > 0)
                        drukuj_dieta.SelectedIndex = 0;
                    break;
                case "Drukowanie -> Jadłospisy w danym okresie":
                    break;
            }
        }

        #endregion Drukuj

        private void panel_dekadowka_zapisz_Paint(object sender, PaintEventArgs e)
        {

        }


        private void tb_masa_TextChanged(object sender, EventArgs e)
        {

        }

        private void receptura_masa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    receptura_produkt_dodaj_Click(sender, e);
                    e.Handled = true;

                }
                else
                    e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_nazwa_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void produkt_energia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_bialko_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_tluszcze_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_tluszcze_nn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_weglowodany_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkty_przyswajalne_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkty_blonnik_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_sod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void produkt_sol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dekadowka_dodaj_dni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_energia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_bialko_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_tluszcze_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_ktn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_weglowodany_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_przyswajalne_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_blonnik_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_sod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dieta_sol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
        }

        private void dekadowka_wczytaj_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), wybranaDekadowkaDoWczytania.miasto, dekadowka_wczytaj_dzien.SelectedIndex + 1);
                foreach (Jadlospis d in jadlospisyDanegoDnia)
                {
                    if (d.dzien - 1 == dekadowka_wczytaj_dzien.SelectedIndex && d.dieta.nazwa == dekadowka_wczytaj_dieta.SelectedItem.ToString())
                    {
                        dekadowka_wczytaj_dieta.Items.Add(d.dieta.nazwa);
                        jadlospisDekadowkiDoWczytania = d;
                    }
                }
            }
        }

        #region Kontrola

        private void label58_Click(object sender, EventArgs e)
        {
            KontrolaClick();
        }

        private void pictureBox26_Click_1(object sender, EventArgs e)
        {
            KontrolaClick();
        }

        private void panel14_Click(object sender, EventArgs e)
        {
            KontrolaClick();
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            if (k_miasto.SelectedIndex != -1 && k_dieta.SelectedIndex != -1)
            {
                string miasto = k_miasto.SelectedItem.ToString();
                string dieta = k_dieta.SelectedItem.ToString();
                string data_od = dateTimePicker4.Text;
                string data_do = dateTimePicker3.Text;

                List<Jadlospis> jadlospisy = DAO.JadlospisDAO.SelectAll(data_od, data_do, miasto, dieta);
                LiczSredniaKontrola(jadlospisy);
            }
        }

        public void LiczSredniaKontrola(List<Jadlospis> jadlospisy)
        {
            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma[i, k] = 0;
                    procent[i, k] = 0;
                }
            }

            foreach (Jadlospis jadlospis in jadlospisy)
            {
                string[] produkty = jadlospis.sklad_sniadanie.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }

                    for (int k = 0; k < 9; k++)
                    {
                        double a = 0;
                        try
                        {
                            a = double.Parse(itm.SubItems[k + 2].Text);
                        }
                        catch { }
                        suma[0, k] += a;
                    }
                }

                produkty = jadlospis.sklad_IIsniadanie.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }

                    for (int k = 0; k < 9; k++)
                    {
                        double a = 0;
                        try
                        {
                            a = double.Parse(itm.SubItems[k + 2].Text);
                        }
                        catch { }
                        suma[0, k] += a;
                    }
                }

                produkty = jadlospis.sklad_obiad.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }

                    for (int k = 0; k < 9; k++)
                    {
                        double a = 0;
                        try
                        {
                            a = double.Parse(itm.SubItems[k + 2].Text);
                        }
                        catch { }
                        suma[0, k] += a;
                    }
                }

                produkty = jadlospis.sklad_podwieczorek.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }

                    for (int k = 0; k < 9; k++)
                    {
                        double a = 0;
                        try
                        {
                            a = double.Parse(itm.SubItems[k + 2].Text);
                        }
                        catch { }
                        suma[0, k] += a;
                    }
                }

                produkty = jadlospis.sklad_kolacja.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[11];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm;
                    if (arr.Length != 11)
                    {
                        arg[0] = arr[0];
                        arg[1] = arr[1];
                        arg[2] = arr[2];
                        arg[3] = arr[3];
                        arg[4] = arr[4];
                        arg[5] = arr[5];
                        arg[6] = arr[6];
                        arg[7] = arr[7];
                        arg[8] = "0";
                        arg[9] = arr[8];
                        arg[10] = arr[9];
                        itm = new ListViewItem(arg);
                    }
                    else
                    {
                        itm = new ListViewItem(arr);
                    }

                    for (int k = 0; k < 10; k++)
                    {
                        double a = 0;
                        try
                        {
                            a = double.Parse(itm.SubItems[k + 2].Text);
                        }
                        catch { }
                        suma[0, k] += a;
                    }
                }
            }

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    suma[5, k] += suma[i, k];
                }
            }

            for (int k = 0; k < 10; k++)
            {
                suma[5, k] = suma[5, k] / jadlospisy.Count;
            }

            //WARTOŚCI
            k_energia.Text = Math.Round(suma[5, 0], 2).ToString();
            k_bialko.Text = Math.Round(suma[5, 1], 2).ToString();
            k_tluszcze.Text = Math.Round(suma[5, 2], 2).ToString();
            k_kwasy.Text = Math.Round(suma[5, 3], 2).ToString() ;
            k_wegle.Text = Math.Round(suma[5, 4], 2).ToString();
            k_przyswajalne.Text = Math.Round(suma[5, 5], 2).ToString();
            k_cukry.Text = Math.Round(suma[5, 6], 2).ToString() ;
            k_blonnik.Text = Math.Round(suma[5, 7], 2).ToString();
            k_sod.Text = Math.Round(suma[5, 8], 2).ToString() ;
            k_sol.Text = Math.Round(suma[5, 8]*0.0025, 2).ToString() ;

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (suma[i, 0] != 0)
                    {
                        double wartosc_odzywcza = suma[i, k];
                        double przelicznik = 0;
                        if (k == 1)
                            przelicznik = przelicznik_Bialko;
                        if (k == 2 || k == 3)
                            przelicznik = przelicznik_Tluszcze;
                        if (k == 4 || k == 5 || k == 6 || k == 7)
                            przelicznik = przelicznik_Weglowodany;

                        procent[i, k] = (wartosc_odzywcza * przelicznik * 100.0) / suma[i, 0];
                    }
                }
            }

            //PROCENTY
            double bialkoProcent = Math.Round(procent[5, 1], 2);
            k_bialko_procent.Text = bialkoProcent.ToString();
            k_bialko_procent.ForeColor = Color.DarkGray;

            double tluszczeProcent = Math.Round(procent[5, 2], 2);
            k_tluszcze_procent.Text = tluszczeProcent.ToString();
            k_tluszcze_procent.ForeColor = Color.DarkGray;

            double kwasyProcent = Math.Round(procent[5, 3], 2);
            k_kwasy_procent.Text = kwasyProcent.ToString();
            k_kwasy_procent.ForeColor = Color.DarkGray;

            double wegleProcent = Math.Round(procent[5, 4], 2);
            k_wegle_procent.Text = wegleProcent.ToString();
            k_wegle_procent.ForeColor = Color.DarkGray;

            double przyswajalneProcent = Math.Round(procent[5, 5], 2);
            k_przyswajalne_procent.Text = przyswajalneProcent.ToString();
            k_przyswajalne_procent.ForeColor = Color.DarkGray;

            double cukryProcent = Math.Round(procent[5, 6], 2);
            k_cukry_procent.Text = cukryProcent.ToString();
            k_cukry_procent.ForeColor = Color.DarkGray;

            double blonnikProcent = Math.Round(procent[5, 7], 2);
            k_blonnik_procent.Text = blonnikProcent.ToString();
            k_blonnik_procent.ForeColor = Color.DarkGray;


            //NA TYSIAC
            double bialkoNaTysiac = Math.Round(suma[5, 1] * 1000.0 / suma[5, 0], 2);
            k_bialko_tysiac.Text = bialkoNaTysiac.ToString();
            k_bialko_tysiac.ForeColor = Color.DarkGray;

            double tluszczeNaTysiac = Math.Round(suma[5, 2] * 1000.0 / suma[5, 0], 2);
            k_tluszcze_tysiac.Text = tluszczeNaTysiac.ToString();
            k_tluszcze_tysiac.ForeColor = Color.DarkGray;

            double kwasyNaTysiac = Math.Round(suma[5, 3] * 1000.0 / suma[5, 0], 2);
            k_kwasy_tysiac.Text = kwasyNaTysiac.ToString();
            k_kwasy_tysiac.ForeColor = Color.DarkGray;

            double wegleNaTysiac = Math.Round(suma[5, 4] * 1000.0 / suma[5, 0], 2);
            k_wegle_tysiac.Text = wegleNaTysiac.ToString();
            k_wegle_tysiac.ForeColor = Color.DarkGray;

            double przyswajalneNaTysiac = Math.Round(suma[5, 5] * 1000.0 / suma[5, 0], 2);
            k_przyswajalne_tysiac.Text = przyswajalneNaTysiac.ToString();
            k_przyswajalne_tysiac.ForeColor = Color.DarkGray;

            double cukryNaTysiac = Math.Round(suma[5, 6] * 1000.0 / suma[5, 0], 2);
            k_cukry_tysiac.Text = cukryNaTysiac.ToString();
            k_cukry_tysiac.ForeColor = Color.DarkGray;

            double blonnikNaTysiac = Math.Round(suma[5, 7] * 1000.0 / suma[5, 0], 2);
            k_blonnik_tysiac.Text = blonnikNaTysiac.ToString();
            k_blonnik_tysiac.ForeColor = Color.DarkGray;

            //ZAWARTOSC
            try
            {
                if (k_dieta.SelectedIndex != -1)
                {
                    if (Diety[k_dieta.SelectedIndex].energiaDo != 0)
                    {
                        k_energia_zakres.Text = $"{Diety[k_dieta.SelectedIndex].energiaOd.ToString()} - {Diety[k_dieta.SelectedIndex].energiaDo.ToString()}";
                        if (suma[5, 0] > Diety[k_dieta.SelectedIndex].energiaDo)
                        {
                            k_energia_plus.Text = "+ " + Math.Round(suma[5, 0] - Diety[k_dieta.SelectedIndex].energiaDo, 2);
                            if (suma[5, 0] > Diety[cb_dieta.SelectedIndex].energiaDo * 1.1)
                                k_energia_plus.ForeColor = Color.Red;
                            else
                                k_energia_plus.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 0] < Diety[k_dieta.SelectedIndex].energiaOd)
                        {
                            k_energia_plus.Text =  Math.Round(suma[5, 0] - Diety[k_dieta.SelectedIndex].energiaOd, 2).ToString();
                            if (suma[5, 0] < Diety[cb_dieta.SelectedIndex].energiaOd * 0.9)
                                k_energia_plus.ForeColor = Color.Red;
                            else
                                k_energia_plus.ForeColor = Color.Orange;
                        }
                        else
                        {
                            k_energia_plus.Text = "OK";
                            k_energia_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_energia_plus.Text = "";
                        k_energia_zakres.Text = "";
                        k_energia_plus.ForeColor = Color.DarkGray;
                    }

                    if (Diety[k_dieta.SelectedIndex].bialkoDo != 0)
                    {
                        k_bialko_zakres.Text = $"{Diety[k_dieta.SelectedIndex].bialkoOd.ToString()} - {Diety[k_dieta.SelectedIndex].bialkoDo.ToString()}";
                        if (suma[5, 1] > Diety[k_dieta.SelectedIndex].bialkoDo)
                        {
                            k_bialko_plus.Text = "+ " + Math.Round(suma[5, 1] - Diety[k_dieta.SelectedIndex].bialkoDo, 2);
                            if (suma[5, 1] > Diety[cb_dieta.SelectedIndex].bialkoDo * 1.1)
                                k_bialko_plus.ForeColor = Color.Red;
                            else
                                k_bialko_plus.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 1] < Diety[k_dieta.SelectedIndex].bialkoOd)
                        {
                            k_bialko_plus.Text =  Math.Round(suma[5, 1] - Diety[k_dieta.SelectedIndex].bialkoOd, 2).ToString();
                            if (suma[5, 1] < Diety[cb_dieta.SelectedIndex].bialkoOd * 0.9)
                                k_bialko_plus.ForeColor = Color.Red;
                            else
                                k_bialko_plus.ForeColor = Color.Orange; ;
                        }
                        else
                        {
                            k_bialko_plus.Text = "OK";
                            k_bialko_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_bialko_plus.Text = "";
                        k_bialko_zakres.Text = "";
                        k_bialko_plus.ForeColor = Color.DarkGray;
                    }

                    if (Diety[k_dieta.SelectedIndex].bialkoDoNaTysiąc != 0)
                    {
                        k_bialko_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].bialkoOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].bialkoDoNaTysiąc}";
                    }
                    else
                    {
                        k_bialko_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].bialkoProcentDo != 0)
                    {
                        k_bialko_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].bialkoProcentOd} - {Diety[k_dieta.SelectedIndex].bialkoProcentDo} % kcal";
                        if (bialkoProcent > Diety[cb_dieta.SelectedIndex].bialkoProcentDo * 1.1)
                        {
                            k_bialko_procent.ForeColor = Color.Red;
                        }
                        else if (bialkoProcent > Diety[cb_dieta.SelectedIndex].bialkoProcentDo)
                        {
                            k_bialko_procent.ForeColor = Color.Orange;
                        }
                        else if (bialkoProcent < Diety[cb_dieta.SelectedIndex].bialkoProcentOd * 0.9)
                        {
                            k_bialko_procent.ForeColor = Color.Red;
                        }
                        else if (bialkoProcent < Diety[cb_dieta.SelectedIndex].bialkoProcentOd)
                        {
                            k_bialko_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            k_bialko_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_bialko_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].tluszczeDo != 0)
                    {
                        k_tluszcze_zakres.Text = $"{Diety[k_dieta.SelectedIndex].tluszczeOd.ToString()} - {Diety[k_dieta.SelectedIndex].tluszczeDo.ToString()}";
                        if (suma[5, 2] > Diety[k_dieta.SelectedIndex].tluszczeDo)
                        {
                            k_tluszcze_plus.Text = "+ " + Math.Round(suma[5, 2] - Diety[k_dieta.SelectedIndex].tluszczeDo, 2);
                            k_tluszcze_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 2] < Diety[k_dieta.SelectedIndex].tluszczeOd)
                        {
                            k_tluszcze_plus.Text =  Math.Round(suma[5, 2] - Diety[k_dieta.SelectedIndex].tluszczeOd, 2).ToString();
                            k_tluszcze_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_tluszcze_plus.Text = "OK";
                            k_tluszcze_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_tluszcze_plus.Text = "";
                        k_tluszcze_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].tluszczeDoNaTysiąc != 0)
                    {
                        k_tluszcze_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].tluszczeOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].tluszczeDoNaTysiąc}";
                        if (tluszczeNaTysiac > Diety[k_dieta.SelectedIndex].tluszczeDoNaTysiąc)
                        {
                            k_tluszcze_tysiac.ForeColor = Color.Red;
                        }
                        else if (tluszczeNaTysiac < Diety[k_dieta.SelectedIndex].tluszczeOdNaTysiąc)
                        {
                            k_tluszcze_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_tluszcze_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_tluszcze_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].tluszczeProcentDo != 0)
                    {
                        k_tluszcze_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].tluszczeProcentOd} - {Diety[k_dieta.SelectedIndex].tluszczeProcentDo}";
                        if (tluszczeProcent > Diety[k_dieta.SelectedIndex].tluszczeProcentDo)
                        {
                            k_tluszcze_procent.ForeColor = Color.Red;
                        }
                        else if (tluszczeProcent < Diety[k_dieta.SelectedIndex].tluszczeProcentOd)
                        {
                            k_tluszcze_procent.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_tluszcze_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_tluszcze_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].kwasyDo != 0)
                    {
                        k_kwasy_zakres.Text = $"{Diety[k_dieta.SelectedIndex].kwasyOd.ToString()} - {Diety[k_dieta.SelectedIndex].kwasyDo.ToString()}";
                        if (suma[5, 3] > Diety[k_dieta.SelectedIndex].kwasyDo)
                        {
                            k_kwasy_plus.Text = "+ " + Math.Round(suma[5, 3] - Diety[k_dieta.SelectedIndex].kwasyDo, 2);
                            k_kwasy_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 3] < Diety[k_dieta.SelectedIndex].kwasyOd)
                        {
                            k_kwasy_plus.Text =  Math.Round(suma[5, 3] - Diety[k_dieta.SelectedIndex].kwasyOd, 2).ToString();
                            k_kwasy_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_kwasy_plus.Text = "OK";
                            k_kwasy_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_kwasy_plus.Text = "";
                        k_kwasy_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].kwasyDoNaTysiąc != 0)
                    {
                        k_kwasy_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].kwasyOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].kwasyDoNaTysiąc}";
                        if (kwasyNaTysiac > Diety[k_dieta.SelectedIndex].kwasyDoNaTysiąc)
                        {
                            k_kwasy_tysiac.ForeColor = Color.Red;
                        }
                        else if (kwasyNaTysiac < Diety[k_dieta.SelectedIndex].kwasyOdNaTysiąc)
                        {
                            k_kwasy_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_kwasy_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_kwasy_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].kwasyProcentDo != 0)
                    {
                        k_kwasy_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].kwasyProcentOd} - {Diety[k_dieta.SelectedIndex].kwasyProcentDo}";
                        if (kwasyProcent > Diety[k_dieta.SelectedIndex].kwasyProcentDo)
                        {
                            k_kwasy_procent.ForeColor = Color.Red;
                        }
                        else if (kwasyProcent < Diety[k_dieta.SelectedIndex].kwasyProcentOd)
                        {
                            k_kwasy_procent.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_kwasy_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_kwasy_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].wegleDo != 0)
                    {
                        k_wegle_zakres.Text = $"{Diety[k_dieta.SelectedIndex].wegleOd.ToString()} - {Diety[k_dieta.SelectedIndex].wegleDo.ToString()}";
                        if (suma[5, 4] > Diety[k_dieta.SelectedIndex].wegleDo)
                        {
                            k_wegle_plus.Text = "+ " + Math.Round(suma[5, 4] - Diety[k_dieta.SelectedIndex].wegleDo, 2);
                            k_wegle_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 4] < Diety[k_dieta.SelectedIndex].wegleOd)
                        {
                            k_wegle_plus.Text =  Math.Round(suma[5, 4] - Diety[k_dieta.SelectedIndex].wegleOd, 2).ToString();
                            k_wegle_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_wegle_plus.Text = "OK";
                            k_wegle_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_wegle_plus.Text = "";
                        k_wegle_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].wegleDoNaTysiąc != 0)
                    {
                        k_wegle_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].wegleOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].wegleDoNaTysiąc}";
                        if (wegleNaTysiac > Diety[k_dieta.SelectedIndex].wegleDoNaTysiąc)
                        {
                            k_wegle_tysiac.ForeColor = Color.Red;
                        }
                        else if (wegleNaTysiac < Diety[k_dieta.SelectedIndex].wegleOdNaTysiąc)
                        {
                            k_wegle_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_wegle_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_wegle_tysiac.Text = "";
                        k_wegle_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].wegleProcentDo != 0)
                    {
                        k_wegle_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].wegleProcentOd} - {Diety[k_dieta.SelectedIndex].wegleProcentDo}";
                        if (wegleProcent > Diety[k_dieta.SelectedIndex].wegleProcentDo)
                        {
                            k_wegle_procent.ForeColor = Color.Red;
                        }
                        else if (wegleProcent < Diety[k_dieta.SelectedIndex].wegleProcentOd)
                        {
                            k_wegle_procent.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_wegle_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_wegle_procent.Text = "";
                        k_wegle_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].przyswajalneDo != 0)
                    {
                        k_przyswajalne_zakres.Text = $"{Diety[k_dieta.SelectedIndex].przyswajalneOd.ToString()} - {Diety[k_dieta.SelectedIndex].przyswajalneDo.ToString()}";
                        if (suma[5, 5] > Diety[k_dieta.SelectedIndex].przyswajalneDo)
                        {
                            k_przyswajalne_plus.Text = "+ " + Math.Round(suma[5, 5] - Diety[k_dieta.SelectedIndex].przyswajalneDo, 2);
                            k_przyswajalne_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 5] < Diety[k_dieta.SelectedIndex].przyswajalneOd)
                        {
                            k_przyswajalne_plus.Text =  Math.Round(suma[5, 5] - Diety[k_dieta.SelectedIndex].przyswajalneOd, 2).ToString();
                            k_przyswajalne_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_przyswajalne_plus.Text = "OK";
                            k_przyswajalne_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_przyswajalne_plus.Text = "";
                        k_przyswajalne_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].przyswajalneDoNaTysiąc != 0)
                    {
                        k_przyswajalne_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].przyswajalneOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].przyswajalneDoNaTysiąc}";
                        if (przyswajalneNaTysiac > Diety[k_dieta.SelectedIndex].przyswajalneDoNaTysiąc)
                        {
                            k_przyswajalne_tysiac.ForeColor = Color.Red;
                        }
                        else if (przyswajalneNaTysiac < Diety[k_dieta.SelectedIndex].przyswajalneOdNaTysiąc)
                        {
                            k_przyswajalne_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_przyswajalne_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_przyswajalne_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].przyswajalneProcentDo != 0)
                    {
                        k_przyswajalne_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].przyswajalneProcentOd} - {Diety[k_dieta.SelectedIndex].przyswajalneProcentDo}";
                        if (przyswajalneProcent > Diety[k_dieta.SelectedIndex].przyswajalneProcentDo)
                        {
                            k_przyswajalne_procent.ForeColor = Color.Red;
                        }
                        else if (przyswajalneProcent < Diety[k_dieta.SelectedIndex].przyswajalneProcentOd)
                        {
                            k_przyswajalne_procent.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_przyswajalne_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_przyswajalne_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].cukryDo != 0)
                    {
                        k_cukry_zakres.Text = $"{Diety[k_dieta.SelectedIndex].cukryOd.ToString()} - {Diety[k_dieta.SelectedIndex].cukryDo.ToString()}";
                        if (suma[5, 6] > Diety[k_dieta.SelectedIndex].cukryDo)
                        {
                            k_cukry_plus.Text = "+ " + Math.Round(suma[5, 6] - Diety[k_dieta.SelectedIndex].cukryDo, 2);
                            k_cukry_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 6] < Diety[k_dieta.SelectedIndex].cukryOd)
                        {
                            k_cukry_plus.Text =  Math.Round(suma[5, 6] - Diety[k_dieta.SelectedIndex].cukryOd, 2).ToString();
                            k_cukry_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_cukry_plus.Text = "OK";
                            k_cukry_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_cukry_plus.Text = "";
                        k_cukry_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].cukryDoNaTysiąc != 0)
                    {
                        k_cukry_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].cukryOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].cukryDoNaTysiąc}";
                        if (cukryNaTysiac > Diety[k_dieta.SelectedIndex].cukryDoNaTysiąc)
                        {
                            k_cukry_tysiac.ForeColor = Color.Red;
                        }
                        else if (cukryNaTysiac < Diety[k_dieta.SelectedIndex].cukryOdNaTysiąc)
                        {
                            k_cukry_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_cukry_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_cukry_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].cukryProcentDo != 0)
                    {
                        k_cukry_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].cukryProcentOd} - {Diety[k_dieta.SelectedIndex].cukryProcentDo}";
                        if (cukryProcent > Diety[k_dieta.SelectedIndex].cukryProcentDo)
                        {
                            k_cukry_procent.ForeColor = Color.Red;
                        }
                        else if (cukryProcent < Diety[k_dieta.SelectedIndex].cukryProcentOd)
                        {
                            k_cukry_procent.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_cukry_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_cukry_procent_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].blonnikDo != 0)
                    {
                        k_blonnik_zakres.Text = $"{Diety[k_dieta.SelectedIndex].blonnikOd.ToString()} - {Diety[k_dieta.SelectedIndex].blonnikDo.ToString()}";
                        if (suma[5, 7] > Diety[k_dieta.SelectedIndex].blonnikDo)
                        {
                            k_blonnik_plus.Text = "+ " + Math.Round(suma[5, 7] - Diety[k_dieta.SelectedIndex].blonnikDo, 2);
                            k_blonnik_plus.ForeColor = Color.Red;
                        }
                        else if (suma[5, 7] < Diety[k_dieta.SelectedIndex].blonnikOd)
                        {
                            k_blonnik_plus.Text =  Math.Round(suma[5, 7] - Diety[k_dieta.SelectedIndex].blonnikOd, 2).ToString();
                            k_blonnik_plus.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_blonnik_plus.Text = "OK";
                            k_blonnik_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_blonnik_plus.Text = "";
                        k_blonnik_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].blonnikDoNaTysiąc != 0)
                    {
                        k_blonnik_tysiac_zakres.Text = $"{Diety[k_dieta.SelectedIndex].blonnikOdNaTysiąc} - {Diety[k_dieta.SelectedIndex].blonnikDoNaTysiąc}";
                        if (blonnikNaTysiac > Diety[k_dieta.SelectedIndex].blonnikDoNaTysiąc)
                        {
                            k_blonnik_tysiac.ForeColor = Color.Red;
                        }
                        else if (blonnikNaTysiac < Diety[k_dieta.SelectedIndex].blonnikOdNaTysiąc)
                        {
                            k_blonnik_tysiac.ForeColor = Color.Red;
                        }
                        else
                        {
                            k_blonnik_tysiac.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_blonnik_tysiac_zakres.Text = "";
                    }

                    if (Diety[k_dieta.SelectedIndex].blonnikProcentDo != 0)
                    {
                        k_blonnik_procent_zakres.Text = $"{Diety[k_dieta.SelectedIndex].blonnikProcentOd} - {Diety[k_dieta.SelectedIndex].blonnikProcentDo} % kcal";
                        if (blonnikProcent > Diety[cb_dieta.SelectedIndex].blonnikProcentDo * 1.1)
                        {
                            k_blonnik_procent.ForeColor = Color.Red;
                        }
                        else if (blonnikProcent > Diety[cb_dieta.SelectedIndex].blonnikProcentDo)
                        {
                            k_blonnik_procent.ForeColor = Color.Orange;
                        }
                        else if (blonnikProcent < Diety[cb_dieta.SelectedIndex].blonnikProcentOd * 0.9)
                        {
                            k_blonnik_procent.ForeColor = Color.Red;
                        }
                        else if (blonnikProcent < Diety[cb_dieta.SelectedIndex].blonnikProcentOd)
                        {
                            k_blonnik_procent.ForeColor = Color.Orange;
                        }
                        else
                        {
                            k_blonnik_procent.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_blonnik_procent_zakres.Text = "";
                        k_blonnik_procent.ForeColor = Color.DarkGray;
                    }


                    if (Diety[k_dieta.SelectedIndex].sodDo != 0)
                    {
                        k_sod_zakres.Text = $"{Diety[k_dieta.SelectedIndex].sodOd.ToString()} - {Diety[k_dieta.SelectedIndex].sodDo.ToString()}";
                        if (suma[5, 8] > Diety[k_dieta.SelectedIndex].sodDo)
                        {
                            k_sod_plus.Text = "+ " + Math.Round(suma[5, 8] - Diety[k_dieta.SelectedIndex].sodDo, 2);
                            if (suma[5, 8] > Diety[cb_dieta.SelectedIndex].sodDo * 1.1)
                                k_sod_plus.ForeColor = Color.Red;
                            else
                                k_sod_plus.ForeColor = Color.Orange;
                        }
                        else if (suma[5, 8] < Diety[k_dieta.SelectedIndex].sodOd)
                        {
                            k_sod_plus.Text =  Math.Round(suma[5, 8] - Diety[k_dieta.SelectedIndex].sodOd, 2).ToString();
                            if (suma[5, 8] < Diety[cb_dieta.SelectedIndex].sodOd * 0.9)
                                k_sod_plus.ForeColor = Color.Red;
                            else
                                k_sod_plus.ForeColor = Color.Orange;
                        }
                        else
                        {
                            k_sod_plus.Text = "OK";
                            k_sod_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_sod_plus.Text = "";
                        k_sod_zakres.Text = "";
                        k_sod_plus.ForeColor = Color.DarkGray;
                    }


                    if (Diety[k_dieta.SelectedIndex].solDo != 0)
                    {
                        k_sol_zakres.Text = $"{Diety[k_dieta.SelectedIndex].solOd.ToString()} - {Diety[k_dieta.SelectedIndex].solDo.ToString()}";
                        if ((Math.Round(suma[5, 8] * 0.0025, 2)) > Diety[k_dieta.SelectedIndex].solDo)
                        {
                            k_sol_plus.Text = "+ " + Math.Round(suma[5, 8] * 0.0025 - Diety[k_dieta.SelectedIndex].solDo, 2);
                            if (Math.Round(suma[5, 8] * 0.0025, 2) > Diety[cb_dieta.SelectedIndex].solDo * 1.1)
                                k_sol_plus.ForeColor = Color.Red;
                            else
                                k_sol_plus.ForeColor = Color.Orange;
                        }
                        else if ((Math.Round(suma[5, 8] * 0.0025, 2)) < Diety[k_dieta.SelectedIndex].solOd)
                        {
                            k_sol_plus.Text =  Math.Round(suma[5, 8] * 0.0025 - Diety[k_dieta.SelectedIndex].solOd, 2).ToString();
                            if (Math.Round(suma[5, 8] * 0.0025, 2) < Diety[cb_dieta.SelectedIndex].solDo * 0.9)
                                k_sol_plus.ForeColor = Color.Red;
                            else
                                k_sol_plus.ForeColor = Color.Orange;
                        }
                        else
                        {
                            k_sol_plus.Text = "OK";
                            k_sol_plus.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        k_sol_plus.Text = "";
                        k_sol_zakres.Text = "";
                        k_sol_plus.ForeColor = Color.DarkGray;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można przeliczyć wartości, o które przekroczono limity diety,\r\n{ex.Message}.", "Błąd");
            }
        }

        #endregion

        private void jadlospis_cb_ktn_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            SetRoundedRegion(p_g, borderRadius);
        }

        private void SetRoundedRegion(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            SetRoundedRegion(p_p, borderRadius);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        { 
            SetRoundedRegion(p_r, borderRadius);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lv_sniadanie_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }

        private void lb_produkty_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (e.Index < 0) return;

            string text = lb.Items[e.Index].ToString();

            // Account for the vertical scrollbar potentially showing
            int availableWidth = lb.Width - SystemInformation.VerticalScrollBarWidth - 4;
            if (availableWidth < 1) availableWidth = 1;

            SizeF textSize = e.Graphics.MeasureString(text, lb.Font, availableWidth);

            e.ItemHeight = (int)Math.Ceiling(textSize.Height) + 4; // small padding
        }

        private void lb_produkty_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox lb = (ListBox)sender;
            string text = lb.Items[e.Index].ToString();

            e.DrawBackground();

            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds, sf);
            }

            e.DrawFocusRectangle();
        }

        private void p_d_Paint(object sender, PaintEventArgs e)
        {

            SetRoundedRegion(p_d, borderRadius);
        }

        private void p_h_Paint(object sender, PaintEventArgs e)
        {

            SetRoundedRegion(p_h, borderRadius);
        }

        private void p_de_Paint(object sender, PaintEventArgs e)
        {

            SetRoundedRegion(p_de, borderRadius);
        }

        private void p_k_Paint(object sender, PaintEventArgs e)
        {

            SetRoundedRegion(p_k, borderRadius);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            SetRoundedRegion(panel1, 45);
        }
    }
}
