using Gecko;
using System;
using System.IO;
using System.Windows;

namespace 活動記録システム
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var profileDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\活動記録システム";
            var isExistsProfileDir = Directory.Exists(profileDir);
            if (!isExistsProfileDir)
                Directory.CreateDirectory(profileDir);
            Xpcom.ProfileDirectory = profileDir;

            var exePath = Environment.GetCommandLineArgs()[0];
            var dir = exePath.Substring(0, exePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            Xpcom.Initialize(dir + "xulrunner");

            var profilePath = profileDir + "\\活動記録システム.conf";
            if (isExistsProfileDir)
                GeckoPreferences.Load(profilePath);
            else
                InitializeGeckoPreferences(profilePath);

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void InitializeGeckoPreferences(string profilePath)
        {
            GeckoPreferences.User["browser.cache.disk.enable"] = false;
            GeckoPreferences.User["browser.cache.disk.capacity"] = 0;
            GeckoPreferences.Save(profilePath);
        }
    }
}
