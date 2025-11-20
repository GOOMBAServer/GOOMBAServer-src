# Compile.ps1

$fInput = Read-Host "LINUX or WINDOWS? (type the answer in CAPITAL LETTERS)"
$filePath = "Module1.cs"
if ($fInput -eq "WINDOWS") {
    $text = Get-Content $filePath -Raw
    $newText = $text -replace "//Do not delete this comment! \(You can delete the above comments though\)", "#define BUILDTYPE_WINDOWS"
    Set-Content -Path $filePath -Value $newText
    dotnet publish GOOMBAServer.sln --runtime win-x86
}
elseif ($fInput -eq "LINUX") {
    $text = Get-Content $filePath -Raw
    $newText = $text -replace "#define BUILDTYPE_WINDOWS", "//Do not delete this comment! (You can delete the above comments though)"
    Set-Content -Path $filePath -Value $newText
    dotnet publish GOOMBAServer.sln --runtime ubuntu.16.04-x64
}
else {
    Write-Host "ERROR while compiling"
}
