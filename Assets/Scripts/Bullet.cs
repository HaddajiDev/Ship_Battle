using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    public int Damage;

    public BulletType type = new BulletType();
    Animator anim;
    public GameObject Fire_Partical;

    public bool Player_Bullet = false;

    public bool inFire = false;

    void Start()
    {
        cam = GameObject.FindWithTag("GameController").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = this.transform;       

        Invoke("Destroy_bullet", 10);
        Physics2D.IgnoreLayerCollision(6, 7);

        if (type == BulletType.Bomb)
        {
            //transform.DORotate(new Vector3(0, 0, 300), 5);
            anim = GetComponent<Animator>();
        }

        if (inFire)
        {
            Instantiate(Fire_Partical, gameObject.transform);
            Damage += 2;
        }
    }

    void Destroy_bullet()
    {
        if(this != null)
        {
            transform.DOScale(0, 1.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }

    public enum BulletType
    {
        Normal_Bullet,
        Bomb
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Water")
        {
            if (type == BulletType.Bomb)
            {
                anim.SetTrigger("off");
            }
                
        }
    }
}
