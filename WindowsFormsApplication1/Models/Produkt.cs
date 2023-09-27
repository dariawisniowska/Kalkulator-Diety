namespace KalkulatorDiety
{
    public class Produkt
    {
        public string nazwa;
        public char kategoria;
        public WartosciOdzywcze wartosciOdzywcze;

        public Produkt(char kategoria, string nazwa, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double weglowodany_przyswajalne, double blonnik, double cukry)
        {
            this.kategoria = kategoria;
            this.nazwa = nazwa;
            this.wartosciOdzywcze = new WartosciOdzywcze(energia, bialko, tluszcze, tluszcze_nn, weglowodany, weglowodany_przyswajalne, cukry, blonnik, sod);
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
