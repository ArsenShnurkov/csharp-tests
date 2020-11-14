#!/bin/bash

BINDIR="./bin"
if [ ! -d "${BINDIR}" ] 
then
	mkdir -p "${BINDIR}"
fi

/usr/bin/msbuild -p:Project=typical_project.csproj ./do_patching.proj
