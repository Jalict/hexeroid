using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexel : MonoBehaviour
{
    public static readonly float DIST_BETWEEN = 3.5f;

    [Header("State")]
    public bool alive = true;
    public Hexel[] neighbours; 

    public HexelData data;

    [Header("Object References")]
    public Text textName;
    public SpriteRenderer hexelRenderer;

    [Header("Asset References")]
    public GameObject childHexel;
    public Sprite emptyHexelSprite;
    public Sprite filledHexelSprite;

    private Color deadTextColor;
    private Color aliveTextColor;

    void Start()
    {
        textName.text = data.type.ToString();

        deadTextColor = Color.white;
        aliveTextColor = Camera.main.backgroundColor;

        neighbours = new Hexel[6];
    }

    void Update()
    {
        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life

        if(Input.GetKeyUp(KeyCode.A))
        {
            for (int i = 0; i < neighbours.Length; i++)
            {
                Birth(i);
            }
        }
    }

    [ContextMenu("Invert Life")]
    public void InvertLife()
    {
        alive = !alive;

        textName.color = alive ? aliveTextColor : deadTextColor;

        if (alive) hexelRenderer.sprite = filledHexelSprite;
        else hexelRenderer.sprite = emptyHexelSprite;
    }

    private static readonly float a0 = 0;                       // Right
    private static readonly float a1 = Mathf.Deg2Rad * 60;      // Top Right
    private static readonly float a2 = Mathf.Deg2Rad * 120;     // Top Left
    private static readonly float a3 = Mathf.PI;                // Left
    private static readonly float a4 = Mathf.Deg2Rad * 240;     // Top Left
    private static readonly float a5 = Mathf.Deg2Rad * 300;     // Top Right

    public void Birth(int i)
    {
        float x = 0.0f;
        float y = 0.0f;

        switch(i)
        {
            case 0:
                x = Mathf.Cos(a0);
                y = Mathf.Sin(a0);
            break;
            case 1:
                x = Mathf.Cos(a1);
                y = Mathf.Sin(a1);
            break;
            case 2:
                x = Mathf.Cos(a2);
                y = Mathf.Sin(a2);
            break;
            case 3:
                x = Mathf.Cos(a3);
                y = Mathf.Sin(a3);
            break;
            case 4:
                x = Mathf.Cos(a4);
                y = Mathf.Sin(a4);
            break;
            case 5:
                x = Mathf.Cos(a5);
                y = Mathf.Sin(a5);
            break;
        }

        Vector3 pos = transform.position + (new Vector3(x, y, 0) * DIST_BETWEEN);

        Instantiate(childHexel, pos, Quaternion.identity);
    }

    public void Mutate()
    {

    }
}
