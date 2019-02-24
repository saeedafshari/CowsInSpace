using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CloudGenerator : MonoBehaviour {
    public Sprite[] Sprites;
    public GameObject[] GameObjects;

    public float MinDelay;
    public float MaxDelay;
    public float MinSpeed;
    public float MaxSpeed;
    public float MinScale;
    public float MaxScale;

    public float EnableTime;
    public float DisableTime;

    public bool Tint;

    new Camera camera;
    float bounds;

    float timer;
    float nextTimer;
    float totalTimer;

    float killZoneY;

    // Use this for initialization
    void Start ()
    {
        camera = GameObject.Find("Game Camera").GetComponent<Camera>();
        bounds = camera.orthographicSize * 2 * 1.2f;
        killZoneY = GameObject.Find("Killzone").transform.position.y;
        nextTimer = Random.Range(MinDelay, MaxDelay);
    }
	
	// Update is called once per frame
	void Update () {
        totalTimer += Time.deltaTime;
        if (totalTimer < EnableTime) return;
        if (DisableTime > 0 && totalTimer > DisableTime) return;

        timer += Time.deltaTime;
        if (timer > nextTimer)
        {
            timer = 0;
            nextTimer = Random.Range(MinDelay, MaxDelay);
            if (Sprites.Length > 0)
            {
                var sprite = Sprites.OrderBy(x => Random.value).First();
                Spawn(sprite);
            }
            if (GameObjects.Length > 0)
            {
                var gameObject = GameObjects.OrderBy(x => Random.value).First();
                var instance = GameObject.Instantiate(gameObject);
                Spawn(instance);
            }
        }
	}

    void Spawn(Sprite sprite)
    {
        var g = new GameObject();

        var renderer = g.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.flipX = Random.value < 0.5;
        renderer.flipY = Random.value < 0.5;
        if (Tint)
        {
            renderer.color = Color.HSVToRGB(Random.value, 1.0f, 0.5f);
        }
        
        Spawn(g);
    }

    void Spawn(GameObject g)
    {
        g.transform.SetParent(transform);
        g.transform.localScale *= Random.Range(MinScale, MaxScale);
        
        g.transform.position = new Vector3(
            Random.Range(-bounds, bounds),
            transform.position.y,
            transform.position.z);
        
        var fall = g.AddComponent<Fall>();
        fall.Speed = Random.Range(MinSpeed, MaxSpeed);
    }
}
