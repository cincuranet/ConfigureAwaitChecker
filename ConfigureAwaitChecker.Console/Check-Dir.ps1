param([Parameter(Mandatory=$True,Position=1)][string]$directory)
Get-ChildItem $directory -Filter *.cs -Recurse | foreach { 
	$fullName = $_.FullName
	Write-Output "Checking $fullName."
	.\ConfigureAwaitChecker.Console.exe $fullName
}