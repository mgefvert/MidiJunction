@echo off

if ""=="%LIBPATH%" (
    echo LIBPATH missing -- this needs to run from Visual Studio Command Prompt
    goto :error
)

setlocal
set SRC=MidiJunction\bin\Release
set TARGET=Releases\%DATE:-=%

if not exist %SRC% (
    echo Path %SRC% does not exist
    goto :error
)

if exist %TARGET% (
    echo Removing existing release directory
    rd /s /q %TARGET%
)

echo Creating release directory %TARGET%
mkdir %TARGET%
if errorlevel 1 goto :error

echo.
echo = Building target
msbuild MidiJunction.sln /v:minimal /p:Configuration=Release
if errorlevel 1 goto :error

copy %SRC%\MidiJunction.exe* %TARGET%
copy %SRC%\*.dll %TARGET%
copy %SRC%\midi-config.xml %TARGET%

pushd %TARGET%

echo.
echo = Building zip file
7z a -r -y MidiJunction.zip *
if errorlevel 1 (
    popd
    goto :error
)

popd

echo.
echo = OK

goto :eof

:error
echo *** Build failed ***
