"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o retractsolution -name "CA.WorkFlow.UI.PM.wsp" -immediate
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o execadmsvcjobs
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o deletesolution -name "CA.WorkFlow.UI.PM.wsp"
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o execadmsvcjobs
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o addsolution -filename "CA.WorkFlow.UI.PM.wsp"
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o execadmsvcjobs
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o deploysolution -name "CA.WorkFlow.UI.PM.wsp" -immediate -allowGacDeployment -allowCasPolicies -force
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe" -o execadmsvcjobs
