# ChatTCP

TCP LAN chat demo for a classroom network:

- `ChatServer`: console TCP server.
- `WpfChatClient`: WPF desktop client.

The host laptop runs the server. Other laptops on the same Wi-Fi open the WPF client and connect to the host laptop's Wi-Fi IPv4 address.

## Requirements

- Windows for the WPF client.
- .NET 9 SDK.
- All laptops must be on the same Wi-Fi/LAN.

## Build

```powershell
dotnet build ChatServer\ChatServer.csproj
dotnet build WpfChatClient\WpfChatClient.csproj
```

If build fails because `WpfChatClient.exe` is locked, close the running WPF app and build again.

## Run Demo

### 1. Host laptop: run the server

```powershell
dotnet run --project ChatServer\ChatServer.csproj
```

The server listens on TCP port `5000` and prints the LAN/Wi-Fi IPv4 address. Give that IP to classmates.

If Windows Firewall asks, allow access on the Private network.

### 2. Host laptop: test locally

Run the client:

```powershell
dotnet run --project WpfChatClient\WpfChatClient.csproj
```

For same-machine testing, enter:

```text
127.0.0.1
```

### 3. Classmates: connect over Wi-Fi

Each classmate runs the WPF client and enters the host laptop's Wi-Fi IPv4 address, for example:

```text
192.168.1.25
```

Do not use `127.0.0.1` on classmates' laptops. `127.0.0.1` only points to their own laptop.

## Find Host IP

On the host laptop:

```powershell
ipconfig
```

Use the `IPv4 Address` under the Wi-Fi adapter.

## Notes

- Use unique usernames.
- If clients cannot connect, check that every laptop is on the same Wi-Fi and the host firewall allows the server.
- If the Wi-Fi reconnects, the host IP may change. Run `ipconfig` again.
