using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;
    [HideInInspector] public Player_Bullets bullets = new Player_Bullets();
    [HideInInspector] public Player_Skins skins = new Player_Skins();

    public Transform Container;
    public GameObject Bullet_UI_Prefab;

    public int Got_Extra_Slot = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(Got_Extra_Slot == 1)
        {
            UI_Controller.instance.bullet_slot_Extra.transform.parent.gameObject.SetActive(true);
            UI_Controller.instance.Extra_Bullet_Buy_Button.SetActive(false);
        }

        
    }

    public void Buy_Bullet(int index)
    {        
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        Cost cost = bullet.cost;
        if(GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            bullets.Add_Bullet(index);
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            Button clickedButton = selectedObject.GetComponent<Button>();
            clickedButton.interactable = false;
            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
            GameManager.Instance.SaveData("bullets", bullets.data);
        }        
    }

    public void Get_Player_Bullets()
    {
        if (Container.childCount == 1)
        {
            PopulateBulletUI();
        }
        else if (Container.childCount - 1 < bullets.data.Count)
        {
            AddNewBulletUI();
        }
        for (int i = 0; i < bullets.data.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.player_1.bulletsData.Get_Lenght; j++)
            {
                if (bullets.data[i] == j)
                {
                    Button select = Container.GetChild(i + 1).transform.GetChild(6).GetComponent<Button>();
                    var selected = j;
                    GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
                    Button clickedButton = selectedObject.GetComponent<Button>();
                    select.onClick.RemoveAllListeners();

                    //UI_Controller.instance.Clear_Button_Bullet.onClick.RemoveAllListeners();
                    if (clickedButton.name == "Bullet_1")
                    {
                        select.onClick.AddListener(() => Select_Bullet_From_list_1(selected));
                        //UI_Controller.instance.Clear_Button_Bullet.onClick.AddListener(() => Clear_Slot_1());
                    }
                    else if(clickedButton.name == "Bullet_2")
                    {
                        select.onClick.AddListener(() => Select_Bullet_From_list_2(selected));
                        //UI_Controller.instance.Clear_Button_Bullet.onClick.AddListener(() => Clear_Slot_2());
                    }
                    else
                    {
                        select.onClick.AddListener(() => Select_Bullet_From_list_Extra(selected));
                    }
                }
            }            
        }
        CheckForSelctedBullets();
    }

    private void PopulateBulletUI()
    {
        for (int i = 0; i < bullets.data.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.player_1.bulletsData.Get_Lenght; j++)
            {
                if (bullets.data[i] == j)
                {
                    InstantiateBulletUI(j);
                }
            }
        }
    }

    private void AddNewBulletUI()
    {
        for (int i = Container.childCount - 1; i < bullets.data.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.player_1.bulletsData.Get_Lenght; j++)
            {
                if (bullets.data[i] == j)
                {
                    InstantiateBulletUI(j);
                }
            }
        }
    }

    private void InstantiateBulletUI(int j)
    {
        GameObject prefab = Instantiate(Bullet_UI_Prefab, Container);
        prefab.transform.GetChild(4).GetComponent<Image>().sprite = GameManager.Instance.player_1.bulletsData.Get_Bullet(j).sr;
        prefab.transform.GetChild(5).GetComponent<TMPro.TMP_Text>().text = GameManager.Instance.player_1.bulletsData.Get_Bullet(j).Name;
        prefab.transform.GetChild(6).GetComponentInChildren<TMPro.TMP_Text>().text = "Select";        
    }

    public void Select_Bullet_From_list_1(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_1.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
        GameManager.Instance.SaveData("slot1", index);
    }
    public void Select_Bullet_From_list_2(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_2.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
        GameManager.Instance.SaveData("slot2", index);
    }
    public void Select_Bullet_From_list_Extra(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_Extra.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
        GameManager.Instance.SaveData("slotExtra", index);
    }


    public void CheckForSelctedBullets()
    {
        for (int i = 1; i < Container.childCount; i++)
        {
            GameObject Bullet = Container.GetChild(i).gameObject;
            string Name = Bullet.transform.GetChild(5).GetComponent<TMPro.TMP_Text>().text;
            
            if(Name == UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet.Name || Name == UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet.Name || Name == UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet.Name)
            {
                Bullet.transform.GetChild(6).GetComponent<Button>().interactable = false;
                Bullet.transform.GetChild(6).GetComponentInChildren<TMPro.TMP_Text>().text = "Selected";
            }
            else
            {
                Bullet.transform.GetChild(6).GetComponent<Button>().interactable = true;
                Bullet.transform.GetChild(6).GetComponentInChildren<TMPro.TMP_Text>().text = "Select";
            }
        }
    }

    public void Buy_Extra_Bullet()
    {
        Cost cost = new Cost(30000, 500);
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            UI_Controller.instance.bullet_slot_Extra.transform.parent.gameObject.SetActive(true);
            UI_Controller.instance.Extra_Bullet_Buy_Button.SetActive(false);

            UI_Controller.instance.Shop_Buy_Extra_Bullet.interactable = false;
            Got_Extra_Slot = 1;
            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
            GameManager.Instance.SaveData("extraSlot", 1);
        }
    }

    public void BuyShip_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.shipCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Ships_Skins, index);

            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("ship_skins", skins.Ships_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }
    public void BuySail_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.sailCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Sail_Skins, index);

            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("sail_skins", skins.Sail_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }
    public void BuyFlag_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.flagCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Flag_Skins, index);

            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("flag_skins", skins.Flag_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }
    public void BuyCannon_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.CannonCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Cannon_Skins, index);

            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("cannon_skins", skins.Cannon_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }
    public void BuyAnchor_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.anchorCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Anchors_Skins, index);
            
            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("anchor_skins", skins.Anchors_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }

    public void BuyHelm_Skin(int index)
    {
        Cost cost = GameManager.Instance.player_1.helmCosmatic.Get_Skin(index).cost;
        if (GameManager.Instance.Coins >= cost.Coins && GameManager.Instance.Diamond >= cost.Diamond)
        {
            GameManager.Instance.Coins -= cost.Coins;
            GameManager.Instance.Diamond -= cost.Diamond;
            skins.Add_Skin(skins.Helm_Skins, index);

            UI_Controller.instance.SetCurrencyUI();
            GameManager.Instance.SaveData("helm_skins", skins.Helm_Skins);
            GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
    }

    //public void Clear_Slot_1()
    //{
    //    //sprite null X or smth
    //    UI_Controller.instance.bullet_slot_1.sprite = null;
    //    UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet = null;
    //    UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index = 0;
    //    CheckForSelctedBullets();
    //}
    //public void Clear_Slot_2()
    //{
    //    //sprite null X or smth
    //    UI_Controller.instance.bullet_slot_2.sprite = null;
    //    UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet = null;
    //    UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index = 0;
    //    CheckForSelctedBullets();
    //}
}

[System.Serializable]
public class Cost
{
    public int Coins;
    public int Diamond;
    public Cost(int coins, int diamond = 0)
    {
        Coins = coins;
        Diamond = diamond;
    }
}
