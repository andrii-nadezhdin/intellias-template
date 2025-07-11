Some demo commands:



winget install -e --id Microsoft.NuGet 



nuget pack .\\src\\intellias-api.nuspec -OutputDirectory .\\nupkgs



dotnet new -i .\\nupkgs\\intellias-api.1.0.4.nupkg



dotnet new list intellias



dotnet new search intellias



dotnet new uninstall intellias-api



dotnet new intellias-api --name Intellias.MyNewTemplateService --distributed-cache Redis --docker true --IIS false --sqlserver true



