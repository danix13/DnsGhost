# DnsGhost
Want to run a server on a dynamic IP address? Allows a server to periodically report its IP address. Code to allow clients to find the server.

Install the web application on a static IP address machine
e.g. 
http://ownmeca0.w15.wh-2.com/DnsGhost/

Install the server on the reporting computers and don't forget to modify the DnsGhost.exe.config 
 
 <!-- DONT FORGET TO PUT YOUR OWN USERNAME COMPUTERNAME !!! -->
 <add key="url" value="http://ownmeca0.w15.wh-2.com/DnsGhost/dns/update/username/computername"/>
 
The server installs as a windows service and it will run in the background reporting the IP address of each and every instance

On client use the DnsGhostClient.cs code to obtain the IP address of the machine of interest. Don't forget to populate the app.config or web.config
with the right computer you are looking for
e.g.
<appSettings>
    <add key="dnsGhostUrl" value="http://ownmeca0.w15.wh-2.com/DnsGhost/dns/retrieve/midaed/midaedserver"/>
</appSettings>
