using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy_AI : MonoBehaviour
{
    public Level_Data levels;
    private GameObject BulletPrefab;
    
    public Transform shootPoint;
    private float MaxshootForce = 30f;
    private float MinshootForce = 50f;

    
    private float Min_Angle = 10;
    private float Max_Angle = 60;
    
    private bool Can_Fire = false;
    [HideInInspector] public bool Fire = false;

    private int Burst_Count;

    private int DamageMin;
    private int DamageMax;

    public Transform Canon;
    Animator anim;

    CinemachineImpulseSource source;

    private void Start()
    {
        anim = Canon.gameObject.GetComponent<Animator>();
        source = GetComponent<CinemachineImpulseSource>();        
    }

    void Shoot()
    {
        getRandomBullet(GameManager.Instance.Current_Level);
        if (Can_Fire)
        {            
            Fire = Random.Range(0, 2) == 0 ? true : false;
        }
        float angle = Random.Range(Min_Angle, Max_Angle);
        shootPoint.localRotation = Quaternion.Euler(shootPoint.rotation.x, shootPoint.rotation.y, angle);
        Canon.localRotation = Quaternion.Euler(shootPoint.rotation.x, shootPoint.rotation.y, angle);

        int Current_Burst = Random.Range(0, 3) == 0 ? Burst_Count : 1;
        for (int i = 0; i < Current_Burst; i++)
        {            
            GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float random_Force = Random.Range(MaxshootForce, MinshootForce + 1);
            rb.AddForce(Vector2.right * -random_Force, ForceMode2D.Impulse);
            rb.AddForce(Vector2.down * -random_Force / 3, ForceMode2D.Impulse);            
            Bullet bull = bullet.GetComponent<Bullet>();
            bull.Damage = Random.Range(DamageMin, DamageMax + 1);
            if (Fire)
                bull.inFire = true;
        }
        
        GameManager.Instance.isChecking = false;
        anim.SetTrigger("shoot");
        Camera_Shake.Instance.Shake(source, 1);
    }

    public void Shoot_Invoked(float value)
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(Start_Shooting(value));
    }

    IEnumerator Start_Shooting(float value)
    {
        yield return new WaitForSeconds(value);
        Shoot();
    }

    public void Get_Stats(int index)
    {
        Level lvl = levels.Get_Level(index);
        MaxshootForce = lvl.MaxshootForce;
        MinshootForce = lvl.MinshootForce;

        Max_Angle = lvl.Max_Angle;
        Min_Angle = lvl.Min_Angle;

        Can_Fire = lvl.Fire;

        Burst_Count = lvl.Burst_Shoots;

        DamageMax = lvl.DamageMax;
        DamageMin = lvl.DamageMin;
    }

    void getRandomBullet(int index)
    {
        Level lvl = levels.Get_Level(index);
        if(lvl.bullets_prefabs.Length > 1)
            BulletPrefab = lvl.bullets_prefabs[Random.Range(0, lvl.bullets_prefabs.Length)];
        else
            BulletPrefab = lvl.bullets_prefabs[0];
    }
}
