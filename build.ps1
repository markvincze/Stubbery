param (
	[Parameter(Mandatory=$true)]
	[ValidatePattern("^\d+\.\d+\.(?:\d+\.\d+$|\d+$)")]
	[string]
	$ReleaseVersionNumber,
	[Parameter(Mandatory=$true)]
	[string]
	[AllowEmptyString()]
	$PreReleaseName
)

Write-Output "Dotnet CLI version:"
& dotnet --version
Write-Output ""

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName

" PSScriptFilePath = $PSScriptFilePath"

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent

$DOTNET = "dotnet"

# Make sure we don't have a release folder for this version already
$BuildFolder = Join-Path -Path $SolutionRoot -ChildPath "build";
$ReleaseFolder = Join-Path -Path $BuildFolder -ChildPath "Releases\v$ReleaseVersionNumber$PreReleaseName";
if ((Get-Item $ReleaseFolder -ErrorAction SilentlyContinue) -ne $null)
{
	Write-Warning "$ReleaseFolder already exists on your local machine. It will now be deleted."
	Remove-Item $ReleaseFolder -Recurse
}

$ProjectJsonPath = Join-Path -Path $SolutionRoot -ChildPath "src\Stubbery\project.json"

& $DOTNET restore "$ProjectJsonPath"
if (-not $?)
{
	throw "The dotnet restore process returned an error code."
}

& $DOTNET build "$ProjectJsonPath"
if (-not $?)
{
	throw "The dotnet build process returned an error code."
}

& $DOTNET pack "$ProjectJsonPath" --configuration Release --output "$ReleaseFolder"
if (-not $?)
{
	throw "The dotnet pack process returned an error code."
}
