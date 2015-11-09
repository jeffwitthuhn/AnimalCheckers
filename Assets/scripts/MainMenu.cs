using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class MainMenu : MonoBehaviour {
	public Toggle isGreenHumanToggle; 
	public Toggle isBlueHumanToggle; 
	public InputField infield; 
	// Use this for initialization
	public void setDepth(){
		int depth;
		string str = infield.text; 
		if (Int32.TryParse (str, out depth))
			gameObject.GetComponent<game> ().depth = depth;
	}
	public void reset(){
		Application.LoadLevel ("Main Menu");
	}
	public void StartGameGreenFirst(){
		game.turn = 1;
		toggleBlueHuman ();
		toggleGreenHuman ();
		Application.LoadLevel ("game");
	}
	public void StartGameBlueFirst(){
		game.turn = 2;
		toggleBlueHuman ();
		toggleGreenHuman ();
		Application.LoadLevel ("game");
	}

	public void toggleGreenHuman(){
		if (isGreenHumanToggle.isOn)
			game.isHumanGreen = true;
		else 
			game.isHumanGreen = false;
	}
	public void toggleBlueHuman(){
		if (isBlueHumanToggle.isOn)
			game.isHumanBlue = true;
		else 
			game.isHumanBlue = false;
	}
	public void quitgame(){
		Application.Quit ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("escape"))
			quitgame ();
	}
}
