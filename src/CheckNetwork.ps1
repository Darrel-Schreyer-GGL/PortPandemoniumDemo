
# List all the connections involving port 5001
netstat -ano | findstr :5001

# Execute netstat and get the output
$data = netstat -ano | findstr :5001 | Select-String -Pattern "TCP|UDP"

# Parse the output and create objects
$connections = foreach ($line in $data) {
    $line = $line -replace '^\s+', '' -split '\s+'
    [PSCustomObject]@{
        Protocol = $line[0]
        LocalAddress = $line[1]
        ForeignAddress = $line[2]
        State = if ($line.Length -ge 4) { $line[3] } else { "N/A" }
    }
}

# Group by state and count
$connections | Group-Object -Property State | Select-Object Name, Count | Sort-Object Count -Descending | Format-Table -AutoSize
