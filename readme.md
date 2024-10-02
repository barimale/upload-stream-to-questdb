# upload-stream-questdb 
## Prereqs:
```
docker run -p 9000:9000 -p 9009:9009 -p 8812:8812 -p 9003:9003 questdb/questdb:8.1.1
```

## Antivirus(optional, implementation has to be modified):
```
dotnet add package nClam --version 9.0.0
https://www.clamav.net/downloads#otherversions
```
## ToDos
support zip file.
https://learn.microsoft.com/pl-pl/dotnet/api/system.io.compression.gzipstream?view=net-8.0
