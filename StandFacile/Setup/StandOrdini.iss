; 24.04.2026
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.16.6 

AppVerName=StandOrdini {#SetupSetting("AppVersion")}
AppName=StandOrdini 2026
AppPublisher=Mauro Artuso
AppId={{B834DAE6-1D42-4329-A956-6CA2B26D1372}
DefaultDirName={sd}\StandFacile\StandOrdini_516x
DefaultGroupName=StandFacile\StandOrdini_516x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandOrdiniSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\StandAux\Licenza.txt
AppPublisherURL=http://www.standfacile.org/
SetupIconFile=..\..\StandOrdini\src\Resources\Stand_O.ico
UninstallDisplayIcon={app}\StandOrdini.exe

[Languages]
Name: it; MessagesFile: compiler:Languages\Italian.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: "..\..\StandOrdini\exe\Release\StandOrdini.exe"; DestDir: "{app}"; Flags: replacesameversion
Source: "..\..\StandOrdini\doc\Manuale_StandOrdini.pdf"; DestDir: "{app}"
Source: "..\StandAux\Licenza.txt"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"

[Icons]
Name: {group}\StandOrdini; Filename: {app}\StandOrdini.exe; WorkingDir: {app}
Name: {group}\Vedi StandOrdini files; Filename: {app}; WorkingDir: {app}
Name: {commondesktop}\StandOrdini; Filename: {app}\StandOrdini.exe; WorkingDir: {app}; Tasks: desktopicon


[Run]
Filename: {app}\StandOrdini.exe; Description: {cm:LaunchProgram,StandOrdini}; Flags: nowait postinstall skipifsilent unchecked; WorkingDir: {app}
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandOrdini"" dir=in action=allow program=""{app}\StandOrdini.exe"" enable=yes"; Flags: runhidden
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandOrdini"" dir=out action=allow program=""{app}\StandOrdini.exe"" enable=yes"; Flags: runhidden

