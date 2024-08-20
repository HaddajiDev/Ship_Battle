using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CrazyGames;

public class Set_Skin_Buy : MonoBehaviour
{   
    public int index;
    public Part part = new Part();

    
    public ShipCosmaticData shipCosmatic;    
    public CanonSkins CannonCosmatic;    
    public SailCosmaticData sailCosmatic;    
    public HelmCosmaticData helmCosmatic;    
    public FlagCosmaticData flagCosmatic;    
    public AnchorCosmaticData anchorCosmatic;

    
    public TMP_Text Coins;    
    public TMP_Text Diammond;

    public Image image;
    public GameObject BuyButton;

    public GameObject CostObject;
    public GameObject AdObject;

    private void Start()
    {        
        if(part == Part.ship)
        {
            ShipCosmatic shipSkin = shipCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Ships_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinShip(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedShip == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (shipSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = shipSkin.cost.Coins.ToString();
                    Diammond.text = shipSkin.cost.Diamond.ToString();
                }
            }
            image.sprite = shipSkin.Cover;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = shipSkin.spriteSheet;            
        }
        else if(part == Part.sail)
        {
            Cosmatic sailSkin = sailCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Sail_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinSail(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedSail == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (sailSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = sailSkin.cost.Coins.ToString();
                    Diammond.text = sailSkin.cost.Diamond.ToString();
                }
            }
            image.sprite = sailSkin.Cover;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = sailSkin.spriteSheet;            
        }
        else if (part == Part.flag)
        {
            Cosmatic flagSkin = flagCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Flag_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinFlag(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedFlag == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (flagSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = flagSkin.cost.Coins.ToString();
                    Diammond.text = flagSkin.cost.Diamond.ToString();
                }
            }
            image.sprite = flagSkin.Cover;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = flagSkin.spriteSheet;            
        }
        else if (part == Part.helm)
        {
            Cosmatic helmSkin = helmCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Helm_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinHelm(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedHelm == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (helmSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = helmSkin.cost.Coins.ToString();
                    Diammond.text = helmSkin.cost.Diamond.ToString();
                }
            }
            image.sprite = helmSkin.Cover;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.enabled = false;
        }
        else if (part == Part.cannon)
        {
            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Cannon_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinCannon(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedCannon == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (cannonSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = cannonSkin.cost.Coins.ToString();
                    Diammond.text = cannonSkin.cost.Diamond.ToString();
                }
            }
            image.sprite = cannonSkin.Cover;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.enabled = false;
        }
        else if(part == Part.anchor)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(index);            

            if (Shop.Instance.skins.Anchors_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinAnchor(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if(GameManager.Instance.player_1._selectedAnchor == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }                
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (anchorSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => WatchAd());
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = anchorSkin.cost.Coins.ToString();
                    Diammond.text = anchorSkin.cost.Diamond.ToString();
                }                
            }
            image.sprite = anchorSkin.Bottom;
            UI_Animator _animator = image.gameObject.GetComponent<UI_Animator>();
            _animator.enabled = false;
        }
    }

    private void BuySkin()
    {
        if (part == Part.ship)
        {
            Shop.Instance.BuyShip_Skin(index);
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinShip(index);
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinShip(index));
        }
        else if (part == Part.sail)
        {
            Shop.Instance.BuySail_Skin(index);
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinSail(index);
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinSail(index));
        }
        else if (part == Part.flag)
        {
            Shop.Instance.BuyFlag_Skin(index);
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinFlag(index);
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinFlag(index));
        }
        else if (part == Part.helm)
        {
            Shop.Instance.BuyHelm_Skin(index);
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinHelm(index);
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinHelm(index));
        }
        else if (part == Part.cannon)
        {
            Shop.Instance.BuyCannon_Skin(index);
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinCannon(index);
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinCannon(index));
        }
        else if (part == Part.anchor)
        {            
            Shop.Instance.BuyAnchor_Skin(index);            
            BuyButton.transform.GetChild(0).gameObject.SetActive(true);
            SelectSkinAnchor(index);            
            BuyButton.transform.GetChild(1).gameObject.SetActive(false);            
            BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinAnchor(index));
        }
    }
    public void WatchAd()
    {        
        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        {
            Time.timeScale = 0;
            UI_Controller.instance.Block.SetActive(true);
            //mute audio
            //ad Started
        }, (error) =>
        {
            Time.timeScale = 1;
            UI_Controller.instance.Block.SetActive(false);
            UI_Controller.instance.Show_AdFeedback_Controller(1);
            UI_Controller.instance.feed_sprite.sprite = UI_Controller.instance.failure;
            UI_Controller.instance.feed_sprite.color = Color.red;
            UI_Controller.instance.feed_text.text = "Someting went wrong try again later";
            //unmute audio
            //ad Error
        }, () =>
        {
            //unmute audio
            Time.timeScale = 1;
            UI_Controller.instance.Block.SetActive(false);
            UI_Controller.instance.Show_AdFeedback_Controller(1);
            UI_Controller.instance.feed_sprite.sprite = UI_Controller.instance.succes;
            UI_Controller.instance.feed_sprite.color = Color.green;
            UI_Controller.instance.feed_text.text = "Congratulations!! You've unlocked a new skin for your ship";
            CrazySDK.Game.HappyTime();
            if (part == Part.ship)
            {
                GetSkinFromAd(Shop.Instance.skins.Ships_Skins, "ship_skins");
                SelectSkinShip(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinShip(index));
            }
            else if (part == Part.sail)
            {
                GetSkinFromAd(Shop.Instance.skins.Sail_Skins, "sail_skins");
                SelectSkinSail(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinSail(index));
            }
            else if (part == Part.flag)
            {
                GetSkinFromAd(Shop.Instance.skins.Flag_Skins, "flag_skins");
                SelectSkinFlag(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinFlag(index));
            }
            else if (part == Part.helm)
            {
                GetSkinFromAd(Shop.Instance.skins.Cannon_Skins, "helm_skins");
                SelectSkinHelm(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinHelm(index));
            }
            else if (part == Part.cannon)
            {
                GetSkinFromAd(Shop.Instance.skins.Cannon_Skins, "cannon_skins");
                SelectSkinCannon(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinCannon(index));
            }
            else if (part == Part.anchor)
            {
                GetSkinFromAd(Shop.Instance.skins.Anchors_Skins, "anchor_skins");
                SelectSkinAnchor(index);
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinAnchor(index));
            }
        });
    }

    private void GetSkinFromAd(List<int> skinList, string key)
    {
        Shop.Instance.skins.Add_Skin(skinList, index);
        GameManager.Instance.SaveData(key, skinList);
        BuyButton.transform.GetChild(0).gameObject.SetActive(true);        
        BuyButton.transform.GetChild(1).gameObject.SetActive(false);
        BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();        
        AdObject.SetActive(false);
    }
    
    private void Update()
    {
        if (UI_Controller.instance.Buy_Skins.gameObject.activeInHierarchy)
        {
            if (part == Part.ship)
            {
                CheckForSelected("select_skin_ship");                
            }
            else if (part == Part.sail)
            {
                CheckForSelected("select_skin_sail");                
            }
            else if (part == Part.flag)
            {
                CheckForSelected("select_skin_flag");                
            }
            else if (part == Part.helm)
            {
                CheckForSelected("select_skin_helm");
            }
            else if (part == Part.cannon)
            {
                CheckForSelected("select_skin_cannon");                
            }
            else if (part == Part.anchor)
            {
                CheckForSelected("select_skin_anchor");
            }            
        }        
    }


    private void CheckForSelected(string key)
    {
        if (index == CrazyGames.CrazySDK.Data.GetInt(key))
        {
            BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
            BuyButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
            BuyButton.GetComponent<Button>().interactable = true;
        }
    }

    private void SelectSkinAnchor(int index)
    {
        GameManager.Instance.player_1._selectedAnchor = index;
        GameManager.Instance.SaveData("select_skin_anchor", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }
    private void SelectSkinSail(int index)
    {
        GameManager.Instance.player_1._selectedSail = index;
        GameManager.Instance.SaveData("select_skin_sail", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }
    private void SelectSkinFlag(int index)
    {
        GameManager.Instance.player_1._selectedFlag = index;
        GameManager.Instance.SaveData("select_skin_flag", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }
    private void SelectSkinCannon(int index)
    {
        GameManager.Instance.player_1._selectedCannon = index;
        GameManager.Instance.SaveData("select_skin_cannon", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }
    private void SelectSkinHelm(int index)
    {
        GameManager.Instance.player_1._selectedHelm = index;
        GameManager.Instance.SaveData("select_skin_helm", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }
    private void SelectSkinShip(int index)
    {
        GameManager.Instance.player_1._selectedShip = index;
        GameManager.Instance.SaveData("select_skin_ship", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
    }

    public void Show_Skin()
    {
        if (part == Part.ship)
        {
            ShipCosmatic shipSkin = shipCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image ship = show.transform.Find("BG/ship").GetComponent<Image>();

            ship.sprite = shipSkin.Cover;
            UI_Animator _animator = ship.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = shipSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.sail)
        {
            Cosmatic sailSkin = sailCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image sail = show.transform.Find("BG/sail").GetComponent<Image>();

            sail.sprite = sailSkin.Cover;
            UI_Animator _animator = sail.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = sailSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.flag)
        {
            Cosmatic flagSkin = flagCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);

            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image flag = show.transform.Find("BG/flag").GetComponent<Image>();

            flag.sprite = flagSkin.Cover;
            UI_Animator _animator = flag.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = flagSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.helm)
        {
            Cosmatic helmSkin = helmCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image helm = show.transform.Find("BG/helm").GetComponent<Image>();

            helm.sprite = helmSkin.Cover;
            
        }
        else if (part == Part.cannon)
        {
            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image cannon_Top = show.transform.Find("BG/cannon/top").GetComponent<Image>();
            Image cannon_stand = show.transform.Find("BG/cannon/stand").GetComponent<Image>();

            cannon_Top.sprite = cannonSkin.Cover;
            cannon_stand.sprite = cannonSkin.Stand;
        }
        else if (part == Part.anchor)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image anchor_Top = show.transform.Find("BG/anchor/1").GetComponent<Image>();
            Image anchor_Bottom = show.transform.Find("BG/anchor/2").GetComponent<Image>();

            anchor_Top.sprite = anchorSkin.Top;
            anchor_Bottom.sprite = anchorSkin.Bottom;
        }
    }

    public enum Part
    {
        ship,
        sail,
        flag,
        helm,
        cannon,
        anchor
    }
}
