using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    MazeManager mazeManager;
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
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            // mazeManager.FindShortestPath(5, 0, 8, 13);
        }    
    }


}