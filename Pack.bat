@echo off
nuget pack TeramFramework.nuspec

for %%a in (*.nupkg) do nuget.exe push %%a Teram@Team2020 -Source http://192.168.0.166:9090/nuget
for %%a in (*.nupkg) do del %%a


set /p DUMMY=Hit ENTER to continue...

