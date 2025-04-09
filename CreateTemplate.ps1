<#
.SYNOPSIS
    Prepares a Clean Architecture solution as a template.

.DESCRIPTION
    This script prepares the Clean Architecture solution as a template by 
    tokenizing namespace references and setting up the structure for packaging.
    It does NOT create the actual NuGet package - this is handled separately.

.PARAMETER SourceDirectory
    The directory containing the source code to templatize. Default is the current directory.

.PARAMETER TemplateNamespace
    The current namespace prefix used in the solution that will be tokenized. Default is "Contact".

.PARAMETER OutputDirectory
    The directory where the prepared template will be created. Default is ".\template-output".
#>

param(
    [string]$SourceDirectory = ".",
    [string]$TemplateNamespace = "Contact",
    [string]$OutputDirectory = ".\template-output"
)

# Create a timestamp for logging purposes
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Write-Host "[$timestamp] Template preparation process started"

# Ensure output directory exists and is empty
if (Test-Path $OutputDirectory) {
    Write-Host "[$timestamp] Cleaning existing output directory..."
    Remove-Item -Path "$OutputDirectory\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "[$timestamp] Creating output directory..."
    New-Item -Path $OutputDirectory -ItemType Directory | Out-Null
}

# Create template structure directories
$templateConfigDir = Join-Path $OutputDirectory ".template.config"
New-Item -Path $templateConfigDir -ItemType Directory -Force | Out-Null

# Copy all solution files to the output directory
Write-Host "[$timestamp] Copying solution files..."
$excludedItems = @(
    "bin", "obj", "node_modules", ".vs", ".vscode", "dist", ".git",
    ".angular", "package-lock.json", "yarn.lock", ".env", 
    "appsettings.Development.json", "CreateTemplate.ps1", "nupkg"
)

# Get absolute path of the output directory to avoid copying it
$absoluteOutputPath = (Resolve-Path -Path $OutputDirectory).Path

# Get all directories in the source directory except excluded ones
Get-ChildItem -Path $SourceDirectory -Directory | 
    Where-Object { 
        $excludedItems -notcontains $_.Name -and 
        # Skip if the current directory is the output directory
        $_.FullName -ne $absoluteOutputPath -and 
        # Skip if the current directory is inside the output directory
        -not $_.FullName.StartsWith("$absoluteOutputPath\") -and
        # Skip if the output directory is inside the current directory
        -not $absoluteOutputPath.StartsWith("$($_.FullName)\")
    } | 
    ForEach-Object {
        Write-Host "Copying directory: $($_.Name)"
        Copy-Item -Path $_.FullName -Destination $OutputDirectory -Recurse -Force
    }

# Get all files in the source directory except excluded ones or hidden files
Get-ChildItem -Path $SourceDirectory -File | 
    Where-Object { 
        -not $_.Name.StartsWith('.') -and 
        $_.Name -ne "CreateTemplate.ps1" -and 
        $_.Extension -ne ".nupkg"
    } | 
    ForEach-Object {
        Write-Host "Copying file: $($_.Name)"
        Copy-Item -Path $_.FullName -Destination $OutputDirectory -Force
    }

# Clean up binary files from the output directory
Write-Host "[$timestamp] Cleaning up binary and temporary files..."
Get-ChildItem -Path $OutputDirectory -Include @("bin", "obj", "node_modules", ".vs", ".vscode", "dist", ".git", ".angular", ".github") -Recurse -Directory | 
    ForEach-Object {
        Write-Host "Removing $($_.FullName)"
        Remove-Item -Path $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }

# Find and replace namespace references in C# files
Write-Host "[$timestamp] Tokenizing namespace references..."
$filesToProcess = Get-ChildItem -Path $OutputDirectory -Include @("*.cs", "*.csproj", "*.sln") -Recurse -File

foreach ($file in $filesToProcess) {
    Write-Host "Processing $($file.FullName)"
    $content = Get-Content -Path $file.FullName -Raw

    # Replace namespace references
    if ($file.Extension -eq ".cs") {
        $content = $content -replace "namespace\s+$TemplateNamespace\.", "namespace ${TemplateNamespace}."
        $content = $content -replace "using\s+$TemplateNamespace\.", "using ${TemplateNamespace}."
    }
    elseif ($file.Extension -eq ".csproj") {
        $content = $content -replace "<RootNamespace>$TemplateNamespace\.", "<RootNamespace>${TemplateNamespace}."
        $content = $content -replace "<AssemblyName>$TemplateNamespace\.", "<AssemblyName>${TemplateNamespace}."
    }
    elseif ($file.Name -eq "*.sln") {
        $content = $content -replace "$TemplateNamespace\.", "${TemplateNamespace}."
    }

    # Write the modified content back to the file
    Set-Content -Path $file.FullName -Value $content -NoNewline
}

# Create template.json configuration file
Write-Host "[$timestamp] Creating template configuration..."
$templateJson = @{
    '$schema' = "http://json.schemastore.org/template"
    author = "Template Author"  # This will be overridden in the GitHub Action
    classifications = @("Web", "API", "Angular", "Clean Architecture", "Full-Stack")
    identity = "CleanArchitecture.FullStack.Template"
    name = "Clean Architecture Full-Stack Solution"
    shortName = "cleanarch-fullstack"
    tags = @{
        language = "C#"
        type = "project"
    }
    sourceName = $TemplateNamespace
    preferNameDirectory = $true
    symbols = @{
        Framework = @{
            type = "parameter"
            description = "The target framework for the project."
            datatype = "choice"
            choices = @(
                @{
                    choice = "net9.0"
                    description = ".NET 9.0"
                }
            )
            defaultValue = "net9.0"
        }
        Organization = @{
            type = "parameter"
            datatype = "string"
            description = "Organization name to use in the project"
            defaultValue = "YourCompany"
            replaces = $TemplateNamespace
        }
        SkipRestore = @{
            type = "parameter"
            datatype = "bool"
            description = "If specified, skips the automatic restore of the project on create."
            defaultValue = $false
        }
        IncludeAngular = @{
            type = "parameter"
            datatype = "bool"
            description = "If specified, includes the Angular frontend project."
            defaultValue = $true
        }
    }
    sources = @(
        @{
            source = "backend/"
            target = "backend/"
        }
        @{
            source = "frontend/"
            target = "frontend/"
            condition = "IncludeAngular"
        }
        @{
            source = "docker-compose.yml"
            target = "docker-compose.yml"
        }
        @{
            source = ".env-example"
            target = ".env-example"
        }
        @{
            source = "README.md"
            target = "README.md"
        }
    )
    postActions = @(
        @{
            condition = "(!SkipRestore)"
            description = "Restore NuGet packages required by this project."
            manualInstructions = @(
                @{
                    text = "Run 'dotnet restore' in the backend directory"
                }
            )
            actionId = "210D431B-A78B-4D2F-B762-4ED3E3EA9025"
            continueOnError = $true
        }
    )
}

# Convert the template configuration to JSON
$templateJsonContent = $templateJson | ConvertTo-Json -Depth 10
Set-Content -Path (Join-Path $templateConfigDir "template.json") -Value $templateJsonContent

Write-Host "[$timestamp] Template preparation completed successfully."
Write-Host "Template output directory: $OutputDirectory"