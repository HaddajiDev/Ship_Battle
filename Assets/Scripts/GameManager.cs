using Cinemachine;
using CrazyGames;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections.Generic;

///<summary>
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
    [SerializeField] private Transform End_Point;
    public Transform Main_Point;

    [HideInInspector] public bool isChecking = false;

    [Header("Abilities usage")]
    public int Fire_Uses;
    public int Burst_Uses;

    [Header("Currency")]
    public int Coins = 50000;
    public int Diamond = 1000;
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

    [HideInInspector]
    public string username;
    public string imgURL;
    public string token;

    bool isAvailable;

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
    }

    public void Play()
    {
        UI_Controller.instance.Menu_BG.interactable = true;
        UI_Controller.instance.Menu_BG.blocksRaycasts = true;
        UI_Controller.instance.Menu_BG.DOFade(1, 0.5f);
        UI_Controller.instance.Level_Counter.text = Current_Level.ToString();
    }

    public void Start_Game()
    {
        //ui controller
        UI_Controller.instance.Main_Menu.DOFade(0, 0.3f);
        UI_Controller.instance.Main_Menu.interactable = false;
        UI_Controller.instance.Main_Menu.blocksRaycasts = false;

        //get abilites if exited
        Check_Abilites();
        UI_Controller.instance.Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            cam_.GetComponent<CameraFollow>().SetTarget(null);
            cam_.transform.DOMove(End_Point.localPosition, 5).OnComplete(() =>
            {
                Get_Ready_UI(1);
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
        Reset_Ships();


        player_2.Get_Stats(Current_Level);

        turn = 2;

        Set_Player();

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
            //cam_.GetComponent<CameraFollow>().SetTarget(player_1.transform.parent.transform);
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
            //cam_.GetComponent<CameraFollow>().SetTarget(player_2.transform.GetChild(1).GetChild(1).transform);
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
        if (Burst_Uses == 0)
            UI_Controller.instance.Burst_Object.SetActive(false);
    }
    public void Get_Fire()
    {
        if (Coins >= Fire_Cost)
        {
            Fire_Uses++;
            Coins -= Fire_Cost;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
        }
    }    public void Dump_Fire()
    {
        if (Fire_Uses != 0)
        {
            Fire_Uses--;
            Coins += Fire_Cost;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
        }
    }
    public void Get_Burst()
    {
        if (Coins >= Burst_Cost)
        {
            Burst_Uses++;
            Coins -= Burst_Cost;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
        }
    }
    public void Dump_Burst()
    {
        if (Burst_Uses != 0)
        {
            Burst_Uses--;
            Coins += Burst_Cost;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
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

        //|---------------------------------------|//
        //|                                       |//
        //|                                       |//
        //|                                       |//
        //|       Remove fire if existed          |//
        //|                                       |//
        //|                                       |//
        //|                                       |//
        //|---------------------------------------|//
    }

    public void Set_Player()
    {
        if (UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);

        if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
        if (Shop.Instance.Got_Extra_Slot == 1)
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
        CrazySDK.Data.SetString("bullets", bulletList);        
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

        CrazySDK.Data.SetString("username", "");
        CrazySDK.Data.SetString("imgUrl", "");
        CrazySDK.Data.SetString("token", "");

        //Player Bullets
        string bulletList = string.Join(",", shop.bullets.data.ConvertAll(i => i.ToString()).ToArray());
        CrazySDK.Data.SetString("bullets", bulletList);
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

        username = CrazySDK.Data.GetString("username");
        imgURL = CrazySDK.Data.GetString("imgUrl");
        token = CrazySDK.Data.GetString("token");

        //Player Bullets
        string intListString = CrazySDK.Data.GetString("bullets");
        
        if (!string.IsNullOrEmpty(intListString))
        {            
            string[] stringArray = intListString.Split(',');            
            shop.bullets.data = new List<int>();
            
            foreach (string str in stringArray)
            {
                int value;
                if (int.TryParse(str, out value))
                {                    
                    shop.bullets.data.Add(value);
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
}

[System.Serializable]
public class UserInfo
{
    public string Token { get; set; }
    public PortalUser User { get; set; }
}