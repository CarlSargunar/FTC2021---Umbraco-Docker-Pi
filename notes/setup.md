# Set up Umbraco

## Set up the Umbraco Site

### Create solution/project
    dotnet new globaljson --sdk-version 5.0.404
    dotnet new sln --name UmbDockPi

### Ensure we have the latest Umbraco templates
    dotnet new -i Umbraco.Templates

### Add the Umbraco Project

    dotnet new umbraco -n UmbDockPi --friendly-name "Admin User" --email "admin@admin.com" --password "Pa55word!!" --connection-string "Server=[ServerIpAddress];Database=umbracoDb;User Id=sa;Password=SQL_password123;"
    dotnet sln add UmbDockPi
    dotnet add UmbDockPi package Clean
    dotnet add UmbDockPi package Newtonsoft.Json

### Modify csProj

Edit the csproj file to change following element:

    <!-- Force windows to use ICU. Otherwise Windows 10 2019H1+ will do it, but older windows 10 and most if not all winodws servers will run NLS -->
    <ItemGroup Condition="'$(OS)' == 'Windows_NT'">
        <PackageReference Include="Microsoft.ICU.ICU4C.Runtime" Version="68.2.0.9" />
        <RuntimeHostConfigurationOption Include="System.Globalization.AppLocalIcu" Value="68.2" />
    </ItemGroup>

Without this step, the project won't compile on Linux, but will compile in windows.

### Run the site

    dotnet run --project UmbDockPi

# Docker

## Create the Network

To create a network for the containers, run the following.

    docker network create umbNet


## Set up SQL Azure container

Run the following.

    docker run -d --restart unless-stopped --name sql_server --network umbNet -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=SQL_password123' -v mssqlsystem:/var/opt/mssql -v mssqluser:/var/opt/sqlserver -p 1433:1433 mcr.microsoft.com/azure-sql-edge

You will need to connect to your SQL server and create a database. I suggest using the excellent LinqPad tool, which can be downloaded [here](https://www.linqpad.net/).

To create a new database you just need to connect to the server and run the following SQL

    CREATE DATABASE umbracoDb;

## Set up RabbitMQ

Run the following.

    docker pull rabbitmq:3-management

This will map port 15672 for the management web app and port 5672 for the message broker.

    docker run -it -d --name rabbit_mq --network umbNet --restart unless-stopped -p 15672:15672 -p 5672:5672 rabbitmq:3-management

You can test the management web app by going to http://[Pi.IP.Add.ress]:15672 in your browser. You can also test the message broker by going to http://[Pi.IP.Add.ress]:5672 in your browser. Pi.IP.Add.ress is the IP address of your Pi.

The default username is "guest" and the default password is "guest".

## Build the Umbraco Docker Image

To build the docker image for the umbraco site, you need to change to the UmbDockPi folder and run the following command. Replace my username with your docker Username. *Note : that last dot is important, as it gives the docker CLI the current context*

**NOTE: You will need to build the docker image on the raspberry pi in order to be able to use it on the Pi since it is on the linux/Arm64 architecture, whereas your PC is most likely on x86, unless you're on an M1 Mac**

    docker build -t carlsargunar/umbraco-docker .

Push the image to the repository

    docker push carlsargunar/umbraco-docker

To run that image on a docker instance. Here we're passing the AspNet Environment as "Development"

    docker run --name UmbracoDocker --network umbNet --restart unless-stopped -e ASPNETCORE_ENVIRONMENT=Development -d -p 5080:80 carlsargunar/umbraco-docker:latest 

For Production Run

    docker run --name UmbracoDocker --network umbNet --restart unless-stopped -e ASPNETCORE_ENVIRONMENT=Production -d -p 5080:80 carlsargunar/umbraco-docker:latest 