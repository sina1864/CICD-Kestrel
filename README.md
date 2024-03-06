
# CI/CD-Kestrel

Sample CI/CD Project - Using GitHub Actions with FTP and Kestrel on Ubuntu 22.04

## Deployment

Steps to deploy this project:

**Step 1 (Install dependencies):**

Download the Microsoft repository package, install it, remove the downloaded file, update package lists, install the apt-transport-https tool to handle secure repositories, update package lists again, and finally install the .NET SDK 8.0:

```bash
  wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  rm packages-microsoft-prod.deb
  sudo apt-get update
  sudo apt-get install -y apt-transport-https
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-8.0
```

**Step 2 (Create a self-hosted runner):**

Go to GitHub Settings -> Actions -> Runners. Then create a Linux x64 runner. Before the Configuration step, execute this command: `export RUNNER_ALLOW_RUNASROOT="1"`. Also, do not execute the `./run.sh` command.
-  install and then start a service using the script named `svc.sh`:
```bash
  sudo ./svc.sh install
  sudo ./svc.sh start
```

**Step 3 (Setup the application directory):**

Set up a directory structure for a sample application (/var/www/sample-app) and create a user (ubuntu) with its own group to manage the application files:
```bash
  sudo mkdir -p /var/www/sample-app
  sudo useradd ubuntu
  sudo groupadd ubuntu
  sudo chown ubuntu:ubuntu /var/www/sample-app
```
**Step 4 (Instal FTP on server):**

Install FTP:
```bash
  sudo apt update
  sudo apt install vsftpd
  sudo systemctl start vsftpd
  sudo systemctl enable vsftpd
  sudo cp /etc/vsftpd.conf /etc/vsftpd.conf_default
  sudo useradd -m testuser
  sudo passwd testuser
  sudo ufw allow 20/tcp
  sudo ufw allow 21/tcp
  apt install ncftp
  ncftp -u testuser -p some_password server_ip
```
Modify settings of the FTP server:
```bash
  sudo nano /etc/vsftpd.conf
```
- add this line: `write_enable=YES`
- restart the vsftpd service:
``` bash
  sudo systemctl restart vsftpd.service
```

**Step 5 (Define GitHub secrets):**

Define the secrets that will be used in the GitHub Actions YAML file:
```
CONNECTION_STRING : database_connection_string
FTP_SERVER : server_ip
FTP_USER : testuser
FTP_PASSWORD : some_password
```

**Step 6 (Define GitHub actions):**

Define the GitHub Actions YAML file (use this project YAML file).

**Step 7 (Create a background service for the application):**
Create a directory structure for a systemd user service for the "sample-app" and open the service file for editing, allowing the app to run under the "ubuntu" user:
```bash
  mkdir -p ~/.config/systemd/user/
  loginctl enable-linger ubuntu
  nano ~/.config/systemd/user/sample-app.service
```
- copy and paste this sample:
```
[Unit]
Description=Example .NET 8 Application

[Service]
# This is the directory where our published files are
WorkingDirectory=/var/www/sample-app
# We set up `dotnet` PATH in Step 1. The second one is path of our executable
ExecStart=/usr/bin/dotnet /var/www/sample-app/CICD-API.dll --urls "http://0.0.0.0:5299"
Restart=always
# Restart service after 10 seconds if the dotnet service crashes
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=sample-app-log
# We can even set environment variables
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
# When a systemd user instance starts, it brings up the per user target default.target
WantedBy=default.target
```
Reload systemd for the user, enable a user service named "sample-app.service" to start at login, and then start the service immediately:
```bash
systemctl --user daemon-reload
systemctl --user enable sample-app.service
systemctl --user start sample-app.service
```

**Step 8 (Obtain SSL certificate):**

Connect your domain to the server's IP by using an A record.

Install Certbot as a Snap package, create a symbolic link to make it globally accessible, and then use Certbot to obtain an SSL certificate for the Nginx server:
```bash
  sudo snap install --classic certbot
  sudo ln -s /snap/bin/certbot /usr/bin/certbot
  sudo certbot --nginx
```
- add your email address and domain and also accept the rules (Y).

**Step 9 (Re-run the GitHub action):**

Re-run all jobs of your last GitHub push and then go to `https://your_domain/swagger/index.html` to view your website.
