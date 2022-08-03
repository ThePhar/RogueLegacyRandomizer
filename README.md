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
  - ~~Traps~~ **Coming soon.*

### Manor

You can no longer by skill upgrades in the Manor, instead it becomes a repository of additional locations to spend gold
and find and unlock more items.

### Starting Character Customization

You can define the parameters of your starting character, including the class you have unlocked from the start:

  - Starting Name
  - Starting Gender
  - Starting Class
  - ~~Starting Spell~~ **Coming soon.*

### Additional Randomization Options

You can also tweak the randomization with the following settings:

  - NG+ level.
  - Number of Chests and Fairy Chests which contain items.
  - Vendor placement logic.
  - Toggling Charon from taking your money.
  - Gold multiplier settings.
  - Toggling purchasing runes and armor once acquired before you can equip them.
  - Setting the number of children you can choose from on the Lineage screen.
  - Allow the diary in the starting room to be a source of new checks every new castle generation.
  - Toggling of challenge bosses.
  - Number of Stat Skill increases in the item pool.
  - Ability to change the default names your children can have.
  - Ability to change the fee percentage of the Architect's services.

### Multi-World Randomization

You can also randomize items across additional randomizers via the Archipelago service. Create a player settings file at
the [Archipelago Generator Site](https://archipelago.gg/games/Rogue%20Legacy/player-settings) and create a new Room.
From the title screen, connect to your AP room using the hostname, port, and slot name to join. 

#### Chatting

There is also a built-in TextClient for communicating with others in the same room as you. Use the `Enter` key to pull
up the chat input. See more chat options in the Options menu.

#### Death Link

You can also enable Death Link with other players who have it enabled. If one of you dies, you all die. Very exciting.

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
the Rogue Legacy role and communicate more on this project in the `#rogue-legacy` channel.

## License & Copyright

This product is licensed under GPLv3. The source code is not maintained by any of the original creators and the original 
source this is being built on is not my own work! Therefore, all additional copyright notices of Cellar Doors Inc. 
applies, including not using this source for commercial use and re-releasing it with game files (dlls, assets, etc.)
