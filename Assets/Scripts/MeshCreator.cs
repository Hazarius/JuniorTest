using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCreator : MonoBehaviour {
	public Texture2D Maintx;
	public List<Vector2> p1;
	public List<Vector2> p2;

	public int LineHeight = 4;
	
	private Rect ViewGUIRect;
	private GameObject RedLine;
	private GameObject GreenLine;	
	GameObject result_line;

	void Start () {
		ViewGUIRect = new Rect(20,20,300,200);
	}
	
	void Update(){
		int i; // default iterator											// РѕС‚Р»Р°РґРѕС‡РЅР°СЏ РіСЂР°С„РёС‡РµСЃРєР°СЏ РёРЅС„РѕСЂРјР°С†РёСЏ
		if (p1.Count > 2){
			for (i = 0; i < p1.Count-1; i++) Debug.DrawLine(p1[i], p1[i+1],Color.red);
		}
		if (p2.Count > 2){
			for (i = 0; i < p2.Count-1; i++) Debug.DrawLine(p2[i], p2[i+1], Color.green);
		}
	}
	
	GameObject BuildLine(List<Vector2> points, Color maincolor, float height ){
		
		Mesh m = new Mesh();
		Vector2 dir = (points[0]-points[1]).normalized;						// РѕРїСЂРµРґРµР»РµРЅРёРµ РїРµСЂРІРѕРЅР°С‡Р°Р»СЊРЅРѕРіРѕ РЅР°РїСЂР°РІР»РµРЅРёСЏ РІРµРєС‚РѕСЂР°, РґР»СЏ РЅР°С…РѕР¶РґРµРЅРёСЏ РїРµСЂРІС‹С… РґРІСѓС… РІРµСЂС€РёРЅ
		int current_vert = 0;												// РёРЅРґРµРєСЃ С‚РµРєСѓС‰РµР№ РІРµСЂС€РёРЅС‹ РІ РјР°СЃСЃРёРІРµ
		Vector3[] verts = new Vector3[points.Count * 2];
		Vector2[] uvs	= new Vector2[points.Count * 2];
		 
		verts[current_vert] = points[0]+(dir * height) + new Vector2(dir.y, -dir.x) * height; 
		uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
		current_vert++; // 1-СЏ РЅР°С‡Р°Р»СЊРЅР°СЏ РІРµСЂС€РёРЅР°
		
		verts[current_vert] = points[0]+(dir * height) + new Vector2(-dir.y, dir.x) * height; 
		uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
		current_vert++; // 2-СЏ РЅР°С‡Р°Р»СЊРЅР°СЏ РІРµСЂС€РёРЅР°
		
		for (int i = 1; i<points.Count-1; i++){
			dir = (points[i]-points[i+1]).normalized;						// РѕРїСЂРµРґРµР»РµРЅРёРµ РЅР°РїСЂР°РІР»РµРЅРёСЏ РІРµРєС‚РѕСЂР° СЃР»РµРґСѓСЋС‰РµР№ С‚РѕС‡РєРё
			Vector2 back_dir = (points[i]-points[i-1]).normalized;			// РѕРїСЂРµРґРµР»РµРЅРёРµ РЅР°РїСЂР°РІР»РµРЅРёСЏ РІРµРєС‚РѕСЂР° РїСЂРµРґС‹РґСѓС‰РµР№ С‚РѕС‡РєРё
			Vector2 v_median_dir;
			float v_median_length;
			float v_angle;			
			if (Vector2.Dot(dir, back_dir)-0.00001 <= -1){					// СЃС‚СЂР°РЅРЅС‹Р№ РіР»СЋРє.. Р·РЅР°С‡РµРЅРёРµ РЅРµ РІСЃРµРіРґР° = -1 Р° РїСЂРёР±Р»РёР¶РµРЅРѕ Рє РЅРµРјСѓ (0,9999999)
				v_median_dir = new Vector2(dir.y, -dir.x);					// РѕРїСЂРµРґРµР»РµРЅРёРµ РЅР°РїСЂР°РІР»РµРЅРёСЏ Р±РёСЃРµРєС‚СЂРёСЃС‹ СѓРіР»Р° РјРµР¶РґСѓ С‚РµРєСѓС‰РёРјРё РґРІСѓРјСЏ Р»РёРЅРёСЏРјРё.
				v_median_length = height;									// РґР»СЏ СЂР°Р·РІРµСЂРЅСѓС‚РѕРіРѕ СѓРіР»Р° РґР»РёРЅРЅР° Р±РёСЃРµРєС‚СЂРёСЃС‹ СЂР°РІРЅР° Р·Р°РґР°РЅРЅРѕР№ С‚РѕР»С‰РёРЅРµ.				
			} else {
				v_median_dir = new Vector2((dir.x + back_dir.x)/2, (dir.y + back_dir.y)/2).normalized; // РѕРїСЂРµРґРµР»РµРЅРёРµ РЅР°РїСЂР°РІР»РµРЅРёСЏ Р±РёСЃРµРєС‚СЂРёСЃС‹ 				
				v_angle = Mathf.Acos(Vector2.Dot(dir , back_dir));			// СЂР°СЃС‡РµС‚ СѓРіР»Р° РјРµР¶РґСѓ РІРµРєС‚РѕСЂР°РјРё		
				v_median_length = height /Mathf.Sin(v_angle / 2);			// СЂР°СЃС‡РµС‚ РґР»РёРЅРЅС‹ Р±РёСЃРµРєС‚СЂРёСЃС‹ РґР»СЏ РґР°РЅРЅРѕРіРѕ СѓРіР»Р° РїСЂРё СѓРєР°Р·Р°РЅРЅРѕР№ С‚РѕР»С‰РёРЅРµ
			}																// РЅРµРјРЅРѕРіРѕ Р·Р°РґСѓР±Р»РёСЂРѕРІР°РЅРЅРѕРіРѕ РєРѕРґР° ... 
																			// РѕРїСЂРµРґРµР»РµРЅРёРµ, СЃ РєР°РєРѕР№ СЃС‚РѕСЂРѕРЅС‹ РѕС‚РЅРѕСЃРёС‚РµР»СЊРЅРѕ С‚РѕС‡РєРё РЅР°С…РѕРґРёС‚СЃСЏ СЃР»РµРґСѓСЋС‰Р°СЏ РІРµСЂС€РёРЅР°. 			
																			// РµСЃР»Рё СЃРїСЂР°РІР°, С‚Рѕ Р·Р°РїРёСЃС‹РІР°РµС‚СЃСЏ РѕР±СЂР°С‚РЅР°СЏ РїРѕСЃС‡РёС‚Р°РЅРЅРѕР№ РІРµСЂС€РёРЅР°
																			// РµСЃР»Рё СЃР»РµРІР°, С‚Рѕ Р·Р°РїРёСЃС‹РІР°РµС‚СЃСЏ РїРѕСЃС‡РёС‚Р°РЅРЅРѕР№ РІРµСЂС€РёРЅР°
																			// РЅРµРѕР±С…РѕРґРёРјРѕ РґР»СЏ РєРѕСЂСЂРµРєС‚РЅРѕРіРѕ РїРѕСЃС‚СЂРѕРµРЅРёСЏ РјР°СЃСЃРёРІР° С‚СЂРµСѓРіРѕР»СЊРЅРёРєРѕРІ
			if (Vector2.Dot(new Vector2(back_dir.y, -back_dir.x), v_median_dir) > 0) 
			{
				verts[current_vert] = points[i] - (v_median_dir * v_median_length);
				uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
				current_vert++;
				
				verts[current_vert] = points[i] + (v_median_dir * v_median_length);
				uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
				current_vert++;	
			} else {
				verts[current_vert] = points[i] + (v_median_dir * v_median_length);
				uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
				current_vert++;
				
				verts[current_vert] = points[i] - (v_median_dir * v_median_length);
				uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
				current_vert++;	
			}
		}
		dir = (points[points.Count-2]-points[points.Count-1]).normalized;	// РѕРїСЂРµРґРµР»РµРЅРёРµ РєРѕРЅРµС‡РЅРѕРіРѕ РЅР°РїСЂР°РІР»РµРЅРёСЏ Р»РёРЅРёРё
		
		verts[current_vert] = points[points.Count-1]-(dir * height) + new Vector2(dir.y, -dir.x) * height; 
		uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
		current_vert++; // РєРѕРЅРµС‡РЅР°СЏ РІРµСЂС€РёРЅР°
		
		verts[current_vert] = points[points.Count-1]-(dir * height) + new Vector2(-dir.y, dir.x) * height;
		uvs[current_vert] = new Vector2(verts[current_vert].x, verts[current_vert].y);
		
		m.vertices = verts;
		m.uv = uvs;
		int tri_length = (points.Count-1) * 2 * 3;
		int[] triangles = new int[tri_length];	
																		// Р°Р»РіРѕСЂРёС‚Рј СЂР°СЃС‡РµС‚Р° РїРѕР·РёС†РёР№ РІРµСЂС€РёРЅ С‚СЂРµСѓРіРѕР»СЊРЅРёРєРѕРІ
		for (int n = 0; n <points.Count-1; n++){
			triangles[n*6] 	 = n*2+2;
			triangles[n*6+1] = n*2+1;
			triangles[n*6+2] = n*2;
			
			triangles[n*6+3] = n*2+3;
			triangles[n*6+4] = n*2+1;
			triangles[n*6+5] = n*2+2;
		}
		m.triangles = triangles;
		m.RecalculateNormals();
		m.RecalculateBounds();
		m.Optimize();	
		
		result_line = new GameObject("New mesh line");
		result_line.AddComponent<MeshFilter>();
		
		result_line.GetComponent<MeshFilter>().mesh = m;
		result_line.AddComponent<MeshRenderer>();
		Material mat; 
		mat = new Material(Shader.Find("Transparent/Diffuse"));
		mat.mainTexture = Maintx;
		mat.color = maincolor;
		result_line.GetComponent<MeshRenderer>().material = mat;
		return result_line;
	}
	
	void OnGUI(){
		GUI.BeginGroup(ViewGUIRect);
		GUI.Box(new Rect(0, 0, ViewGUIRect.width,ViewGUIRect.height),"");
		if (GUI.Button( new Rect (20,20,150,30),"Create red line")){
			if (RedLine) {
				Destroy(RedLine);
			}
			RedLine = BuildLine(p1, Color.red, LineHeight * 0.3f);			
		}
		if (GUI.Button( new Rect (20,50,150,30),"Create green line")){
			if (GreenLine) {
				Destroy(GreenLine);
			}
			GreenLine = BuildLine(p2, Color.green, LineHeight * 0.2f);
		}
		GUI.EndGroup();		
	}	
}
