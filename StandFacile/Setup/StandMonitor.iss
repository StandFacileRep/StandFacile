; 20.07.2025 
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.14.5

AppVerName=StandMonitor {#SetupSetting("AppVersion")}
AppName=StandMonitor 2025
AppPublisher=Mauro Artuso
AppId={{B22AA5A8-5696-485D-B890-F84B3286C6CC}
DefaultDirName={sd}\StandFacile\StandMonitor_514x
DefaultGroupName=StandFacile\StandMonitor_514x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandMonitorSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\StandAux\licenza.txt
AppPublisherURL=http://www.standfacile.org/
SetupIconFile=..\..\StandMonitor\src\Resources\Stand_M.ico
UninstallDisplayIcon={app}\StandMonitor.exe

[Languages]
Name: it; MessagesFile: compiler:Languages\Italian.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: "..\..\StandMonitor\exe\Release\StandMonitor.exe"; DestDir: "{app}"; Flags: replacesameversion
Source: "..\..\StandMonitor\doc\Manuale_StandMonitor.pdf"; DestDir: "{app}"
Source: "..\..\StandMonitor\StandAux\config.ini"; DestDir: "{app}";
Source: "..\StandAux\Licenza.txt"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"
//Source: "..\exe\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\StandMonitor"; Filename: "{app}\StandMonitor.exe"; WorkingDir: "{app}"
Name: "{group}\Vedi StandMonitor files"; Filename: "{app}"; WorkingDir: "{app}"
Name: "{commondesktop}\StandMonitor"; Filename: "{app}\StandMonitor.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: {app}\StandMonitor.exe; WorkingDir: {app}; Flags: nowait postinstall unchecked

