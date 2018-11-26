using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebBrowser
{
    /// <summary>
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Window
    {
        public string HistoryPath = Properties.Settings.Default.HistoryPath;
        public HistoryPage()
        {
            InitializeComponent();
        }

    

        private void GridLoad(object sender, RoutedEventArgs e)
        {
           
            List<HistoryTable> tables = new List<HistoryTable>(3);
            History[] AllHistory =  History.GetHisttory(HistoryPath);
            foreach (var CurrentHistory in AllHistory.Reverse())
            {
                if (CurrentHistory != null)
                {
                    if (CurrentHistory.URL != "")
                    {
                        HistoryTable currentTable = new HistoryTable(CurrentHistory.URL, CurrentHistory.TdateTime);
                        tables.Add(currentTable);
                    }
                   
                }
              
            }
            MainGrid.ItemsSource = tables;
            MainGrid.MaxColumnWidth = 628;
        }
    }

    class HistoryTable
    {
        public string URL { get; set; }
        public DateTime Date { get; set; }

        public HistoryTable(string URL,DateTime date)
        {
            this.URL = URL;
            this.Date = date;
        }
    }
}
