using System.Windows;
using System.IO;
using System.Text;

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
            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) {}
            }
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8"));
        }
    }
}
