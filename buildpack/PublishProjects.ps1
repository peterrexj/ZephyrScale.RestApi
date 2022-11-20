Set-ExecutionPolicy Bypass

$apiKey = [System.Environment]::GetEnvironmentVariable('NugetApiKey', 'User')
$packageVersion = '.1.0.0.12.nupkg'

$zephyr = [IO.Path]::Combine($PSScriptRoot, '..\ZephyrScale.RestApi\bin\Debug\ZephyrScale.Rest.Sdk' + $packageVersion)


Get-ChildItem -Path $zephyr -ErrorAction Stop


dotnet nuget push $zephyr --api-key $apiKey --source https://api.nuget.org/v3/index.json