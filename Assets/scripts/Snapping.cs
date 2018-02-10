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

    void Update()
    {
        mPos = Input.mousePosition;
        wPos = Camera.main.ScreenToWorldPoint(mPos) + (Vector3.back * -10);

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] objs = Physics.OverlapSphere(wPos, 0.25f);
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].CompareTag("Hexel"))
                {
                    Hexel h = objs[i].GetComponent<Hexel>();
                    h.SetFill(true);

                    break;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(wPos, 0.25f);
    }
}
