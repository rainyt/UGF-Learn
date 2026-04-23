#!/bin/bash

WORKSPACE=..
LUBAN_DLL=$WORKSPACE/Tools/Luban/Luban.dll
CONF_ROOT=.

dotnet $LUBAN_DLL \
    -t all \
    -d bin \
    -c cs-bin \
    --conf $CONF_ROOT/luban.conf \
    -x outputDataDir=$WORKSPACE/Assets/XlsData \
    -x outputCodeDir=$WORKSPACE/Assets/UnityGameFramework/Scripts/Runtime/LuBan/cfg