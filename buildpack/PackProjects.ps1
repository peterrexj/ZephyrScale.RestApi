
Set-ExecutionPolicy Bypass

$pjOutput = [IO.Path]::Combine($PSScriptRoot, '..\Output\')
$zephyr = [IO.Path]::Combine($PSScriptRoot, '..\ZephyrScale.RestApi\ZephyrScale.RestApi.csproj')

dotnet pack $zephyr --output $pjOutput