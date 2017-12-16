// topwowserver.de automatic voter

var user = $.User("names", "name[" + $.User("names", "currentId") + "]") || "betauser";
var error = false;
try {
    var code = $.Net.Get("http://www.topwowserver.de/in.php?id=406&user=" + user);
    if ($.Net.Error != null) {
        error = true;
        $.Log($.Net.Error);
    }
}
catch (e) {
    error = true;
    $.Log(e);
}
$.Event("vote", "topwowserver.de", !error);