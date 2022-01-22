
$WindowTitle = "Select *.pbix file"
$TmpSuffix = "_tmp"

Add-Type -AssemblyName System.Windows.Forms # Load Forms Assembly
$OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog # Open Class
$OpenFileDialog.Title = $WindowTitle # Define Title
# $OpenFileDialog.InitialDirectory = $InitialDirectory
$Filter = 'Pbix File|*.pbix' # Define Filter
$OpenFileDialog.Filter = $Filter
# $OpenFileDialog.MultiSelect = $true
$OpenFileDialog.ShowHelp = $true    # Without this line the ShowDialog() function may hang depending on system configuration and running from console vs. ISE.
$OpenFileDialog.ShowDialog() | Out-Null

$FilePath= $OpenFileDialog.Filename

# $FilePath = "C:\Devs\pbixunpacker\pbixunpacker\sample\sample.zip"##

## Path-fu
$LastBarPos = $FilePath.lastindexof("\")
$LastDotPos = $FilePath.lastindexof(".")
$BasePath =  $FilePath.substring(0,$LastBarPos)
$ZipPath = $FilePath -replace ".pbix$", ".zip"
$FileName = $FilePath.substring($LastBarPos+1,$LastDotPos - $LastBarPos-1)
$UnzipPath =$BasePath + "\" + $FileName
## Remove destination folder if exists
if (Test-Path $UnzipPath -PathType Container) 
{
    Remove-Item $UnzipPath -Recurse
}

Copy-Item $FilePath $ZipPath # what if exists
$output = try{
    Expand-Archive -Path "$ZipPath" -DestinationPath "$UnzipPath"
}catch{
    $_
}

$(Get-ChildItem $UnzipPath ) |% { 
    $item = $_
    if ($item.PSIsContainer) 
    {
        write-host "Folder: " $item.name 
    } else {
        write-host "File: " $item.name 
    }
}


remove-item $ZipPath # Clean up


 Read-Host


