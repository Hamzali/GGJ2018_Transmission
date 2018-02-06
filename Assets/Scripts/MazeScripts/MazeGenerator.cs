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

	void Awake()
	{
		if (useRandomSeed) {
            seed = (int)Time.time;
        }

		Random.InitState(seed);
	}
	// Use this for initialization
	void Start () {
		GenerateMaze();
		DrawMaze();
	}
	
	// Update is called once per frame
	bool isGenerating = false;
	void Update () {
		if (Input.GetKeyDown(KeyCode.A) && !isGenerating) {
			DestroyMaze();
			GenerateMaze();
			DrawMaze();	
		}
	}

	
	Cell[,] mazeData;
	public void GenerateMaze() {

		isGenerating = true;

		DFSMethod method = new DFSMethod(width, height);

		mazeData = method.GenerateMaze();

		isGenerating = false;
	}

	void DestroyMaze() {
		Destroy(mazeParent);
	}

	public Vector3 Index2Vector(int x, int y) {
		return new Vector3((cellWidth + wallThickness) * x, (cellHeight + wallThickness) * y, 0);
    }

	GameObject[,] mazeObjects;
	GameObject mazeParent;
	public void DrawMaze() {
		mazeObjects = new GameObject[width, height];
		mazeParent = new GameObject("Maze");
		if (mazeData != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GameObject cellObj = Instantiate(MazeCell, Index2Vector(x, y), Quaternion.identity);
                    var cellComp = cellObj.GetComponent<MazeCell>();
                    
                    cellComp.x = x;
                    cellComp.y = y;

					cellComp.SetCellScale(cellWidth, cellHeight, wallThickness);

					cellComp.SetTopWall(mazeData[x, y].top);
					cellComp.SetBottomWall(mazeData[x, y].bottom);
					cellComp.SetLeftWall(mazeData[x, y].left);
					cellComp.SetRightWall(mazeData[x, y].right);

                    cellObj.transform.parent = mazeParent.transform;
					mazeObjects[x, y] = cellObj;
                    
                }
            }

			Camera.main.transform.position = new Vector3(width / 2, height / 2, width > height ? -width : -height);;
        }

	}

	public List<GameObject> ShortestPath(int startX, int startY, int endX, int endY) {
		
		return new List<GameObject>();
	}
}
