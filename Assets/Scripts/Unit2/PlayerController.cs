using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	private Unit2_gameController GC;
	private int[][] map;
	private Transform me;
	private Vector2 CurrPos;
	private Vector2 MapSize;
	private Transform box;
	private float timer;
	private string Route="";
	private int[] route;
	private int route_length;
	private int current_step = 0;
	
	private bool EnableMove = true;
	private bool RouteAvaliable = false;
	
	void Start(){
		me 		= transform;
		GC 		= GameObject.Find("Game_Controller").GetComponent("Unit2_gameController") as Unit2_gameController;	
		box		= GC.box;
		map 	= GC.map;
		Route	= GC.Route;
		CurrPos = GC.PlayerPosition;
		MapSize = new Vector2(GC.Rows,GC.Cols);
		if (Route.Length > 0){
			route_length = Route.Length;
			route = new int[route_length];
			char[] ch = Route.ToCharArray();
			for (int i =0; i < route_length; i++){
				if (int.TryParse(""+ch[i],out route[i])){
					route[i] = 	int.Parse(""+ch[i]);
				}				
			}
			RouteAvaliable = true;
		}
	}
	void Update () {
		if (RouteAvaliable){
			if (timer + Time.deltaTime > 0.5){
				timer = 0;
				if (current_step < route_length){
					Move(route[current_step]);
				}
				current_step++;
			} else timer += Time.deltaTime;			
		}			
		if (EnableMove){
			if (Input.GetKeyDown(KeyCode.W)) Move (2);
			if (Input.GetKeyDown(KeyCode.S)) Move (4);
			if (Input.GetKeyDown(KeyCode.A)) Move (1);
			if (Input.GetKeyDown(KeyCode.D)) Move (3);
		}
	}
	private bool CheckMove(Vector2 pos){
		if ((pos.x < 0) ||
			(pos.y < 0) ||
			(pos.y > MapSize.y-1) ||
			(pos.x > MapSize.x-1) ||
			(map[(int)pos.x][(int)pos.y] == 3)			
		) return false;
		else return true;
	}
	void MoveTo(Vector2 destination){
		
	}
	
	private void Move(int way){
		switch (way){
			case 1:		// down
				if (CheckMove(new Vector2(CurrPos.x, CurrPos.y -1))){
					if (map[(int)CurrPos.x][(int)CurrPos.y-1] == 1){
						if (TryMove(new Vector2(CurrPos.x, CurrPos.y -1) , new Vector2(CurrPos.x, CurrPos.y -2))){
							me.position = new Vector3(me.position.x, me.position.y, me.position.z-1);	
							me.localEulerAngles = new Vector3 (0,0,0);
							CurrPos = new Vector2(CurrPos.x, CurrPos.y -1);	
						}
					} else {
						me.position = new Vector3(me.position.x, me.position.y, me.position.z-1);	
						me.localEulerAngles = new Vector3 (0,0,0);
						CurrPos = new Vector2(CurrPos.x, CurrPos.y -1);	
					}
					
				}
			break;
			case 2: 	// up
				if (CheckMove (new Vector2(CurrPos.x -1, CurrPos.y))){
					if (map[(int)CurrPos.x-1][(int)CurrPos.y] == 1){
						if (TryMove(new Vector2(CurrPos.x - 1, CurrPos.y) , new Vector2(CurrPos.x - 2, CurrPos.y))){
							me.position = new Vector3(me.position.x - 1, me.position.y, me.position.z);
							me.localEulerAngles = new Vector3 (0,90,0);
							CurrPos = new Vector2(CurrPos.x - 1, CurrPos.y);
						}
					} else {
						me.position = new Vector3(me.position.x-1, me.position.y,me.position.z);
						me.localEulerAngles = new Vector3 (0,90,0);
						CurrPos = new Vector2(CurrPos.x -1, CurrPos.y);
					}	
				}
			break;
			case 3:		// right
				if (CheckMove (new Vector2(CurrPos.x, CurrPos.y + 1))){
					if (map[(int)CurrPos.x][(int)CurrPos.y + 1] == 1){
						if (TryMove(new Vector2(CurrPos.x, CurrPos.y + 1) , new Vector2(CurrPos.x, CurrPos.y + 2))){
							me.position = new Vector3(me.position.x, me.position.y, me.position.z+1);
							me.localEulerAngles = new Vector3 (0,180,0);
							CurrPos = new Vector2(CurrPos.x, CurrPos.y + 1);	
						}
					} else {
						me.position = new Vector3(me.position.x, me.position.y, me.position.z+1);
						me.localEulerAngles = new Vector3 (0,180,0);
						CurrPos = new Vector2(CurrPos.x, CurrPos.y + 1);
					}					
				}
			break;
			case 4:		// left
				if (CheckMove (new Vector2(CurrPos.x + 1, CurrPos.y))){
					if (map[(int)CurrPos.x + 1][(int)CurrPos.y] == 1){
						if (TryMove(new Vector2(CurrPos.x + 1, CurrPos.y) , new Vector2(CurrPos.x + 2, CurrPos.y))){
							me.position = new Vector3(me.position.x + 1, me.position.y,me.position.z);
							me.localEulerAngles = new Vector3 (0,270,0);
							CurrPos = new Vector2(CurrPos.x +1, CurrPos.y);	
						}
					} else {
						me.position = new Vector3(me.position.x + 1, me.position.y,me.position.z);
						me.localEulerAngles = new Vector3 (0,270,0);
						CurrPos = new Vector2(CurrPos.x +1, CurrPos.y);
					}					
				}
			break;
			default :
				Debug.LogWarning("Unknown command");
			break;
		}
	}
	private bool TryMove(Vector2 start_pos, Vector2 dest_pos){
		if 	(CheckMove (dest_pos)){
			box.position = new Vector3(GC.LeftBorder + dest_pos.x, box.position.y, GC.TopBorder + dest_pos.y);
			map[(int)start_pos.x][(int)start_pos.y] = 0;
			Debug.Log("Move box from "+ start_pos+" to " + dest_pos);
			if (map[(int)dest_pos.x][(int)dest_pos.y] == 2){
				EnableMove = false;
				Debug.Log("Level complete!");
			} else map[(int)dest_pos.x][(int)dest_pos.y] = 1;
			
			return true;
		} else return false;
	}
	
	void SetPosition(Vector2 v){
		CurrPos	= v;
	}	
}
