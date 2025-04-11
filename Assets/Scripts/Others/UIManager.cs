using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI waveText;

    [SerializeField][HideInInspector] private PlayerCombat playerHealth;
    [SerializeField][HideInInspector] private WaveManager waveManager;

    void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        if (playerHealth != null)
            healthText.text = "Health: " + playerHealth.currentHealth;

        if (waveManager != null)
            waveText.text = "Wave: " + waveManager.currentWave;
    }
}