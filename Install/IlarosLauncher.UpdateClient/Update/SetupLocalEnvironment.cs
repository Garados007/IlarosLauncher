using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class SetupLocalEnvironment : UpdateStage<SetupLocalEnvironment.SetupEnvironmentTask>
    {
        public override string GlobalTaskVerb => "Einrichten:";

        public override bool CanStart(UpdateManager manager)
        {
            return true;
        }

        public class SetupEnvironmentTask : UpdateTask
        {
            public SetupEnvironmentTask()
            {
                Name = "Richte lokale Umgebung ein";
            }

            public override void Execute(UpdateManager manager)
            {
                Name = "Erstelle Ordner";
                CreateDirs();
                Progress = 0.5f;
                Name = "Erstelle Einstellungen";
                WriterUserSettings();
                Progress = 1;
            }

            void CreateDirs()
            {
                var ds = DownloadSettings.Current;
                CreateDir(ds.LauncherPath);
                CreateDir(ds.LauncherPath + "\\Tools");
                CreateDir(ds.LauncherPath + "\\Content");
                if (ds.UseTemp)
                    CreateDir(Environment.ExpandEnvironmentVariables("%TEMP%\\IlarosLauncher"));
                else CreateDir(ds.LauncherPath + "\\Temp");
                if (ds.UseAppData)
                {
                    CreateDir(Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher"));
                    CreateDir(Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher\\Content"));
                    CreateDir(Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher\\Content\\Images"));
                }
                else CreateDir(ds.LauncherPath + "\\Content\\Images");
            }

            void WriterUserSettings()
            {
                var ds = DownloadSettings.Current;
                var path = ds.UseAppData ? Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher\\settings.ini") :
                    ds.LauncherPath + "\\settings.ini";
                if (!File.Exists(path))
                    File.WriteAllText(path,
                        string.Format("[update]\r\ndownload-backgrounds={0}\r\nsearch-updates={1}\r\ndownload-updates={2}\r\n",
                        ds.LSDownloadBackgrounds ? "true" : "false",
                        ds.LSSearchUpdates ? "true" : "false",
                        ds.LSDownloadUpdates ? "true" : "false"));
            }

            void CreateDir(string path)
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
        }
    }
}
