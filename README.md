# CryptoGadget

- [Introduction](#introduction)
- [Features](#features)
- [How to get & run it](#get_run)
- [Customization examples](#examples)
- [Additional notes](#notes)
- [Known Issues](#notes)

# <a name ="introduction"></a> Introduction

CryptoGadget is a C# based 'gadget' to view the current cryptocurrencies prices for Windows with a customizable graphical user interface. CryptoGadget uses the Cryptonator API to display the prices, which supports more than 1000 different cryptocurrencies.

# <a name="features"></a> Features

- Customizable user interface
- Supports over 1000 cryptocurrencies
- Easily change the displayed cryptocurrencies
- Select any of the supported fiat currencies or cryptocurrencies as price reference

# <a name="get_run"></a> How to get and run it ?

<i>**Note:** .NET Framework 4.5 or higher is required, you can download it from here: https://www.microsoft.com/en-us/download/details.aspx?id=30653</i>

Instructions: 
- Download the binaries from here: https://github.com/neo3587/CryptoGadget/releases
- Extract the zip archive
- Run CryptoGadget.exe
- Right-click to the gadget and select 'Settings'
- Customize your gadget at your liking

# <a name="examples"></a> Customization examples

<img src="http://i.imgur.com/VhZ2AQE.png" /> <img src="http://i.imgur.com/3aOcagn.png" /> <img src="http://i.imgur.com/wfKx5BU.png" /> <img src="http://i.imgur.com/tPJX8ic.png" />

# <a name="notes"></a> Additional notes

This is the first time I program in C#, also is the first time that I use Windows Forms and the first time I use GitHub to make a public project, so probably some things could be done in a better way.

Also my english is far from perfect, so expect some grammatical errors if you read the tooltips (by holding the mouse on almost any thing in the settings).

# <a name="issues"></a> Known Issues

- The Cryptonator server only allows ~10 requests at a time and rejects any other until a period of time has passed, if you try to add more than 10 cryptocurrencies in the settings it will pop up a message advertising you from this (but it will allow you to add as many cryptocurrencies as you want), working on a way to be more friendly with the server. 
