@echo off
SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

set curDir=%CD%

rem ### \Updater Creation ###
mkdir "Updater Creation"
mkdir "Updater Creation\Content"
copy /Y "Source\IlarosLauncher.UpdaterCreator.exe" "Updater Creation\IlarosLauncher.UpdaterCreator.exe"
copy /Y "Source\DisplayDownload.cs" "Updater Creation\Content\DisplayDownload.cs"

rem ### \Web ###
mkdir "Web"
mkdir "Web\Backgrounds"
mkdir "Web\Client"
mkdir "Web\Client\Tools"
mkdir "Web\ClientContent"
mkdir "Web\Update"
copy /Y "Source\IlarosLauncher.exe" "Web\Client\IlarosLauncher.exe"
copy /Y "Source\MaxLib.dll" "Web\Client\MaxLib.dll"
copy /Y "Source\compacthelper.exe" "Web\Client\Tools\compacthelper.exe"
copy /Y "Source\fileinfo.exe" "Web\Client\Tools\fileinfo.exe"
copy /Y "Source\IlarosLauncher.UpdateClient.exe" "Web\Update\IlarosLauncher.UpdateClient.exe"
copy /Y "Source\License.txt" "Web\Update\License.txt"
copy /Y "Source\MaxLib.dll" "Web\Update\MaxLib.dll"
copy /Y "Source\MaxLib.WinForm.dll" "Web\Update\MaxLib.WinForm.dll"
copy /Y "Source\ilweb.php" "Web\ilweb.php"
copy /Y "Source\mimetypes.ini" "Web\ClientContent\mimetypes.ini"

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