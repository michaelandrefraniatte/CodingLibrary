FOR /L %%A IN (1, 1, 3600000000) DO (
  timeout /t 1 /nobreak > NUL
  ipconfig/flushdns
  del /q "C:\Users\mic\AppData\Local\Temp\*" /s /q
  echo WScript.Sleep^(WScript.Arguments^(0^)^) >"sleep.vbs" && cscript "sleep.vbs" 10000
  cls
)