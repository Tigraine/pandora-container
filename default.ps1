# This script was derived from the Rhino.Mocks buildscript written by Ayende Rahien.
include .\psake_ext.ps1

properties {
    $config = 'debug'
    $showtestresult = $FALSE
    $base_dir = resolve-path .
    $lib_dir = "$base_dir\lib\"
    $build_dir = "$base_dir\build\" 
    $release_dir = "$base_dir\release\"
    $source_dir = "$base_dir\src"
	$version = Get-Git-Version
}

task default -depends Release

task Clean {
    remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
    remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
}

task Init -depends Clean {
    Write-Host $version
    Generate-Assembly-Info `
		-file "$source_dir\Pandora\Properties\AssemblyInfo.cs" `
		-title "Pandora Container $version" `
		-description "Lightweight .NET IoC Container" `
		-company "Tigraine" `
		-product "Pandora Container" `
		-version $version `
		-copyright "Copyright © Daniel Hölbling 2009"
    Generate-Assembly-Info `
		-file "$source_dir\Pandora.Tests\Properties\AssemblyInfo.cs" `
		-title "Pandora Container Tests $version" `
		-description "Lightweight .NET IoC Container" `
		-company "Tigraine" `
		-product "Pandora Container" `
		-version $version `
		-copyright "Copyright © Daniel Hölbling 2009"
        
    new-item $build_dir -itemType directory
    new-item $release_dir -itemType directory
    
}

task Build -depends Init {
    msbuild $source_dir\Pandora\Pandora.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: compile failed"
    }
}

task Test -depends Build {
    msbuild $source_dir\Pandora.Tests\Pandora.Tests.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: Test compile failed"
    }
    $old = pwd
    cd $build_dir
    & $lib_dir\xunit\xunit.console.exe $build_dir\Pandora.Tests.dll /html $build_dir\TestResult.htm
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute tests"
        if ($showtestresult)
        {
            start $build_dir\TestResult.htm
        }
    }
    cd $old
}


task Release-NoTest -depends Build {
    $commit = Get-Git-Commit
    $filename = "Pandora"
    & $lib_dir\7zip\7za.exe a $release_dir\pandora-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\dotless.compiler.exe `
    $build_dir\Microsoft.Practices.ServiceLocation.* `
    license.txt `
    
    
    Write-Host -ForegroundColor Yellow "Please note that no tests where run during release process!"
    Write-host "-----------------------------"
    Write-Host "Pandora $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using Pandora!"
}

task Release -depends Test {
    $commit = Get-Git-Commit
    $filename = "Pandora"
    & $lib_dir\7zip\7za.exe a $release_dir\pandora-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\Microsoft.Practices.ServiceLocation.* `
    $build_dir\Testresult.htm `
    license.txt `
    
    
    Write-host "-----------------------------"
    Write-Host "Pandora Container $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using Pandora!"
}