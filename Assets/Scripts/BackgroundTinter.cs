using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTinter : MonoBehaviour {
    public float StartTime;
    public float TargetOpacity;
    public float Rate;

    float timer;

    SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > StartTime)
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                spriteRenderer.color.a - Rate * Time.deltaTime);
            if (spriteRenderer.color.a <= TargetOpacity)
                enabled = false;
        }
	}
}
