cd /
FOR /L %%A IN (1, 1, 3600) DO (
  timeout /t 1 /nobreak > NUL
  %windir%\SysWOW64\rundll32.exe advapi32.dll,ProcessIdleTasks
  ipconfig/flushdns
  attrib +R "C:\Users\mic\Documents\Call of Duty Modern Warfare\players\*.*" /s
  del /q "C:\Users\mic\AppData\Local\Temp\*"
  FOR /D %%p IN ("C:\Users\mic\AppData\Local\Temp\*.*") DO rmdir "%%p" /s /q
  sc query http | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop http /y"
  sc query BluetoothUserService_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop BluetoothUserService_41dd8c2 /y"
  sc query CDPUserSvc_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop CDPUserSvc_41dd8c2 /y"
  sc query WpnUserService_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop WpnUserService_41dd8c2 /y"
  sc query cbdhsvc_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop cbdhsvc_41dd8c2 /y"
  sc query OneSyncSvc_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop OneSyncSvc_41dd8c2 /y"
  sc query UserDataSvc_41dd8c2 | find /i "RUNNING"
  IF NOT ERRORLEVEL 1 RUNAS /user:mic /savecred "net stop UserDataSvc_41dd8c2 /y"
)