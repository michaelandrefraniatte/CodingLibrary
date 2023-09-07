powershell -command "attrib -S '%LOCALAPPDATA%\CrashBandicoot4\Saved\SaveGames\*' /s"
powershell -command "attrib -S '%LOCALAPPDATA%\CrashBandicoot4\Saved\SaveGames\*' /d /s"
powershell -command "attrib -S '%LOCALAPPDATA%\CrashBandicoot4\Saved\SaveGames' /d /s"
powershell -command "Start-Sleep -s 2"