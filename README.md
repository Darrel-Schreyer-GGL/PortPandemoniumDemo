# Port Pandemonium Demo

Dive into the common pitfalls of using HttpClient in .NET and learn why improper usage can exhaust system resources and lock up your applications. This session will guide you through best practices and patterns to ensure robust and efficient HTTP communications, keeping your servers happy and your ports plentiful.

## How to Run this Demo

Everything in the demo revolves around port 5001 so make sure it is not being used before you start.

### Start the Server

Open a PowerShell window and run the following command:

```powershell
.\StartServer.ps1
```

### Check the Network

Open a PowerShell window and run the following command:

```powershell
.\CheckNetwork.ps1
```

### Run the Client

Open a PowerShell window and run the following command:

```powershell
.\RunClient.ps1
```

Pick the option you want to run and then check the Network after each run.

You may have to wait for the ports to be reclaimed by the OS.

## Information

### Bad Request Handler

The output from the `CheckNetwork.ps1` command, which displays the network connections, is shown below.

```plaintext
  TCP    127.0.0.1:5001         0.0.0.0:0              LISTENING       14560
  TCP    [::1]:5001             [::]:0                 LISTENING       14560
  TCP    [::1]:60398            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60399            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60400            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60401            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60402            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60403            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60404            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60405            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60406            [::1]:5001             TIME_WAIT       0
  TCP    [::1]:60407            [::1]:5001             TIME_WAIT       0

Name      Count
----      -----
TIME_WAIT    10
LISTENING     2
```




### Better Request Handler

The output from the `CheckNetwork.ps1` command, which displays the network connections, is shown below.

```plaintext
  TCP    127.0.0.1:5001         0.0.0.0:0              LISTENING       14560
  TCP    [::1]:5001             [::]:0                 LISTENING       14560
  TCP    [::1]:5001             [::1]:60419            ESTABLISHED     14560
  TCP    [::1]:60419            [::1]:5001             ESTABLISHED     23304

Name        Count
----        -----
ESTABLISHED     2
LISTENING       2
```

This is an explanation of the ESTABLISHED connections in the output above:

```plaintext
+------------------+                   +------------------+
| Server Process   |                   | Client Process   |
| (PID 14560)      |                   | (PID 23304)      |
|                  |                   |                  |
| [::1]:5001       | <--Connection-->  | [::1]:60419      |
| (Listening)      |                   | (Ephemeral Port) |
+------------------+                   +------------------+
```
The server listens on port 5001 and the client connects to it using an ephemeral port.

### Best Request Handler

The output from the `CheckNetwork.ps1` command, which displays the network connections, is shown below.

```plaintext
  TCP    127.0.0.1:5001         0.0.0.0:0              LISTENING       14560
  TCP    [::1]:5001             [::]:0                 LISTENING       14560
  TCP    [::1]:5001             [::1]:60419            ESTABLISHED     14560
  TCP    [::1]:60419            [::1]:5001             ESTABLISHED     23304

Name        Count
----        -----
ESTABLISHED     2
LISTENING       2
```

Observations:
- Few Active Connections: With HttpClientFactory, you should see fewer active connections, as it reuses existing ones.
- Transient Connections: Connections might be transient, appearing briefly and then closing, due to efficient connection reuse.

Using HttpClientFactory reduces the number of active and lingering connections due to its connection pooling mechanism. As a result, netstat may show fewer results because the connections are efficiently managed and reused, preventing the typical port exhaustion scenario you would see with improper use of HttpClient.