Const ForReading = 1
Const ForWriting = 2
Set WshShell = CreateObject("WScript.Shell")
Dim fInput
fInput = InputBox("LINUX or WINDOWS? (type the answer in CAPITAL LETTERS)")
if fInput = "WINDOWS" then
Set objFSO = CreateObject("Scripting.FileSystemObject")
Set objFile = objFSO.OpenTextFile("Module1.cs", ForReading)

strText = objFile.ReadAll
objFile.Close
strNewText = Replace(strText, "//Do not delete this comment! (You can delete the above comments though)", "#define BUILDTYPE_WINDOWS")
Set objFile = objFSO.OpenTextFile("Module1.cs", ForWriting)
objFile.WriteLine strNewText

objFile.Close
WshShell.Run "%comspec% /K dotnet publish GOOMBAServer.sln --runtime win-x86", 1, True
elseif fInput = "LINUX" then
Set objFSO = CreateObject("Scripting.FileSystemObject")
Set objFile = objFSO.OpenTextFile("Module1.cs",, ForReading)

strText = objFile.ReadAll
objFile.Close
strNewText = Replace(strText, "#define BUILDTYPE_WINDOWS", "//Do not delete this comment! (You can delete the above comments though)")
Set objFile = objFSO.OpenTextFile("Module1.cs", ForWriting)
objFile.WriteLine strNewText

objFile.Close
MsgBox ("%comspec% /K cd """ + WshShell.CurrentDirectory + """")
WshShell.Run "%comspec% /K dotnet publish GOOMBAServer.sln --runtime ubuntu.16.04-x64", 1, True
else
MsgBox("ERROR while compiling")
end if