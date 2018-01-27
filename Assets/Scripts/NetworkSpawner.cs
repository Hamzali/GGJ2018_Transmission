using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isServer) {
			GameManager.instance.SpawnPipes();
		}
	}
	
	
}
