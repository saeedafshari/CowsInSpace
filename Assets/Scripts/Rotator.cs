using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public float MinSpeed;
    public float MaxSpeed;

    float speed;
	// Use this for initialization
	void Start () {
        speed = Random.Range(MinSpeed, MaxSpeed);
        speed *= Random.value < 0.5 ? -1 : 1;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, speed * Time.deltaTime, Space.Self);
	}
}
