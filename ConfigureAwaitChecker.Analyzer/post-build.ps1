try {
	$version = (gi ConfigureAwaitChecker.dll).VersionInfo.ProductVersion
	echo "Building package with version $version."
	& '..\..\..\packages\NuGet.CommandLine.4.3.0\tools\NuGet.exe' pack ConfigureAwaitChecker.Analyzer.nuspec -Version $version -OutputDirectory .
}
catch {
	exit 1
}