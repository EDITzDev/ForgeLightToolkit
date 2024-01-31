# ForgeLight Toolkit
A Unity package that imports ForgeLight assets within Unity.

[Free Realms Demo](https://github.com/EDITzDev/ForgeLightToolkit/assets/7481152/3c76814b-f04e-4037-98a8-c59909a809a9)

[Clone Wars Adventures Demo](https://github.com/EDITzDev/ForgeLightToolkit/assets/7481152/73eb2056-bb28-4e6d-a2d4-3cc6d9db88d2)

## Supported ForgeLight Games

> :heavy_check_mark: Complete :warning: Incomplete :x: Unavailable

| Game Title | `.adr` | `.dma` | `.dme` | `.gck2` | `.gcnk` | `.gzne` | `.map` |
| :--------: | :--: | :--: | :--: | :---: | :---: | :--: | :--: |
| Free Realms | :warning: | :heavy_check_mark: | :warning: | :heavy_check_mark: | :warning:| :warning: | :heavy_check_mark: |
| Clone Wars Adventures | :warning: | :heavy_check_mark: | :warning: | :heavy_check_mark: | :warning: | :warning: | :x: |

> If you want another ForgeLight game to be supported please create a new discussion in the discussions section.

## Setup

1. Ensuring you have git installed, open the Unity Package Manager.
2. Click the `+` button and choose `Add package from git URL...`
3. Enter the current repository git URL. `https://github.com/EDITzDev/ForgeLightToolkit.git`
4. Create the folowing folder structure `Assets/ForgeLight/<game>`, Eg: `Assets/ForgeLight/FreeRealms`
5. Copy all or relevant assets to the folder you created, this can take a few minutes.

## Features

* Load World (WIP) - Loads the `.gzne` and all available `.gcnk` files for the chosen world including all the objects and lights (if enabled).
* Load All Worlds (WIP) - Loads all the available `.gzne` and `.gcnk` files for the chosen world including all the objects and lights (if enabled).

## Default Shaders
The provided shaders are example shaders based on the inputs of the equivalent Forgelight shaders. They may not be perfect recreations. The provided shaders also have culling disabled. To enable "backface" culling, every shader's `Cull Off` line should be changed to `Cull Front` when the `Invert Z Axis` setting is false, and to `Cull Back` when the `Invert Z Axis` setting is set to true.
