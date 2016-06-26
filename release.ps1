$tempRepoDir = "C:\Temp\stubbery-docs-site"

Write-Output "Generating documentation"
docfx -t .\_exported_templates\default docs/docfx.json

Write-Output "Clear temp directory"
if(Test-Path -Path $tempRepoDir)
{
    Remove-Item -Recurse -Force $tempRepoDir
}

New-Item -ItemType Directory -Force -Path $tempRepoDir

Write-Output "Clone Stubbery repository"
git clone git@github.com:markvincze/Stubbery.git --branch gh-pages $tempRepoDir

Write-Output "Copy documentation into the repository"

Get-ChildItem -Path $tempRepoDir -Recurse |
Select -ExpandProperty FullName |
Where {$_ -notlike $tempRepoDir + "\.git"} |
Remove-Item -Force 

Copy-Item -Force -Recurse ./docs/_site/* -Destination $tempRepoDir

Write-Output "Push the new documentation to the remote"
$gitDir = $tempRepoDir + "\.git"
git --git-dir $gitDir add . -A
git --git-dir $gitDir commit -m "Update generated documentation"
git --git-dir $gitDir push origin gh-pages
