﻿namespace KalkulatorDiety
{
    public class WartosciOdzywcze
    {
        public double energia;
        public double bialko;
        public double weglowodany;
        public double tluszcze;
        public double tluszcze_nn;
        public double sod;
        public double weglowodany_przyswajalne;
        public double cukry;
        public double blonnik;

        public WartosciOdzywcze(double energia, double bialko, double tluszcze, double tluszcze_nn, double weglowodany, double weglowodany_przyswajalne, double cukry, double blonnik, double sod)
        {
            this.energia = energia;
            this.bialko = bialko;
            this.cukry = cukry;
            this.weglowodany = weglowodany;
            this.sod = sod;
            this.tluszcze = tluszcze;
            this.tluszcze_nn = tluszcze_nn;
            this.blonnik = blonnik;
            this.weglowodany_przyswajalne = weglowodany_przyswajalne;
        }

        public WartosciOdzywcze()
        {
            this.energia = 0;
            this.bialko = 0;
            this.weglowodany = 0;
            this.sod = 0;
            this.tluszcze = 0;
            this.tluszcze_nn = 0;
            this.blonnik = 0;
            this.weglowodany_przyswajalne = 0;
            this.cukry = 0;
        }
    }
}
