$repoDir = (Get-Item -Path ".\" -Verbose).FullName
$tempRepoDir = "C:\Temp\stubbery-docs-site"

Try
{
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
    cd $tempRepoDir
    git rm -r *

    Copy-Item -Force -Recurse ($repoDir + "/docs/_site/*") -Destination $tempRepoDir

    Write-Output "Push the new documentation to the remote"
    git add . -A
    git commit -m "Update generated documentation"
    git push origin gh-pages
}
Finally
{
    cd $repoDir
}
