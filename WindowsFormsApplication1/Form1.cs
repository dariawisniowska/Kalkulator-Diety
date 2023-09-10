using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WindowsFormsApplication1.DAO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int wybranaDieta;
        int wybraneMiasto;
        string kategoria; 
        
        public static double przelicznik_Bialko = 4; //kcal na 1g
        public static double przelicznik_Weglowodany = 4; //kcal na 1g
        public static double przelicznik_Tluszcze = 9; //kcal na 1g

        Color highlightColor = Color.LightBlue;
        Color primaryColor = Color.FromArgb(44, 57, 64);

        public double[,] suma;
        public double[,] procent;
        public int[] last;

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
        Encoding enc = Encoding.GetEncoding("Windows-1250");
        
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            suma = new double[6, 34];
            procent = new double[6, 34];

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

            dekadowka_panel.AutoScroll = true;
            dekadowka_panel.FlowDirection = FlowDirection.LeftToRight;
            dekadowka_panel.VerticalScroll.Visible = false;
            dekadowka_panel.HorizontalScroll.Visible = false;
            dekadowka_panel.WrapContents = false; // Vertical rather than horizontal scrolling
            dekadowka_panel.BackColor = Color.White;
            dekadowka_panel.Size = new System.Drawing.Size(dekadowkaSize[0], dekadowkaSize[1]);

            LiczSrednia();
  }

        #region Aplikacja

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

        private void tb_masa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8 || e.KeyChar == ',' || (Keys)e.KeyChar == Keys.Enter)
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    btn_dodaj_Click(sender, e);
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

        private void lv_sniadanie_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btn_usun_Click(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void lv_IIsniadanie_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btn_usun_Click(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void lv_obiad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btn_usun_Click(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void lv_podwieczorek_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btn_usun_Click(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void lv_kolacja_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Back)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Usuwanie produktu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    btn_usun_Click(sender, e);
                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
        }
        #endregion

        #region Strona Główna
        private void btn_dodaj_Click(object sender, EventArgs e)
        {
            if(lb_produkty.SelectedIndex!=-1)
            {
                if (tb_masa.Text != "")
                {
                    try
                    {
                        double masa = Math.Round(double.Parse(tb_masa.Text),2);
                        int posilek = Int32.Parse(tc_posilki.SelectedIndex.ToString());
                        int ktory = lb_produkty.SelectedIndex;
                        string[] arr = new string[34];
                        ListViewItem itm;

                        switch (kategoria)
                        {
                            case "Wszystkie":
                                arr[0] = Lista[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Lista[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Lista[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();                               
                                arr[4] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Lista[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Lista[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Lista[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Lista[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Lista[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Lista[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Lista[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Lista[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Lista[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Lista[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Lista[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Lista[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Lista[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Lista[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Lista[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Lista[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Lista[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Lista[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Lista[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Lista[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Lista[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Lista[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Lista[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Lista[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Lista[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Lista[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Lista[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Lista[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();

                                break;
                            case "M":
                                arr[0] = Mieso[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Mieso[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Mieso[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Mieso[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Mieso[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Mieso[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Mieso[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Mieso[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Mieso[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Mieso[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Mieso[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Mieso[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Mieso[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Mieso[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Mieso[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Mieso[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Mieso[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Mieso[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Mieso[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Mieso[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Mieso[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Mieso[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Mieso[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Mieso[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Mieso[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Mieso[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Mieso[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Mieso[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Mieso[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Mieso[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Mieso[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Mieso[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Mieso[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "W":
                                arr[0] = Warzywa[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Warzywa[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Warzywa[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Warzywa[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Warzywa[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Warzywa[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Warzywa[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Warzywa[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Warzywa[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Warzywa[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Warzywa[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Warzywa[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Warzywa[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Warzywa[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Warzywa[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Warzywa[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Warzywa[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Warzywa[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Warzywa[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Warzywa[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Warzywa[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Warzywa[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Warzywa[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Warzywa[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Warzywa[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "O":
                                arr[0] = Owoce[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Owoce[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Owoce[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Owoce[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Owoce[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Owoce[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Owoce[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Owoce[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Owoce[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Owoce[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Owoce[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Owoce[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Owoce[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Owoce[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Owoce[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Owoce[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Owoce[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Owoce[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Owoce[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Owoce[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Owoce[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Owoce[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Owoce[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Owoce[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Owoce[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Owoce[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Owoce[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Owoce[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Owoce[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Owoce[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Owoce[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Owoce[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Owoce[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "S":
                                arr[0] = Slodycze[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Slodycze[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Slodycze[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Slodycze[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Slodycze[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Slodycze[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Slodycze[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Slodycze[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Slodycze[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Slodycze[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Slodycze[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Slodycze[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Slodycze[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Slodycze[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Slodycze[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Slodycze[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Slodycze[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Slodycze[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Slodycze[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Slodycze[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Slodycze[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Slodycze[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Slodycze[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Slodycze[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Slodycze[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "R":
                                arr[0] = Ryby[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Ryby[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Ryby[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Ryby[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Ryby[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Ryby[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Ryby[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Ryby[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Ryby[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Ryby[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Ryby[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Ryby[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Ryby[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Ryby[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Ryby[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Ryby[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Ryby[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Ryby[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Ryby[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Ryby[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Ryby[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Ryby[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Ryby[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Ryby[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Ryby[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Ryby[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Ryby[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Ryby[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Ryby[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Ryby[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Ryby[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Ryby[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Ryby[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "D":
                                arr[0] = Napoje[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Napoje[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Napoje[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Napoje[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Napoje[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Napoje[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Napoje[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Napoje[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Napoje[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Napoje[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Napoje[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Napoje[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Napoje[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Napoje[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Napoje[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Napoje[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Napoje[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Napoje[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Napoje[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Napoje[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Napoje[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Napoje[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Napoje[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Napoje[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Napoje[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Napoje[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Napoje[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Napoje[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Napoje[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Napoje[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Napoje[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Napoje[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Napoje[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "Z":
                                arr[0] = Zboza[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Zboza[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Zboza[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Zboza[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Zboza[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Zboza[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Zboza[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Zboza[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Zboza[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Zboza[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Zboza[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Zboza[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Zboza[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Zboza[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Zboza[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Zboza[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Zboza[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Zboza[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Zboza[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Zboza[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Zboza[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Zboza[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Zboza[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Zboza[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Zboza[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Zboza[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Zboza[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Zboza[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Zboza[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Zboza[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Zboza[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Zboza[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Zboza[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "P":
                                arr[0] = Przyprawy[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "N":
                                arr[0] = Nabial[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Nabial[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Nabial[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Nabial[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Nabial[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Nabial[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Nabial[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Nabial[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Nabial[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Nabial[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Nabial[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Nabial[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Nabial[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Nabial[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Nabial[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Nabial[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Nabial[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Nabial[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Nabial[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Nabial[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Nabial[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Nabial[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Nabial[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Nabial[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Nabial[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Nabial[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Nabial[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Nabial[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Nabial[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Nabial[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Nabial[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Nabial[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Nabial[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "B":
                                arr[0] = Bakalie[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Bakalie[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Bakalie[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Bakalie[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Bakalie[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Bakalie[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Bakalie[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Bakalie[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Bakalie[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Bakalie[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Bakalie[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Bakalie[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Bakalie[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Bakalie[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Bakalie[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Bakalie[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Bakalie[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Bakalie[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Bakalie[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Bakalie[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Bakalie[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Bakalie[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Bakalie[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Bakalie[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Bakalie[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                            case "T":
                                arr[0] = Tluszcze[ktory].nazwa;
                                arr[1] = masa.ToString();
                                arr[2] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                                arr[3] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                                arr[4] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                                arr[5] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                                arr[6] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                                arr[7] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.cukry * masa / 100.0, 2).ToString();
                                arr[8] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                                arr[9] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                                arr[10] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witA * masa / 100.0, 2).ToString();
                                arr[11] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witB1 * masa / 100.0, 2).ToString();
                                arr[12] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witB2 * masa / 100.0, 2).ToString();
                                arr[13] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.niacyna * masa / 100.0, 2).ToString();
                                arr[14] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witB6 * masa / 100.0, 2).ToString();
                                arr[15] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witB12 * masa / 100.0, 2).ToString();
                                arr[16] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witC * masa / 100.0, 2).ToString();
                                arr[17] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.foliany * masa / 100.0, 2).ToString();
                                arr[18] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.fosfor * masa / 100.0, 2).ToString();
                                arr[19] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.magnez * masa / 100.0, 2).ToString();
                                arr[20] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.zelazo * masa / 100.0, 2).ToString();
                                arr[21] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.cynk * masa / 100.0, 2).ToString();
                                arr[22] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.jod * masa / 100.0, 2).ToString();
                                arr[23] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.selen * masa / 100.0, 2).ToString();
                                arr[24] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.miedz * masa / 100.0, 2).ToString();
                                arr[25] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.cholina * masa / 100.0, 2).ToString();
                                arr[26] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.kwasPantotenowy * masa / 100.0, 2).ToString();
                                arr[27] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.biotyna * masa / 100.0, 2).ToString();
                                arr[28] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witD * masa / 100.0, 2).ToString();
                                arr[29] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witE * masa / 100.0, 2).ToString();
                                arr[30] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.witK * masa / 100.0, 2).ToString();
                                arr[31] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.mangan * masa / 100.0, 2).ToString();
                                arr[32] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.fluor * masa / 100.0, 2).ToString();
                                arr[33] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.potas * masa / 100.0, 2).ToString();
                                break;
                        }

                        itm = new ListViewItem(arr);
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
                    } catch {
                     MessageBox.Show("Nieprawidłowa wartość","Błąd");
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

        private void btn_usun_Click(object sender, EventArgs e)
        {
            int posilek = tc_posilki.SelectedIndex; 
            string produkt="";
            List<int> ktory = new List<int>();
            switch (posilek)
            {
                case 0:
                    ktory = new List<int>();
                         for (int k = 0; k < lv_sniadanie.SelectedIndices.Count; k++)
                             ktory.Add(Int32.Parse(lv_sniadanie.SelectedIndices[k].ToString()));
                         if(ktory.Count>0)
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
                MessageBox.Show("Nie wybrano produktu","Błąd");
            }
        }

        public void LiczSrednia()
        {
            for (int k = 0; k < 34; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma[i, k] = 0;
                    procent[i, k] = 0;
                }

            }

            string[] arr = new string[34];
            for (int i = 0; i < 34; i++)
                arr[i] = "0";
            ListViewItem itm = new ListViewItem(arr);
            itm.UseItemStyleForSubItems = false;

            for (int k = 0; k < 34; k++)
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

            for (int k = 0; k < 34; k++)
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

            for (int k = 0; k < 34; k++)
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
            for (int k = 0; k < 34; k++)
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


            for (int k = 0; k < 34; k++)
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

            for (int k = 0; k < 34; k++)
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

            //ZAWARTOSC
            try
            {
                if (cb_dieta.SelectedIndex != -1)
                {
                    e_limitOd.Text = Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energiaOd.ToString();
                    e_limitDo.Text = Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energiaDo.ToString();
                    b_limitOd.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialkoOd / 100, 2).ToString();
                    b_limitDo.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialkoDo / 100, 2).ToString();
                    t_limit_Od.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszczeOd / 100, 2).ToString();
                    t_limit_Do.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszczeDo / 100, 2).ToString();
                    ktn_limit.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn / 100, 2).ToString();
                    w_limitOd.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyOd / 100, 2).ToString();
                    w_limitDo.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyDo / 100, 2).ToString();
                    c_limit.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry / 100, 2).ToString();
                    blonnik_limit.Text = Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik / 100, 2).ToString();
                    s_limit.Text = Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Nie można przeliczyć wartości, o które przekroczono limity diety", "Błąd");
            }

            //WARTOŚCI
            e_text.Text = Math.Round(suma[5, 0], 2).ToString() + " kcal";
            if (Math.Round(suma[5, 0], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energiaDo / 100, 2) || Math.Round(suma[5, 0], 2) < Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.energiaOd / 100, 2))
                e_text.ForeColor = Color.Red;
            else
                e_text.ForeColor = Color.Green;

            b_text.Text = Math.Round(suma[5, 1], 2).ToString() + " g";
            if (Math.Round(suma[5, 1], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialkoDo / 100, 2) || Math.Round(suma[5, 1], 2) < Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.bialkoOd / 100, 2))
                b_text.ForeColor = Color.Red;
            else
                b_text.ForeColor = Color.Green;

            t_text.Text = Math.Round(suma[5, 2], 2).ToString() + " g";
            if (Math.Round(suma[5, 2], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszczeDo / 100, 2) || Math.Round(suma[5, 2], 2) < Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszczeOd / 100, 2))
                t_text.ForeColor = Color.Red;
            else
                t_text.ForeColor = Color.Green;

            k_text.Text = Math.Round(suma[5, 3], 2).ToString() + " g";
            if (Math.Round(suma[5, 3], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn / 100, 2))
                k_text.ForeColor = Color.Red;
            else
                k_text.ForeColor = Color.Green;

            w_text.Text = Math.Round(suma[5, 4], 2).ToString() + " g";
            if (Math.Round(suma[5, 4], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyDo / 100, 2) || Math.Round(suma[5, 4], 2) < Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyOd / 100, 2))
                w_text.ForeColor = Color.Red;
            else
                w_text.ForeColor = Color.Green;

            wp_text.Text = Math.Round(suma[5, 5], 2).ToString() + " g";
            if (Math.Round(suma[5, 5], 2) > Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cukry / 100, 2))
                wp_text.ForeColor = Color.Red;
            else
               wp_text.ForeColor = Color.Green;

            bp_text.Text = Math.Round(suma[5, 6], 2).ToString() + " g";
            if (Math.Round(suma[5, 6], 2) > 1.1*Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik / 100, 2) || Math.Round(suma[5, 6], 2) < 0.9*Math.Round(suma[5, 0] * Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.blonnik / 100, 2))
                bp_text.ForeColor = Color.Red;
            else
                bp_text.ForeColor = Color.Green;

            s_text.Text = Math.Round(suma[5, 7], 2).ToString() + " mg";
            if (Math.Round(suma[5, 7], 2) > Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.sod)
                s_text.ForeColor = Color.Red;
            else
                s_text.ForeColor = Color.Green;

            if (Convert.ToInt32(suma[5, 8] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witA) > 100)
                pb_witA.Value = 100;
            else
                pb_witA.Value = Convert.ToInt32(suma[5, 8]*100/ Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witA);

            if (Convert.ToInt32(suma[5, 9] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB1) > 100)
                pb_witb1.Value = 100;
            else
                pb_witb1.Value = Convert.ToInt32(suma[5, 9] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB1);

            if (Convert.ToInt32(suma[5, 10] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB2) > 100)
                pb_witb2.Value = 100;
            else
                pb_witb2.Value = Convert.ToInt32(suma[5, 10] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB2);

            if (Convert.ToInt32(suma[5, 11] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.niacyna) > 100)
                pb_niacyna.Value = 100;
            else
                pb_niacyna.Value = Convert.ToInt32(suma[5, 11] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.niacyna);

            if (Convert.ToInt32(suma[5, 12] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB6) > 100)
                pb_witB6.Value = 100;
            else
                pb_witB6.Value = Convert.ToInt32(suma[5, 12] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB6);

            if (Convert.ToInt32(suma[5, 13] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB12) > 100)
                pb_witB12.Value = 100;
            else
                pb_witB12.Value = Convert.ToInt32(suma[5, 13] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witB12);

            if (Convert.ToInt32(suma[5, 14] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witC) > 100)
                pb_witC.Value = 100;
            else
                pb_witC.Value = Convert.ToInt32(suma[5, 14] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witC);

            if (Convert.ToInt32(suma[5, 15] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.foliany) > 100)
                pb_foliany.Value = 100;
            else
                pb_foliany.Value = Convert.ToInt32(suma[5, 15] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.foliany);

            if (Convert.ToInt32(suma[5, 16] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.fosfor) > 100)
                pb_fosfor.Value = 100;
            else
                pb_fosfor.Value = Convert.ToInt32(suma[5, 16] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.fosfor);

            if (Convert.ToInt32(suma[5, 17] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.magnez) > 100)
                pb_magnez.Value = 100;
            else
                pb_magnez.Value = Convert.ToInt32(suma[5, 17] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.magnez);

            if (Convert.ToInt32(suma[5, 18] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.zelazo) > 100)
                pb_zelazo.Value = 100;
            else
                pb_zelazo.Value = Convert.ToInt32(suma[5, 18] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.zelazo);

            if (Convert.ToInt32(suma[5, 19] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cynk) > 100)
                pb_cynk.Value = 100;
            else
                pb_cynk.Value = Convert.ToInt32(suma[5, 19] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cynk);

            if (Convert.ToInt32(suma[5, 20] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.jod) > 100)
                pb_jod.Value = 100;
            else
                pb_jod.Value = Convert.ToInt32(suma[5, 20] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.jod);

            if (Convert.ToInt32(suma[5, 21] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.selen) > 100)
                pb_selen.Value = 100;
            else
                pb_selen.Value = Convert.ToInt32(suma[5, 21] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.selen);

            if (Convert.ToInt32(suma[5, 22] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.miedz) > 100)
                pb_miedz.Value = 100;
            else
                pb_miedz.Value = Convert.ToInt32(suma[5, 22] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.miedz);

            if (Convert.ToInt32(suma[5, 23] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cholina) > 100)
                pb_cholina.Value = 100;
            else
                pb_cholina.Value = Convert.ToInt32(suma[5, 23] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.cholina);

            if (Convert.ToInt32(suma[5, 24] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.kwasPantotenowy) > 100)
                pb_kwasPantotenowy.Value = 100;
            else
                pb_kwasPantotenowy.Value = Convert.ToInt32(suma[5, 24] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.kwasPantotenowy);

            if (Convert.ToInt32(suma[5, 25] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.biotyna) > 100)
                pb_biotyna.Value = 100;
            else
                pb_biotyna.Value = Convert.ToInt32(suma[5, 25] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.biotyna);

            if (Convert.ToInt32(suma[5, 26] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witD) > 100)
                pb_witD.Value = 100;
            else
                pb_witD.Value = Convert.ToInt32(suma[5, 26] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witD);

            if (Convert.ToInt32(suma[5, 27] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witD) > 100)
                pb_witE.Value = 100;
            else
                pb_witE.Value = Convert.ToInt32(suma[5, 27] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witD);

            if (Convert.ToInt32(suma[5, 28] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witK) > 100)
                pb_witK.Value = 100;
            else
                pb_witK.Value = Convert.ToInt32(suma[5, 28] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.witK);

            if (Convert.ToInt32(suma[5, 29] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.mangan) > 100)
                pb_mangan.Value = 100;
            else
                pb_mangan.Value = Convert.ToInt32(suma[5, 29] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.mangan);

            if (Convert.ToInt32(suma[5, 30] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.fluor) > 100)
                pb_fluor.Value = 100;
            else
                pb_fluor.Value = Convert.ToInt32(suma[5, 30] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.fluor);

            if (Convert.ToInt32(suma[5, 31] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.potas) > 100)
                pb_potas.Value = 100;
            else
                pb_potas.Value = Convert.ToInt32(suma[5, 31] * 100 / Diety[cb_dieta.SelectedIndex].wartosciOdzywcze.potas);

            for (int k = 0; k < 8; k++)
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
                        if (k ==7)
                            wartosc_odzywcza = wartosc_odzywcza / 1000;

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
        }

        private void cb_kategorie_SelectedIndexChanged(object sender, EventArgs e)
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
              
        private string GetMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "Styczeń";
                case 2:
                    return "Luty";
                case 3:
                    return "Marzec";
                case 4:
                    return "Kwiecień";
                case 5:
                    return "Maj";
                case 6:
                    return "Czerwiec";
                case 7:
                    return "Lipiec";
                case 8:
                    return "Sierpień";
                case 9:
                    return "Wrzesień";
                case 10:
                    return "Październik";
                case 11:
                    return "Listopad";
                case 12:
                    return "Grudzień";
            }
            return "";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try { 
                int tab = tc_posilki.SelectedIndex;
                switch (tab)
                {
                    case 0:
                        int liczba = lv_sniadanie.Items.Count;
                        int wybrany = lv_sniadanie.SelectedIndices[0];
                            string[] arr = new string[10];
                            double masa = double.Parse(tb_masa.Text);
                            arr[0] = lv_sniadanie.Items[wybrany].SubItems[0].Text;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[2].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text),2).ToString();
                            arr[3] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[3].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                            arr[4] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[4].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                            arr[5] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[5].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                            arr[6] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[6].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                            arr[7] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[7].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[8] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[8].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        arr[9] = Math.Round(masa * double.Parse(lv_sniadanie.Items[wybrany].SubItems[9].Text) / double.Parse(lv_sniadanie.Items[wybrany].SubItems[1].Text), 2).ToString();
                        ListViewItem itm = new ListViewItem(arr);

                            lv_sniadanie.Items.Remove(lv_sniadanie.Items[wybrany]);
                            lv_sniadanie.Items.Insert(wybrany, itm);
                            LiczSrednia();
                        break;
                    case 1:
                      liczba = lv_IIsniadanie.Items.Count;
                        wybrany = lv_IIsniadanie.SelectedIndices[0];
                            arr = new string[10];
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
                        itm = new ListViewItem(arr);

                            lv_IIsniadanie.Items.Remove(lv_IIsniadanie.Items[wybrany]);
                            lv_IIsniadanie.Items.Insert(wybrany, itm);
                            LiczSrednia();
                        break;
                    case 2:
                                            liczba = lv_obiad.Items.Count;
                        wybrany = lv_obiad.SelectedIndices[0];
                            arr = new string[10];
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
                        itm = new ListViewItem(arr);

                            lv_obiad.Items.Remove(lv_obiad.Items[wybrany]);
                            lv_obiad.Items.Insert(wybrany, itm);
                            LiczSrednia();
                        break;
                    case 3:
                                              liczba = lv_podwieczorek.Items.Count;
                        wybrany = lv_podwieczorek.SelectedIndices[0];
                            arr = new string[10];
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
                        itm = new ListViewItem(arr);

                            lv_podwieczorek.Items.Remove(lv_podwieczorek.Items[wybrany]);
                            lv_podwieczorek.Items.Insert(wybrany, itm);
                            LiczSrednia();
                        break;
                    case 4:
                                            liczba = lv_kolacja.Items.Count;
                        wybrany = lv_kolacja.SelectedIndices[0];
                            arr = new string[10];
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
                        itm = new ListViewItem(arr);

                            lv_kolacja.Items.Remove(lv_kolacja.Items[wybrany]);
                            lv_kolacja.Items.Insert(wybrany, itm);
                            LiczSrednia();
                        break;

                }
            }
            catch
            {
             MessageBox.Show("Nie można edytować","Błąd");
           }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
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
                   sklad_sniadanie += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "|" + lv_sniadanie.Items[i].SubItems[8].Text + "|" + lv_sniadanie.Items[i].SubItems[9].Text + "|" + lv_sniadanie.Items[i].SubItems[10].Text + "|" + lv_sniadanie.Items[i].SubItems[11].Text + "|" + lv_sniadanie.Items[i].SubItems[12].Text + "|" + lv_sniadanie.Items[i].SubItems[13].Text + "|" + lv_sniadanie.Items[i].SubItems[14].Text + "|" + lv_sniadanie.Items[i].SubItems[15].Text + "|" + lv_sniadanie.Items[i].SubItems[16].Text + "|" + lv_sniadanie.Items[i].SubItems[17].Text + "|" + lv_sniadanie.Items[i].SubItems[18].Text + "|" + lv_sniadanie.Items[i].SubItems[19].Text + "|" + lv_sniadanie.Items[i].SubItems[20].Text + "|" + lv_sniadanie.Items[i].SubItems[21].Text + "|" + lv_sniadanie.Items[i].SubItems[22].Text + "|" + lv_sniadanie.Items[i].SubItems[23].Text + "|" + lv_sniadanie.Items[i].SubItems[24].Text + "|" + lv_sniadanie.Items[i].SubItems[25].Text + "|" + lv_sniadanie.Items[i].SubItems[26].Text + "|" + lv_sniadanie.Items[i].SubItems[27].Text + "|" + lv_sniadanie.Items[i].SubItems[28].Text + "|" + lv_sniadanie.Items[i].SubItems[29].Text + "|" + lv_sniadanie.Items[i].SubItems[30].Text + "|" + lv_sniadanie.Items[i].SubItems[31].Text + "|" + lv_sniadanie.Items[i].SubItems[32].Text + "|" + lv_sniadanie.Items[i].SubItems[33].Text + "$";

                string sklad_IIsniadanie = "";
                for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                    sklad_IIsniadanie += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "|" + lv_IIsniadanie.Items[i].SubItems[11].Text + "|" + lv_IIsniadanie.Items[i].SubItems[12].Text + "|" + lv_IIsniadanie.Items[i].SubItems[13].Text + "|" + lv_IIsniadanie.Items[i].SubItems[14].Text + "|" + lv_IIsniadanie.Items[i].SubItems[15].Text + "|" + lv_IIsniadanie.Items[i].SubItems[16].Text + "|" + lv_IIsniadanie.Items[i].SubItems[17].Text + "|" + lv_IIsniadanie.Items[i].SubItems[18].Text + "|" + lv_IIsniadanie.Items[i].SubItems[19].Text + "|" + lv_IIsniadanie.Items[i].SubItems[20].Text + "|" + lv_IIsniadanie.Items[i].SubItems[21].Text + "|" + lv_IIsniadanie.Items[i].SubItems[22].Text + "|" + lv_IIsniadanie.Items[i].SubItems[23].Text + "|" + lv_IIsniadanie.Items[i].SubItems[24].Text + "|" + lv_IIsniadanie.Items[i].SubItems[25].Text + "|" + lv_IIsniadanie.Items[i].SubItems[26].Text + "|" + lv_IIsniadanie.Items[i].SubItems[27].Text + "|" + lv_IIsniadanie.Items[i].SubItems[28].Text + "|" + lv_IIsniadanie.Items[i].SubItems[29].Text + "|" + lv_IIsniadanie.Items[i].SubItems[30].Text + "|" + lv_IIsniadanie.Items[i].SubItems[31].Text + "|" + lv_IIsniadanie.Items[i].SubItems[32].Text + "|" + lv_IIsniadanie.Items[i].SubItems[33].Text + "$";

                string sklad_obiad = "";
                for (int i = 0; i < lv_obiad.Items.Count; i++)
                    sklad_obiad += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "|" + lv_obiad.Items[i].SubItems[11].Text + "|" + lv_obiad.Items[i].SubItems[12].Text + "|" + lv_obiad.Items[i].SubItems[13].Text + "|" + lv_obiad.Items[i].SubItems[14].Text + "|" + lv_obiad.Items[i].SubItems[15].Text + "|" + lv_obiad.Items[i].SubItems[16].Text + "|" + lv_obiad.Items[i].SubItems[17].Text + "|" + lv_obiad.Items[i].SubItems[18].Text + "|" + lv_obiad.Items[i].SubItems[19].Text + "|" + lv_obiad.Items[i].SubItems[20].Text + "|" + lv_obiad.Items[i].SubItems[21].Text + "|" + lv_obiad.Items[i].SubItems[22].Text + "|" + lv_obiad.Items[i].SubItems[23].Text + "|" + lv_obiad.Items[i].SubItems[24].Text + "|" + lv_obiad.Items[i].SubItems[25].Text + "|" + lv_obiad.Items[i].SubItems[26].Text + "|" + lv_obiad.Items[i].SubItems[27].Text + "|" + lv_obiad.Items[i].SubItems[28].Text + "|" + lv_obiad.Items[i].SubItems[29].Text + "|" + lv_obiad.Items[i].SubItems[30].Text + "|" + lv_obiad.Items[i].SubItems[31].Text + "|" + lv_obiad.Items[i].SubItems[32].Text + "|" + lv_obiad.Items[i].SubItems[33].Text + "$";

                string sklad_podwieczorek = "";
                for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                    sklad_podwieczorek += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "|" + lv_podwieczorek.Items[i].SubItems[11].Text + "|" + lv_podwieczorek.Items[i].SubItems[12].Text + "|" + lv_podwieczorek.Items[i].SubItems[13].Text + "|" + lv_podwieczorek.Items[i].SubItems[14].Text + "|" + lv_podwieczorek.Items[i].SubItems[15].Text + "|" + lv_podwieczorek.Items[i].SubItems[16].Text + "|" + lv_podwieczorek.Items[i].SubItems[17].Text + "|" + lv_podwieczorek.Items[i].SubItems[18].Text + "|" + lv_podwieczorek.Items[i].SubItems[19].Text + "|" + lv_podwieczorek.Items[i].SubItems[20].Text + "|" + lv_podwieczorek.Items[i].SubItems[21].Text + "|" + lv_podwieczorek.Items[i].SubItems[22].Text + "|" + lv_podwieczorek.Items[i].SubItems[23].Text + "|" + lv_podwieczorek.Items[i].SubItems[24].Text + "|" + lv_podwieczorek.Items[i].SubItems[25].Text + "|" + lv_podwieczorek.Items[i].SubItems[26].Text + "|" + lv_podwieczorek.Items[i].SubItems[27].Text + "|" + lv_podwieczorek.Items[i].SubItems[28].Text + "|" + lv_podwieczorek.Items[i].SubItems[29].Text + "|" + lv_podwieczorek.Items[i].SubItems[30].Text + "|" + lv_podwieczorek.Items[i].SubItems[31].Text + "|" + lv_podwieczorek.Items[i].SubItems[32].Text + "|" + lv_podwieczorek.Items[i].SubItems[33].Text + "$";

                string sklad_kolacja = "";
                for (int i = 0; i < lv_kolacja.Items.Count; i++)
                    sklad_kolacja += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "|" + lv_kolacja.Items[i].SubItems[11].Text + "|" + lv_kolacja.Items[i].SubItems[12].Text + "|" + lv_kolacja.Items[i].SubItems[13].Text + "|" + lv_kolacja.Items[i].SubItems[14].Text + "|" + lv_kolacja.Items[i].SubItems[15].Text + "|" + lv_kolacja.Items[i].SubItems[16].Text + "|" + lv_kolacja.Items[i].SubItems[17].Text + "|" + lv_kolacja.Items[i].SubItems[18].Text + "|" + lv_kolacja.Items[i].SubItems[19].Text + "|" + lv_kolacja.Items[i].SubItems[20].Text + "|" + lv_kolacja.Items[i].SubItems[21].Text + "|" + lv_kolacja.Items[i].SubItems[22].Text + "|" + lv_kolacja.Items[i].SubItems[23].Text + "|" + lv_kolacja.Items[i].SubItems[24].Text + "|" + lv_kolacja.Items[i].SubItems[25].Text + "|" + lv_kolacja.Items[i].SubItems[26].Text + "|" + lv_kolacja.Items[i].SubItems[27].Text + "|" + lv_kolacja.Items[i].SubItems[28].Text + "|" + lv_kolacja.Items[i].SubItems[29].Text + "|" + lv_kolacja.Items[i].SubItems[30].Text + "|" + lv_kolacja.Items[i].SubItems[31].Text + "|" + lv_kolacja.Items[i].SubItems[32].Text + "|" + lv_kolacja.Items[i].SubItems[33].Text + "$";

                string data = dateTimePicker1.Text.Split(',')[1];
                string first = data.Substring(0, 1);
                if (first == " ")
                    data = data.Remove(0, 1);
                DAO.JadlospisDAO.InsertSQL(data, cb_dieta.Text.Split('/')[0], cb_miasto.SelectedItem.ToString(), cb_dieta.Text.Split('/')[1], textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, sklad_sniadanie,sklad_IIsniadanie,sklad_obiad,sklad_podwieczorek,sklad_kolacja);

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
        private void utwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void wartościOdżywczeDietyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Czy na pewno utworzyć dokument z wartościami odżywczymi stworzonej diety?", "Tworzenie dokumentu", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Encoding enc = Encoding.GetEncoding("Windows-1250");
                string path = "Jadłospisy PDF\\" + cb_miasto.SelectedItem + "\\" + dateTimePicker1.Value.Year + "\\" + GetMonth(dateTimePicker1.Value.Month) + "\\" + dateTimePicker1.Value.Day + "\\";
                string nazwa = path + dateTimePicker1.Text + ", " + cb_dieta.Text + ".pdf";
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                System.IO.Directory.CreateDirectory(path);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(nazwa, FileMode.Create));
                var Calibri16 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, 11);
                var Calibri12 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, 9);
                doc.Open();
                string naglowek = dateTimePicker1.Text + "\n\n" + cb_dieta.Text + "\n";
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(naglowek, Calibri16);
                paragraph.Alignment = 1;
                doc.Add(paragraph);
                iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, 1)));
                doc.Add(p);
                string[] kolumny = new string[8];
                kolumny[0] = "Nazwa produktu";
                kolumny[1] = "Masa [g]";
                kolumny[2] = "Energia [kcal]";
                kolumny[3] = "Białko [g]";
                kolumny[5] = "Węglowodany [g]";
                kolumny[4] = " Tłuszcze[g]";
                kolumny[7] = "NKT[g]";
                kolumny[6] = "Sód [mg]";


                if (suma[0, 0] != 0)
                {
                    string s;
                    string naglowek2 = "\n\n";
                    iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(naglowek2, Calibri12);
                    doc.Add(paragraph2);
                    PdfPTable table = new PdfPTable(8);

                    PdfPCell cell = new PdfPCell(new Phrase("Śniadanie", Calibri16));
                    cell.Colspan = 8;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    if (textBox1.Text != "")
                    {
                        PdfPCell cel = new PdfPCell(new Phrase(textBox1.Text, Calibri16));
                        cel.Colspan = 8;
                        cel.HorizontalAlignment = 1;
                        table.AddCell(cel);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                        table.AddCell(left);
                    }

                    for (int k = 0; k < lv_sniadanie.Items.Count; k++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            s = lv_sniadanie.Items[k].SubItems[i].ToString();
                            s = s.Remove(0, 18);
                            s = s.Replace('}', ' ');
                            PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                            table.AddCell(left);
                        }
                    }
                    PdfPCell ce = new PdfPCell(new Phrase("Razem", Calibri16));
                    ce.Colspan = 2;
                    table.AddCell(ce);
                    ce.HorizontalAlignment = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        s = suma[0, i].ToString();
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                        table.AddCell(left);
                    }
                    doc.Add(table);
                }
                if (suma[1, 0] != 0)
                {
                    string s = "";
                    string naglowek2 = "\n\n";
                    iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(naglowek2, Calibri12);
                    doc.Add(paragraph2);
                    PdfPTable table = new PdfPTable(8);

                    PdfPCell cell = new PdfPCell(new Phrase("Drugie śniadanie", Calibri16));
                    cell.Colspan = 8;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    if (textBox2.Text != "")
                    {
                        PdfPCell cel = new PdfPCell(new Phrase(textBox2.Text, Calibri16));
                        cel.Colspan = 8;
                        cel.HorizontalAlignment = 1;
                        table.AddCell(cel);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                        table.AddCell(left);
                    }

                    for (int k = 0; k < lv_IIsniadanie.Items.Count; k++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            s = lv_IIsniadanie.Items[k].SubItems[i].ToString();
                            s = s.Remove(0, 18);
                            s = s.Replace('}', ' ');
                            PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                            table.AddCell(left);
                        }
                    }

                    PdfPCell ce = new PdfPCell(new Phrase("Razem", Calibri16));
                    ce.Colspan = 2;
                    table.AddCell(ce);
                    ce.HorizontalAlignment = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        s = suma[1, i].ToString();
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                        table.AddCell(left);
                    }
                    doc.Add(table);
                }
                if (suma[2, 0] != 0)
                {
                    string s = "";
                    string naglowek2 = "\n\n";
                    iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(naglowek2, Calibri16);
                    doc.Add(paragraph2);
                    PdfPTable table = new PdfPTable(8);

                    PdfPCell cell = new PdfPCell(new Phrase("Obiad", Calibri16));
                    cell.Colspan = 8;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    if (textBox3.Text != "")
                    {
                        PdfPCell cel = new PdfPCell(new Phrase(textBox3.Text, Calibri16));
                        cel.Colspan = 8;
                        cel.HorizontalAlignment = 1;
                        table.AddCell(cel);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                        table.AddCell(left);
                    }

                    for (int k = 0; k < lv_obiad.Items.Count; k++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            s = lv_obiad.Items[k].SubItems[i].ToString();
                            s = s.Remove(0, 18);
                            s = s.Replace('}', ' ');
                            PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                            table.AddCell(left);
                        }
                    }
                    PdfPCell ce = new PdfPCell(new Phrase("Razem", Calibri16));
                    ce.Colspan = 2;
                    table.AddCell(ce);
                    ce.HorizontalAlignment = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        s = suma[2, i].ToString();
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                        table.AddCell(left);
                    }
                    doc.Add(table);

                }
                if (suma[3, 0] != 0)
                {
                    string naglowek2 = "\n\n";
                    iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(naglowek2, Calibri16);
                    doc.Add(paragraph2);
                    PdfPTable table = new PdfPTable(8);


                    PdfPCell cell = new PdfPCell(new Phrase("Podwieczorek", Calibri16));
                    cell.Colspan = 8;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    if (textBox4.Text != "")
                    {
                        PdfPCell cel = new PdfPCell(new Phrase(textBox4.Text, Calibri16));
                        cel.Colspan = 8;
                        cel.HorizontalAlignment = 1;
                        table.AddCell(cel);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                        table.AddCell(left);
                    }
                    string s = "";
                    for (int k = 0; k < lv_podwieczorek.Items.Count; k++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            s = lv_podwieczorek.Items[k].SubItems[i].ToString();
                            s = s.Remove(0, 18);
                            s = s.Replace('}', ' ');
                            PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                            table.AddCell(left);
                        }
                    }
                    PdfPCell ce = new PdfPCell(new Phrase("Razem", Calibri16));
                    ce.Colspan = 2;
                    table.AddCell(ce);
                    ce.HorizontalAlignment = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        s = suma[3, i].ToString();
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                        table.AddCell(left);
                    }
                    doc.Add(table);

                }


                if (suma[4, 0] != 0)
                {
                    string naglowek2 = "\n\n";
                    iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(naglowek2, Calibri16);
                    doc.Add(paragraph2);
                    PdfPTable table = new PdfPTable(8);

                    PdfPCell cell = new PdfPCell(new Phrase("Kolacja", Calibri16));
                    cell.Colspan = 8;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    if (textBox5.Text != "")
                    {
                        PdfPCell cel = new PdfPCell(new Phrase(textBox5.Text, Calibri16));
                        cel.Colspan = 8;
                        cel.HorizontalAlignment = 1;
                        table.AddCell(cel);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                        table.AddCell(left);
                    }
                    string s = "";
                    for (int k = 0; k < lv_kolacja.Items.Count; k++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            s = lv_kolacja.Items[k].SubItems[i].ToString();
                            s = s.Remove(0, 18);
                            s = s.Replace('}', ' ');
                            PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                            table.AddCell(left);
                        }
                    }
                    PdfPCell ce = new PdfPCell(new Phrase("Razem", Calibri16));
                    ce.Colspan = 2;
                    table.AddCell(ce);
                    ce.HorizontalAlignment = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        s = suma[4, i].ToString();
                        PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                        table.AddCell(left);
                    }
                    doc.Add(table);

                }
                PdfPTable table2 = new PdfPTable(6);
                string naglowek3 = "\n\n";
                iTextSharp.text.Paragraph paragraph3 = new iTextSharp.text.Paragraph(naglowek3, Calibri16);
                doc.Add(paragraph3);
                PdfPCell cell2 = new PdfPCell(new Phrase("Razem", Calibri16));
                cell2.Colspan = 6;
                cell2.HorizontalAlignment = 1;
                table2.AddCell(cell2);
                for (int i = 2; i < 8; i++)
                {
                    PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(kolumny[i], Calibri12));
                    table2.AddCell(left);
                }

                for (int i = 0; i < 6; i++)
                {

                    string s = suma[5, i].ToString();
                    //s = s.Remove(0, 18);
                    //s = s.Replace(':', ' ');
                    PdfPCell left = new PdfPCell(new iTextSharp.text.Paragraph(s, Calibri12));
                    table2.AddCell(left);
                }
                doc.Add(table2);

                doc.Close();
                MessageBox.Show("Utworzono dokument z wartościami odżywczymi dla diety:\n" + cb_dieta.SelectedItem + " " + dateTimePicker1.Value.Day + " " + GetMonth(dateTimePicker1.Value.Month) + " " + dateTimePicker1.Value.Year, "Tworzenie dokumentu");
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }
        
        private void jadłospisDziennyToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                string nazwa = "";
                string sklad = "";
                switch (posilek)
                {
                    case 0:
                        nazwa = textBox1.Text;
                        for (int i = 0; i < lv_sniadanie.Items.Count; i++)
                            sklad += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox1.Text, "Zapisano");
                        break;
                    case 1:
                        nazwa = textBox2.Text;
                        for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                            sklad += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox2.Text, "Zapisano");
                        break;
                    case 2:
                        nazwa = textBox3.Text;
                        for (int i = 0; i < lv_obiad.Items.Count; i++)
                            sklad += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox3.Text, "Zapisano");
                        break;
                    case 3:
                        nazwa = textBox4.Text;
                        for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                            sklad += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox4.Text, "Zapisano");
                        break;
                    case 4:
                        nazwa = textBox5.Text;
                        for (int i = 0; i < lv_kolacja.Items.Count; i++)
                            sklad += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "$";
                        MessageBox.Show("Zapisano recepturę: " + textBox5.Text, "Zapisano");
                        break;
                }
                RecepturaDAO.InsertSQL(nazwa, sklad);
            }
            else if (dialogResult == DialogResult.No)
            {

            }
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

            panel_receptura.Visible = true;
            panel_receptura.BringToFront();

            pictureBox14_Click(null,null);

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
            label10.Text = "Jadłospisy";

            panel_jadlospis.Visible = true;
            panel_jadlospis.BringToFront();

            pictureBox23.Visible = false;
            pictureBox24.Visible = false;
            pictureBox25.Visible = true;

            wczytajJadlospis();

            if(jadlospis_miasto.Items.Count>0)
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
            label10.Text = "Szablony";
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = true;
            panel_dekadowka.BringToFront();

            dekadowka_miasto.Items.Clear();
            listaJednostek = JednostkaDAO.SelectAllSQL();
            foreach (Jednostka j in listaJednostek)
                dekadowka_miasto.Items.Add(j.miasto);

            if(dekadowka_miasto.Items.Count>0)
                dekadowka_miasto.SelectedIndex = 0;
            dekadowka_dekadowka.Items.Clear();
            listaDekadowek =  DekadowkaDAO.SelectSQL(dekadowka_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowek)
                dekadowka_dekadowka.Items.Add(d.nazwa);
            if(dekadowka_dekadowka.Items.Count > 0)
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
            panel_glowny.Visible = true;
            panel_produkty.Visible = false;
            panel_dekadowka.Visible = false;
            panel_dekadowka_zapisz.Visible = false;
            panel_dieta.Visible = false;
            panel_glowny.BringToFront();
            label10.Text = "Strona Główna";

            

            lb_produkty.Items.Clear();
            Lista = DAO.ProduktDAO.SelectAllSQL();
            Lista = Lista.OrderBy(x=>x.nazwa).Cast<Produkt>().ToList();

            foreach (Produkt p in Lista)
                lb_produkty.Items.Add(p.nazwa);

            cb_kategorie.SelectedIndex = 0;

            cb_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
            produkt_witA.Enabled = true;
            produkt_witA.BackColor = Color.White;
            produkt_witB1.Enabled = true;
            produkt_witB1.BackColor = Color.White;
            produkt_witB2.Enabled = true;
            produkt_witB2.BackColor = Color.White;
            produkt_witC.Enabled = true;
            produkt_witC.BackColor = Color.White;
            produkt_niacyna.Enabled = true;
            produkt_niacyna.BackColor = Color.White;
            produkt_witB6.Enabled = true;
            produkt_witB6.BackColor = Color.White;
            produkt_witB12.Enabled = true;
            produkt_witB12.BackColor = Color.White;
            produkt_foliany.Enabled = true;
            produkt_foliany.BackColor = Color.White;
            produkt_fosfor.Enabled = true;
            produkt_fosfor.BackColor = Color.White;
            produkt_magnez.Enabled = true;
            produkt_magnez.BackColor = Color.White;
            produkt_zelazo.Enabled = true;
            produkt_zelazo.BackColor = Color.White;
            produkt_cynk.Enabled = true;
            produkt_cynk.BackColor = Color.White;
            produkt_selen.Enabled = true;
            produkt_selen.BackColor = Color.White;
            produkt_miedz.Enabled = true;
            produkt_miedz.BackColor = Color.White;
            produkt_cholina.Enabled = true;
            produkt_cholina.BackColor = Color.White;
            produkt_kwasPantotenowy.Enabled = true;
            produkt_kwasPantotenowy.BackColor = Color.White;
            produkt_biotyna.Enabled = true;
            produkt_biotyna.BackColor = Color.White;
            produkt_witD.Enabled = true;
            produkt_witD.BackColor = Color.White;
            produkt_witE.Enabled = true;
            produkt_witE.BackColor = Color.White;
            produkt_witK.Enabled = true;
            produkt_witK.BackColor = Color.White;
            produkt_mangan.Enabled = true;
            produkt_mangan.BackColor = Color.White;
            produkt_fluor.Enabled = true;
            produkt_fluor.BackColor = Color.White;
            produkt_potas.Enabled = true;
            produkt_potas.BackColor = Color.White;
            produkt_jod.Enabled = true;
            produkt_jod.BackColor = Color.White;

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
                    DAO.ProduktDAO.DeleteSQL(Lista[produkt_wczytaj.SelectedIndex]);
                    MessageBox.Show("Usunięto: " + Lista[produkt_wczytaj.SelectedIndex].nazwa);
                    produktClick();
                    break;
                default:
                    break;
            }
          
        }

        private void produkt_wstecz_Click(object sender, EventArgs e)
        {
            produkt_kategoria.Items.Clear();
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

            produkt_nazwa.Enabled = false;
            produkt_nazwa.BackColor = Color.FromName("ControlLight");
            produkty_przyswajalne.Enabled = false;
            produkty_przyswajalne.BackColor = Color.FromName("ControlLight");
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
            produkt_witA.Enabled = false;
            produkt_witA.BackColor = Color.FromName("ControlLight");
            produkt_witB1.Enabled = false;
            produkt_witB1.BackColor = Color.FromName("ControlLight");
            produkt_witB2.Enabled = false;
            produkt_witB2.BackColor = Color.FromName("ControlLight");
            produkt_witC.Enabled = false;
            produkt_witC.BackColor = Color.FromName("ControlLight");
            produkt_niacyna.Enabled = false;
            produkt_niacyna.BackColor = Color.FromName("ControlLight");
            produkt_witB6.Enabled = false;
            produkt_witB6.BackColor = Color.FromName("ControlLight");
            produkt_witB12.Enabled = false;
            produkt_witB12.BackColor = Color.FromName("ControlLight");
            produkt_foliany.Enabled = false;
            produkt_foliany.BackColor = Color.FromName("ControlLight");
            produkt_fosfor.Enabled = false;
            produkt_fosfor.BackColor = Color.FromName("ControlLight");
            produkt_magnez.Enabled = false;
            produkt_magnez.BackColor = Color.FromName("ControlLight");
            produkt_zelazo.Enabled = false;
            produkt_zelazo.BackColor = Color.FromName("ControlLight");
            produkt_cynk.Enabled = false;
            produkt_cynk.BackColor = Color.FromName("ControlLight");
            produkt_selen.Enabled = false;
            produkt_selen.BackColor = Color.FromName("ControlLight");
            produkt_miedz.Enabled = false;
            produkt_miedz.BackColor = Color.FromName("ControlLight");
            produkt_cholina.Enabled = false;
            produkt_cholina.BackColor = Color.FromName("ControlLight");
            produkt_kwasPantotenowy.Enabled = false;
            produkt_kwasPantotenowy.BackColor = Color.FromName("ControlLight");
            produkt_biotyna.Enabled = false;
            produkt_biotyna.BackColor = Color.FromName("ControlLight");
            produkt_witD.Enabled = false;
            produkt_witD.BackColor = Color.FromName("ControlLight");
            produkt_witE.Enabled = false;
            produkt_witE.BackColor = Color.FromName("ControlLight");
            produkt_witK.Enabled = false;
            produkt_witK.BackColor = Color.FromName("ControlLight");
            produkt_mangan.Enabled = false;
            produkt_mangan.BackColor = Color.FromName("ControlLight");
            produkt_fluor.Enabled = false;
            produkt_fluor.BackColor = Color.FromName("ControlLight");
            produkt_potas.Enabled = false;
            produkt_potas.BackColor = Color.FromName("ControlLight");
            produkt_jod.Enabled = false;
            produkt_jod.BackColor = Color.FromName("ControlLight");

            produkt_edytuj.Visible = true;
            produkt_usun.Visible = true;
            produkt_dodaj.Visible = true;
            produkt_wstecz.Visible = false;
            produkt_zapisz.Visible = false;

            label27.Visible = true;
            produkt_wczytaj.Visible = true;

            Lista = DAO.ProduktDAO.SelectAllSQL();
            produkt_wczytaj.Items.Clear();
           
            Lista = Lista.OrderBy(x => x.nazwa).Cast<Produkt>().ToList();
            for (int i = 0; i < Lista.Count; i++)
            {
                produkt_wczytaj.Items.Add(Lista[i].nazwa);
                produkt_wczytaj.SelectedIndex = 0;
            }
  
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
            produkt_witA.Enabled = true;
            produkt_witA.BackColor = Color.White;
            produkt_witB1.Enabled = true;
            produkt_witB1.BackColor = Color.White;
            produkt_witB2.Enabled = true;
            produkt_witB2.BackColor = Color.White;
            produkt_witC.Enabled = true;
            produkt_witC.BackColor = Color.White;
            produkt_niacyna.Enabled = true;
            produkt_niacyna.BackColor = Color.White;
            produkt_witB6.Enabled = true;
            produkt_witB6.BackColor = Color.White;
            produkt_witB12.Enabled = true;
            produkt_witB12.BackColor = Color.White;
            produkt_foliany.Enabled = true;
            produkt_foliany.BackColor = Color.White;
            produkt_fosfor.Enabled = true;
            produkt_fosfor.BackColor = Color.White;
            produkt_magnez.Enabled = true;
            produkt_magnez.BackColor = Color.White;
            produkt_zelazo.Enabled = true;
            produkt_zelazo.BackColor = Color.White;
            produkt_cynk.Enabled = true;
            produkt_cynk.BackColor = Color.White;
            produkt_selen.Enabled = true;
            produkt_selen.BackColor = Color.White;
            produkt_miedz.Enabled = true;
            produkt_miedz.BackColor = Color.White;
            produkt_cholina.Enabled = true;
            produkt_cholina.BackColor = Color.White;
            produkt_kwasPantotenowy.Enabled = true;
            produkt_kwasPantotenowy.BackColor = Color.White;
            produkt_biotyna.Enabled = true;
            produkt_biotyna.BackColor = Color.White;
            produkt_witD.Enabled = true;
            produkt_witD.BackColor = Color.White;
            produkt_witE.Enabled = true;
            produkt_witE.BackColor = Color.White;
            produkt_witK.Enabled = true;
            produkt_witK.BackColor = Color.White;
            produkt_mangan.Enabled = true;
            produkt_mangan.BackColor = Color.White;
            produkt_fluor.Enabled = true;
            produkt_fluor.BackColor = Color.White;
            produkt_potas.Enabled = true;
            produkt_potas.BackColor = Color.White;
            produkt_jod.Enabled = true;
            produkt_jod.BackColor = Color.White;

            produkt_edytuj.Visible = false;
            produkt_usun.Visible = false;
            produkt_dodaj.Visible = false;
            produkt_wstecz.Visible = true;
            produkt_zapisz.Visible = true;

            produkt_nazwa.Text = "";
            produkt_kategoria.SelectedIndex = -1;
            produkt_energia.Text = "";
            produkt_bialko.Text = "";
            produkt_weglowodany.Text = "";
            produkt_tluszcze.Text = "";
            produkt_sod.Text = "";
            produkt_tluszcze_nn.Text = "";
            produkt_sol.Text = "";
            produkty_blonnik.Text = "";
            produkty_przyswajalne.Text = "";
            produkt_witA.Text = "";
            produkt_witB1.Text = "";
            produkt_witB2.Text = "";
            produkt_witC.Text = "";
            produkt_niacyna.Text = "";
            produkt_witB6.Text = "";
            produkt_witB12.Text = "";
            produkt_foliany.Text = "";
            produkt_fosfor.Text = "";
            produkt_magnez.Text = "";
            produkt_zelazo.Text = "";
            produkt_cynk.Text = "";
            produkt_selen.Text = "";
            produkt_miedz.Text = "";
            produkt_cholina.Text = "";
            produkt_kwasPantotenowy.Text = "";
            produkt_biotyna.Text = "";
            produkt_witD.Text = "";
            produkt_witE.Text = "";
            produkt_witK.Text = "";
            produkt_mangan.Text = "";
            produkt_fluor.Text = "";
            produkt_potas.Text = "";
            produkt_jod.Text = "";

            label10.Text = "Produkty -> Dodaj";
        }

        private void produkt_zapisz_Click(object sender, EventArgs e)
        {
            char kategoria = 'A';
            switch (label10.Text)
            {
                case "Produkty -> Dodaj":
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
                            DAO.ProduktDAO.InsertSQL(produkt_nazwa.Text, kategoria, new WartosciOdzywcze(Convert.ToDouble(produkt_energia.Text), Convert.ToDouble(produkt_bialko.Text), Convert.ToDouble(produkt_tluszcze.Text), Convert.ToDouble(produkt_tluszcze_nn.Text), Convert.ToDouble(produkt_weglowodany.Text), Convert.ToDouble(produkty_przyswajalne.Text), Convert.ToDouble(produkty_blonnik.Text), Convert.ToDouble(produkt_sod.Text), Convert.ToDouble(produkt_witA.Text), Convert.ToDouble(produkt_witB1.Text), Convert.ToDouble(produkt_witB2.Text), Convert.ToDouble(produkt_witB6.Text), Convert.ToDouble(produkt_witB12.Text), Convert.ToDouble(produkt_niacyna.Text), Convert.ToDouble(produkt_witC.Text), Convert.ToDouble(produkt_witD.Text), Convert.ToDouble(produkt_witE.Text), Convert.ToDouble(produkt_witK.Text), Convert.ToDouble(produkt_foliany.Text), Convert.ToDouble(produkt_fosfor.Text), Convert.ToDouble(produkt_magnez.Text), Convert.ToDouble(produkt_zelazo.Text), Convert.ToDouble(produkt_cynk.Text), Convert.ToDouble(produkt_jod.Text), Convert.ToDouble(produkt_selen.Text), Convert.ToDouble(produkt_miedz.Text), Convert.ToDouble(produkt_cholina.Text), Convert.ToDouble(produkt_kwasPantotenowy.Text), Convert.ToDouble(produkt_biotyna.Text), Convert.ToDouble(produkt_mangan.Text), Convert.ToDouble(produkt_fluor.Text), Convert.ToDouble(produkt_potas.Text)));
                            MessageBox.Show("Dodano: " + produkt_nazwa.Text);
                            produktClick();
                        }
                        else
                        {
                            MessageBox.Show("Nie uzupełniono wszystkich danych", "Błąd");
                        }
                    }
                    catch {
                        MessageBox.Show("Błąd dodawania produktu","Błąd");
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
                    DAO.ProduktDAO.UpdateSQL(Lista[produkt_wczytaj.SelectedIndex], produkt_nazwa.Text, kategoria, new WartosciOdzywcze(Convert.ToDouble(produkt_energia.Text), Convert.ToDouble(produkt_bialko.Text), Convert.ToDouble(produkt_tluszcze.Text), Convert.ToDouble(produkt_tluszcze_nn.Text), Convert.ToDouble(produkt_weglowodany.Text), Convert.ToDouble(produkty_przyswajalne.Text), Convert.ToDouble(produkty_blonnik.Text), Convert.ToDouble(produkt_sod.Text), Convert.ToDouble(produkt_witA.Text), Convert.ToDouble(produkt_witB1.Text), Convert.ToDouble(produkt_witB2.Text), Convert.ToDouble(produkt_witB6.Text), Convert.ToDouble(produkt_witB12.Text), Convert.ToDouble(produkt_niacyna.Text), Convert.ToDouble(produkt_witC.Text), Convert.ToDouble(produkt_witD.Text), Convert.ToDouble(produkt_witE.Text), Convert.ToDouble(produkt_witK.Text), Convert.ToDouble(produkt_foliany.Text), Convert.ToDouble(produkt_fosfor.Text), Convert.ToDouble(produkt_magnez.Text), Convert.ToDouble(produkt_zelazo.Text), Convert.ToDouble(produkt_cynk.Text), Convert.ToDouble(produkt_jod.Text), Convert.ToDouble(produkt_selen.Text), Convert.ToDouble(produkt_miedz.Text), Convert.ToDouble(produkt_cholina.Text), Convert.ToDouble(produkt_kwasPantotenowy.Text), Convert.ToDouble(produkt_biotyna.Text), Convert.ToDouble(produkt_mangan.Text), Convert.ToDouble(produkt_fluor.Text), Convert.ToDouble(produkt_potas.Text)));
                            MessageBox.Show("Edytowano: "+produkt_nazwa.Text);
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
            if (Lista.Count > 0)
            {
                produkt_nazwa.Text = Lista[produkt_wczytaj.SelectedIndex].nazwa;
                produkt_energia.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.energia.ToString();
                produkt_bialko.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.bialko.ToString();
                produkt_weglowodany.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.weglowodany.ToString();
                produkt_tluszcze.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.tluszcze.ToString();
                produkt_sod.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.sod.ToString();
                produkty_przyswajalne.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.cukry.ToString();
                produkt_tluszcze_nn.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.tluszcze_nn.ToString();
                produkty_blonnik.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.blonnik.ToString();
                produkt_witA.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witA.ToString();
                produkt_witB1.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witB1.ToString();
                produkt_witB2.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witB2.ToString();
                produkt_witB6.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witB6.ToString();
                produkt_witB12.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witB12.ToString();
                produkt_witD.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witD.ToString();
                produkt_witE.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witE.ToString();
                produkt_witK.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witK.ToString();
                produkt_witC.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.witC.ToString();
                produkt_niacyna.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.niacyna.ToString();
                produkt_foliany.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.foliany.ToString();
                produkt_fosfor.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.fosfor.ToString();
                produkt_magnez.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.magnez.ToString();
                produkt_zelazo.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.zelazo.ToString();
                produkt_cynk.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.cynk.ToString();
                produkt_selen.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.selen.ToString();
                produkt_miedz.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.miedz.ToString();
                produkt_cholina.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.cholina.ToString();
                produkt_kwasPantotenowy.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.kwasPantotenowy.ToString();
                produkt_mangan.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.mangan.ToString();
                produkt_fluor.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.fluor.ToString();
                produkt_potas.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.potas.ToString();
                produkt_jod.Text = Lista[produkt_wczytaj.SelectedIndex].wartosciOdzywcze.jod.ToString();

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
        }

        private void cb_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybraneMiasto = cb_miasto.SelectedIndex;
            if (wybraneMiasto != -1)
            {
                cb_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAllSQL(cb_miasto.SelectedItem.ToString());
                foreach (Dieta d in Diety)
                    cb_dieta.Items.Add(d.nazwa + '/' + d.plec);
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

        List<Dekadowka> listaDekadowek;
        Dekadowka wybranaDekadowka;

        int[] dekadowkaSize = new int[] { 900, 470 };
        int[] dzienSize = new int[] { 150, 400 };
        int[] dietaSize = new int[] { 140, 200 };

        public void GenerateCards()
        {
            dekadowka_panel.Controls.Clear();

            Dekadowka[] jadlospisyDanejDekadowki = DAO.JadlospisDekadowkiDAO.SelectForAllDaysSQL(wybranaDekadowka);

            for (int j = 0; j < wybranaDekadowka.dni; j++)
            {
                FlowLayoutPanel dayOfWeek = new FlowLayoutPanel
                {
                    BackColor = Color.White,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false, // Vertical rather than horizontal scrolling
                    Size = new System.Drawing.Size(dzienSize[0], dzienSize[1])
                };
                dayOfWeek.VerticalScroll.Visible = false;
                dayOfWeek.HorizontalScroll.Visible = false;

                string day = GetDay(wybranaDekadowka.dzienStart,j+1);
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
            listaDekadowek = DekadowkaDAO.SelectSQL(dekadowka_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowek)
                dekadowka_dekadowka.Items.Add(d.nazwa);
            if(dekadowka_dekadowka.Items.Count>0)
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
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                sklad_sniadanie += lv_sniadanie.Items[i].SubItems[0].Text + "|" + lv_sniadanie.Items[i].SubItems[1].Text + "|" + lv_sniadanie.Items[i].SubItems[2].Text + "|" + lv_sniadanie.Items[i].SubItems[3].Text + "|" + lv_sniadanie.Items[i].SubItems[4].Text + "|" + lv_sniadanie.Items[i].SubItems[5].Text + "|" + lv_sniadanie.Items[i].SubItems[6].Text + "|" + lv_sniadanie.Items[i].SubItems[7].Text + "|" + lv_sniadanie.Items[i].SubItems[8].Text + "|" + lv_sniadanie.Items[i].SubItems[9].Text + "|" + lv_sniadanie.Items[i].SubItems[10].Text + "|" + lv_sniadanie.Items[i].SubItems[11].Text + "|" + lv_sniadanie.Items[i].SubItems[12].Text + "|" + lv_sniadanie.Items[i].SubItems[13].Text + "|" + lv_sniadanie.Items[i].SubItems[14].Text + "|" + lv_sniadanie.Items[i].SubItems[15].Text + "|" + lv_sniadanie.Items[i].SubItems[16].Text + "|" + lv_sniadanie.Items[i].SubItems[17].Text + "|" + lv_sniadanie.Items[i].SubItems[18].Text + "|" + lv_sniadanie.Items[i].SubItems[19].Text + "|" + lv_sniadanie.Items[i].SubItems[20].Text + "|" + lv_sniadanie.Items[i].SubItems[21].Text + "|" + lv_sniadanie.Items[i].SubItems[22].Text + "|" + lv_sniadanie.Items[i].SubItems[23].Text + "|" + lv_sniadanie.Items[i].SubItems[24].Text + "|" + lv_sniadanie.Items[i].SubItems[25].Text + "|" + lv_sniadanie.Items[i].SubItems[26].Text + "|" + lv_sniadanie.Items[i].SubItems[27].Text + "|" + lv_sniadanie.Items[i].SubItems[28].Text + "|" + lv_sniadanie.Items[i].SubItems[29].Text + "|" + lv_sniadanie.Items[i].SubItems[30].Text + "|" + lv_sniadanie.Items[i].SubItems[31].Text + "|" + lv_sniadanie.Items[i].SubItems[32].Text + "|" + lv_sniadanie.Items[i].SubItems[33].Text + "$";

            string sklad_IIsniadanie = "";
            for (int i = 0; i < lv_IIsniadanie.Items.Count; i++)
                sklad_IIsniadanie += lv_IIsniadanie.Items[i].SubItems[0].Text + "|" + lv_IIsniadanie.Items[i].SubItems[1].Text + "|" + lv_IIsniadanie.Items[i].SubItems[2].Text + "|" + lv_IIsniadanie.Items[i].SubItems[3].Text + "|" + lv_IIsniadanie.Items[i].SubItems[4].Text + "|" + lv_IIsniadanie.Items[i].SubItems[5].Text + "|" + lv_IIsniadanie.Items[i].SubItems[6].Text + "|" + lv_IIsniadanie.Items[i].SubItems[7].Text + "|" + lv_IIsniadanie.Items[i].SubItems[8].Text + "|" + lv_IIsniadanie.Items[i].SubItems[9].Text + "|" + lv_IIsniadanie.Items[i].SubItems[10].Text + "|" + lv_IIsniadanie.Items[i].SubItems[11].Text + "|" + lv_IIsniadanie.Items[i].SubItems[12].Text + "|" + lv_IIsniadanie.Items[i].SubItems[13].Text + "|" + lv_IIsniadanie.Items[i].SubItems[14].Text + "|" + lv_IIsniadanie.Items[i].SubItems[15].Text + "|" + lv_IIsniadanie.Items[i].SubItems[16].Text + "|" + lv_IIsniadanie.Items[i].SubItems[17].Text + "|" + lv_IIsniadanie.Items[i].SubItems[18].Text + "|" + lv_IIsniadanie.Items[i].SubItems[19].Text + "|" + lv_IIsniadanie.Items[i].SubItems[20].Text + "|" + lv_IIsniadanie.Items[i].SubItems[21].Text + "|" + lv_IIsniadanie.Items[i].SubItems[22].Text + "|" + lv_IIsniadanie.Items[i].SubItems[23].Text + "|" + lv_IIsniadanie.Items[i].SubItems[24].Text + "|" + lv_IIsniadanie.Items[i].SubItems[25].Text + "|" + lv_IIsniadanie.Items[i].SubItems[26].Text + "|" + lv_IIsniadanie.Items[i].SubItems[27].Text + "|" + lv_IIsniadanie.Items[i].SubItems[28].Text + "|" + lv_IIsniadanie.Items[i].SubItems[29].Text + "|" + lv_IIsniadanie.Items[i].SubItems[30].Text + "|" + lv_IIsniadanie.Items[i].SubItems[31].Text + "|" + lv_IIsniadanie.Items[i].SubItems[32].Text + "|" + lv_IIsniadanie.Items[i].SubItems[33].Text + "|" + "$";
          
            string sklad_obiad = "";
            for (int i = 0; i < lv_obiad.Items.Count; i++)
                sklad_obiad += lv_obiad.Items[i].SubItems[0].Text + "|" + lv_obiad.Items[i].SubItems[1].Text + "|" + lv_obiad.Items[i].SubItems[2].Text + "|" + lv_obiad.Items[i].SubItems[3].Text + "|" + lv_obiad.Items[i].SubItems[4].Text + "|" + lv_obiad.Items[i].SubItems[5].Text + "|" + lv_obiad.Items[i].SubItems[6].Text + "|" + lv_obiad.Items[i].SubItems[7].Text + "|" + lv_obiad.Items[i].SubItems[8].Text + "|" + lv_obiad.Items[i].SubItems[9].Text + "|" + lv_obiad.Items[i].SubItems[10].Text + "|" + lv_obiad.Items[i].SubItems[11].Text + "|" + lv_obiad.Items[i].SubItems[12].Text + "|" + lv_obiad.Items[i].SubItems[13].Text + "|" + lv_obiad.Items[i].SubItems[14].Text + "|" + lv_obiad.Items[i].SubItems[15].Text + "|" + lv_obiad.Items[i].SubItems[16].Text + "|" + lv_obiad.Items[i].SubItems[17].Text + "|" + lv_obiad.Items[i].SubItems[18].Text + "|" + lv_obiad.Items[i].SubItems[19].Text + "|" + lv_obiad.Items[i].SubItems[20].Text + "|" + lv_obiad.Items[i].SubItems[21].Text + "|" + lv_obiad.Items[i].SubItems[22].Text + "|" + lv_obiad.Items[i].SubItems[23].Text + "|" + lv_obiad.Items[i].SubItems[24].Text + "|" + lv_obiad.Items[i].SubItems[25].Text + "|" + lv_obiad.Items[i].SubItems[26].Text + "|" + lv_obiad.Items[i].SubItems[27].Text + "|" + lv_obiad.Items[i].SubItems[28].Text + "|" + lv_obiad.Items[i].SubItems[29].Text + "|" + lv_obiad.Items[i].SubItems[30].Text + "|" + lv_obiad.Items[i].SubItems[31].Text + "|" + lv_obiad.Items[i].SubItems[32].Text + "|" + lv_obiad.Items[i].SubItems[33].Text + "$";

            string sklad_podwieczorek = "";
            for (int i = 0; i < lv_podwieczorek.Items.Count; i++)
                sklad_podwieczorek += lv_podwieczorek.Items[i].SubItems[0].Text + "|" + lv_podwieczorek.Items[i].SubItems[1].Text + "|" + lv_podwieczorek.Items[i].SubItems[2].Text + "|" + lv_podwieczorek.Items[i].SubItems[3].Text + "|" + lv_podwieczorek.Items[i].SubItems[4].Text + "|" + lv_podwieczorek.Items[i].SubItems[5].Text + "|" + lv_podwieczorek.Items[i].SubItems[6].Text + "|" + lv_podwieczorek.Items[i].SubItems[7].Text + "|" + lv_podwieczorek.Items[i].SubItems[8].Text + "|" + lv_podwieczorek.Items[i].SubItems[9].Text + "|" + lv_podwieczorek.Items[i].SubItems[10].Text + "|" + lv_podwieczorek.Items[i].SubItems[11].Text + "|" + lv_podwieczorek.Items[i].SubItems[12].Text + "|" + lv_podwieczorek.Items[i].SubItems[13].Text + "|" + lv_podwieczorek.Items[i].SubItems[14].Text + "|" + lv_podwieczorek.Items[i].SubItems[15].Text + "|" + lv_podwieczorek.Items[i].SubItems[16].Text + "|" + lv_podwieczorek.Items[i].SubItems[17].Text + "|" + lv_podwieczorek.Items[i].SubItems[18].Text + "|" + lv_podwieczorek.Items[i].SubItems[19].Text + "|" + lv_podwieczorek.Items[i].SubItems[20].Text + "|" + lv_podwieczorek.Items[i].SubItems[21].Text + "|" + lv_podwieczorek.Items[i].SubItems[22].Text + "|" + lv_podwieczorek.Items[i].SubItems[23].Text + "|" + lv_podwieczorek.Items[i].SubItems[24].Text + "|" + lv_podwieczorek.Items[i].SubItems[25].Text + "|" + lv_podwieczorek.Items[i].SubItems[26].Text + "|" + lv_podwieczorek.Items[i].SubItems[27].Text + "|" + lv_podwieczorek.Items[i].SubItems[28].Text + "|" + lv_podwieczorek.Items[i].SubItems[29].Text + "|" + lv_podwieczorek.Items[i].SubItems[30].Text + "|" + lv_podwieczorek.Items[i].SubItems[31].Text + "|" + lv_podwieczorek.Items[i].SubItems[32].Text + "|" + lv_podwieczorek.Items[i].SubItems[33].Text + "$";

            string sklad_kolacja = "";
            for (int i = 0; i < lv_kolacja.Items.Count; i++)
                sklad_kolacja += lv_kolacja.Items[i].SubItems[0].Text + "|" + lv_kolacja.Items[i].SubItems[1].Text + "|" + lv_kolacja.Items[i].SubItems[2].Text + "|" + lv_kolacja.Items[i].SubItems[3].Text + "|" + lv_kolacja.Items[i].SubItems[4].Text + "|" + lv_kolacja.Items[i].SubItems[5].Text + "|" + lv_kolacja.Items[i].SubItems[6].Text + "|" + lv_kolacja.Items[i].SubItems[7].Text + "|" + lv_kolacja.Items[i].SubItems[8].Text + "|" + lv_kolacja.Items[i].SubItems[9].Text + "|" + lv_kolacja.Items[i].SubItems[10].Text + "|" + lv_kolacja.Items[i].SubItems[11].Text + "|" + lv_kolacja.Items[i].SubItems[12].Text + "|" + lv_kolacja.Items[i].SubItems[13].Text + "|" + lv_kolacja.Items[i].SubItems[14].Text + "|" + lv_kolacja.Items[i].SubItems[15].Text + "|" + lv_kolacja.Items[i].SubItems[16].Text + "|" + lv_kolacja.Items[i].SubItems[17].Text + "|" + lv_kolacja.Items[i].SubItems[18].Text + "|" + lv_kolacja.Items[i].SubItems[19].Text + "|" + lv_kolacja.Items[i].SubItems[20].Text + "|" + lv_kolacja.Items[i].SubItems[21].Text + "|" + lv_kolacja.Items[i].SubItems[22].Text + "|" + lv_kolacja.Items[i].SubItems[23].Text + "|" + lv_kolacja.Items[i].SubItems[24].Text + "|" + lv_kolacja.Items[i].SubItems[25].Text + "|" + lv_kolacja.Items[i].SubItems[26].Text + "|" + lv_kolacja.Items[i].SubItems[27].Text + "|" + lv_kolacja.Items[i].SubItems[28].Text + "|" + lv_kolacja.Items[i].SubItems[29].Text + "|" + lv_kolacja.Items[i].SubItems[30].Text + "|" + lv_kolacja.Items[i].SubItems[31].Text + "|" + lv_kolacja.Items[i].SubItems[32].Text + "|" + lv_kolacja.Items[i].SubItems[33].Text + "$";

            DAO.JadlospisDekadowkiDAO.InsertSQL(Convert.ToInt32(wybranaDekadowkaDoZapisania.id), dekadowka_zapisz_dzien.SelectedIndex + 1, DAO.DietaDAO.SelectSQL(dekadowka_zapisz_dieta.SelectedItem.ToString().Split('/')[0],dekadowka_zapisz_miasto.Text, dekadowka_zapisz_dieta.SelectedItem.ToString().Split('/')[1]), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, sklad_sniadanie, sklad_IIsniadanie, sklad_obiad, sklad_podwieczorek, sklad_kolacja);

            MessageBox.Show("Zapisano jadłospis szablonu","Zapisz");
            dekadowka_zapisz_wstec_Click(null, null);
        }

        private void dekadowka_zapisz_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {

            dekadowka_zapisz_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAllSQL(dekadowka_zapisz_miasto.Text);
            foreach (Dieta d in Diety)
                dekadowka_zapisz_dieta.Items.Add(d.nazwa+'/'+d.plec);
            if (dekadowka_zapisz_dieta.Items.Count > 0)
                dekadowka_zapisz_dieta.SelectedIndex = 0;

            dekadowka_zapisz_dekadowka.Items.Clear();
            listaDekadowekDoZapisania = DekadowkaDAO.SelectSQL(dekadowka_zapisz_miasto.SelectedItem.ToString());
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
            dieta_kod.Text = Diety[dieta_dieta.SelectedIndex].kod;
            dieta_plec.SelectedItem = Diety[dieta_dieta.SelectedIndex].plec;
            dieta_energia_od.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.energiaOd.ToString();
            dieta_tluszczeOd.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.tluszczeOd.ToString();
            dieta_bialkoOd.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.bialkoOd.ToString();
            dieta_weglowodanyOd.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyOd.ToString();
            dieta_energia_do.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.energiaDo.ToString();
            dieta_TluszczeDo.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.tluszczeDo.ToString();
            dieta_BialkoDo.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.bialkoDo.ToString();
            dieta_WeglowodanyDo.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.weglowodanyDo.ToString();
            dieta_blonnik.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.blonnik.ToString();
            dieta_przyswajalne.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.cukry.ToString();
            dieta_ktn.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.tluszcze_nn.ToString();
            dieta_sod.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.sod.ToString();
            dieta_witA.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witA.ToString();
            dieta_witB1.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witB1.ToString();
            dieta_witB2.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witB2.ToString();
            dieta_witB6.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witB6.ToString();
            dieta_witB12.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witB12.ToString();
            dieta_niacyna.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.niacyna.ToString();
            dieta_witC.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witC.ToString();
            dieta_witD.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witD.ToString();
            dieta_witE.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witE.ToString();
            dieta_witK.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.witK.ToString();
            dieta_foliany.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.foliany.ToString();
            dieta_fosfor.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.fosfor.ToString();
            dieta_magnez.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.magnez.ToString();
            dieta_zelazo.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.zelazo.ToString();
            dieta_cynk.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.cynk.ToString();
            dieta_selen.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.selen.ToString();
            dieta_miedz.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.miedz.ToString();
            dieta_cholina.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.cholina.ToString();
            dieta_kwasPantontenowy.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.kwasPantotenowy.ToString();
            dieta_biotyna.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.biotyna.ToString();
            dieta_mangan.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.mangan.ToString();
            dieta_jod.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.jod.ToString();
            dieta_potas.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.potas.ToString();
            dieta_fluor.Text = Diety[dieta_dieta.SelectedIndex].wartosciOdzywcze.fluor.ToString();
        }

        private void dieta_ok_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                case "Diety -> Dodaj":
                    if (dieta_nazwa.Text!=""&&dieta_miasto.Text!=""&& dieta_energia_od.Text!=""&& dieta_bialkoOd.Text!=""&& dieta_tluszczeOd.Text!=""&& dieta_weglowodanyOd.Text!=""&&dieta_sod.Text!=""&&dieta_ktn.Text!=""&&dieta_przyswajalne.Text!="" &&dieta_blonnik.Text!="")
                    {
                        try
                        {
                            DAO.DietaDAO.InsertSQL(dieta_nazwa.Text, dieta_miasto.Text, dieta_kod.Text, dieta_plec.SelectedItem.ToString(), new WartosciOdzywcze(Convert.ToDouble(dieta_energia_od.Text), Convert.ToDouble(dieta_energia_do.Text), Convert.ToDouble(dieta_bialkoOd.Text), Convert.ToDouble(dieta_BialkoDo.Text), Convert.ToDouble(dieta_tluszczeOd.Text), Convert.ToDouble(dieta_TluszczeDo.Text), Convert.ToDouble(dieta_ktn.Text), Convert.ToDouble(dieta_weglowodanyOd.Text), Convert.ToDouble(dieta_WeglowodanyDo.Text), Convert.ToDouble(dieta_przyswajalne.Text), Convert.ToDouble(dieta_blonnik.Text), Convert.ToDouble(dieta_sod.Text), Convert.ToDouble(dieta_witA.Text), Convert.ToDouble(dieta_witB1.Text), Convert.ToDouble(dieta_witB2.Text), Convert.ToDouble(dieta_witB6.Text), Convert.ToDouble(dieta_witB12.Text), Convert.ToDouble(dieta_niacyna.Text), Convert.ToDouble(dieta_witC.Text), Convert.ToDouble(dieta_witD.Text), Convert.ToDouble(dieta_witE.Text), Convert.ToDouble(dieta_witK.Text), Convert.ToDouble(dieta_foliany.Text), Convert.ToDouble(dieta_fosfor.Text), Convert.ToDouble(dieta_magnez.Text), Convert.ToDouble(dieta_zelazo.Text), Convert.ToDouble(dieta_cynk.Text), Convert.ToDouble(dieta_jod.Text), Convert.ToDouble(dieta_selen.Text), Convert.ToDouble(dieta_miedz.Text), Convert.ToDouble(dieta_cholina.Text), Convert.ToDouble(dieta_kwasPantontenowy.Text), Convert.ToDouble(dieta_biotyna.Text), Convert.ToDouble(dieta_mangan.Text), Convert.ToDouble(dieta_fluor.Text), Convert.ToDouble(dieta_potas.Text)));
                            MessageBox.Show("Dodano: " + dieta_nazwa.Text);
                            dietaClick();
                        }
                        catch {
                            MessageBox.Show("Błąd dodawania diety","Błąd");

                        }
                    }
                    else
                        MessageBox.Show("Nie uzupełniono wszystkich danych","Błąd");
                    break;
                case "Diety -> Edytuj":
                    if (dieta_nazwa.Text != "" && dieta_miasto.Text != "" && dieta_energia_od.Text != "" && dieta_bialkoOd.Text != "" && dieta_tluszczeOd.Text != "" && dieta_weglowodanyOd.Text != "" && dieta_sod.Text != "" && dieta_ktn.Text != "" && dieta_przyswajalne.Text != "" && dieta_blonnik.Text != "")
                    {
                        try
                        {
                            DAO.DietaDAO.UpdateSQL(Diety[dieta_dieta.SelectedIndex], dieta_nazwa.Text, dieta_miasto.Text, dieta_kod.Text, dieta_plec.SelectedItem.ToString(), new WartosciOdzywcze(Convert.ToDouble(dieta_energia_od.Text), Convert.ToDouble(dieta_energia_do.Text), Convert.ToDouble(dieta_bialkoOd.Text), Convert.ToDouble(dieta_BialkoDo.Text), Convert.ToDouble(dieta_tluszczeOd.Text), Convert.ToDouble(dieta_TluszczeDo.Text), Convert.ToDouble(dieta_ktn.Text), Convert.ToDouble(dieta_weglowodanyOd.Text), Convert.ToDouble(dieta_WeglowodanyDo.Text), Convert.ToDouble(dieta_przyswajalne.Text), Convert.ToDouble(dieta_blonnik.Text), Convert.ToDouble(dieta_sod.Text), Convert.ToDouble(dieta_witA.Text), Convert.ToDouble(dieta_witB1.Text), Convert.ToDouble(dieta_witB2.Text), Convert.ToDouble(dieta_witB6.Text), Convert.ToDouble(dieta_witB12.Text), Convert.ToDouble(dieta_niacyna.Text), Convert.ToDouble(dieta_witC.Text), Convert.ToDouble(dieta_witD.Text), Convert.ToDouble(dieta_witE.Text), Convert.ToDouble(dieta_witK.Text), Convert.ToDouble(dieta_foliany.Text), Convert.ToDouble(dieta_fosfor.Text), Convert.ToDouble(dieta_magnez.Text), Convert.ToDouble(dieta_zelazo.Text), Convert.ToDouble(dieta_cynk.Text), Convert.ToDouble(dieta_jod.Text), Convert.ToDouble(dieta_selen.Text), Convert.ToDouble(dieta_miedz.Text), Convert.ToDouble(dieta_cholina.Text), Convert.ToDouble(dieta_kwasPantontenowy.Text), Convert.ToDouble(dieta_biotyna.Text), Convert.ToDouble(dieta_mangan.Text), Convert.ToDouble(dieta_fluor.Text), Convert.ToDouble(dieta_potas.Text)));
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
            dieta_kod.Enabled = false;
            dieta_kod.BackColor = Color.FromName("ControlLight");
            dieta_plec.Enabled = false;
            dieta_plec.BackColor = Color.FromName("ControlLight");
            dieta_energia_od.Enabled = false;
            dieta_energia_od.BackColor = Color.FromName("ControlLight");
            dieta_bialkoOd.Enabled = false;
            dieta_bialkoOd.BackColor = Color.FromName("ControlLight");
            dieta_tluszczeOd.Enabled = false;
            dieta_tluszczeOd.BackColor = Color.FromName("ControlLight");
            dieta_weglowodanyOd.Enabled = false;
            dieta_weglowodanyOd.BackColor = Color.FromName("ControlLight");
            dieta_energia_do.Enabled = false;
            dieta_energia_do.BackColor = Color.FromName("ControlLight");
            dieta_BialkoDo.Enabled = false;
            dieta_BialkoDo.BackColor = Color.FromName("ControlLight");
            dieta_TluszczeDo.Enabled = false;
            dieta_TluszczeDo.BackColor = Color.FromName("ControlLight");
            dieta_WeglowodanyDo.Enabled = false;
            dieta_WeglowodanyDo.BackColor = Color.FromName("ControlLight");
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
            dieta_jod.Enabled = false;
            dieta_jod.BackColor = Color.FromName("ControlLight");
            dieta_witA.Enabled = false;
            dieta_witA.BackColor = Color.FromName("ControlLight");
            dieta_witB1.Enabled = false;
            dieta_witB1.BackColor = Color.FromName("ControlLight");
            dieta_witB2.Enabled = false;
            dieta_witB2.BackColor = Color.FromName("ControlLight");
            dieta_niacyna.Enabled = false;
            dieta_niacyna.BackColor = Color.FromName("ControlLight");
            dieta_witB6.Enabled = false;
            dieta_witB6.BackColor = Color.FromName("ControlLight");
            dieta_witB12.Enabled = false;
            dieta_witB12.BackColor = Color.FromName("ControlLight");
            dieta_witC.Enabled = false;
            dieta_witC.BackColor = Color.FromName("ControlLight");
            dieta_foliany.Enabled = false;
            dieta_foliany.BackColor = Color.FromName("ControlLight");
            dieta_fosfor.Enabled = false;
            dieta_fosfor.BackColor = Color.FromName("ControlLight");
            dieta_magnez.Enabled = false;
            dieta_magnez.BackColor = Color.FromName("ControlLight");
            dieta_zelazo.Enabled = false;
            dieta_zelazo.BackColor = Color.FromName("ControlLight");
            dieta_cynk.Enabled = false;
            dieta_cynk.BackColor = Color.FromName("ControlLight");
            dieta_selen.Enabled = false;
            dieta_selen.BackColor = Color.FromName("ControlLight");
            dieta_miedz.Enabled = false;
            dieta_miedz.BackColor = Color.FromName("ControlLight");
            dieta_cholina.Enabled = false;
            dieta_cholina.BackColor = Color.FromName("ControlLight");
            dieta_kwasPantontenowy.Enabled = false;
            dieta_kwasPantontenowy.BackColor = Color.FromName("ControlLight");
            dieta_biotyna.Enabled = false;
            dieta_biotyna.BackColor = Color.FromName("ControlLight");
            dieta_witD.Enabled = false;
            dieta_witD.BackColor = Color.FromName("ControlLight");
            dieta_witE.Enabled = false;
            dieta_witE.BackColor = Color.FromName("ControlLight");
            dieta_witK.Enabled = false;
            dieta_witK.BackColor = Color.FromName("ControlLight");
            dieta_mangan.Enabled = false;
            dieta_mangan.BackColor = Color.FromName("ControlLight");
            dieta_fluor.Enabled = false;
            dieta_fluor.BackColor = Color.FromName("ControlLight");
            dieta_potas.Enabled = false;
            dieta_potas.BackColor = Color.FromName("ControlLight");
            dieta_kod.Enabled = false;
            dieta_kod.BackColor = Color.FromName("ControlLight");
            dieta_plec.Enabled = false;
            dieta_plec.BackColor = Color.FromName("ControlLight");

            label10.Text = "Diety";

            dieta_miasto.Items.Clear();
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                    DAO.DietaDAO.DeleteSQL(Diety[dieta_dieta.SelectedIndex]);
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
            dieta_energia_od.Enabled = true;
            dieta_energia_od.BackColor = Color.White;
            dieta_energia_do.Enabled = true;
            dieta_energia_do.BackColor = Color.White;
            dieta_bialkoOd.Enabled = true;
            dieta_bialkoOd.BackColor = Color.White;
            dieta_BialkoDo.Enabled = true;
            dieta_BialkoDo.BackColor = Color.White;
            dieta_tluszczeOd.Enabled = true;
            dieta_tluszczeOd.BackColor = Color.White;
            dieta_TluszczeDo.Enabled = true;
            dieta_TluszczeDo.BackColor = Color.White;
            dieta_weglowodanyOd.Enabled = true;
            dieta_weglowodanyOd.BackColor = Color.White;
            dieta_WeglowodanyDo.Enabled = true;
            dieta_WeglowodanyDo.BackColor = Color.White;
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
            dieta_jod.Enabled = true;
            dieta_jod.BackColor = Color.White;
            dieta_witA.Enabled = true;
            dieta_witA.BackColor = Color.White;
            dieta_witB1.Enabled = true;
            dieta_witB1.BackColor = Color.White;
            dieta_witB2.Enabled = true;
            dieta_witB2.BackColor = Color.White;
            dieta_niacyna.Enabled = true;
            dieta_niacyna.BackColor = Color.White;
            dieta_witB6.Enabled = true;
            dieta_witB6.BackColor = Color.White;
            dieta_witB12.Enabled = true;
            dieta_witB12.BackColor = Color.White;
            dieta_witC.Enabled = true;
            dieta_witC.BackColor = Color.White;
            dieta_foliany.Enabled = true;
            dieta_foliany.BackColor = Color.White;
            dieta_fosfor.Enabled = true;
            dieta_fosfor.BackColor = Color.White;
            dieta_magnez.Enabled = true;
            dieta_magnez.BackColor = Color.White;
            dieta_zelazo.Enabled = true;
            dieta_zelazo.BackColor = Color.White;
            dieta_cynk.Enabled = true;
            dieta_cynk.BackColor = Color.White;
            dieta_selen.Enabled = true;
            dieta_selen.BackColor = Color.White;
            dieta_miedz.Enabled = true;
            dieta_miedz.BackColor = Color.White;
            dieta_cholina.Enabled = true;
            dieta_cholina.BackColor = Color.White;
            dieta_kwasPantontenowy.Enabled = true;
            dieta_kwasPantontenowy.BackColor = Color.White;
            dieta_biotyna.Enabled = true;
            dieta_biotyna.BackColor = Color.White;
            dieta_witD.Enabled = true;
            dieta_witD.BackColor = Color.White;
            dieta_witE.Enabled = true;
            dieta_witE.BackColor = Color.White;
            dieta_witK.Enabled = true;
            dieta_witK.BackColor = Color.White;
            dieta_mangan.Enabled = true;
            dieta_mangan.BackColor = Color.White;
            dieta_fluor.Enabled = true;
            dieta_fluor.BackColor = Color.White;
            dieta_potas.Enabled = true;
            dieta_potas.BackColor = Color.White;
            dieta_kod.Enabled = true;
            dieta_kod.BackColor = Color.White;
            dieta_plec.Enabled = true;
            dieta_plec.BackColor = Color.White;
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
            dieta_energia_od.Enabled = true;
            dieta_energia_od.BackColor = Color.White;
            dieta_energia_do.Enabled = true;
            dieta_energia_do.BackColor = Color.White;
            dieta_bialkoOd.Enabled = true;
            dieta_bialkoOd.BackColor = Color.White;
            dieta_BialkoDo.Enabled = true;
            dieta_BialkoDo.BackColor = Color.White;
            dieta_tluszczeOd.Enabled = true;
            dieta_tluszczeOd.BackColor = Color.White;
            dieta_TluszczeDo.Enabled = true;
            dieta_TluszczeDo.BackColor = Color.White;
            dieta_weglowodanyOd.Enabled = true;
            dieta_weglowodanyOd.BackColor = Color.White;
            dieta_WeglowodanyDo.Enabled = true;
            dieta_WeglowodanyDo.BackColor = Color.White;
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
            dieta_jod.Enabled = true;
            dieta_jod.BackColor = Color.White;
            dieta_witA.Enabled = true;
            dieta_witA.BackColor = Color.White;
            dieta_witB1.Enabled = true;
            dieta_witB1.BackColor = Color.White;
            dieta_witB2.Enabled = true;
            dieta_witB2.BackColor = Color.White;
            dieta_niacyna.Enabled = true;
            dieta_niacyna.BackColor = Color.White;
            dieta_witB6.Enabled = true;
            dieta_witB6.BackColor = Color.White;
            dieta_witB12.Enabled = true;
            dieta_witB12.BackColor = Color.White;
            dieta_witC.Enabled = true;
            dieta_witC.BackColor = Color.White;
            dieta_foliany.Enabled = true;
            dieta_foliany.BackColor = Color.White;
            dieta_fosfor.Enabled = true;
            dieta_fosfor.BackColor = Color.White;
            dieta_magnez.Enabled = true;
            dieta_magnez.BackColor = Color.White;
            dieta_zelazo.Enabled = true;
            dieta_zelazo.BackColor = Color.White;
            dieta_cynk.Enabled = true;
            dieta_cynk.BackColor = Color.White;
            dieta_selen.Enabled = true;
            dieta_selen.BackColor = Color.White;
            dieta_miedz.Enabled = true;
            dieta_miedz.BackColor = Color.White;
            dieta_cholina.Enabled = true;
            dieta_cholina.BackColor = Color.White;
            dieta_kwasPantontenowy.Enabled = true;
            dieta_kwasPantontenowy.BackColor = Color.White;
            dieta_biotyna.Enabled = true;
            dieta_biotyna.BackColor = Color.White;
            dieta_witD.Enabled = true;
            dieta_witD.BackColor = Color.White;
            dieta_witE.Enabled = true;
            dieta_witE.BackColor = Color.White;
            dieta_witK.Enabled = true;
            dieta_witK.BackColor = Color.White;
            dieta_mangan.Enabled = true;
            dieta_mangan.BackColor = Color.White;
            dieta_fluor.Enabled = true;
            dieta_fluor.BackColor = Color.White;
            dieta_potas.Enabled = true;
            dieta_potas.BackColor = Color.White;
            dieta_kod.Enabled = true;
            dieta_kod.BackColor = Color.White;
            dieta_plec.Enabled = true;
            dieta_plec.BackColor = Color.White;

            dieta_nazwa.Text = "";
            dieta_kod.Text = "";
            dieta_plec.SelectedIndex = -1;
            dieta_energia_od.Text = "";
            dieta_energia_do.Text = "";
            dieta_bialkoOd.Text = "";
            dieta_weglowodanyOd.Text = "";
            dieta_tluszczeOd.Text = "";
            dieta_BialkoDo.Text = "";
            dieta_WeglowodanyDo.Text = "";
            dieta_TluszczeDo.Text = "";
            dieta_jod.Text = "";
            dieta_sod.Text = "";
            dieta_ktn.Text = "";
            dieta_sol.Text = "";
            dieta_przyswajalne.Text = "";
            dieta_blonnik.Text = "";
            dieta_witA.Text = "";
            dieta_witB1.Text = "";
            dieta_witB2.Text = "";
            dieta_witC.Text = "";
            dieta_niacyna.Text = "";
            dieta_witB6.Text = "";
            dieta_witB12.Text = "";
            dieta_witB12.Text = "";
            dieta_foliany.Text = "";
            dieta_fosfor.Text = "";
            dieta_fluor.Text = "";
            dieta_magnez.Text = "";
            dieta_zelazo.Text = "";
            dieta_cynk.Text = "";
            dieta_selen.Text = "";
            dieta_miedz.Text = "";
            dieta_cholina.Text = "";
            dieta_kwasPantontenowy.Text = "";
            dieta_biotyna.Text = "";
            dieta_witD.Text = "";
            dieta_witE.Text = "";
            dieta_witK.Text = "";
            dieta_mangan.Text = "";
            dieta_fluor.Text = "";
            dieta_potas.Text = "";
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
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
            foreach (DAO.Jednostka j in listaJednostek)
                jednostka_jednostka.Items.Add(j.miasto);

            if(jednostka_jednostka.Items.Count>0)
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
                    DAO.JednostkaDAO.DeleteSQL(listaJednostek[jednostka_jednostka.SelectedIndex]);
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
                        DAO.JednostkaDAO.InsertSQL(jednostka_miasto.Text);
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
                        DAO.JednostkaDAO.UpdateSQL(listaJednostek[jednostka_jednostka.SelectedIndex], jednostka_miasto.Text);
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
                    DekadowkaDAO.DeleteSQL(listaDekadowek[dekadowka_dekadowka.SelectedIndex]);
                    MessageBox.Show("Usunięto szablon: " + listaDekadowek[dekadowka_dekadowka.SelectedIndex].nazwa+" z: "+ listaDekadowek[dekadowka_dekadowka.SelectedIndex].miasto);
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
                    if (dekadowka_dodaj_nazwa.Text!=""&& dekadowka_dodaj_dni.Text!="")
                    {
                        try
                        {
                            DekadowkaDAO.InsertSQL(dekadowka_dodaj_nazwa.Text, dekadowka_dodaj_miasto.Text, Convert.ToInt32(dekadowka_dodaj_dni.Text), dekadowka_dodaj_dzienStart.SelectedItem.ToString(), null);
                            MessageBox.Show("Dodano szablon: " + dekadowka_dodaj_nazwa.Text + " w: " + dekadowka_dodaj_miasto.Text, "Dodawanie szablonu");
                            dekadowkaClick();
                        }
                        catch {
                            MessageBox.Show("Błąd dodawania szablonu","Błąd");

                        }

                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono wszystkich danych","Błąd");
                    }
                    break;
                case "Szablony -> Generuj jadłospisy":
                    List<string> daty = new List<string>();
                    int dni = (Convert.ToDateTime(dekadowka_generuj_data2.Text) - Convert.ToDateTime(dekadowka_generuj_data1.Text)).Days + 1;
                    if (dni == wybranaDekadowka.dni) { 
                        DateTime data = Convert.ToDateTime(dekadowka_generuj_data1.Text);
                    for (int i = 0; i < dni; i++)
                    {
                        string aktualna_data = data.Day+" "+GetMonthForDate(data.Month)+" "+ data.Year;
                        List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDaySQL(Convert.ToInt32(wybranaDekadowka.id),i+ 1);
                        foreach (Jadlospis jadlospis in jadlospisyDanegoDnia)
                        {
                            if(jadlospis.dzien == i+1)
                                DAO.JadlospisDAO.InsertSQL(aktualna_data, jadlospis.dieta.nazwa, wybranaDekadowka.miasto, jadlospis.dieta.plec, jadlospis.nazwa_sniadanie, jadlospis.nazwa_IIsniadanie, jadlospis.nazwa_obiad, jadlospis.nazwa_podwieczorek, jadlospis.nazwa_kolacja, jadlospis.sklad_sniadanie, jadlospis.sklad_IIsniadanie, jadlospis.sklad_obiad, jadlospis.sklad_podwieczorek, jadlospis.sklad_kolacja);
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

        List<Receptura> listaReceptur;
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            switch (label10.Text)
            {
                default:
                    label10.Text = "Receptury";

                    receptura_wczytaj.Items.Clear();
                    listaReceptur = DAO.RecepturaDAO.SelectAllSQL();
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
                string[] arg = new string[10];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                if (arr.Length != 10)
                {
                    arg[0] = arr[0];
                    arg[1] = arr[1];
                    arg[2] = arr[2];
                    arg[3] = arr[3];
                    arg[4] = arr[4];
                    arg[5] = arr[7];
                    arg[6] = arr[5];
                    arg[7] = "0";
                    arg[8] = "0";
                    arg[9] = arr[6];
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
                    DAO.RecepturaDAO.DeleteSQL(listaReceptur[receptura_wczytaj.SelectedIndex]);
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
            label61.Location = new Point(40+185, label61.Location.Y);
            receptura_wczytaj.Location = new Point(210+185, receptura_wczytaj.Location.Y);

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

        private void receptura_sklad_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            double[] suma_receptura = new double[8];
            
            for (int i = 0; i < 8; i++)
            {
                suma_receptura[i] = 0;
            }

            for (int k = 0; k < 8; k++)
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
            receptura_sod.Text = Math.Round(suma_receptura[7], 2).ToString() + " mg";
            receptura_przyswajalne.Text = Math.Round(suma_receptura[5], 2).ToString() + " g";
            receptura_blonnik.Text = Math.Round(suma_receptura[6], 2).ToString() + " mg";
            receptura_ktn.Text = Math.Round(suma_receptura[3], 2).ToString() + " g";
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
            if(receptura_produkty.SelectedIndex!=-1)
            { 
               if (receptura_masa.Text != "")
                {
                try
            {
                    double masa = Math.Round(double.Parse(receptura_masa.Text),2);
                    int ktory = receptura_produkty.SelectedIndex;
                    string[] arr = new string[10];
                    ListViewItem itm;

                    switch (kategoria)
                    {
                        case "Wszystkie":
                            arr[0] = Lista[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Lista[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Lista[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Lista[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Lista[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Lista[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Lista[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Lista[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();

                            break;
                        case "M":
                            arr[0] = Mieso[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Mieso[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Mieso[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Mieso[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Mieso[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Mieso[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Mieso[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Mieso[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Mieso[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "W":
                            arr[0] = Warzywa[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Warzywa[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Warzywa[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Warzywa[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Warzywa[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Warzywa[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Warzywa[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Warzywa[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Warzywa[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "O":
                            arr[0] = Owoce[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Owoce[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Owoce[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Owoce[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Owoce[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Owoce[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Owoce[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Owoce[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Owoce[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "S":
                            arr[0] = Slodycze[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Slodycze[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Slodycze[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Slodycze[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Slodycze[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Slodycze[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Slodycze[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Slodycze[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Slodycze[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "R":
                            arr[0] = Ryby[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Ryby[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Ryby[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Ryby[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Ryby[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Ryby[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Ryby[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Ryby[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Ryby[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "D":
                            arr[0] = Napoje[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Napoje[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Napoje[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Napoje[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Napoje[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Napoje[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Napoje[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Napoje[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Napoje[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "Z":
                            arr[0] = Zboza[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Zboza[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Zboza[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Zboza[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Zboza[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Zboza[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Zboza[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Zboza[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Zboza[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "P":
                            arr[0] = Przyprawy[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Przyprawy[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "N":
                            arr[0] = Nabial[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Nabial[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Nabial[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Nabial[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Nabial[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Nabial[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Nabial[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Nabial[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Nabial[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "B":
                            arr[0] = Bakalie[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Bakalie[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Bakalie[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Bakalie[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Bakalie[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Bakalie[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Bakalie[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Bakalie[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Bakalie[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                        case "T":
                            arr[0] = Tluszcze[ktory].nazwa;
                            arr[1] = masa.ToString();
                            arr[2] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.energia * masa / 100.0, 2).ToString();
                            arr[3] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.bialko * masa / 100.0, 2).ToString();
                            arr[4] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.tluszcze * masa / 100.0, 2).ToString();
                            arr[5] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.tluszcze_nn * masa / 100.0, 2).ToString();
                            arr[6] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.weglowodany * masa / 100.0, 2).ToString();
                            arr[7] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.weglowodany_przyswajalne * masa / 100.0, 2).ToString();
                            arr[8] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.blonnik * masa / 100.0, 2).ToString();
                            arr[9] = Math.Round(Tluszcze[ktory].wartosciOdzywcze.sod * masa / 100.0, 2).ToString();
                            break;
                    }

                    itm = new ListViewItem(arr);

                    receptura_sklad.Items.Add(itm);

                    } catch
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
                        string[] arr = new string[10];
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
                ListViewItem itm = new ListViewItem(arr);

                        receptura_sklad.Items.Remove(receptura_sklad.Items[wybrany]);
                        receptura_sklad.Items.Insert(wybrany, itm);
                        LiczSredniaDlaReceptur();
                     
            }
            catch
            {

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
                if (wybrany < liczba-1)
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
            string sklad="";
            for (int i = 0; i < receptura_sklad.Items.Count; i++)
                sklad += receptura_sklad.Items[i].SubItems[0].Text + "|" + receptura_sklad.Items[i].SubItems[1].Text + "|" + receptura_sklad.Items[i].SubItems[2].Text + "|" + receptura_sklad.Items[i].SubItems[3].Text + "|" + receptura_sklad.Items[i].SubItems[4].Text + "|" + receptura_sklad.Items[i].SubItems[5].Text + "|" + receptura_sklad.Items[i].SubItems[6].Text + "|" + receptura_sklad.Items[i].SubItems[7].Text + "|" + receptura_sklad.Items[i].SubItems[8].Text + "|" + receptura_sklad.Items[i].SubItems[9].Text + "$";

            switch (label10.Text)
            {
                case "Receptury -> Dodaj":
                    if (sklad != "" && receptura_nazwa.Text != "")
                    {
                        DAO.RecepturaDAO.InsertSQL(receptura_nazwa.Text, sklad);
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
                        DAO.RecepturaDAO.UpdateSQL(listaReceptur[receptura_wczytaj.SelectedIndex], receptura_nazwa.Text,sklad);
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
                    for (int i = 0; i < receptura_sklad.Items.Count; i++) {
                        string[] arr = new string[10];
                        ListViewItem itm = null;
                        if (receptura_sklad.Items[i].SubItems.Count != 10)
                        {
                            arr[0] = receptura_sklad.Items[i].SubItems[0].Text;
                            arr[1] = receptura_sklad.Items[i].SubItems[1].Text;
                            arr[2] = receptura_sklad.Items[i].SubItems[2].Text;
                            arr[3] = receptura_sklad.Items[i].SubItems[3].Text;
                            arr[4] = receptura_sklad.Items[i].SubItems[4].Text;
                            arr[5] = receptura_sklad.Items[i].SubItems[7].Text;
                            arr[6] = receptura_sklad.Items[i].SubItems[5].Text;
                            arr[7] = "0";
                            arr[8] = "0";
                            arr[9] = receptura_sklad.Items[i].SubItems[6].Text;
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
                            textBox1.Text += " "+receptura_nazwa.Text;
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
            listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
            foreach (DAO.Jednostka j in listaJednostek)
                dekadowka_wczytaj_miasto.Items.Add(j.miasto);
            dekadowka_wczytaj_miasto.SelectedIndex = 0;

            dekadowka_wczytaj_dekadowka.Items.Clear();
            listaDekadowekDoWczytania = DekadowkaDAO.SelectSQL(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;


        }
        List<Dekadowka> listaDekadowekDoWczytania;
        Dekadowka wybranaDekadowkaDoWczytania;
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
            listaDekadowekDoWczytania = DekadowkaDAO.SelectSQL(dekadowka_wczytaj_miasto.SelectedItem.ToString());
            foreach (Dekadowka d in listaDekadowekDoWczytania)
                dekadowka_wczytaj_dekadowka.Items.Add(d.nazwa);
            if (dekadowka_wczytaj_dekadowka.Items.Count > 0)
                dekadowka_wczytaj_dekadowka.SelectedIndex = 0;
        }
        Jadlospis jadlospisDekadowkiDoWczytania;
        private void dekadowka_wczytaj_dzien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wybranaDekadowkaDoWczytania != null)
            {
                dekadowka_wczytaj_dieta.Items.Clear();
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDaySQL(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), dekadowka_wczytaj_dzien.SelectedIndex + 1);
                foreach (Jadlospis d in jadlospisyDanegoDnia)
                {
                    if (d.dzien - 1 == dekadowka_wczytaj_dzien.SelectedIndex)
                    {
                        dekadowka_wczytaj_dieta.Items.Add(d.dieta.nazwa+'/'+d.dieta.plec);
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
            flowLayoutPanel1.Size = new System.Drawing.Size(dekadowkaSize[0], dekadowkaSize[1]-75);

            for (int j = 0; j < wybranaDekadowkaDoWczytania.dni; j++)
            {
                FlowLayoutPanel dayOfWeek = new FlowLayoutPanel();
                dayOfWeek.BackColor = Color.White;
                dayOfWeek.AutoScroll = true;
                dayOfWeek.FlowDirection = FlowDirection.TopDown;
                dayOfWeek.VerticalScroll.Visible = false;
                dayOfWeek.HorizontalScroll.Visible = false;
                dayOfWeek.WrapContents = false; // Vertical rather than horizontal scrolling
                dayOfWeek.Size = new System.Drawing.Size(dzienSize[0], dzienSize[1]-60);

                Label myDay = new Label();
                string day = GetDay(wybranaDekadowkaDoWczytania.dzienStart, j + 1);
                myDay.Text = day;
                myDay.MaximumSize = new Size(dzienSize[0], 0);
                myDay.AutoSize = true;
                dayOfWeek.Controls.Add(myDay);

                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDaySQL(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), j + 1);
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
                string[] arg = new string[34];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                itm = new ListViewItem(arr);
                lv_sniadanie.Items.Add(itm);              
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_IIsniadanie.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[34];
               string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                    itm = new ListViewItem(arr);
                lv_IIsniadanie.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_obiad.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[34];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                    itm = new ListViewItem(arr);
                lv_obiad.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_podwieczorek.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[34];
               string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                    itm = new ListViewItem(arr);
                lv_podwieczorek.Items.Add(itm);
            }
            produkty = jadlospisDekadowkiDoWczytania.sklad_kolacja.Split('$');
            for (int j = 0; j < produkty.Length - 1; j++)
            {
                string[] arg = new string[34];
                string[] arr = produkty[j].Split('|');
                ListViewItem itm = null;
                    itm = new ListViewItem(arr);
                lv_kolacja.Items.Add(itm);
            }

            cb_miasto.SelectedItem = jadlospisDekadowkiDoWczytania.dieta.miasto;
            cb_dieta.SelectedItem = jadlospisDekadowkiDoWczytania.dieta.nazwa + '/' + jadlospisDekadowkiDoWczytania.dieta.plec;

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
            string dieta = jadlospis_dieta.SelectedItem.ToString().Split('/')[0];
            string data = ja.Text;

            Jadlospis jadlospis = DAO.JadlospisDAO.SelectAllSQL(data, miasto, dieta, jadlospis_dieta.SelectedItem.ToString().Split('/')[1]);
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
                    string[] arg = new string[10];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                        itm = new ListViewItem(arr);
                    lv_sniadanie.Items.Add(itm);
                }
                produkty = jadlospis.sklad_IIsniadanie.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[10];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                        itm = new ListViewItem(arr);
                    lv_IIsniadanie.Items.Add(itm);
                }
                produkty = jadlospis.sklad_obiad.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[10];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                        itm = new ListViewItem(arr);
                    lv_obiad.Items.Add(itm);
                }
                produkty = jadlospis.sklad_podwieczorek.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[10];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                        itm = new ListViewItem(arr);
                    lv_podwieczorek.Items.Add(itm);
                }
                produkty = jadlospis.sklad_kolacja.Split('$');
                for (int j = 0; j < produkty.Length - 1; j++)
                {
                    string[] arg = new string[10];
                    string[] arr = produkty[j].Split('|');
                    ListViewItem itm = null;
                        itm = new ListViewItem(arr);
                    lv_kolacja.Items.Add(itm);
                }

                cb_miasto.SelectedItem = jadlospis.miasto;
                cb_dieta.SelectedItem = jadlospis.dieta.nazwa + '/' + jadlospis.dieta.plec;
            }
            LiczSrednia();
        }

        public void wczytajJadlospis()
        {           
            jadlospis_miasto.Items.Clear();
            listaJednostek= DAO.JednostkaDAO.SelectAllSQL();
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
                string dieta = jadlospis_dieta.SelectedItem.ToString().Split('/')[0];
                string data = ja.Text;

                Jadlospis jadlospis = DAO.JadlospisDAO.SelectAllSQL(data, miasto, dieta, jadlospis_dieta.SelectedItem.ToString().Split('/')[1]);

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
                        string[] arg = new string[10];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                            itm = new ListViewItem(arr);
                        listView1.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_IIsniadanie.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[10];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                            itm = new ListViewItem(arr);
                        listView2.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_obiad.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[10];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                            itm = new ListViewItem(arr);
                        listView3.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_podwieczorek.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[10];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                            itm = new ListViewItem(arr);
                        listView4.Items.Add(itm);
                    }
                    produkty = jadlospis.sklad_kolacja.Split('$');
                    for (int j = 0; j < produkty.Length - 1; j++)
                    {
                        string[] arg = new string[10];
                        string[] arr = produkty[j].Split('|');
                        ListViewItem itm = null;
                            itm = new ListViewItem(arr);
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
            double[,] suma_jad = new double[6, 8];
            double[,] proc_jad = new double[6, 8];

            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma_jad[i, k] = 0;
                    proc_jad[i, k] = 0;
                }

            }

            string[] arr = new string[8];
            for (int i = 0; i < 8; i++)
                arr[i] = "0";
            ListViewItem itm = new ListViewItem(arr);
            itm.UseItemStyleForSubItems = false;

            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    double a = double.Parse(listView1.Items[i].SubItems[k + 2].Text);
                    suma_jad[0, k] += a;
                }
            }

            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    double a = double.Parse(listView2.Items[i].SubItems[k + 2].Text);
                    suma_jad[1, k] += a;
                }
            }

            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    double a = double.Parse(listView3.Items[i].SubItems[k + 2].Text);
                    suma_jad[2, k] += a;
                }
            }
            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    double a = double.Parse(listView4.Items[i].SubItems[k + 2].Text);
                    suma_jad[3, k] += a;
                }
            }


            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < listView5.Items.Count; i++)
                {
                    double a = double.Parse(listView5.Items[i].SubItems[k + 2].Text);
                    suma_jad[4, k] += a;
                }
            }

            for (int k = 0; k < 8; k++)
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
            jadlospis_weglowodany.Text = Math.Round(suma_jad[5, 4], 2).ToString() + " mg";
            jadlospis_przyswajalne.Text = Math.Round(suma_jad[5, 5], 2).ToString() + " g";
            jadlospis_blonnik.Text = Math.Round(suma_jad[5, 6], 2).ToString() + " g";
            jadlospis_sod.Text = Math.Round(suma_jad[5, 7], 2).ToString() + " g";

            for (int k = 0; k < 8; k++)
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
                            wartosc_odzywcza = wartosc_odzywcza / 1000;

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
                jadlospis_cb_ktn.Value = 99;
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
                jadlospis_cb_przyswajalne.Value = 99;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 6] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik) > 100)
                { jadlospis_cb_blonnik.Value = 100; }
                else
                    jadlospis_cb_blonnik.Value = Convert.ToInt32(suma_jad[5, 6] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.blonnik);
            }
            else
                jadlospis_cb_blonnik.Value = 99;
            if (Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod != 0)
            {
                if (Convert.ToInt32(suma_jad[5, 7] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod) > 100)
                { jadlospis_cb_sod.Value = 100; }
                else
                    jadlospis_cb_sod.Value = Convert.ToInt32(suma_jad[5, 7] * 100 / Diety[jadlospis_dieta.SelectedIndex].wartosciOdzywcze.sod);
            }
            else
                jadlospis_cb_sod.Value = 99;
            
        }

        private void ja_ValueChanged(object sender, EventArgs e)
        {
            wpiszJadlospis();
        }

        private void jadlospis_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            jadlospis_dieta.Items.Clear();
            Diety = DAO.DietaDAO.SelectAllSQL(jadlospis_miasto.Text);
            foreach (Dieta d in Diety)
                jadlospis_dieta.Items.Add(d.nazwa + '/' + d.plec);

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
                string dieta = jadlospis_dieta.SelectedItem.ToString().Split('/')[0];
                string data = ja.Text;

                DAO.JadlospisDAO.DeleteSQL(data, miasto, dieta, jadlospis_dieta.SelectedItem.ToString().Split('/')[1]);
                MessageBox.Show("Usunięto wybrany jadłospis","Usuwanie jadłospisu");
            }
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(label10.Text!="Diety -> Dodaj")
                {
                dieta_dieta.Items.Clear();
                Diety = DAO.DietaDAO.SelectAllSQL(dieta_miasto.SelectedItem.ToString());
                foreach (Dieta d in Diety)
                    dieta_dieta.Items.Add(d.nazwa+'/'+d.plec);
                if (dieta_dieta.Items.Count > 0)
                    dieta_dieta.SelectedIndex = 0;
                else
                {
                    dieta_nazwa.Text = "";
                    dieta_energia_od.Text = "";
                    dieta_bialkoOd.Text = "";
                    dieta_weglowodanyOd.Text = "";
                    dieta_tluszczeOd.Text = "";
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
            
                //drukuj
                switch (drukuj_rodzaj.SelectedItem.ToString())
                {
                    case "Szablon":
                    Printer.Dekadowka(drukuj_combo.SelectedItem.ToString(), drukuj_od.Text, drukuj_do.Text,DAO.JadlospisDAO.SelectAllSQL(drukuj_od.Text, drukuj_do.Text));
                    MessageBox.Show("Wygenerowano szablon");
                    break;
                    case "Jadłospis":
                    Printer.Jadlospis(DAO.JadlospisDAO.SelectAllSQL(drukuj_data.Text, drukuj_combo.SelectedItem.ToString(), drukuj_dieta.SelectedItem.ToString().Split('/')[0], drukuj_dieta.SelectedItem.ToString().Split('/')[1]));
                    MessageBox.Show("Wygenerowano jadłospis");
                    break;
                case "Jadłospisy w danym okresie":
                    DateTime dateFrom = Convert.ToDateTime(drukuj_od.Text);
                    DateTime dateTo = Convert.ToDateTime(drukuj_do.Text);
                    for (DateTime data = dateFrom; data <= dateTo; data = data.AddDays(1))
                    {
                        string dt = (data.Day + " " + GetMonthForDate(data.Month) + " " + data.Year).ToString();
                        List<Jadlospis> jad = DAO.JadlospisDAO.SelectSQL(dt, drukuj_combo.SelectedItem.ToString());
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
                        Printer.JadlospisDzienny(DAO.JadlospisDAO.SelectSQL(dt, drukuj_combo.SelectedItem.ToString()));
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
                glownaClick();
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
                    listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                    listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                    listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                    listaJednostek = DAO.JednostkaDAO.SelectAllSQL();
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
                    listaReceptur = DAO.RecepturaDAO.SelectAllSQL();
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
            switch(label10.Text)
            {
                case "Drukowanie -> Szablon":
                    break;
                case "Drukowanie -> Jadłospis":
                    drukuj_dieta.Items.Clear();
                    Diety = DAO.DietaDAO.SelectAllSQL(drukuj_combo.SelectedItem.ToString());
                    foreach (Dieta r in Diety)
                        drukuj_dieta.Items.Add(r.nazwa+'/'+r.plec);
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

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            this.Width = 45;
            this.Height = 45;
        }

        private void dekadowka_wczytaj_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (wybranaDekadowkaDoWczytania != null)
            {
                List<Jadlospis> jadlospisyDanegoDnia = DAO.JadlospisDekadowkiDAO.SelectForDaySQL(Convert.ToInt32(wybranaDekadowkaDoWczytania.id), dekadowka_wczytaj_dzien.SelectedIndex + 1);
                foreach (Jadlospis d in jadlospisyDanegoDnia)
                {
                    if (d.dzien - 1 == dekadowka_wczytaj_dzien.SelectedIndex && d.dieta.nazwa == dekadowka_wczytaj_dieta.SelectedItem.ToString())
                    {
                        dekadowka_wczytaj_dieta.Items.Add(d.dieta.nazwa+'/'+d.dieta.plec);
                        jadlospisDekadowkiDoWczytania = d;
                    }
                }
            }
        }
        private void produkty_przyswajalne_TextChanged(object sender, EventArgs e)
        {

        }

        private void label58_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void dieta_energia_do_TextChanged(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void label213_Click(object sender, EventArgs e)
        {

        }
    }
}
