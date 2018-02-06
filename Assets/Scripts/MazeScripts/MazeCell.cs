using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

	public GameObject Top, Bottom, Left, Right;
	public int x, y;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetCellScale(float cellWidth, float cellHeight, float wallThickness) {
		// transform.localScale = new Vector3(s, s, 1);

		Top.transform.localScale = new Vector3(2 * wallThickness + cellWidth, wallThickness, 1);
		Top.transform.localPosition = new Vector3(0, cellHeight / 2 + wallThickness / 2, 0);

		Bottom.transform.localScale = new Vector3(2 * wallThickness + cellWidth, wallThickness, 1);
		Bottom.transform.localPosition = new Vector3(0, -(cellHeight / 2 + wallThickness / 2), 0);

		Left.transform.localScale = new Vector3(wallThickness, 2 * wallThickness + cellHeight, 1);
		Left.transform.localPosition = new Vector3(-(cellWidth / 2 + wallThickness / 2), 0, 0);

		Right.transform.localScale = new Vector3(wallThickness, 2 * wallThickness + cellHeight, 1);
		Right.transform.localPosition = new Vector3(cellWidth / 2 + wallThickness / 2, 0, 0);
		
	}

	public void SetTopWall(bool wall) {
		Top.SetActive(wall);
	}
	public void SetBottomWall(bool wall) {
		Bottom.SetActive(wall);
	}
	public void SetLeftWall(bool wall) {
		Left.SetActive(wall);
	}
	public void SetRightWall(bool wall) {
		Right.SetActive(wall);
	}
}
