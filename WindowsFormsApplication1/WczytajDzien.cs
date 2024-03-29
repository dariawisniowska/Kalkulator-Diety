﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KalkulatorDiety
{
    public partial class WczytajDzien : Form
    {
        public double[,] suma = new double[6, 6];
        Encoding enc = Encoding.GetEncoding("Windows-1250");
        List<string> lista = new List<string>();
        KalkulatorDietyDatabase DataSet = new KalkulatorDietyDatabase();
        String XML_Location = @"DataBase.xml";

        public WczytajDzien()
        {
            InitializeComponent();

            DataSet.ReadXml(XML_Location);

            if (DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DataSet.Diety.Rows.Count; i++)
                {
                    cb_dieta.Items.Add(DataSet.Diety.Rows[i]["Nazwa diety"]);
                }
                cb_dieta.SelectedIndex = 0;
            }

            if (DataSet.Tables["Jadłospisy"].Rows.Count > 0)
            {
                Wyswietl();
            }
            else
            {
                btn_dodaj.Enabled = false;
                button2.Enabled = false;
                cb_dieta.Enabled = false;
            }
            cb_miasto.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        //WCZYTAJ DZIEŃ
        private void btn_dodaj_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("WczytajDzien.txt", false, enc))
            {
                sw.Write("{0}*", textBox6.Text);
                for (int i = 0; i < listView1.Items.Count; i++)
                    sw.Write("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}$", listView1.Items[i].SubItems[0].Text, listView1.Items[i].SubItems[1].Text, listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[3].Text, listView1.Items[i].SubItems[4].Text, listView1.Items[i].SubItems[5].Text, listView1.Items[i].SubItems[6].Text, listView1.Items[i].SubItems[7].Text);
                sw.Write("\n{0}*", textBox7.Text);
                for (int i = 0; i < listView2.Items.Count; i++)
                    sw.Write("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}$", listView2.Items[i].SubItems[0].Text, listView2.Items[i].SubItems[1].Text, listView2.Items[i].SubItems[2].Text, listView2.Items[i].SubItems[3].Text, listView2.Items[i].SubItems[4].Text, listView2.Items[i].SubItems[5].Text, listView2.Items[i].SubItems[6].Text, listView2.Items[i].SubItems[7].Text);
                sw.Write("\n{0}*", textBox8.Text);
                for (int i = 0; i < listView3.Items.Count; i++)
                    sw.Write("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}$", listView3.Items[i].SubItems[0].Text, listView3.Items[i].SubItems[1].Text, listView3.Items[i].SubItems[2].Text, listView3.Items[i].SubItems[3].Text, listView3.Items[i].SubItems[4].Text, listView3.Items[i].SubItems[5].Text, listView3.Items[i].SubItems[6].Text, listView3.Items[i].SubItems[7].Text);
                sw.Write("\n{0}*", textBox9.Text);
                for (int i = 0; i < listView4.Items.Count; i++)
                    sw.Write("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}$", listView4.Items[i].SubItems[0].Text, listView4.Items[i].SubItems[1].Text, listView4.Items[i].SubItems[2].Text, listView4.Items[i].SubItems[3].Text, listView4.Items[i].SubItems[4].Text, listView4.Items[i].SubItems[5].Text, listView4.Items[i].SubItems[6].Text, listView4.Items[i].SubItems[7].Text);
                sw.Write("\n{0}*", textBox10.Text);
                for (int i = 0; i < listView5.Items.Count; i++)
                    sw.Write("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}$", listView5.Items[i].SubItems[0].Text, listView5.Items[i].SubItems[1].Text, listView5.Items[i].SubItems[2].Text, listView5.Items[i].SubItems[3].Text, listView5.Items[i].SubItems[4].Text, listView5.Items[i].SubItems[5].Text, listView5.Items[i].SubItems[6].Text, listView5.Items[i].SubItems[7].Text);

            }
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void Wyswietl()
        {
            try
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
                listView3.Items.Clear();
                listView4.Items.Clear();
                listView5.Items.Clear();
                for (int k = 0; k < 6; k++)
                    for (int j = 0; j < 6; j++)
                    {
                        suma[j, k] = 0;
                    }

                int wybrana = -1;
                for (int i = 0; i < DataSet.Tables["Jadłospisy"].Rows.Count; i++)
                {
                    if (DataSet.Tables["Jadłospisy"].Rows[i]["Data"] + "," + DataSet.Tables["Jadłospisy"].Rows[i]["Dieta"] + "," + DataSet.Tables["Jadłospisy"].Rows[i]["Miasto"] == dateTimePicker1.Text + "," + cb_dieta.SelectedItem+","+cb_miasto.SelectedItem)
                    {
                        wybrana = i;
                    }
                }

                textBox6.Text = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Nazwa-Śniadanie"].ToString();
                string[] sniadanie_produkty = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Skład-Śniadanie"].ToString().Split('$');
                for (int j = 0; j < sniadanie_produkty.Length - 1; j++)
                {
                    string[] arr = sniadanie_produkty[j].Split('|');
                    ListViewItem itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                }


                textBox7.Text = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Nazwa-IIŚniadanie"].ToString();
                sniadanie_produkty = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Skład-IIŚniadanie"].ToString().Split('$');
                for (int j = 0; j < sniadanie_produkty.Length - 1; j++)
                {
                    string[] arr = sniadanie_produkty[j].Split('|');
                    ListViewItem itm = new ListViewItem(arr);
                    listView2.Items.Add(itm);
                }

                textBox8.Text = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Nazwa-Obiad"].ToString();
                sniadanie_produkty = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Skład-Obiad"].ToString().Split('$');
                for (int j = 0; j < sniadanie_produkty.Length - 1; j++)
                {
                    string[] arr = sniadanie_produkty[j].Split('|');
                    ListViewItem itm = new ListViewItem(arr);
                    listView3.Items.Add(itm);
                }

                textBox9.Text = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Nazwa-Podwieczorek"].ToString();
                sniadanie_produkty = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Skład-Podwieczorek"].ToString().Split('$');
                for (int j = 0; j < sniadanie_produkty.Length - 1; j++)
                {
                    string[] arr = sniadanie_produkty[j].Split('|');
                    ListViewItem itm = new ListViewItem(arr);
                    listView4.Items.Add(itm);
                }

                textBox10.Text = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Nazwa-Kolacja"].ToString();
                sniadanie_produkty = DataSet.Tables["Jadłospisy"].Rows[wybrana]["Skład-Kolacja"].ToString().Split('$');
                for (int j = 0; j < sniadanie_produkty.Length - 1; j++)
                {
                    string[] arr = sniadanie_produkty[j].Split('|');
                    ListViewItem itm = new ListViewItem(arr);
                    listView5.Items.Add(itm);
                }

                LiczSume();
            }
            catch (Exception)
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
                listView3.Items.Clear();
                listView4.Items.Clear();
                listView5.Items.Clear();
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                lv_suma_razem.Items.Clear();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Wyswietl();
        }

        public void LiczSume()
        {
            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    suma[i, k] = 0;
                }

            }
            lv_suma_razem.Items.Clear();
            string[] arr = new string[6];
            for (int i = 0; i < 6; i++)
                arr[i] = "0";
            ListViewItem itm = new ListViewItem(arr);
            itm.UseItemStyleForSubItems = false;
            lv_suma_razem.Items.Add(itm);

            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    double a = double.Parse(listView1.Items[i].SubItems[k + 2].Text);
                    suma[0, k] += a;
                }
            }

            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    double a = double.Parse(listView2.Items[i].SubItems[k + 2].Text);
                    suma[1, k] += a;
                }
            }

            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    double a = double.Parse(listView3.Items[i].SubItems[k + 2].Text);
                    suma[2, k] += a;
                }
            }
            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    double a = double.Parse(listView4.Items[i].SubItems[k + 2].Text);
                    suma[3, k] += a;
                }
            }


            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < listView5.Items.Count; i++)
                {
                    double a = double.Parse(listView5.Items[i].SubItems[k + 2].Text);
                    suma[4, k] += a;
                }
            }

            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    suma[5, k] += suma[i, k];
                }
            }
            for (int k = 0; k < 6; k++)
            {
                lv_suma_razem.Items[0].SubItems[k].Text = Math.Round(suma[5, k], 2).ToString();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int wybrana = -1;
            for (int i = 0; i < DataSet.Tables["Jadłospisy"].Rows.Count; i++)
            {
                if (DataSet.Tables["Jadłospisy"].Rows[i]["Data"] + "," + DataSet.Tables["Jadłospisy"].Rows[i]["Dieta"] + "," + DataSet.Tables["Jadłospisy"].Rows[i]["Miasto"] == dateTimePicker1.Text + "," + cb_dieta.SelectedItem+","+cb_miasto.SelectedText)
                {
                    wybrana = i;
                }
            }

            DataSet.Tables["Jadłospisy"].Rows[wybrana].Delete();
            DataSet.WriteXml(XML_Location);

            Form1 f = new Form1();
            this.Close();
            f.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Wyswietl();
        }

        private void cb_dieta_SelectedIndexChanged(object sender, EventArgs e)
        {
            Wyswietl();
        }

        private void cb_miasto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Wyswietl();
        }

        private void cb_miasto_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Wyswietl();
        }

    }
}
