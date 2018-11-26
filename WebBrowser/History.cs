using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace WebBrowser
{
    [Serializable]
    public class History
    {

        public string URL;
        public DateTime TdateTime;


        //TODO Добавить сохранение истории
        //TODO Добавить чтение из истории
        public History()
        {
            TdateTime = DateTime.Now;
        }
        public History(string URL)
        {
            this.URL = URL;
            TdateTime = DateTime.Now;
        }
        public static void AddToHistory(string URL,string Path)
        {
            bool HistoryExist = true;
            History[] AllHistory = new History[100];

            XmlSerializer Serializer = new XmlSerializer(typeof(History[]));
            using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
            {
                try
                {
                    AllHistory = (History[])Serializer.Deserialize(fs);
                }
                catch (System.InvalidOperationException e)
                {
                    HistoryExist = false;
                   
                }
                
            }

            if (!HistoryExist)
            {
                History[] NewHistory = new History[1];
                WriteToFile(NewHistory, Path);
                return;
            }
            History HistoryToAdd = new History{URL = URL};
            History[] HistoryToWrite = new History[AllHistory.Length+1];
            for (int i = 0; i < AllHistory.Length; i++)
            {
                HistoryToWrite[i] = AllHistory[i];
            }

            HistoryToWrite[AllHistory.Length] = HistoryToAdd;
            WriteToFile(HistoryToWrite,Path);
          
            


        }

        public static void WriteToFile(History[] HistoryToWrite,string Path)
        {


            XmlSerializer Serializer = new XmlSerializer(typeof(History[]));
            using (FileStream fs = new FileStream(Path, FileMode.Create))
            {
                Serializer.Serialize(fs, HistoryToWrite);
            }
        }

        public static History[] GetHisttory(string Path)
        {
            History[] AllHistory = new History[100];

            XmlSerializer Serializer = new XmlSerializer(typeof(History[]));
            using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
            {
                try
                {
                    AllHistory = (History[])Serializer.Deserialize(fs);
                }
                catch (System.InvalidOperationException e)
                {
                   

                }

            }

            return AllHistory;
        }
    }

}
