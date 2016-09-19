using System.Windows;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;

namespace 活動記録システム
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Activity> Collection { get; set; }
        private string path = "./activity.csv";

        public MainWindow()
        {
            Collection = new ObservableCollection<Activity>();
            InitializeComponent();

            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) {}
            }

            updateHistory();
        }

        private void updateHistory()
        {
            string csvText;

            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {
                csvText = sr.ReadToEnd();
            }

            StringReader rs = new StringReader(csvText);
            Collection.Add(new Activity { Date = "2016/09/19", Title = "サンプル", Content = "本文" });
        }
    }
}

public class Activity
{
    public string Date { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
