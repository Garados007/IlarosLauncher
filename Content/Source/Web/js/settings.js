//dept: news.js

(function (ctx) {
    var cacheData = {};
    var settingsReady = false;
    var groupHandler = {};
    var settingHandler = {};
    var initQueue = [];
    $.ajax({
        async: true,
        data: {},
        dataType: "json",
        error: function () {
            alert("Bibliotheksfehler > Code: ajax,setting");
        },
        method: "GET",
        success: function (data, status, xhr) {
            cacheData = data;
            settingsReady = true;
            for (var g in groupHandler)
                if (data[g] != undefined)
                    for (var k in groupHandler[g])
                        for (var n in data[g])
                            groupHandler[g][k](n, data[g][n]);
            for (var g in settingHandler)
                if (data[g] != undefined)
                    for (var n in settingHandler[g])
                        if (data[g][n] != undefined)
                            for (var k in settingHandler[g][n])
                                settingHandler[g][n][k](data[g][n]);
            for (var k in initQueue)
                initQueue[k]();
        },
        url: "/settings/"
    });

    function onChanged(group, name, value) {
        if (groupHandler[group] != undefined)
            for (var k in groupHandler[group])
                groupHandler[group][k](name, value);
        if (settingHandler[group] != undefined && settingHandler[group][name] != undefined)
            for (var k in settingHandler[group][name])
                settingHandler[group][name][k](value);
    }

    //settings()
    //settings(function)
    //settings(group, name)
    //settings(group, name, value)
    ctx.settings = function (group, name, value) {
        if (arguments.length == 0)
            return settingsReady;
        else if (arguments.length == 1) {
            initQueue.push(group);
            if (settingsReady) group();
        }
        else if (arguments.length == 2) {
            if (cacheData[group] != undefined)
                return cacheData[group][name];
            else return undefined;
        }
        else if (arguments.length == 3) {
            var type = typeof value;
            var isNull = value === null || value === undefined;
            var isFloat = type == "number";
            var isInt = isFloat && Math.round(value) == value;
            var isBool = type == "boolean";
            var isString = type == "string";
            if (!(isFloat || isInt || isBool || isString || isNull))
                throw Error("Wrong value type");
            if (cacheData[group] == undefined)
                cacheData[group] = {};
            if (cacheData[group][name] != value) {
                $.ajax({
                    async: true,
                    data: {
                        group: group,
                        setting: name,
                        value: value,
                        type: isNull ? "null" : isString ? "string" : isBool? "bool" :
                            isInt ? "int" : "float"
                    },
                    dataType: "text",
                    error: function () {
                        alert("Bibliotheksfehler > Code: ajax,setting,group=" +
                            group + ",setting=" + name + ",value=" + value);
                    },
                    method: "POST",
                    success: function (data, status, xhr) {

                    },
                    url: "/settings/"
                });
                cacheData[group][name] = value;
                onChanged(group, name, value);
            }
        }
        else throw Error("wrong number of arguments");
    };

    //settingsChanged(handler(name, value), group)
    //settingsChanged(handler(value), group, name)
    ctx.settingsChanged = function (handler, group, name) {
        if (arguments.length == 2) {
            if (groupHandler[group] == undefined)
                groupHandler[group] = [];
            groupHandler[group].push(handler);
        }
        else if (arguments.length == 3) {
            if (settingHandler[group] == undefined)
                settingHandler[group] = [];
            if (settingHandler[group][name] == undefined)
                settingHandler[group][name] = [];
            settingHandler[group][name].push(handler);
        }
        else throw Error("wrong number of arguments");
    }

    ctx.newsEvents.uset = function (data) {
        var key = JSON.parse(data.key);
        if (cacheData[key[0]] == undefined) cacheData[key[0]] = {};
        cacheData[key[0]][key[1]] = data.value;
        onChanged(key[0], key[1], data.value);
    };
})(this);