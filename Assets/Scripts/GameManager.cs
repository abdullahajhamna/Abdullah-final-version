using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Coin System")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text specialCoinText;
    private int coinsCollected = 0;
    private int specialCoinsCollected = 0;
    
    [Header("Win Conditions")]
    [SerializeField] private int specialCoinsRequiredToWin = 3;

    [Header("Health System")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private int currentHealth;

    [Header("Audio System")]
    public AudioClip backgroundMusic;
    [Range(0, 1)] public float musicVolume = 0.5f;
    private AudioSource musicPlayer;

    [Header("Scene Management")]
    [SerializeField] private string winScene = "WinScene";
    [SerializeField] private string gameOverScene = "GameOverScene";
    [SerializeField] private string nextLevelScene = " ";

    [Header("Timer Settings")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float levelTime = 60f;
    private float currentTime;
    private bool timerRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            musicPlayer = gameObject.AddComponent<AudioSource>();
            musicPlayer.playOnAwake = false;
            musicPlayer.loop = true;
            musicPlayer.volume = musicVolume;
            
            InitializeAudio();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeHealth();
        InitializeTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void InitializeTimer()
    {
        currentTime = levelTime;
        timerRunning = true;
    }

    private void UpdateTimer()
    {
        if (!timerRunning) return;
        
        currentTime -= Time.deltaTime;
        
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (currentTime <= 0f)
        {
            timerRunning = false;
            PlayerDeath();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {
            FindCoinText();
            FindHeartImages();
            UpdateHealthUI();
            InitializeTimer();
        }
    }

    private void FindCoinText()
    {
        coinText = GameObject.FindGameObjectWithTag("CoinText")?.GetComponent<TMP_Text>();
        if (coinText != null) coinText.text = coinsCollected.ToString();
    }

    private void FindHeartImages()
    {
        GameObject healthUI = GameObject.FindGameObjectWithTag("HealthUI");
        if (healthUI != null)
        {
            heartImages = healthUI.GetComponentsInChildren<Image>(true);
            UpdateHealthUI();
        }
    }

    public void AddCoins(int amount, bool isSpecial = false)
    {
        if (isSpecial)
        {
            specialCoinsCollected += amount;
            if (specialCoinText != null)
                specialCoinText.text = $"{specialCoinsCollected}/{specialCoinsRequiredToWin}";
            
            if (specialCoinsCollected >= specialCoinsRequiredToWin)
            {
                WinGame(true);
            }
        }
        else
        {
            coinsCollected += amount;
            if (coinText != null) 
                coinText.text = coinsCollected.ToString();
        }
    }

    public void WinGame(bool proceedToNextLevel = false)
    {
        string targetScene = proceedToNextLevel ? nextLevelScene : winScene;
        if (!string.IsNullOrEmpty(targetScene))
            SceneManager.LoadScene(targetScene);
        else
            Debug.LogError("Target scene not specified!");
    }

    private void PlayerDeath()
    {
        if (!string.IsNullOrEmpty(gameOverScene))
            SceneManager.LoadScene(gameOverScene);
        else
            Debug.Log("Game Over - No scene specified");
        
        if (musicPlayer != null) 
            musicPlayer.Stop();
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;
        
        currentHealth--;
        UpdateHealthUI();
        
        if (currentHealth <= 0)
            PlayerDeath();
    }

    private void UpdateHealthUI()
    {
        if (heartImages == null || heartImages.Length == 0) return;
        
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
                heartImages[i].gameObject.SetActive(i < maxHealth);
            }
        }
    }

    private void InitializeAudio()
    {
        if (backgroundMusic != null)
        {
            musicPlayer.clip = backgroundMusic;
            musicPlayer.Play();
        }
    }

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void ResetGameState()
    {
        coinsCollected = 0;
        specialCoinsCollected = 0;
        InitializeHealth();
        if (musicPlayer != null && backgroundMusic != null)
            musicPlayer.Play();
    }

    public void SetMusicVolume(float volume) 
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicPlayer != null)
            musicPlayer.volume = musicVolume;
    }
}