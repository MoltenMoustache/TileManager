# TileManager
##Overview
This project was created because I'm a big fan of tile-based games such as Turn-Based Tactics and have very often had ideas for games that required a grid. So instead of recreating them every time I decided to make an easy to use .unitypackage that anybody can import any time they are creating a tile-based game.

##Current Features:
* Grid Spawning
* Get Row (y)
* Get Column (x)
* Get Random Tile
* Get Random Tiles(number of tiles)
* GetTile(x,y)
* GetTiles
* Get Line From Point (point, direction, length)
* Get Line With Point (point, direction, length)
* Get Cross From Point (point, length)
* Get Cross With Point (point, length)

##Future Features:
* Get Circle From Point(point, radius)
* Get Circle With Point(point, radius)
* Djikstra's Shortest Path

##Feature Documentation

###Getting Started
To start using the TileManager, simply import the .unitypackage in to your project and add the TileManager script to an object in your scene.

###Grid Spawning 
To spawn a grid, add either your own tile prefab or a included prefab to the "Tile Prefab" slot.
Then add the x,y size of the grid in the Grid Dimensions variable.
Finally specify how far apart you want each tile to be and then call the 'SpawnGrid()' function.

###Getting Tiles
The TileManager script includes 12 'Getter' functions for acquiring certain tiles, which you can then mess around with as you wish. Each Getter function returns either a reference to a Tile object or an array of Tile objects.

####GetTile(x,y)
This function will simply return a reference to the Tile object at grid coordinates x,y.

####GetTiles()
This function will return the entire 2D array of Tile objects.

####GetRandomTile()
This will return a reference to a random Tile object on the grid.

####GetRandomTiles(count)
This will return 'count' amount of random Tile objects, it will also ensure no tile is randomly selected more than once.

####GetRow(y)
This function will return the entire row at coordinate 'y' as an array of Tile objects.
[][][][] y1
[][][][] **y2** <- Returns this row
[][][][] y3
[][][][] y4

####GetColumm(x)
This function will return the entire column at coordinate 'x' as an array of Tile objects.
[] [] [] []
[] [] [] []
[] [] [] []
[] [] [] []
x1 x2 **x3** x4
       ^ Returns this entire column
	  
####GetLineFromPoint(point, direction, length)
This function will return an array of Tile objects in the form of a line from the point coordinates provided.
The array list is equivelant of the 'length' argument and the direction is determined on a number between 0-3.
0 = North
1 = East
2 = South
3 = West

The returned line does NOT include the originating tile.
	  
####GetLineWithPoint(point, direction, length)
This function will return an array of Tile objects in the form of a line from the point coordinates provided.
The array list is equivelant of the 'length' argument and the direction is determined on a number between 0-3.
0 = North
1 = East
2 = South
3 = West

The returned line DOES include the originating tile.

####GetCrossFromPoint(point, length)
This function returns an array of Tile objects in the form of a cross (+) formation, originating from the point coordinates provided.
The 'length' argument determines how long each line of the cross is.
_GetCrossWithPoint(*, 2)_
[ ] [ ] [.] [ ] [ ]
[ ] [ ] [.] [ ] [ ]		* = point
[.] [.] [*] [.] [.]		. = returned tile
[ ] [ ] [.] [ ] [ ]
[ ] [ ] [.] [ ] [ ]
The array returned does NOT include the originating tile.

####GetCrossWithPoint(point, length)
This function returns an array of Tile objects in the form of a cross (+) formation, originating from the point coordinates provided.
The 'length' argument determines how long each line of the cross is.

_GetCrossWithPoint(*, 2)_
[ ] [ ] [.] [ ] [ ]
[ ] [ ] [.] [ ] [ ]		* = point
[.] [.] [*] [.] [.]		. = returned tile
[ ] [ ] [.] [ ] [ ]
[ ] [ ] [.] [ ] [ ]
The array returned DOES include the originating tile.