#!/bin/sh
set -e

export VSINSTALLDIR="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community"
export VisualStudioVersion="16.0"

docfx ./docs/docfx.json

SOURCE_DIR=$PWD
TEMP_REPO_DIR=$PWD/../my-project-gh-pages

echo "Removing temporary doc directory $TEMP_REPO_DIR"
rm -rf $TEMP_REPO_DIR
mkdir $TEMP_REPO_DIR

echo "Cloning the repo with the gh-pages branch"
git clone https://github.com/markvincze/Stubbery --branch gh-pages $TEMP_REPO_DIR

echo "Clear repo directory"
cd $TEMP_REPO_DIR
git rm -r *

echo "Copy documentation into the repo"
cp -r $SOURCE_DIR/docs/_site/* .

if [ "$APPVEYOR_REPO_BRANCH" == "master" ]
then
    echo "Push the new docs to the remote branch"
    git add . -A
    git commit --allow-empty -m "Update generated documentation"
    git push origin gh-pages
else
    echo "Not on master, skipping pushing docs"
fi