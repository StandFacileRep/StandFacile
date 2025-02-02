; 02.02.2025
; ricordarsi di mettere in passo la "AppVersion" qui sotto

[Setup]
AppVersion= 5.13.3 

AppVerName=StandCucina {#SetupSetting("AppVersion")}
AppName=StandCucina 2025
AppPublisher=Mauro Artuso
AppId={{638B59C3-F1E1-4796-BC8C-B71396509D77}
DefaultDirName={sd}\StandFacile\StandCucina_513x
DefaultGroupName=StandFacile\StandCucina_513x\
SourceDir=..\exe
OutputDir=..\Setup
OutputBaseFilename=StandCucinaSetup_{#SetupSetting("AppVersion")}
LicenseFile=..\..\StandLibrary\licenza.txt
AppPublisherURL=http://www.standfacile.org/
SetupIconFile=..\..\StandCucina\src\Resources\Stand_C.ico
UninstallDisplayIcon={app}\StandCucina.exe

[Languages]
Name: it; MessagesFile: compiler:Languages\Italian.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: "..\..\StandCucina\exe\Release\StandCucina.exe"; DestDir: "{app}"; Flags: replacesameversion
Source: "..\..\StandCucina\doc\Manuale_StandCucina.pdf"; DestDir: "{app}"
Source: "..\..\StandLibrary\Licenza.txt"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.MySql.dll"; DestDir: "{app}"
Source: "..\exe\Debug\Devart.Data.PostgreSql.dll"; DestDir: "{app}"

[Icons]
Name: {group}\StandCucina; Filename: {app}\StandCucina.exe; WorkingDir: {app}
Name: {group}\Vedi StandCucina files; Filename: {app}; WorkingDir: {app}
Name: {commondesktop}\StandCucina; Filename: {app}\StandCucina.exe; WorkingDir: {app}; Tasks: desktopicon

[Run]
Filename: {app}\StandCucina.exe; WorkingDir: {app}; Flags: nowait postinstall unchecked
