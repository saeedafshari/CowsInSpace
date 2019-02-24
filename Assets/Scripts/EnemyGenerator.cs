using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour {
    public GameObject[] Enemies;

    new Camera camera;
    float bounds;

    float timer;
    float spawnTimer;
    
    // Use this for initialization
    void Start () {
        camera = GameObject.Find("Game Camera").GetComponent<Camera>();
        bounds = camera.orthographicSize * 2 * 1.2f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 5.0)
        {
            spawnTimer = 0;
            var candidate = Enemies
                .Select(x => x.GetComponent<Enemy>())
                .Where(x => x.StartTime < timer)
                .Where(x => x.EndTime <= 0 || x.EndTime > timer)
                .OrderBy(x => Random.value)
                .FirstOrDefault();

            Debug.Assert(candidate != null);

            Spawn(candidate);
        }
	}

    void Spawn(Enemy meta)
    {
        bool side = Random.Range(0.0f, 1.0f) > 0.5f;
        var y = camera.orthographicSize * 1.5f / camera.aspect;
        var x = side ? -bounds : bounds;
        var obj = GameObject.Instantiate(meta.gameObject);
        obj.transform.SetParent(transform);
        obj.transform.position = new Vector3(x, y, 0);

        var enemy = obj.GetComponent<Enemy>();
        if (side)
        {
            enemy.Side = side;
            var sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.flipX = true;
        }
    }
}
