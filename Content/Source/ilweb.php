<?php

if (!isset($_GET["mode"])) return;

switch ($_GET["mode"]) {
  case "version": 
    header("Content-Type: text/plain");
	echo "1"; 
	return;

  case "installer":
    $rootPath = realpath('./Update/');
	
	// Initialize archive object
	$zip = new ZipArchive();
	$zip->open('updater.zip', ZipArchive::CREATE | ZipArchive::OVERWRITE);
	
	// Create recursive directory iterator
	/** @var SplFileInfo[] $files */
	$files = new RecursiveIteratorIterator(
	    new RecursiveDirectoryIterator($rootPath),
	    RecursiveIteratorIterator::LEAVES_ONLY
	);
	
	foreach ($files as $name => $file)
	{
	    // Skip directories (they would be added automatically)
	    if (!$file->isDir())
	    {
	        // Get real and relative path for current file
	        $filePath = $file->getRealPath();
	        $relativePath = substr($filePath, strlen($rootPath) + 1);
	
	        // Add current file to archive
	        $zip->addFile($filePath, $relativePath);
	    }
	}
	
	// Zip archive will be created only after closing object
	$zip->close();

	header("Content-Type: application/octet-stream");
	header("Content-Disposition: attachment; filename=installer.zip");
	header("Content-Description: Ilaros Installer");

	readfile("updater.zip");

	unlink("updater.zip");

    return;

  case "changes":
    header("Content-Type: application/json");
	include "version-manager.php";
	VersionsList::checkForChanges();
	if (isset($_GET["version"]) && preg_match("/^[0-9]+(\.[0-9]+)*$/", $_GET["version"]))
		echo json_encode(VersionsList::getGlobalDiff($_GET["version"])->toArray(), JSON_PRETTY_PRINT);
	else echo json_encode(VersionsList::$current->toArray(), JSON_PRETTY_PRINT);
	return;

  case "current-version":
    header("Content-Type: text/plain");
	include "version-manager.php";
	VersionsList::checkForChanges();
	echo VersionsList::$current->version;
	return;

  case "bgcount":
    header("Content-Type: text/plain");
	$count = 0;
	foreach (scandir("Backgrounds") as $e)
		if ($e != "." && $e != "..")
			$count++;
	echo $count;
	return;
	
  case "bglist":
    header("Content-Type: text/plain");
	foreach (scandir("Backgrounds") as $e)
		if ($e != "." && $e != "..")
			echo $e.PHP_EOL;
	return;

  default: return; //unsupported
}