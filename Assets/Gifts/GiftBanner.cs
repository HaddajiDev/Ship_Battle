using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CrazyGames;
using UnityEngine.UI;
using DG.Tweening;

public class GiftBanner : MonoBehaviour
{
    public int index;
    public GiftsData giftsData;
    public GameObject DimmondObject;

    public TMP_Text dimmond;
    public TMP_Text coins;

    public Button FreeButton;
    public Button AdButton;
    
    void Start()
    {
        Gifts gift = giftsData.GetGift(index);
        if (gift.prize.Diamond != 0)
        {
            DimmondObject.SetActive(true);
            dimmond.text = gift.prize.Diamond.ToString();
        }
            

        if (!gift.ad)
            AdButton.gameObject.SetActive(false);
        else
            FreeButton.gameObject.SetActive(false);

        coins.text = gift.prize.Coins.ToString();

    }

    public void GetForFree()
    {
        FreeButton.interactable = false;
        Gifts gift = giftsData.GetGift(index);

        GameManager.Instance.Coins += gift.prize.Coins;
        GameManager.Instance.Diamond += gift.prize.Diamond;
        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
        GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);

        GameManager.Instance.currentGifts.Remove(index);
        GameManager.Instance.SetList("current_Gifts", GameManager.Instance.currentGifts);

        TMPro.TMP_Text count = UI_Controller.instance.GiftsNotification.GetComponentInChildren<TMPro.TMP_Text>();
        count.text = $"{GameManager.Instance.currentGifts.Count}";

        gameObject.transform.DOScale(1.1f, 0.2f).OnComplete(() =>
        {
            gameObject.transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
        if (GameManager.Instance.currentGifts.Count == 0)
        {
            GiftsSpawner.instance.noGifts.SetActive(true);
            UI_Controller.instance.GiftsNotification.SetActive(false);
        }
    }

    public void GetForAd()
    {
        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        {
            Time.timeScale = 0;
            UI_Controller.instance.Block.SetActive(true);
            GameManager.Instance.MusicSource.Pause();
            GameManager.Instance.OceanBackGround.Pause();
        }, (error) =>
        {
            Time.timeScale = 1;
            UI_Controller.instance.FeedBackPopUp("Someting went wrong, try again later", UI_Controller.FeedbackType.failed);
            GameManager.Instance.MusicSource.Play();
            GameManager.Instance.OceanBackGround.Play();
            //ad error
        }, () =>
        {
            //ad finished            
            Time.timeScale = 1;
            GameManager.Instance.MusicSource.Play();
            GameManager.Instance.OceanBackGround.Play();
            UI_Controller.instance.FeedBackPopUp("Congratulations!! You've earned a special reward", UI_Controller.FeedbackType.succes);
            GetForFree();
        }
);
    }
}
