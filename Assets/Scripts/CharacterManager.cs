using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnPlayerConnected() {
		foreach(var ps in GameObject.FindGameObjectsWithTag("Player")) {
			int charType = ps.GetComponent<CharacterAbility> ().charType;
			if (charType == 1) {
				GetComponent<SpriteRenderer> ().color = Color.blue;


			} else {
				GetComponent<SpriteRenderer>().color = Color.yellow;
			}
		}
	}
}
