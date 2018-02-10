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

    private Color deadTextColor;
    private Color aliveTextColor;

    void Awake()
    {
        neighbours = new Hexel[6];
    }

    void Start()
    {
        textName.text = data.type.ToString();

        deadTextColor = Color.white;
        aliveTextColor = Camera.main.backgroundColor;

        SetFill(false);
    }

    public void Activate()
    {
        activated = true;

        SpawnMissingNeighbours();
    }

    void Update()
    {
        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
    }

    public void SetFill(bool s)
    {
        filled = s;

        textName.color = filled ? aliveTextColor : deadTextColor;

        if (filled) hexelRenderer.sprite = filledHexelSprite;
        else hexelRenderer.sprite = emptyHexelSprite;

        if(s) { 
            SpawnMissingNeighbours();
            particlesFill.Play();
        }
    }

    public void Mutate()
    {

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

        switch(i)
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
}
