using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Cosmatic : ScriptableObject
{
    public Sprite img;
    public Type type;
    public enum Type
    {
        ship,
        cannon,
        flag,
        sail,
    }
}
