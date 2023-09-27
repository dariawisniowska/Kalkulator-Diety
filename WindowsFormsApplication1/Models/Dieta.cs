
namespace KalkulatorDiety
{    
    public class Dieta
    {
        public string nazwa;
        public string miasto;
        public WartosciOdzywcze wartosciOdzywcze;

        public Dieta(string nazwa, string miasto, double energia, double bialko, double tluszcze, double weglowodany, double sod, double tluszcze_nn, double weglowodany_przyswajalne, double blonnik, double cukry)
        {
            this.nazwa = nazwa;
            this.miasto = miasto;
            this.wartosciOdzywcze = new WartosciOdzywcze(energia, bialko, tluszcze, tluszcze_nn, weglowodany, weglowodany_przyswajalne, cukry, blonnik, sod);
        }
    }
}
