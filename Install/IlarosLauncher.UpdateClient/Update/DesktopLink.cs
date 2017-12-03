using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace IlarosLauncher.UpdateClient.Update
{
    class DesktopLink : UpdateStage<DesktopLink.DesktopLinkTask>
    {
        public override string GlobalTaskVerb => "Verknüpfe: ";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<ManageFiles>().Finished;
        }

        public DesktopLink()
        {
            if (DownloadSettings.Current.CreateDesktopLink)
                Tasks.Add(new DesktopLinkTask());
        }

        public class DesktopLinkTask : UpdateTask
        {

            public DesktopLinkTask()
            {
                Name = "Erstelle Desktopverknüpfung";
            }
            public override void Execute(UpdateManager manager)
            {
                if (!DownloadSettings.Current.CreateDesktopLink) return;
                var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Ilaros Launcher.lnk";
                var shell = new WshShell();
                var shortcut = (IWshShortcut)shell.CreateShortcut(path);
                shortcut.Description = "Dein Tor zur Ilaros WoW Welt";
                shortcut.IconLocation = DownloadSettings.Current.LauncherPath + "\\IlarosLauncher.exe,0";
                shortcut.TargetPath = DownloadSettings.Current.LauncherPath + "\\IlarosLauncher.exe";
                shortcut.WorkingDirectory = DownloadSettings.Current.LauncherPath;
                shortcut.Save();
                Progress = 1;
            }
        }
    }
}
