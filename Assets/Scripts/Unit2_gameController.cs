using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Unit2_gameController : MonoBehaviour {
	public string Route;
	public Transform Box;
	public Transform Finish;
	public Transform Floor;
	public Transform Player_prefab;
	public Transform Start_pos;
	public Transform Wall;
	
	private Rect ViewGUIRect;
	private int LevelTotalH;
	private int LevelTotalW;
	public int LeftBorder = 10;
	public int TopBorder = 10;
	
	[HideInInspector]
	public int Rows;
	[HideInInspector]
	public int Cols;
	[HideInInspector]
	public Transform box;
	
	private GameObject player;
	
	//public List<List<int>> map;
	public int[][] map;
	public Vector2 PlayerPosition;
	
	void Start(){
		ViewGUIRect = new Rect(20,20,170,70);
	}
	void BuildMap(){
		Rows = 0;
		Cols = 0;
		LevelTotalW = 0;
		LevelTotalH = 0;
		StreamReader sr = new StreamReader(Application.dataPath + "/Unit2/level.txt");
		string templine = sr.ReadLine();
		while (templine != null) {
			char[] ch = templine.ToCharArray();
			Cols = ch.Length;
			templine = sr.ReadLine();
			Rows++;
		}
		sr.Dispose();
		
		map = new int[Rows][];
		for (int n = 0; n < Rows; n++){
			map[n] = new int[Cols];
		}
		StreamReader txtmap = new StreamReader(Application.dataPath + "/Unit2/level.txt");
		string line = txtmap.ReadLine();
		while (line != null) {
			LevelTotalW = 0;
			
			foreach (char ch in line){
				switch(ch){
					case '0':
						map[LevelTotalH][LevelTotalW] = 0;
						GameObject.Instantiate(Floor, new Vector3(LeftBorder + LevelTotalH, 0.5f, TopBorder + LevelTotalW), Quaternion.identity);						
					break;
					case '1':
						map[LevelTotalH][LevelTotalW] = 1;
						GameObject.Instantiate(Floor, new Vector3(LeftBorder + LevelTotalH, 0.5f, TopBorder + LevelTotalW), Quaternion.identity);
						box = GameObject.Instantiate(Box,   new Vector3(LeftBorder + LevelTotalH, 1.5f, TopBorder + LevelTotalW), Quaternion.identity) as Transform;
					break;
					case '2':	
						map[LevelTotalH][LevelTotalW] = 2;
						GameObject.Instantiate(Finish,new Vector3(LeftBorder + LevelTotalH, 0.5f, TopBorder + LevelTotalW), Quaternion.identity);
					break;
					case '3':
						map[LevelTotalH][LevelTotalW] = 3;
						GameObject.Instantiate(Floor, new Vector3(LeftBorder + LevelTotalH, 0.5f, TopBorder + LevelTotalW), Quaternion.identity);
						GameObject.Instantiate(Wall,  new Vector3(LeftBorder + LevelTotalH, 1.5f, TopBorder + LevelTotalW), Quaternion.identity);
					break;
					case '4':
						map[LevelTotalH][LevelTotalW] = 4;
						PlayerPosition = new Vector2(LevelTotalH, LevelTotalW);
						GameObject.Instantiate(Start_pos, new Vector3(LeftBorder + LevelTotalH, 0.5f, TopBorder + LevelTotalW), Quaternion.identity);
						GameObject.Instantiate(Player_prefab,   new Vector3(LeftBorder + LevelTotalH, 1.5f, TopBorder + LevelTotalW), Quaternion.identity);
					break;
					default :
						Debug.LogWarning("Unknown symbol: "+ch);
						break;
				}
				LevelTotalW++;
			}
			LevelTotalH++;			
			line = txtmap.ReadLine(); 
		}
	}
	
	void OnGUI(){
		GUI.BeginGroup(ViewGUIRect);
		GUI.Box(new Rect(0,0,ViewGUIRect.width,ViewGUIRect.height),"");
		
		if (GUI.Button( new Rect (10,10,150,30),"Load map")){
			BuildMap();
		}
		GUI.EndGroup();
	}
}
