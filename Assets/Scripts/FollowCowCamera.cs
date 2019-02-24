using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCowCamera : MonoBehaviour {
    public GameObject FollowObject;
    float initialDistance;
    Camera camera;
	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
        initialDistance = (transform.position - FollowObject.transform.position).y;
	}
	
	// Update is called once per frame
	void Update () {
        var oheight = camera.orthographicSize / camera.aspect;
        var distance = (transform.position - FollowObject.transform.position).y;
        if (initialDistance > distance)
        {
            transform.position = new Vector3(
                transform.position.x, 
                FollowObject.transform.position.y + initialDistance, 
                transform.position.z);
        }
	}
}
