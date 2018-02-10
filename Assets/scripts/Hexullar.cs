using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexullar : MonoBehaviour
{
    public bool evaluating;

    public Hexel[] hexels;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Evaluate());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator Evaluate()
    {
        // Maybe experiment with progression of time (t)
        // Have a global value of time that can be stopped, paused, slowed down

        // Evaluate based on click?

        while(evaluating)
        { 
            yield return new WaitForSeconds(4);

            Debug.Log("PULSE");
        }
    }
}
