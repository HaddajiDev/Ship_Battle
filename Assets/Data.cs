using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public float MaxForce;
    public int Health;
    public List<int> playerBullets;
    public int ExtraSlot;

    public int Slot_1_index;
    public int Slot_2_index;
    public int Slot_Extra_index;

    public int Current_Level;

    public int Coins;
    public int Diamond;

    public int Fire_Count;
    public int Burst_Count;

    public int up_lvl_Health;
    public int up_lvl_Force;

    public Data(GameManager manager)
    {
        MaxForce = manager.player_1.maxForce;
        Health = manager.Ships[0].GetComponent<Ship>().Health;

        playerBullets = manager.shop.bullets.data;
        ExtraSlot = manager.shop.Got_Extra_Slot;

        Slot_1_index = manager.uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index;
        Slot_2_index = manager.uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index;
        Slot_Extra_index = manager.uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index;

        Current_Level = manager.Current_Level;

        Coins = manager.Coins;
        Diamond = manager.Diamond;

        Fire_Count = manager.Fire_Uses;
        Burst_Count = manager.Burst_Uses;

        up_lvl_Health = manager.upgrades.lvl_health;
        up_lvl_Force = manager.upgrades.lvl_force;
    }
}
