# ForgeLight Toolkit
A Unity package that imports ForgeLight assets within Unity.

![Demo](Demo.gif)

## Supported ForgeLight Games

> :heavy_check_mark: Complete :warning: Incomplete :x: Unavailable

| Game Title | `.adr` | `.dma` | `.dme` | `.gck2` | `.gcnk` | `.map` |
| :--------: | :--: | :--: | :--: | :---: | :---: | :--: |
| Free Realms | :warning: | :heavy_check_mark: | :warning: | :heavy_check_mark: | :warning: | :heavy_check_mark: |
| Clone Wars Adventures | :warning: | :heavy_check_mark: | :warning: | :heavy_check_mark: | :warning: | :x: |

> If you want another ForgeLight game to be supported please create a new discussion in the discussions section.

## Setup

1. Ensuring you have git installed, open the Unity Package Manager.
2. Click the `+` button and choose `Add package from git URL...`
3. Enter the current repository git URL. `https://github.com/EDlTz/ForgeLightToolkit.git`
4. Create the folowing folder structure `Assets/ForgeLight/<game>`, Eg: `Assets/ForgeLight/FreeRealms`
5. Copy all or relevant assets to the folder you created, this can take a few minutes.

## Features

* Load World (WIP) - Loads all the available `.gcnk` files for the chosen world including all the objects and lights (if enabled).
