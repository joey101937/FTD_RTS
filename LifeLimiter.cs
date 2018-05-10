using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLimiter : MonoBehaviour {
    public float Duration = 2; //in seconds, modified in inspector
    private float realDur = 0;
	// Use this for initialization
	void Start () {
        realDur = Duration;
        realDur += Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > realDur)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
