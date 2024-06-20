using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check_Bought_Bullet : MonoBehaviour
{
    Button _Mybutton;
    public int index;
    
    void Start()
    {
        _Mybutton = GetComponent<Button>();

        if (GameManager.Instance.shop.bullets.data.Contains(index))
        {
            _Mybutton.interactable = false;
            _Mybutton.GetComponentInChildren<TMPro.TMP_Text>().text = "Owned";
        }
    }

    
}
