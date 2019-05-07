# CheckTestFiles

Utility tool for check test files.

Instalation:

dotnet tool install --global CheckTestFiles --version 1.0.2

Requires .net core 2.2
https://dotnet.microsoft.com/download/dotnet-core/2.2

Usage:

checktestfiles [parameters]

Parameters:

Check on folders:
 -c or --check [optional directory path]

Optional args:
 Filter code file types:
     -f or --filter [files filter]
     Default: \.ts*$|\.js*$

 Test files name filter:
     -t or --test [test filter]
     Default: \.spec\.*|\.test\.*

 Exclude directories:
     -ed or --exclude-dir [exclude filter]
     Default: 'node_modules|^\.'

 Include directories:
     -id or --include-dir [include filter]
     Default: '[.]*

 Exclude files:
     -ed or --exclude-files [exclude filter]
     Default: '\.conf\.*'

 Include files:
     -if or --include-files [include filter]
     Default: '[.]*

 Show Only Ok:
     -ok or --ok-only

 Show Only Not found:
     -nf or --not-found-only

 Generate csv file:
     -csv or --csv [output file name: default 'output.csv']
     Default: ''

 Get current dir:
     -env or --env

 Program Version:
     -v or --version


