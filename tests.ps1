$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName

" PSScriptFilePath = $PSScriptFilePath"

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent
$TestsFolder = Join-Path -Path $SolutionRoot -ChildPath "test/Stubbery.IntegrationTests";

$DOTNET = "dotnet"

& $DOTNET restore "$TestsFolder"
if (-not $?)
{
	throw "The dotnet restore process returned an error code."
}

& $DOTNET build "$TestsFolder"
if (-not $?)
{
	throw "The dotnet build process returned an error code."
}

# run them
& $DOTNET test "$TestsFolder"
if (-not $?)
{
	throw "The dotnet test process returned an error code."
}
