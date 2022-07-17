![Logo](ResourcePackerGUI-logo.png)

# ResourcePackerGUI
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](LICENSE)
[![Workflow status](https://github.com/vonhoff/ResourcePackerGUI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/vonhoff/ResourcePackerGUI/actions/)

ResourcePackerGUI is a graphical user interface application based on the ResourcePacker command-line tool.
ResourcePacker is a general purpose asset packager with optional AES-128 encryption to protect your game data. It integrates seamlessly with 
[Megamarc's Tilengine](https://github.com/megamarc/Tilengine), which has built-in support for it.

## Features

- Create a package, optionally with AES-128 encryption, and to choose which files to include or exclude from the package. 
- Create a file list with a package, or just the package.
- Preview resources from inside a package, with a hex view, text view, or image view.
- Reconstruct a folder structure from the entry IDs and a provided file list.
- Extract all resources from a package, or just a selected amount, to a folder of your choice.
- Preview or extract an existing package without a file list. The IDs are used as the file names, where the extensions are determined by the MIME type based on its contents.

## Screenshots

![Main application screenshot](screenshot1.png)
![Main application screenshot](screenshot2.png)

## Licenses

- Copyright &copy; 2022 Simon Vonhoff & Contributors - Provided under the [GNU General Public License, Version 3.0](LICENSE).
- Silk icon set by Mark James, [famfamfam.com](http://www.famfamfam.com/lab/icons/silk/) - Provided under the [Creative Commons Attribution 2.5 License](https://creativecommons.org/licenses/by/2.5/).
