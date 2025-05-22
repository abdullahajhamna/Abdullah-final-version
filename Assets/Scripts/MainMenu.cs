using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private TMP_Text quitButtonText;

    [Header("Settings")]
    [SerializeField] private string gameSceneName = "LevelOne";
    [SerializeField] private float titleAnimationAmplitude = 0.5f;
    [SerializeField] private float titleAnimationFrequency = 1f;

    private Vector3 titleOriginalPosition;

    private void Start()
    {
        titleOriginalPosition = titleText.transform.position;
        playButtonText.text = "PLAY";
        quitButtonText.text = "QUIT";

        playButtonText.GetComponentInParent<Button>().onClick.AddListener(OnPlayButtonClicked);
        quitButtonText.GetComponentInParent<Button>().onClick.AddListener(OnQuitButtonClicked);
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * titleAnimationFrequency) * titleAnimationAmplitude;
        titleText.transform.position = titleOriginalPosition + new Vector3(0, yOffset, 0);
    }

    public void OnPlayButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameState(); // Reset coins & health
        }
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

