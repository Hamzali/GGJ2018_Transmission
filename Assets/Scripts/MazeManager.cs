using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {

    public GameObject Cable, StartPoint, EndPoint, CheckPoint, PathWay;
	
    int startX = 0, startY = 0, endX, endY;
    Vector3 startPosition, endPosition;
    public int pipeCount = 0;

    MazeGenerator mazeGenerator;
	

    public void InitMaze() {
        mazeGenerator = GetComponent<MazeGenerator>();

        startPosition = mazeGenerator.Index2Vector(startX, startY);
        endPosition = mazeGenerator.Index2Vector(endX = mazeGenerator.width - 1, endY = mazeGenerator.height - 1);

        var gamePoints = new GameObject("GamePoints");

        Instantiate(StartPoint, startPosition, Quaternion.identity).transform.parent = gamePoints.transform;
        Instantiate(EndPoint, endPosition, Quaternion.identity).transform.parent = gamePoints.transform;

        mazeGenerator.GenerateMaze();
        mazeGenerator.DrawMaze();
        
        SpawnCheckPoints();
        pipeCount = FindCheckPointPaths();
        

        // SpawnPipes();
    }

    public void ReGenerateMaze() {
        mazeGenerator.DestroyMaze();
        mazeGenerator.GenerateMaze();
        mazeGenerator.DrawMaze();
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
			randX = Random.Range(0, mazeGenerator.width);
			randY = Random.Range(0, mazeGenerator.height);
			var randPos = mazeGenerator.Index2Vector(randX, randY);
            bool isStartPoint = randX == startX && randY == startY;
            bool isEndPoint = randX == endX && randY == endY;
            
			if (!isStartPoint && !isEndPoint &&  !CheckPipeExists(randPos) || tryCount > 4) {
				var pipeInstance = Instantiate(Cable, randPos, Quaternion.identity);
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
    List<Cell> checkPointCells = new List<Cell>();
    public void SpawnCheckPoints() {

        int checkPointRange = mazeGenerator.height / checkPointCount;
        var parentObj = new GameObject("CheckPoints");

        for (int i = 0; i < checkPointCount; i++) {
            int randY = Random.Range(i * checkPointRange, (i + 1) * checkPointRange);
            int randX = Random.Range(0, mazeGenerator.width);
            checkPoints.Add(Instantiate(CheckPoint, mazeGenerator.Index2Vector(randX, randY), Quaternion.identity, parentObj.transform));
            checkPointCells.Add(mazeGenerator.GetMazeCell(randX, randY));
        }

    }

    public List<GameObject> pathObjects = new List<GameObject>();
    public List<List<Cell>> shortestPaths = new List<List<Cell>>();
    int FindCheckPointPaths() {
        // initial path
        shortestPaths.Add(
            mazeGenerator.ShortestPath(startX, startY, checkPointCells[0].x, checkPointCells[0].y)
        );

        // mid paths
        for(int i = 0; i < checkPointCells.Count - 1; i++) {
            shortestPaths.Add(
                mazeGenerator.ShortestPath(checkPointCells[i].x, checkPointCells[i].y, checkPointCells[i + 1].x, checkPointCells[i + 1].y)
            );
        }

        // final path
        shortestPaths.Add(
            mazeGenerator.ShortestPath(checkPointCells[checkPointCells.Count - 1].x, checkPointCells[checkPointCells.Count - 1].y, endX, endY)
        );

        var pMap = new Dictionary<Cell, bool>();
        

        int totalPipeCount = 0;
        int pathIndex = 0;
        foreach(var path in shortestPaths) {
            var pathColor = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, 0.5f);
            var parentObj = new GameObject("path" + pathIndex);
            
            foreach(var cell in path) {
                if (!pMap.ContainsKey(cell)) {
                    var pathObj = Instantiate(PathWay, mazeGenerator.Index2Vector(cell.x, cell.y), Quaternion.identity, parentObj.transform);
                    pathObj.GetComponent<SpriteRenderer>().color = pathColor;
                    totalPipeCount++;
                } 
            }
            parentObj.SetActive(false);
            pathObjects.Add(parentObj);
            pathIndex++;
        }
        return totalPipeCount;
    }

    public void ActivatePath(int pathIndex) {
        if (pathIndex > pathObjects.Count - 1) {
            throw new UnityException("Path index out of range");
        }

        pathObjects[pathIndex].SetActive(true);
    }
}
