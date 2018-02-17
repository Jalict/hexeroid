using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {

    private Vector3 mPos;   // Mouse position (on screen)
    private Vector3 wPos;   // Mouse position (in world)
    private Vector3 nPos;   // Nearest snapping pos from mouse

    private float halfDist;

	void Start ()
    {
        nPos = Vector3.zero;

        halfDist = HexelHelper.DIST_BETWEEN / 2;
    }

    private Vector3 lastMousePos;
    private bool lastHold;

    void Update()
    {
        mPos = Input.mousePosition;
        wPos = Camera.main.ScreenToWorldPoint(mPos) + (Vector3.back * -10);

        if(!lastHold) { 
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Collider[] objs = Physics.OverlapSphere(wPos, 0.25f);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].CompareTag("Hexel"))
                    {
                        Hexel h = objs[i].GetComponent<Hexel>();
                        if (h.mutate) continue;
                        h.SetFill(true);

                        break;
                    }
                }
            }
        }

        if(Input.mouseScrollDelta != Vector2.zero)
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.mouseScrollDelta.y * Time.deltaTime * 100, 5, 50);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Collider[] objs = Physics.OverlapSphere(wPos, 0.25f);
            if (objs.Length <= 0)
            {
                Camera.main.transform.position -= (mPos - lastMousePos)/(500/Camera.main.orthographicSize);
            }
        }

        lastMousePos = mPos;

        if (Input.GetKeyUp(KeyCode.Mouse0)) lastHold = false;
        if (Input.GetKeyDown(KeyCode.Mouse0)) lastHold = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(wPos, 0.25f);
    }

    public void GoHome()
    {
        transform.position = new Vector3(0, 0, -10);
    }
}
