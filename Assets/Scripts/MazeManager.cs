using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {

    public GameObject Cell, Pipe, StartPoint, EndPoint, CheckPoint, PathWay;
	
    private GameObject MazeObject;
    public System.Random randomGenerator;
    public int width;
    public int height;

    int startX, startY, endX, endY;
    Vector3 startPosition, endPosition;
    public int pipeCount = 0;
    public string seed;
    public bool useRandomSeed;

	
	void Awake() {
		randomGenerator = CreatetRandomizer();	
	}

    void Start() {
        startPosition = Index2Vector(startX, startY);
        endPosition = Index2Vector(endX = width - 1, endY = height - 1);

        var gamePoints = new GameObject("GamePoints");

        Instantiate(StartPoint, startPosition, Quaternion.identity).transform.parent = gamePoints.transform;
        Instantiate(EndPoint, endPosition, Quaternion.identity).transform.parent = gamePoints.transform;

        GenerateMaze();
        DrawMaze();
        SpawnCheckPoints();

        pipeCount = FindCheckPointPaths();

        SpawnPipes();
    }

    public class MazeCell {
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

	 public Vector3 Index2Vector(int x, int y) {
        return new Vector3(x + .5f, y + .5f, 0);
    }

	System.Random CreatetRandomizer() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        return new System.Random(seed.GetHashCode());
    }

    MazeCell[, ] maze;

    public void GenerateMaze () {

        int visitedCount = 0;

        Stack<MazeCell> mazeStack = new Stack<MazeCell>();

        maze = new MazeCell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = new MazeCell(i, j);
            }   
        }

        int currX = randomGenerator.Next(0, width),
            currY = randomGenerator.Next(0, height);

        MazeCell currentCell = maze[currX, currY];
        
        currentCell.isVisited = true;
        visitedCount++;
        MazeCell neighbour = null;

        while(visitedCount < width * height) {      
            neighbour = selectUnvisitedNeighbour(currentCell.x, currentCell.y, currentCell);
            if (neighbour != null) {                
                mazeStack.Push(currentCell);

                currentCell = neighbour;
                currentCell.isVisited = true;

                visitedCount++;

                neighbour = null;
            } else if (mazeStack.Count > 0) {
                currentCell = mazeStack.Pop();
            } else {
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
                        currMaze.down = 1;
                        maze[x, y + 1].up = 1;
                        return maze[x, y + 1];
                    }
                    break;
                case 3:
                     if (y - 1 >= 0 && !maze[x, y - 1].isVisited) {
                        currMaze.up = 1;
                        maze[x, y - 1].down = 1;
                        return maze[x, y - 1];
                    }
                    break;
            }

            dirList.RemoveAt(index);
        }

        return null;
    }
    
    public void DrawMaze () {
        
        if (MazeObject) {
            // Debug.Log("Destroyed");
            Destroy(MazeObject);
        }

        MazeObject = new GameObject("MapObjects");
        
        if (maze != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GameObject cellObj = Instantiate(Cell, Index2Vector(x, y), Quaternion.identity);
                    cellObj.GetComponent<Wall>().setWalls(maze[x, y].left, maze[x, y].right, maze[x, y].up, maze[x, y].down);
                    cellObj.transform.parent = MazeObject.transform;
                }
            }
        }
    }


	
	List<GameObject> pipeList = new List<GameObject>();
	GameObject pipeParent;

	bool CheckPipeExists(Vector3 position) {
		RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up);

		return hit.collider != null;
	}
	public void SpawnPipes() {
		int randX, randY;
		pipeParent = new GameObject("Pipes");
        int tryCount = 0;
		for (int i = 0; i < pipeCount; i++) {
			randX = randomGenerator.Next(0, width);
			randY = randomGenerator.Next(0, height);
			var randPos = Index2Vector(randX, randY);

			if (!CheckPipeExists(randPos) || tryCount > 4) {
				var pipeInstance = Instantiate(Pipe, randPos, Quaternion.identity);
				pipeInstance.transform.parent = pipeParent.transform;
				pipeList.Add(pipeInstance);
                tryCount = 0;
			} else {
                tryCount++;
				i--;
			}
		}
	}


    public int checkPointCount = 3;
    public List<GameObject> checkPoints = new List<GameObject>();
    List<MazeCell> checkPointCells = new List<MazeCell>();
    public void SpawnCheckPoints() {

        int checkPointRange = height / checkPointCount;

        for (int i = 0; i < checkPointCount; i++) {
            int randY = randomGenerator.Next(i * checkPointRange, (i + 1) * checkPointRange);
            int randX = randomGenerator.Next(0, width);
            checkPoints.Add(Instantiate(CheckPoint, Index2Vector(randX, randY), Quaternion.identity));
            checkPointCells.Add(maze[randX, randY]);
        }

    }

    
    int FindCheckPointPaths() {
        // initial path
        shortestPaths.Add(
            FindShortestPath(startX, startY, checkPointCells[0].x, checkPointCells[0].y)
        );

        // mid paths
        for(int i = 0; i < checkPointCells.Count - 1; i++) {
            checkPointCells[i].LogCell();
            shortestPaths.Add(
                FindShortestPath(checkPointCells[i].x, checkPointCells[i].y, checkPointCells[i + 1].x, checkPointCells[i + 1].y)
            );
        }

        // final path
        shortestPaths.Add(
            FindShortestPath(checkPointCells[checkPointCells.Count - 1].x, checkPointCells[checkPointCells.Count - 1].y, endX, endY)
        );

        var pMap = new Dictionary<MazeCell, bool>();

        int totalPipeCount = 0;
        foreach(var path in shortestPaths) {
            var pathColor = new Color(randomGenerator.Next(0, 255) / 255f, randomGenerator.Next(0, 255) / 255f, randomGenerator.Next(0, 255) / 255f, 0.5f);
            foreach(var cell in path) {
                if (!pMap.ContainsKey(cell)) {
                    var pathObj = Instantiate(PathWay, Index2Vector(cell.x, cell.y), Quaternion.identity);
                    pathObj.GetComponent<SpriteRenderer>().color = pathColor;
                    totalPipeCount++;
                } 
            }
        }
        return totalPipeCount;
    }
    public List<List<MazeCell>> shortestPaths = new List<List<MazeCell>>();
    List<MazeCell> FindShortestPath(int startX, int startY, int endX, int endY) {
        var previous = new Dictionary<MazeCell, MazeCell>();

        Queue<MazeCell> mazeQueue = new Queue<MazeCell>();

        mazeQueue.Enqueue(maze[startX, startY]);

        while(mazeQueue.Count > 0) {
            var vertex = mazeQueue.Dequeue();
            vertex.isVisited = true;
            int x = vertex.x, y = vertex.y;
            
            if (x - 1 >= 0 && maze[x - 1, y].right == 1 && !previous.ContainsKey(maze[x - 1, y])) {
                previous[maze[x - 1, y]] = vertex;
                mazeQueue.Enqueue(maze[x - 1, y]);
            }
            
            if (x + 1 < width && maze[x + 1, y].left == 1 && !previous.ContainsKey(maze[x + 1, y])) {
                previous[maze[x + 1, y]] = vertex;
                mazeQueue.Enqueue(maze[x + 1, y]);
            }
        
            if (y + 1 < height && maze[x, y + 1].up == 1 && !previous.ContainsKey(maze[x, y + 1])) {
                previous[maze[x, y + 1]] = vertex;
                mazeQueue.Enqueue(maze[x, y + 1]);
            }
        
            if (y - 1 >= 0 && maze[x, y - 1].down == 1 && !previous.ContainsKey(maze[x, y - 1])) {
                previous[maze[x, y - 1]] = vertex;
                mazeQueue.Enqueue(maze[x, y - 1]);
            }

        }

        var path = new List<MazeCell>{};

        var current = maze[endX, endY];


        while (current.x != startX || current.y != startY) {
            path.Add(current);
            // Instantiate(PathWay, Index2Vector(current.x, current.y), Quaternion.identity);
            current = previous[current];
        };

        path.Add(maze[startX, startY]);
        path.Reverse();

        return path;
    }
}
