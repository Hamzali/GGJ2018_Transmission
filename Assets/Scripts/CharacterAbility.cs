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
	void Update () {
		
	}
	public override void OnStartLocalPlayer()
	{
		charType = GameObject.FindGameObjectsWithTag ("Player").Length;

		if (charType == 1) {
			//GetComponent<SpriteRenderer> ().color = Color.blue;
			this.gameObject.GetComponent<Animator> ().SetInteger ("charType",1);


		} else {
			//GetComponent<SpriteRenderer>().color = Color.yellow;
		}
		//GetComponent<SpriteRenderer>().color = Color.blue;
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Cable") {
			if (charType == 1) {
				takeCable (col.gameObject);
			}
		}

	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Checker") {
			if (Input.GetKey (KeyCode.C)) {
				print ("CharType is " + charType.ToString());
				if (charType == 2) {
					//col.gameObject.GetComponent<CheckerBehavior> ().SetActive();
					//CmdSetActive(col.gameObject);
					GameManager.instance.ActivatePath(col.gameObject);

				}
			}
		} else if (col.gameObject.tag == "Endpoint") {
			if (Input.GetKey (KeyCode.C)) {
				print ("CharType is " + charType.ToString());
				if (charType == 2) {
					//col.gameObject.GetComponent<CheckerBehavior> ().SetActive();
					//CmdSetActive(col.gameObject);
					GameManager.instance.ActivatePath(col.gameObject);
				}
			}
		}
	}
	[ClientRpc]
	void RpcSetActive(GameObject go) {
		go.GetComponent<CheckerBehavior> ().SetActive ();
	}

	[Command]
	void CmdSetActive(GameObject go) {
		go.GetComponent<NetworkIdentity> ().AssignClientAuthority (connectionToClient);
		RpcSetActive (go);
		go.GetComponent<NetworkIdentity> ().RemoveClientAuthority (connectionToClient);

		
	}
	void takeCable(GameObject cable) {
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
