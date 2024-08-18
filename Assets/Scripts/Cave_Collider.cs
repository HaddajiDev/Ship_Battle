using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_Collider : MonoBehaviour
{
    public NPC_s npc;
    int hit = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.tag = "Respawn";
            hit++;
            if (hit == 1)
            {                
                Invoke("Check_Turns", 2);
            }

            if (collision.gameObject.GetComponent<Bullet>().Player_Bullet)
            {
                if (!GameManager.Instance.MissShot)
                    GameManager.Instance.MissShot = true;
            }
        }
    }

    void Check_Turns()
    {
        hit = 0;
        npc.hit = 2;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();
            npc.hit = 0;            
        }
    }
}
