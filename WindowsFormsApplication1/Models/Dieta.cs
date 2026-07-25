
using System.Web;

namespace KalkulatorDiety
{    
    public class Dieta
    {
        public string nazwa;
        public string miasto;
        public string kod;

        public double energiaOd;
        public double energiaDo;
        public double energiaOdNaTysiąc;
        public double energiaDoNaTysiąc;
        public double energiaProcentOd;
        public double energiaProcentDo;

        public double bialkoOd;
        public double bialkoDo;
        public double bialkoOdNaTysiąc;
        public double bialkoDoNaTysiąc;
        public double bialkoProcentOd;
        public double bialkoProcentDo;

        public double tluszczeOd;
        public double tluszczeDo;
        public double tluszczeOdNaTysiąc;
        public double tluszczeDoNaTysiąc;
        public double tluszczeProcentOd;
        public double tluszczeProcentDo;

        public double kwasyOd;
        public double kwasyDo;
        public double kwasyOdNaTysiąc;
        public double kwasyDoNaTysiąc;
        public double kwasyProcentOd;
        public double kwasyProcentDo;

        public double wegleOd;
        public double wegleDo;
        public double wegleOdNaTysiąc;
        public double wegleDoNaTysiąc;
        public double wegleProcentOd;
        public double wegleProcentDo;

        public double przyswajalneOd;
        public double przyswajalneDo;
        public double przyswajalneOdNaTysiąc;
        public double przyswajalneDoNaTysiąc;
        public double przyswajalneProcentOd;
        public double przyswajalneProcentDo;

        public double cukryOd;
        public double cukryDo;
        public double cukryOdNaTysiąc;
        public double cukryDoNaTysiąc;
        public double cukryProcentOd;
        public double cukryProcentDo;

        public double blonnikOd;
        public double blonnikDo;
        public double blonnikOdNaTysiąc;
        public double blonnikDoNaTysiąc;
        public double blonnikProcentOd;
        public double blonnikProcentDo;

        public double sodOd;
        public double sodDo;
        public double sodOdNaTysiąc;
        public double sodDoNaTysiąc;
        public double sodProcentOd;
        public double sodProcentDo;

        public double solOd;
        public double solDo;
        public double solOdNaTysiąc;
        public double solDoNaTysiąc;
        public double solProcentOd;
        public double solProcentDo;

