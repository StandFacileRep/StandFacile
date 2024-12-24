; 06.12.2024
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.13.1

AppVerName=StandMonitor {#SetupSetting("AppVersion")}
AppName=StandMonitor 2024
AppPublisher=Mauro Artuso
AppId={{684A0063-7D5A-4576-85CB-1135A0BDB14C}
DefaultDirName={sd}\StandFacile\StandMonitor_513x
DefaultGroupName=StandFacile\StandMonitor_513x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandMonitorSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\..\StandLibrary\licenza.txt
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
Source: "..\..\StandLibrary\Licenza.txt"; DestDir: "{app}"
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

