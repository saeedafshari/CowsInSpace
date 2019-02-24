using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Interaction : MonoBehaviour {

    public Camera Camera;
    float movementSpeed = 0.1f;
    float bounds;
    List<Rigidbody2D> bodies;
    Rigidbody2D body;

    // Use this for initialization
    const float rotationZMax = 4.0f; //going left
    const float rotationRate = 2.0f;

    void Start () {
        bounds = Camera.orthographicSize * 2 * 1.2f;
        body = GetComponent<Rigidbody2D>();
        bodies = GameObject.FindGameObjectsWithTag("Balloon")
            .Select(x => x.GetComponent<Rigidbody2D>())
            .ToList();

        Input.simulateMouseWithTouches = true;
	}
	
    float GetHorizInput()
    {
        var horiz = Input.GetAxis("Horizontal");
        if (Input.GetMouseButton(0) && Input.mousePosition.y < Screen.height * 0.8)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            horiz = pos.x * 0.01f * Camera.orthographicSize;
        }
        return horiz;
    }

    void UpdatePlayerMovement()
    {
        var horiz = GetHorizInput() * movementSpeed;

        body.velocity = new Vector2(horiz * 100.0f, 0);

        //var rate = (rotationRate * Mathf.Sign(horiz) * Time.deltaTime);
        //transform.Rotate(0, 0, rate, Space.Self);
        //var rot = transform.localRotation.eulerAngles.z;
        //if (rot > rotationZMax) rot = rotationZMax;
        //else if (rot < -rotationZMax) rot = -rotationZMax;
        //transform.localRotation = Quaternion.Euler(0, 0, rot);

        //transform.Translate(horiz, 0, 0);

        var camX = (Camera.transform.position - transform.position).x;
        float adjustmentShift = 0;
        if (camX < -bounds) adjustmentShift = -bounds * 1.9f;
        else if (camX > bounds) adjustmentShift = bounds * 1.9f;
        if (adjustmentShift != 0)
        {
            transform.Translate(adjustmentShift, 0, 0);
            foreach (var body in bodies)
            {
                if (body != null && body.gameObject != null && body.gameObject.activeInHierarchy)
                    body.transform.Translate(adjustmentShift, 0, 0);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        //if (Input.GetButtonDown("Fire1")) CowFace.Instance.HolyShit();
        
        UpdatePlayerMovement();
    }
}
