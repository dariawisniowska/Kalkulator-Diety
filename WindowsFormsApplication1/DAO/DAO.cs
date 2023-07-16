using System;

namespace WindowsFormsApplication1.DAO
{
    public static class DAO
    {
        public static KalkulatorDietyDatabase DataSet { get; set; }
        private static String XML_Location = @"DataBase.xml";

        public static void ReloadDatabase()
        {
            DataSet = new KalkulatorDietyDatabase();
            DataSet.ReadXml(XML_Location);
        }

        public static void WriteXml()
        {
            DataSet.WriteXml(XML_Location);
        }
    }
}
