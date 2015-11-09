using UnityEngine;
using System.Collections;

public class boardgenerator : MonoBehaviour {

	void Start () {
		GameObject.Instantiate (Resources.Load ("tileB"), new Vector2 (0f, 0f), new Quaternion(0f,0f,0f,0f));

		float startx=-4f, starty=3f; 
		for (int i=0; i<9; i++) {
			for (int j=0; j<7; j++) {
				if ((j + i) % 2 == 0)
					GameObject.Instantiate (Resources.Load ("tileB"), new Vector2 (startx, starty), new Quaternion (0f, 0f, 0f, 0f));
				else
					GameObject.Instantiate (Resources.Load ("tileW"), new Vector2 (startx, starty), new Quaternion (0f, 0f, 0f, 0f));
				starty-=1f;
			}
			startx+=1f;
			starty=3f;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
