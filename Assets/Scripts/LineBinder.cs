using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBinder : MonoBehaviour {
    //You should put it on a balloon

    Transform StartTransform;
    Transform EndTransform;
    NeatLine line;

	// Use this for initialization
	void Start () {
        StartTransform = GameObject.Find("Cow Anchor").transform;
        EndTransform = transform.Find("Balloon/Anchor");
        line = GetComponent<NeatLine>();
        if (line == null)
        {
            line = gameObject.AddComponent<NeatLine>();
            line.Thickness = 0.05f;
            line.Color = Color.black;
        }
	}
	
	// Update is called once per frame
	void Update () {
        line.HeadPosition = StartTransform.position;
        line.TailPosition = EndTransform.position;
	}
}
