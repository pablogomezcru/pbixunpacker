function ScanFolder {
    param (
        [string] $PathToScan
    )
    write-host -BackgroundColor Yellow "Escaneando $PathToScan"
    Get-ChildItem $PathToScan |% { 
        $item = $_; 
        if ($item.PSIsContainer) 
        {
             ScanFolder -Path $item.FullName 
        } else {
            write-host $item.FullName
            try {
                $converted = get-content $item.FullName -raw | convertfrom-json
                write-host -BackgroundColor Green "Convertido correctamente"
            }
            catch {
                write-host -BackgroundColor Red "No se pudo convertir"
            }
        }
    }
}

### OJO dependencia de powershell >6? Me instalo powershell 7.2.1 de https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2#msi


# $WindowTitle = "Select *.pbix file"
# $TmpSuffix = "_tmp"
# Add-Type -AssemblyName System.Windows.Forms # Load Forms Assembly
# $OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog # Open Class
# $OpenFileDialog.Title = $WindowTitle # Define Title
# # $OpenFileDialog.InitialDirectory = $InitialDirectory
# $Filter = 'Pbix File|*.pbix' # Define Filter
# $OpenFileDialog.Filter = $Filter
# # $OpenFileDialog.MultiSelect = $true
# $OpenFileDialog.ShowHelp = $true    # Without this line the ShowDialog() function may hang depending on system configuration and running from console vs. ISE.
# $OpenFileDialog.ShowDialog() | Out-Null
# $FilePath= $OpenFileDialog.Filename

$FilePath = "C:\Devs\pbixunpacker\pbixunpacker\sample\sample.pbix" ##

## Path-fu
$LastBarPos = $FilePath.lastindexof("\")
$LastDotPos = $FilePath.lastindexof(".")
$BasePath =  $FilePath.substring(0,$LastBarPos)
$ZipPath = $FilePath -replace ".pbix$", ".zip"
$FileName = $FilePath.substring($LastBarPos+1,$LastDotPos - $LastBarPos-1)
$UnzipPath =$BasePath + "\" + $FileName
write-host "Remove destination folder if exists"
if (Test-Path $UnzipPath -PathType Container) 
{
    Remove-Item $UnzipPath -Recurse
}

write-host "Copy zip file"

Copy-Item $FilePath $ZipPath # what if exists

write-host "Expand Archive"

$output = try{
    Expand-Archive -Path "$ZipPath" -DestinationPath "$UnzipPath"
}catch{
    $_
}

write-host "Scan Folder"
ScanFolder -Path $UnzipPath

write-host "Delete Zip"
remove-item $ZipPath # Clean up


#  Read-Host


# ####



