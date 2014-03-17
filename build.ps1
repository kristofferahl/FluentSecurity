properties {
	$product		= 'FluentSecurity'
	$version		= '2.1.0'
	$label			= ''
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
	$branch			= $null
	$dotCover		= $null
	
	$mygetApiKey = $null
	$nugetApiKey = $null
	
	$copyright		= 'Copyright (c) 2009-2013, Kristoffer Ahl'
	
	$setupMessage	= 'Executed Setup!'
	$cleanMessage	= 'Executed Clean!'
	$compileMessage	= 'Executed Compile!'
	$testMessage	= 'Executed Test!'
	$packMessage	= 'Executed Pack!'
	$deployMessage	= 'Executed Deploy!'
}

task default -depends Run

task Run {
	if ($branch -ne $null) {
		$branch = $branch -replace "refs/heads/",""
	} else {
		try { $branch = (git rev-parse --abbrev-ref HEAD) } catch { $branch = 'unknown' }
	}
	
	$script:buildId			= ([guid]::NewGuid().ToString().Substring(0,5))
	$script:buildVersion	= ?: {$buildNumber -ne $null} {"$version.$buildNumber"} {"$version"}
	$script:buildLabel		= ?: {$label -ne $null -and $label -ne ''} {"$version-$label"} {"$version"}
	
	if ($branch -ne 'master') {
		$script:buildLabel	= ?: {$buildNumber -ne $null} {"$buildLabel-build$buildNumber"} {"$buildLabel-buildx$buildId"}
	}
	
	# SemVer : 2.0.0-alpha.4+build.3 | 2.0.0-alpha.4
	# NuGet  : 2.0.0-alpha4-build3 | 2.0.0-alpha4
	
	# SemVer : 2.0.0+build.3 | 2.0.0
	# NuGet  : 2.0.0-build3 | 2.0.0

	Write-Host "Running build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Label:          $label" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Write-Host "Build label:    $buildLabel" -fore Yellow
	Write-Host "Branch:         $branch" -fore Yellow
	
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
	run_tests $outputDir "*.Specification.dll" $dotCover
	$testMessage
}

task Pack -depends Test {
	create_directory "$artifactsDir\$artifactsName"
	
	$fluentDir = (join-path -path (resolve-path "$artifactsDir\$artifactsName") -childpath "\FluentSecurity")
	
	create_directory $fluentDir
	
	copy-item "$outputDir\FluentSecurity.Core.dll" $fluentDir
	copy-item "$outputDir\FluentSecurity.Mvc.dll" $fluentDir
	copy-item "$outputDir\FluentSecurity.TestHelper.dll" $fluentDir
	copy-item "$rootDir\License.txt" $fluentDir
	copy-item "$rootDir\Readme.md" $fluentDir
	
	get-content "$buildDir\NuGet\FluentSecurity.nuspec" | % { $_ -replace "@CURRENT-VERSION@", $buildLabel -replace "@ARTIFACT-PATH@", $fluentDir } | set-content "$buildDir\FluentSecurity.nuspec"
	get-content "$buildDir\NuGet\FluentSecurity.TestHelper.nuspec" | % { $_ -replace "@CURRENT-VERSION@", $buildLabel -replace "@ARTIFACT-PATH@", $fluentDir } | set-content "$buildDir\FluentSecurity.TestHelper.nuspec"
	
	exec {
		nuget_exe pack "$buildDir\FluentSecurity.nuspec" -OutputDirectory "$artifactsDir\$artifactsName"
		nuget_exe pack "$buildDir\FluentSecurity.TestHelper.nuspec" -OutputDirectory "$artifactsDir\$artifactsName"
	}
	
	$packMessage
}

task Deploy -depends Pack {
	$localFeedDir = "C:\Develop\LocalNugetFeed"
	if (test-path $localFeedDir) {
		copy_files "$artifactsDir\$artifactsName" $localFeedDir "*.nupkg"
	}
	
	$isNightlyBuild = (((get-date) -gt "02:55") -and ((get-date) -lt "03:55"))
	if ($branch -eq 'develop' -and $mygetApiKey -ne $null -and $isNightlyBuild) {
		$feed = 'https://www.myget.org/F/fluentsecurity/api/v2/package'
		$apiKey = $mygetApiKey
	}
	
	if ($branch -eq 'master' -and $nugetApiKey -ne $null) {
		$feed = 'https://nuget.org/'
		$apiKey = $nugetApiKey
	}
	
	if ($feed -ne $null -and $apiKey -ne $null) {
		get-childitem -path "$artifactsDir\$artifactsName" -filter "*.nupkg" | % {
			write-host "Pushing nuget package $_ to $feed" -fore cyan
			nuget_exe push $_.FullName -apikey $apiKey -source $feed
		}
	}
	
	$deployMessage
}

task ? -Description "Help" {
	Write-Documentation
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

function run_tests($source, $include=@("*.dll"), $dotCover) {
	$runner = "$sourceDir\packages\NUnit.Runners.2.6.2\tools\nunit-console-x86.exe"
	if ($dotCover -eq $null) {
		$dotCover = "C:\Program Files (x86)\JetBrains\dotCover\v2.2\Bin\dotcover.exe"
		if (!(test-path $dotCover)) {
			$dotCover = $null
		} else {
			write-host "Using dotCover from $dotCover."
		}
	} else {
		write-host "Using dotCover from TeamCity."
	}
	
	$fullReportsDir = (resolve-path $reportsDir)
	
	$testassemblies = get-childitem $source -recurse -include $include 
	$testassemblies | % {
		$assembly = $_
		$assemblyName = $_.BaseName
		$projectName = $assemblyName -replace ".Specification", ""
		$testReportName = "$projectName-Test-results.xml"
		$coverageReportName = "$projectName-CodeCoverage-results.dcvr"
		
		?: {$dotCover -ne $null} {"Running test in $assembly with code coverage."} {"Running test in $assembly."} | write-host
		exec {
			if ($dotCover -ne $null) {
				& $dotCover cover `
					/TargetExecutable=$runner `
					/TargetArguments="$assembly /framework:net-4.5 /xml=$fullReportsDir\$testReportName" `
					/TargetWorkingDir="$buildDir\Output" `
					/Filters="+:$projectName;-:*.Specification;" `
					/Output="$reportsDir\$coverageReportName"
			} else {
				& $runner $assembly /nologo /framework:net-4.5 /xml="$fullReportsDir\$testReportName"
			}
		}
	}
	
	if ($dotCover -ne $null) {
		write-host "Creating code coverage reports"
		
		$reports = get-childitem -path $reportsDir -filter "*.dcvr" | % { $_.FullName }		
		$reports | % {
			$coverageReport = $_
			$htmlReport = $_ -replace ".dcvr", ".html"
			
			write-host "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='$coverageReport']"
			
			exec {
				& $dotCover report `
					/Source=$coverageReport `
					/Output=$htmlReport `
					/ReportType=HTML
			}
		}
	}
}

function read_xml($file) {
	[xml]$xml = Get-Content $file
	return $xml
}