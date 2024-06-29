using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipCosmatic
{
    public Sprite Cover;
    public RuntimeAnimatorController anim;

    public Sprite half_1;
    public Sprite half_2;

    [SerializeField]
    public Cost cost;
}
