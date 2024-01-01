Write-Host "[MCIO] Install ReportGenerator CLI Tool"
$reportgeneratorFolderPath = "./.reportgenerator/reportgenerator"

if (-not (Test-Path $reportgeneratorFolderPath -PathType Container))
{
    New-Item -Path $reportgeneratorFolderPath -ItemType Directory
    dotnet tool update dotnet-reportgenerator-globaltool --tool-path $reportgeneratorFolderPath
}

Write-Host "[MCIO] Install stryker CLI Tool"
$strykerfolderPath = "./.stryker/stryker"
if (-not (Test-Path $strykerfolderPath -PathType Container))
{
    New-Item -Path $strykerfolderPath -ItemType Directory
    dotnet tool update dotnet-stryker --tool-path $strykerfolderPath
}

Write-Host "[MCIO] Restore"
dotnet restore

Write-Host "[MCIO] Build Release"
dotnet build -c Release --no-restore

Write-Host "[MCIO] Run Unit Tests"
dotnet test -c Release --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="Coverage/" ./tst/UnitTests/MCIO.OutputEnvelop.UnitTests.csproj

$currentLocation = Get-Location
Set-Location ./tst/UnitTests

Write-Host "[MCIO] Run MutationTest"
$result = ../../.stryker/stryker/dotnet-stryker --reporter "html" --output ".stryker-output"
Set-Location $currentLocation

$result

if ($result -like "*The final mutation score is 100.00 %*") {
    Write-Host "[MCIO] Stryker mutant check passed. No surviving mutants found."
} else {
    Write-Error "[MCIO] Stryker detected surviving mutants. Pipeline failed."
}

Write-Host "[MCIO] Generate Report"
./.reportgenerator/reportgenerator/reportgenerator.exe "-reports:./tst/UnitTests/Coverage/coverage.opencover.xml" "-targetdir:./tst/UnitTests/Coverage/"

Start-Sleep -Seconds 1

Write-Host "[MCIO] Open Mutation Report"
./tst/UnitTests/.stryker-output/reports/mutation-report.html

Start-Sleep -Seconds 1

Write-Host "[MCIO] Open Report"
./tst/UnitTests/Coverage/index.html
