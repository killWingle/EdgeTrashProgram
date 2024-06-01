Option Explicit
Dim objShell, objFSO, strEdgePath
Set objShell = CreateObject("Shell.Application")
Set objFSO = CreateObject("Scripting.FileSystemObject")

' Microsoft EdgeÇÃÉpÉXÇê›íË
strEdgePath = "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
If Not objFSO.FileExists(strEdgePath) Then
    strEdgePath = "C:\Program Files\Microsoft\Edge\Application\msedge.exe"
End If

If objFSO.FileExists(strEdgePath) Then
    UnpinFromTaskbar strEdgePath
End If

Sub UnpinFromTaskbar(strAppPath)
    Dim objShellApp, objFolder, objItem
    Set objShellApp = CreateObject("Shell.Application")
    Set objFolder = objShellApp.Namespace("shell:AppsFolder")
    
    For Each objItem In objFolder.Items
        If objItem.Path = strAppPath Then
            objItem.InvokeVerb("unpin from taskbar")
            Exit For
        End If
    Next
End Sub
