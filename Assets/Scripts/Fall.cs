using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour {
    public float Speed;
    float killZoneY;
    public bool Remain;

    GameDirector director;

    // Use this for initialization
    void Start ()
    {
        director = FindObjectOfType<GameDirector>();
        killZoneY = GameObject.Find("Killzone").transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (director.IsPaused) return;

        transform.Translate(0, -Speed * Time.deltaTime, 0);
        if (transform.position.y < killZoneY)
        {
            if (Remain) enabled = false;
            else Destroy(gameObject);
        }
    }
}
