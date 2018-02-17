using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexel : MonoBehaviour
{
    [Header("State")]
    public bool activated = false;
    public bool filled = true;
    public Hexel[] neighbours; 

    public HexelData data;

    [Header("Object References")]
    public Text textName;
    public SpriteRenderer hexelRenderer;
    public ParticleSystem particlesFill;

    [Header("Asset References")]
    public GameObject childHexel;
    public Sprite emptyHexelSprite;
    public Sprite filledHexelSprite;

    private Color emptyTextColor;
    private Color filledTextColor;

    private Color mutateSpriteColor;

    void Awake()
    {
        neighbours = new Hexel[6];
    }

    void Start()
    {
        //textName.text = data.type.ToString();

        emptyTextColor = Color.white;
        filledTextColor = Camera.main.backgroundColor;
        mutateSpriteColor = Color.red;

        SetFill(false);

        for (int id = 0; id < neighbours.Length; id++)
        {
            if (neighbours[id]) continue;

            Vector3 checkPos = transform.position + HexelHelper.GetOffsetPosition(id);

            Collider[] objs = Physics.OverlapSphere(checkPos, 0.25f);
            for (int o = 0; o < objs.Length; o++)
            {
                if (objs[o].CompareTag("Hexel"))
                {
                    neighbours[id] = objs[o].GetComponent<Hexel>();
                    if (!neighbours[id].neighbours[InverseIndex(id)]) neighbours[id].neighbours[InverseIndex(id)] = this;
                }
            }
        }

        Score.instance.cells++;
    }

    public void Update()
    {
        // Check for dead children
        for(int i = 0; i < neighbours.Length;i++)
        {
            if (neighbours[i] && !neighbours[i].gameObject) neighbours[i] = null;
        }
    }

    IEnumerator TickNeighbours()
    {
        while(filled)
        {
            for(int i = 0; i < neighbours.Length;i++)
            {
                if (neighbours[i])
                    neighbours[i].CheckNeighbours();

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public bool mutate;

    public void CheckNeighbours()
    {
        int liveNeighbours = 0;
        int fullNeighbours = 0;

        for(int i = 0;i < neighbours.Length; i++)
        {
            if (neighbours[i]) liveNeighbours++;
            if (neighbours[i] && neighbours[i].filled) fullNeighbours++;
        }

        if (fullNeighbours >= 5)
        {
            Mutate();
            return;
        }

        if (liveNeighbours < 2 || liveNeighbours > 4) { 
            Die();
            return;
        }


        if (fullNeighbours > 2 && fullNeighbours < 5) { 
            SetFill(true);
            return;
        }

    }

    private void Die()
    {
        if (filled) { 
            Score.instance.score--;
        }

        Score.instance.cells--;

        Destroy(gameObject);
    }

    public void Activate()
    {
        activated = true;

        SpawnMissingNeighbours();
    }

    private bool doubleCheck = false;
    public void SetFill(bool s)
    {
        filled = s;

        textName.color = filled ? filledTextColor : emptyTextColor;

        if (filled) hexelRenderer.sprite = filledHexelSprite;
        else hexelRenderer.sprite = emptyHexelSprite;

        if(filled && !doubleCheck) {
            Score.instance.score++;

            SpawnMissingNeighbours();
            particlesFill.Play();
            StartCoroutine(TickNeighbours());

            doubleCheck = true;
        }
    }


    public Coroutine mutateCheck;
    public void Mutate()
    {
        hexelRenderer.color = mutateSpriteColor;

        mutate = true;

        if (filled) Score.instance.score--;
        Score.instance.cells--;

        mutateCheck = StartCoroutine(MuateTick());
    }

    IEnumerator MuateTick()
    {
        while(mutate)
        {
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i])
                {
                    if (neighbours[i] && !filled)
                        neighbours[i].Die();
                    if (neighbours[i] && filled)
                        neighbours[i].Mutate();
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void SpawnMissingNeighbours()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i]) continue;

            SpawnNeighbour(i);
        }
    }

    public void SpawnNeighbour(int i)
    {
        float x = 0.0f;
        float y = 0.0f;

        switch (i)
        {
            case 0:
                x = Mathf.Cos(HexelHelper.a0);
                y = Mathf.Sin(HexelHelper.a0);
                break;
            case 1:
                x = Mathf.Cos(HexelHelper.a1);
                y = Mathf.Sin(HexelHelper.a1);
                break;
            case 2:
                x = Mathf.Cos(HexelHelper.a2);
                y = Mathf.Sin(HexelHelper.a2);
                break;
            case 3:
                x = Mathf.Cos(HexelHelper.a3);
                y = Mathf.Sin(HexelHelper.a3);
                break;
            case 4:
                x = Mathf.Cos(HexelHelper.a4);
                y = Mathf.Sin(HexelHelper.a4);
                break;
            case 5:
                x = Mathf.Cos(HexelHelper.a5);
                y = Mathf.Sin(HexelHelper.a5);
                break;
        }

        Vector3 pos = transform.position + (new Vector3(x, y, 0) * HexelHelper.DIST_BETWEEN);

        neighbours[i] = Instantiate(childHexel, pos, Quaternion.identity).GetComponent<Hexel>();
        neighbours[i].neighbours[InverseIndex(i)] = this;
    }

    public int InverseIndex(int i)
    {
        int r = -1;

        // Lets not talk about this
        switch(i)
        {
            case 0:
                r = 3;
                break;
            case 1:
                r = 4;
                break;
            case 2:
                r = 5;
                break;
            case 3:
                r = 0;
                break;
            case 4:
                r = 1;
                break;
            case 5:
                r = 2;
                break;
        }

        if (r == -1)
            throw new System.IndexOutOfRangeException("Inverse Index not found");

        return r;
    }

    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < neighbours.Length; i++) {
            if (!neighbours[i]) continue;
            Gizmos.DrawLine(transform.position, neighbours[i].transform.position);
        }
    }
}

public static class HexelHelper
{
    public static readonly float DIST_BETWEEN = 3.5f;

    public static readonly float a0 = 0;                       // Right
    public static readonly float a1 = Mathf.Deg2Rad * 60;      // Top Right
    public static readonly float a2 = Mathf.Deg2Rad * 120;     // Top Left
    public static readonly float a3 = Mathf.PI;                // Left
    public static readonly float a4 = Mathf.Deg2Rad * 240;     // Top Left
    public static readonly float a5 = Mathf.Deg2Rad * 300;     // Top Right

    public static Vector3 GetOffsetPosition( int i)
    {
        float x = 0.0f;
        float y = 0.0f;

        switch (i)
        {
            case 0:
                x = Mathf.Cos(HexelHelper.a0);
                y = Mathf.Sin(HexelHelper.a0);
                break;
            case 1:
                x = Mathf.Cos(HexelHelper.a1);
                y = Mathf.Sin(HexelHelper.a1);
                break;
            case 2:
                x = Mathf.Cos(HexelHelper.a2);
                y = Mathf.Sin(HexelHelper.a2);
                break;
            case 3:
                x = Mathf.Cos(HexelHelper.a3);
                y = Mathf.Sin(HexelHelper.a3);
                break;
            case 4:
                x = Mathf.Cos(HexelHelper.a4);
                y = Mathf.Sin(HexelHelper.a4);
                break;
            case 5:
                x = Mathf.Cos(HexelHelper.a5);
                y = Mathf.Sin(HexelHelper.a5);
                break;
        }

        return new Vector3(x, y, 0) * HexelHelper.DIST_BETWEEN;
    }
}
