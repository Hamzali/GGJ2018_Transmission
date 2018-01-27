using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject Cell;
    private GameObject MazeObject;
    System.Random randomGenerator;

    // Use this for initialization
    void Awake () {
        if (instance == null) {
            instance = this;
        }

        if (instance != this) {
            DestroyObject (instance);
        }

        DontDestroyOnLoad (this);

        InitGame ();
    }

    void InitGame () {
        Debug.Log ("Initializing Game...");
    }

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    class MazeCell {
        public MazeCell(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int left = 0, right = 0, up = 0, down = 0, x, y;
        public bool isVisited = false;

        public void LogCell () {
            Debug.Log( "x: " + x + " y: " + y + "\n" + "left: " + left + " right: " + right + " up: " + up + " down: " + down + " isVisited: " + isVisited);
        }
    };

    MazeCell[, ] maze;

    void Start () {
        GenerateMaze ();
        DrawMaze();
    }

    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            GenerateMaze ();
            DrawMaze();
        }
    }

    int visitedCount = 0;
    Stack<MazeCell> mazeStack = new Stack<MazeCell>();
    void GenerateMaze () {
        maze = new MazeCell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = new MazeCell(i, j);
            }   
        }

        randomGenerator = CreatetRandomizer();

        int currX = randomGenerator.Next(0, width),
            currY = randomGenerator.Next(0, height);

        Debug.Log("x " + currX);
        Debug.Log("y " + currY);
        MazeCell currentCell = maze[currX, currY];
        
        currentCell.isVisited = true;
        visitedCount++;
        MazeCell neighbour = null;

        while(visitedCount < width * height) {      
            neighbour = selectUnvisitedNeighbour(currentCell.x, currentCell.y, currentCell);
            Debug.Log(mazeStack.Count);
            currentCell.LogCell();
            if (neighbour != null) {
                Debug.Log("Neighbour: ");
                neighbour.LogCell();
                mazeStack.Push(currentCell);

                currentCell = neighbour;
                currentCell.isVisited = true;

                visitedCount++;

                neighbour = null;
            } else if (mazeStack.Count > 0) {
                currentCell = mazeStack.Pop();
            } else {
                Debug.Log("Wrooooong");
                currX = randomGenerator.Next(0, width);
                currY = randomGenerator.Next(0, height);

                currentCell = maze[currX, currY];
                break;
            }
        }
        
    }

    MazeCell selectUnvisitedNeighbour(int x, int y, MazeCell currMaze) {
        List<int> dirList = new List<int>();

        dirList.Add(0); // left
        dirList.Add(1); // right 
        dirList.Add(2); // up
        dirList.Add(3); // down

        while(dirList.Count > 0) {
            int index = randomGenerator.Next(0, dirList.Count);

            switch(index) {
                case 0:
                    if (x - 1 >= 0 && !maze[x - 1, y].isVisited) {
                        currMaze.left = 1;
                        maze[x - 1, y].right = 1;
                        return maze[x - 1, y];
                    }
                    break;

                case 1:
                    if (x + 1 < width && !maze[x + 1, y].isVisited) {
                        currMaze.right = 1;
                        maze[x + 1, y].left = 1;
                        return maze[x + 1, y];
                    }
                    break;
                case 2:
                     if (y + 1 < height && !maze[x, y + 1].isVisited) {
                        currMaze.up = 1;
                        maze[x, y + 1].down = 1;
                        return maze[x, y + 1];
                    }
                    break;
                case 3:
                     if (y - 1 >= 0 && !maze[x, y - 1].isVisited) {
                        currMaze.down = 1;
                        maze[x, y - 1].up = 1;
                        return maze[x, y - 1];
                    }
                    break;
            }

            dirList.RemoveAt(index);
        }

        return null;
    }
    System.Random CreatetRandomizer() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        return new System.Random(seed.GetHashCode());
    }

    void DrawMaze () {
        
        if (MazeObject) {
            Debug.Log("Destroyed");   
            Destroy(MazeObject);
        }

        MazeObject = new GameObject("MapObjects");
        
        if (maze != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Vector3 pos = new Vector3 (-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    GameObject cellObj = Instantiate(Cell, pos, Quaternion.identity);
                    cellObj.GetComponent<Wall>().setWalls(maze[x, y].left, maze[x, y].right, maze[x, y].up, maze[x, y].down);
                    cellObj.transform.parent = MazeObject.transform;
                }
            }
        }
    }

}