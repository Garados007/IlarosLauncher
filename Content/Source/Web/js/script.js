IL = {
    Backgrounds: [],
    DefaultBackground: "/Web/img/background-default.jpg",
    CurrentBackground: 0,
    SetBackground: function () {
        var image = IL.DefaultBackground;
        if (IL.Backgrounds.length > 0) {
            image = "/bg/" + IL.Backgrounds[IL.CurrentBackground];
            IL.CurrentBackground = (IL.CurrentBackground + 1) % IL.Backgrounds.length;
        }
        var cont = $("#background-container");
        if (cont.children().length == 0) {
            $("<div/>")
                .addClass("background-layer")
                .addClass("show")
                .css("background-image", "url(\"" + image + "\")")
                .appendTo(cont);
        }
        else {
            $("<div/>")
                .addClass("background-layer")
                .css("background-image", "url(\"" + image + "\")")
                .appendTo(cont)
                .delay(100)
                .queue(function (param) {
                    $(param).dequeue();
                    $(this).addClass("show");
                })
                .delay(6000)
                .queue(function (param) {
                    $(param).dequeue();
                    $(this).parent().children().eq(0).remove();
                });
        }
    }
};

$(function () {
    //Menüwähler
    $(".link").click(function () {
        var name = $(this).attr("data-name");
        if (name != "website") {
            $(".nav-containers").attr("data-mode", name);
        }
    });
    //Hintergründe
    IL.CurrentBackground = Math.floor(Math.random() * IL.Backgrounds.length);
    IL.SetBackground();
    setInterval(IL.SetBackground, 20000); //20s
    newsEvents.bgdl = (function (old) {
        return function (data) {
            if (old != undefined) old(data);
            if (data.value)
                IL.Backgrounds.push(data.key);
        };
    })(newsEvents.bgdl);
    //Faltbare Blöcke
    $(".fold-header").click(function () {
        $(this).parent().toggleClass("open");
    });
    //Große Buttons
    var wowPath;
    settingsChanged(function (value) {
        wowPath = value;
        if (value != null && $(".big-button.wow").attr("data-mode") == "link-wow")
            $(".big-button.wow").attr("data-mode", "start");
    }, "wow", "path");
    if ((wowPath = settings("wow", "path")) != undefined) {
        if ($(".big-button.wow").attr("data-mode") == "link-wow")
            $(".big-button.wow").attr("data-mode", "start");
    }
    $(".big-button.wow").click(function () {
        switch ($(this).attr("data-mode")) {
            case "link-wow": {
                window.showBrowser("WoW Pfad heraussuchen", "wow", function (path) {
                    settings("wow", "path", path.substring(0, path.lastIndexOf("\\")));
                });
            } break;
        }
    });
});

newsEvents.ip = (function (old) {
    return function (data) {
        if (old != undefined) old(data);
        if (data.value == "") {
            $(".big-button.wow .big-button-low-title").attr("data-mode", "notfound");
        }
        else if (data.value != null) {
            $(".server-ip").html(data.value);
            $(".big-button.wow .big-button-low-title").attr("data-mode", "online");
        }
    };
})(newsEvents.ip);