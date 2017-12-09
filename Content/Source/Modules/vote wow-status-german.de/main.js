// wow-status-german.de automatic voter

var user = "betauser";
var error = false;
try {
    $.Net.Request.Referer = "http://wow.ilaros.de";
    $.Net.Cookie.xxxtopo = "1920919:1436382863";
    $.Net.Request["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0";
    $.Net.Request.Host = "www.wow-status-german.de";
    var code = $.Net.Get("http://www.wow-status-german.de/in.php?id=658&user=" + user);
    if ($.Net.Error != null) {
        error = true;
        $.Log($.Net.Error);
    }
    else {
        $.Net.Request.Referer = "http://www.wow-status-german.de/in.php?id=658&user=" + user;
        code = $.Net.Get("http://www.wow-status-german.de/");
        if ($.Net.Error != null) {
            error = true;
            $.Log($.Net.Error);
        }
    }
}
catch (e) {
    error = true;
    $.Log(e);
}
$.Event("vote", "wow-status-german.de", !error);