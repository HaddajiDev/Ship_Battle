using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    //Max : 7
    private int lvl_health = 1;
    public Image[] levels_health;
    private int Health_Level
    {
        get { return lvl_health; }
        set { lvl_health = value; }
    }

    //Max : 10
    private int lvl_force = 1;
    public Image[] levels_force;
    private int Force_Level
    {
        get { return lvl_force; }
        set { lvl_force = value; }
    }

    private void Awake()
    {
        Load_Data();        
    }
    public void Load_Data()
    {
        //load
    }

    public void Upgrade_Health()
    {
        Cost cost = new Cost(1000 * Health_Level, 20 + (Health_Level * 2));
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            if(Health_Level <= 7)
            {
                Ship ship = GameManager.Instance.player_1.GetComponentInParent<Ship>();
                ship.Health += 10;
                addlevels(levels_health, Health_Level);
                GameManager.Instance.Coins -= cost.Coins;
                GameManager.Instance.Diamond -= cost.Diamond;
                Health_Level++;
            }
            //Save
        }
        Debug.Log(cost.Coins);
    }

    public void Upgrade_Force()
    {
        Cost cost = new Cost(1000 * Force_Level, 20 + (Force_Level * 2));
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            if(Force_Level <= 10)
            {
                GameManager.Instance.player_1.maxForce += 5;
                addlevels(levels_force, Force_Level);
                GameManager.Instance.Coins -= cost.Coins;
                GameManager.Instance.Diamond -= cost.Diamond;
                Force_Level++;
            }            
            //Save Force
        }
    }
    
    private void addlevels(Image[] imgs, int index)
    {
        for (int i = 0; i < index; i++)
        {
            var col = imgs[i].color;
            col.a = 255;
            imgs[i].color = col;
            if (i == index - 1)
                return;
        }
    }
}


