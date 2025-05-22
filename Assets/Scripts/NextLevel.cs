using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class NextLevel : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private string nextLevelName = "LevelTwo"; // Set in Inspector
    
    private Button levelButton;
    private TMP_Text buttonText;

    private void Awake()
    {
        // Get components
        levelButton = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        
        // Setup button
        if (buttonText != null)
        {
            buttonText.text = "NEXT LEVEL";
        }
        
        if (levelButton != null)
        {
            levelButton.onClick.AddListener(LoadNextLevel);
        }
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            // Reset game state if using GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGameState();
            }
            
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogWarning("Next level name not assigned!");
        }
    }

    public void Exit()
    {
        // Reset game state if using GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameState();
        }
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}