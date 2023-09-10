using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    
    public class Dieta
    {
        public string nazwa;
        public string miasto;
        public WartosciOdzywcze wartosciOdzywcze;
        public string kod;
        public string plec;

        public Dieta(string nazwa, string miasto, string kod, string plec, WartosciOdzywcze wartosciOdzywcze)
        {
            this.nazwa = nazwa;
            this.miasto = miasto;
            this.wartosciOdzywcze = wartosciOdzywcze;
            this.kod = kod;
            this.plec = plec;
        }
    }
}
