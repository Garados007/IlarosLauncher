var ip = $.Net.Get("http://www.wow.ilaros.de/ip.txt");
if ($.Net.Error != null) {
    $.Props.GameServerIP = "";
    $.Log("Webseite offline");
}
else {
    $.Log("IP: " + ip /*+ " (Ping: " + $.Net.Ping(ip) + "ms)"*/);
    var checkup = $.Net.Get("http://" + ip + "/");
    if ($.Net.Error == null) {
        $.Props.GameServerIP = ip;
        $.Log("Server online");
    }
    else {
        $.Props.GameServerIP = "";
        $.Log("Server offline");
        $.Log($.Net.Error);
    }
}