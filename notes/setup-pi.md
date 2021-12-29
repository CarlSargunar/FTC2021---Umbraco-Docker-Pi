# Instructions to Setup Raspberry Pi

To be completed! This guide is a WIP.

## Steps

1. Install Docker

    curl -sSL https://get.docker.com | sh

2. Add permission to Pi User to run Docker Commands

    sudo usermod -aG docker pi

Reboot here or run the next commands with a sudo

3. Test Docker installation

    docker run hello-world

4. IMPORTANT! Install proper dependencies

    sudo apt-get install -y libffi-dev libssl-dev

    sudo apt-get install -y python3 python3-pip

    sudo apt-get remove python-configparser

5. Install Docker Compose

This step takes a little while.

    sudo pip3 -v install docker-compose

6. Enable the Docker system service to start your containers on boot

    sudo systemctl enable docker

Boom! 🔥 It's done!


## To Get a stable Raspberry Pi Image

Browse to [http://downloads.raspberrypi.org/](http://downloads.raspberrypi.org/) and download the latest Buster 64 bit image. 

The full URL is [http://downloads.raspberrypi.org/raspios_lite_arm64/images/raspios_lite_arm64-2021-05-28/2021-05-07-raspios-buster-arm64-lite.zip](http://downloads.raspberrypi.org/raspios_lite_arm64/images/raspios_lite_arm64-2021-05-28/2021-05-07-raspios-buster-arm64-lite.zip)

To burn the image to a Pi you can use Balena Etcher, which can be downloaded here [https://balena.io/etcher/](https://balena.io/etcher/).
