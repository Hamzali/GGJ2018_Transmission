﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	Rigidbody2D rb;
	bool onAir;
	bool wallCollision;
	Vector2 collisionNormal;
	public float jumpPowerUp;
	public float jumpPowerNormal;
	public float speedMultipler;
	Animator ani;
	public float speedLimit;
	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			return;
		} else {
			//Vector2 collisionNormal = new Vector2(Get0,0);
			GetComponent<AudioListener> ().enabled = true;
			rb = this.GetComponent<Rigidbody2D> ();
			ani = GetComponent<Animator> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		//transform.Rotate(0, 0, -x);.
		transform.position = transform.position + Vector3.right*x*6.0f*Time.deltaTime;

		if (Mathf.Abs (x) > 0) {
			ani.SetBool ("isMoving", true);	
			/*if (rb.velocity.magnitude < speedLimit ) {
				print (!wallCollision);
				if (wallCollision) {
					rb.velocity += Vector2.right * x * speedMultipler;	
				} else {
					rb.velocity += Vector2.right * x * speedMultipler/10;	
				}
			} */
			if (x < 0)
				GetComponent<SpriteRenderer> ().flipX = true;
			else GetComponent<SpriteRenderer> ().flipX = false;
		} else {
			ani.SetBool ("isMoving", false);	
		} 

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (wallCollision) {
				Jump ();
			} else {
				
			}
		}
	}
	void Jump() {
		rb.velocity += Vector2.up * jumpPowerUp - collisionNormal*jumpPowerNormal;
	}

	[Command]
	void CmdFire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		//bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 50;

		NetworkServer.Spawn (bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);        
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (isLocalPlayer) {
			if (col.gameObject.tag == "Wall") {
				wallCollision = true;
				ani.SetBool ("onAir", false);
				//GetComponent<Animator> ().SetBool ("onAir", false);
				collisionNormal = (col.contacts [0].point - new Vector2 (transform.position.x, transform.position.y)).normalized;
				print (collisionNormal);

			}
		}
	}
	void OnCollisionStay2D(Collision2D col) {
		if (isLocalPlayer) {
			if (col.gameObject.tag == "Wall") {
				wallCollision = true;
				//ani.SetBool ("onAir", false);	
				GetComponent<Animator> ().SetBool ("onAir", false);
				collisionNormal = (col.contacts [0].point - new Vector2 (transform.position.x, transform.position.y)).normalized;
			}
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (isLocalPlayer) {
			if (col.gameObject.tag == "Wall") {
				ani.SetBool ("onAir", true);	
				//GetComponent<Animator> ().SetBool ("onAir", true);
				wallCollision = false;
			}
		}
	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<SpriteRenderer>().color = Color.blue;
	}
}
