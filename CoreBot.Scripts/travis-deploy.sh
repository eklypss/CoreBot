#!/bin/bash

set -e

if [ ! $TRAVIS_TEST_RESULT -eq 0 ]; then
    echo "not deploying because build failed"
    exit 1
fi

BUILD_DIR="CoreBot/bin/Release/netcoreapp2.0/linux-arm/publish"

eval "$(ssh-agent -s)"

openssl aes-256-cbc -K $encrypted_a207b69bc07f_key -iv $encrypted_a207b69bc07f_iv \
  -in CoreBot.Scripts/deploy_key.enc -out deploy_key -d

chmod 600 deploy_key
ssh-add deploy_key
rm -f deploy_key

echo "publishing..."
dotnet publish -c Release -r linux-arm CoreBot

echo "killing previous process..."
ssh -o StrictHostKeyChecking=no -p $CORE_PORT $CORE_SERVER \
    "fuser -s -k -SIGTERM publish/CoreBot || true" &>/dev/null

echo "rsyncing new files..."
rsync -chrv -e "ssh -o StrictHostKeyChecking=no -p $CORE_PORT" \
    $BUILD_DIR $CORE_SERVER: &>/dev/null


echo "starting process..."
ssh -o StrictHostKeyChecking=no -p $CORE_PORT $CORE_SERVER \
    "tmux new-window 'publish/CoreBot' ||
    tmux new-session -d 'publish/CoreBot'" &>/dev/null

echo "deploy done"
