XRunUO
======

[![Travis Build Status](https://travis-ci.org/xrunuo/xrunuo.svg)](https://travis-ci.org/xrunuo/xrunuo)

[![Join the chat at https://gitter.im/xrunuo/xrunuo](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/xrunuo/xrunuo?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

**XRunUO** is a server emulator of the MMORPG game Ultima Online, forked from the RunUO project, adding support for the Stygian Abyss expansion, written in C# and targeting .NET Core 3.1.

## Foreword

**XRunUO** starts as a closed project developed and maintained by owners of the former Ultima Online free server UO Legends. The code was initially forked on late 2005 from RunUO RE 1.2.7.0, and over the years has received updates from newest versions SunUO, RunUO and other forks, as well as code from the community developed for these emulators and adapted for the codebase, plus tons of new features and patches from in-house development. Ten years later, in 2015, the code is finally released to the community, hoping that it will be useful for current and future developers of UO emulation scene.

## Installation

### Requirements

- .NET Core 3.1.
- An Ultima Online installation, or a folder containing UO `.mul` and `.uop` files (XRunUO uses these for extracting map and tileset info).

### Configuring

Configuration is done via `x-runuo.xml` file, located at the root. Before first run, you need to:
- Under `locations` section, configure `data-path` element with the folder where your UO files are located.
- If you are going to access the server via the internet, under `network` section put your public IP address in `bind` element.

### Building and running from sources

Build server and script library binaries:
```sh
~/xrunuo$ make build
```

Then launch the server:
```sh
~/xrunuo$ make run
```

### Binary packages

If you don't want to build from sources, you can also download pre-compiled binaries:

* v0.18.0 (Windows) + Saves w/fully spawned world: https://www.dropbox.com/s/1n2smffgwrwcuzu/xrunuo-distro_0-18-0-41100.zip?dl=0

#### zlib

zlib is required for certain functionality. Windows zlib builds are packaged with releases and can also be obtained separately here: https://github.com/msturgill/zlib/releases/latest

## Features

### Stygian Abyss expansion

**XRunUO** adds almost 100% support for the Stygian Abyss expansion, including the Gargoyle race, Ter Mur facet, Instanced Peerless system, Mini Champ system, new dungeons, new monsters and pets, Pit and Unliving champion spawns, Loyalty system, Tiered quests, Void invassion, and more.

### OSI Updated

Not only the content of the latest expansions is added, but also minor patches and publishes from official servers, including content from up to OSI Publish 73.

### Miscellaneous

Tons of new features have been added over the years that are not listed yet. As soon as info about them is gathered, they will be added to this list.

## Disclaimers

### Old eras and backwards compatibility

In order to allow the development of new features and keep the codebase simple, all the code targeting expansions prior to ML has been removed, as well as the compatibility with old client versions. This prevents the possibility to create a shard targeting, for instance, a pre-AOS era.

### Stability

The current code has only been tested in one live server and is not considered stable, thus its use in production shards is not yet recommended.

### Upgrading from RunUO

Trying to convert world saves from other similar emulators to **XRunUO** is NOT recommended at all, given the substancial differences in the serialization algorithms of main entities. **XRunUO** is only recommended for fresh new shards.

### World building

Some of the scripts to create a fully usable world from a fresh save (such as spawners, sub-system generation, etc.) are not included. However, a save with a fully created world is provided.

## Contributing

### Community

*Coming soon*

### Bug reports

If you discover any bugs, feel free to create an issue on GitHub. Please add as much information as possible to help us fixing the possible bug. We also encourage you to help even more by forking and sending us a pull request.

https://github.com/xrunuo/xrunuo/issues

## Maintainers

* Pedro Pardal (https://github.com/ppardalj)

## Special thanks

* Mark Sturgill (@msturgill) and The RunUO Team
* Wyatt and RunUO:RE developers
* Max Kellerman and SunUO developers
* The RunUO Community
* The UO Legends Staff

## License

XRunUO - Ultima Online Server Emulator
Copyright (C) 2015 Pedro Pardal

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
