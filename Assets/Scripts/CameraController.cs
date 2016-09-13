using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public enum _cd{
		XY,
		XZ, 
		YZ
	}	
	public _cd ControlDirection;
	
	private Transform me;
	private float ortosize;
	void Start () {
		me = transform;
		ortosize = me.GetComponent<Camera>().orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)){
			switch (ControlDirection){
				case _cd.XY :
					me.position = new Vector3 (me.position.x, me.position.y - Input.GetAxis("Mouse Y") ,me.position.z - Input.GetAxis("Mouse X"));
				break;
				case _cd.XZ :
					me.position = new Vector3 (me.position.x  + Input.GetAxis("Mouse Y"), me.position.y, me.position.z  - Input.GetAxis("Mouse X") );
				break;
				case _cd.YZ :
					me.position = new Vector3 (me.position.x  - Input.GetAxis("Mouse X"), me.position.y - Input.GetAxis("Mouse Y") ,me.position.z);
				break;
			}
			
		}
		if (Input.GetAxis("Mouse ScrollWheel") !=0 ){
			ortosize += Input.GetAxis("Mouse ScrollWheel")*5;
			ortosize = Mathf.Clamp(ortosize,1,50);
			me.GetComponent<Camera>().orthographicSize = ortosize;
		}
	}
}
