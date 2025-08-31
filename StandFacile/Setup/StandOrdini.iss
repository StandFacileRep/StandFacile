; 02.08.2025 
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.15.1

AppVerName=StandOrdini {#SetupSetting("AppVersion")}
AppName=StandOrdini 2025
AppPublisher=Mauro Artuso
AppId={{499B36E8-5239-4FAD-8930-D9FF1D8ABA56}
DefaultDirName={sd}\StandFacile\StandOrdini_515x
DefaultGroupName=StandFacile\StandOrdini_515x\
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

