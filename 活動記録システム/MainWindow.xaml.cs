using System.Windows;
using System.IO;
using System.Text;
using System.Collections;

namespace 活動記録システム
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = "./activity.csv";
        public MainWindow()
        {
            InitializeComponent();
            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) {}
                updateHistory();
            }
        }
        private void updateHistory()
        {
            string csvText;
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {
                csvText = sr.ReadToEnd();
            }
            StringReader rs = new StringReader(csvText);
            while (rs.Peek() > -1)
            {
            }
        }
    }
}
