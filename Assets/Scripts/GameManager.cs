using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;



    // In game data.

    // maze generation and spawn
    public GameObject mazeManagerObject;
    MazeManager mazeManager;
    MazeGenerator mazeGenerator;
    public GameObject spawnObject;


    // players data.
    public Vector3 p1SpawnPos;
    public Vector3 p2SpawnPos;
    // Use this for initialization
    void Awake () {
        if (instance == null) {
            instance = this;
        }

        if (instance != this) {
            DestroyObject (instance);
        }

        DontDestroyOnLoad (this);
    }

    void Start() {

        InitGame();
        
    }

    void InitGame() {
        // Create the maze manager to initialize the random level.
        mazeManagerObject = Instantiate(mazeManagerObject);
        mazeManager = mazeManagerObject.GetComponent<MazeManager>();
        mazeGenerator = mazeManagerObject.GetComponent<MazeGenerator>();
        mazeManager.InitMaze();

        // Set player start points.
        Instantiate(spawnObject, p1SpawnPos, Quaternion.identity).name = "Spawn Position1";
        Instantiate(spawnObject, p2SpawnPos, Quaternion.identity).name = "Spawn Position2";
        
        foreach(var path in mazeManager.pathObjects) {
            int x = (int)path.transform.position.x;
            int y = (int)path.transform.position.y;

            var cell = GetCellByIndex(x, y);

            if (!pathDict.ContainsKey(cell)) {
                pathDict.Add(cell, false);
            }

        }        
    }
    public void SpawnPipes() {
        mazeManager.SpawnPipes();
    }

	public void ActivatePath(GameObject checkpoint) {
		if (checkpoint.tag == "Endpoint") {
			mazeManager.ActivatePath (mazeManager.checkPoints.Count);
			return;
		}
		int c = mazeManager.checkPoints.IndexOf (checkpoint);
		mazeManager.ActivatePath (c);
	
	}

    public GameObject GetCellByIndex(int i, int j) {
        return mazeGenerator.GetMazeCellObjectByIndex(i, j);
    }

    // Win condition check.
     Dictionary<GameObject, bool> pathDict = new Dictionary<GameObject, bool>();

    public bool CheckWinCodition() {
        foreach(bool val in pathDict.Values) {
            if (!val) {
                return false;
            }
        }
        return true;
    }

    public void UpdatePathDict(GameObject cell, bool value) {
        if (pathDict.ContainsKey(cell)) {
            pathDict[cell] = value;
        }
    }
}