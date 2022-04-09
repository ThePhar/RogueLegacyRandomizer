# Rogue Legacy Randomizer
![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/thephar/roguelegacyrandomizer?label=latest&style=flat-square)
![GitHub](https://img.shields.io/github/license/thephar/roguelegacyrandomizer?style=flat-square)
![GitHub commits since latest release (by date) for a branch](https://img.shields.io/github/commits-since/thephar/roguelegacyrandomizer/latest/master?color=8df702&style=flat-square)

This project is based on decompiled and *mostly* cleaned-up source of Rogue Legacy by Cellar Door Games. This project 
makes some quality of life improvements and adds a spin to the core game mechanics by adding additional randomization 
elements.

## Randomization Features / Changes

### Chests

Chests no longer contain their respective loot table of contents. Instead, each chest (fairy or otherwise) can contain
the following:

  - Class Unlocks and Upgrades
  - Skill Upgrades
  - Blueprints
  - Runes
  - Gold
  - Stat Increases
  - Traps

### Manor

You can no longer by skill upgrades in the Manor, instead it becomes a repository of additional locations to spend gold
and find your items.

### Multi-World Randomization

You can also randomize items across additional randomizers via the Archipelago service. Create a player settings file at
the [Archipelago Generator Site](https://archipelago.gg/games/Rogue%20Legacy/player-settings) and create a new Room.
From the title screen, connect to your AP room using the hostname, port, and slot name to join. There is also a built-in
TextClient for communicating with others in the same room as you.

## Install Instructions

In order to run Rogue Legacy Randomizer you will need to have Rogue Legacy installed on your local machine.

### Automated Installer Instructions

Go to the releases tab and download the latest version of Rogue Legacy Randomizer and run the installer. It will ask you
where your vanilla Rogue Legacy install is located, so be sure to choose the right directory. By default, Rogue Legacy 
Randomizer installs to a directory in `C:\ProgramData\Rogue Legacy Randomizer`.

If you would prefer to build from source and install, please see the below directions.

### Manual Build Instructions

TODO: Write me.

## Discord / Contact Info

For feature requests or bug reports on Rogue Legacy Randomizer, reach out to me on discord. My handle is `Phar#4444`.
Alternatively, you can also pop in the [Archipelago Discord](https://discord.gg/8Z65BR2) and in `#role-request` accept 
the Rogue Legacy role and communicate more on this project in the `#rogue-general` and `#rogue-suggestions` channels.

## License & Copyright

This product is licensed under GPLv3. The source code is not maintained by any of the original creators and the original 
source this is being built on is not my own work! Therefore, all additional copyright notices of Cellar Doors Inc. 
applies, including not using this source for commercial use and re-releasing it with game files (dlls, assets, etc.)
