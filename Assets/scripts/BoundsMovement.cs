using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsMovement : MonoBehaviour
{
    public float dist;
    public float speed;

    private Vector3 m = Vector3.zero;  // Mouse
    private Vector3 s = Vector3.zero;  // Screen 

	// Update is called once per frame
	void Update ()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (!screenRect.Contains(Input.mousePosition))
            return;

        m = Input.mousePosition;
        s.x = Screen.width / 2;
        s.y = Screen.height / 2;

        Vector3 pos = transform.position;       // Current camera position
        Vector3 vec = Vector3.zero;             // Movement vector     
            
        if(m.x < dist || m.x > Screen.width - dist || m.y < dist || m.y > Screen.height - dist)
        {
            vec = Vector3.Normalize(new Vector3(m.x - s.x, m.y - s.y, m.z - s.z)) * speed * Time.deltaTime;
        } 

        transform.position = pos + vec;
    }
}
