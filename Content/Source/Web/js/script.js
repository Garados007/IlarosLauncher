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
    },
    Account: (function () {
        var validEmail = function (email) {
            var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return regex.test(email);
        };
        var validUsername = function (name) {
            var regex = /^[a-zA-Z0-9]{3,}$/;
            return regex.test(name);
        };
        var validPassword = function (pw) {
            var regex = /^([a-zA-Z0-9!"#\$\% ,\.]){8,16}$/;
            return regex.test(pw);
        };
        var checkForErrors = function () {
            var eb = $(".in-err");
            eb.html("");
            var error = false;
            if (!validUsername($("#accountname").val())) {
                eb.eq(0).html("Benutzername ist ungültig. Er darf nur aus Buchstaben und Ziffern bestehen.");
                error = true;
            }
            if (!validPassword($("#password").val())) {
                eb.eq(1).html("Das Passwort ist ungültig. Es darf nur aus Buchstaben, Zahlen und den Sonderzeichen !, \", #, $, %, (Leerzeichen), (Komma), . bestehen und muss zwischen 8 und 16 Zeichen lang sein.");
                error = true;
            }
            if ($("#password").val() != $("#password2").val()) {
                eb.eq(2).html("Die Passwörter stimmen nicht überein.");
                error = true;
            }
            if (!validEmail($("#email").val())) {
                eb.eq(3).html("Die Emailadresse ist ungültig.");
                error = true;
            }
            return !error;
        };
        return {
            checkForErrors: checkForErrors,
            createAccount: function () {
                if ($(".big-button.wow .big-button-low-title").attr("data-mode") != "online") {
                    alert("Die IP vom Server konnte noch nicht abgerufen werden, oder er ist noch offline. Bitte versuchen Sie es gleich erneut.");
                    return;
                }
                if (checkForErrors()) {
                    var data = {
                        reg: "",
                        accountname: $("#accountname").val(),
                        password: $("#password").val(),
                        password2: $("#password2").val(),
                        email: $("#email").val(),
                        expansion: $("#expansion").val(),
                        submit: $("#nb-anmeldung input[type=\"submit\"]").val()
                    };
                    $.ajax({
                        url: "http://" + $(".server-ip").html() + "/index.php",
                        cache: false,
                        data: data,
                        type: "POST",
                        dataType: "html",
                        success: function (html) {
                            alert("Account wurde erstellt");
                        },
                        error: function () {
                            alert("Bibliotheksfehler > Code: ajax,create-account");
                        }
                    })
                }
            }
        };
    })()
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
            case "start": {
                if ($(".big-button.wow .big-button-low-title").attr("data-mode") == "online")
                    $.ajax({
                        async: true,
                        data: null,
                        dataType: "text",
                        error: function () {
                            alert("Bibliotheksfehler > Code: ajax,run-wow");
                        },
                        method: "GET",
                        success: function (data, status, xhr) {

                        },
                        url: "/run-wow/"
                    });
            } break;
        }
    });
    //Klickbare Links
    var user = "betauser";
    $(".ext-url").click(function () {
        var url = $(this).attr("data-url");
        if ($(this).hasClass("ext-user"))
            url = url.replace(/\{0\}/, user);
        $.ajax({
            async: true,
            data: {
                url: url
            },
            dataType: "text",
            error: function () {
                alert("Bibliotheksfehler > Code: ajax,run-url,url=" + url);
            },
            method: "GET",
            success: function (data, status, xhr) {

            },
            url: "/run-url/"
        })
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