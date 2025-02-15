# Define project directories
$projectNames = @("HealthCare", "HealthCare.Tests")
$projectDirs = @(".", "HealthCare.Tests")

# Loop through each project directory to remove bin and obj folders
foreach ($projectDir in $projectDirs) {
    Write-Output "Cleaning bin and obj folders in $projectDir..."
    Remove-Item -Recurse -Force "$projectDir\bin" -ErrorAction SilentlyContinue
    Remove-Item -Recurse -Force "$projectDir\obj" -ErrorAction SilentlyContinue
}

# Run dotnet clean for each project
for ($i = 0; $i -lt $projectNames.Count; $i++) {
    Write-Output "Running dotnet clean for $($projectDirs[$i])\$($projectNames[$i]).csproj..."
    dotnet clean "$($projectDirs[$i])\$($projectNames[$i]).csproj"
}

# Run dotnet build for each project
for ($i = 0; $i -lt $projectNames.Count; $i++) {
    Write-Output "Running dotnet build for $($projectDirs[$i])\$($projectNames[$i]).csproj..."
    dotnet build "$($projectDirs[$i])\$($projectNames[$i]).csproj"
}

# Run migrations
dotnet-ef database update --project HealthCare.csproj

Write-Output "All projects cleaned and built successfully."