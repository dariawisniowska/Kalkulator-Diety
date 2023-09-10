using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class WartosciOdzywcze
    {
        public double energia;
        public double energiaOd;
        public double energiaDo;
        public double bialko;
        public double bialkoOd;
        public double bialkoDo;
        public double weglowodany;
        public double weglowodanyOd;
        public double weglowodanyDo;
        public double cukry;
        public double tluszcze;
        public double tluszczeOd;
        public double tluszczeDo;
        public double tluszcze_nn;
        public double sod;
        public double weglowodany_przyswajalne;
        public double blonnik;
        public double witA;
        public double witB1;
        public double witB2;
        public double witB6;
        public double witB12;
        public double niacyna;
        public double witC;
        public double witD;
        public double witE;
        public double witK;
        public double foliany;
        public double fosfor;
        public double magnez;
        public double zelazo;
        public double cynk;
        public double jod;
        public double selen;
        public double miedz;
        public double cholina;
        public double kwasPantotenowy;
        public double biotyna;
        public double mangan;
        public double fluor;
        public double potas;


        public WartosciOdzywcze(double energiaOd, double energiaDo, double bialkoOd, double bialkoDo, double tluszczeOd, double tluszczeDo, double tluszcze_nn, double weglowodanyOd, double weglowodanyDo, double cukry, double blonnik, 
            double sod, double witA, double witB1, double witB2, double witB6, double witB12, double niacyna, double witC, double witD, double witE, double witK,
            double foliany, double fosfor, double magnez, double zelazo, double cynk, double jod, double selen, double miedz, double cholina, double kwasPantotenowy,
            double biotyna, double mangan, double fluor, double potas)
        {
            this.energiaOd = energiaOd;
            this.energiaDo = energiaDo;
            this.bialkoOd = bialkoOd;
            this.bialkoDo = bialkoDo;
            this.weglowodanyOd = weglowodanyOd;
            this.weglowodanyDo = weglowodanyDo;
            this.sod = sod;
            this.tluszczeOd = tluszczeOd;
            this.tluszczeDo = tluszczeDo;
            this.tluszcze_nn = tluszcze_nn;
            this.blonnik = blonnik;
            this.cukry = cukry;
            this.witA = witA;
            this.witB1 = witB1;
            this.witB2 = witB2;
            this.witB6 = witB6;
            this.witB12 = witB12;
            this.niacyna = niacyna;
            this.witC = witC;
            this.witD = witD;
            this.witE = witE;
            this.witK = witK;
            this.foliany = foliany;
            this.fosfor = fosfor;
            this.magnez = magnez;
            this.zelazo = zelazo;
            this.cynk = cynk;
            this.jod = jod;
            this.selen = selen;
            this.miedz = miedz;
            this.cholina = cholina;
            this.kwasPantotenowy = kwasPantotenowy;
            this.biotyna = biotyna;
            this.mangan = mangan;
            this.fluor = fluor;
            this.potas = potas;
        }

        public WartosciOdzywcze(double energia, double bialko, double tluszcze, double tluszcze_nn, double weglowodany, double cukry, double blonnik,
    double sod, double witA, double witB1, double witB2, double witB6, double witB12, double niacyna, double witC, double witD, double witE, double witK,
    double foliany, double fosfor, double magnez, double zelazo, double cynk, double jod, double selen, double miedz, double cholina, double kwasPantotenowy,
    double biotyna, double mangan, double fluor, double potas)
        {
            this.energia = energia;
            this.bialko = bialko;
            this.weglowodany = weglowodany;
            this.sod = sod;
            this.tluszcze = tluszcze;
            this.tluszcze_nn = tluszcze_nn;
            this.blonnik = blonnik;
            this.cukry = cukry;
            this.witA = witA;
            this.witB1 = witB1;
            this.witB2 = witB2;
            this.witB6 = witB6;
            this.witB12 = witB12;
            this.niacyna = niacyna;
            this.witC = witC;
            this.witD = witD;
            this.witE = witE;
            this.witK = witK;
            this.foliany = foliany;
            this.fosfor = fosfor;
            this.magnez = magnez;
            this.zelazo = zelazo;
            this.cynk = cynk;
            this.jod = jod;
            this.selen = selen;
            this.miedz = miedz;
            this.cholina = cholina;
            this.kwasPantotenowy = kwasPantotenowy;
            this.biotyna = biotyna;
            this.mangan = mangan;
            this.fluor = fluor;
            this.potas = potas;
        }
    }
}
