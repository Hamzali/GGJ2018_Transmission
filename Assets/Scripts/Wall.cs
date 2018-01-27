using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	
	public GameObject leftWall, rightWall, upWall, downWall;

	public void setWalls(int left, int right, int up, int down) {
		leftWall.SetActive(right == 0);
		rightWall.SetActive(left == 0);
		upWall.SetActive(up == 0);
		downWall.SetActive(down == 0);
	}
}
