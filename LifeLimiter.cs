using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLimiter : MonoBehaviour {
    public float Duration = 2; //in seconds, modified in inspector
	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSecondsRealtime(Duration);
        Destroy(this.gameObject);
	}
	


}
