using Cinemachine;
using CrazyGames;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using System;


///<summary>
///
/// Fire removed
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("General")]
    public Player player_1;
    public Enemy_AI player_2;
    int turn = 0;
    public CinemachineVirtualCamera cam;
    public Camera cam_;
    [SerializeField] private Transform End_Point; // Camera player pos
    public Transform Main_Point;

    public Transform PlayerStuff;
    public Level_Data level_Data;

    [HideInInspector] public bool isChecking = false;

    [Header("Abilities usage")]
    public int Fire_Uses;
    public int Burst_Uses;

    [Header("Power Ups")]
    [Header("Sheild")]
    public GameObject SheildObj;
    public int Sheild_UsagePerGame = 2;
    private int current_Usage_Sheild;
    public int Sheild_count; // sheild block shots

    [Header("Freez")]
    public GameObject FreezObj;
    public int Freez_count; // freez upcoming shots
    private int current_Usage_freeze;
    public int Freez_UsagePerGame = 2;

    [Header("Tiny Shots")]
    public int TinyShots_count; // make upcoming shots tiny and reduce damage by 50%
    private int current_Usage_TinyShots;
    public int TinyShots_UsagePerGame = 3;

    [Header("Tiny Ship")]
    public int TinyShip_count; // make ship tiny
    private int current_Usage_TinyShip;
    public int TinyShip_UsagePerGame = 3;

    [Header("Power Ups Cost")]
    public Cost Sheild_cost = new Cost(1200, 25);
    public Cost Freez_cost = new Cost(1500, 35);
    public Cost TinyShots_cost = new Cost(1000, 10);
    public Cost TinyShip_cost = new Cost(1100, 15);

    [Header("Currency")]
    public int Coins = 50000;
    public int Diamond = 1000;
    public int Coins_Start;

    [Header("Abilites Cost")]
    public Cost Fire_Cost = new Cost(60, 0);
    public Cost Burst_Cost = new Cost(120, 0);

    [Header("Current level")]
    public int Current_Level = 0;

    [Header("Ships")]
    public GameObject[] Ships;

    [Header("Quests")]
    public DateTime lastLogin;
    public List<int> currentQuests;
    public QuestData questsData;
    public int WinCount;
    public int FireShots;
    public bool MissShot;
    public int noMissShots;
    public int ReadNotification;

    [Header("Gifts")]
    public List<int> currentGifts;
    public GiftsData giftsData;

    [Header("Account")]
    public int totalMatchLost;
    public int totalMatchWin;
    public int FireShotsHit;
    public int TotalShotsFired;
    public int TotalShotsHit;
    public int TotalShotsMiss;
    [Header("Game Phase")]
    public GamePhase phase;
    [Space(20)]

    public Shop shop;
    public UI_Controller uI_Controller;
    public Upgrades upgrades;

    int first;

    [HideInInspector]
    public string username;
    public string imgURL;
    public string token;

    bool isAvailable;

    private bool WatchedRewardAd;
    [HideInInspector] public bool Revived = false;

    

    private void Awake()
    {
        Instance = this;
        
        first = PlayerPrefs.GetInt("First", first);
        if(PlayerPrefs.GetInt("First") == 0)
        {
            PlayerPrefs.SetInt("First", 1);
            setDefaultData();            
        }        
        LoadData();
        CrazySDK.User.SyncUnityGameData();
    }

    private async void Start()
    {
        var isAvailable = CrazySDK.User.IsUserAccountAvailable;
        if (isAvailable == false)
            UI_Controller.instance.AccountButton.SetActive(false);

        PortalUser user = await GetCurrentUser();
        if(user != null)
        {
            await DownloadImageAsync(user.profilePictureUrl);
            UI_Controller.instance.username.text = user.username;            
            username = user.username;
            imgURL = user.profilePictureUrl;
            UI_Controller.instance.SignInButton.SetActive(false);
        }
        phase = GamePhase.MainMenu;
    }

    public void Play()
    {
        UI_Controller.instance.Menu_BG.interactable = true;
        UI_Controller.instance.Menu_BG.blocksRaycasts = true;
        UI_Controller.instance.Menu_BG.DOFade(1, 0.5f);
        UI_Controller.instance.Level_Counter.text = Current_Level.ToString();
        UI_Controller.instance.SetAbilitesCount();
    }
    void MovePlayerAndCamera()
    {
        PlayerStuff.localPosition = new Vector3(0, 0, 0);
        End_Point.localPosition = new Vector3(-49.97f, 2.18f, -18.4f);
        Level lvl = level_Data.Get_Level(Current_Level);
        PlayerStuff.localPosition = new Vector3(-lvl.distance, 0, 0);
        End_Point.localPosition = new Vector3(End_Point.localPosition.x - lvl.distance, End_Point.localPosition.y, -18.4f);
    }
    public void Start_Game()
    {        
        //ui controller
        UI_Controller.instance.Main_Menu.DOFade(0, 0.3f);
        UI_Controller.instance.Main_Menu.interactable = false;
        UI_Controller.instance.Main_Menu.blocksRaycasts = false;
        UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(false);

        MovePlayerAndCamera();
        phase = GamePhase.Start;
        //get abilites if exited
        Check_Abilites();
        UI_Controller.instance.Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            cam_.GetComponent<CameraFollow>().SetTarget(null);
            cam_.transform.DOMove(End_Point.localPosition, 5).OnComplete(() =>
            {
                UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(true);
                Get_Ready_UI(1);
                phase = GamePhase.ReadyPhase;
                player_1.enabled = true;
                player_1.GetComponent<Rotate_Object>().enabled = true;
                player_2.enabled = false;
                cam_.GetComponent<CinemachineBrain>().enabled = true;
                cam.gameObject.SetActive(true);
                cam.Follow = player_1.transform.parent.transform;
                //cam_.GetComponent<CameraFollow>().SetTarget(player_1.transform.parent.transform);
            });
        });
        UI_Controller.instance.Menu_BG.interactable = false;
        UI_Controller.instance.Menu_BG.blocksRaycasts = false;
        
        Coins_Start = Coins;
        Reset_Ships(false);
        Set_Player();
        player_1.GetAllSkins();
        player_2.Get_Stats(Current_Level);
        turn = 2;
        MissShot = false;
        WatchedRewardAd = false;
        Revived = false;
        ResetPowerUpsCurrent();

        UI_Controller.instance.SetPlayerStats();
        CrazySDK.Game.GameplayStart();
    }

    public void Check_Turn()
    {
        turn++;
        if (turn % 2 == 0)
        {
            player_1.enabled = true;
            player_1.GetComponent<Rotate_Object>().enabled = true;
            player_2.enabled = false;
            cam.Follow = player_1.transform.parent.transform;            
            Check_Abilites();            
            Get_Ready_UI(1);
            phase = GamePhase.ReadyPhase;
            UncheckPowerUps();
            if (SheildObj.activeInHierarchy)
                SheildObj.SetActive(false);
        }
        else
        {
            player_1.enabled = false;
            player_1.GetComponent<Rotate_Object>().enabled = false;
            player_2.enabled = true;
            player_2.Shoot_Invoked(2);
            cam.Follow = player_2.transform.GetChild(1).GetChild(1).transform;
            phase = GamePhase.EnemyReadyPhase;            
        }
    }


    public void Get_Ready_UI(float fade, float duration = 0.5f)
    {
        if (fade == 1)
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration);
            UI_Controller.instance.Getting_Ready_Object.interactable = true;
            UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = true;
        }
        else
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration).OnComplete(() =>
            {
                UI_Controller.instance.Getting_Ready_Object.interactable = false;
                UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
            });
        }

    }

    void Check_Abilites()
    {
        if (Fire_Uses == 0)
            UI_Controller.instance.Fire_Object.SetActive(false);
        else
            UI_Controller.instance.Fire_Object.SetActive(true);

        if (Burst_Uses == 0)
            UI_Controller.instance.Burst_Object.SetActive(false);
        else
            UI_Controller.instance.Burst_Object.SetActive(true);
    }

    public void Check_PowerUps()
    {
        UI_Controller.instance.All_PowerUps.SetActive(true);
        if (Sheild_count > 0 && current_Usage_Sheild < Sheild_UsagePerGame)
            UI_Controller.instance.Sheild_Object.SetActive(true);
        else
            UI_Controller.instance.Sheild_Object.SetActive(false);

        if (Freez_count > 0 && current_Usage_freeze < Freez_UsagePerGame)
            UI_Controller.instance.Freez_Object.SetActive(true);
        else
            UI_Controller.instance.Freez_Object.SetActive(false);

        if (TinyShots_count > 0 && current_Usage_TinyShots < TinyShots_UsagePerGame)
            UI_Controller.instance.TinyShots_Object.SetActive(true);
        else
            UI_Controller.instance.TinyShots_Object.SetActive(false);

        if (TinyShip_count > 0 && current_Usage_TinyShip < TinyShip_UsagePerGame)
            UI_Controller.instance.TinyShip_Object.SetActive(true);
        else
            UI_Controller.instance.TinyShip_Object.SetActive(false);
    }

    void UncheckPowerUps()
    {
        UI_Controller.instance.All_PowerUps.SetActive(false);
        UI_Controller.instance.Sheild_Object.SetActive(false);
        UI_Controller.instance.Freez_Object.SetActive(false);
        UI_Controller.instance.TinyShots_Object.SetActive(false);
        UI_Controller.instance.TinyShip_Object.SetActive(false);        
    }
    void ResetPowerUpsCurrent()
    {
        current_Usage_freeze = 0;
        current_Usage_Sheild = 0;
        current_Usage_TinyShip = 0;
        current_Usage_TinyShots = 0;
    }
    public void Get_Fire()
    {
        if (Coins >= Fire_Cost.Coins)
        {
            Fire_Uses++;
            Coins -= Fire_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
            SaveData("coins", Coins);
        }
    }
    public void Dump_Fire()
    {
        if (Fire_Uses != 0)
        {
            Fire_Uses--;
            Coins += Fire_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
            SaveData("coins", Coins);
        }
    }
    public void Get_Burst()
    {
        if (Coins >= Burst_Cost.Coins)
        {
            Burst_Uses++;
            Coins -= Burst_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
            SaveData("coins", Coins);
        }
    }
    public void Dump_Burst()
    {
        if (Burst_Uses != 0)
        {
            Burst_Uses--;
            Coins += Burst_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
            SaveData("coins", Coins);
        }
    }

    public void Reset_play()
    {
        player_1.enabled = false;
        player_2.enabled = false;
        player_2.StopAllCoroutines();
    }

    private void Reset_Ships(bool restart)
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            Ships[i].gameObject.SetActive(true);
        }
        Ship[] ships = FindObjectsOfType<Ship>();
        for (int i = 0; i < ships.Length; i++)
        {
            if (!restart)
                ships[i].Get_Health();
            else
            {
                if (ships[i].player)
                    ships[i].Get_Player_Health();
            }

            if(ships[i].Start_Pos != null)
                ships[i].transform.localPosition = ships[i].Start_Pos;
        }

        Ships[0].GetComponentInChildren<Player>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        Ships[1].GetComponentInChildren<Enemy_AI>().Canon.localRotation = Quaternion.Euler(0, 0, 0);

        Transform fireTransform_0 = Ships[0].transform.Find("Fire(Clone)");
        if (fireTransform_0 != null)
        {
            GameObject fire_0 = fireTransform_0.gameObject;
            Destroy(fire_0);
        }

        Transform fireTransform_1 = Ships[1].transform.Find("Fire(Clone)");
        if (fireTransform_1 != null)
        {
            GameObject fire_1 = fireTransform_1.gameObject;
            Destroy(fire_1);
        }       
    }

    public void Set_Player()
    {
        if (UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);
        else
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(true);

        if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
        else
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(true);

        if (Shop.Instance.Got_Extra_Slot == 1)
        {
            if (UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index == 0)
                UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(false);
            else
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
        player_1.ready = false;
        player_1._bulletsLimit.Clear();
    }


    public void SaveData(string key, int value)
    {        
        CrazySDK.Data.SetInt(key, value);
    }
    public void SaveData(string key, float value)
    {        
        CrazySDK.Data.SetFloat(key, value);
    }
    public void SaveData(string key, string value)
    {        
        CrazySDK.Data.SetString(key, value);
    }
    public void SaveData(string key, List<int> value)
    {        
        string bulletList = string.Join(",", value.ConvertAll(i => i.ToString()).ToArray());
        CrazySDK.Data.SetString(key, bulletList);
    }



    public void SaveLastLogin()
    {
        lastLogin = DateTime.Now;
        CrazySDK.Data.SetString("LastLogin", lastLogin.ToString("o"));
    }
    public void SaveLogin()
    {        
        string lastLoginString = CrazySDK.Data.GetString("LastLogin");
        DateTime lastLogin;
        
        if (DateTime.TryParse(lastLoginString, out lastLogin))
        {            
            lastLogin = lastLogin.AddDays(1);
        }
        else
        {            
            lastLogin = DateTime.Now;
        }
        
        CrazySDK.Data.SetString("LastLogin", lastLogin.ToString("o"));
    }

    public bool Has24HoursPassed()
    {
        if (CrazySDK.Data.HasKey("LastLogin"))
        {
            string lastLoginStr = CrazySDK.Data.GetString("LastLogin");
            DateTime lastLogin = DateTime.Parse(lastLoginStr);

            TimeSpan timeSinceLastLogin = DateTime.Now - lastLogin;            
            if (timeSinceLastLogin.TotalHours >= 24)
            {
                return true;
            }
        }
        return false;
    }

    void GenerateQuests()
    {
        currentQuests.Clear();
        HashSet<int> uniqueQuests = new HashSet<int>();
        HashSet<Quest.Type> usedQuestTypes = new HashSet<Quest.Type>();

        for (int i = 0; i < 3; i++)
        {
            System.Random random = new System.Random();
            int value;
            Quest quest;

            do
            {
                value = random.Next(0, questsData.Get_Length);
                quest = questsData.Get_Quest(value);
            } while (uniqueQuests.Contains(value) || usedQuestTypes.Contains(quest.type));
            
            uniqueQuests.Add(value);
            usedQuestTypes.Add(quest.type);
            
            currentQuests.Add(value);
        }

        SetQuestsValues();
        QuestSpawner.instance.NoQuests.SetActive(false);
    }

    void GenerateGifts()
    {
        currentGifts.Clear();
        HashSet<int> uniqueGifts = new HashSet<int>();

        for (int i = 0; i < 2; i++)
        {
            System.Random random = new System.Random();
            int value;
            do
            {
                value = random.Next(0, giftsData.Get_Length);
            } while (uniqueGifts.Contains(value));            
            uniqueGifts.Add(value);
            currentGifts.Add(value);
        }
    }

    private void SetQuestsValues()
    {
        CrazySDK.Data.SetInt("WinCount", 0);
        CrazySDK.Data.SetInt("FireShots", 0);
        CrazySDK.Data.SetInt("noMissShots", 0);
        CrazySDK.Data.SetInt("readNotification", 0);
    }

    private void LoadQuestValue()
    {
        WinCount =  CrazySDK.Data.GetInt("WinCount");
        FireShots = CrazySDK.Data.GetInt("FireShots");
        noMissShots =  CrazySDK.Data.GetInt("noMissShots");
        ReadNotification = CrazySDK.Data.GetInt("readNotification");
    }

    public void CheckForQuestsAndGifts()
    {
        if (Has24HoursPassed())
        {
            GenerateQuests();
            GenerateGifts();
            SaveLogin();
            SetList("current_Quests", currentQuests);
            SetList("current_Gifts", currentGifts);
            SetQuestsValues();
        }
        else
        {
            LoadList("current_Quests", currentQuests);
            LoadList("current_Gifts", currentGifts);
            LoadQuestValue();
        }
    }

    public Quest.Type CheckQuestType(int index)
    {
        if (currentQuests.Contains(index))
        {
            Quest quest = questsData.Get_Quest(index);
            return quest.type;
        }
        return Quest.Type.none;
    }

    public async void SignIn()
    {
        if (isAvailable && username != null)
        {
            UserInfo userInfo = await Show_Prompt();
            await DownloadImageAsync(userInfo.User.profilePictureUrl);
            UI_Controller.instance.username.text = userInfo.User.username;            
            username = userInfo.User.username;
            imgURL = userInfo.User.profilePictureUrl;
            token = userInfo.Token;
            SaveData("username", username);
            SaveData("imgUrl", imgURL);
            SaveData("token", token);
        }        
    }

    public async Task<UserInfo> Show_Prompt()
    {
        TaskCompletionSource<UserInfo> taskCompletionSource = new TaskCompletionSource<UserInfo>();

        CrazySDK.User.ShowAuthPrompt((error, user) =>
        {
            if (error != null)
            {                
                taskCompletionSource.SetResult(null);
                return;
            }

            CrazySDK.User.GetUserToken((tokenError, token) =>
            {
                if (tokenError != null)
                {                    
                    taskCompletionSource.SetResult(null);
                    return;
                }

                UserInfo userInfo = new UserInfo
                {
                    User = user,
                    Token = token
                };

                taskCompletionSource.SetResult(userInfo);
            });
        });

        return await taskCompletionSource.Task;
    }

    public async Task<PortalUser> GetCurrentUser()
    {
        TaskCompletionSource<PortalUser> taskCompletionSource = new TaskCompletionSource<PortalUser>();

        CrazySDK.User.GetUser(user =>
        {
            if (user != null)
            {
                Debug.Log("Get user result: " + user);
                taskCompletionSource.SetResult(user);
            }
            else
            {
                Debug.Log("User is not logged in");
                taskCompletionSource.SetResult(null);
            }
        });

        return await taskCompletionSource.Task;
    }    
    void setDefaultData()
    {
        CrazySDK.Data.SetFloat("force", player_1.maxForce);
        CrazySDK.Data.SetInt("health", Ships[0].GetComponent<Ship>().Health);
        
        CrazySDK.Data.SetInt("extraSlot", shop.Got_Extra_Slot);

        CrazySDK.Data.SetInt("slot1", uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index);
        CrazySDK.Data.SetInt("slot2", uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index);
        CrazySDK.Data.SetInt("slotExtra", uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index);

        CrazySDK.Data.SetInt("level", Current_Level);

        CrazySDK.Data.SetInt("coins", Coins);
        CrazySDK.Data.SetInt("diamond", Diamond);

        CrazySDK.Data.SetInt("fireUses", Fire_Uses);
        CrazySDK.Data.SetInt("burstUses", Burst_Uses);

        CrazySDK.Data.SetInt("levelHealth", upgrades.lvl_health);
        CrazySDK.Data.SetInt("levelForce", upgrades.lvl_force);

        //acountStuff
        CrazySDK.Data.SetString("username", "");
        CrazySDK.Data.SetString("imgUrl", "");
        CrazySDK.Data.SetString("token", "");

        CrazySDK.Data.SetInt("totalWins", 0);
        CrazySDK.Data.SetInt("totalLost", 0);
        CrazySDK.Data.SetInt("totalShotsFired", 0);
        CrazySDK.Data.SetInt("fireShotsHit", 0);
        CrazySDK.Data.SetInt("totalShotsHit", 0);
        CrazySDK.Data.SetInt("totalShotsMiss", 0);

        //player bullets
        SetList("bullets", shop.bullets.data);

        //skins
        SetList("ship_skins", shop.skins.Ships_Skins);
        SetList("sail_skins", shop.skins.Sail_Skins);
        SetList("flag_skins", shop.skins.Flag_Skins);
        SetList("cannon_skins", shop.skins.Cannon_Skins);
        SetList("anchor_skins", shop.skins.Anchors_Skins);
        SetList("helm_skins", shop.skins.Helm_Skins);

        //selected skin
        CrazySDK.Data.SetInt("select_skin_ship", player_1._selectedShip);
        CrazySDK.Data.SetInt("select_skin_sail", player_1._selectedSail);
        CrazySDK.Data.SetInt("select_skin_flag", player_1._selectedFlag);
        CrazySDK.Data.SetInt("select_skin_cannon", player_1._selectedCannon);
        CrazySDK.Data.SetInt("select_skin_anchor", player_1._selectedAnchor);
        CrazySDK.Data.SetInt("select_skin_helm", player_1._selectedHelm);

        SaveLastLogin();

        //current Quests
        GenerateQuests();
        SetList("current_Quests", currentQuests);

        //gifts
        GenerateGifts();
        SetList("current_Gifts", currentGifts);
    }

    void LoadData()
    {
        player_1.maxForce = CrazySDK.Data.GetFloat("force");
        Ships[0].GetComponent<Ship>().Health = CrazySDK.Data.GetInt("health");
        
        shop.Got_Extra_Slot = CrazySDK.Data.GetInt("extraSlot");

        uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index = CrazySDK.Data.GetInt("slot1");
        uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index = CrazySDK.Data.GetInt("slot2");
        uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index = CrazySDK.Data.GetInt("slotExtra");

        Current_Level = CrazySDK.Data.GetInt("level");

        Coins = CrazySDK.Data.GetInt("coins");
        Diamond = CrazySDK.Data.GetInt("diamond");

        Fire_Uses = CrazySDK.Data.GetInt("fireUses");
        Burst_Uses = CrazySDK.Data.GetInt("burstUses");

        upgrades.lvl_health = CrazySDK.Data.GetInt("levelHealth");
        upgrades.lvl_force = CrazySDK.Data.GetInt("levelForce");

        //acountStuff
        username = CrazySDK.Data.GetString("username");
        imgURL = CrazySDK.Data.GetString("imgUrl");
        token = CrazySDK.Data.GetString("token");

        totalMatchWin = CrazySDK.Data.GetInt("totalWins");
        totalMatchLost = CrazySDK.Data.GetInt("totalLost");
        TotalShotsFired = CrazySDK.Data.GetInt("totalShotsFired");
        FireShotsHit = CrazySDK.Data.GetInt("fireShotsHit");
        TotalShotsHit = CrazySDK.Data.GetInt("totalShotsHit");
        TotalShotsMiss = CrazySDK.Data.GetInt("totalShotsMiss");

        //Player Bullets
        LoadList("bullets", shop.bullets.data);

        //skins
        LoadList("ship_skins", shop.skins.Ships_Skins);
        LoadList("sail_skins", shop.skins.Sail_Skins);
        LoadList("flag_skins", shop.skins.Flag_Skins);
        LoadList("cannon_skins", shop.skins.Cannon_Skins);
        LoadList("anchor_skins", shop.skins.Anchors_Skins);
        LoadList("helm_skins", shop.skins.Helm_Skins);


        //selected skins
        player_1._selectedShip =  CrazySDK.Data.GetInt("select_skin_ship");
        player_1._selectedSail = CrazySDK.Data.GetInt("select_skin_sail");
        player_1._selectedFlag = CrazySDK.Data.GetInt("select_skin_flag");
        player_1._selectedCannon = CrazySDK.Data.GetInt("select_skin_cannon");
        player_1._selectedAnchor = CrazySDK.Data.GetInt("select_skin_anchor");
        player_1._selectedHelm = CrazySDK.Data.GetInt("select_skin_helm");

        //quests and gifts
        CheckForQuestsAndGifts();
    }

    public void SetList(string key, List<int> list)
    {
        if (key.Contains("skins"))
        {
            list.Add(0);
        }        
        string _list = string.Join(",", list.ConvertAll(i => i.ToString()).ToArray());
        CrazySDK.Data.SetString(key, _list);
    }

    private void LoadList(string key, List<int> list)
    {
        string intListString = CrazySDK.Data.GetString(key);

        if (!string.IsNullOrEmpty(intListString))
        {
            string[] stringArray = intListString.Split(',');
            list.Clear();
            foreach (string str in stringArray)
            {
                int value;
                if (int.TryParse(str, out value))
                {
                    list.Add(value);
                }
            }
        }
    }


    public async Task DownloadImageAsync(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = SpriteFromTexture2D(texture);
                UI_Controller.instance.userImg.sprite = sprite;
            }
        }
    }

    private Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    
    public void RevivePlayer()
    {
        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, ()=>
        {
            Time.timeScale = 0;
            UI_Controller.instance.Block.SetActive(true);
        }, (error) =>
        {
            Time.timeScale = 1;
            UI_Controller.instance.FeedBackPopUp("Someting went wrong", UI_Controller.FeedbackType.failed);
            UI_Controller.instance.Back_Main();
            //ad error
        }, () =>
        {
            //ad finished
            //player Revive
            Time.timeScale = 1;
            Reset_Ships(true);
            UI_Controller.instance.Block.SetActive(false);
            UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(true);
            UI_Controller.instance.Win_Tigger(0);            
            WatchedRewardAd = true;
            Revived = true;
        }
        );
    }

    public void WatchMidGameAd()
    {
        if (!WatchedRewardAd)
        {
            CrazySDK.Ad.RequestAd(CrazyAdType.Midgame, null, null, null);
        }        
    }

    public void OpenSheild()
    {
        SheildObj.SetActive(true);
        Animator anim = SheildObj.GetComponent<Animator>();
        anim.SetTrigger("open");
        UncheckPowerUps();
        current_Usage_Sheild++;
        Sheild_count -= 1;
    }

    public void Freez()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            GameObject iceCube = Instantiate(FreezObj, bullets[i].transform.position, bullets[i].transform.rotation);            
            bullets[i].transform.SetParent(iceCube.transform);

            SpriteRenderer sr = bullets[i].GetComponent<SpriteRenderer>();
            sr.sortingOrder = 0;


            Rigidbody2D bulletRb = bullets[i].GetComponent<Rigidbody2D>();
            bulletRb.velocity *= 0.2f;

            Collider2D col = bullets[i].GetComponent<Collider2D>();
            col.isTrigger = true;

            Rigidbody2D iceRb = iceCube.GetComponent<Rigidbody2D>();
            iceRb.velocity = bulletRb.velocity;            
            bulletRb.isKinematic = true;            
            bullets[i].tag = "Finish";
            
            ParticleSystem particleSystem = bullets[i].GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
        UncheckPowerUps();
        current_Usage_freeze++;
        Freez_count--;
    }

    public void TinyShots()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            Vector3 scale = bullets[i].transform.localScale;
            bullets[i].transform.DOScale(new Vector3(scale.x / 2, scale.y / 2, scale.z), 0.2f);

            Bullet bullet = bullets[i].GetComponent<Bullet>();
            bullet.Damage /= 2;

            ParticleSystem particleSystem = bullets[i].GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                Vector3 scalePart = particleSystem.gameObject.transform.localScale;
                particleSystem.gameObject.transform.DOScale(new Vector3(scalePart.x / 2, scalePart.y / 2, scalePart.z), 0.2f);
            }
        }
        UncheckPowerUps();
        current_Usage_TinyShots++;
        TinyShots_count--;
    }

    public void TinyShip()
    {
        UncheckPowerUps();
        current_Usage_TinyShip++;
        TinyShip_count--;
    }

    public enum GamePhase
    {
        MainMenu,
        Start,
        ReadyPhase,
        PlayerShootPhase,
        EnemyReadyPhase,
        EnemyShootPhase,
        GameOver,
    }
}

[System.Serializable]
public class UserInfo
{
    public string Token { get; set; }
    public PortalUser User { get; set; }
}