# Rogue Legacy: Randomizer
![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/thephar/roguelegacyrandomizer?label=latest&style=flat-square)
![GitHub](https://img.shields.io/github/license/thephar/roguelegacyrandomizer?style=flat-square)
![GitHub commits since latest release (by date) for a branch](https://img.shields.io/github/commits-since/thephar/roguelegacyrandomizer/latest/master?color=8df702&style=flat-square)

This project is based of the decompiled and cleaned source code by Cellar Door Games. This project makes some quality of life improvements and adds a spin to the core game mechanics by adding randomization via the Archipelago service by the [Archipelago Community](https://archipelago.gg).

## Install Instructions

### Automated Installer Instructions

Go to the releases tab and download the latest version of Rogue Legacy Randomizer and run the installer. It will ask you where your vanilla Rogue Legacy install is located, so be sure to choose the right directory. Rogue Legacy Randomizer will install to a new directory in `C:\ProgramData\Rogue Legacy Randomizer`.

If you would prefer to manually install, please see the below directions.

### Manual Installation Instructions

In order to run Rogue Legacy Randomizer you will need to have Rogue Legacy installed on your local machine. Extract the Randomizer release into a desired folder **outside** of your Rogue Legacy install. Copy the following files from your Rogue Legacy install into the main directory of your Rogue Legacy Randomizer install:

- DS2DEngine.dll
- InputSystem.dll
- Nuclex.Input.dll
- SpriteSystem.dll
- Tweener.dll

And copy the directory from your Rogue Legacy install as well into the main directory of your Rogue Legacy Randomizer install:

- Content/

Then copy the contents of the CustomContent directory in your Rogue Legacy Randomizer into the newly copied Content directory and overwrite all files. 

**BE SURE YOU ARE REPLACING THE COPIED FILES IN YOUR ROGUE LEGACY RANDOMIZER DIRECTORY AND NOT REPLACING YOUR ROGUE LEGACY FILES!**

You should be good to run RogueLegacyRandomizer.exe! You will also need to generate a game at [archipelago.gg](https://archipelago.gg) to create a new game.

## License & Copyright

This product is licensed under GPLv3. The source code is not maintained by any of the original creators and the original 
source this is being built on is not my own work! Therefore, all additional copyright notices of Cellar Doors Inc. applies, 
including not using this source for commercial use and re-releasing it with game files (dlls, assets, etc.)
