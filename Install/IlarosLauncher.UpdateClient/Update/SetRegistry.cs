using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace IlarosLauncher.UpdateClient.Update
{
    class SetRegistry : UpdateStage<SetRegistry.RegistryTask>
    {
        public override string GlobalTaskVerb => "Einstellungen:";

        public override bool CanStart(UpdateManager manager)
        {
            return true;
        }

        public SetRegistry()
        {
            Tasks.Add(new RegistryTask());
        }

        public class RegistryTask : UpdateTask
        {
            public override void Execute(UpdateManager manager)
            {
                Name = "Erstelle Registrierungseinträge";
                var ds = DownloadSettings.Current;
                var key = Registry.CurrentUser.OpenSubKey("Software", true);
                var skey = key.OpenSubKey("IlarosLauncher", true);
                if (skey == null) skey = key.CreateSubKey("IlarosLauncher", true);
                skey.SetValue("TempPath", ds.UseTemp ?
                    Environment.ExpandEnvironmentVariables("%TEMP%\\IlarosLauncher") :
                    ds.LauncherPath + "\\Temp");
                skey.SetValue("LauncherPath", ds.LauncherPath);
                skey.SetValue("DataPath", ds.UseAppData ?
                    Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher") :
                    ds.LauncherPath);
                skey.Flush();
                skey.Close();
                key.Close();
                Progress = 1;
            }
        }
    }
}
