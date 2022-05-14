using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Produkt
    {
        public string nazwa;
        public char kategoria;
        public WartosciOdzywcze wartosciOdzywcze;

        public Produkt(char kategoria, string nazwa, WartosciOdzywcze wartosciOdzywcze)
        {
            this.kategoria = kategoria;
            this.nazwa = nazwa;
            this.wartosciOdzywcze = wartosciOdzywcze;
        }

        public Produkt(char kategoria, string nazwa, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double weglowodany_przyswajalne, double blonnik)
        {
            this.kategoria = kategoria;
            this.nazwa = nazwa;
            this.wartosciOdzywcze = new WartosciOdzywcze(energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn, weglowodany_przyswajalne,blonnik);
        }
        public Produkt(char kategoria, string nazwa, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn)
        {
            this.kategoria = kategoria;
            this.nazwa = nazwa;
            this.wartosciOdzywcze = new WartosciOdzywcze(energia, bialko, tluszcze, weglowodany, sod, tluszcze_nn);
        }
    }
    public enum TypyProduktow
    {
        B,
        M,
        P,
        N,
        O,
        W,
        R,
        T,
        S,
        D,
        Z,
        A //WSZYSTKIE
    }
}
