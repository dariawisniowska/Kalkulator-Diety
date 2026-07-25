namespace KalkulatorDiety.DAO
{
    using System.Collections.Generic;
    using System.Data;
    
    public class RecepturaDAO
    {
        public static void Insert(string nazwa,string sklad)
        {
            DataTable dtProdukty = DAO.DataSet.Tables["Receptury"];
            DataRow drProdukty = dtProdukty.NewRow();
            drProdukty["Nazwa receptury"] = nazwa;
            drProdukty["Skład receptury"] = sklad;
            dtProdukty.Rows.Add(drProdukty);
            DAO.WriteXml();
        }

        public static void Update(Receptura receptura, string nazwa, string sklad)
        {
            Delete(receptura);
            Insert(nazwa, sklad);
        }

        public static void Delete(Receptura receptura)
        {
            if (DAO.DataSet.Receptury.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Receptury.Rows.Count; i++)
                {
                    if (DAO.DataSet.Receptury.Rows[i]["Nazwa receptury"].ToString() == receptura.nazwa && DAO.DataSet.Receptury.Rows[i]["Skład receptury"].ToString() == receptura.sklad)
                        DAO.DataSet.Receptury.Rows[i].Delete();
                }
            }
            DAO.WriteXml();
        }

        public static List<Receptura> SelectAll()
        {
            List<Receptura> listaDiet = new List<Receptura>();

            if (DAO.DataSet.Receptury.Rows.Count > 0)
            {
                for (int i = 0; i < DAO.DataSet.Receptury.Rows.Count; i++)
                {
                    listaDiet.Add(new Receptura(DAO.DataSet.Receptury.Rows[i]["Nazwa receptury"].ToString(), DAO.DataSet.Receptury.Rows[i]["Skład receptury"].ToString()));
                }
            }

            return listaDiet;
        }
    }
}
