﻿using System.Windows;
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
        private string @path = "./activity.csv";

        public MainWindow()
        {
            Collection = new ObservableCollection<Activity>();
            InitializeComponent();

            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) { }
            }

            updateHistory();
        }

        private void updateHistory()
        {
            Collection.Clear();
            string csvText;

            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {
                csvText = sr.ReadToEnd();
            }

            using (StringReader reader = new StringReader(csvText))
            {
                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    if (!line.Equals(""))
                    {
                        string[] cells = line.Split(',');
                        Collection.Insert(0, new Activity { Date = cells[0], Title = cells[1], Content = cells[2] });
                    }
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            File.AppendAllText(@path, date.Text + "," + title.Text + "," + content.Text + "\n");
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
            MessageBox.Show("活動記録システム Version 1.0\n対象フレームワーク .NET Framework 4.5.2\n開発者 0918nobita");
        }
    }

    public class Activity
    {
        public string Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
