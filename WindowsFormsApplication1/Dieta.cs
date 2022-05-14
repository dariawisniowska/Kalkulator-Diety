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

        public Dieta(string nazwa, string miasto, WartosciOdzywcze wartosciOdzywcze)
        {
            this.nazwa = nazwa;
            this.miasto = miasto;
            this.wartosciOdzywcze = wartosciOdzywcze;
        }
    }
}
