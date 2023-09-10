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
