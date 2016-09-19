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
        public MainWindow()
        {
            InitializeComponent();
            string path = "./activity.csv";
            string csvText;
            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) {}
            }
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {
                csvText = sr.ReadToEnd();
            }
        }
    }
}
