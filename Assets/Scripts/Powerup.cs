using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cow"))
        {
            FindObjectOfType<GameDirector>().Hit(gameObject.tag, gameObject);
            foreach (Transform item in transform)
            {
                if (item.name.Contains("Line"))
                    Destroy(item.gameObject);
                else
                    item.gameObject.AddComponent<FadeOutDie>();
            }
            Destroy(this);
            Debug.Log($"Hit cow {name} - {other.name}");
            Destroy(gameObject, 0.1f);
        }
    }
}
