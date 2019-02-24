using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutDie : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    TextMesh textMesh;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        if (spriteRenderer != null)
        {
            var a = spriteRenderer.color.a - Time.deltaTime;
            if (a < 0) Destroy(gameObject);
            else spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                a);
        }
        if (textMesh != null)
        {
            var a = textMesh.color.a - Time.deltaTime * 0.25f;
            if (a < 0) Destroy(gameObject);
            else textMesh.color = new Color(
                textMesh.color.r,
                textMesh.color.g,
                textMesh.color.b,
                a);
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - Time.deltaTime * 2.0f,
                transform.position.z);
        }
	}
}
