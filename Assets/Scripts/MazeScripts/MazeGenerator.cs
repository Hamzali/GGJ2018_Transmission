using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
	public int width = 10, height = 10;
	int seed;
	public bool useRandomSeed = true;
	public GameObject MazeCell;
	public float cellScale = 1, cellWidth = 1, cellHeight = 1, wallThickness = 1;

	public Vector3 mazeCenter;

	void Awake () {
		if (useRandomSeed) {
			seed = (int) Time.time;
		}

		Random.InitState (seed);
	}

	// Update is called once per frame
	bool isGenerating = false;
	void Update () {
		if (Input.GetKeyDown (KeyCode.A) && !isGenerating) {
			DestroyMaze ();
			GenerateMaze ();
			DrawMaze ();
		}
	}

	Cell[, ] mazeData;
	public void GenerateMaze () {

		isGenerating = true;

		DFSMethod method = new DFSMethod (width, height);

		mazeData = method.GenerateMaze ();

		isGenerating = false;
	}

	public void DestroyMaze () {
		Destroy (mazeParent);
	}

	public Vector3 Index2Vector (int x, int y) {
		return new Vector3 ((cellWidth + wallThickness) * x, (cellHeight + wallThickness) * y, 0);
	}

	GameObject[, ] mazeObjects;
	GameObject mazeParent;
	public void DrawMaze () {
		mazeObjects = new GameObject[width, height];
		mazeParent = new GameObject ("Maze");
		if (mazeData != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					GameObject cellObj = Instantiate (MazeCell, Index2Vector (x, y), Quaternion.identity);
					var cellComp = cellObj.GetComponent<MazeCell> ();

					cellComp.x = x;
					cellComp.y = y;

					cellComp.SetCellScale (cellWidth, cellHeight, wallThickness);

					cellComp.SetTopWall (mazeData[x, y].top);
					cellComp.SetBottomWall (mazeData[x, y].bottom);
					cellComp.SetLeftWall (mazeData[x, y].left);
					cellComp.SetRightWall (mazeData[x, y].right);

					cellObj.transform.parent = mazeParent.transform;
					mazeObjects[x, y] = cellObj;

				}
			}

			Camera.main.transform.position = new Vector3 (width / 2, height / 2, width > height ? -width : -height);;
		}

	}
	public List<Cell> ShortestPath (int startX, int startY, int endX, int endY) {
		var previous = new Dictionary<Cell, Cell> ();

		Queue<Cell> mazeQueue = new Queue<Cell> ();

		mazeQueue.Enqueue (mazeData[startX, startY]);

		while (mazeQueue.Count > 0) {
			Cell vertex = mazeQueue.Dequeue ();
			vertex.LogCell ();
			int x = vertex.x, y = vertex.y;

			if (x - 1 >= 0 && mazeData[x - 1, y].right == false && !previous.ContainsKey (mazeData[x - 1, y])) {
				previous[mazeData[x - 1, y]] = vertex;
				mazeQueue.Enqueue (mazeData[x - 1, y]);
			}

			if (x + 1 < width && mazeData[x + 1, y].left == false && !previous.ContainsKey (mazeData[x + 1, y])) {
				previous[mazeData[x + 1, y]] = vertex;
				mazeQueue.Enqueue (mazeData[x + 1, y]);
			}

			if (y + 1 < height && mazeData[x, y + 1].bottom == false && !previous.ContainsKey (mazeData[x, y + 1])) {
				previous[mazeData[x, y + 1]] = vertex;
				mazeQueue.Enqueue (mazeData[x, y + 1]);
			}

			if (y - 1 >= 0 && mazeData[x, y - 1].top == false && !previous.ContainsKey (mazeData[x, y - 1])) {
				previous[mazeData[x, y - 1]] = vertex;
				mazeQueue.Enqueue (mazeData[x, y - 1]);
			}

		}

		var path = new List<Cell> { };

		var current = mazeData[endX, endY];

		while (current.x != startX || current.y != startY) {
			path.Add (current);
			current = previous[current];
		};

		path.Add (mazeData[startX, startY]);
		path.Reverse ();

		return path;
	}

	public Cell GetMazeCell (int x, int y) {
		return mazeData[x, y];
	}
	public GameObject GetMazeCellObjectByIndex (int x, int y) {
		return mazeObjects[x, y];
	}
}