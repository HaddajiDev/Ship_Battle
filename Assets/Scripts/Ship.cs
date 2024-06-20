using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using CrazyGames;
public class Ship : MonoBehaviour
{
    public static Ship Instance;
    float yPos, pos;
    bool cap;
    public GameObject Explode_Effect;
    public GameObject Fire_Effect;
    public int Health = 10;
    int Current_Health;

    private Vector3 Start_Pos;

    public bool player;
    public Animator Sail_Anim;
    int hit = 0;

    Animator anim;

    public CanvasGroup HealthBar_Player;
    public CanvasGroup HealthBar_Enemy;
    public GameObject Destroyed_Ship;
    public Transform Destroy_Point;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        cap = true;
        pos = transform.localPosition.y;
        yPos = transform.localPosition.y;       

        Start_Pos = transform.localPosition;
        anim = GetComponent<Animator>();                
    }

    public void Get_Health()
    {
        if (player)
            Get_Player_Health();
        else
            Get_Enemy_Health();

        if (player)
        {
            Slider slider = HealthBar_Player.GetComponentInChildren<Slider>();
            slider.maxValue = Current_Health;
            slider.value = Current_Health;
        }
        else
        {
            Slider slider = HealthBar_Enemy.GetComponentInChildren<Slider>();
            slider.maxValue = Current_Health;
            slider.value = Current_Health;
        }
    }
    
    void Update()
    {
        if (cap)
        {
            yPos -= Time.deltaTime;
            if (yPos <= pos - 0.2f)
            {
                cap = false;
            }
        }
        if (!cap)
        {
            yPos += Time.deltaTime;
            if (yPos >= pos + 0.2f)
            {
                cap = true;
            }
        }
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, 0);        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.tag = "Untagged";
            Instantiate(Explode_Effect, collision.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            anim.SetTrigger("Hit");

            hit++;
            if(hit == 1)
            {
                Invoke("Check_Turns", 1);                
            }
                
            Take_Damage(collision.gameObject.GetComponent<Bullet>().Damage);
            
            if(collision.gameObject.GetComponent<Bullet>().transform.childCount != 0)
            {
                Instantiate(Fire_Effect, collision.transform.position, Quaternion.identity, gameObject.transform);
                collision.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                GameManager.Instance.Coins += player ? Random.Range(5, 11) : 0;
            }

            //Coins
            if (!player)
            {
                GameManager.Instance.Coins += Random.value < 0.85f ? Random.Range(15, 21) : Random.Range(40, 61);
            }            
        }
    }

    void Take_Damage(int Damage)
    {
        Current_Health -= Damage;
        if (player)
        {
            HealthBar_Player.DOFade(1, 0.5f);
            Slider slider = HealthBar_Player.GetComponentInChildren<Slider>();
            slider.value = Current_Health;
        }
        else
        {
            HealthBar_Enemy.DOFade(1, 0.5f);
            Slider slider = HealthBar_Enemy.GetComponentInChildren<Slider>();
            slider.value = Current_Health;
        }
        if(Current_Health <= 0)
        {            
            gameObject.SetActive(false);
            GameManager.Instance.Coins += player ? 0 : Random.Range(50, 81);
            if (player)
            {
                Invoke("Win_Obj_Lose", 3);
                Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity);
                //Invoke("Fade_Out_Ready_Obj", 1.01f);
            }
            else
            {
                Invoke("Win_Obj_Win", 3);
                Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity).transform.localScale = new Vector3(-1, 1, 0);
            }            
            GameManager.Instance.Reset_play();            
        }
        
    }

    void Win_Obj_Win()
    {
        UI_Controller.instance.Win_Tigger(1, "You Win");
        UI_Controller.instance.Coins_text.text = "Coins : " + (GameManager.Instance.Coins - GameManager.Instance.Coins_Start).ToString();
        GameManager.Instance.current_level++;
        CrazySDK.Game.HappyTime();
        CrazySDK.Game.GameplayStop();
    }
    void Win_Obj_Lose()
    {
        UI_Controller.instance.Win_Tigger(1, "You Lost");        
        UI_Controller.instance.Coins_text.text = "Coins : " + (GameManager.Instance.Coins - GameManager.Instance.Coins_Start).ToString();
        CrazySDK.Game.GameplayStop();
    }

    void Fade_Out_Ready_Obj()
    {
        UI_Controller.instance.Getting_Ready_Object.DOFade(0, 0.2f);
        UI_Controller.instance.Getting_Ready_Object.interactable = false;
        UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
    }

    void Check_Turns()
    {
        Manager_Turn();
        if (player)
        {
            if (-transform.position.x >= -Start_Pos.x + 7 || -transform.position.x <= -Start_Pos.x + 7)
                Invoke("Move", 0.5f);            
        }
        else
        {
            if (transform.position.x >= Start_Pos.x + 7)
                Invoke("Move", 0.5f);
        }
        
    }

    void Manager_Turn()
    {
        hit = 0;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();            
        }
        Invoke("Health_Bar_Out", 2);
    }

    void Health_Bar_Out()
    {
        if (player)
        {
            HealthBar_Player.DOFade(0, 0.5f);
        }
        else
        {
            HealthBar_Enemy.DOFade(0, 0.5f);
        }
    }

    public void Move()
    {
        Sail_Anim.SetTrigger("toWind");
        transform.DOMove(new Vector3(Start_Pos.x, Start_Pos.y, 0), 2).OnComplete(() =>
        {
            Sail_Anim.SetTrigger("toNoWind");
        });
    }


    void Get_Player_Health()
    {
        Current_Health = Health;
    }

    void Get_Enemy_Health()
    {
        Enemy_AI enemy = GetComponent<Enemy_AI>();
        Level lvl = enemy.levels.Get_Level(GameManager.Instance.Current_Level);
        Health = lvl.Health;
        Current_Health = Health;
    }
}
