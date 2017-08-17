#!/bin/bash

set -e

if [ ! $TRAVIS_TEST_RESULT -eq 0 ]; then
    echo "not deploying because build failed"
    exit 0
fi

BUILD_DIR="CoreBot/bin/Release/netcoreapp2.0/linux-arm/publish"

eval "$(ssh-agent -s)"
ssh-add deploy_key

dotnet publish -c Release -r linux-arm CoreBot
echo "publish done"

ssh -o StrictHostKeyChecking=no -p $CORE_PORT $CORE_SERVER \
    "fuser -s -k -SIGTERM publish/CoreBot || true" &>/dev/null
echo "killed previous"

rsync -chrv -e "ssh -o StrictHostKeyChecking=no -p $CORE_PORT" \
    $BUILD_DIR $CORE_SERVER: &>/dev/null

echo "rsync"

ssh -o StrictHostKeyChecking=no -p $CORE_PORT $CORE_SERVER \
    "tmux new-window 'publish/CoreBot' ||
    tmux new-session -d 'publish/CoreBot'" &>/dev/null

echo "start"
