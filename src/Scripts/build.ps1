param 
( 
    [String]$solutionPath="$(Split-Path $PSScriptRoot)",
    [String]$solutionName="Acr.UserDialogs.sln" 
)

function Get-MSBuild-Path {

    $vs15key = "HKLM:\SOFTWARE\wow6432node\Microsoft\VisualStudio\SxS\VS7"
    if (Test-Path $vs15key) {
        $key = Get-ItemProperty $vs15key
        $subkey = $key."15.0"
        if ($subkey) {
            return Join-Path $subkey "MSBuild\15.0\bin\amd64\msbuild.exe"
        }
    }

    $vs14key = "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"
    if (Test-Path $vs14key) {
        $key = Get-ItemProperty $vs14key
        $subkey = $key.MSBuildToolsPath
        if ($subkey) {
            return Join-Path $subkey "msbuild.exe"
        }
    }
}

function BuildSolution
{
    param
    (
        [parameter(Mandatory=$true)]
        [String] $solutionFolder,
        
        [parameter(Mandatory=$true)]
        [String] $slnName,

        [parameter(Mandatory=$false)]
        [bool] $nuget = $true,
        
        [parameter(Mandatory=$false)]
        [bool] $clean = $true
    )

    process
    {   
        Clear-Host
        $msBuildExe = Get-MSBuild-Path
        if (!$msBuildExe){
            Write-Host "MsBuild possible location: C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
            $msBuildExe = Read-Host "Enter MsBuild exe path"
            if (!(Test-Path $msBuildExe.Trim())) {
                Write-Host "MSBuild not found:" $msBuildExe -foregroundcolor red
                exit 1
            }            
        }
       
       $path = Join-Path $solutionFolder -ChildPath $slnName
       if (!(Test-Path $path)) {
                Write-Host "Solution not found:" $path -foregroundcolor red
                exit 1
        } 
        
        if ($clean) {
            Write-Host "Cleaning $($path)" -foregroundcolor green 
            & "$($msBuildExe)" "$($path)" /t:Clean /p:Configuration=Release /m /nologo
            Get-ChildItem (Split-Path $path -Resolve) -include Release,Debug,obj -Recurse | ForEach-Object ($_) { remove-item $_.fullname -Force -Recurse }
        }  
        
        if ($nuget) {
            Write-Host "Restoring NuGet packages" -foregroundcolor green
            & "$solutionFolder\Nuget\nuget.exe" restore "$($path)"
        } 

        Write-Host "Building $($path)" -foregroundcolor green
        & "$($msBuildExe)" "$($path)" /t:Build /p:Configuration=Release /m /nologo

        if (! $?) {        
             Write-Host "MSBuild failed" 
             exit 1
         }
    }
}

BuildSolution $solutionPath $solutionName