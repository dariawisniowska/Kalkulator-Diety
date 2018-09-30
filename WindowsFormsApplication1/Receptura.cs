using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Receptura
    {
        public string nazwa;
        public string sklad;

        public Receptura(string nazwa, string sklad)
        {
            this.nazwa = nazwa;
            this.sklad = sklad;
        }
    }
}