# Tests

There are three different test options:
```bash
dotnet test
```
(builds sync with build-in cryptographic features)


```bash
dotnet test /p:DefineConstants="ASYNC_WITH_CUSTOM"
```
(builds async without cryptographic features)