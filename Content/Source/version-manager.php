<?php

function endsWith($haystack, $needle)
{
    $length = strlen($needle);

    return $length === 0 || 
    (substr($haystack, -$length) === $needle);
}

function toArrayMaker($e) {
	return $e->toArray();
}

function versionComparer($a, $b) {
	$l = min(count($a), count($b));
	for ($i = 0; $i<$l; ++$i)
		if ($a[$i] != $b[$i])
			return $a[$i] - $b[$i];
	return count($a) - count($b);
}

class VersionsList {
	public $modules = array();
	public $ressources = array();
	public $deleted = array();
	public $version = "1.0.0";
	public $date;

	public function __construct() {
		$this->date = time();
	}

	public function loadListFromSystem() {
		$this->modules = array();
		$this->ressources = array();
		$this->deleted = array();
		$this->loadFromDirectory("Client");
		$this->loadFromDirectory("ClientContent");
		$this->loadFromDirectory("Update");
	}

	private function loadFromDirectory($dir) {
		$prefix = $dir == "." ? "" : $dir . "/";
		foreach (scandir($dir) as $e) {
			if ($e == "." || $e == "..") continue;
			$path = $prefix . $e;
			if (is_file($path)) $this->loadFromFile($path, $e);
			if (is_dir($path)) $this->loadFromDirectory($path);
		}
	}

	private function loadFromFile($path, $name) {
		if (endsWith($name, ".version")) return;
		if (is_file($path . ".version")) {
			$e = new VersionModule();
			$e->name = $name;
			$e->path = $path;
			$e->loadVersionInfo();
			$this->modules[] = $e;
		}
		else {
			$e = new VersionRessource();
			$e->name = $name;
			$e->path = $path;
			$e->loadVersionInfo();
			$this->ressources[] = $e;
		}
	}

	public function toArray() {
		$a = array(
			"version" => $this->version,
			"date" => $this->date,
			"modules" => array_map("toArrayMaker", $this->modules),
			"ressources" => array_map("toArrayMaker", $this->ressources)
		);
		if (count($this->deleted) > 0)
			$a["deleted"] = array_map("toArrayMaker", $this->deleted);
		return $a;
	}

	public function load($path) {
		$this->modules = array();
		$this->ressources = array();
		$this->deleted = array();
		$d = json_decode(file_get_contents($path), false);
		$this->version = $d->version;
		$this->date = $d->date;
		foreach ($d->modules as $e) {
			$m = new VersionModule();
			$m->load($e);
			$this->modules[] = $m;
		}
		foreach ($d->modules as $e) {
			$r = new VersionRessource();
			$r->load($e);
			$this->ressources[] = $r;
		}
		if (property_exists($d, "deleted"))
			foreach ($d->deleted as $e) {
				$r = new VersionDeleted();
				$r->load($e);
				$this->ressources[] = $r;
			}
	}

	public function save($name = null) {
		if ($name == null) $name = $this->version;
		if (!file_exists('Versions/'))
			mkdir('Versions/', 0777, true);
		file_put_contents("Versions/" . $name . ".json", 
			json_encode($this->toArray(), JSON_PRETTY_PRINT));
	}

	public function createDiffTo($list) {
		$d = new VersionsList();
		$index = array();
		foreach ($list->modules as $m)
			$index[$m->path] = $m;
		foreach ($list->ressources as $r)
			$index[$r->path] = $r;
		foreach ($this->modules as $e) {
			if (!isset($index[$e->path]) || !$index[$e->path]->equals($e))
				$d->modules[] = $e;
			unset($index[$e->path]);
		}
		foreach ($this->ressources as $e) {
			if (!isset($index[$e->path]) || !$index[$e->path]->equals($e))
				$d->ressources[] = $e;
			unset($index[$e->path]);
		}
		foreach ($index as $e)
			$d->deleted[] = $e->toDeleted();
		return $d;
	}

	public function incrementVersion() {
		$p = explode(".", $this->version);
		$p[count($p)-1] = intval($p[count($p)-1]) + 1;
		$this->version = implode(".", $p);
	}

	public function combineWith($older) {
		$f = array();
		$l = new VersionList();
		foreach ($this->deleted as $e) {
			$f[] = $e->path;
			$l->deleted[] = $e;
		}
		foreach ($this->modules as $e) {
			$f[] = $e->path;
			$l->modules[] = $e->cloneEntry();
		}
		foreach ($this->ressources as $e) {
			$f[] = $e->path;
			$l->ressources[] = $e->cloneEntry();
		}
		foreach ($older->modules as $e)
			if (!in_array($e->path, $f)) {
				$f[] = $e->path;
				$l->modules[] = $e->cloneEntry();
			}
		foreach ($older->ressources as $e)
			if (!in_array($e->path, $f)) {
				$f[] = $e->path;
				$l->ressources[] = $e->cloneEntry();
			}
		foreach ($older->deleted as $e)
			if (!in_array($e->path, $f)) {
				$f[] = $e->path;
				$l->deleted[] = $e->cloneEntry();
			}
		return $l;
	}

