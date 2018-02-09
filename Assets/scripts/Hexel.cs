using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexel : MonoBehaviour
{
    [Header("State")]
    public bool used;
    public Hexel[] neighbours;

    public HexelData data;

    [Header("Object References")]
    public Text textName;
    public SpriteRenderer hexelRenderer;

    [Header("Asset References")]
    public Sprite emptyHexelSprite;
    public Sprite filledHexelSprite;

    void Start()
    {
        textName.text = data.type.ToString();
    }

    void Update()
    {
        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
    }

    [ContextMenu("Use")]
    public void Use()
    {
        used = !used;

        textName.color = !used ? Color.white : Color.black;

        if (used) hexelRenderer.sprite = filledHexelSprite;
        else hexelRenderer.sprite = emptyHexelSprite;
    }

    public void Birth()
    {

    }

    public void Death()
    {

    }

    public void Mutate()
    {

    }
}
