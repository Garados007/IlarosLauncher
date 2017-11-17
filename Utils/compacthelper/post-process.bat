
REM Kopiert alle relevanten Dateien in den Ausgabeordner

SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

set curDir=%CD%
set versionChecker=%CD%\..\..\..\bin\%1\Utils\fileinfo.exe


rem ####### Utils package #######
set tarDir=%CD%\..\..\..\bin\%1\Utils

mkdir "%tarDir%"

copy /Y "%curDir%\compacthelper.exe" "%tarDir%\compacthelper.exe"
"%versionChecker%" "%tarDir%\compacthelper.exe" > "%tarDir%\compacthelper.exe.version"


rem ####### Official package #######
set tarDir=%CD%\..\..\..\bin\%1\Web

mkdir "%tarDir%"

copy /Y "%curDir%\compacthelper.exe" "%tarDir%\compacthelper.exe"
"%versionChecker%" "%tarDir%\compacthelper.exe" > "%tarDir%\compacthelper.exe.version"
