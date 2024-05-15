# Define the target port
$targetPort = 5001

# Get the list of ports in TIME_WAIT state connected to the target port
$connections = netstat -ano | Select-String ":$targetPort\s+.*\s+TIME_WAIT\s+\d+" | ForEach-Object {
    if ($_.Matches.Count -gt 0) {
        return $_.Line
    }
}

if ($connections) {
    Write-Host "Ports in TIME_WAIT state connected to port $targetPort:"
    $connections | ForEach-Object { Write-Host $_ }
} else {
    Write-Host "No ports in TIME_WAIT state connected to port $targetPort."
}

# NOTE: You cannot directly reclaim TIME_WAIT ports using PowerShell.
# The TIME_WAIT state will expire naturally. This script only lists the ports.
