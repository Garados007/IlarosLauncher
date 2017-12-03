(function (ctx) {
    var cacheData = {};
    var settingsReady = false;
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
        },
        url: "/settings/"
    });

    //settings()
    //settings(group, name)
    //settings(group, name, value)
    ctx.settings = function (group, name, value) {
        if (arguments.length == 0)
            return settingsReady;
        if (arguments.length == 2) {
            if (cacheData[group] != undefined)
                return cacheData[group][name];
            else return undefined;
        }
        else if (arguments.length == 3) {
            var type = typeof value;
            var isFloat = type == "number";
            var isInt = isFloat && Number.isInteger(value);
            var isBool = type == "boolean";
            var isString = type == "string";
            if (!(isFloat || isInt || isBool || isString))
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
                        type: isString ? "string" : isBool ? "bool" :
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
            }
        }
        else throw Error("wrong number of arguments");
    };
})(this);