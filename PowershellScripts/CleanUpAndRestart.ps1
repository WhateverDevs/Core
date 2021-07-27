param(
    [string]$UnityPath
)

if ($UnityPath)
{
    Write-Host "Checking Rider..."
    $RiderProcess = Get-Process "rider64" -ErrorAction SilentlyContinue
    if ($RiderProcess)
    {
        Write-Host "Attempting to close Rider..."
        # try gracefully first
        $RiderProcess.CloseMainWindow()
        # kill after five seconds
        Sleep 5
        if (!$RiderProcess.HasExited)
        {
            Write-Host "Rider doesn't want to close, killing process..."
            $RiderProcess | Stop-Process -Force
        }

        Write-Host "Closed Rider."
    }
    else
    {
        Write-Host "Rider is closed."
    }
    Remove-Variable RiderProcess

    Write-Host "Checking Unity..."
    $UnityProcess = Get-Process "Unity" -ErrorAction SilentlyContinue
    if ($UnityProcess)
    {
        Write-Host "Attempting to close Unity..."
        # try gracefully first
        $UnityProcess.CloseMainWindow()
        # kill after five seconds
        Sleep 5
        if (!$UnityProcess.HasExited)
        {
            Write-Host "Unity doesn't want to close, killing process..."
            $UnityProcess | Stop-Process -Force
        }

        Write-Host "Closed Unity."
    }
    else
    {
        Write-Host "Unity is closed."
    }
    Remove-Variable UnityProcess

    $folder = "Library"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = "Temp"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = "Build"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = "Bundles"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = ".idea"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = "Logs"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }

    $folder = "obj"
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        Remove-Item $folder -Recurse;
    }
    
    $extension = "sln"
    Write-Host "Deleting $extension files..."
    del "*.$extension"

    $extension = "csproj"
    Write-Host "Deleting $extension files..."
    del "*.$extension"

    Write-Host "Reopening Unity..."
    $ProjectPath = Get-Location
    $Expression = "& '$UnityPath' -projectPath $ProjectPath"
    Invoke-Expression $Expression
    Write-Host "This console will close in 5 seconds..."
    Sleep 5
}
else
{
    Write-Host "Unity path has not been set! Have you run this script from the Unity editor menu?"
    Write-Host "Press any key to close."
    $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}