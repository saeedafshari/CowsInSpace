using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int Loops = 2;
    public Vector2 MinSpeed;
    public Vector2 MaxSpeed;
    public Sprite[] Sprites;
    public float FrameRate;


    public float StartTime;
    public float EndTime;

    [HideInInspector]
    public bool Side;

    Vector2 speed;
    Rigidbody2D body;
    float timer;
    int frame = 0;
    int loopsDone = 0;
    SpriteRenderer spriteRenderer;
    new Camera camera;
    float bounds;
    bool dangerous;
    GameDirector director;

    // Use this for initialization
    void Start () {
        dangerous = true;
        body = GetComponent<Rigidbody2D>();
        director = FindObjectOfType<GameDirector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = new Vector2(
            Random.Range(MinSpeed.x, MaxSpeed.x) * (Side ? -1 : 1),
            Random.Range(MinSpeed.y, MaxSpeed.y));

        camera = GameObject.Find("Game Camera").GetComponent<Camera>();
        bounds = camera.orthographicSize * 2 * 1.2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (director.IsPaused)
        {
            if (body.bodyType == RigidbodyType2D.Dynamic)
                body.bodyType = RigidbodyType2D.Static;
            return;
        }
        else if (body.bodyType == RigidbodyType2D.Static)
            body.bodyType = RigidbodyType2D.Dynamic;

        body.velocity = speed;
        timer += Time.deltaTime;
        if (timer > FrameRate)
        {
            frame = (frame + 1) % Sprites.Length;
            timer = 0;
            spriteRenderer.sprite = Sprites[frame];
        }
        
        var camX = (camera.transform.position - transform.position).x;
        float adjustmentShift = 0;
        if (camX < -bounds) adjustmentShift = -bounds * 1.9f;
        else if (camX > bounds) adjustmentShift = bounds * 1.9f;
        if (adjustmentShift != 0)
        {
            transform.Translate(adjustmentShift, 0, 0);
            loopsDone++;
            if (loopsDone > Loops)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!dangerous) return;
        if (other.CompareTag("Balloon"))
        {
            dangerous = false;
            Debug.Log($"Hit balloon {name} - {other.name}");
            Destroy(other.transform.parent.gameObject);

            gameObject.AddComponent<FadeOutDie>();

            director.Hit();
        }
    }
}
