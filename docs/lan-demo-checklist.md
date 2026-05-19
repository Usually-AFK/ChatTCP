# LAN TCP Chat Demo Checklist

## Before Class

1. Connect the host laptop and classmates' laptops to the same Wi-Fi.
2. Build the server:
   ```powershell
   dotnet build ChatServer\ChatServer.csproj
   ```
3. Build the client:
   ```powershell
   dotnet build WpfChatClient\WpfChatClient.csproj
   ```
4. Find the host IP:
   ```powershell
   ipconfig
   ```
5. Use the Wi-Fi `IPv4 Address`, for example `192.168.2.18`.

## Demo Steps

1. Run `ChatServer` on the host laptop.
2. If Windows Firewall asks, allow private network access.
3. On the host laptop, test one client with `127.0.0.1`.
4. On classmates' laptops, enter the host Wi-Fi IPv4 address.
5. Use unique usernames.
6. Send messages from at least two clients.
7. Close one client and confirm the online list updates.

## Common Problems

- `127.0.0.1` only works when server and client are on the same machine.
- If classmates cannot connect, check that everyone is on the same Wi-Fi.
- If classmates still cannot connect, check Windows Firewall on the host.
- If the IP changes after reconnecting Wi-Fi, run `ipconfig` again.
