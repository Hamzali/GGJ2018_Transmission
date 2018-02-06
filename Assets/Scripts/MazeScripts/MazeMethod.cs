using UnityEngine;

public class Cell {
	// Wall data.
	public bool top = true, bottom = true, left = true, right = true;

	public int x, y;

	// Visited data.
	public bool isVisited = false;

	public void LogCell () {
		Debug.Log ("x: " + x + " y: " + y + " isVisited: " + isVisited);
	}

};

public abstract class MazeMethod {

	protected int height, width;
	protected Cell[, ] maze;

	public MazeMethod (int w, int h) {
		width = w;
		height = h;
	}

	public abstract Cell[, ] GenerateMaze ();

}