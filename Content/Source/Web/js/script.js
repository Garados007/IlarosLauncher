IL = {
    Backgrounds: [],
    DefaultBackground: "/Web/img/background-default.jpg",
    CurrentBackground: 0,
    SetBackground: function () {
        var image = IL.DefaultBackground;
        if (IL.Backgrounds.length > 0) {
            image = IL.Backgrounds[IL.CurrentBackground];
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
    },
    FetchStatus: function () {
        //$.get("/status/", function (data) {
        //    for (var i = 0; i < data.events.length; ++i) {
        //        var obj = data.events[i].obj;
        //        switch (data.events[i].name) {
        //            case "bg-added": 
        //                IL.Backgrounds.push(obj);
        //                break;
        //        }
        //    }
        //    setTimeout(IL.FetchStatus, 1000);
        //});
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
    //Faltbare Blöcke
    $(".fold-header").click(function () {
        $(this).parent().toggleClass("open");
    });
    //Große Buttons
    $(".big-button.wow").click(function () {
        switch ($(this).attr("data-mode")) {
            case "link-wow": {
                window.showBrowser("WoW Pfad heraussuchen", "wow", function (path) {
                    settings("wow", "path", path.substring(0, path.lastIndexOf("\\")));
                });
            } break;
        }
    });
    //Status abrufen
    IL.FetchStatus();
});