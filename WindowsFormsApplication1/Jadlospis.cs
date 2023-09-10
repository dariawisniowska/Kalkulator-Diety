using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Jadlospis
    {
        public int? identyfikator;
        public string data;
        public Dieta dieta;
        public int dzien;
        public string miasto;
        public string nazwa_sniadanie;
        public string nazwa_IIsniadanie;
        public string nazwa_obiad;
        public string nazwa_podwieczorek;
        public string nazwa_kolacja;
        public string sklad_sniadanie;
        public string sklad_IIsniadanie;
        public string sklad_obiad;
        public string sklad_podwieczorek;
        public string sklad_kolacja;

        public Jadlospis(string data, Dieta dieta, string miasto, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad,string nazwa_podwieczorek, string nazwa_kolacja,string sklad_sniadanie,string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek,string sklad_kolacja)
        {
            this.data = data;
            this.dieta = dieta;
            this.miasto = miasto;
            this.nazwa_IIsniadanie = nazwa_IIsniadanie;
            this.nazwa_sniadanie = nazwa_sniadanie;
            this.nazwa_obiad = nazwa_obiad;
            this.nazwa_podwieczorek = nazwa_podwieczorek;
            this.nazwa_kolacja = nazwa_kolacja;
            this.sklad_IIsniadanie = sklad_IIsniadanie;
            this.sklad_sniadanie = sklad_sniadanie;
            this.sklad_obiad = sklad_obiad;
            this.sklad_podwieczorek = sklad_podwieczorek;
            this.sklad_kolacja = sklad_kolacja;
        }

        public Jadlospis(int? identyfikator, int dzien, Dieta dieta, string nazwa_sniadanie, string nazwa_IIsniadanie, string nazwa_obiad, string nazwa_podwieczorek, string nazwa_kolacja, string sklad_sniadanie, string sklad_IIsniadanie, string sklad_obiad, string sklad_podwieczorek, string sklad_kolacja)
        {
            this.identyfikator = identyfikator;
            this.dzien = dzien;
            this.dieta = dieta;
            this.nazwa_IIsniadanie = nazwa_IIsniadanie;
            this.nazwa_sniadanie = nazwa_sniadanie;
            this.nazwa_obiad = nazwa_obiad;
            this.nazwa_podwieczorek = nazwa_podwieczorek;
            this.nazwa_kolacja = nazwa_kolacja;
            this.sklad_IIsniadanie = sklad_IIsniadanie;
            this.sklad_sniadanie = sklad_sniadanie;
            this.sklad_obiad = sklad_obiad;
            this.sklad_podwieczorek = sklad_podwieczorek;
            this.sklad_kolacja = sklad_kolacja;

        }
    }
}
