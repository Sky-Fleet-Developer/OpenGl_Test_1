echo Copying fils from %1Resources to %2Resources
xcopy %1Resources %2Resources /E /I /Y
echo Copying %1libs\freeglut.dll to %2
copy %1libs\freeglut.dll %2freeglut.dll