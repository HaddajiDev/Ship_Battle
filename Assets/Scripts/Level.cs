using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int level;

    [Header("Bullets")]
    public GameObject[] bullets_prefabs;

    [Header("Health")]
    public int Health = 20;

    [Header("Force")]
    public float MaxshootForce = 30;
    public float MinshootForce = 50;

    [Space]
    [Header("Angle")]
    public float Min_Angle = 10;
    public float Max_Angle = 60;

    [Space]
    [Header("Fire Shots")]
    public bool Fire = false;

    [Space]
    [Header("Burst Shots")]    
    public int Burst_Shoots = 1;

    [Space(10)]
    [Header("Damage")]
    public int DamageMin = 1;
    public int DamageMax = 2;    
}
