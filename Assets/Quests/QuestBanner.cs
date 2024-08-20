using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class QuestBanner : MonoBehaviour
{
    public int index;
    public QuestData questData;

    public TMP_Text Descrption;
    public TMP_Text diammonds;
    public TMP_Text coins;
    public TMP_Text Counter;

    public Button ClaimButton;

    public Slider slider;


    void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        Quest quest = questData.Get_Quest(index);
        slider.maxValue = quest.Goal_Value;
        if (quest.type == Quest.Type.wins)
        {
            slider.value = GameManager.Instance.WinCount;
            Counter.text = $"{GameManager.Instance.WinCount} / {quest.Goal_Value}";
            if (GameManager.Instance.WinCount >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
            }                
        }
        else if (quest.type == Quest.Type.fireShots)
        {
            slider.value = GameManager.Instance.FireShots;
            Counter.text = $"{GameManager.Instance.FireShots} / {quest.Goal_Value}";
            if (GameManager.Instance.FireShots >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
            }
        }
        else if (quest.type == Quest.Type.noMissShots)
        {
            slider.value = GameManager.Instance.noMissShots;
            Counter.text = $"{GameManager.Instance.noMissShots} / {quest.Goal_Value}";
            if (GameManager.Instance.noMissShots >= quest.Goal_Value)
            {
                ClaimButton.onClick.RemoveAllListeners();
                ClaimButton.onClick.AddListener(() => ClaimReward(quest.cost));
                ClaimButton.interactable = true;
            }
        }
        Descrption.text = quest.Descrption;
        diammonds.text = quest.cost.Diamond.ToString();
        coins.text = quest.cost.Coins.ToString();
    }


    private void ClaimReward(Cost cost)
    {
        ClaimButton.interactable = false;
        GameManager.Instance.Coins += cost.Coins;
        GameManager.Instance.Diamond += cost.Diamond;
        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
        GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);

        GameManager.Instance.currentQuests.Remove(index);
        GameManager.Instance.SetList("current_Quests", GameManager.Instance.currentQuests);
        gameObject.transform.DOScale(1.1f, 0.2f).OnComplete(() =>
        {
            gameObject.transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
        if (GameManager.Instance.currentQuests.Count == 0)
        {
            QuestSpawner.instance.NoQuests.SetActive(true);
        }
    }
    
}
