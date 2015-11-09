using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine.UI;
public class intV3{
	public int sefscore;
	public Vector3 startpos;
	public Vector3 endpos;
	public intV3(int _sefscore, Vector3 _startpos, Vector3 _destination){
		sefscore = _sefscore;
		startpos = _startpos;
		endpos = _destination;
	}
}
public class game : MonoBehaviour {
	public Button reset; 
	public int depth;
	int minalpha;
	int maxbeta;
	public static Vector2 AnimalClicked;
	public static Vector2 clickedTile;
	Vector2 lastClickedAnimal;
	Vector2 lastClickedTile;
	public static bool isHumanGreen; 
	public static bool isHumanBlue; 
	Vector2 GreenPieceToMove;
	Vector2 GreenMoveDestination;
	Vector2 nullpos; 
	int greenpieceindex;
	int greendestindex;
	Vector2 BluePieceToMove;
	Vector2 BlueMoveDestination;
	int bluepieceindex;
	int bluedestindex;
	string winner;
	public string PlayerTurn; 
	public GameObject elephantgreen, mousegreen, wolfgreen, tigergreen,
		elephantblue, mouseblue, wolfblue, tigerblue; 
	public List<Vector2> blueEMWT, greenEMWT;
	public List<bool> deadblueEMWT, deadgreenEMWT;

	public Vector2 greenDen, blueDen;
	public static int turn; 
	public Text turndisplay;
	int greenPieceCount;
	int bluePieceCount;
	int state; 
	bool donecalculating;
	int selectGPiece,selectGMove, evalGMove, selectBPiece,selectBMove, evalBMove,complete, Gcalculating, Bcalculating;
	// Use this for initialization

	void Start () {
		nullpos = new Vector2 (-10f, -10f);
		minalpha = -5000000;
		maxbeta = 5000000;
		deadblueEMWT.Add (false);
		deadblueEMWT.Add (false);
		deadblueEMWT.Add (false);
		deadblueEMWT.Add (false);
		deadgreenEMWT.Add (false);
		deadgreenEMWT.Add (false);
		deadgreenEMWT.Add (false);
		deadgreenEMWT.Add (false);
		blueEMWT.Add (elephantblue.transform.position);
		blueEMWT.Add (mouseblue.transform.position);
		blueEMWT.Add (wolfblue.transform.position);
		blueEMWT.Add (tigerblue.transform.position);
		greenEMWT.Add (elephantgreen.transform.position);
		greenEMWT.Add (mousegreen.transform.position);
		greenEMWT.Add (wolfgreen.transform.position);
		greenEMWT.Add (tigergreen.transform.position);
		selectGPiece = 1;
		selectGMove = 2; 
		evalGMove = 3;

		selectBPiece = 4;
		selectBMove = 5;
		evalBMove = 6; 

		complete = 7;
		Gcalculating = 8;
		Bcalculating = 9;
		greenPieceCount = 4;
		bluePieceCount = 4;
		if (game.turn == 1) {
			PlayerTurn="Green Player";
			turndisplay.text="Turn: "+PlayerTurn;
			turndisplay.color=Color.green;
			state=selectGPiece;
		} else if (game.turn == 2) {
			PlayerTurn="Blue Player";
			turndisplay.text="Turn: "+PlayerTurn;
			turndisplay.color=Color.blue;
			state=selectBPiece; 

		} else 
			turndisplay.text = "error";
		lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
		lastClickedTile = clickedTile =new Vector2 (-10f, -10f);


	}
	
