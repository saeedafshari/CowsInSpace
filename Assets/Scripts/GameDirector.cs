using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {
    // Use this for initialization
    BackgroundScroll backgroundScroll;
    Interaction interaction;
    EnemyGenerator enemyGenerator;
    List<GameObject> balloons;
    CloudGenerator[] clouds;
    BackgroundTinter tinter;
    GameObject help;
    AudioSource audioSource;
    GameObject cow;
    Transform oxygenBar;
    Image oxygenBarImage;
    SpriteRenderer cowSprite;
    public AudioClip PopSound;
    public AudioClip PowerupSound;
    public AudioClip OxygenSound;
    public float WaitTime;
    public GameObject ScoreTextFab;

    Text txtScore;
    Text txtHighScore;
    Text txtAltitude;
    int lives;
    bool alive;
    public int Score { get; private set; }
    public float Altitude { get; private set; }
    float oxygen;
    float timer;
    float lastAltitude;
    bool gameStarted = false;

    public bool IsPaused => pauseCounter > 0;
    public bool IsHighScore { get; private set; }

    static bool IsFirstTime = true;

    GameObject enemies, stars, planets, oxygens, resizes;

    public void UpdatePersistedScores()
    {
        PlayerPrefs.SetInt(nameof(HighScore), Mathf.Max(HighScore, Score));
        PlayerPrefs.SetFloat(nameof(HighAltitude), Mathf.Max(HighAltitude, Altitude));
        PlayerPrefs.Save();
        LoadPersistedScores();
    }

    public void LoadPersistedScores()
    {
        HighScore = PlayerPrefs.GetInt(nameof(HighScore));
        HighAltitude = PlayerPrefs.GetFloat(nameof(HighAltitude));
    }

    public int HighScore { get; set; }

    public float HighAltitude { get; set; }

	void Start () {
        Application.targetFrameRate = 60;
        LoadPersistedScores();
        Score = 0;
        alive = true;
        audioSource = GetComponent<AudioSource>();
        
        balloons = GameObject.FindGameObjectsWithTag("Balloon").ToList();
        help = GameObject.Find("help");
        if (IsFirstTime)
            help.SetActive(true);
        IsFirstTime = false;
        backgroundScroll = FindObjectOfType<BackgroundScroll>();
        interaction = FindObjectOfType<Interaction>();
        enemyGenerator = FindObjectOfType<EnemyGenerator>();
        clouds = FindObjectsOfType<CloudGenerator>();
        tinter = FindObjectOfType<BackgroundTinter>();
        lives = GameObject.FindGameObjectsWithTag("Balloon").Length;
        cow = GameObject.Find("Cow");
        cowSprite = cow.GetComponent<SpriteRenderer>();
        oxygenBar = GameObject.Find("Oxygen Bar").transform;
        oxygenBarImage = oxygenBar.gameObject.GetComponent<Image>();
        txtScore = GameObject.Find("Score Value").GetComponent<Text>();
        txtHighScore = GameObject.Find("High Score Value").GetComponent<Text>();
        txtAltitude = GameObject.Find("Altitude Value").GetComponent<Text>();

        enemies = GameObject.Find("Enemies");
        stars = GameObject.Find("Stars");
        planets = GameObject.Find("Planets");
        oxygens = GameObject.Find("Oxygen Parachutes");
        resizes = GameObject.Find("Resize Parachutes");

        ResumeGame(false);
        oxygen = 1;
        Altitude = 0;
        lastAltitude = 0;
        IsHighScore = false;
        UpdateScoreUI();
    }

    static Color LerpColor(Color cA, Color cB, float v, float a, float b)
    {
        v -= a;
        var t = v / (b - a);
        return Color.Lerp(cA, cB, t);
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        
        if (timer > WaitTime * 0.7f && !gameStarted)
        {
            help.SetActive(false);
            if (timer > WaitTime)
            {
                gameStarted = true;
                ResumeGame(true);
            }
        }
        if (pauseCounter > 0) return;

        if (!gameStarted) return;
        if (!alive) return;
        if (lives <= 0) return;

        //Update Game Logic
        if (timer > 60.0)
        {
            float consumptionRate = 0.01f;
            if (timer > 90.0)
                consumptionRate = 0.02f;
            else if (timer > 120.0)
                consumptionRate = 0.03f;
            oxygen -= Time.deltaTime * consumptionRate;
            
            UpdateOxygenUI();
        }

        Altitude += Time.deltaTime;
        if ((int)Altitude > (int)lastAltitude)
            GiveScore(1);
        lastAltitude = Altitude;
        if (HighAltitude < Altitude)
        {
            IsHighScore = true;
        }
        UpdateScoreUI();
	}

    void UpdateOxygenUI()
    {
        if (oxygen > 0.5)
        {
            oxygenBarImage.color = Color.white;
        }
        else if (oxygen > 0.25)
        {
            oxygenBarImage.color = LerpColor(Color.magenta, Color.white, oxygen, 0.25f, 0.5f);
        }
        else
        {
            oxygenBarImage.color = LerpColor(Color.black, Color.magenta, oxygen, 0.0f, 0.25f);
        }
        cowSprite.color = oxygen > 0.5 ?
            Color.white :
            Color.Lerp(Color.magenta, Color.white, oxygen * 2.0f);

        if (oxygen < 0)
        {
            oxygen = 0;
            StartCoroutine(DieSequence());
        }

        oxygenBar.localScale =
            new Vector3(oxygen, 1, 1);

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        txtAltitude.text = $"{Altitude * 100:N0} ft";
        txtScore.text = $"{Score} pts";
        var highScore = Mathf.Max(Score, HighScore);
        var highAltitude = Mathf.Max(Altitude, HighAltitude);
        txtHighScore.text = $"HI: {highAltitude * 100:N0} ft / {highScore} pts";
        txtHighScore.color = IsHighScore ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }

    int pauseCounter = 0;

    /// <summary>
    /// flag=true: resume; false: pause
    /// </summary>
    /// <param name="flag"></param>
    void ResumeGame(bool flag, bool force = false)
    {
        if (flag)
        {
            if (force) pauseCounter = 0;
            else pauseCounter--;
            if (pauseCounter < 0) pauseCounter = 0;
            if (pauseCounter > 0) return;
        }
        else
        {
            if (force || pauseCounter < 0) pauseCounter = 0;
            pauseCounter++;
            if (pauseCounter > 1) return;
        }

        foreach (var item in GameObject.FindObjectsOfType<AudioListener>())
        {
            item.enabled = !InGameMenuDirector.IsMuted;
        }
        AudioListener.volume = InGameMenuDirector.IsMuted ? 0 : 1;

        interaction.enabled = flag;
        if (backgroundScroll != null) backgroundScroll.enabled = flag;
        enemyGenerator.enabled = flag;
        tinter.enabled = flag;
        
    }

    public void Hit()
    {
        if (!InGameMenuDirector.IsMuted)
            audioSource.PlayOneShot(PopSound);
        CowFace.Instance.HolyShit();

        lives--;
        if (lives == 0)
        {
            StartCoroutine(DieSequence());
        }
    }

    IEnumerator DieSequence()
    {
        if (!alive) yield break;
        alive = false;
        Debug.Log("Game Over!");
        yield return new WaitForSeconds(0.5f);
        var fall = cow.AddComponent<Fall>();
        fall.Speed = 4;
        fall.Remain = true;
        UpdatePersistedScores();
        yield return new WaitForSeconds(2);
        // End Game
        ResumeGame(false, true);
        SceneManager.LoadScene("endgame", LoadSceneMode.Additive);
    }

    public void Restart()
    {
        SceneManager.LoadScene("game");
    }

    public void ShowSettings()
    {
        if (GameObject.FindObjectOfType<InGameMenuDirector>() != null) return;
        ResumeGame(false);
        SceneManager.LoadScene("ingamemenu", LoadSceneMode.Additive);
        
    }

    public void HideSettings()
    {
        if (GameObject.FindObjectOfType<InGameMenuDirector>() == null) return;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        ResumeGame(true);
    }

    public void Hit(string tag, GameObject go = null)
    {
        if (!alive) return;
        Debug.Log($"Hit {tag}");
        if (tag == "Grow")
        {
            if (!InGameMenuDirector.IsMuted)
                audioSource.PlayOneShot(PowerupSound);
            foreach (var j in FindObjectsOfType<ConnectJointToCow>())
            {
                j.UpdateDistance(0.5f);
            }
        }
        else if (tag == "Shrink")
        {
            if (!InGameMenuDirector.IsMuted)
                audioSource.PlayOneShot(PowerupSound);
            foreach (var j in FindObjectsOfType<ConnectJointToCow>())
            {
                j.UpdateDistance(-0.5f);
            }
        }
        else if (tag == "Oxygen")
        {
            if (!InGameMenuDirector.IsMuted)
                audioSource.PlayOneShot(OxygenSound);
            oxygen += 0.4f;
            if (oxygen > 1) oxygen = 1;
        }
        GiveScore(50, go?.transform);
    }

    public void GiveScore(int points, Transform t = null)
    {
        points *= lives;
        Score += points;
        if (!IsHighScore && Score > HighScore) IsHighScore = true;
        if (t != null)
        {
            var go = Instantiate(ScoreTextFab);
            go.transform.position = t.position;
            var text = go.GetComponent<TextMesh>();
            text.text = $"+{points}";
        }
    }
}
