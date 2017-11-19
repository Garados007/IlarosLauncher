<?php

if (!isset($_GET["mode"])) return;

switch ($_GET["mode"]) {
  case "version": echo "0"; return;
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

	header("Content-type: application/octet-stream");
	header("Content-Disposition: attachment; filename=installer.zip");
	header("Content-Description: Ilaros Installer");

	readfile("updater.zip");

	unlink("updater.zip");

    return;

  default: return; //unsupported
}