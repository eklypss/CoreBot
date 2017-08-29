#!/bin/bash

# Usage
# cd to project root
# run: CoreBot.Scripts/deploy.sh

set -e

BUILD_DIR="CoreBot/bin/Release/netcoreapp2.0/linux-arm/publish"

if [ ! -d $BUILD_DIR ]
then
    echo "Build missing in $BUILD_DIR"
    exit 1
fi

echo "waiting for previous process"
ssh -p $CORE_PORT $CORE_SERVER "systemctl --user stop corebot"
echo "stopped previous bot process"
rsync -chrv -e 'ssh -p '$CORE_PORT $BUILD_DIR $CORE_SERVER:
echo "new files copied"
rm -rf $BUILD_DIR

ssh -t -p $CORE_PORT $CORE_SERVER "systemctl --user start corebot \
    && journalctl -f -o cat --user-unit corebot"
