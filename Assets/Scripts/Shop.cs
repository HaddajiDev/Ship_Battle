using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;
    Player_Bullets bullets = new Player_Bullets();

    public Transform Container;
    public GameObject Bullet_UI_Prefab;

    public int Got_Extra_Slot = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Buy_Bullet(int index)
    {        
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        if(GameManager.Instance.Coins >= bullet.Price)
        {
            GameManager.Instance.Coins -= bullet.Price;
            bullets.Add_Bullet(index);
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            Button clickedButton = selectedObject.GetComponent<Button>();
            clickedButton.interactable = false;
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
                    Button select = Container.GetChild(i + 1).transform.GetChild(2).GetComponent<Button>();
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
        prefab.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.player_1.bulletsData.Get_Bullet(j).sr;
        prefab.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = GameManager.Instance.player_1.bulletsData.Get_Bullet(j).Name;
        prefab.transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>().text = "Select";        
    }

    public void Select_Bullet_From_list_1(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_1.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
    }
    public void Select_Bullet_From_list_2(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_2.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
    }
    public void Select_Bullet_From_list_Extra(int index)
    {
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);
        UI_Controller.instance.bullet_slot_Extra.sprite = bullet.sr;
        UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index = index;
        UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet = bullet;
        CheckForSelctedBullets();
    }


    void CheckForSelctedBullets()
    {
        for (int i = 1; i < Container.childCount; i++)
        {
            GameObject Bullet = Container.GetChild(i).gameObject;
            string Name = Bullet.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text;
            
            if(Name == UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet.Name || Name == UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet.Name || Name == UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet.Name)
            {
                Bullet.transform.GetChild(2).GetComponent<Button>().interactable = false;
                Bullet.transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>().text = "Selected";
            }
            else
            {
                Bullet.transform.GetChild(2).GetComponent<Button>().interactable = true;
                Bullet.transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>().text = "Select";
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

public class Cost
{
    public int Coins { get; set; }
    public int Diamond { get; set; }    
    public Cost(int coins, int diamond = 0)
    {
        Coins = coins;
        Diamond = diamond;
    }
}
