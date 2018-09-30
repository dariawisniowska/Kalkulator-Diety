using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Dekadowka
    {
        public int? id;
        public string nazwa;
        public string miasto;
        public int dni;
        public string dzienStart;
        public List<Jadlospis> listaJadlospisow;
        
        public Dekadowka(int? id, string nazwa, string miasto, int dni, string dzienStart, List<Jadlospis> listaJadlospisow)
        {
            this.id = id;
            this.nazwa = nazwa;
            this.miasto = miasto;
            this.dni = dni;
            this.dzienStart = dzienStart;
            this.listaJadlospisow = listaJadlospisow;

        }
    }
}
