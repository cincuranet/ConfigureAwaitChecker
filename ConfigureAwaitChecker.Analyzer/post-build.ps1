try {
	$version = (gi ConfigureAwaitChecker.dll).VersionInfo.ProductVersion
	echo "Building package with version $version."
	& '..\..\..\packages\NuGet.CommandLine.2.8.5\tools\NuGet.exe' pack ConfigureAwaitChecker.Analyzer.nuspec -Version $version -OutputDirectory .
}
catch {
	exit 1
}