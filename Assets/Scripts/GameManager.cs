using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using CrazyGames;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("General")]
    public Player player_1;
    public Enemy_AI player_2;
    int turn = 0;
    public CinemachineVirtualCamera cam;
    [SerializeField] private Transform End_Point;
    public Transform Main_Point;

    [HideInInspector] public bool isChecking = false;

    [Header("Abilities usage")]
    public int Fire_Uses;
    public int Burst_Uses;

    [Header("Currency")]
    public int Coins = 0;
    public int Diamond = 0;
    public int Coins_Start;

    [Header("Abilites Cost")]
    public int Fire_Cost = 60; 
    public int Burst_Cost = 120;

    [Header("Current level")]
    public int Current_Level = 0;

    [Header("Ships")]
    public GameObject[] Ships;

    public Shop shop;
    public UI_Controller uI_Controller;
    public Upgrades upgrades;

    int first;
    public int current_level
    {
        get { return Current_Level; }
        set { Current_Level = Mathf.Min(value, player_2.levels.Get_Lenght - 1); }
    }
    private void Awake()
    {
        Instance = this;
        first = PlayerPrefs.GetInt("First", first);
        Load_Data();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveSysteme.Reset();
        }
    }

    public void Play()
    {
        UI_Controller.instance.Menu_BG.interactable = true;
        UI_Controller.instance.Menu_BG.blocksRaycasts = true;
        UI_Controller.instance.Menu_BG.DOFade(1, 0.5f);
        UI_Controller.instance.Level_Counter.text = current_level.ToString();
    }

    public void Start_Game()
    {
        //ui controller
        UI_Controller.instance.Main_Menu.DOFade(0, 0.3f);
        UI_Controller.instance.Main_Menu.interactable = false;
        UI_Controller.instance.Main_Menu.blocksRaycasts = false;

        //get abilites if exited
        Check_Abilites();
        UI_Controller.instance.Menu_BG.DOFade(0, 0.3f).OnComplete(() => {
            cam.transform.DOMove(End_Point.localPosition, 5).OnComplete(() => {
                Get_Ready_UI(1);
                player_1.enabled = true;
                player_1.GetComponent<Rotate_Object>().enabled = true;
                player_2.enabled = false;
                cam.Follow = player_1.transform.parent.transform;
            });
        });
        UI_Controller.instance.Menu_BG.interactable = false;
        UI_Controller.instance.Menu_BG.blocksRaycasts = false;

        Coins_Start = Coins;
        Reset_Ships();

        player_2.Get_Stats(current_level);

        turn = 2;

        Set_Player();
        
        CrazySDK.Game.GameplayStart();
    }

    public void Check_Turn()
    {
        turn++;
        if(turn % 2 == 0)
        {
            player_1.enabled = true;
            player_1.GetComponent<Rotate_Object>().enabled = true;
            player_2.enabled = false;
            cam.Follow = player_1.transform.parent.transform;
            Check_Abilites();
            Get_Ready_UI(1);
        }
        else
        {
            player_1.enabled = false;
            player_1.GetComponent<Rotate_Object>().enabled = false;
            player_2.enabled = true;
            player_2.Shoot_Invoked(2);
            cam.Follow = player_2.transform.GetChild(1).GetChild(1).transform;            
        }        
    }
    

    public void Get_Ready_UI(float fade, float duration = 0.5f)
    {
        if(fade == 1)
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration);
            UI_Controller.instance.Getting_Ready_Object.interactable = true;
            UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = true;
        }
        else
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration).OnComplete(() => {
                UI_Controller.instance.Getting_Ready_Object.interactable = false;
                UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
            });
        }
            
    }

    void Check_Abilites()
    {       
        if (Fire_Uses == 0)
            UI_Controller.instance.Fire_Object.SetActive(false);
        if (Burst_Uses == 0)
            UI_Controller.instance.Burst_Object.SetActive(false);
    }

    public void Get_Fire()
    {        
        if(Coins >= Fire_Cost)
        {
            Fire_Uses++;
            Coins -= Fire_Cost;
        }
    }
    public void Dump_Fire()
    {
        if(Fire_Uses != 0)
        {
            Fire_Uses--;
            Coins += Fire_Cost;
        }        
    }


    public void Get_Burst()
    {        
        if (Coins >= Burst_Cost)
        {
            Burst_Uses++;
            Coins -= Burst_Cost;
        }
    }
    public void Dump_Burst()
    {
        if (Burst_Uses != 0)
        {
            Burst_Uses--;
            Coins += Burst_Cost;
        }
    }

    public void Reset_play()
    {        
        player_1.enabled = false;
        player_2.enabled = false;
        player_2.StopAllCoroutines();        
    }

    private void Reset_Ships()
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            Ships[i].gameObject.SetActive(true);
        }
        Ship[] ships = FindObjectsOfType<Ship>();
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].Get_Health();
        }

        Ships[0].GetComponentInChildren<Player>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        Ships[1].GetComponentInChildren<Enemy_AI>().Canon.localRotation = Quaternion.Euler(0, 0, 0);

        //Remove Fire if existed
    }

    public void Set_Player()
    {
        if(UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);

        if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
        if(Shop.Instance.Got_Extra_Slot == 1)
        {
            if (UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index != 0)
                UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(true);

            UI_Controller.instance.Select_Bullet_Extra.onClick.RemoveAllListeners();
            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet;
            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index;

            UI_Controller.instance.Select_Bullet_Extra.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index));

            UI_Controller.instance.Select_Bullet_Extra.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().CheckForSelcted());

            UI_Controller.instance.bullet_slot_Extra.gameObject.SetActive(true);
            UI_Controller.instance.Extra_Bullet_Buy_Button.SetActive(false);

            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().Get_Bullet_Stuff();
        }

        UI_Controller.instance.Select_Bullet_1.onClick.RemoveAllListeners();
        UI_Controller.instance.Select_Bullet_2.onClick.RemoveAllListeners();

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet;
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet;

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index;
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index;

        UI_Controller.instance.Select_Bullet_1.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index));
        UI_Controller.instance.Select_Bullet_2.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index));

        UI_Controller.instance.Select_Bullet_1.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().CheckForSelcted());
        UI_Controller.instance.Select_Bullet_2.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().CheckForSelcted());

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().Get_Bullet_Stuff();
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().Get_Bullet_Stuff();

        UI_Controller.instance.unCheckAbilities();

        UI_Controller.instance.ResetChecks();
        UI_Controller.instance.Checks[0].SetActive(true);
        player_1.SelectBullet(0);

        player_1._bulletsLimit.Clear();
    }


    public void SaveData()
    {
        SaveSysteme.SaveData(this);
    }
    public void Load_Data()
    {
        Data data = SaveSysteme.Load_Data();

        player_1.maxForce = data.MaxForce;
        Ships[0].GetComponent<Ship>().Health = data.Health;

        shop.bullets.data = data.playerBullets;

        shop.Got_Extra_Slot = data.ExtraSlot;

        uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index = data.Slot_1_index;
        uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index = data.Slot_2_index;
        uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index = data.Slot_Extra_index;

        Current_Level = data.Current_Level;

        Coins = data.Coins;
        Diamond = data.Diamond;

        Fire_Uses = data.Fire_Count;
        Burst_Uses = data.Burst_Count;

        upgrades.lvl_health = data.up_lvl_Health;
        upgrades.lvl_force = data.up_lvl_Force;
    }
    
}


