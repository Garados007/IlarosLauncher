using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class CreateCompactFile : UpdateStage<CreateCompactFile.CreateCompactFileTask>
    {
        public override string GlobalTaskVerb => "Sicherung:";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<DownloadFile>().Finished && manager.GetStage<CloseWaiter>().Finished;
        }

        public CreateCompactFile()
        {
            Tasks.Add(new CreateCompactFileTask());
        }

        public class CreateCompactFileTask : UpdateTask
        {
            public override void Execute(UpdateManager manager)
            {
                Name = "Fasse Ressourcendateien zusammen";

                //Das System nutzt eine Pointergröße von 3 Byte (Blockweise) und eine Blockgröße
                //von 1024. Somit kann man theoretisch maximal 16GB in der Datei speichern.
                //Bei einer Pointergröße von 2 Byte (Blockweise) und einer Blockgröße von 1024 
                //wären es nur noch theoretische 64 MB (dafür ist der Overhead auch minimal kleiner).

                var ds = DownloadSettings.Current;
                var source = ds.UseTemp ? Environment.ExpandEnvironmentVariables("%TEMP%\\IlarosLauncher\\Downloads\\ClientContent") :
                    ds.LauncherPath + "\\Temp\\Downloads\\ClientContent";
                var target = ds.LauncherPath + "\\Content\\Asset.csf";
                var builder = ds.LauncherPath + "\\Tools\\compacthelper.exe";
                if (!File.Exists(builder)) builder = ds.UseTemp ? 
                        Environment.ExpandEnvironmentVariables("%TEMP%\\IlarosLauncher\\Downloads\\Client\\Tools\\compacthelper.exe") :
                    ds.LauncherPath + "\\Temp\\Downloads\\Client\\Tools\\compacthelper.exe";

                var fi = new FileInfo(target);
                if (!fi.Directory.Exists) fi.Directory.Create();

                File.WriteAllText(source + "\\settings.ini", DownloadSettings.ConfigIni.Export());

                var pr = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = "/D /N /P 3 /B 1024 /R /S \"" + source + "\" /T \"" + target + "\"",
                        CreateNoWindow = true,
                        FileName = builder
                    }
                };
                pr.Start();
                pr.WaitForExit();
                pr.Dispose();

                Progress = 1;
            }
        }
    }
}
