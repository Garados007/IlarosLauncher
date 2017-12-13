var version = $.Props.Version;
var newversion = false; //false = no new version; string = new version number

switch ($.Server(null, "ServerType")) {
    case "ApachePHPV1": {
        var url = $.Server(null, "ServerUrl") + "?mode=changes&version=" + version;
        var changes = $.Net.Get(url);
        if (changes != null) {
            var nv = JSON.parse(changes).version;
            if (nv != version)
                newversion = nv;
        }
    } break;
    default:
        $.Log("ServerType: " + $.Server(null, "ServerType"));
        break;
}

$.Event("update", null, newversion);