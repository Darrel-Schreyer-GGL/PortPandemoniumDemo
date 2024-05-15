$projectPathRelative = ".\PortPandemoniumDemo.Cli"
$projectPath = Resolve-Path $projectPathRelative

Push-Location $projectPath

dotnet build

Start-Process PowerShell -ArgumentList "-NoExit", `
	"-Command `$env:InstanceId='0'; dotnet run --no-build"

Pop-Location