(function (ctx) {
    var token = null;

    var eventHandler = {
        update: function (data) { },
        ip: function (data) { },
        vote: function (data) { },
        bgup: function (data) { },
        bgdl: function (data) { },
        uset: function (data) { } //defined in settings.js
    };

    function loop() {
        $.ajax({
            async: true,
            data: token == null ? {} : {
                token: token
            },
            dataType: "json",
            error: function () {
                alert("Bibliotheksfehler > Code: ajax,news,token=" + token);
            },
            method: "GET",
            success: function (data, status, xhr) {
                token = data.token;
                for (var k in data.events) {
                    var type = data.events[k].type;
                    if (eventHandler[type] == undefined)
                        alert("Bibliotheksfehler > Code: ajax,news,events=" + type);
                    else eventHandler[type](data.events[k]);
                }
            },
            url: "/news/"
        });
    }

    setInterval(loop, 500);
    loop();

    ctx.newsEvents = eventHandler;
})(this);