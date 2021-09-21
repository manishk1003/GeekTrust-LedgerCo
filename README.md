# GeekTrust-LedgerCo
Solution For Geek Trust's Ledger Co problem statement

To build the solution use below command

```
dotnet build -o geektrust
```
To get the output you can use below commands

```
dotnet geektrust/geektrust.dll Input_1.txt
dotnet geektrust/geektrust.dll Input_2.txt
```

To execute test cases execute below command

```
dotnet test
```
For calculating the test coverage run below command 

```
dotnet test --collect="XPlat Code Coverage"
```

To generate coverage report in html format after generating code coverage report, install Report Generator Tool from https://www.nuget.org/packages/dotnet-reportgenerator-globaltool and then run below command.

```
reportgenerator -reports:"{PathToCodeBase}\Ledger\Ledger.Test\TestResults\{GUID}\coverage.cobertura.xml" -targetdir:"{OutputDirectory}" -reporttypes:Html
```
*PathToCodeBase : Folder where code base is located.*

*GUID: Folder name generated after running code coverage command.*

*OutputDirectory: Folder where you need html reports and supporting files to be placed.*



