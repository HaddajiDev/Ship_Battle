using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    [Header("UI Routes")]
    public CanvasGroup Getting_Ready_Object;
    public CanvasGroup Main_Menu;
    public CanvasGroup Extra_Slot_Buy_UI;
    public CanvasGroup Menu_BG; //Start Game
    public CanvasGroup Ready_Button;
    public CanvasGroup Bullets_Shop;
    public CanvasGroup Select_Bullets;
    public CanvasGroup Win_Obj;
    public CanvasGroup Buy_Upgrades;
    public CanvasGroup Buy_Skins;

    [Header("Select Bullet UI")]
    public Button Select_Bullet_1;
    public Button Select_Bullet_2;
    public Button Select_Bullet_Extra;
    public GameObject[] Checks;

    [Header("Abilites")]
    public Sprite Check;
    public GameObject Fire_Object;
    public GameObject Burst_Object;

    [Header("Texts")]
    public TMP_Text Coins_Main_Text;
    public TMP_Text Diamond_Text;
    public TMP_Text Coins_text;
    public TMP_Text State_Win;
    public TMP_Text Fire_Shot_Price;
    public TMP_Text Busrt_Shot_Price;
    public TMP_Text Level_Counter;
    public TMP_Text Bullet_Limit_1;
    public TMP_Text Bullet_Limit_2;
    public TMP_Text Bullet_Limit_Extra;
    public TMP_Text Total_Fire;
    public TMP_Text Total_Busrt;
    public TMP_Text Health_Cost_Upgrade_Coins;
    public TMP_Text Health_Cost_Upgrade_Diamond;
    public TMP_Text Force_Cost_Upgrade_Coins;    
    public TMP_Text Force_Cost_Upgrade__Diamond;

    [Header("Bullets Slots")]
    public Image bullet_slot_1;
    public Image bullet_slot_2;
    public Image bullet_slot_Extra;
    public GameObject Extra_Bullet_Buy_Button;
    public Button Shop_Buy_Extra_Bullet;
    public Button Clear_Button_Bullet;

    [Header("Account Stuff")]
    public CanvasGroup account;
    public Image userImg;
    public TMP_Text username;
    public GameObject SignInButton;
    public GameObject AccountButton;

    [Header("Skins Stuff")]
    public Transform skins_Container;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Fire_Shot_Price.text = GameManager.Instance.Fire_Cost.ToString();
        Busrt_Shot_Price.text = GameManager.Instance.Burst_Cost.ToString();

        SetAbilitesCount();
        SetCurrencyUI();
    }
    public void Check_Fire()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        Image my_img = null;

        if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
        {
            Button clickedButton = selectedObject.GetComponent<Button>();
            my_img = clickedButton.transform.GetChild(0).GetComponentInChildren<Image>();

            if(GameManager.Instance.Fire_Uses == 0)
            {
                clickedButton.interactable = false;
                return;
            }
        }

        //Fire
        if (my_img.sprite == Check)
        {
            my_img.sprite = null;
            my_img.enabled = false;
            GameManager.Instance.player_1.inFire = false;
        }            
        else if (my_img.sprite == null)
        {
            my_img.sprite = Check;
            my_img.enabled = true;
            GameManager.Instance.player_1.inFire = true;
        }
    }
    public void Check_Busrt()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        Image my_img = null;

        if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
        {
            Button clickedButton = selectedObject.GetComponent<Button>();
            my_img = clickedButton.transform.GetChild(0).GetComponentInChildren<Image>();

            if (GameManager.Instance.Burst_Uses == 0)
            {
                clickedButton.interactable = false;
                return;
            }
        }

        //burst
        if (my_img.sprite == Check)
        {
            my_img.sprite = null;
            my_img.enabled = false;
            GameManager.Instance.player_1.burstCount = 1;
        }
        else if (my_img.sprite == null)
        {
            my_img.sprite = Check;
            my_img.enabled = true;
            GameManager.Instance.player_1.burstCount = 3;
        }
    }
    public void unCheckAbilities()
    {
        Image fire_img = Fire_Object.transform.GetChild(0).GetComponentInChildren<Image>();
        fire_img.sprite = null;
        fire_img.enabled = false;
        GameManager.Instance.player_1.inFire = false;

        Image burst_img = Burst_Object.transform.GetChild(0).GetComponentInChildren<Image>();
        burst_img.sprite = null;
        burst_img.enabled = false;
        GameManager.Instance.player_1.burstCount = 1;
    }
    public void Win_Tigger(int value, string state = "")
    {
        if(value == 1)
        {
            Win_Obj.interactable = true;
            Win_Obj.blocksRaycasts = true;
            Win_Obj.DOFade(1, 0.3f);
            State_Win.text = state;
            Getting_Ready_Object.DOFade(0, 0.1f);
            Getting_Ready_Object.interactable = false;
            Getting_Ready_Object.blocksRaycasts = false;
        }
        else
        {            
            Win_Obj.DOFade(0, 0.3f).OnComplete(() => {
                Win_Obj.interactable = false;
                Win_Obj.blocksRaycasts = false;
            });
        }        
    }

    public void Back_Main()
    {
        Win_Tigger(0);
        GameManager.Instance.cam.Follow = null;
        GameManager.Instance.cam.gameObject.SetActive(false);
        GameManager.Instance.cam_.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        GameManager.Instance.cam_.GetComponent<CameraFollow>().SetTarget(null);
        GameManager.Instance.cam_.transform.DOMove(GameManager.Instance.Main_Point.localPosition, 3).OnComplete(() => {
            Main_Menu.DOFade(1, 0.3f);
            Main_Menu.interactable = true;
            Main_Menu.blocksRaycasts = true;
        });
    }

    public void Bullets_Shop_Controller(int index)
    {
        if(index == 1)
        {
            Bullets_Shop.interactable = true;
            Bullets_Shop.blocksRaycasts = true;
            Bullets_Shop.DOFade(1, 0.3f);
        }
        else
        {
            Bullets_Shop.DOFade(0, 0.3f).OnComplete(() => {
                Bullets_Shop.interactable = false;
                Bullets_Shop.blocksRaycasts = false;
            });
        }
    }

    public void Select_Bullet_Controller(int index)
    {
        if (index == 1)
        {
            Select_Bullets.interactable = true;
            Select_Bullets.blocksRaycasts = true;
            Select_Bullets.DOFade(1, 0.3f);
        }
        else
        {
            Select_Bullets.DOFade(0, 0.3f).OnComplete(() => {
                Select_Bullets.interactable = false;
                Select_Bullets.blocksRaycasts = false;
            });
        }
    }

    public void Buy_Extra_Bullet_Slot(int index)
    {
        if (index == 1)
        {
            Extra_Slot_Buy_UI.interactable = true;
            Extra_Slot_Buy_UI.blocksRaycasts = true;
            Extra_Slot_Buy_UI.DOFade(1, 0.3f);
        }
        else
        {
            Extra_Slot_Buy_UI.DOFade(0, 0.3f).OnComplete(() => {
                Extra_Slot_Buy_UI.interactable = false;
                Extra_Slot_Buy_UI.blocksRaycasts = false;
            });
        }        
    }

    public void Buy_Upgrades_Controller(int index)
    {
        if (index == 1)
        {
            Buy_Upgrades.interactable = true;
            Buy_Upgrades.blocksRaycasts = true;
            Buy_Upgrades.DOFade(1, 0.3f);
        }
        else
        {
            Buy_Upgrades.DOFade(0, 0.3f).OnComplete(() => {
                Buy_Upgrades.interactable = false;
                Buy_Upgrades.blocksRaycasts = false;
            });
        }
    }
    public void Open_Account(int index)
    {
        if (index == 1)
        {
            account.interactable = true;
            account.blocksRaycasts = true;
            account.DOFade(1, 0.3f);
        }
        else
        {
            account.DOFade(0, 0.3f).OnComplete(() => {
                account.interactable = false;
                account.blocksRaycasts = false;
            });
        }
    }

    public void Buy_Skins_Controller(int index)
    {
        if (index == 1)
        {
            Buy_Skins.interactable = true;
            Buy_Skins.blocksRaycasts = true;
            Buy_Skins.DOFade(1, 0.3f);
        }
        else
        {
            Buy_Skins.DOFade(0, 0.3f).OnComplete(() => {
                Buy_Skins.interactable = false;
                Buy_Skins.blocksRaycasts = false;
            });
        }
    }

    public void Close_Start_Menu()
    {
        Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            Menu_BG.interactable = false;
            Menu_BG.blocksRaycasts = false;
        });
    }

    public void ResetChecks()
    {
        for (int i = 0; i < Checks.Length; i++)
        {
            Checks[i].SetActive(false);
        }
    }

    public void SetCurrencyUI()
    {
        Coins_Main_Text.text = GameManager.Instance.Coins.ToString();
        Diamond_Text.text = GameManager.Instance.Diamond.ToString();
    }
    public void SetAbilitesCount()
    {
        Total_Fire.text = GameManager.Instance.Fire_Uses.ToString();
        Total_Busrt.text = GameManager.Instance.Burst_Uses.ToString();
    }

    public void ExitSkin()
    {
        foreach(Transform child in skins_Container)
        {
            if (child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(false);
        }
    }
}
