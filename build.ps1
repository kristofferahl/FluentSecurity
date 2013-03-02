properties {
	$product		= 'FluentSecurity'
	$version		= '2.0.0'
	$label			= 'alpha4'
	$configuration	= 'release'
	$useVerbose		= $false

	$rootDir		= '.'
	$sourceDir		= "$rootDir"
	$buildDir		= "$rootDir\Build"
	$outputDir		= "$buildDir\Output"
	$reportsDir		= "$buildDir\Reports"
	$artifactsDir	= "$buildDir\Artifacts"	
	$artifactsName	= "$product-$version-$configuration" -replace "\.","_"
	
	$buildNumber	= $null
	
	$copyright		= 'Copyright (c) 2009-2013, Kristoffer Ahl'
	
	$setupMessage	= 'Executed Setup!'
	$cleanMessage	= 'Executed Clean!'
	$compileMessage	= 'Executed Compile!'
	$testMessage	= 'Executed Test!'
	$packMessage	= 'Executed Pack!'
	$deployMessage	= 'Executed Deploy!'
}

task default -depends Local

task Local {
	Write-Host "Running local build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Write-Host "Build label:    $buildLabel" -fore Yellow
	Invoke-Task Deploy
}
task Release {
	Write-Host "Running release build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Write-Host "Build label:    $buildLabel" -fore Yellow
	Invoke-Task Deploy
}

task Setup {
	nuget_exe install ".nuget\packages.config" -outputdirectory "packages"
	generate_assemblyinfo `
		-file "$sourceDir\SharedAssemblyInfo.cs" `
		-description "$product ($configuration)" `
		-company $company `
		-product $product `
		-version $version `
		-buildVersion $buildVersion `
		-buildLabel $buildLabel `
		-clsCompliant "false" `
		-copyright $copyright
	$setupMessage
}

task Clean {
	delete_directory $artifactsDir
	delete_directory $outputDir
	delete_directory $reportsDir
	create_directory $artifactsDir
	create_directory $outputDir
	create_directory $reportsDir
	$cleanMessage
}

task Compile -depends Setup, Clean {
	build_solution "$sourceDir\$product.sln"
	$compileMessage
}

task Test -depends Compile {
	run_tests $outputDir "*.Specification.dll"
	$testMessage
}

task Pack -depends Test {
	create_directory "$artifactsDir\$artifactsName"
	
	$fluentDir = (join-path -path (resolve-path "$artifactsDir\$artifactsName") -childpath "\FluentSecurity")
	
	create_directory $fluentDir
	
	copy-item "$outputDir\FluentSecurity.dll" $fluentDir
	copy-item "$outputDir\FluentSecurity.TestHelper.dll" $fluentDir
	copy-item "$rootDir\License.txt" $fluentDir
	copy-item "$rootDir\Readme.md" $fluentDir
	
	get-content "$buildDir\NuGet\FluentSecurity.nuspec" | % { $_ -replace "@CURRENT-VERSION@", $buildLabel -replace "@ARTIFACT-PATH@", $fluentDir } | set-content "$buildDir\FluentSecurity.nuspec"
	get-content "$buildDir\NuGet\FluentSecurity.TestHelper.nuspec" | % { $_ -replace "@CURRENT-VERSION@", $buildLabel -replace "@ARTIFACT-PATH@", $fluentDir } | set-content "$buildDir\FluentSecurity.TestHelper.nuspec"
	
	nuget_exe pack "$buildDir\FluentSecurity.nuspec" -OutputDirectory "$artifactsDir\$artifactsName"
	nuget_exe pack "$buildDir\FluentSecurity.TestHelper.nuspec" -OutputDirectory "$artifactsDir\$artifactsName"
	
	$packMessage
}

task Deploy -depends Pack {
	$localFeedDir = "C:\Develop\LocalNugetFeed"
	if (test-path $localFeedDir) {
		copy_files "$artifactsDir\$artifactsName" $localFeedDir "*.nupkg"
	}
	$deployMessage
}

task ? -Description "Help" {
	Write-Documentation
}

taskSetup {
	$script:buildVersion	= ?: {$buildNumber -ne $null} {"$version.$buildNumber"} {$version}

	# TODO: Make sure buildLabel do not include build number when building from master
	if ($label -ne $null -and $label -ne '') {
		# SemVer : 2.0.0-alpha.4+build.3 | 2.0.0-alpha.4
		# NuGet  : 2.0.0-alpha4-build3 | 2.0.0-alpha4
		$script:buildLabel		= ?: {$buildNumber -ne $null} {"$version-$label-build$buildNumber"} {"$version-$label"}
	} else {
		# SemVer : 2.0.0+build.3 | 2.0.0
		# NuGet  : 2.0.0-build3 | 2.0.0
		$script:buildLabel		= ?: {$buildNumber -ne $null} {"$version-build$buildNumber"} {"$version"}
	}
}

#------------------------------------------------------------
# Reusable functions
#------------------------------------------------------------

function generate_assemblyinfo {
	param(
		[string]$clsCompliant = "true",
		[string]$description, 
		[string]$company, 
		[string]$product, 
		[string]$copyright, 
		[string]$version,
		[string]$buildVersion,
		[string]$buildLabel,
		[string]$file = $(throw "file is a required parameter.")
	)
	
	$asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute($clsCompliant)]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$buildLabel"")]
[assembly: AssemblyFileVersionAttribute(""$buildVersion"")]"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	
	Write-Host "Generating assembly info file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}

function run_tests($source, $include=@("*.dll")) {
	$testassemblies = get-childitem $source -recurse -include $include 
	$testassemblies | foreach-object {
		$assembly = $_
		$assemblyName = $_.BaseName
		write-host "Running test in $assembly."
		exec {
			& $sourceDir\packages\NUnit.Runners.2.6.2\tools\nunit-console-x86.exe $assembly /nologo /framework:net-4.5 /xml="$reportsDir\$assemblyName-tests-results.xml"
		}
	}
}

function read_xml($file) {
	[xml]$xml = Get-Content $file
	return $xml
}