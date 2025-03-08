# Hide And Seek Game Console App


## Table of Contents
- [Description](#description)
- [Installation](#installation)
  - [How to install](#install)
  - [Requirements](#requirements)
- [Run the app](#run)
  - [Run executable by double-clicking it](#run-double-click)
  - [Run executable from command line](#run-exe-cl)
  - [Run app using the dotnet command from command line](#run-dll-cl)
- [App usage](#usage)
  - [Move in specific direction](#usage-move)
  - [Check for hiding opponents](#usage-check)
  - [Save game](#usage-save)
  - [Load game](#usage-load)
  - [Delete game](#usage-delete)
- [Support](#support)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [Credits](#credits)
- [Licenses](#licenses)



<a name="description"></a>
## Description

The Hide and Seek Game Console app allows a user (playing the role of the seeker) to navigate through rooms in a virtual house to find hiding opponents.  To win the game, the user must find all the opponents.  The goal is to win the game in the fewest number of moves possible.



<a name="installation"></a>
## Installation

<a name="install"></a>
### How to install

Download the latest release's .zip file to your machine and unzip it.


<a name="requirements"></a>
### Requirements
Hide and Seek Game Console app is a framework-dependent deployment, so it can only run on a machine that has the .NET runtime installed.  It is a Console app, so the user must have access to a command line interface on the machine.


<a name="run"></a>
## Run the app
Use any of the following 3 ways to run the app.  You can view more details about the non-project-specific process of running a published Console app in [Microsoft Learn's Tutorial: Publish a .NET console application using Visual Studio](https://learn.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio#run-the-published-app) article.


<a name="run-double-click"></a>
### Run executable by double-clicking on it
Double-click the executable file HideAndSeekConsole.exe (extension may not display) in the Publish directory.


<a name="run-exe-cl"></a>
### Run executable from command line
Navigate to the Publish directory via the command line interface and enter this command to run the executable:
```bash
HideAndSeekConsole.exe
```


<a name="run-dll-cl"></a>
### Run app using the dotnet command from command line
Navigate to the Publish directory via the command line interface and enter this command in the command prompt:
```bash
dotnet HideAndSeekConsole.dll
```



<a name="usage"></a>
## App usage

Follow the prompts and options given.  To enter a command, type the command and then press the Enter key on your keyboard.

*Remember that the "save" and "delete" commands create/delete actual files stored on your machine.  So, whenever you run the app, the files saved/deleted during previous app usages will remain saved/deleted.


<a name="usage-move"></a>
### Move in specific direction
Move in any of the directions listed in the exit list (right below the "You see the following exits:" text) by typing the direction and pressing the Enter key on your keyboard. (This counts as a move!)

For example, when you're in the Garage, you could type "In" and press the Enter key to move to the Entry.
```
You are in the Garage. You see the following exits:
 - the Entry is In
Someone could hide behind the car
You have not found any opponents
2: Which direction do you want to go (or type 'check'): In
```


<a name="usage-check"></a>
### Check for hiding opponents

Check to see if any opponents are hiding in your current location by typing "check" and pressing the Enter key on your keyboard.  Only do this when you're in a location with a hiding place (described below the exit list).  (This counts as a move!)

For example, when you're in the Garage, you could type "check" and press the Enter key to check if there's anyone hiding behind the car in the Garage.
```
You are in the Garage. You see the following exits:
 - the Entry is In
Someone could hide behind the car
You have not found any opponents
2: Which direction do you want to go (or type 'check'): check
```

You will be told whether you found any opponents.  If you found one, you will see something like:
```
You found 1 opponent hiding behind the car
```

If you found more than one, you will see something like:
```
You found 2 opponents hiding behind the car
```

If you did not find anyone, you will see something like:
```
Nobody was hiding behind the car
```


<a name="usage-save"></a>
### Save game*
You may save the game anytime before the game ends using the "save" command followed by a space and your desired game name. The game name must not include spaces, backslashes, or forward slashes.  Also, the game name must not be identical to the name of any already-existing saved game.  (This does not count as a move or affect the current game in any other way.)

For example, if you want to save the current game as MyGameName:

```
10: Which direction do you want to go (or type 'check'): save MyGameName
```

If the game was saved successfully, you'll see something like:
```
Game successfully saved in MyGameName
```

If the game was not saved successfully because the file name was invalid, you'll see something like:
```
Cannot perform action because file name is invalid (is empty or contains illegal characters, e.g. \, /, or whitespace)
```

If the game was not saved successfully because the file name is already in use, you'll see something like:
```
Cannot perform action because a file named MyGameName already exists
```

If the game was not saved successfully, try again with a valid file name not currently in use or keep playing the game - no harm done.


<a name="usage-load"></a>
### Load game*

You may load a saved game anytime before the current game ends using the "load" command followed by a space and the name of the game to load.  (Careful because you will lose your progress in your current game after successfully running the load command!) 

For example, if you had previously saved a game called MyGameName, you could type the following and press the Enter key to load MyGameName:
```
10: Which direction do you want to go (or type 'check'): load MyGameName
```

If the game was loaded successfully, you'll see something like:
```
Game successfully loaded from MyGameName
```

If the game was not successfully loaded because it doesn't exist, you'll see something like:
```
Cannot load game because file MyGameName does not exist
```

If the game was not loaded successfully, try again with the name of an existing saved game or keep playing the current game - no harm done.
 

<a name="usage-delete"></a>
### Delete game*

You may delete a saved game anytime before the current game ends using the "delete" command followed by a space and the name of the game to delete.  (Careful because this is a permanent, irreversible action!)  This does not count as a move or affect your current game in any other way.

For example, if you want to delete a game previously saved as MyGameName:
```
10: Which direction do you want to go (or type 'check'): delete MyGameName
```

If the game was successfully deleted, you'll see something like:
```
Game file MyGameName has been successfully deleted
```

If the game was not deleted successfully because it does not exist, you'll see something like:
```
Could not delete game because file MyGameName does not exist
```

If the game was not deleted successfully, try again with the name of an existing saved game or keep playing the current game - no harm done.


<a name="support"></a>
## Support

Please message me via the [contact form here](https://minnystuff.com/contact/) with any questions, comments, or concerns.



<a name="roadmap"></a>
## Roadmap

### Next up (Version 1.1)
- Make the House class non-static and use composition (GameController "has a" House object).  Update tests to use House mocks.


### Version 2.0 - location tracking, saved game list, direction shorthands
- Allow the user to move in a direction by only entering one or two characters rather than a whole word.
- Add a feature for tracking the places the user has already checked during the current game.  The user can request this list at any point during the game.  (Should viewing the list count as a move?)  The list will be saved/restored when a game is saved/restored.
- Add a feature for tracking the names of currently saved games.  The user can request to view the names of currently saved games at any time.


### Version 3.0 - different house layouts
- Create different house layouts as House objects.  Serialize the House objects and store them in files.
- Allow the user to select an existing House layout at the start of a new game.
- When a game is saved, make sure the name of the house layout being used is saved in the SavedGame object and file.
- When a saved game is loaded, make sure the proper house layout is loaded.
- Create a library of house layout files for anyone to download and use.  (Note difficulty levels.)
- Create a library of saved game files for anyone to download and play.  (Note difficulty levels.)


### Version 4.0 - custom house layouts
- Allow the user to create custom House layouts for personal game play and to share with other players.


### Version 5.0 - custom saved games
- Allow the user to create custom saved games (specifying House layout and where opponents are hiding) to share with other players.


### Version 6.0 - multiplayer
- Create a multiplayer version of the game using multiple GameControllers simultaneously so multiple people seek in the same house for the same opponents.  Players take turns moving.  The player who finds the most opponents wins (each opponent can only be found by 1 player).


### More ideas
- Allow the player to "teleport" (be transported to a random location in the House).
- Allow the user to specify how many opponents they want.
- Allow the user to set the names of the opponents.
- Add a timer.
- Add surprises and bonuses (e.g. time bonuses) to find.
- Allow the user to get a hint.
- Allow the user to bulk delete games.
- Make web app (possibly using Blazor or PHP) to enable game play via web browser.
- Make WinForm app to enable game play via native app with visual interface.
- Make Android and iOS apps to enable game play via native app with visual interface on mobile devices.
- Create UML diagrams for classes.



<a name="contributing"></a>
## Contributing

Pull requests are welcome!  Please make sure to update tests in the HideAndSeekTestProject as appropriate.  Ensure adherence to best practices in all code submitted and remember to comment your code (and, of course, give credit where credit is due!).

Please feel free to clone, fork, or download this repository to your heart's content.  This repository contains the following Visual Studio projects:
- HideAndSeekConsoleApp (containing the code allowing the game to be run via the command line interface)
- HideAndSeekClassLibrary (containing the code controlling the game functionality)
- HideAndSeekTestProject (containing the code for testing the HideAndSeekClassLibrary; utilizes NUnit, Moq, TestableIO.System.IO.Abstractions.TestingHelpers, and TestableIO.System.IO.Abstractions.Wrappers)



<a name="credits"></a>
## Credits
Adapted from Stellman & Greene's [HideAndSeek](https://github.com/head-first-csharp/fourth-edition/tree/master/Code/Chapter_10/HideAndSeek_part_3)\
© 2023 Andrew Stellman and Jennifer Greene\
Published under the [MIT License](https://github.com/head-first-csharp/fourth-edition/blob/master/LICENSE)\
Link valid as of 02-25-2025



<a name="licenses"></a>
## Licenses

[General MIT License](https://choosealicense.com/licenses/mit/)\
[Stellman & Greene's copy of the MIT License](https://github.com/head-first-csharp/fourth-edition/blob/master/LICENSE)