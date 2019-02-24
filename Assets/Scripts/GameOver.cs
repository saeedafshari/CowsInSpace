using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var director = FindObjectOfType<GameDirector>();
        GameObject.Find("Final Score Value").GetComponent<Text>().text =
            $"{director.Altitude * 100:N0} ft / {director.Score} pts";
        GameObject.Find("Final High Score Value").GetComponent<Text>().text =
            $"{director.HighAltitude * 100:N0} ft / {director.HighScore} pts";
        GameObject.Find("High Score Achieved").SetActive(director.IsHighScore);
    }

    public void Restart()
    {
        FindObjectOfType<GameDirector>().Restart();
    }
    private void Update()
    {
    }
}
