PUSHD "."

RMDIR /S /Q Package
MKDIR Package
CD Package

COPY "..\Latenite\Bin\Latenite.exe"
COPY "..\Latenite\Bin\Latenite.exe.config"

MKDIR Projects

MKDIR Compile
COPY "..\Latenite\Bin\Compile\Brass.exe" Compile

MKDIR Plugins
XCOPY "..\Latenite\Bin\Plugins\*.*" "Plugins" /EXCLUDE:..\Exclude /S /Y

MKDIR Templates
XCOPY "..\Latenite\Bin\Templates\*.*" "Templates" /EXCLUDE:..\Exclude /S /Y

MKDIR Help
XCOPY "..\Latenite\Bin\Help\*.*" "Help" /EXCLUDE:..\Exclude /S /Y

MKDIR Tools
XCOPY "..\Latenite\Bin\Tools\*.*" "Tools" /EXCLUDE:..\Exclude /S /Y

MKDIR Debug
MKDIR Debug\PindurTI
XCOPY "..\Latenite\Bin\Debug\PindurTI\*.*" "Debug\PindurTI" /EXCLUDE:..\Exclude /S /Y

POPD