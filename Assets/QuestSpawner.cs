using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSpawner : MonoBehaviour
{
    public static QuestSpawner instance;    
    public GameObject QuestPrefab;
    public RectTransform container;
    public List<QuestBanner> questsBanners;

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
    }
    
    void Update()
    {
        
    }
}
