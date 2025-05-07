; 06.05.2025 
; ricordarsi di mettere in passo la "AppVersion" qui sotto
; tra una versione e l'altra cambiare GUID per facilitare l'installazione distinta
; The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.

[Setup]
AppVersion= 5.14.2

AppVerName=StandFacile {#SetupSetting("AppVersion")}
AppName=StandFacile 2025
AppPublisher=Mauro Artuso
AppId={{CB9EF8A9-4FC0-4B97-B8B5-507C70AA713E}
DefaultDirName={sd}\StandFacile\StandFacile_514x
DefaultGroupName=StandFacile\StandFacile_514x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandFacileSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\..\StandLibrary\Licenza.txt
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
Source: "..\exe\Debug\QRCoder.dll"; DestDir: "{app}"
Source: "..\exe\Debug\x86\sqlite3.dll"; DestDir: "{app}\x86"
Source: "..\exe\Debug\x64\sqlite3.dll"; DestDir: "{app}\x64"
Source: "..\..\StandLibrary\Listino.txt"; DestDir: "{app}"; Flags: uninsneveruninstall confirmoverwrite; Languages: it
Source: "..\..\StandLibrary\config.ini"; DestDir: "{app}";
Source: "..\..\StandLibrary\Licenza.txt"; DestDir: "{app}"; Languages: it


[Icons]
Name: "{group}\StandFacile"; Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"
Name: "{group}\Vedi StandFacile files"; Filename: "{app}"; WorkingDir: "{app}"
Name: "{commondesktop}\StandFacile"; Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\StandFacile.exe"; WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent unchecked; Description: "{cm:LaunchProgram,StandFacile}"
