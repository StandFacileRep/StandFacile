; 16.05.2026
; ricordarsi di mettere in passo la "AppVersion" qui sotto
; tra una versione e l'altra cambiare GUID per facilitare l'installazione distinta
; The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.

[Setup]
AppVersion= 5.16.6 

AppVerName=StandFacile {#SetupSetting("AppVersion")}
AppName=StandFacile 2026
AppPublisher=Mauro Artuso
AppId={{FD0B7D99-DF6A-4B38-B554-A9FC44990F16}
DefaultDirName={sd}\StandFacile\StandFacile_516x
DefaultGroupName=StandFacile\StandFacile_516x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandFacileSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\StandAux\Licenza.txt
AppPublisherURL=http://www.standfacile.org/
SetupIconFile=..\src\Resources\StandFacile.ico
UninstallDisplayIcon={app}\StandFacile.exe
; DisableDirPage=no


[Languages]
Name: it; MessagesFile: compiler:Languages\Italian.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: "..\exe\\Release\StandFacile.exe"; DestDir: "{app}"; Flags: replacesameversion; Languages: it
Source: "..\doc\Manuale_StandFacile.pdf"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.SQLite.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\FreeDataExports.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"
Source: "..\exe\Debug\QRCoder.dll"; DestDir: "{app}"
Source: "..\exe\Debug\x86\sqlite3.dll"; DestDir: "{app}\x86"
Source: "..\exe\Debug\x64\sqlite3.dll"; DestDir: "{app}\x64"
Source: "..\StandAux\Listino.txt"; DestDir: "{app}"; Flags: uninsneveruninstall confirmoverwrite; Languages: it
Source: "..\StandAux\config.ini"; DestDir: "{app}";  Flags: uninsneveruninstall confirmoverwrite; Languages: it
Source: "..\StandAux\Licenza.txt"; DestDir: "{app}"; Languages: it


[Icons]
Name: "{group}\StandFacile"; Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"
Name: "{group}\Vedi StandFacile files"; Filename: "{app}"; WorkingDir: "{app}"
Name: "{commondesktop}\StandFacile"; Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent unchecked; Description: "{cm:LaunchProgram,StandFacile}"
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandFacile"" dir=in action=allow program=""{app}\StandFacile.exe"" enable=yes"; Flags: runhidden
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""StandFacile"" dir=out action=allow program=""{app}\StandFacile.exe"" enable=yes"; Flags: runhidden


