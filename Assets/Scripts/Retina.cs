using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retina : MonoBehaviour {
    Transform anchor;
    const float maxDistance = 0.03f;
	// Use this for initialization
	void Start () {
        anchor = transform.parent.Find("Eyeball");
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, anchor.position); //distance from ~green object~ to *black circle*

        if (distance > maxDistance) //If the distance is less than the radius, it is already within the circle.
        {
            Vector3 fromOriginToObject = transform.position - anchor.position;
            fromOriginToObject *= maxDistance / distance; //Multiply by radius //Divide by Distance
            transform.position = anchor.position + fromOriginToObject; //*BlackCenter* + all that Math
        }
    }
}
