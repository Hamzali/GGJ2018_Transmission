using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CharacterAbility : NetworkBehaviour {
	public int charType;
	int cableCount;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame

	public GameObject currentCell, previousCell;
	public GameObject BuiltCable;
	// GameObject builtCableParent = new GameObject ("BuiltCables");

	bool builtMode = false;
	bool collectMode = true;

	void Update () {

		if (charType == 1) {
			if (Input.GetKeyDown (KeyCode.B)) {
				builtMode = !builtMode;
				collectMode = !collectMode;

			}

			if (builtMode && Input.GetKey(KeyCode.C)) {
				BuildCable ();
			}
		}

		int currX = (int) transform.position.x;
		int currY = (int) transform.position.y;

		if (currentCell == null) {
			currentCell = GameManager.instance.GetCellByIndex (currX, currY);
		} else if (currentCell.GetComponent<Wall> ().indexX != currX || currentCell.GetComponent<Wall> ().indexY != currY) {
			previousCell = currentCell;
			currentCell = GameManager.instance.GetCellByIndex (currX, currY);
		}

	}

	void BuildCable () {
		var currWall = currentCell.GetComponent<Wall> ();
		var prevWall = previousCell.GetComponent<Wall> ();
		if (!currWall.isBuilt && cableCount > 0) {
			// GameObject cable = null;
			Vector3 pos = Vector3.up;
			Quaternion rot = Quaternion.identity;
			bool isCreated = false;
			if (prevWall.indexX != currWall.indexX) {

				pos = prevWall.indexX < currWall.indexX ?
					currentCell.transform.position - new Vector3 (.5f, 0, 0) :
					currentCell.transform.position + new Vector3 (.5f, 0, 0);

				// cable = Instantiate (BuiltCable, pos, Quaternion.identity);
				rot = Quaternion.identity;

				currWall.isBuilt = true;
				isCreated = true;
			} else if (prevWall.indexY != currWall.indexY) {
				pos = prevWall.indexY < currWall.indexY ?
					currentCell.transform.position - new Vector3 (0, .5f, 0) :
					currentCell.transform.position + new Vector3 (0, .5f, 0);

				//cable = Instantiate (BuiltCable, pos, Quaternion.Euler (0, 0, 90));
				rot = Quaternion.Euler (0, 0, 90);
				currWall.isBuilt = true;
				
				isCreated = true;
			}

			if (isCreated) {
				GameManager.instance.UpdatePathDict(currentCell, true);
				CmdBuiltCableNetwork(pos, rot, currWall.indexX, currWall.indexY);
				cableCount--;

				if (GameManager.instance.CheckWinCodition()) {
					Debug.Log("Winnnn!!!");
				}
			}
		}
	}

	[Command]
	void CmdBuiltCableNetwork (Vector3 pos, Quaternion rot, int x, int y) {
		GameObject go = (GameObject)Instantiate (BuiltCable, pos, rot);

		NetworkServer.Spawn (go);
		
		go.AddComponent<BuiltCable>().SetIndex(x, y);
		go.AddComponent<BoxCollider2D>().isTrigger = true;
	}

	public override void OnStartLocalPlayer () {
		charType = GameObject.FindGameObjectsWithTag ("Player").Length;
		print ("CharType" + charType);
		if (charType == 1) {
			//GetComponent<SpriteRenderer> ().color = Color.blue;
			this.gameObject.GetComponent<Animator> ().SetInteger ("charType", 1);

		} else {
			this.gameObject.GetComponent<Animator> ().SetInteger ("charType", 2);
			//GetComponent<SpriteRenderer>().color = Color.yellow;
		}
		//GetComponent<SpriteRenderer>().color = Color.blue;
		if (!isServer) {
			foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Player")) {
				if (go == this.gameObject) {
					continue;

				}
				go.GetComponent<CharacterAbility> ().charType = 1;
				go.GetComponent<Animator> ().SetInteger ("charType", 1);
			}
		}

	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Cable" || col.gameObject.tag == "BuiltCable") {
			if (charType == 1 && collectMode) {
				takeCable (col.gameObject);
			}
		}

	}
	void OnTriggerStay2D (Collider2D col) {
		if (col.gameObject.tag == "Checker") {
			if (Input.GetKey (KeyCode.C)) {
				print ("CharType is " + charType.ToString ());
				if (charType == 2) {
					//col.gameObject.GetComponent<CheckerBehavior> ().SetActive();
					//CmdSetActive(col.gameObject);
					GameManager.instance.ActivatePath (col.gameObject);

				}
			}
		} else if (col.gameObject.tag == "Endpoint") {
			if (Input.GetKey (KeyCode.C)) {
				print ("CharType is " + charType.ToString ());
				if (charType == 2) {
					//col.gameObject.GetComponent<CheckerBehavior> ().SetActive();
					//CmdSetActive(col.gameObject);
					GameManager.instance.ActivatePath (col.gameObject);
				}
			}
		}
	}

	[ClientRpc]
	void RpcSetActive (GameObject go) {
		go.GetComponent<CheckerBehavior> ().SetActive ();
	}

	[Command]
	void CmdSetActive (GameObject go) {
		go.GetComponent<NetworkIdentity> ().AssignClientAuthority (connectionToClient);
		RpcSetActive (go);
		go.GetComponent<NetworkIdentity> ().RemoveClientAuthority (connectionToClient);

	}
	void takeCable (GameObject cable) {
		if (cable.tag == "BuiltCable") {
			var builtCable = cable.GetComponent<BuiltCable>();
			var cell = GameManager.instance.GetCellByIndex(builtCable.x, builtCable.y);
			cell.GetComponent<Wall>().isBuilt = false;
			GameManager.instance.UpdatePathDict(cell, false);
		}
		
		Destroy (cable);
		cableCount += 1;
		print ("Cable count is " + cableCount);
	}
	/*
	 
	[Command]
	public void CmdSetActive(GameObject go) {
		go.GetComponent<CheckerBehavior> ().SetActive ();
	}
	*/
}