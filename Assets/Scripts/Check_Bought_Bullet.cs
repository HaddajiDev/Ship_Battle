using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Check_Bought_Bullet : MonoBehaviour
{
    Button _Mybutton;
    public int index;
    public TMPro.TMP_Text Coins_Text;
    public TMPro.TMP_Text Diamond_Text;

    public GameObject CostObject;
    public GameObject OwnedObject;

    public TMPro.TMP_Text BulletName;
    public Image BulletSprite;

    public GameObject infos;

    void Start()
    {
        _Mybutton = GetComponent<Button>();
        _Mybutton.onClick.AddListener(() => Shop.Instance.Buy_Bullet(index));
        Bullets bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(index);

        BulletName.text = bullet.Name;
        BulletSprite.sprite = bullet.sr;

        infos.GetComponentInChildren<TMPro.TMP_Text>().text = $"Damage : {bullet.Damage} \n Usage per game : {bullet.Limit}";

        if (GameManager.Instance.shop.bullets.data.Contains(index))
        {
            _Mybutton.interactable = false;
            OwnedObject.SetActive(true);
            CostObject.SetActive(false);
        }
        else
        {
            CostObject.SetActive(true);            
            Coins_Text.text = bullet.cost.Coins.ToString();
            Diamond_Text.text = bullet.cost.Diamond.ToString();
        }
    }

    private bool isFlipped = false;

    public void flip(Transform img)
    {
        float targetRotation = isFlipped ? 0f : 180f;
        img.DORotate(new Vector3(0, 0, targetRotation), 0.3f);
        isFlipped = !isFlipped;
    }

    public void setInfo()
    {
        if (infos.activeInHierarchy)
            infos.SetActive(false);
        else
            infos.SetActive(true);
    }

    private void OnMouseOver()
    {
        
    }
}
