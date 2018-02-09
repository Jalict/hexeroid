using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hexel Type", menuName = "Hexeroid/Hexels")]
public class HexelData : ScriptableObject {
    public HexelType type;

    // time to new child
    // needed for new child
    // amount to generate
}

public enum HexelType
{
    IRIBU,
    ALUTE,
    CELER
}
