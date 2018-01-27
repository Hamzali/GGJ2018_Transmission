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
        Instantiate(spawnObject, p1SpawnPos, Quaternion.identity).name = "Spawn Position1";
        Instantiate(spawnObject, p2SpawnPos, Quaternion.identity).name = "Spawn Position2";
    }

    public void SpawnPipes() {
        mazeManager.SpawnPipes();
    }

    int i = 0;
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            // mazeManager.ActivatePath(i);
            // i++;
        }    
    }

}