	public static $current = null;

	public static function loadCurrent() {
		if (self::$current != null) return;
		self::$current = new VersionList();
		if (is_file("Versions/current.json")) {
			self::$current->load("Versions/current.json");
		}
		else {
			self::$current->loadListFromSystem();
			self::$current->save();
			self::$current->save("current");
		}
	}

	public static function checkForChanges() {
		self::loadCurrent();
		if (time() < self::$current->date + 3600) return false;
		$cur = new VersionList();
		$cur->version = self::$current->version;
		$cur->loadListFromSystem();
		$dif = $cur->createDiffTo(self::$current);
		$changes = false;
		if (count($dif->modules) + count($dif->ressources) + count($dif->deleted) != 0) {
			$cur->incrementVersion();
			$dif->version = $cur->version;
			$dif->save();
			$changes = true;
		}
		$cur->save("current");
		self::$current = $cur;
		return $changes;
	}

	public static function getGlobalDiff($fromVersion) {
		$vers = self::getVersionList();
		$v = array();
		foreach ($f as $e) $v[] = implode(".", $e);
		$index = array_search($fromVersion, $v);
		if ($index === false) {
			$t = explode(".", $fromVersion);
			foreach ($t as &$n)
				$n = intval($n);
			$vers[] = $t;
			usort($vers, "versionComparer");
			$v = array();
			foreach ($f as $e) $v[] = implode(".", $e);
			$index = array_search($fromVersion, $v);
		}
		$l = new VersionList();
		for ($i = $index; $i<count($v); ++$i) {
			$path = "Versions/" . $v[$i] . ".json";
			if (!file_exists($path)) continue;
			$n = new VersionList();
			$n->load($path);
			$l = $n->combineWith($l);
		}
		$l->version = self::$current->version;
		return $l;
	}

	private static function getVersionList() {
		if (!file_exists('Versions/')) return array();
		$f = array();
		foreach (scandir('Versions/') as $e) {
			if ($e == "." || $e == ".." || $e == "current.json" || !is_file($e) || !endsWith($e, ".json")) continue;
			$v = explode(".", $e);
			unset($v[count($v) - 1]);
			foreach ($v as &$n)
				$n = intval($n);
			$f[] = $v;
		}
		usort($f, "versionComparer");
		return $f;
	}
}

class VersionEntry {
	public $path;
	public $name;

	//Überprüft, ob die genannte Versionsnummer mit der im Dateisystem übereinstimmt.
	public function isCurrent() {
		return false;
	}

	//aktualisiert die Versionsinformationen anhand der Datei
	public function loadVersionInfo() {}

	public function toDeleted() {
		$d = new VersionDeleted();
		$d->path = $this->path;
		$d->name = $this->name;
		return $d;
	}

	public function toArray() {
		return array(
			"path" => $this->path,
			"name" => $this->name
		);
	}

	public function load($data) {
		$this->path = $data->path;
		$this->name = $data-name;
	}

	public function equals($entrys) {
		return $this->path == $entrys->path && $this->name == $entrys->name;
	}

	public function cloneEntry() {
		return null;
	}
}

class VersionModule extends VersionEntry {
	public $version;

	public function isCurrent() {
		return $this->version == trim(file_get_contents($this->path . ".version"));
	}

	public function loadVersionInfo() {
		$this->version = trim(file_get_contents($this->path . ".version"));
	}

	public function toArray() {
		$a = parent::toArray();
		$a["version"] = $this->version;
		return $a;
	}

	public function load($data) {
		parent::load($data);
		$this->version = $data->version;
	}

	public function equals($entrys) {
		return parent::equals($entrys) && $this == $entrys && $this->version == $entrys->version;
	}

	public function cloneEntry() {
		$e = new VersionModule();
		$e->path = $this->path;
		$e->name = $this->name;
		$e->version = $this->version;
		return $e;
	}
}

class VersionRessource extends VersionEntry {
	public $hash;

	public function isCurrent() {
		return $this->hash == hash_file("md5", $this->path);
	}

	public function loadVersionInfo() {
		$this->hash = hash_file("md5", $this->path);
	}

	public function toArray() {
		$a = parent::toArray();
		$a["hash"] = $this->hash;
		return $a;
	}
	
	public function load($data) {
		parent::load($data);
		$this->hash = $data->hash;
	}

	public function equals($entrys) {
		return parent::equals($entrys) && $this == $entrys && $this->hash == $entrys->hash;
	}

	public function cloneEntry() {
		$e = new VersionRessource();
		$e->path = $this->path;
		$e->name = $this->name;
		$e->hash = $this->hash;
		return $e;
	}
}

class VersionDeleted extends VersionEntry {

	public function cloneEntry() {
		$e = new VersionDeleted();
		$e->path = $this->path;
		$e->name = $this->name;
		return $e;
	}
}