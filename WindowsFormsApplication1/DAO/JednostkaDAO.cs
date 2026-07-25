namespace KalkulatorDiety.DAO
{
    using System.Collections.Generic;
    using System.Data;
    using KalkulatorDiety.Models;

    public class JednostkaDAO
    {
        public static void Insert(string miasto)
        {
            DataTable dtProdukty = DAO.DataSet.Tables["Jednostka"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Miasto"] = miasto;
            dtProdukty.Rows.Add(drProdukty);
            DAO.WriteXml();
        }

        public static void Update(Jednostka jednostka, string miasto)
        {
            Delete(jednostka);
            Insert(miasto);
        }

        public static void Delete(Jednostka jednostka)
        {
            if (DAO.DataSet.Diety.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jednostka.Rows.Count; i++)
                {
                    if (DAO.DataSet.Jednostka.Rows[i]["Miasto"].ToString() == jednostka.miasto)
                        DAO.DataSet.Jednostka.Rows[i].Delete();
                }
            }
            DAO.WriteXml();
        }

        public static List<Jednostka> SelectAll()
        {
            List<Jednostka> listaJednostek = new List<Jednostka>();

            if (DAO.DataSet.Jednostka.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Jednostka.Rows.Count; i++)
                {
                    listaJednostek.Add(new Jednostka(DAO.DataSet.Jednostka.Rows[i]["Miasto"].ToString()));
                }
            }

            return listaJednostek;
        }
    }
}
