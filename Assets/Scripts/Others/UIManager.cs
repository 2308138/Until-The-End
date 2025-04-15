using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField][HideInInspector] private List<GameObject> heartImages = new List<GameObject>();
    [SerializeField][HideInInspector] private WaveManager waveManager;

    [Header("--- Health UI Settings ---")]
    [SerializeField] public GameObject heartPrefab;
    [SerializeField] public Transform healthContainer;

    [Header("--- Wave UI Settings ---")]
    [SerializeField] public Image waveNumberImage;
    [SerializeField] public Sprite tutorialSprite;
    [SerializeField] public List<Sprite> waveNumberSprites;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void SetupHealthDisplay(int maxHealth)
    {
        foreach (var heart in heartImages)
            Destroy(heart);

        heartImages.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthContainer);
            heartImages.Add(newHeart);
        }
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
            heartImages[i].SetActive(i < currentHealth);
    }

    public void UpdateWaveCounter()
    {
        if (waveManager.currentWave == 0)
            waveNumberImage.sprite = tutorialSprite;
        else if (waveManager.currentWave > 0 && waveManager.currentWave <= waveNumberSprites.Count)
            waveNumberImage.sprite = waveNumberSprites[waveManager.currentWave - 1];
    }
}