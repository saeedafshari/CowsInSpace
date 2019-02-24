using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuDirector : MonoBehaviour
{
    Image
        imgCloudMute,
        imgCloudUnmute;

    static bool? _isMuted = null;
    public static bool IsMuted
    {
        get
        {
            if (_isMuted == null)
                _isMuted= PlayerPrefs.GetInt(nameof(IsMuted), 0) > 0;
            return _isMuted.GetValueOrDefault();
        }
        set
        {
            PlayerPrefs.SetInt(nameof(IsMuted), value ? 1 : 0);
            PlayerPrefs.Save();
            _isMuted = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        imgCloudMute = GameObject.Find("Cloud Mute").GetComponent<Image>();
        imgCloudUnmute = GameObject.Find("Cloud Unmute").GetComponent<Image>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        var selectedColor = Color.white;
        imgCloudMute.color = IsMuted ? selectedColor : Color.clear;
        imgCloudUnmute.color = IsMuted ? Color.clear : selectedColor;
    }

    public void Dismiss()
    {
        Debug.Log("Dismiss Settings");
        FindObjectOfType<GameDirector>().HideSettings();
    }

    public void Mute()
    {
        IsMuted = true;
        UpdateUI();
    }

    public void Unmute()
    {
        IsMuted = false;
        UpdateUI();
    }
}
