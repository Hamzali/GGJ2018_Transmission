using System.Collections.Generic;
using UnityEngine;
public class DFSMethod : MazeMethod {

	public DFSMethod (int w, int h) : base (w, h) { }

	Stack<Cell> mazeStack = new Stack<Cell> ();
	int visitedCount = 0;

	void DFSCall (int x, int y) {
		if (visitedCount > width * height && mazeStack.Count == 0) {
			maze[x, y].LogCell ();
			return;
		}

		Cell currentCell = maze[x, y];

		Cell selectedCell = SelectUnvisitedNeighbour (x, y);
		if (selectedCell != null) {
			selectedCell.isVisited = true;
			visitedCount++;

			mazeStack.Push (currentCell);

			DFSCall (selectedCell.x, selectedCell.y);
		} else if (mazeStack.Count > 0) {
			selectedCell = mazeStack.Pop ();
			DFSCall (selectedCell.x, selectedCell.y);
		}
	}

	override public Cell[, ] GenerateMaze () {
		maze = new Cell[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				maze[i, j] = new Cell ();
				maze[i, j].x = i;
				maze[i, j].y = j;
			}
		}

		int randX = Random.Range (0, width);
		int randY = Random.Range (0, height);

		visitedCount++;
		maze[randX, randY].isVisited = true;

		DFSCall (randX, randY);

		return maze;
	}

	Cell SelectUnvisitedNeighbour (int x, int y) {
		List<int> dirList = new List<int> ();

		dirList.Add (0); // left
		dirList.Add (1); // right 
		dirList.Add (2); // up
		dirList.Add (3); // down

		while (dirList.Count > 0) {
			int index = Random.Range (0, dirList.Count);

			switch (dirList[index]) {
				case 0:
					if (x - 1 >= 0 && !maze[x - 1, y].isVisited) {
						maze[x, y].left = false;
						maze[x - 1, y].right = false;
						return maze[x - 1, y];
					}
					break;

				case 1:
					if (x + 1 < width && !maze[x + 1, y].isVisited) {
						maze[x, y].right = false;
						maze[x + 1, y].left = false;
						return maze[x + 1, y];
					}
					break;
				case 2:
					if (y + 1 < height && !maze[x, y + 1].isVisited) {
						maze[x, y].top = false;
						maze[x, y + 1].bottom = false;
						return maze[x, y + 1];
					}
					break;
				case 3:
					if (y - 1 >= 0 && !maze[x, y - 1].isVisited) {
						maze[x, y].bottom = false;
						maze[x, y - 1].top = false;
						return maze[x, y - 1];
					}
					break;
			}

			dirList.RemoveAt (index);
		}

		return null;
	}

}