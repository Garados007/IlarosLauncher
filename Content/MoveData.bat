@echo off
SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

set curDir=%CD%

rem ### \Updater Creation ###
mkdir "Updater Creation"
mkdir "Updater Creation\Content"
copy /Y "Source\IlarosLauncher.UpdaterCreator.exe" "Updater Creation\IlarosLauncher.UpdaterCreator.exe"
copy /Y "Source\DisplayDownload.cs" "Updater Creation\Content\DisplayDownload.cs"
ResGen.exe "Source\DisplayDownload.resx" "Updater Creation\Content\IlarosLauncher.Update.DisplayDownload.resources" /str:cs,IlarosLauncher.Update,DisplayDownload
copy /Y "Source\wow-icon-transparent.ico" "Updater Creation\Content\wow-icon-transparent.ico"

rem ### \Web ###
mkdir "Web"
mkdir "Web\Backgrounds"
mkdir "Web\Client"
mkdir "Web\Client\Tools"
mkdir "Web\ClientContent"
mkdir "Web\ClientContent\Web"
mkdir "Web\ClientContent\Modules"
mkdir "Web\Update"
copy /Y "Source\IlarosLauncher.exe" "Web\Client\IlarosLauncher.exe"
copy /Y "Source\MaxLib.dll" "Web\Client\MaxLib.dll"
copy /Y "Source\compacthelper.exe" "Web\Client\Tools\compacthelper.exe"
copy /Y "Source\fileinfo.exe" "Web\Client\Tools\fileinfo.exe"
copy /Y "Source\MaxLib.dll" "Web\Client\Tools\MaxLib.dll"
copy /Y "mimetypes.ini" "Web\ClientContent\mimetypes.ini"
copy /Y "Source\IlarosLauncher.UpdateClient.exe" "Web\Update\IlarosLauncher.UpdateClient.exe"
copy /Y "Source\License.txt" "Web\Update\License.txt"
copy /Y "Source\MaxLib.dll" "Web\Update\MaxLib.dll"
copy /Y "Source\MaxLib.WinForm.dll" "Web\Update\MaxLib.WinForm.dll"
copy /Y "Source\ilweb.php" "Web\ilweb.php"
copy /Y "Source\version-manager.php" "Web\version-manager.php"
copy /Y "Source\mimetypes.ini" "Web\ClientContent\mimetypes.ini"
xcopy "Source\Web" "Web\ClientContent\Web" /D /E /R /Y
xcopy "Source\Modules" "Web\ClientContent\Modules" /D /E /R /Y

rem Create Version Infos
cd Web
call :fileInfoTree
goto :endFileInfoTree
:fileInfoTree
for %%f in (*.exe, *.dll) do (
	echo %%f
	"%curDir%\Source\fileinfo.exe" %%f > %%f.version
)
for /D %%d in (*) do (
		cd %%d
		call :fileInfoTree
		cd ..
)
exit /b
:endFileInfoTree
cd %curDir%