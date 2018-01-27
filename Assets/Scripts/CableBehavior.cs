using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CableBehavior : NetworkBehaviour {
	//[SyncVar(hook="SendToGo")]
	bool toGo = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (toGo)
			Destroy (this.gameObject);
	}
	void SendToGo() {
		toGo = true;
	}
}
