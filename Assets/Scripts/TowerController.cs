using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {
	public float BulletSpeed;
	public GameObject Bullet_prefab;
	public Transform Target_prefab;
	public Transform BulletSpawner;
	public Transform TargetSpawner;
	public Transform Turret;
	public Texture2D TX_yes;
	public Texture2D TX_no;
	
	private GameObject Target;
	private GameObject bullet;
	
	private Rect ViewGUIRect;
	private float timer;
	private string GUI_firepower = "30";
	#region CALCULATION_VARIABLES
	
	private float xangle;
	private float xtime;
	private float xheight;
	
	private float GhostAngle;
	private float GhostDistance;
	private float GhostFirstTime;
	private float GhostDeltaTime;
	private Vector3 GhostPosition;
	
	private Vector3 Ghost_first_pos;
	private Vector3 Ghost_second_pos;
	private Vector3 Ghost_speed;
	
	private Vector3 distance_per_tic;
	
	private float _G 	= 9.80665f;
	private float _RTG	= 180 / Mathf.PI; // radian to grad
	#endregion
	
	void Start () {
		ViewGUIRect = new Rect(20,20,350,200);
	}
	
	void LateUpdate () {
		if (!Target) {
			if (timer + Time.deltaTime >= 1){
				Target = GameObject.FindGameObjectWithTag("target");
				if (Target) {
					GhostPosition = Target.transform.position;
					Ghost_first_pos = GhostPosition;
					Ghost_second_pos = GhostPosition;
				}
				timer = 0;
			} else timer += Time.deltaTime;
		} else {
			
			xangle 	= GetMainAngle(Turret.position, Target.transform.position, BulletSpeed);
			xtime	= GetTotalFlowTime(Turret.position, Target.transform.position, xangle, BulletSpeed);
			
			Ghost_first_pos 	= Ghost_second_pos;
			Ghost_second_pos 	= Target.transform.position;
			Ghost_speed 		= Ghost_second_pos - Ghost_first_pos;						// РѕРїСЂРµРґРµР»РµРЅРёРµ РІРµРєС‚РѕСЂРЅРѕР№ СЃРєРѕСЂРѕСЃС‚Рё С†РµР»Рё			
			distance_per_tic	= (Ghost_speed * xtime ) / Time.deltaTime;					// РѕРїСЂРµРґРµРґРµРЅРёРµ РїСЂРѕР№РґРµРЅРЅРѕРіРѕ СЂР°СЃСЃС‚РѕРЅРёСЏ С†РµР»Рё Р·Р° СЂР°СЃС‡РёС‚Р°РЅРЅРѕРµ РїСЂРёРјРµСЂРЅРѕРµ РІРµСЂРјСЏ РїРѕР»РµС‚Р°
			GhostPosition		= Ghost_second_pos + distance_per_tic;						// СЂР°СЃС‡РµС‚ РїСЂРёРјРµСЂРЅРѕРіРѕ РїРѕР»РѕР¶РµРЅРёСЏ РїСЂРёР·СЂР°С‡РЅРѕР№ С†РµР»Рё
			
			GhostFirstTime		= GetTotalFlowTime(Turret.position, GhostPosition, xangle, BulletSpeed);
			GhostDeltaTime 		= GhostFirstTime - xtime;									// СЂР°СЃС‡РµС‚ СЂР°Р·РЅРёС†С‹ РІСЂРµРјРµРЅРё РїРѕР»РµС‚Р° СЃРЅР°СЂСЏРґР°, РёСЃС…РѕРґСЏ РёР· РїРѕР»СѓС‡РµРЅРЅРѕР№ РїРµСЂРІРЅРѕР°С‡Р°Р»СЊРЅРѕР№ СЃРєРѕСЂРѕСЃС‚Рё
			
			distance_per_tic	= (Ghost_speed * (xtime + GhostDeltaTime) ) / Time.deltaTime;// РѕРїСЂРµРґРµРґРµРЅРёРµ РїСЂРѕР№РґРµРЅРЅРѕРіРѕ СЂР°СЃСЃС‚РѕРЅРёСЏ С†РµР»Рё Р·Р° СЂР°СЃС‡РёС‚Р°РЅРЅРѕРµ РїСЂРёРјРµСЂРЅРѕРµ РІРµСЂРјСЏ РїРѕР»РµС‚Р°
			GhostPosition		= Ghost_second_pos + distance_per_tic;						// СЂР°СЃС‡РµС‚ РїСЂРёРјРµСЂРЅРѕРіРѕ РїРѕР»РѕР¶РµРЅРёСЏ РїСЂРёР·СЂР°С‡РЅРѕР№ С†РµР»Рё
			
			GhostAngle			= GetMainAngle(Turret.position, GhostPosition, BulletSpeed);	// СЂР°СЃС‡РµС‚ СѓРіР»Р°, РЅРµРѕР±С…РѕРґРёРјРѕРіРѕ РґР»СЏ РїРѕСЂР°Р¶РµРЅРёСЏ РїСЂРёР·СЂР°С‡РЅРѕР№ С†РµР»Рё 
			Turret.transform.localEulerAngles = new Vector3(GhostAngle * _RTG, Turret.transform.localEulerAngles.y, Turret.transform.localEulerAngles.z); // РЅР°РІРµРґРµРЅРёРµ "РѕСЂСѓРґРёСЏ"	
		}		
	}
	
	void OnGUI(){
		GUI.BeginGroup(ViewGUIRect);
		GUI.Box(new Rect(0,0,ViewGUIRect.width,ViewGUIRect.height),"");
		if (GUI.Button( new Rect (10,10,150,30),"Fire!")){
			bullet = (GameObject)Instantiate(Bullet_prefab, BulletSpawner.position, BulletSpawner.rotation);
			bullet.transform.GetComponent<Rigidbody>().velocity = (BulletSpawner.transform.TransformDirection(Vector3.forward * BulletSpeed));		
		}
		if (GUI.Button( new Rect (180,10,150,30),"Create new target")){
			Target = GameObject.Instantiate(Target_prefab, TargetSpawner.position, TargetSpawner.rotation) as GameObject;
		}
		GUI.Label(new Rect(20,50,350,20),"Ghost Delta flow time: "+GhostDeltaTime);
		GUI.Label(new Rect(20,70,350,20),"Turret Angle:          " + -(_RTG * xangle) + " grad");		
		GUI.Label(new Rect(20,90,350,20),"Approx. flow time:  " + xtime +" seconds");		
		GUI.Label(new Rect(20,110,350,20),"BulletSpeed " + BulletSpeed +" m/s");
		GUI_firepower = GUI.TextField(new Rect(150,130,80,20),GUI_firepower);
		if (GUI.Button(new Rect(20,130,100,20),"Set power")){
			BulletSpeed = float.Parse(GUI_firepower);
		}
		GUI.EndGroup();
	}
	
	float GetAngle(float dist, float power) {						// СѓРіРѕР», РїРѕРґ РєРѕС‚РѕСЂС‹Рј РЅСѓР¶РЅРѕ Р±СЂРѕСЃРёС‚СЊ СЃРЅР°СЂСЏРґ, СЃ СЃРёР»РѕР№ power РЅР° СЂР°СЃСЃС‚РѕСЏРЅРёРµ dist (РЅР° РѕРґРЅРѕР№ РїР»РѕСЃРєРѕСЃС‚Рё). СѓРїСЂРѕС‰РµРЅРЅР°СЏ С„РѕСЂРјСѓР»Р°
		return (Mathf.Asin((_G * dist) / (power * power)) / 2);
	}
	
	float GetMaxHeight(float angle, float power){					// РјР°РєСЃРёРјР°Р»СЊРЅР°СЏ РІС‹СЃРѕС‚Р° РїРѕРґСЉРµРјР° СЃРЅР°СЂСЏРґР°
		return ((power * power) * (Mathf.Sin(angle) * Mathf.Sin(angle) ) /(2 * _G));
	}
	
	float GetTotalFlowTime(Vector3 FirePos, Vector3 target, float angle, float power) {		// РѕР±С‰РµРµ РІСЂРµРјСЏ РїРѕР»РµС‚Р° СЃРЅР°СЂСЏРґР°
		float distance = Mathf.Sqrt(Mathf.Pow(Vector3.Distance(target, FirePos),2) - Mathf.Pow((FirePos.y - target.y), 2));
		return distance / (power * Mathf.Cos(angle));
	}
	
	float GetMainAngle(Vector3 FirePos, Vector3 target, float power){ 	// С„СѓРЅРєС†РёСЏ СЂР°СЃС‡РµС‚Р° СѓРіР»Р°. Р’РѕР·РІСЂР°С‰Р°РµРјРѕРµ Р·РЅР°С‡РµРЅРёРµ РІ СЂР°РґРёР°РЅР°С…!
		bool debug = false;
		if (debug) print ("===============================================");
		float deltaHeight = FirePos.y - target.y;
		if (debug) print ("deltaHeight: "+deltaHeight);		
		float tmp =  Mathf.Pow(Vector3.Distance(target, FirePos),2) - Mathf.Pow(deltaHeight , 2);
		if (debug) print ("distance in square "+ tmp);
		float distance = Mathf.Sqrt(tmp);
		if (debug) print ("distance: "+distance);
		float _a = (_G * Mathf.Pow(distance,2) ) / (Mathf.Pow(power,2) * 2);
		if (debug) print ("_a: "+_a);
		float _c = -deltaHeight + _a;
		if (debug) print ("_c: "+ _c);
		float sqrt_dscrm = Mathf.Sqrt(Mathf.Pow(distance,2) - (4 * _a * _c));
		if (debug) print ("sqrt_dscrm: " + sqrt_dscrm);		
		float x1 = Mathf.Atan((-distance + sqrt_dscrm) / (2 * _a));
		if (debug) 
			print ("x1: "+ x1);
		float x2 = Mathf.Atan((-distance - sqrt_dscrm) / (2 * _a));
		if (debug) 
			print ("x2: "+x2);
		if (debug) print ("===============================================");
		return Mathf.Max(x1,x2);
	}
}