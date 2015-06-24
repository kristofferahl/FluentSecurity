Param(
	[Parameter(Position=0,Mandatory=0)] [string]$target,
	[Parameter(Position=1,Mandatory=0)] [string]$taskFile='koshufile.ps1',
	[Parameter(Position=2,Mandatory=0)] [hashtable]$parameters = @{},
	[Parameter(Position=3,Mandatory=0)] [switch]$load
)

# Ensure errors stops execution
$ErrorActionPreference = 'Stop'

# Restore koshu nuget package
$paths = (1..3) | % { '.' + ('\*' * $_) }
$nuget = (Get-ChildItem -Path $paths -Filter NuGet.exe | Select-Object -First 1)
if ($nuget) { $nuget = $nuget.FullName } else { $nuget = "NuGet.exe" }
try {
	& $nuget install Koshu -version 0.7.0 -outputdirectory ".\packages"
} catch [System.Management.Automation.CommandNotFoundException] {
	throw 'Could not find NuGet.exe and it does not seem to be in your path! Aborting.'
}

$initParameters = $parameters.clone()
if (-not $load) { $initParameters.nologo = $true }

# Initialize koshu
.\packages\Koshu.0.7.0\tools\init.ps1 -parameters $initParameters

if (-not $load) {
	# Trigger koshu
	Invoke-Koshu $taskFile $target $parameters
}