using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    MazeManager mazeManager;

    public GameObject spawnObject;
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

        // Get other managers.
        mazeManager = this.GetComponent<MazeManager>();

        InitGame ();
    }

    void InitGame () {
        Debug.Log ("Initializing Game...");
        
        
    }
    
    void Start() {
        p1SpawnPos = mazeManager.Index2Vector(0, 1);
        p2SpawnPos = mazeManager.Index2Vector(0, mazeManager.width - 1);

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
        return mazeManager.cellList[i * mazeManager.width + j];
    }

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
        

    int i = 0;
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            // mazeManager.ActivatePath(i);
            // i++;
        }    
    }

}