# Helper script to run PostgreSQL queries with proper UTF-8 display
param(
    [Parameter(Mandatory=$true)]
    [string]$Query
)

# Configure UTF-8 encoding
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
chcp 65001 | Out-Null

# Load credentials
. "$PSScriptRoot\db_config.ps1"

$Server = $DbConfig.Server
$Port = $DbConfig.Port
$Database = $DbConfig.Database
$Username = $DbConfig.Username
$Password = $DbConfig.Password

# Set PGCLIENTENCODING to UTF8
$env:PGCLIENTENCODING = "UTF8"
$env:PGPASSWORD = $Password

# Run query
& "C:\Program Files\PostgreSQL\17\bin\psql.exe" `
    -h $Server `
    -p $Port `
    -U $Username `
    -d $Database `
    -c $Query

# Clear password from environment
$env:PGPASSWORD = $null
