(function (ctx) {
	var path = "", targetPath;
	var url = "/dir/";
	var infoCache = {};
	var dirCache = {};
	var winTitle = "WoW Pfad heraussuchen";
	var filter = "all";
	var dirContainer, dirPathBox, dirInfo;
	var finalFunction;

	function getDirInfo(cache, success) {
		if (cache && infoCache[url] != undefined) {
			if (success != undefined) success(data);
		}
		else $.ajax({
			async: true,
			data: {
				filter: filter,
				emptydirs: "false"
			},
			dataType: "json",
			error: function () {
				alert("Bibliotheksfehler > Code: ajax,dirsel,p=" + path);
			},
			method: "GET",
			success: function (data, status, xhr) {
				infoCache[url] = data;
				if (success != undefined) success(data);
			},
			url: url
		});
	}

	//html()
	//html(classNames)                  
	//html(classNames, content)         
	//html(classNames, data, content)   
	//html(tag, classNames, data, content)
	//html(tag, classNames, data, contents...)
	function html(tag, classNames, data, content) {
		switch (arguments.length) {
			case 0: return html('div', '', {}, []);
			case 1: return html('div', tag, {}, []);
			case 2: return html('div', tag, {}, classNames);
			case 3: return html('div', tag, classNames, data);
			case 4:
				{
					var s = '<' + tag + ' class="' + classNames + '"';
					for (var p in data)
						if (data.hasOwnProperty(p))
							s += ' ' + p + '="' + data[p] + '"';
					s += '>';
					for (var c in content)
						s += content[c];
					return s + '</' + tag + '>';
				}
			default: return html(tag, classNames, data, arguments.slice(3));
		}
	}

	//convertAll(list, func(e))
	function convertAll(list, func) {
		if (list == undefined) return [];
		var l = new Array(list.length);
		for (var i = 0; i < list.length; ++i)
			l[i] = func(list[i]);
		return l;
	}

	function buildDirView(url, data) {
		var h = html("dir-view", { "data-url": url },
            (data.parent != undefined ? [
                html("dir-folder dir-button", {
                	"data-url": data.parent,
                    "data-path": data.parentpath
                }, [
                    html("dir-img-box dir-img-box-up"),
                    html("dir-name", [".."])
                ])
            ] : []).concat(
			convertAll(data.dirs, function (e) {
				return html("dir-folder dir-button", {
					"data-url": e.url,
					"data-path": e.path,
					"data-name": e.name,
					"data-created": e.created,
					"data-modified": e.modified
				}, [
					html("dir-img-box dir-img-box-dir"),
					html("dir-name", [e.name])
				]);
			}).concat(convertAll(data.files, function (e) {
				return html("dir-file dir-button", {
					"data-path": e.path,
					"data-name": e.name,
					"data-created": e.created,
					"data-modified": e.modified,
					"data-size": e.size,
					"data-sizet": e.sizet,
					"data-types": JSON.stringify(e.types)
				}, [
					html("dir-img-box dir-img-box-file"),
					html("dir-name", [e.name])]);
			})))
		);
		var cont = $("<div/>");
		cont.addClass("dir-view-container");
		cont.html(h);
		return cont;
	}

	function getDirView(cache, success) {
		if (cache && dirCache[url] != undefined) {
			if (success != undefined) success(dirCache[url]);
		}
		else getDirInfo(cache, function (data) {
			var v = dirCache[url] = buildDirView(url, data);
			if (success != undefined) success(v);
		});
	}

	function updateDirView() {
		dirPathBox.text(path);
		dirContainer.children().remove();
		getDirView(true, function (d) {
			dirContainer.append(d);
			d.find(".dir-button").click(function () {
				var b = $(this);
				if (b.hasClass("dir-file")) {
				    targetPath = b.attr("data-path");
				    dirInfo.find(".dir-info-select-name").text(b.attr("data-name"));
				    dirInfo.find(".dir-content-info-lower").addClass("show");
				}
				else {
					path = b.attr("data-path");
					url = b.attr("data-url");
					dirInfo.find(".dir-info-name").text("");
					dirInfo.find(".dir-info-path").text("");
					dirInfo.find(".dir-content-info-lower").removeClass("show");
					updateDirView();
				}
			}).mouseenter(function () {
				var b = $(this);
				dirInfo.find(".dir-info-name").text(this.hasAttribute("data-name") ? b.attr("data-name") : "Übergeordneter Ordner");
				dirInfo.find(".dir-info-path").text(this.hasAttribute("data-path") ? b.attr("data-path") != "" ? b.attr("data-path") : "Arbeitsplatz" : "");
			});
		});
	}

	function buildWindow() {
	    var h = html("dir-window", [
			html("dir-header", [
				html("dir-header-title", [winTitle]),
				html("dir-header-close", ["X"])
			]),
			html("dir-path-bar", [
				html("dir-path", [path == "" ? "Arbeitsplatz" : path])
			]),
			html("dir-content-box", [
				html("dir-content-view"),
				html("dir-content-info", [
                    html("", [
                        html("dir-content-info-container", [
                            html("dir-content-info-upper", [
					            html("dir-info-name"),
					            html("dir-info-path")
                            ]),
                            html("dir-content-info-lower", [
                                html("dir-info-select-name"),
                                html("dir-info-select-button", ["Auswählen"])
                            ])
                        ])
                    ])
				])
			])
	    ]);
		var d = $("<div/>");
		d.addClass("dir-window-box");
		d.html(h);
		d.appendTo($("body"));
		$($("body")).addClass("dir-visible");
		//events
		d.find(".dir-header-close").click(function () {
			d.remove();
			$($("body")).removeClass("dir-visible");
		});
		d.find(".dir-info-select-button").click(function () {
		    d.remove();
		    $($("body")).removeClass("dir-visible");
		    finalFunction(targetPath);
		});
		//content
		dirContainer = d.find(".dir-content-view");
		dirPathBox = d.find(".dir-path");
		dirInfo = d.find(".dir-content-info");
		updateDirView();
	}

	ctx.showBrowser = function (title, fileFilter, success) {
		winTitle = title;
		filter = fileFilter;
		finalFunction = success || function () { };

		buildWindow();
	};
})(this);