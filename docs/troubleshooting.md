# Troubleshooting

## Connection Failed

Verify:

- Host name or IP address
- Port number
- Username and password
- Private key path
- Firewall or VPN access

---

## SFTP Private Key Issues

Supported key formats:

- `.ppk`
- `.pem`
- OpenSSH private keys

Verify the private key file exists and is accessible.

---

## Authentication Failed

Check:

- Username
- Password
- Private key passphrase
- Authentication type configuration

---

## Connection Drops During Browsing

RemoteFileDialog includes background connection monitoring and reconnect support.

If the connection is lost:

- Browser views are cleared
- Reconnect button becomes available

---

## Empty Folder or File List

Verify:

- The remote path exists
- User has permission to access the folder
- The folder contains visible items

---

## Build Issues

Recommended environment:

- Visual Studio 2022
- .NET 8 SDK
- Windows with WPF support
