﻿namespace KalkulatorDiety
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using KalkulatorDiety.DAO;

    public partial class Form1 : Form
    {
        #region Zmienne 

        private int wybranaDieta;
        private int wybraneMiasto;
        private string kategoria;

        public static double przelicznik_Bialko = 4; //kcal na 1g
        public static double przelicznik_Weglowodany = 4; //kcal na 1g
        public static double przelicznik_Tluszcze = 9; //kcal na 1g

        private readonly Color highlightColor = Color.LightBlue; 
        private readonly Color primaryColor = Color.FromArgb(44, 57, 64);

        private double[,] suma;
        private double[,] procent;

        private List<Receptura> listaReceptur;
        private List<Dekadowka> listaDekadowekDoWczytania;
        private Dekadowka wybranaDekadowkaDoWczytania;
        private Jadlospis jadlospisDekadowkiDoWczytania;
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

        private readonly int[] dekadowkaSize = new int[] { 900, 470 };
        private readonly int[] dzienSize = new int[] { 150, 400 };
        private readonly int[] dietaSize = new int[] { 140, 200 };

        private readonly KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        private readonly String XML_Location = @"DataBase.xml";

        #endregion

        #region Obsługa aplikacji

        public Form1()
        {
            InitializeComponent();
            DAO.DAO.ReloadDatabase();
            this.WindowState = FormWindowState.Maximized;
            suma = new double[6, 9];
            procent = new double[6, 9];

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
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Usun_Click(sender, e);
                }
            }
        }

        #endregion

        #region Strona Główna

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
                        MessageBox.Show("Nieprawidłowa wartość", "Błąd");
                    }
                    LiczSrednia();
                }
                else
                {
                    MessageBox.Show("Nie wpisano masy produktu", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu", "Błąd");
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
                MessageBox.Show("Nie wybrano produktu", "Błąd");
            }
        }

        public void LiczSrednia()
        {
            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma[i, k] = 0;
                    procent[i, k] = 0;
                }

            }

            string[] arr = new string[9];
            for (int i = 0; i < 9; i++)
            {
                arr[i] = "0";
            }
            ListViewItem itm = new ListViewItem(arr)
            {
                UseItemStyleForSubItems = false
            };

            for (int k = 0; k < 9; k++)
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

            for (int k = 0; k < 9; k++)
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

            for (int k = 0; k < 9; k++)
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
            for (int k = 0; k < 9; k++)
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


            for (int k = 0; k < 9; k++)
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

            for (int k = 0; k < 9; k++)
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
            e_text.Text = Math.Round(suma[5, 0], 2).ToString() + " kcal";
            b_text.Text = Math.Round(suma[5, 1], 2).ToString() + " g";
            t_text.Text = Math.Round(suma[5, 2], 2).ToString() + " g";
            k_text.Text = Math.Round(suma[5, 3], 2).ToString() + " g";
            w_text.Text = Math.Round(suma[5, 4], 2).ToString() + " g";
            wp_text.Text = Math.Round(suma[5, 5], 2).ToString() + " g";
            c_text.Text = Math.Round(suma[5, 6], 2).ToString() + " g";
            bp_text.Text = Math.Round(suma[5, 7], 2).ToString() + " g";
            s_text.Text = Math.Round(suma[5, 8], 2).ToString() + " mg";
            sol_text.Text = (Math.Round(suma[5, 8] * 0.0025, 2)).ToString() + " g";


            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (suma[i, 0] != 0)
                    {
                        double wartosc_odzywcza = suma[i, k];
                        double przelicznik = 0;
                        if (k == 1)
                            przelicznik = przelicznik_Bialko;
                        if (k == 2)
                            przelicznik = przelicznik_Tluszcze;
                        if (k == 4)
                            przelicznik = przelicznik_Weglowodany;
                        if (k == 7)
                            wartosc_odzywcza /= 1000;

                        procent[i, k] = (wartosc_odzywcza * przelicznik * 100.0) / suma[i, 0];
                    }
                }
            }

            //PROCENTY
            //pb_Energia.SuperscriptText = Math.Round(procent[5, 0], 2).ToString() + " % kalorii";
            b_subtext.Text = Math.Round(procent[5, 1], 2).ToString();
            t_subtext.Text = Math.Round(procent[5, 2], 2).ToString();
            w_subtext.Text = Math.Round(procent[5, 4], 2).ToString();
            //pb_Sod.SuperscriptText = Math.Round(procent[5, 4], 2).ToString() + " % kalorii";
            //pb_TluszczeNN.SuperscriptText = Math.Round(procent[5, 5], 2).ToString() + " % kalorii";

            //ZAWARTOSC
            try
            {
                if (cb_dieta.SelectedIndex != -1)
                {
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energia != 0)
                    {
                        if (Convert.ToInt32(suma[5, 0] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energia) > 100)
                        {
                            pb_Energia.Value = 100;
                            plus_energia.Text = "+ " + Math.Round(suma[5, 0] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energia, 2) + " kalorii";
                        }
                        else
                        {
                            plus_energia.Text = "";
                            pb_Energia.Value = Convert.ToInt32(suma[5, 0] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energia);
                        }
                    }
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialko != 0)
                    {
                        if (Convert.ToInt32(suma[5, 1] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialko) > 100)
                        {
                            pb_Bialko.Value = 100;
                            plus_bialko.Text = "+ " + Math.Round(suma[5, 1] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialko, 2) + " g";
                        }
                        else
                        {
                            plus_bialko.Text = "";
                            pb_Bialko.Value = Convert.ToInt32(suma[5, 1] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialko);
                        }
                    }
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze != 0)
                    {
                        if (Convert.ToInt32(suma[5, 2] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze) > 100)
                        {
                            pb_Tluszcze.Value = 100;
                            plus_tluszcze.Text = "+ " + Math.Round(suma[5, 2] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze, 2) + " g";
                        }
                        else
                        {
                            plus_tluszcze.Text = "";
                            pb_Tluszcze.Value = Convert.ToInt32(suma[5, 2] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze);
                        }
                    }
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany != 0)
                    {
                        if (Convert.ToInt32(suma[5, 4] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany) > 100)
                        {
                            pb_Weglowodany.Value = 100;
                            plus_wegle.Text = "+ " + Math.Round(suma[5, 4] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany, 2) + " g";
                        }
                        else
                        {
                            plus_wegle.Text = "";
                            pb_Weglowodany.Value = Convert.ToInt32(suma[5, 4] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany);
                        }
                    }
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne != 0)
                    {
                        if (Convert.ToInt32(suma[5, 5] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne) > 100)
                        {
                            pb_przyswajalne.Value = 100;
                            plus_przyswajalne.Text = "+ " + Math.Round(suma[5, 5] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne, 2) + " g";
                        }
                        else
                        {
                            plus_przyswajalne.Text = "";
                            pb_przyswajalne.Value = Convert.ToInt32(suma[5, 5] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne);
                        }
                    }
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry != 0)
                    {
                        if (Convert.ToInt32(suma[5, 6] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry) > 100)
                        {
                            pb_cukry.Value = 100;
                            plus_wegle.Text = "+ " + Math.Round(suma[5, 6] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry, 2) + " g";
                        }
                        else
                        {
                            plus_wegle.Text = "";
                            pb_cukry.Value = Convert.ToInt32(suma[5, 6] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry);
                        }
                    }
                    else
                        pb_cukry.Value = 0;
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik != 0)
                    {
                        if (Convert.ToInt32(suma[5, 7] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik) > 100)
                        {
                            pb_blonnik.Value = 100;
                            plus_blonnik.Text = "+ " + Math.Round(suma[5, 7] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik, 2) + " g";
                        }
                        else
                        {
                            plus_blonnik.Text = "";
                            pb_blonnik.Value = Convert.ToInt32(suma[5, 7] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik);
                        }
                    }
                    else
                        pb_blonnik.Value = 0;
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod != 0)
                    {
                        if (Convert.ToInt32(suma[5, 8] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod) > 100)
                        {
                            pb_Sod2.Value = 100;
                            plus_sod.Text = "+ " + Math.Round(suma[5, 8] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod, 2) + " mg";
                        }
                        else
                        {
                            plus_sod.Text = "";
                            pb_Sod2.Value = Convert.ToInt32(suma[5, 8] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod);
                        }
                    }
                    else
                        pb_Sod2.Value = 0;
                    if (Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn != 0)
                    {
                        if (Convert.ToInt32(suma[5, 3] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn) > 100)
                        {
                            pb_ktn.Value = 100;
                            plus_kwasy.Text = "+ " + Math.Round(suma[5, 3] - Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn, 2) + " g";
                        }
                        else
                        {
                            plus_kwasy.Text = "";
                            pb_ktn.Value = Convert.ToInt32(suma[5, 3] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn);
                        }
                    }
                    else
                        pb_ktn.Value = 0;
                }
            }
            catch
            {
                MessageBox.Show("Nie można przeliczyć wartości, o które przekroczono limity diety", "Błąd");
            }
        }

        private void Kategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int wybor = cb_kategorie.SelectedIndex;

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
                MessageBox.Show($"Nie można edytować.{ex.Message}", "Błąd");
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
            catch
            {
                MessageBox.Show("Nie można przesunąć", "Błąd");
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
            catch
            {
                MessageBox.Show("Nie można przesunąć", "Błąd");
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
            MessageBox.Show("Wyczyszczono recepturę: " + tc_posilki.SelectedTab.Text, "Wyczyszczono");
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
            MessageBox.Show("Wyczyszczono dzień: " + dateTimePicker1.Text + " (" + cb_dieta.SelectedItem + ")", "Wczyszczono");
        }

        private void dzieńToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zapisać dzień: " + '\n' + dateTimePicker1.Text + '\n' + cb_dieta.Text + '\n' + cb_miasto.Text, "Zapisz dzień", MessageBoxButtons.YesNo);
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

                MessageBox.Show("Zapisano dzień:" + '\n' + dateTimePicker1.Text + '\n' + cb_dieta.Text + '\n' + cb_miasto.Text, "Zapisano");

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
            LiczSredniaJadlospisu();
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
            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zapisać: " + '\n' + tc_posilki.SelectedTab.ToString().Remove(tc_posilki.SelectedTab.ToString().Length - 1).Remove(0, 10) + '\n' + tekst, "Zapisz recepturę", MessageBoxButtons.YesNo);
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
                        MessageBox.Show("Zapisano recepturę: " + textBox1.Text, "Zapisano");
                        break;
                    case 1:
                        drProdukty["Nazwa receptury"] = textBox2.Text;
                        for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox2.Text, "Zapisano");
                        break;
                    case 2:
                        drProdukty["Nazwa receptury"] = textBox3.Text;
                        for (int i = 0; i < lv_obiad.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox3.Text, "Zapisano");
                        break;
                    case 3:
                        drProdukty["Nazwa receptury"] = textBox4.Text;
                        for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox4.Text, "Zapisano");
                        break;
                    case 4:
                        drProdukty["Nazwa receptury"] = textBox5.Text;
                        for (int i = 0; i < lv_kolacja.Items.Count; i++)
                            drProdukty["Skład receptury"] += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox5.Text, "Zapisano");
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
            panel14.BackColor = highlightColor;
            panel3.BackColor = primaryColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;

            panel_kontrola.Visible = true;
            panel_kontrola.BringToFront();

            label10.Text = "Kontrola";
            if (k_miasto.Items.Count == 0)
            {
                listaJednostek = DAO.JednostkaDAO.SelectAll();
                foreach (DAO.Jednostka r in listaJednostek)
                    k_miasto.Items.Add(r.miasto);
            }
            k_miasto.SelectedIndex = 0;

            if (k_dieta.Items.Count == 0)
            {
                Diety = DAO.DietaDAO.SelectAll(k_miasto.SelectedItem.ToString());
                foreach (Dieta d in Diety)
                    k_dieta.Items.Add(d.nazwa);
            }
            k_dieta.SelectedIndex = 0;
        }

        private void produktClick()
        {
            panel3.BackColor = highlightColor;
            pictureBox6.BackColor = highlightColor;
            label11.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;

            panel_produkty.Visible = true;
            panel_produkty.BringToFront();

            produkt_wstecz_Click(null, null);
        }

        private void recepturaClick()
        {
            panel5.BackColor = highlightColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;

            panel_receptura.Visible = true;
            panel_receptura.BringToFront();

            pictureBox14_Click(null, null);

        }

        private void jadlospisClick()
        {
            panel6.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;
            label10.Text = "Jadłospisy";

            panel_jadlospis.Visible = true;
            panel_jadlospis.BringToFront();

            pictureBox23.Visible = false;
            pictureBox24.Visible = false;
            pictureBox25.Visible = true;

            wczytajJadlospis();

            if (jadlospis_miasto.Items.Count > 0)
                jadlospis_miasto.SelectedIndex = 0;

        }

        private void dekadowkaClick()
        {
            panel7.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;
            label10.Text = "Szablony";
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = true;
            panel_dekadowka.BringToFront();

            dekadowka_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAll();
            foreach (Jednostka j in listaJednostek)
                dekadowka_miasto.Items.Add(j.miasto);

            if (dekadowka_miasto.Items.Count > 0)
                dekadowka_miasto.SelectedIndex = 0;
            dekadowka_dekadowka.Items.Clear();
            listaDekadowek = DekadowkaDAO.Select(dekadowka_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowek)
                dekadowka_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_dekadowka.Items.Count > 0)
                dekadowka_dekadowka.SelectedIndex = 0;

            dekadowka_nope_Click(null, null);
        }
        private void dietaClick()
        {
            panel8.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;
            panel_dieta.Visible = true;
            panel_dieta.BringToFront();

            dieta_wstecz_Click(null, null);

        }

        private void glownaClick()
        {
            panel10.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel11.BackColor = primaryColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;
            panel_glowny.Visible = true;
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = false;
            panel_kontrola.Visible = false;
            panel_dekadowka_zapisz.Visible = false;
            panel_dieta.Visible = false;
            panel_glowny.BringToFront();
            label10.Text = "Strona Główna";



            lb_produkty.Items.Clear();
            Lista = DAO.ProduktDAO.SelectAll();
            Lista = Lista.OrderBy(x => x.nazwa).Cast<Produkt>().ToList();

            foreach (Produkt p in Lista)
                lb_produkty.Items.Add(p.nazwa);

            cb_kategorie.SelectedIndex = 0;

            cb_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka j in listaJednostek)
                cb_miasto.Items.Add(j.miasto);

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
            panel9.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            label10.Text = "Jednostki";
            panel11.BackColor = primaryColor;
            pictureBox6.BackColor = primaryColor;
            label11.BackColor = primaryColor;
            panel14.BackColor = primaryColor;

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
            catch
            {
                MessageBox.Show("Błąd przeliczania", "Błąd");
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
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.ProduktDAO.Delete(Lista[produkt_wczytaj.SelectedIndex]);
                    MessageBox.Show("Usunięto: " + Lista[produkt_wczytaj.SelectedIndex].nazwa);
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
            produkt_wczytaj.Items.Clear();
            produkt_kategoria.Items.Clear();

            Lista = Lista.OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
            for (int i = 0; i < Lista.Count; i++)
            {
                produkt_wczytaj.Items.Add(Lista[i].nazwa);
            }

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
            produkt_wczytaj.SelectedIndex = 0;
            produkt_wczytaj_SelectedIndexChanged(sender, e);

            label10.Text = "Produkty -> Przeglądaj";
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
                            MessageBox.Show("Dodano: " + produkt_nazwa.Text);
                            produktClick();
                        }
                        else
                        {
                            MessageBox.Show("Nie uzupełniono wszystkich danych", "Błąd");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Błąd dodawania produktu", "Błąd");
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
                            MessageBox.Show("Edytowano: " + produkt_nazwa.Text);
                            produktClick();
                        }
                        else
                        {
                            MessageBox.Show("Nie uzupełniono wszystkich danych", "Błąd");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Błąd dodawania produktu", "Błąd");
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
                cb_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAll(cb_miasto.SelectedItem.ToString());
                foreach (Dieta d in Diety)
                    cb_dieta.Items.Add(d.nazwa);
                try { cb_dieta.SelectedIndex = wybranaDieta; } catch { if (cb_dieta.Items.Count > 0) cb_dieta.SelectedIndex = 0; }
            }
        }

        private void cb_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybranaDieta = cb_dieta.SelectedIndex;
            LiczSrednia();
        }
        #endregion

        #region Dekadowka

        public void GenerateCards()
        {
            dekadowka_panel.Controls.Clear();

            Dekadowka[] jadlospisyDanejDekadowki = DAO.JadlospisDekadowkiDAO.SelectForAllDays(wybranaDekadowka);

            for (int j = 0; j < wybranaDekadowka.dni; j++)
            {
                FlowLayoutPanel dayOfWeek = new FlowLayoutPanel
                {
                    BackColor = Color.White,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false, // Vertical rather than horizontal scrolling
                    Size = new Size(dzienSize[0], dzienSize[1])
                };
                dayOfWeek.VerticalScroll.Visible = false;
                dayOfWeek.HorizontalScroll.Visible = false;

                string day = GetDay(wybranaDekadowka.dzienStart, j + 1);
                Label myDay = new Label
                {
                    Text = day,
                    MaximumSize = new Size(dzienSize[0], 0),
                    AutoSize = true
                };
                dayOfWeek.Controls.Add(myDay);

                foreach (Jadlospis jadlospis in jadlospisyDanejDekadowki[j].listaJadlospisow)
                {
                    FlowLayoutPanel myPanel = new FlowLayoutPanel();
                    myPanel.BackColor = Color.LightBlue;
                    myPanel.AutoScroll = true;
                    myPanel.VerticalScroll.Visible = false;
                    myPanel.HorizontalScroll.Enabled = false;
                    myPanel.FlowDirection = FlowDirection.TopDown;
                    myPanel.WrapContents = false;
                    myPanel.AutoSize = true;
                    //myPanel.Size = new System.Drawing.Size(dietaSize[0], dietaSize[1]);

                    Panel divider = new Panel();
                    divider.BackColor = Color.Gray;
                    divider.Size = new System.Drawing.Size(dietaSize[0] - 25, 5);
                    myPanel.Controls.Add(divider);

                    Label diet = new Label();
                    diet.Text = jadlospis.dieta.nazwa;
                    diet.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    diet.Font = new System.Drawing.Font("Sagoe UI", 12);
                    diet.Margin = new Padding(0, 0, 0, 10);
                    diet.AutoSize = true;
                    myPanel.Controls.Add(diet);

                    Label meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    Label meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    meal_content.Margin = new Padding(10, 0, 0, 5);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Śniadanie:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_sniadanie != "")
                        meal_content.Text = jadlospis.nazwa_sniadanie;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "II śniadanie:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_IIsniadanie != "")
                        meal_content.Text = jadlospis.nazwa_IIsniadanie;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Obiad:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_obiad != "")
                        meal_content.Text = jadlospis.nazwa_obiad;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Podwieczorek:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_podwieczorek != "")
                        meal_content.Text = jadlospis.nazwa_podwieczorek;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Kolacja:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_kolacja != "")
                        meal_content.Text = jadlospis.nazwa_kolacja;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    dayOfWeek.Controls.Add(myPanel);
                }

                dekadowka_panel.Controls.Add(dayOfWeek);
            }
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
            dekadowka_dekadowka.Items.Clear();
            listaDekadowek = DekadowkaDAO.Select(dekadowka_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowek)
                dekadowka_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_dekadowka.Items.Count > 0)
                dekadowka_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowka != null)
            {
                if (wybranaDekadowka.nazwa != listaDekadowek[dekadowka_dekadowka.SelectedIndex].nazwa || wybranaDekadowka.miasto != listaDekadowek[dekadowka_dekadowka.SelectedIndex].miasto)
                {
                    wybranaDekadowka = listaDekadowek[dekadowka_dekadowka.SelectedIndex];
                    GenerateCards();
                }
            }
            else
            {
                wybranaDekadowka = listaDekadowek[dekadowka_dekadowka.SelectedIndex];
                GenerateCards();
            }
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

            dekadowka_dodaj_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka j in listaJednostek)
                dekadowka_dodaj_miasto.Items.Add(j.miasto);

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

            dekadowka_dodaj_dni.Visible = false;
            dekadowka_dodaj_label_dzienStart.Visible = false;
            dekadowka_dodaj_label_dekadowka.Visible = false;
            dekadowka_dodaj_label_dni.Visible = false;
            dekadowka_dodaj_label_miasto.Visible = false;
            dekadowka_dodaj_miasto.Visible = false;
            dekadowka_dodaj_nazwa.Visible = false;
            dekadowka_dodaj_dzienStart.Visible = false;
        }


        List<Dekadowka> listaDekadowekDoZapisania;
        Dekadowka wybranaDekadowkaDoZapisania;
        private void zapiszJadłospisDekadówkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablon -> Dodaj jadłospis";
            panel_dekadowka_zapisz.Visible = true;
            panel_dekadowka_zapisz.BringToFront();
            dekadowka_zapisz_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka j in listaJednostek)
                dekadowka_zapisz_miasto.Items.Add(j.miasto);
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

            MessageBox.Show("Zapisano jadłospis szablonu", "Zapisz");
            dekadowka_zapisz_wstec_Click(null, null);
        }

        private void dekadowka_zapisz_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {

            dekadowka_zapisz_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAll(dekadowka_zapisz_miasto.Text);
            foreach (Dieta d in Diety)
                dekadowka_zapisz_dieta.Items.Add(d.nazwa);
            if (dekadowka_zapisz_dieta.Items.Count > 0)
                dekadowka_zapisz_dieta.SelectedIndex = 0;

            dekadowka_zapisz_dekadowka.Items.Clear();
            listaDekadowekDoZapisania = DekadowkaDAO.Select(dekadowka_zapisz_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoZapisania)
                dekadowka_zapisz_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_zapisz_dekadowka.Items.Count > 0)
                dekadowka_zapisz_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_zapisz_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybranaDekadowkaDoZapisania = listaDekadowekDoZapisania[dekadowka_zapisz_dekadowka.SelectedIndex];
            dekadowka_zapisz_dzien.Items.Clear();
            for (int j = 0; j < wybranaDekadowkaDoZapisania.dni; j++)
            {
                dekadowka_zapisz_dzien.Items.Add(GetDay(wybranaDekadowkaDoZapisania.dzienStart, j + 1));
            }
            if (dekadowka_zapisz_dzien.Items.Count > 0)
                dekadowka_zapisz_dzien.SelectedIndex = 0;
        }


        #endregion

        #region Dieta
        private void dieta_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            dieta_nazwa.Text = Diety[dieta_dieta.SelectedIndex].nazwa;
            dieta_energia.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.energia.ToString();
            dieta_tluszcze.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.tluszcze.ToString();
            dieta_bialko.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.bialko.ToString();
            dieta_weglowodany.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.weglowodany.ToString();
            dieta_blonnik.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.blonnik.ToString();
            dieta_przyswajalne.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne.ToString();
            dieta_cukry.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.cukry.ToString();
            dieta_ktn.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn.ToString();
            dieta_sod.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.sod.ToString();
        }

        private void dieta_ok_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Diety -> Dodaj":
                    if (dieta_nazwa.Text != "" && dieta_miasto.Text != "" && dieta_energia.Text != "" && dieta_bialko.Text != "" && dieta_tluszcze.Text != "" && dieta_weglowodany.Text != "" && dieta_sod.Text != "" && dieta_ktn.Text != "" && dieta_przyswajalne.Text != "" && dieta_blonnik.Text != "" && dieta_cukry.Text != "")
                    {
                        try
                        {
                            DAO.DietaDAO.Insert(dieta_nazwa.Text, dieta_miasto.Text, Convert.ToDouble(dieta_energia.Text), Convert.ToDouble(dieta_bialko.Text), Convert.ToDouble(dieta_tluszcze.Text), Convert.ToDouble(dieta_weglowodany.Text), Convert.ToDouble(dieta_sod.Text), Convert.ToDouble(dieta_ktn.Text), Convert.ToDouble(dieta_przyswajalne.Text), Convert.ToDouble(dieta_blonnik.Text), Convert.ToDouble(dieta_cukry.Text));
                            MessageBox.Show("Dodano: " + dieta_nazwa.Text);
                            dietaClick();
                        }
                        catch
                        {
                            MessageBox.Show("Błąd dodawania diety", "Błąd");

                        }
                    }
                    else
                        MessageBox.Show("Nie uzupełniono wszystkich danych", "Błąd");
                    break;
                case "Diety -> Edytuj":
                    if (dieta_nazwa.Text != "" && dieta_miasto.Text != "" && dieta_energia.Text != "" && dieta_bialko.Text != "" && dieta_tluszcze.Text != "" && dieta_weglowodany.Text != "" && dieta_sod.Text != "" && dieta_ktn.Text != "" && dieta_przyswajalne.Text != "" && dieta_blonnik.Text != "" && dieta_cukry.Text != "")
                    {
                        try
                        {
                            DAO.DietaDAO.Update(Diety[dieta_dieta.SelectedIndex], dieta_nazwa.Text, dieta_miasto.Text, Convert.ToDouble(dieta_energia.Text), Convert.ToDouble(dieta_bialko.Text), Convert.ToDouble(dieta_tluszcze.Text), Convert.ToDouble(dieta_weglowodany.Text), Convert.ToDouble(dieta_sod.Text), Convert.ToDouble(dieta_ktn.Text), Convert.ToDouble(dieta_przyswajalne.Text), Convert.ToDouble(dieta_blonnik.Text), Convert.ToDouble(dieta_cukry.Text));
                            MessageBox.Show("Edytowano: " + dieta_nazwa.Text);
                            dietaClick();
                        }
                        catch
                        {
                            MessageBox.Show("Błąd edytowania diety", "Błąd");

                        }
                    }
                    else
                        MessageBox.Show("Nie uzupełniono wszystkich danych", "Błąd");
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
            dieta_energia.Enabled = false;
            dieta_energia.BackColor = Color.FromName("ControlLight");
            dieta_bialko.Enabled = false;
            dieta_bialko.BackColor = Color.FromName("ControlLight");
            dieta_tluszcze.Enabled = false;
            dieta_tluszcze.BackColor = Color.FromName("ControlLight");
            dieta_weglowodany.Enabled = false;
            dieta_weglowodany.BackColor = Color.FromName("ControlLight");
            dieta_cukry.Enabled = false;
            dieta_cukry.BackColor = Color.FromName("ControlLight");
            dieta_ktn.Enabled = false;
            dieta_ktn.BackColor = Color.FromName("ControlLight");
            dieta_sod.Enabled = false;
            dieta_sod.BackColor = Color.FromName("ControlLight");
            dieta_przyswajalne.Enabled = false;
            dieta_przyswajalne.BackColor = Color.FromName("ControlLight");
            dieta_blonnik.Enabled = false;
            dieta_blonnik.BackColor = Color.FromName("ControlLight");
            dieta_lbl_sol.Visible = false;
            dieta_sol.Visible = false;
            dieta_przelicz.Visible = false;

            label10.Text = "Diety";

            dieta_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka d in listaJednostek)
                dieta_miasto.Items.Add(d.miasto);
            if (dieta_miasto.Items.Count > 0) dieta_miasto.SelectedIndex = 0;


        }

        private void dieta_usun_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć tę dietę?", "Usuwanie diety", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.DietaDAO.Delete(Diety[dieta_dieta.SelectedIndex]);
                    MessageBox.Show("Usunięto: " + Diety[dieta_dieta.SelectedIndex].nazwa);
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
            dieta_energia.Enabled = true;
            dieta_energia.BackColor = Color.White;
            dieta_bialko.Enabled = true;
            dieta_bialko.BackColor = Color.White;
            dieta_tluszcze.Enabled = true;
            dieta_tluszcze.BackColor = Color.White;
            dieta_weglowodany.Enabled = true;
            dieta_weglowodany.BackColor = Color.White;
            dieta_cukry.Enabled = true;
            dieta_cukry.BackColor = Color.White;
            dieta_ktn.Enabled = true;
            dieta_ktn.BackColor = Color.White;
            dieta_sod.Enabled = true;
            dieta_sod.BackColor = Color.White;
            dieta_lbl_sol.Visible = true;
            dieta_sol.Visible = true;
            dieta_przelicz.Visible = true;
            dieta_przyswajalne.Enabled = true;
            dieta_przyswajalne.BackColor = Color.White;
            dieta_blonnik.Enabled = true;
            dieta_blonnik.BackColor = Color.White;
            dieta_sol.Text = "";
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
            dieta_energia.Enabled = true;
            dieta_energia.BackColor = Color.White;
            dieta_bialko.Enabled = true;
            dieta_bialko.BackColor = Color.White;
            dieta_tluszcze.Enabled = true;
            dieta_tluszcze.BackColor = Color.White;
            dieta_weglowodany.Enabled = true;
            dieta_weglowodany.BackColor = Color.White;
            dieta_cukry.Enabled = true;
            dieta_cukry.BackColor = Color.White;
            dieta_ktn.Enabled = true;
            dieta_ktn.BackColor = Color.White;
            dieta_sod.Enabled = true;
            dieta_sod.BackColor = Color.White;
            dieta_lbl_sol.Visible = true;
            dieta_sol.Visible = true;
            dieta_przelicz.Visible = true;
            dieta_sol.Text = "";
            dieta_przyswajalne.Enabled = true;
            dieta_przyswajalne.BackColor = Color.White;
            dieta_blonnik.Enabled = true;
            dieta_blonnik.BackColor = Color.White;

            dieta_nazwa.Text = "";
            dieta_energia.Text = "";
            dieta_bialko.Text = "";
            dieta_weglowodany.Text = "";
            dieta_tluszcze.Text = "";
            dieta_sod.Text = "";
            dieta_ktn.Text = "";
            dieta_sol.Text = "";
            dieta_przyswajalne.Text = "";
            dieta_blonnik.Text = "";
        }

        private void dieta_przelicz_Click(object sender, EventArgs e)
        {
            try
            {
                if (dieta_sol.Text != "")
                    dieta_sod.Text = (Double.Parse(dieta_sol.Text) / 0.0025).ToString();
            }
            catch
            {
                MessageBox.Show("Błąd przeliczania", "Błąd");
            }
        }
        #endregion

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

            jednostka_jednostka.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka j in listaJednostek)
                jednostka_jednostka.Items.Add(j.miasto);

            if (jednostka_jednostka.Items.Count > 0)
                jednostka_jednostka.SelectedIndex = 0;


        }
        List<DAO.Jednostka> listaJednostek;
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
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć tę jednostkę?", "Usuwanie jednostki", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.JednostkaDAO.Delete(listaJednostek[jednostka_jednostka.SelectedIndex]);
                    MessageBox.Show("Usunięto: " + listaJednostek[jednostka_jednostka.SelectedIndex].miasto);
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
                        DAO.JednostkaDAO.Insert(jednostka_miasto.Text);
                        MessageBox.Show("Dodano: " + jednostka_miasto.Text);

                        jednostkaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych", "Błąd");
                    }
                    break;
                case "Jednostki -> Edytuj":
                    if (jednostka_miasto.Text != "")
                    {
                        DAO.JednostkaDAO.Update(listaJednostek[jednostka_jednostka.SelectedIndex], jednostka_miasto.Text);
                        MessageBox.Show("Edytowano: " + jednostka_miasto.Text);

                        jednostkaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych", "Błąd");
                    }
                    break;
            }

        }

        private void jednostka_jednostka_SelectedIndexChanged(object sender, EventArgs e)
        {
            jednostka_miasto.Text = listaJednostek[jednostka_jednostka.SelectedIndex].miasto;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dekadowka_usun_Click_1(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć tę szablon?", "Usuwanie szablonu", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DekadowkaDAO.Delete(listaDekadowek[dekadowka_dekadowka.SelectedIndex]);
                    MessageBox.Show("Usunięto szablon: " + listaDekadowek[dekadowka_dekadowka.SelectedIndex].nazwa + " z: " + listaDekadowek[dekadowka_dekadowka.SelectedIndex].miasto);
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
                            MessageBox.Show("Dodano szablon: " + dekadowka_dodaj_nazwa.Text + " w: " + dekadowka_dodaj_miasto.Text, "Dodawanie szablonu");
                            dekadowkaClick();
                        }
                        catch
                        {
                            MessageBox.Show("Błąd dodawania szablonu", "Błąd");

                        }

                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych", "Błąd");
                    }
                    break;
                case "Szablony -> Generuj jadłospisy":
                    List<string> daty = new List<string>();
                    int dni = (Convert.ToDateTime(dekadowka_generuj_data2.Text) - Convert.ToDateTime(dekadowka_generuj_data1.Text)).Days + 1;
                    if (dni == wybranaDekadowka.dni)
                    {
                        DateTime data = Convert.ToDateTime(dekadowka_generuj_data1.Text);
                        for (int i = 0; i < dni; i++)
                        {
                            string aktualna_data = data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year;
                            List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowka.id), i + 1);
                            foreach (Jadlospis jadlospis in jadlospisyDanegoDnia)
                            {
                                if (jadlospis.dzien == i + 1)
                                    DAO.JadlospisDAO.Insert(aktualna_data, jadlospis.dieta.nazwa, wybranaDekadowka.miasto, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja);
                            }
                            data = data.AddDays(1);
                        }
                        MessageBox.Show("Dodano jadłospisy według szablonu", "Generowanie jadłospisów");
                    }
                    else
                    {
                        MessageBox.Show("Wpisano inną ilość dni niż wybranego szablonu");
                    }
                    break;
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                default:
                    label10.Text = "Receptury";

                    receptura_wczytaj.Items.Clear();
                    listaReceptur = DAO.RecepturaDAO.SelectAll();
                    foreach (Receptura r in listaReceptur)
                        receptura_wczytaj.Items.Add(r.nazwa);

                    if (receptura_wczytaj.Items.Count > 0)
                        receptura_wczytaj.SelectedIndex = 0;

                    receptura_kategoria.Visible = false;
                    receptura_produkty.Visible = false;
                    receptura_produkt_dodaj.Visible = false;
                    receptura_masa.Visible = false;
                    receptura_masa_label.Visible = false;
                    receptura_dodaj_edytuj.Visible = false;
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

            LiczSredniaDlaReceptur();
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Na pewno chcesz usunąć tę recepturę?", "Usuwanie receptury", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    DAO.RecepturaDAO.Delete(listaReceptur[receptura_wczytaj.SelectedIndex]);
                    MessageBox.Show("Usunięto: " + listaReceptur[receptura_wczytaj.SelectedIndex].nazwa);
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
            receptura_produkt_dodaj.Visible = true;
            receptura_masa.Visible = true;
            receptura_masa_label.Visible = true;
            receptura_dodaj_edytuj.Visible = true;
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
            receptura_produkt_dodaj.Visible = true;
            receptura_masa.Visible = true;
            receptura_masa_label.Visible = true;
            receptura_dodaj_edytuj.Visible = true;
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
                    catch
                    {
                        MessageBox.Show("Nieprawidłowa wartość", "Błąd");
                    }
                    LiczSredniaDlaReceptur();
                }
                else
                {
                    MessageBox.Show("Nie wpisano masy produktu", "Błąd");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu", "Błąd");
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

            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć " + produkt + "?", "Usuwanie produktu z receptury", MessageBoxButtons.YesNo);
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
                MessageBox.Show($"Nie można edytować. {ex.Message}", "Błąd");
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
            catch
            {
                MessageBox.Show("Nie można przesunąć", "Błąd");
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
            catch
            {
                MessageBox.Show("Nie można przesunąć", "Błąd");
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
                        MessageBox.Show("Dodano: " + receptura_nazwa.Text);
                        recepturaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych", "Błąda");
                    }
                    break;
                case "Receptury -> Edytuj":
                    if (sklad != "" && receptura_nazwa.Text != "")
                    {
                        DAO.RecepturaDAO.Update(listaReceptur[receptura_wczytaj.SelectedIndex], receptura_nazwa.Text, sklad);
                        MessageBox.Show("Edytowano: " + receptura_nazwa.Text);
                        recepturaClick();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych", "Błąda");
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

        private void pictureBox13_Click_1(object sender, EventArgs e)
        {
            glownaClick();
        }

        private void wczytajJadłospisDekadówkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label10.Text = "Szablony -> Wczytaj";
            panel_dekadowka_wczytaj.Visible = true;
            panel_dekadowka_wczytaj.BringToFront();

            dekadowka_wczytaj_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka j in listaJednostek)
                dekadowka_wczytaj_miasto.Items.Add(j.miasto);
            dekadowka_wczytaj_miasto.SelectedIndex = 0;

            dekadowka_wczytaj_dekadowka.Items.Clear();
            listaDekadowekDoWczytania = DekadowkaDAO.Select(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;


        }

        private void dekadowka_wczytaj_dekadowka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                if (wybranaDekadowkaDoWczytania.nazwa != listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex].nazwa || wybranaDekadowkaDoWczytania.miasto != listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex].miasto)
                {
                    dekadowka_wczytaj_dzien.Items.Clear();
                    wybranaDekadowkaDoWczytania = listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex];
                    for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
                    {
                        dekadowka_wczytaj_dzien.Items.Add(GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1));
                    }
                    if (dekadowka_wczytaj_dzien.Items.Count > 0)
                        dekadowka_wczytaj_dzien.SelectedIndex = 0;

                    GenerateCardsDoWczytania();
                }
            }
            else
            {
                dekadowka_wczytaj_dzien.Items.Clear();
                wybranaDekadowkaDoWczytania = listaDekadowekDoWczytania[dekadowka_wczytaj_dekadowka.SelectedIndex];
                for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
                {
                    dekadowka_wczytaj_dzien.Items.Add(GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1));
                }
                if (dekadowka_wczytaj_dzien.Items.Count > 0)
                    dekadowka_wczytaj_dzien.SelectedIndex = 0;
                GenerateCardsDoWczytania();
            }
        }

        private void dekadowka_wczytaj_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dekadowka_wczytaj_dekadowka.Items.Clear();
            listaDekadowekDoWczytania = DekadowkaDAO.Select(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;
        }

        private void dekadowka_wczytaj_dzien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                dekadowka_wczytaj_dieta.Items.Clear();
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), dekadowka_wczytaj_dzien.SelectedIndex + 1);
                foreach (Jadlospis d in jadlospisyDanegoDnia)
                {
                    if (d.dzien - 1 == dekadowka_wczytaj_dzien.SelectedIndex)
                    {
                        dekadowka_wczytaj_dieta.Items.Add(d.dieta.nazwa);
                        //    jadlospisDekadowkiDoWczytania = d;
                    }
                }

                if (dekadowka_wczytaj_dieta.Items.Count > 0)
                    dekadowka_wczytaj_dieta.SelectedIndex = 0;
            }
        }

        public void GenerateCardsDoWczytania()
        {
            flowLayoutPanel1.Controls.Clear();

            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.VerticalScroll.Visible = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.WrapContents = false; // Vertical rather than horizontal scrolling
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Size = new System.Drawing.Size(dekadowkaSize[0], dekadowkaSize[1] - 75);

            for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
            {
                FlowLayoutPanel dayOfWeek = new FlowLayoutPanel();
                dayOfWeek.BackColor = Color.White;
                dayOfWeek.AutoScroll = true;
                dayOfWeek.FlowDirection = FlowDirection.TopDown;
                dayOfWeek.VerticalScroll.Visible = false;
                dayOfWeek.HorizontalScroll.Visible = false;
                dayOfWeek.WrapContents = false; // Vertical rather than horizontal scrolling
                dayOfWeek.Size = new System.Drawing.Size(dzienSize[0], dzienSize[1] - 60);

                Label myDay = new Label();
                string day = GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1);
                myDay.Text = day;
                myDay.MaximumSize = new Size(dzienSize[0], 0);
                myDay.AutoSize = true;
                dayOfWeek.Controls.Add(myDay);

                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), j + 1);
                foreach (Jadlospis jadlospis in jadlospisyDanegoDnia)
                {
                    FlowLayoutPanel myPanel = new FlowLayoutPanel();
                    myPanel.BackColor = Color.LightBlue;
                    myPanel.AutoScroll = true;
                    myPanel.VerticalScroll.Visible = false;
                    myPanel.HorizontalScroll.Enabled = false;
                    myPanel.FlowDirection = FlowDirection.TopDown;
                    myPanel.WrapContents = false;
                    myPanel.AutoSize = true;
                    //myPanel.Size = new System.Drawing.Size(dietaSize[0], dietaSize[1]);

                    Panel divider = new Panel();
                    divider.BackColor = Color.Gray;
                    divider.Size = new System.Drawing.Size(dietaSize[0] - 25, 5);
                    myPanel.Controls.Add(divider);

                    Label diet = new Label();
                    diet.Text = jadlospis.dieta.nazwa;
                    diet.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    diet.Font = new System.Drawing.Font("Sagoe UI", 12);
                    diet.Margin = new Padding(0, 0, 0, 10);
                    diet.AutoSize = true;
                    myPanel.Controls.Add(diet);

                    Label meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    Label meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    meal_content.Margin = new Padding(10, 0, 0, 5);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Śniadanie:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_sniadanie != "")
                        meal_content.Text = jadlospis.nazwa_sniadanie;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "II śniadanie:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_IIsniadanie != "")
                        meal_content.Text = jadlospis.nazwa_IIsniadanie;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Obiad:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_obiad != "")
                        meal_content.Text = jadlospis.nazwa_obiad;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Podwieczorek:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_podwieczorek != "")
                        meal_content.Text = jadlospis.nazwa_podwieczorek;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    meal = new Label();
                    meal.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal.AutoSize = true;
                    meal.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal.Text = "Kolacja:";
                    myPanel.Controls.Add(meal);

                    meal_content = new Label();
                    meal_content.MaximumSize = new Size(dietaSize[0] - 25, 0);
                    meal_content.Font = new System.Drawing.Font("Sagoe UI", 10);
                    meal_content.ForeColor = Color.Gray;
                    meal_content.AutoSize = true;
                    if (jadlospis.nazwa_kolacja != "")
                        meal_content.Text = jadlospis.nazwa_kolacja;
                    else
                        meal_content.Text = "-";
                    meal_content.Margin = new Padding(10, 0, 0, 5);
                    myPanel.Controls.Add(meal_content);

                    dayOfWeek.Controls.Add(myPanel);
                }

                flowLayoutPanel1.Controls.Add(dayOfWeek);
            }
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
                string[] arg = new string[10];
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
                    if (arr.Length != 10)
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
                        arg[9] = arr[9];
                        arg[10] = arr[10];
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

        public void wczytajJadlospis()
        {

            jadlospis_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAll();
            foreach (DAO.Jednostka d in listaJednostek)
                jadlospis_miasto.Items.Add(d.miasto);

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
                LiczSredniaJadlospisu();
            }
        }

        public void LiczSredniaJadlospisu()
        {
            double[,] suma_jad = new double[6, 9];
            double[,] proc_jad = new double[6, 9];

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma_jad[i, k] = 0;
                    proc_jad[i, k] = 0;
                }

            }

            string[] arr = new string[9];
            for (int i = 0; i < 9; i++)
                arr[i] = "0";
            ListViewItem itm = new ListViewItem(arr);
            itm.UseItemStyleForSubItems = false;

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    double a = double.Parse(listView1.Items[i].SubItems[k + 2].Text);
                    suma_jad[0, k] += a;
                }
            }

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    double a = double.Parse(listView2.Items[i].SubItems[k + 2].Text);
                    suma_jad[1, k] += a;
                }
            }

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    double a = double.Parse(listView3.Items[i].SubItems[k + 2].Text);
                    suma_jad[2, k] += a;
                }
            }
            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    double a = double.Parse(listView4.Items[i].SubItems[k + 2].Text);
                    suma_jad[3, k] += a;
                }
            }


            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < listView5.Items.Count; i++)
                {
                    double a = double.Parse(listView5.Items[i].SubItems[k + 2].Text);
                    suma_jad[4, k] += a;
                }
            }

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    suma_jad[5, k] += suma_jad[i, k];
                }
            }


            //WARTOŚCI
            jadlospis_energia.Text = Math.Round(suma_jad[5, 0], 2).ToString() + " kcal";
            jadlospis_bialko.Text = Math.Round(suma_jad[5, 1], 2).ToString() + " g";
            jadlospis_tluszcze.Text = Math.Round(suma_jad[5, 2], 2).ToString() + " g";
            jadlospis_ktn.Text = Math.Round(suma_jad[5, 3], 2).ToString() + " g";
            jadlospis_weglowodany.Text = Math.Round(suma_jad[5, 4], 2).ToString() + " g";
            jadlospis_przyswajalne.Text = Math.Round(suma_jad[5, 5], 2).ToString() + " g";
            jadlospis_cukry.Text = Math.Round(suma_jad[5, 6], 2).ToString() + " g";
            jadlospis_blonnik.Text = Math.Round(suma_jad[5, 7], 2).ToString() + " g";
            jadlospis_sod.Text = Math.Round(suma_jad[5, 8], 2).ToString() + " mg";
            jadlospis_sol.Text = Math.Round(suma_jad[5, 8]*0.0025, 2).ToString() + " g";

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (suma_jad[i, 0] != 0)
                    {
                        double wartosc_odzywcza = suma_jad[i, k];
                        double przelicznik = 0;
                        if (k == 1)
                            przelicznik = przelicznik_Bialko;
                        if (k == 2)
                            przelicznik = przelicznik_Tluszcze;
                        if (k == 4)
                            przelicznik = przelicznik_Weglowodany;
                        if (k == 7)
                            wartosc_odzywcza /= 1000;

                        proc_jad[i, k] = (wartosc_odzywcza * przelicznik * 100.0) / suma_jad[i, 0];
                    }
                }
            }

            //proc_jadY
            //pb_Energia.SuperscriptText = Math.Round(proc_jad[5, 0], 2).ToString() + " % kalorii";
            jadlospis_bialko2.Text = Math.Round(proc_jad[5, 1], 2).ToString();
            jadlospis_tluszcze2.Text = Math.Round(proc_jad[5, 2], 2).ToString();
            jadlospis_weglowodany2.Text = Math.Round(proc_jad[5, 4], 2).ToString();
            //pb_Sod.SuperscriptText = Math.Round(proc_jad[5, 4], 2).ToString() + " % kalorii";
            //pb_TluszczeNN.SuperscriptText = Math.Round(proc_jad[5, 5], 2).ToString() + " % kalorii";

            //ZAWARTOSC
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.energia != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 0] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.energia) > 100)
                { jadlospis_cb_energia.Value = 100; }
                else
                    jadlospis_cb_energia.Value = Convert.ToInt32(suma_jad[5, 0] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.energia);
            }
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.bialko != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 1] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.bialko) > 100)
                { jadlospis_cb_bialko.Value = 100; }
                else
                    jadlospis_cb_bialko.Value = Convert.ToInt32(suma_jad[5, 1] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.bialko);
            }
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 2] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze) > 100)
                { jadlospis_cb_tluszcze.Value = 100; }
                else
                    jadlospis_cb_tluszcze.Value = Convert.ToInt32(suma_jad[5, 2] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze);
            }
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 3] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn) > 100)
                { jadlospis_cb_ktn.Value = 100; }
                else
                    jadlospis_cb_ktn.Value = Convert.ToInt32(suma_jad[5, 3] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn);
            }
            else
                jadlospis_cb_ktn.Value = 0;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 4] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany) > 100)
                { jadlospis_cb_weglowodany.Value = 100; }
                else
                    jadlospis_cb_weglowodany.Value = Convert.ToInt32(suma_jad[5, 4] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany);
            }
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 5] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne) > 100)
                { jadlospis_cb_przyswajalne.Value = 100; }
                else
                    jadlospis_cb_przyswajalne.Value = Convert.ToInt32(suma_jad[5, 5] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne);
            }
            else
                jadlospis_cb_przyswajalne.Value = 0;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.cukry != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 6] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.cukry) > 100)
                { jadlospis_cb_cukry.Value = 100; }
                else
                    jadlospis_cb_cukry.Value = Convert.ToInt32(suma_jad[5, 6] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.cukry);
            }
            else
                jadlospis_cb_cukry.Value = 0;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 7] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik) > 100)
                { jadlospis_cb_blonnik.Value = 100; }
                else
                    jadlospis_cb_blonnik.Value = Convert.ToInt32(suma_jad[5, 7] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik);
            }
            else
                jadlospis_cb_blonnik.Value = 0;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 8] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod) > 100)
                { jadlospis_cb_sod.Value = 100; }
                else
                    jadlospis_cb_sod.Value = Convert.ToInt32(suma_jad[5, 8] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod);
            }
            else
                jadlospis_cb_sod.Value = 0;

        }

        private void ja_ValueChanged(object sender, EventArgs e)
        {
            wpiszJadlospis();
        }

        private void jadlospis_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            jadlospis_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAll(jadlospis_miasto.Text);
            foreach (Dieta d in Diety)
                jadlospis_dieta.Items.Add(d.nazwa);

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
                MessageBox.Show("Usunięto wybrany jadłospis", "Usuwanie jadłospisu");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label10.Text != "Diety -> Dodaj")
            {
                dieta_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAll(dieta_miasto.SelectedItem.ToString());
                foreach (Dieta d in Diety)
                    dieta_dieta.Items.Add(d.nazwa);
                if (dieta_dieta.Items.Count > 0)
                    dieta_dieta.SelectedIndex = 0;
                else
                {
                    dieta_nazwa.Text = "";
                    dieta_energia.Text = "";
                    dieta_bialko.Text = "";
                    dieta_weglowodany.Text = "";
                    dieta_cukry.Text = "";
                    dieta_tluszcze.Text = "";
                    dieta_sod.Text = "";
                    dieta_ktn.Text = "";
                    dieta_sol.Text = "";
                    dieta_przyswajalne.Text = "";
                    dieta_blonnik.Text = "";
                }
            }
        }

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
        
        private void panel11_Click(object sender, EventArgs e)
        {
            drukujClick();
        }

        private void panel11_Paint_1(object sender, PaintEventArgs e)
        {

        }

        public void drukujClick()
        {
            label10.Text = "Drukowanie";
            panel11.BackColor = highlightColor;
            panel5.BackColor = primaryColor;
            panel6.BackColor = primaryColor;
            panel7.BackColor = primaryColor;
            panel8.BackColor = primaryColor;
            panel9.BackColor = primaryColor;
            panel10.BackColor = primaryColor;
            panel3.BackColor = primaryColor;
            panel14.BackColor = primaryColor;

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
            switch (drukuj_rodzaj.SelectedItem.ToString())
            {
                case "Szablon":
                    Printer.Dekadowka(drukuj_combo.SelectedItem.ToString(), drukuj_od.Text, drukuj_do.Text, DAO.JadlospisDAO.SelectAll(drukuj_od.Text, drukuj_do.Text));
                    MessageBox.Show("Wygenerowano szablon");
                    break;
                case "Jadłospis":
                    Printer.Jadlospis(DAO.JadlospisDAO.SelectAll(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString()));
                    MessageBox.Show("Wygenerowano jadłospis");
                    break;
                case "Jadłospisy w danym okresie":
                    DateTime dateFrom = Convert.ToDateTime(drukuj_od.Text);
                    DateTime dateTo = Convert.ToDateTime(drukuj_do.Text);
                    for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                    {
                        string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                        List<Jadlospis> jad = DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString());
                        foreach (Jadlospis j in jad)
                            Printer.Jadlospis(j);
                    }
                    MessageBox.Show("Wygenerowano jadłospisy w wybranym okresie");
                    break;
                case "Jadłospis dzienny":
                    DateTime dateFrom2 = Convert.ToDateTime(drukuj_od.Text);
                    DateTime dateTo2 = Convert.ToDateTime(drukuj_do.Text);
                    for (DateTime data = dateFrom2; data <= dateTo2; data = data.AddDays(1))
                    {
                        string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                        Printer.JadlospisDzienny(DAO.JadlospisDAO.Select(dt, drukuj_combo.SelectedItem.ToString()));
                    }
                    MessageBox.Show("Wygenerowano jadłospisy dzienne w wybranym okresie");
                    break;
                case "Receptura":
                    Printer.Receptura(listaReceptur[drukuj_combo.SelectedIndex]);
                    MessageBox.Show("Wygernerowano recepturę");
                    break;
                case "Produkt":
                    Printer.Produkt(Lista[drukuj_combo.SelectedIndex]);
                    MessageBox.Show("Wygernerowano produkt");
                    break;
            }
            //glownaClick();
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
                    drukuj_combo.Items.Clear();
                    listaJednostek = DAO.JednostkaDAO.SelectAll();
                    foreach (DAO.Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
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
                    drukuj_combo.Items.Clear();
                    listaJednostek = DAO.JednostkaDAO.SelectAll();
                    foreach (DAO.Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
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
                    drukuj_combo.Items.Clear();
                    listaJednostek = DAO.JednostkaDAO.SelectAll();
                    foreach (DAO.Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
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
                    drukuj_combo.Items.Clear();
                    listaJednostek = DAO.JednostkaDAO.SelectAll();
                    foreach (DAO.Jednostka r in listaJednostek)
                        drukuj_combo.Items.Add(r.miasto);
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
                    drukuj_combo.Items.Clear();
                    listaReceptur = DAO.RecepturaDAO.SelectAll();
                    foreach (Receptura r in listaReceptur)
                        drukuj_combo.Items.Add(r.nazwa);
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
                    drukuj_combo.Items.Clear();
                    foreach (Produkt r in Lista)
                        drukuj_combo.Items.Add(r.nazwa);
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
                    drukuj_dieta.Items.Clear();
                    Diety = DAO.DietaDAO.SelectAll(drukuj_combo.SelectedItem.ToString());
                    foreach (Dieta r in Diety)
                        drukuj_dieta.Items.Add(r.nazwa);
                    if (drukuj_dieta.Items.Count > 0)
                        drukuj_dieta.SelectedIndex = 0;
                    break;
                case "Drukowanie -> Jadłospisy w danym okresie":
                    break;
            }
        }

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
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDay(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), dekadowka_wczytaj_dzien.SelectedIndex + 1);
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
            }

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    suma[5, k] += suma[i, k];
                }
            }

            for (int k = 0; k < 9; k++)
            {
                suma[5, k] = suma[5, k] / jadlospisy.Count;
            }

            //WARTOŚCI
            label139.Text = Math.Round(suma[5, 0], 2).ToString() + " kcal";
            label137.Text = Math.Round(suma[5, 1], 2).ToString() + " g";
            label134.Text = Math.Round(suma[5, 2], 2).ToString() + " g";
            label124.Text = Math.Round(suma[5, 3], 2).ToString() + " g";
            label131.Text = Math.Round(suma[5, 4], 2).ToString() + " g";
            label81.Text = Math.Round(suma[5, 5], 2).ToString() + " g";
            kontrola_cukry.Text = Math.Round(suma[5, 6], 2).ToString() + " g";
            label113.Text = Math.Round(suma[5, 7], 2).ToString() + " g";
            label128.Text = Math.Round(suma[5, 8], 2).ToString() + " mg";
            kontrola_sol.Text = Math.Round(suma[5, 8]*0.0025, 2).ToString() + " g";

            for (int k = 0; k < 9; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (suma[i, 0] != 0)
                    {
                        double wartosc_odzywcza = suma[i, k];
                        double przelicznik = 0;
                        if (k == 1)
                            przelicznik = przelicznik_Bialko;
                        if (k == 2)
                            przelicznik = przelicznik_Tluszcze;
                        if (k == 4)
                            przelicznik = przelicznik_Weglowodany;
                        if (k == 7)
                            wartosc_odzywcza /= 1000;

                        procent[i, k] = (wartosc_odzywcza * przelicznik * 100.0) / suma[i, 0];
                    }
                }
            }

            //PROCENTY
            label136.Text = Math.Round(procent[5, 1], 2).ToString();
            label133.Text = Math.Round(procent[5, 2], 2).ToString();
            label130.Text = Math.Round(procent[5, 4], 2).ToString();

            //ZAWARTOSC
            try
            {
                if (k_dieta.SelectedIndex != -1)
                {
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.energia != 0)
                    {
                        if (Convert.ToInt32(suma[5, 0] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.energia) > 100)
                        {
                            circularProgressBar5.Value = 100;
                            label148.Text = "+ " + Math.Round(suma[5, 0] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.energia, 2) + " kalorii";
                        }
                        else
                        {
                            label148.Text = "";
                            circularProgressBar5.Value = Convert.ToInt32(suma[5, 0] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.energia);
                        }
                    }
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.bialko != 0)
                    {
                        if (Convert.ToInt32(suma[5, 1] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.bialko) > 100)
                        {
                            circularProgressBar16.Value = 100;
                            label147.Text = "+ " + Math.Round(suma[5, 1] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.bialko, 2) + " g";
                        }
                        else
                        {
                            label147.Text = "";
                            circularProgressBar16.Value = Convert.ToInt32(suma[5, 1] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.bialko);
                        }
                    }
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze != 0)
                    {
                        if (Convert.ToInt32(suma[5, 2] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze) > 100)
                        {
                            circularProgressBar15.Value = 100;
                            label146.Text = "+ " + Math.Round(suma[5, 2] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze, 2) + " g";
                        }
                        else
                        {
                            label146.Text = "";
                            circularProgressBar15.Value = Convert.ToInt32(suma[5, 2] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze);
                        }
                    }
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany != 0)
                    {
                        if (Convert.ToInt32(suma[5, 4] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany) > 100)
                        {
                            circularProgressBar6.Value = 100;
                            label145.Text = "+ " + Math.Round(suma[5, 4] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany, 2) + " g";
                        }
                        else
                        {
                            label145.Text = "";
                            circularProgressBar6.Value = Convert.ToInt32(suma[5, 4] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany);
                        }
                    }
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne != 0)
                    {
                        if (Convert.ToInt32(suma[5, 5] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne) > 100)
                        {
                            circularProgressBar1.Value = 100;
                            label141.Text = "+ " + Math.Round(suma[5, 5] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne, 2) + " g";
                        }
                        else
                        {
                            label141.Text = "";
                            circularProgressBar1.Value = Convert.ToInt32(suma[5, 5] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.weglowodany_przyswajalne);
                        }
                    }
                    else
                        circularProgressBar1.Value = 0;
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.cukry != 0)
                    {
                        if (Convert.ToInt32(suma[5, 6] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.cukry) > 100)
                        {
                            pb_kontrola_cukry.Value = 100;
                            kontrola_plus_cukry.Text = "+ " + Math.Round(suma[5, 6] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.cukry, 2) + " g";
                        }
                        else
                        {
                            kontrola_plus_cukry.Text = "";
                            pb_kontrola_cukry.Value = Convert.ToInt32(suma[5, 6] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.cukry);
                        }
                    }
                    else
                        pb_kontrola_cukry.Value = 0;
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.blonnik != 0)
                    {
                        if (Convert.ToInt32(suma[5, 7] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.blonnik) > 100)
                        {
                            circularProgressBar2.Value = 100;
                            label142.Text = "+ " + Math.Round(suma[5, 7] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.blonnik, 2) + " g";
                        }
                        else
                        {
                            label142.Text = "";
                            circularProgressBar2.Value = Convert.ToInt32(suma[5, 7] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.blonnik);
                        }
                    }
                    else
                        circularProgressBar2.Value = 0;
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.sod != 0)
                    {
                        if (Convert.ToInt32(suma[5, 8] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.sod) > 100)
                        {
                            circularProgressBar4.Value = 100;
                            label144.Text = "+ " + Math.Round(suma[5, 8] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.sod, 2) + " mg";
                        }
                        else
                        {
                            label144.Text = "";
                            circularProgressBar4.Value = Convert.ToInt32(suma[5, 8] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.sod);
                        }
                    }
                    else
                        circularProgressBar4.Value = 0;
                    if (Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn != 0)
                    {
                        if (Convert.ToInt32(suma[5, 3] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn) > 100)
                        {
                            circularProgressBar3.Value = 100;
                            label143.Text = "+ " + Math.Round(suma[5, 3] - Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn, 2) + " g";
                        }
                        else
                        {
                            label143.Text = "";
                            circularProgressBar3.Value = Convert.ToInt32(suma[5, 3] * 100 / Diety[k_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn);
                        }
                    }
                    else
                        circularProgressBar3.Value = 0;
                }
            }
            catch
            {
                MessageBox.Show("Nie można przeliczyć wartości, o które przekroczono limity diety", "Błąd");
            }
        }

        #endregion

        private void jadlospis_cb_ktn_Click(object sender, EventArgs e)
        {

        }
    }
}
