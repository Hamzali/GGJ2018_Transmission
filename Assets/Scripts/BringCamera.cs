using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BringCamera : NetworkBehaviour {
	Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = Camera.main.gameObject.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		} else {
			Camera.main.gameObject.transform.position = transform.position + offset;
		}
	}
}
