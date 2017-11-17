
REM Kopiert alle relevanten Dateien in den Ausgabeordner

SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

set curDir=%CD%
set tarDir=%CD%\..\..\..\bin\%1\Utils

mkdir "%tarDir%"

rem Dateien kopieren

copy /Y "%curDir%\fileinfo.exe" "%tarDir%\fileinfo.exe"