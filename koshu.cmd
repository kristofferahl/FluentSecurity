@echo off
set target="default"
set taskfile="koshufile.ps1"
if not "%1"=="" set target="%1"
if not "%2"=="" set taskfile="%2"
powershell -NoProfile -ExecutionPolicy Bypass .\koshu.ps1 -target %target% -taskfile %taskfile%
pause