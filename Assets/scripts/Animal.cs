using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {
	bool hovering;
	public static Vector2 clickedAnimalPosition;
	// Use this for initialization
	Vector2 nullPosition;
	void Start () {
		hovering = false; 
		nullPosition = new Vector2 (-10f, -10f); 

	}
	
	// Update is called once per frame

	void Update(){

		if (hovering==true) {
			if (Input.GetMouseButtonDown (0)) {
				//Debug.Log ("clicked tile: "+gameObject.name);

				game.AnimalClicked = gameObject.transform.position;
			}
		} 

	}
	void OnMouseEnter(){
		//Debug.Log ("enter tile: "+gameObject.name);

		hovering = true; 
		//Debug.Log ("hovering: "+hovering);

		
	}
	void OnMouseExit(){
		//Debug.Log ("exit tile: "+gameObject.name);

		hovering = false; 
		//Debug.Log ("hovering: "+hovering);

	}

}
