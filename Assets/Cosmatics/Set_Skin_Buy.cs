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
                //select
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
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
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
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
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
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
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
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
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
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
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {                
                Coins.text = anchorSkin.cost.Coins.ToString();
                Diammond.text = anchorSkin.cost.Diamond.ToString();                
            }
            image.sprite = anchorSkin.Bottom;
        }
    }

    public void BuySkin()
    {
        if (part == Part.ship)
        {
            Shop.Instance.BuyShip_Skin(index);
        }
        else if (part == Part.sail)
        {
            Shop.Instance.BuySail_Skin(index);
        }
        else if (part == Part.flag)
        {
            Shop.Instance.BuyFlag_Skin(index);
        }
        else if (part == Part.helm)
        {
            Shop.Instance.BuyHelm_Skin(index);
        }
        else if (part == Part.cannon)
        {
            Shop.Instance.BuyCannon_Skin(index);
        }
        else if (part == Part.anchor)
        {
            Shop.Instance.BuyAnchor_Skin(index);
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
