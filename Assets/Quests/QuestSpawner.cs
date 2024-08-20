using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSpawner : MonoBehaviour
{
    public static QuestSpawner instance;    
    public GameObject QuestPrefab;
    public RectTransform container;
    public List<QuestBanner> questsBanners;
    public GameObject NoQuests;

    public GameObject QuestPop;
    public Transform QuestPopContainer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < GameManager.Instance.currentQuests.Count; i++)
        {
            GameObject ob = Instantiate(QuestPrefab, container);
            QuestBanner quest = ob.GetComponent<QuestBanner>();
            quest.index = GameManager.Instance.currentQuests[i];
            questsBanners.Add(quest);
        }
        if(GameManager.Instance.currentQuests.Count == 0)
        {
            NoQuests.SetActive(true);
        }
    }
    
   public void SpawnQuestPopUp(QuestPopUp.Type type, int index)
    {
        GameObject ob = Instantiate(QuestPop, QuestPopContainer);
        QuestPopUp pop = ob.GetComponent<QuestPopUp>();
        pop.type = type;
        pop.index = index;
    }
}