	// Update is called once per frame
	void Update () {
		//display turn
		if (state == 1 || state == 2 || state == 3||state==8) {
			PlayerTurn = "Green Player";
			turndisplay.text = "Turn: " + PlayerTurn;
			turndisplay.color = Color.green;
		} else {
			PlayerTurn="Blue Player";
			turndisplay.text="Turn: "+PlayerTurn;
			turndisplay.color=Color.blue;
		}
		if (state == 7) {
			turndisplay.text=winner;
			if (winner=="Blue Player Wins!"){
				turndisplay.color=Color.blue;
			}
			else {
				turndisplay.color=Color.green;
			}
		}

	//which state are we in?
		
	//STATE == PLAYER GREEN SELECTING A PIECE
		if (state == selectGPiece) {
			if (game.isHumanGreen) {
				if (lastClickedAnimal != AnimalClicked) {
					lastClickedAnimal = AnimalClicked;
					int selectedPiece = -1; 
					for (int i=0; i<4; i++) {
						if (AnimalClicked == greenEMWT [i])
							selectedPiece = i;
					}
					if (selectedPiece == -1) {
						Debug.Log ("green player tried to select blue piece");
					} else { //a green piece was selected 
						state = selectGMove;
						GreenPieceToMove = greenEMWT [selectedPiece]; 
						greenpieceindex = selectedPiece;
						lastClickedAnimal = AnimalClicked = new Vector2 (-10f, -10f);
						lastClickedTile = clickedTile = new Vector2 (-10f, -10f);

						//Debug.Log ("selected a green piece");
					}
				}
			} else{

				donecalculating=false;
				calcGreenMove();
				state = Gcalculating;
			}
			//STATE == PLAYER GREEN SELECTING A MOVE OR CHANGING PIECE
		} else if (state == selectGMove) {
			if (game.isHumanGreen) {
				//an animal was clicked
				if (lastClickedAnimal != AnimalClicked) {
					lastClickedAnimal = AnimalClicked;
					int selectedPiece = -1; 
					for (int i=0; i<4; i++) {
						if (AnimalClicked == blueEMWT [i])
							selectedPiece = i;
					}
					if (selectedPiece == -1) {
						for (int i=0; i<4; i++) {
							if (AnimalClicked == greenEMWT [i])
								selectedPiece = i;
						}
						if (selectedPiece == -1) {
							Debug.Log ("error in indexes");
						} else { //a green piece was selected 
							state = selectGMove;
							GreenPieceToMove = greenEMWT [selectedPiece]; 
							greenpieceindex = selectedPiece;
							lastClickedAnimal = AnimalClicked = new Vector2 (-10f, -10f);
							lastClickedTile = clickedTile = new Vector2 (-10f, -10f);
						//	Debug.Log ("selected a green piece");
						}
					} else {
						state = evalGMove;
						GreenMoveDestination = blueEMWT [selectedPiece];
						greendestindex = selectedPiece;
						lastClickedAnimal = AnimalClicked = new Vector2 (-10f, -10f);
						lastClickedTile = clickedTile = new Vector2 (-10f, -10f);
					//	Debug.Log ("trying to move from " + GreenPieceToMove + " to " + GreenMoveDestination);

					}

				}
			//a tile was clicked
				else if (lastClickedTile != clickedTile) {
					lastClickedTile = clickedTile;
					GreenMoveDestination = clickedTile;
					state = evalGMove;
					lastClickedAnimal = AnimalClicked = new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile = new Vector2 (-10f, -10f);
					greendestindex = -1;
					//Debug.Log ("trying to move from " + GreenPieceToMove + " to " + GreenMoveDestination);

				}
			} else{
				donecalculating=false;
				calcGreenMove();
				state = Gcalculating;
			}

//STATE==GREEN AI CALCULATING
		}else if(state==Gcalculating){
			if(donecalculating)
				state=evalGMove;

//STATE == EVALUATING GREEN MOVE
		} else if (state == evalGMove) {
			bool valid = false; 
			Vector2 startpos = GreenPieceToMove;
			Vector2 endpos = GreenMoveDestination;
			//check if the piece could move here if space is empty
			if (endpos.x >= -4 && endpos.x <= 4 && endpos.y >= -3 && endpos.y <= 3 && (
				(endpos.y == startpos.y) && ((endpos.x == startpos.x + 1) || (endpos.x == startpos.x - 1)) || 
				(endpos.x == startpos.x) && ((endpos.y == startpos.y + 1) || (endpos.y == startpos.y - 1))
				)) {
				//if desintation space is empty it can move there
				if (greendestindex == -1)
					valid = true;
				else {//else check if it can move on top of other piece
					//greenEMWT 
					//0E->T:W:E
					//1M->E:M
					//2W->W:M
					//3T->T:W:M
					//Debug.Log ("green piece index is:"+greenpieceindex);
					//Debug.Log ("green destination index is:"+greendestindex);

					switch (greenpieceindex) {
					case 0: 
						if (greendestindex == 1)
							valid = false;
						else
							valid = true; 
						break;
					case 1: 
						if (greendestindex == 2 || greendestindex == 3)
							valid = false;
						else
							valid = true;
						break;
					case 2: 
						if (greendestindex == 0 || greendestindex == 3)
							valid = false;
						else
							valid = true;

						break; 
					case 3:
						if (greendestindex == 0)
							valid = false;
						else
							valid = true;
						break; 
					
					}



				}
			}
			if (valid) {


				greenEMWT [greenpieceindex] = GreenMoveDestination;
				switch (greenpieceindex) {
				case 0: 
					elephantgreen.transform.position = GreenMoveDestination;
					break;
				case 1: 
					mousegreen.transform.position = GreenMoveDestination;
					break;
				case 2: 
					wolfgreen.transform.position = GreenMoveDestination;
					break;
				case 3: 
					tigergreen.transform.position = GreenMoveDestination;
					break;
				}
				if (greendestindex != -1){
					switch (greendestindex) {
					case 0: 
						Destroy (elephantblue);
						bluePieceCount--;
						break;
					case 1: 
						Destroy (mouseblue);
						bluePieceCount--;
					break;
					case 2: 
						Destroy (wolfblue);
						bluePieceCount--;
						break;
					case 3: 
						Destroy (tigerblue);
						bluePieceCount--;
						break;
					}
					blueEMWT[greendestindex]=nullpos;
					deadblueEMWT[greendestindex]=true;
					 
				}

				if(GreenMoveDestination==blueDen||bluePieceCount==0){
					state = complete;
					lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
					winner="Green Player Wins!";
				}
				else{
					state=selectBPiece;
					lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
					Debug.Log ("Green finishes, Blue's turn");
				}

			} else {
				Debug.Log ("invalid move, back to choose green move");
				state = selectGMove;
				lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
				lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
			}



//STATE == BLUE PLAYER SELECTING A PIECE TO MOVE
		} else if (state == selectBPiece) {
			if(game.isHumanBlue){
				if (lastClickedAnimal != AnimalClicked) {
					lastClickedAnimal = AnimalClicked;
					int selectedPiece = -1; 
					for (int i=0; i<4; i++) {
						if (AnimalClicked == blueEMWT [i])
							selectedPiece = i;
					}
					if (selectedPiece == -1) {
						Debug.Log ("blue player tried to select green piece");
					} else { //a green piece was selected 
						state = selectBMove;
						BluePieceToMove = blueEMWT [selectedPiece]; 
						bluepieceindex = selectedPiece;
						lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
						lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
						
						Debug.Log ("selected a blue piece");
					}
				}
			}else{
				donecalculating=false;
				calcBlueMove();
				state = Bcalculating;
			}

		
//STATE == BLUE PLAYER SELECTING A DESTINATION OR A DIFFERENT PIECE
		} else if (state == selectBMove) {
			if(game.isHumanBlue){
				//an animal was clicked
				if (lastClickedAnimal != AnimalClicked) {
					lastClickedAnimal = AnimalClicked;
					int selectedPiece = -1; 
					for (int i=0; i<4; i++) {
						if (AnimalClicked == greenEMWT [i])
							selectedPiece = i;
					}
					if (selectedPiece == -1) {
						for (int i=0; i<4; i++) {
							if (AnimalClicked == blueEMWT [i])
								selectedPiece = i;
						}
						if (selectedPiece == -1) {
							Debug.Log ("error in indexes");
						} else { //a blue piece was selected 
							state = selectBMove;
							BluePieceToMove = blueEMWT [selectedPiece]; 
							bluepieceindex = selectedPiece;
							lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
							lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
							Debug.Log ("selected a blue piece");
						}
					} else {
						state = evalBMove;
						BlueMoveDestination = greenEMWT [selectedPiece];
						bluedestindex = selectedPiece;
						lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
						lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
						Debug.Log ("trying to move from " + BluePieceToMove + " to " + BlueMoveDestination);
						
					}
					
				}
				//a tile was clicked
				else if (lastClickedTile != clickedTile) {
					lastClickedTile = clickedTile;
					BlueMoveDestination = clickedTile;
					state = evalBMove;
					lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
					bluedestindex = -1;
					Debug.Log ("trying to move from " + BluePieceToMove + " to " + BlueMoveDestination);
					
				}
			}else{
				donecalculating=false;
				calcBlueMove();
				state = Bcalculating;
			}
			
//STATE==BLUE AI CALCULATING
		}else if(state==Bcalculating){
			if(donecalculating)
				state=evalBMove;

			
//EVALUATING BLUE MOVE
		} else if (state == evalBMove) {
			bool valid = false; 
			Vector2 startpos = BluePieceToMove;
			Vector2 endpos = BlueMoveDestination;
			//check if the piece could move here if space is empty
			if (endpos.x >= -4 && endpos.x <= 4 && endpos.y >= -3 && endpos.y <= 3 && (
				(endpos.y == startpos.y) && ((endpos.x == startpos.x + 1) || (endpos.x == startpos.x - 1)) || 
				(endpos.x == startpos.x) && ((endpos.y == startpos.y + 1) || (endpos.y == startpos.y - 1))
				)) {
				//if desintation space is empty it can move there
				if (bluedestindex == -1)
					valid = true;
				else {//else check if it can move on top of other piece
					//blueEMWT 
					//0E->T:W:E
					//1M->E:M
					//2W->W:M
					//3T->T:W:M
					Debug.Log ("blue piece index is:"+bluepieceindex);
					Debug.Log ("blue destination index is:"+bluedestindex);
					
					switch (bluepieceindex) {
					case 0: 
						if (bluedestindex == 1)
							valid = false;
						else
							valid = true; 
						break;
					case 1: 
						if (bluedestindex == 2 || bluedestindex == 3)
							valid = false;
						else
							valid = true;
						break;
					case 2: 
						if (bluedestindex == 0 || bluedestindex == 3)
							valid = false;
						else
							valid = true;
						
						break; 
					case 3:
						if (bluedestindex == 0)
							valid = false;
						else
							valid = true;
						break; 
						
					}
					
					
					
				}
			}
			if (valid) {
				
				
				blueEMWT [bluepieceindex] = BlueMoveDestination;
				switch (bluepieceindex) {
				case 0: 
					elephantblue.transform.position = BlueMoveDestination;
					break;
				case 1: 
					mouseblue.transform.position = BlueMoveDestination;
					break;
				case 2: 
					wolfblue.transform.position = BlueMoveDestination;
					break;
				case 3: 
					tigerblue.transform.position = BlueMoveDestination;
					break;
				}
				if (bluedestindex != -1){
					switch (bluedestindex) {
						case 0: 
						Destroy (elephantgreen);
						greenPieceCount--;
						break;
						case 1: 
						Destroy (mousegreen);
						greenPieceCount--;
						break;
						case 2: 
						Destroy (wolfgreen);
						greenPieceCount--;
						break;
						case 3: 
						Destroy (tigergreen);
						greenPieceCount--;
						break;
							}
						greenEMWT[bluedestindex]=nullpos;
						deadgreenEMWT[bluedestindex]=true;
					}
				if(BlueMoveDestination==greenDen||greenPieceCount==0){
					state = complete;
					lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
					winner="Blue Player Wins!";
				}
				else{
					state=selectGPiece;
					lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
					lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
					Debug.Log ("Blue finishes, Green's turn");
				}
				
			} else {
				Debug.Log ("invalid move, back to choose blue move");
				Debug.Log ("starpos: "+startpos);
				Debug.Log ("endpos: "+endpos);


				state = selectBMove;
				lastClickedAnimal = AnimalClicked =new Vector2 (-10f, -10f);
				lastClickedTile = clickedTile =new Vector2 (-10f, -10f);
			}
			
			


		} else if (state == complete) {
			turndisplay.text=winner;
		}


		
	
	}
	int SEF(List<Vector2> GP, List<bool> deadGP, List<Vector2> BP, List<bool> deadBP){
		int total;
		//greenEMWT
		total = 0;
		float dangerouspiecedistance;
		int weightedGPcount=0, weightedBPcount=0;
		int weightedPieceDifference; //maximize
		int distancemeasure;
		List<int> BdistancesfromGden =new List<int>(){0,0,0,0}; // minimize
		List<int> GdistancesfromBden =new List<int>(){0,0,0,0}; //maximize
		int distancefromBlueDenAdvangage;
		int distancefromGreenDenAdvantage;
		int winloss;
		winloss=0;
		int closedangerouspieces=0;

	

		if (!deadGP [0]) {
			weightedGPcount += 3;
			GdistancesfromBden[0]=(int)(Math.Abs(GP[0].x-4f)+Math.Abs (GP[0].y));
			if(GdistancesfromBden[0]<2)
				winloss-=5000;
			if(!deadBP[1])
				if(((Math.Abs(BP[1].x-GP[0].x)+Math.Abs (BP[1].y-GP[0].y))<3))
					if(((Math.Abs(BP[1].x-GP[0].x)+Math.Abs (BP[1].y-GP[0].y))<2))
						closedangerouspieces+=300;
				else
				   closedangerouspieces+=30; 

		}
		if (!deadGP [1]) {
			weightedGPcount += 2;
			GdistancesfromBden[1]=(int)(Math.Abs(GP[1].x-4f)+Math.Abs (GP[1].y));
			if(GdistancesfromBden[1]<2)
				winloss-=5000;
			if(!deadBP[2]){
				dangerouspiecedistance=(((Math.Abs(BP[2].x-GP[1].x)+Math.Abs (BP[2].y-GP[1].y))));
				if(dangerouspiecedistance<3)
					if(dangerouspiecedistance<2)
						closedangerouspieces+=200;
				else
					closedangerouspieces+=20;
			}
			if(!deadBP[3]){
				dangerouspiecedistance=(((Math.Abs(BP[3].x-GP[1].x)+Math.Abs (BP[3].y-GP[1].y))));
				if(dangerouspiecedistance<3)
					if(dangerouspiecedistance<2)
						closedangerouspieces+=200;
				else
					closedangerouspieces+=20;
			
			}
		}
		if (!deadGP [2]) {
			weightedGPcount += 1;
			GdistancesfromBden[2]=(int)(Math.Abs(GP[2].x-4f)+Math.Abs (GP[2].y));
			if(GdistancesfromBden[2]<2)
				winloss-=5000;
			if(!deadBP[0]){
				dangerouspiecedistance=(((Math.Abs(BP[0].x-GP[2].x)+Math.Abs (BP[0].y-GP[2].y))));
				if(dangerouspiecedistance<3)
					if(dangerouspiecedistance<2)
						closedangerouspieces+=100;
				else
					closedangerouspieces+=10;

			}
			if(!deadBP[3]){
				dangerouspiecedistance=(((Math.Abs(BP[3].x-GP[2].x)+Math.Abs (BP[3].y-GP[2].y))));
				if(dangerouspiecedistance<3)
					if(dangerouspiecedistance<2)
						closedangerouspieces+=100;
				else
					closedangerouspieces+=10;
			
			}


		}
		if (!deadGP [3]) {
			weightedGPcount += 2;
			GdistancesfromBden[3]=(int)(Math.Abs(GP[3].x-4f)+Math.Abs (GP[3].y));
			if(GdistancesfromBden[3]<2)
				winloss-=5000;
			if(!deadBP[0]){
				dangerouspiecedistance=(((Math.Abs(BP[0].x-GP[3].x)+Math.Abs (BP[0].y-GP[3].y))));
				if(dangerouspiecedistance<3)
					if(dangerouspiecedistance<2)
						closedangerouspieces+=200;
				else
					closedangerouspieces+=20;

			}


		}

		if (!deadBP [0]) {
			weightedBPcount += 3;
			BdistancesfromGden[0]=(int)(Math.Abs(BP[0].x+4f)+Math.Abs (BP[0].y));
			if(BdistancesfromGden[0]<2)
				winloss+=5000;
			if(!deadGP[1])
				if(((Math.Abs(BP[0].x-GP[1].x)+Math.Abs (BP[0].y-GP[1].y))<3)){
					if(((Math.Abs(BP[0].x-GP[1].x)+Math.Abs (BP[0].y-GP[1].y))<2))
						closedangerouspieces-=300; 
					else closedangerouspieces-=30; 
				}
		}
		if (!deadBP [1]) {
			weightedBPcount += 2;
			BdistancesfromGden[1]=(int)(Math.Abs(BP[1].x+4f)+Math.Abs (BP[1].y));
			if(BdistancesfromGden[1]<2)
				winloss+=5000;
			if(!deadGP[2]){
				dangerouspiecedistance=(Math.Abs(BP[1].x-GP[2].x)+Math.Abs (BP[1].y-GP[2].y));
				if((dangerouspiecedistance<3))
					if(dangerouspiecedistance<2)
						closedangerouspieces-=200;
					else
					closedangerouspieces-=20; 

			}
			if(!deadGP[3]){
				dangerouspiecedistance=(Math.Abs(BP[1].x-GP[3].x)+Math.Abs (BP[1].y-GP[3].y));
				if((dangerouspiecedistance<3))
					if(dangerouspiecedistance<2)
						closedangerouspieces-=200;
					else
					closedangerouspieces-=20; 

			}

		}
		if (!deadBP [2]) {
			weightedBPcount += 1;
			BdistancesfromGden[2]=(int)(Math.Abs(BP[2].x+4f)+Math.Abs (BP[2].y));
			if(BdistancesfromGden[2]<2)
				winloss+=5000;
			if(!deadGP[3]){
				dangerouspiecedistance=(Math.Abs(BP[2].x-GP[3].x)+Math.Abs (BP[2].y-GP[3].y));
				if((dangerouspiecedistance<3))
					if(dangerouspiecedistance<2)
						closedangerouspieces-=100;
					else
					closedangerouspieces-=10; 

			}
			if(!deadGP[0]){
					dangerouspiecedistance=((Math.Abs(BP[2].x-GP[0].x)+Math.Abs (BP[2].y-GP[0].y)));
					if((dangerouspiecedistance<3))
							if(dangerouspiecedistance<2)
								closedangerouspieces-=100;
						else
							closedangerouspieces-=10; 

			}

		}
		if (!deadBP [3]) {
			weightedBPcount += 2;
			BdistancesfromGden[3]=(int)(Math.Abs(BP[3].x+4f)+Math.Abs (BP[3].y));
			if(BdistancesfromGden[3]<2)
				winloss+=5000;
			if(!deadGP[0]){
						dangerouspiecedistance=(Math.Abs(BP[3].x-GP[0].x)+Math.Abs (BP[3].y-GP[0].y));
						if((dangerouspiecedistance<3))
							if(dangerouspiecedistance<2)
								closedangerouspieces-=200;
						else
							closedangerouspieces-=20; 
			}

		}
		for(int i=0; i<4; i++){
				if(BP[i]==greenDen)
					winloss+=1000000;
			}
		for(int i=0; i<4; i++){
				if(GP[i]==blueDen)
					winloss+=-1000000;
			}
		int randomness = UnityEngine.Random.Range (-20,20); 
		distancemeasure = 0 - BdistancesfromGden [0] * 2 - BdistancesfromGden [1] * 2 - BdistancesfromGden [2] - BdistancesfromGden [3] * 2
			+ GdistancesfromBden [0] * 2 + GdistancesfromBden [1] * 2 + GdistancesfromBden [2] + GdistancesfromBden [3] * 2;
		weightedPieceDifference = weightedBPcount - weightedGPcount;
		total += randomness + weightedPieceDifference * 1000 + distancemeasure*10+winloss+closedangerouspieces*10;
		/*
		if (total == 0)

			Debug.Log ("total is zero,\n weightedPieceDifference= "
				+ weightedPieceDifference
				+ "\ndistancemeasure= " + distancemeasure
				+ "\nwinloss = " + winloss
			           + "\nclosedangerouspieces =" + closedangerouspieces
			           +"\nGP IS " +GP[0]+", "+GP[1]+", "+GP[2]+", "+GP[3]
			           +"\nBP IS "+BP[0]+", "+BP[1]+", "+BP[2]+", "+BP[3]
			         //  +"\nGdistancesfromBden IS " +GdistancesfromBden[0]+", "+GdistancesfromBden[1]+", "+GdistancesfromBden[2]+", "+GdistancesfromBden[3]
			           +"\nBdistancesfromGden IS "+BdistancesfromGden[0]+", "+BdistancesfromGden[1]+", "+BdistancesfromGden[2]+", "+BdistancesfromGden[3]


			           );
        */
			 
		return total;
	}
	intV3 min_alpha_beta(List<Vector2> GP, List<bool> deadGP, List<Vector2> BP, List<bool> deadBP,
	                   int alpha, int beta, int level, int depth,bool inverted){
		//Debug.Log ("inside of min_alpha_beta at depth: "+level);
		bool bestvalchanged = false;
		int valid; 
		int value; 
		intV3 best_move=new intV3(6000000, nullpos,nullpos);
		intV3 valandmove;
		Vector2 ghost=nullpos;
		Vector2 destination,saved;
		bool gameOVER = false;
		for (int i=0; i<4; i++) {
			if(GP[i]==blueDen)
				gameOVER=true;
		}
		for (int i=0; i<4; i++) {
			if(BP[i]==greenDen)
				gameOVER=true;
			
		}
		if (level == depth||gameOVER==true) {
			if(inverted)
				return new intV3 ((-1)*SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
			else
				return new intV3 (SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
		}
		//for each possible play of the opponent(opponent assumed green, opponent GP)
		for (int i=0; i<4; i++) {//try to move each piece up
			if(!deadGP[i]){
			destination=saved=GP[i];

			destination.y+=1;
			valid=checkPlay (BP,GP,i,destination); 
			
			if(valid>=0){
				//make play
				GP[i]=destination;
				if(valid<5){
					ghost=BP[valid];
					BP[valid]=nullpos;
					deadBP[valid]=true;
				}
				//call recursively
				valandmove=max_alpha_beta (GP,deadGP,BP,deadBP,alpha,beta,level+1, depth,inverted);
				value=valandmove.sefscore;
				//Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));

					//Debug.Log ("value, beta: "+value+","+beta);

				//rescind the play
				GP[i]=saved;
				if(valid<5){
					BP[valid]=ghost;
					deadBP[valid]=false;
				}
				//calculations
				if(beta>value){
					beta=value;
						bestvalchanged=true;
					best_move=new intV3(value,saved,destination);
					

				}
				if(beta<=alpha)
					return new intV3(alpha, nullpos,nullpos);
			}
		  }
		}
		for (int i=0; i<4; i++) {//try to move each piece down
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.y -= 1;
				valid = checkPlay (BP, GP, i, destination); 
			
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = max_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
				//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));

					//Debug.Log ("value, beta: "+value+","+beta);

					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (beta > value) {
						beta = value;
						bestvalchanged=true;

						best_move = new intV3 (value, saved, destination);


					}
					if (beta <= alpha)
						return new intV3 (alpha, nullpos, nullpos);
				}
			}
		}
		for (int i=0; i<4; i++) {//try to move each piece left
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.x -= 1;
				valid = checkPlay (BP, GP, i, destination); 
			
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = max_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
				//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));

					//Debug.Log ("value, beta: "+value+","+beta);
					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (beta > value) {
						beta = value;
						bestvalchanged=true;

						best_move = new intV3 (value, saved, destination);


					}
					if (beta <= alpha)
						return new intV3 (alpha, nullpos, nullpos);
				}
			}
		}
		for (int i=0; i<4; i++) {//try to move each piece right
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.x += 1;
				valid = checkPlay (BP, GP, i, destination); 
			
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = max_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
				//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));

				//	Debug.Log ("value, beta: "+value+","+beta);

					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (beta > value) {
						beta = value;

						bestvalchanged=true;

						best_move = new intV3 (value, saved, destination);
				

					}
					if (beta <= alpha)
						return new intV3 (alpha, nullpos, nullpos);
				}
			}
		}
		/*Debug.Log ("returning best_move\n"+
		           "best.move.startpos: "+best_move.startpos+
		           "\nbest.move.startpos: "+best_move.startpos+
		           "\nbest.move.endpos: "+best_move.endpos+
		           "\nbest.move.value: "+best_move.sefscore);
	*/
		/*if (!bestvalchanged)
			Debug.Log ("best val not changed..\n GP:" + GP [0] + GP [1] + GP [2] + GP [3]
				+ "\ndeadGP" + deadGP [0] + deadGP [1] + deadGP [2] + deadGP [3]
				+ "\nBP" + BP [0] + BP [1] + BP [2] + BP [3]
				+ "\ndeadBP" + deadBP [0] + deadBP [1] + deadBP [2] + deadBP [3]
				+ "\nalpha:" + alpha
				+ "\nbeta:" + beta
				+ "\nlevel:" + level
				+ "\ndepth:" + depth
			);*/
		return best_move; 

	}
	intV3 max_alpha_beta(List<Vector2> GP, List<bool> deadGP, List<Vector2> BP, List<bool> deadBP,
	                   int alpha, int beta, int level, int depth, bool inverted){
		int valid; 
		int value; 
		intV3 best_move=new intV3(-6000000, nullpos,nullpos);
		intV3 valandmove;
		Vector2 ghost=nullpos;
		Vector2 destination,saved;
		bool gameOVER = false;
		for (int i=0; i<4; i++) {
			if(GP[i]==blueDen)
				gameOVER=true;
		}
		for (int i=0; i<4; i++) {
			if(BP[i]==greenDen)
				gameOVER=true;
			
		}
		if(level==depth||gameOVER==true){
			if(inverted)
				return new intV3 ((-1)*SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
			else
				return new intV3 (SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
		}		//for each possible play of the program(program is assumed blue, program BP)
		for (int i=0; i<4; i++) {//try to move each piece up
			if (!deadBP [i]) {
				destination = saved = BP [i];
			//	Debug.Log ("saved is " + saved);
				destination.y += 1;
				valid = checkPlay (GP, BP, i, destination); 
				//Debug.Log ("moving BP:" + i + " up, valid: " + valid);
				if (valid >= 0) {
					//make play
					BP [i] = destination;
					if (valid < 5) {
						ghost = GP [valid];
						GP [valid] = nullpos;
						deadGP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
				//	Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));

					//Debug.Log ("value, alpha: "+value+","+alpha);

					//rescind the play
					BP [i] = saved;
					if (valid < 5) {
						GP [valid] = ghost;
						deadGP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;

						best_move = new intV3 (value, saved, destination);

					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}

			}
		}
		for (int i=0; i<4; i++) {//try to move each piece down
			if (!deadBP [i]) {
				destination = saved = BP [i];
				destination.y -= 1;
				valid = checkPlay (GP, BP, i, destination); 
				if (valid >= 0) {
					//make play
					BP [i] = destination;
					if (valid < 5) {
						ghost = GP [valid];
						GP [valid] = nullpos;
						deadGP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
				//	Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));

					//Debug.Log ("value, alpha: "+value+","+alpha);

					//rescind the play
					BP [i] = saved;
					if (valid < 5) {
						GP [valid] = ghost;
						deadGP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;
					
						best_move = new intV3 (value, saved, destination);


					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}

			}
		}
		for (int i=0; i<4; i++) {//try to move each piece left
			if (!deadBP [i]) {
				destination = saved = BP [i];
				destination.x -= 1;
				valid = checkPlay (GP, BP, i, destination); 
				if (valid >= 0) {
					//make play
					BP [i] = destination;
					if (valid < 5) {
						ghost = GP [valid];
						GP [valid] = nullpos;
						deadGP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
					//Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));

					//rescind the play
					BP [i] = saved;
					if (valid < 5) {
						GP [valid] = ghost;
						deadGP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;

						best_move = new intV3 (value, saved, destination);


					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}

			}
		}
		for (int i=0; i<4; i++) {//try to move each piece right
			if (!deadBP [i]) {
				destination = saved = BP [i];
				destination.x += 1;
				valid = checkPlay (GP, BP, i, destination); 
				if (valid >= 0) {
					//make play
					BP [i] = destination;
					if (valid < 5) {
						ghost = GP [valid];
						GP [valid] = nullpos;
						deadGP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_beta (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
					//Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));

					//Debug.Log ("value, alpha: "+value+","+alpha);

					//rescind the play
					BP [i] = saved;
					if (valid < 5) {
						GP [valid] = ghost;
						deadGP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;

						best_move = new intV3 (value, saved, destination);


					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				
				}

			}
		}
		return best_move; 
	}

	int checkPlay(List<Vector2> GP, List<Vector2> BP, int pieceindex, Vector2 destination){
		//returns: -1 invalid play, 5 move to a tile, else index of animal to destroy
		if (destination.y >= -3 && destination.y <= 3 && destination.x >= -4 && destination.x <= 4) {

			bool BPcolide=false; 
			bool GPcolide=false;
			int colindex=-1;
			for (int i=0; i<4; i++) {
				if(BP[i]==destination)
					BPcolide=true; 
			}
			for (int i=0; i<4; i++) {
				if(GP[i]==destination){
					GPcolide=true; 
					colindex=i;
				}
			}
			if(BPcolide)
				return -1;
			if(GPcolide){
				switch(pieceindex){
				case 0:
					if(colindex==1)
						return -1; 
					else return colindex; 
					break;
				case 1:
					if(colindex==2||colindex==3)
						return -1; 
					else return colindex;
					break;
				case 2: 
					if(colindex==3||colindex==0)
						return -1;
					else return colindex;
					break;
				case 3: 
					if(colindex==0)
						return -1;
					else return colindex;
					break;

				
				}
			}
			else return 5;

		} else
			return -1; 
		return -1;
	}

	void calcGreenMove(){
		intV3 move = max_alpha_betaG (greenEMWT, deadgreenEMWT, blueEMWT, deadblueEMWT, minalpha, maxbeta, 0, depth, true);
		GreenMoveDestination = move.endpos;
		GreenPieceToMove = move.startpos;
		for (int i=0; i<4; i++) {
			if(greenEMWT[i]==GreenPieceToMove)
				greenpieceindex=i;
		}
		greendestindex = -1;
		for (int i=0; i<4; i++) {
			if(blueEMWT[i]==GreenMoveDestination)
				greendestindex=i;
		}
		Debug.Log ("green move calculated:\nvalue:"+
		           move.sefscore+
		           "\nstartpos: "+move.startpos+
		           "\nendpos"+move.endpos);
		donecalculating=true;
	}
void calcBlueMove(){
		intV3 move = max_alpha_beta (greenEMWT, deadgreenEMWT, blueEMWT, deadblueEMWT, minalpha, maxbeta, 0, depth, false);
		BlueMoveDestination = move.endpos;
		BluePieceToMove = move.startpos;
		for (int i=0; i<4; i++) {
			if(blueEMWT[i]==BluePieceToMove)
				bluepieceindex=i;
		}
		bluedestindex = -1;
		for (int i=0; i<4; i++) {
			if(greenEMWT[i]==BlueMoveDestination)
				bluedestindex=i;
		}
		Debug.Log ("blue move calculated:\nvalue:"+
		           move.sefscore+
		           "\nstartpos: "+move.startpos+
		           "\nendpos"+move.endpos);
		donecalculating=true;
	}



intV3 min_alpha_betaG(List<Vector2> GP, List<bool> deadGP, List<Vector2> BP, List<bool> deadBP,
                     int alpha, int beta, int level, int depth, bool inverted){
	int valid; 
	int value; 
	intV3 best_move=new intV3(6000000, nullpos,nullpos);
	intV3 valandmove;
	Vector2 ghost=nullpos;
	Vector2 destination,saved;
	bool gameOVER = false;
	for (int i=0; i<4; i++) {
		if(GP[i]==blueDen)
			gameOVER=true;
	}
	for (int i=0; i<4; i++) {
		if(BP[i]==greenDen)
			gameOVER=true;
		
	}
	if(level==depth||gameOVER==true){
		if(inverted)
			return new intV3 ((-1)*SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
		else
			return new intV3 (SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
	}		//for each possible play of the program(program is assumed blue, program BP)
	for (int i=0; i<4; i++) {//try to move each piece up
		if (!deadBP [i]) {
			destination = saved = BP [i];
			//	Debug.Log ("saved is " + saved);
			destination.y += 1;
			valid = checkPlay (GP, BP, i, destination); 
			//Debug.Log ("moving BP:" + i + " up, valid: " + valid);
			if (valid >= 0) {
				//make play
				BP [i] = destination;
				if (valid < 5) {
					ghost = GP [valid];
					GP [valid] = nullpos;
					deadGP [valid] = true;
				}
				//call recursively
				valandmove = max_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
				value = valandmove.sefscore;
				//	Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));
				
				//Debug.Log ("value, alpha: "+value+","+alpha);
				
				//rescind the play
				BP [i] = saved;
				if (valid < 5) {
					GP [valid] = ghost;
					deadGP [valid] = false;
				}
					//calculations
					if(beta>value){
						beta=value;
						best_move=new intV3(value,saved,destination);
						
						
					}
					if(beta<=alpha)
						return new intV3(alpha, nullpos,nullpos);
			}
			
		}
	}
	for (int i=0; i<4; i++) {//try to move each piece down
		if (!deadBP [i]) {
			destination = saved = BP [i];
			destination.y -= 1;
			valid = checkPlay (GP, BP, i, destination); 
			if (valid >= 0) {
				//make play
				BP [i] = destination;
				if (valid < 5) {
					ghost = GP [valid];
					GP [valid] = nullpos;
					deadGP [valid] = true;
				}
				//call recursively
					valandmove = max_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
				value = valandmove.sefscore;
				//	Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));
				
				//Debug.Log ("value, alpha: "+value+","+alpha);
				
				//rescind the play
				BP [i] = saved;
				if (valid < 5) {
					GP [valid] = ghost;
					deadGP [valid] = false;
				}
					//calculations
					if(beta>value){
						beta=value;
						best_move=new intV3(value,saved,destination);
						
						
					}
					if(beta<=alpha)
						return new intV3(alpha, nullpos,nullpos);
			}
			
		}
	}
	for (int i=0; i<4; i++) {//try to move each piece left
		if (!deadBP [i]) {
			destination = saved = BP [i];
			destination.x -= 1;
			valid = checkPlay (GP, BP, i, destination); 
			if (valid >= 0) {
				//make play
				BP [i] = destination;
				if (valid < 5) {
					ghost = GP [valid];
					GP [valid] = nullpos;
					deadGP [valid] = true;
				}
				//call recursively
					valandmove =max_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
				value = valandmove.sefscore;
				//Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));
				
				//rescind the play
				BP [i] = saved;
				if (valid < 5) {
					GP [valid] = ghost;
					deadGP [valid] = false;
				}
					//calculations
					if(beta>value){
						beta=value;
						best_move=new intV3(value,saved,destination);
						
						
					}
					if(beta<=alpha)
						return new intV3(alpha, nullpos,nullpos);
			}
			
		}
	}
	for (int i=0; i<4; i++) {//try to move each piece right
		if (!deadBP [i]) {
			destination = saved = BP [i];
			destination.x += 1;
			valid = checkPlay (GP, BP, i, destination); 
			if (valid >= 0) {
				//make play
				BP [i] = destination;
				if (valid < 5) {
					ghost = GP [valid];
					GP [valid] = nullpos;
					deadGP [valid] = true;
				}
				//call recursively
					valandmove = max_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
				value = valandmove.sefscore;
				//Debug.Log ("value, alpha: "+value+","+alpha + "at level "+(level+1));
				
				//Debug.Log ("value, alpha: "+value+","+alpha);
				
				//rescind the play
				BP [i] = saved;
				if (valid < 5) {
					GP [valid] = ghost;
					deadGP [valid] = false;
				}
					//calculations
					if(beta>value){
						beta=value;
						best_move=new intV3(value,saved,destination);
						
						
					}
					if(beta<=alpha)
						return new intV3(alpha, nullpos,nullpos);
				
			}
			
		}
	}
	return best_move; 
}


	intV3 max_alpha_betaG(List<Vector2> GP, List<bool> deadGP, List<Vector2> BP, List<bool> deadBP,
	                     int alpha, int beta, int level, int depth,bool inverted){
		//Debug.Log ("inside of min_alpha_beta at depth: "+level);
		bool bestvalchanged = false;
		int valid; 
		int value; 
		intV3 best_move=new intV3(-6000000, nullpos,nullpos);
		intV3 valandmove;
		Vector2 ghost=nullpos;
		Vector2 destination,saved;
		bool gameOVER = false;
		for (int i=0; i<4; i++) {
			if(GP[i]==blueDen)
				gameOVER=true;
		}
		for (int i=0; i<4; i++) {
			if(BP[i]==greenDen)
				gameOVER=true;
			
		}
		if (level == depth||gameOVER==true) {
			if(inverted)
				return new intV3 ((-1)*SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
			else
				return new intV3 (SEF (GP, deadGP, BP, deadBP), nullpos, nullpos);
		}
		//for each possible play of the opponent(opponent assumed green, opponent GP)
		for (int i=0; i<4; i++) {//try to move each piece up
			if(!deadGP[i]){
				destination=saved=GP[i];
				
				destination.y+=1;
				valid=checkPlay (BP,GP,i,destination); 
				
				if(valid>=0){
					//make play
					GP[i]=destination;
					if(valid<5){
						ghost=BP[valid];
						BP[valid]=nullpos;
						deadBP[valid]=true;
					}
					//call recursively
					valandmove=min_alpha_betaG (GP,deadGP,BP,deadBP,alpha,beta,level+1, depth,inverted);
					value=valandmove.sefscore;
					//Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));
					
					//Debug.Log ("value, beta: "+value+","+beta);
					
					//rescind the play
					GP[i]=saved;
					if(valid<5){
						BP[valid]=ghost;
						deadBP[valid]=false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;
						
						best_move = new intV3 (value, saved, destination);
						
						
					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}
			}
		}
		for (int i=0; i<4; i++) {//try to move each piece down
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.y -= 1;
				valid = checkPlay (BP, GP, i, destination); 
				
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
					//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));
					
					//Debug.Log ("value, beta: "+value+","+beta);
					
					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;
						
						best_move = new intV3 (value, saved, destination);
						
						
					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}
			}
		}
		for (int i=0; i<4; i++) {//try to move each piece left
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.x -= 1;
				valid = checkPlay (BP, GP, i, destination); 
				
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
					//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));
					
					//Debug.Log ("value, beta: "+value+","+beta);
					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;
						
						best_move = new intV3 (value, saved, destination);
						
						
					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}
			}
		}
		for (int i=0; i<4; i++) {//try to move each piece right
			if (!deadGP [i]) {
				destination = saved = GP [i];
				destination.x += 1;
				valid = checkPlay (BP, GP, i, destination); 
				
				if (valid >= 0) {
					//make play
					GP [i] = destination;
					if (valid < 5) {
						ghost = BP [valid];
						BP [valid] = nullpos;
						deadBP [valid] = true;
					}
					//call recursively
					valandmove = min_alpha_betaG (GP, deadGP, BP, deadBP, alpha, beta, level + 1, depth, inverted);
					value = valandmove.sefscore;
					//	Debug.Log ("value, beta: "+value+","+beta + "at level "+(level+1));
					
					//	Debug.Log ("value, beta: "+value+","+beta);
					
					//rescind the play
					GP [i] = saved;
					if (valid < 5) {
						BP [valid] = ghost;
						deadBP [valid] = false;
					}
					//calculations
					if (value > alpha) {
						alpha = value;
						
						best_move = new intV3 (value, saved, destination);
						
						
					}
					if (alpha >= beta)
						return new intV3 (beta, nullpos, nullpos);
				}
			}
		}
		/*Debug.Log ("returning best_move\n"+
		           "best.move.startpos: "+best_move.startpos+
		           "\nbest.move.startpos: "+best_move.startpos+
		           "\nbest.move.endpos: "+best_move.endpos+
		           "\nbest.move.value: "+best_move.sefscore);
	*/
		/*if (!bestvalchanged)
			Debug.Log ("best val not changed..\n GP:" + GP [0] + GP [1] + GP [2] + GP [3]
				+ "\ndeadGP" + deadGP [0] + deadGP [1] + deadGP [2] + deadGP [3]
				+ "\nBP" + BP [0] + BP [1] + BP [2] + BP [3]
				+ "\ndeadBP" + deadBP [0] + deadBP [1] + deadBP [2] + deadBP [3]
				+ "\nalpha:" + alpha
				+ "\nbeta:" + beta
				+ "\nlevel:" + level
				+ "\ndepth:" + depth
			);*/
		return best_move; 
		
	}
}
