using UnityEngine;
using System.Collections;

public class MouseEvents : MonoBehaviour {
	public Sprite tileBR, tileWR, tileB, tileW;
	string objectName;
	static Vector3 currentTile;
	bool hovering; 
	Vector2 nullTile;
	Vector3 bigScale;
	Vector3 normalScale;
	Vector3 position; 
	// Use this for initialization
	void Start () {
		objectName = gameObject.name.Substring (0, 5); 
		nullTile = new Vector2 (-10f,-10f);
		bigScale = new Vector3 (0.85f, 0.85f, 1f);
		normalScale = new Vector3 (0.8f, 0.8f, 1f);
		hovering = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hovering) {
			if (Input.GetMouseButtonDown (0)) {
				//Debug.Log("clicked tile: "+gameObject.transform.position);
				game.clickedTile = currentTile;
			}
		} 
	}
	void OnMouseEnter(){
		switch (objectName) {
		case "tileW":
			//Debug.Log ("mouseover White Tile");
			gameObject.GetComponent<SpriteRenderer>().sprite = tileWR;
			break;
		case "tileB": 
		//	Debug.Log ("mouseover Black Tile");
			gameObject.GetComponent<SpriteRenderer>().sprite = tileBR;
			break; 
		}
		hovering = true;
		currentTile = gameObject.transform.position;
		position = currentTile; 
		position.z -= 1;
		gameObject.transform.position = position;
		gameObject.transform.localScale = bigScale;
		//Debug.Log (currentTile+ "is current tile");


	}
	void OnMouseExit(){
		switch (objectName) {
		case "tileW":
			//Debug.Log ("mouseoff White Tile");
			gameObject.GetComponent<SpriteRenderer>().sprite = tileW;
			break;
		case "tileB": 
			//Debug.Log ("mouseoff Black Tile");
			gameObject.GetComponent<SpriteRenderer>().sprite = tileB;
			break; 
		}
		hovering = false;
		position=gameObject.transform.position;
		position.z += 1;
		gameObject.transform.position = position;
		gameObject.transform.localScale = normalScale;

		currentTile = nullTile;
	}

}
