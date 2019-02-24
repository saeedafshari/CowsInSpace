using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour {
    public Sprite[] Sprites;
    public float FrameRate;

    SpriteRenderer spriteRenderer;

    float timer;
    int frame = 0;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > FrameRate)
        {
            timer = 0;
            frame = (frame + 1) % Sprites.Length;
            spriteRenderer.sprite = Sprites[frame];
        }
	}
}
