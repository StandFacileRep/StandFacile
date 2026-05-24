; 16.05.2026
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.16.6 

AppVerName=StandMonitor {#SetupSetting("AppVersion")}
AppName=StandMonitor 2026
AppPublisher=Mauro Artuso
AppId={{180F6795-C8D3-4A65-8A04-7C283C8A2E64}
DefaultDirName={sd}\StandFacile\StandMonitor_516x
DefaultGroupName=StandFacile\StandMonitor_516x\
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
Source: "..\..\StandMonitor\StandAux\config.ini"; DestDir: "{app}"; Flags: uninsneveruninstall confirmoverwrite; Languages: it
Source: "..\StandAux\Licenza.txt"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\FreeDataExports.dll"; DestDir: "{app}"
//Source: "..\exe\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\StandMonitor"; Filename: "{app}\StandMonitor.exe"; WorkingDir: "{app}"
Name: "{group}\Vedi StandMonitor files"; Filename: "{app}"; WorkingDir: "{app}"
Name: "{commondesktop}\StandMonitor"; Filename: "{app}\StandMonitor.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: {app}\StandMonitor.exe; WorkingDir: {app}; Flags: nowait postinstall unchecked
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandMonitor"" dir=in action=allow program=""{app}\StandMonitor.exe"" enable=yes"; Flags: runhidden
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandMonitor"" dir=out action=allow program=""{app}\StandMonitor.exe"" enable=yes"; Flags: runhidden
