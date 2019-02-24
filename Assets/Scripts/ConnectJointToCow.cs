using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectJointToCow : MonoBehaviour {

    DistanceJoint2D joint;
    // Use this for initialization
    void Start () {
        joint = GetComponent<DistanceJoint2D>();
        if (joint == null)
        {
            joint = gameObject.AddComponent<DistanceJoint2D>();
        }
        var cowBody = GameObject.Find("Cow").GetComponent<Rigidbody2D>();
        joint.connectedBody = cowBody;
        joint.distance = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateDistance(float size)
    {
        size = joint.distance + size;
        if (size < 2) size = 2;
        if (size > 4) size = 4;
        joint.distance = size;
    }
}
