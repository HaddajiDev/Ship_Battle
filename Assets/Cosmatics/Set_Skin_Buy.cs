using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
                Coins.text = shipSkin.cost.Coins.ToString();
                Diammond.text = shipSkin.cost.Diamond.ToString();
            }
            image.sprite = shipSkin.Cover;
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
                Coins.text = sailSkin.cost.Coins.ToString();
                Diammond.text = sailSkin.cost.Diamond.ToString();
            }
            image.sprite = sailSkin.Cover;
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
                Coins.text = flagSkin.cost.Coins.ToString();
                Diammond.text = flagSkin.cost.Diamond.ToString();
            }
            image.sprite = flagSkin.Cover;
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
                Coins.text = helmSkin.cost.Coins.ToString();
                Diammond.text = helmSkin.cost.Diamond.ToString();
            }
            image.sprite = helmSkin.Cover;
        }
        else if (part == Part.cannon)
        {
            Cosmatic cannonSkin = CannonCosmatic.Get_Skin(index);
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
                Coins.text = cannonSkin.cost.Coins.ToString();
                Diammond.text = cannonSkin.cost.Diamond.ToString();
            }
            image.sprite = cannonSkin.Cover;
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
                BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                Coins.text = anchorSkin.cost.Coins.ToString();
                Diammond.text = anchorSkin.cost.Diamond.ToString();                
            }
            image.sprite = anchorSkin.Bottom;
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
