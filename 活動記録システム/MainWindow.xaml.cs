﻿using Gecko;
using ClosedXML.Excel;
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
                        Collection.Insert(0, new Activity { Date = cells[0], Name = cells[1].Replace(";;", ","), Title = cells[2].Replace(";;", ","), Content = cells[3].Replace(";;", ",") });
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
                File.AppendAllText(@path, date.Text + "," + MemberCollection[comboBox.SelectedIndex].Name.Replace(",", ";;") + "," + title.Text.Replace(",", ";;") + "," + content.Text.Replace(",", ";;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br>").Replace("\r", "<br>").Replace("\n", "<br>") + "\n");
            } else
            {
                File.AppendAllText(@path, date.Text + "," + MemberCollection[comboBox.SelectedIndex].Name.Replace(",", ";;") + "," + title.Text.Replace(",", ";;") + "," + content.Text.Replace(",", ";;").Replace("\r", "").Replace("\n", "") + "\n");
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
            MessageBox.Show("活動記録システム Version 1.1.1\n対象フレームワーク .NET Framework 4.5.2\n開発者 0918nobita");
        }

        private void add_member_Click(object sender, RoutedEventArgs e)
        {
            if (member_name.Text.Equals("")) { MessageBox.Show("名前が入力されていません"); return; }
            File.AppendAllText("./member.txt", member_name.Text + "\n");
            updateMember();
            MessageBox.Show(member_name.Text + " を追加しました");
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
                "<!DOCTYPE html><html lang='ja'><head><meta charset='utf-8'><style>body { font-family: メイリオ; }</style></head><body>" +
                "<h1>" + title + "</h1><p>" + date + "</p><p>" + content + "</p>" +
                "</body></html>"
            );
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            using (XLWorkbook wb = new XLWorkbook())
            using (IXLWorksheet ws = wb.Worksheets.Add("Sheet1"))
            {
                for (var i = 0; i <= Collection.Count - 1; i++)
                {
                    Activity activity = Collection[i];
                    ws.Cell(i + 1, 1).Value = activity.Date;
                    ws.Cell(i + 1, 2).Value = activity.Name;
                    ws.Cell(i + 1, 3).Value = activity.Title;
                    ws.Cell(i + 1, 4).Value = activity.Content;
                }

                wb.SaveAs("activity.xlsx");
            }
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
