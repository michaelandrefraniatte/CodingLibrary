FOR /L %%A IN (1, 1, 3600000000) DO (
  ipconfig/flushdns
  %windir%\SysWOW64\rundll32.exe advapi32.dll,ProcessIdleTasks
  attrib +S "D:\Call of Duty Black Ops Cold War\*" /s
  attrib +S "D:\Call of Duty Modern Warfare\*" /s
  attrib -R "C:\Users\mic\AppData\Local\Temp\*" /s
  del /q "C:\Users\mic\AppData\Local\Temp\*" /s /q
  attrib +R "C:\Users\mic\AppData\Local\Temp\*" /s
  echo WScript.Sleep^(WScript.Arguments^(0^)^) >"sleep.vbs" && cscript "sleep.vbs" 10000
  cls
)