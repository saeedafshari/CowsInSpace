using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {
    public float Speed;
    public float StopTime;

    // Use this for initialization
    float timer;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0));

        timer += Time.deltaTime;
        if (timer > StopTime)
            Destroy(this);
    }
}
