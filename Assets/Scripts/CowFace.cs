using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowFace : MonoBehaviour {
    public Sprite NormalSprite;
    public Sprite HolyShitSprite;
    public float HolyShitSeconds;
    public AudioClip[] MooSounds;

    AudioSource mooSource;

    public static CowFace Instance;

    float holyShitCounter = 0;
	// Use this for initialization
	void Start () {
        Instance = this;
        mooSource = GetComponent<AudioSource>();
    }

    public void HolyShit()
    {
        if (holyShitCounter > HolyShitSeconds * 0.5) return;
        if (!InGameMenuDirector.IsMuted)
            mooSource.PlayOneShot(MooSounds[Random.Range(0, MooSounds.Length)]);
        holyShitCounter = HolyShitSeconds;
        GetComponent<SpriteRenderer>().sprite = HolyShitSprite;
    }

    void UpdateHolyShit()
    {
        if (holyShitCounter < 0) return;
        holyShitCounter -= Time.deltaTime;
        if (holyShitCounter < 0)
        {
            GetComponent<SpriteRenderer>().sprite = NormalSprite;
        }
    }

	// Update is called once per frame
	void Update () {
        UpdateHolyShit();
	}
}
