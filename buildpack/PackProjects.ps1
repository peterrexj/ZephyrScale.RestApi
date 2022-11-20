
Set-ExecutionPolicy Bypass

$zephyr = [IO.Path]::Combine($PSScriptRoot, '..\ZephyrScale.RestApi\ZephyrScale.RestApi.csproj')

dotnet pack $zephyr