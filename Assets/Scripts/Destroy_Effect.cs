using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destroy_Effect : MonoBehaviour
{
    public float Wait;

    public bool Ship;

    private void Start()
    {
        
    }

    void Update()
    {
        if (!Ship)
            Destroy(gameObject, Wait);
        else
            Invoke("ship_Des", Wait);
    }

    void ship_Des()
    {
        foreach (Transform child in transform)
        {
            child.DOScale(0, 1);
        }
        Destroy(gameObject, 1.2f);
    }
}
