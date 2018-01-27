using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckerBehavior : NetworkBehaviour{
	public bool isActive = false;
	Animator ani;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetActive() {
		isActive = true;
		GetComponent<SpriteRenderer> ().color = Color.green;
	}

}