        public Dieta(string nazwa, string miasto, string kod,
            double energiaOd, double energiaDo, double energiaOdNaTysiąc, double energiaDoNaTysiąc, double energiaProcentOd, double energiaProcentDo,
            double bialkoOd, double bialkoDo, double bialkoOdNaTysiąc, double bialkoDoNaTysiąc, double bialkoProcentOd, double bialkoProcentDo,
            double tluszczeOd, double tluszczeDo, double tluszczeOdNaTysiąc, double tluszczeDoNaTysiąc, double tluszczeProcentOd, double tluszczeProcentDo,
            double kwasyOd, double kwasyDo, double kwasyOdNaTysiąc, double kwasyDoNaTysiąc, double kwasyProcentOd, double kwasyProcentDo,
            double wegleOd, double wegleDo, double wegleOdNaTysiąc, double wegleDoNaTysiąc, double wegleProcentOd, double wegleProcentDo,
            double przyswajalneOd, double przyswajalneDo, double przyswajalneOdNaTysiąc, double przyswajalneDoNaTysiąc, double przyswajalneProcentOd, double przyswajalneProcentDo,
            double cukryOd, double cukryDo, double cukryOdNaTysiąc, double cukryDoNaTysiąc, double cukryProcentOd, double cukryProcentDo,
            double blonnikOd, double blonnikDo, double blonnikOdNaTysiąc, double blonnikDoNaTysiąc, double blonnikProcentOd, double blonnikProcentDo,
            double sodOd, double sodDo, double sodOdNaTysiąc, double sodDoNaTysiąc, double sodProcentOd, double sodProcentDo,
            double solOd, double solDo, double solOdNaTysiąc, double solDoNaTysiąc, double solProcentOd, double solProcentDo)
        {
            this.nazwa = nazwa;
            this.miasto = miasto;
            this.kod = kod;

            this.energiaOd = energiaOd;
            this.energiaDo = energiaDo;
            this.energiaOdNaTysiąc = energiaOdNaTysiąc;
            this.energiaDoNaTysiąc = energiaDoNaTysiąc;
            this.energiaProcentOd = energiaProcentOd;
            this.energiaProcentDo = energiaProcentDo;

            this.bialkoOd = bialkoOd;
            this.bialkoDo = bialkoDo;
            this.bialkoOdNaTysiąc = bialkoOdNaTysiąc;
            this.bialkoDoNaTysiąc = bialkoDoNaTysiąc;
            this.bialkoProcentOd = bialkoProcentOd;
            this.bialkoProcentDo = bialkoProcentDo;

            this.tluszczeOd = tluszczeOd;
            this.tluszczeDo = tluszczeDo;
            this.tluszczeOdNaTysiąc = tluszczeOdNaTysiąc;
            this.tluszczeDoNaTysiąc = tluszczeDoNaTysiąc;
            this.tluszczeProcentOd = tluszczeProcentOd;
            this.tluszczeProcentDo = tluszczeProcentDo;

            this.kwasyOd = kwasyOd;
            this.kwasyDo = kwasyDo;
            this.kwasyOdNaTysiąc = kwasyOdNaTysiąc;
            this.kwasyDoNaTysiąc = kwasyDoNaTysiąc;
            this.kwasyProcentOd = kwasyProcentOd;
            this.kwasyProcentDo = kwasyProcentDo;

            this.wegleOd = wegleOd;
            this.wegleDo = wegleDo;
            this.wegleOdNaTysiąc = wegleOdNaTysiąc;
            this.wegleDoNaTysiąc = wegleDoNaTysiąc;
            this.wegleProcentOd = wegleProcentOd;
            this.wegleProcentDo = wegleProcentDo;

            this.przyswajalneOd = przyswajalneOd;
            this.przyswajalneDo = przyswajalneDo;
            this.przyswajalneOdNaTysiąc = przyswajalneOdNaTysiąc;
            this.przyswajalneDoNaTysiąc = przyswajalneDoNaTysiąc;
            this.przyswajalneProcentOd = przyswajalneProcentOd;
            this.przyswajalneProcentDo = przyswajalneProcentDo;

            this.cukryOd = cukryOd;
            this.cukryDo = cukryDo;
            this.cukryOdNaTysiąc = cukryOdNaTysiąc;
            this.cukryDoNaTysiąc = cukryDoNaTysiąc;
            this.cukryProcentOd = cukryProcentOd;
            this.cukryProcentDo = cukryProcentDo;

            this.blonnikOd = blonnikOd;
            this.blonnikDo = blonnikDo;
            this.blonnikOdNaTysiąc = blonnikOdNaTysiąc;
            this.blonnikDoNaTysiąc = blonnikDoNaTysiąc;
            this.blonnikProcentOd = blonnikProcentOd;
            this.blonnikProcentDo = blonnikProcentDo;

            this.sodOd = sodOd;
            this.sodDo = sodDo;
            this.sodOdNaTysiąc = sodOdNaTysiąc;
            this.sodDoNaTysiąc = sodDoNaTysiąc;
            this.sodProcentOd = sodProcentOd;
            this.sodProcentDo = sodProcentDo;

            this.solOd = solOd;
            this.solDo = solDo;
            this.solOdNaTysiąc = solOdNaTysiąc;
            this.solDoNaTysiąc = solDoNaTysiąc;
            this.solProcentOd = solProcentOd;
            this.solProcentDo = solProcentDo;
        }
    }
}
