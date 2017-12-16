// wow-portal.eu automatic voter

var user = $.User("names", "name[" + $.User("names", "currentId") + "]") || "betauser";
var error = false;
try {
    var code = $.Net.Get("http://wow-portal.eu/toplist.php?mode=in&id=30302");
    if ($.Net.Error == null) {
        var ind = code.indexOf("include.php?mode=votetoplist&id=30302&randomdigit=");
        var ind2 = code.indexOf("\\", ind);
        code = code.substring(ind, ind2);
        $.Net.Get("http://wow-portal.eu/" + code);
    }
    if ($.Net.Error != null) {
        error = true;
        $.Log($.Net.Error);
    }
}
catch (e) {
    error = true;
    $.Log(e);
}
$.Event("vote", "wow-portal.eu", !error);