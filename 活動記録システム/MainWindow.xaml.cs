using Gecko;
using System;
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
        private GeckoWebBrowser _browser;
        public ObservableCollection<Person> MemberCollection { get; set; }
        public ObservableCollection<Activity> Collection { get; set; }
        private string @path = "./activity.csv";

        public MainWindow()
        {
            MemberCollection = new ObservableCollection<Person>();
            Collection = new ObservableCollection<Activity>();
            InitializeComponent();

            if (!System.IO.File.Exists(path))
            {
                using (FileStream fileStream = File.Create(path)) { }
            }

            if (!System.IO.File.Exists("./member.txt"))
            {
                using (FileStream fileStream = File.Create("./member.txt")) { }
            }

            updateMember();
            updateHistory();
        }

        private void updateHistory()
        {
            Collection.Clear();
            string csvText;

            using (StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {
                csvText = streamReader.ReadToEnd();
            }

            using (StringReader stringReader = new StringReader(csvText))
            {
                while (stringReader.Peek() > -1)
                {
                    string line = stringReader.ReadLine();
                    if (!line.Equals(""))
                    {
                        string[] cells = line.Split(',');
                        Collection.Insert(0, new Activity { Date = cells[0], Name = cells[1], Title = cells[2], Content = cells[3] });
                    }
                }
            }
        }

        private void updateMember()
        {
            MemberCollection.Clear();
            string csvText;
            using (StreamReader streamReader = new StreamReader("./member.txt", Encoding.GetEncoding("UTF-8")))
            {
                csvText = streamReader.ReadToEnd();
            }

            using (StringReader stringReader = new StringReader(csvText))
            {
                while (stringReader.Peek() > -1)
                {
                    MemberCollection.Add(new Person { Name = stringReader.ReadLine() });
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.Text.Equals("")) { MessageBox.Show("名前が選択されていません"); return; }
            if (date.Text.Equals("")) { MessageBox.Show("日付が選択されていません"); return; }
            if (title.Text.Equals("")) { MessageBox.Show("タイトルが入力されていません"); return; }
            if (content.Text.Equals("")) { MessageBox.Show("内容が入力されていません"); return; }

            if (format.Text == "Plain Text")
            {
                File.AppendAllText(@path, date.Text + "," + MemberCollection[comboBox.SelectedIndex].Name + "," + title.Text + "," + content.Text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br>").Replace("\r", "<br>").Replace("\n", "<br>") + "\n");
            } else
            {
                File.AppendAllText(@path, date.Text + "," + MemberCollection[comboBox.SelectedIndex].Name + "," + title.Text + "," + content.Text.Replace("\r", "").Replace("\n", "") + "\n");
            }

            updateHistory();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("help.html");
        }

        private void version_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("活動記録システム Version 1.1.0\n対象フレームワーク .NET Framework 4.5.2\n開発者 0918nobita");
        }

        private void add_member_Click(object sender, RoutedEventArgs e)
        {
            if (member_name.Text.Equals("")) { MessageBox.Show("名前が入力されていません"); return; }
            File.AppendAllText("./member.txt", member_name.Text + "\n");
            updateMember();
            member_name.Text = "";
        }

        private void dataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex < 0) return;
            Activity item = Collection[dataGrid.SelectedIndex];
            string date = item.Date;
            string name = item.Name;
            string title = item.Title;
            string content = item.Content;
            _browser.LoadHtml(
                "<!DOCTYPE html><html lang='ja'><head><style>body { font-family: メイリオ; }</style></head><body>" +
                "<h1>" + title + "</h1><p>" + date + "</p><p>" + content + "</p>" +
                "</body></html>"
            );
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var host = new System.Windows.Forms.Integration.WindowsFormsHost();
            _browser = new GeckoWebBrowser();
            host.Child = _browser;
            webBrowser.Children.Add(host);
        }
    }

    public class Activity
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
