; 25.04.2025 
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.14.2

AppVerName=StandOrdini {#SetupSetting("AppVersion")}
AppName=StandOrdini 2025
AppPublisher=Mauro Artuso
AppId={{783104EA-9D30-44D9-9C65-F3135DC0A141}
DefaultDirName={sd}\StandFacile\StandOrdini_514x
DefaultGroupName=StandFacile\StandOrdini_514x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandOrdiniSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\..\StandLibrary\Licenza.txt
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
Source: "..\..\StandLibrary\Licenza.txt"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"

[Icons]
Name: {group}\StandOrdini; Filename: {app}\StandOrdini.exe; WorkingDir: {app}
Name: {group}\Vedi StandOrdini files; Filename: {app}; WorkingDir: {app}
Name: {commondesktop}\StandOrdini; Filename: {app}\StandOrdini.exe; WorkingDir: {app}; Tasks: desktopicon


[Run]
Filename: {app}\StandOrdini.exe; Description: {cm:LaunchProgram,StandOrdini}; Flags: nowait postinstall skipifsilent unchecked; WorkingDir: {app}

