Param(
	[Parameter(Position=0,Mandatory=0)] [string]$buildFile,
	[Parameter(Position=1,Mandatory=0)] [string]$target,
	[Parameter(Position=2,Mandatory=0)] [hashtable]$parameters = @{},
	[Parameter(Position=3,Mandatory=0)] [switch]$load
)

# Ensure errors fail the build
$ErrorActionPreference = 'Stop'

# Restore koshu nuget package
$nuget = (Get-ChildItem -Path . -Filter NuGet.exe -Recurse | Select-Object -First 1)
if ($nuget) { $nuget = $nuget.FullName } else { $nuget = "NuGet.exe" }
try {
	& $nuget install Koshu -version 0.6.2 -outputdirectory ".\Packages"
} catch [System.Management.Automation.CommandNotFoundException] {
	throw 'Could not find NuGet.exe and it does not seem to be in your path! Aborting build.'
}

$initParameters = $parameters.clone()
if (-not $load) { $initParameters.nologo = $true }

# Initialize koshu
.\Packages\Koshu.0.6.2\tools\init.ps1 -parameters $initParameters

if (-not $load) {
	# Trigger koshu
	Koshu-Build $buildFile $target $parameters
}