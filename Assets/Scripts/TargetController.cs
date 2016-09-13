using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour {

	public float StartSpeed;
	public bool RandomMove = true;
	private Transform me;
	private Vector3 VectorMove;
	private Vector3 R_Speed;
	void Start () {
		me = transform;
		if (RandomMove) R_Speed = new Vector3(0,Random.Range(0,StartSpeed), Random.Range(0,StartSpeed));
	}
	
	// Update is called once per frame
	void Update () {
		if (RandomMove){
			me.position = me.position + R_Speed * Time.deltaTime;	
		} else {
			VectorMove = new Vector3(0.0f, 0.0f, (float)(StartSpeed * Time.deltaTime) );
			me.position = me.position + VectorMove;	
		}		
	}
	
	 void OnCollisionEnter(Collision col) {
		if (col.transform.tag == "bullet"){
			Destroy(col.gameObject);
			Destroy(me.gameObject);
		}
    }
}