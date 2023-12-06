# Ultimate Tic Tac Toe

A variation on the children's game of Tic Tac Toe. The program is written in C# and uses Raylib for graphics. 

The AI uses minimax search to choose the best move against the player. It is not optimized, as the evaluation function is strong enough to limit the minimax search depth.

## How To Play
The Ultimate Tic Tac Toe board is divided into nine grids. Each grid plays like normal Tic Tac Toe, where the objective is to score by making a three in a row. The objective of the game is to win three grids in a row.

Each turn the active player will place their symbol on a highlighted square. The player to make the first move may play anywhere on the board. Each subsequent move is confined to the grid corresponding to where the previous player went.
As an example, if X places in the top left of a grid, O is confined to playing in the main board's top left grid. 

The only exception is when it is impossible to play in the confining grid, in which case the player can play on any spot on the grid.

## Build & Run

This app is built with dotnet 7.
To run via the command line:
```console
dotnet run
```
To run and debug via Visual Studio Code:
Open the folder in VS Code, install C# base language support, then run via the debug menu.

## Debugging Raylib issues for MacOS and Linux
Raylib_cs on MacOS and Linux sometimes have compatibility issues with underlying graphics APIs.
To remedy this, change to other versions of Raylib_cs in the dotnet package manager.
