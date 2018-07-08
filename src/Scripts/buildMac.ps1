param 
( 
    [String]$solutionPath="$(Split-Path $PSScriptRoot)",
    [String]$solutionName="Acr.UserDialogs.sln" 
)

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
               
       $path = Join-Path $solutionFolder -ChildPath $slnName
       if (!(Test-Path $path)) {
                Write-Host "Solution not found:" $path -foregroundcolor red
                exit 1
        } 
        
        if ($clean) {
            Write-Host "Cleaning $($path)" -foregroundcolor green 
            & msbuild "$($path)" /t:Clean /p:Configuration=Release /m /nologo
            Get-ChildItem (Split-Path $path -Resolve) -include Release,Debug,obj -Recurse | ForEach-Object ($_) { remove-item $_.fullname -Force -Recurse }
        }  
        
        if ($nuget) {
            Write-Host "Restoring NuGet packages" -foregroundcolor green
            & nuget restore "$($path)"
        } 

        Write-Host "Building $($path)" -foregroundcolor green
        & msbuild "$($path)" /t:Build /p:Configuration=Release /m /nologo

        if (! $?) {        
             Write-Host "MSBuild failed" 
             exit 1
         }
    }
}

BuildSolution $solutionPath $solutionName