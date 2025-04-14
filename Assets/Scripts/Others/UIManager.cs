using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public GameObject heartPrefab;
    [SerializeField] public Transform heartContainer;

    [SerializeField][HideInInspector] private List<GameObject> heartImages = new List<GameObject>();
    [SerializeField][HideInInspector] private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        int currentHealth = playerCombat.currentHealth;
        int maxHealth = playerCombat.maxHealth;

        foreach (GameObject heart in heartImages)
            Destroy(heart);

        heartImages.Clear();

        for (int i = 0; i < playerCombat.currentHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartImages.Add(heart);
        }
    }
}