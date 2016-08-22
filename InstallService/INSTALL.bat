echo off
installutil DnsGhost.exe

NET START DnsGhost

echo -------------------------------------------------  
echo "Installation complete! Press a key to exit..."
PAUSE