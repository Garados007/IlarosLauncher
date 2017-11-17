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
mkdir "Web\ClientContent"
copy /Y "Source\compacthelper.exe" "Web\Client\copacthelper.exe"
copy /Y "Source\IlarosLauncher.exe" "Web\Client\IlarosLauncher.exe"
copy /Y "Source\MaxLib.dll" "Web\Client\MaxLib.dll"

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