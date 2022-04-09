$build = ".\bin\Debug\"
$libraries = ".\lib\"
$content = ".\Content\"
$custom = ".\CustomContent\"

# Clear our build path.
if (Test-Path -Path $build)
{
    Remove-Item $build -Recurse
}

# Create our build directory.
New-Item $build -Type Directory | Out-Null
New-Item ($build + "Content\") -Type Directory | Out-Null

# Copy all required folders.
Get-ChildItem -Path $content | Copy-Item -Destination ($build + "Content\") -Recurse
Get-ChildItem -Path $custom | Copy-Item -Destination ($build + "Content\") -Recurse -Force
Get-ChildItem -Path $libraries | Copy-Item -Destination $build -Recurse

# Copy all .dll files to our build dir.
Get-ChildItem -Path $libraries -Recurse | Where-Object Name -like "*.dll" | Copy-Item -Destination $build -Recurse
