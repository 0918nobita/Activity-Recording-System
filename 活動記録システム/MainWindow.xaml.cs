using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Controls;

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

            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "日付", IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "タイトル", IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "内容", IsReadOnly = true });

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
