using UnityEngine;
using System.Collections;

public class CheckDestroyBullet : MonoBehaviour {
	private Transform me;
	private float lifetimer;
	// Update is called once per frame
	
	void Start (){
		me = transform;
		lifetimer = 0;
	}
	void Update () {
		lifetimer+=Time.deltaTime;
		if (me.position.y <= -20) Destroy(me.gameObject);
		Destroy(this, 60);
	}
	void OnDestroy(){
		Debug.Log(lifetimer);
	}
}
