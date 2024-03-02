using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    public event UnityAction OnGameFinished;

    [SerializeField] private UnityEvent onGameFinished;
    [SerializeField] private List<PCCore> pcsToBoot;

    private const string LEVEL_SAVE_KEY = "LevelIndex";
    
    private int _pcsBootedCount;

    private void Awake()
    {
        Instance = this;
        OnGameFinished += SaveLevelProgress;
    }

    private void Start()
    {
        foreach (PCCore pcToBoot in pcsToBoot)
        {
            pcToBoot.OnBooted += BootSinglePC;
        }
    }

    private void BootSinglePC()
    {
        _pcsBootedCount++;
        CheckForLevelFinished();
    }

    private void CheckForLevelFinished()
    {
        if(_pcsBootedCount >= pcsToBoot.Count)
            FinishLevel();
    }

    [ContextMenu("Forced Finish Game")]
    private void FinishLevel()
    {
        OnGameFinished?.Invoke();
        onGameFinished?.Invoke();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadNextScene()
    {
        int nextSceneIndex = PlayerPrefs.GetInt(LEVEL_SAVE_KEY, 0) + 1;
        nextSceneIndex %= SceneManager.sceneCountInBuildSettings;
        if (nextSceneIndex == 0) nextSceneIndex = 1;
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Should be in Save Class if prototype accepted
    private void SaveLevelProgress()
    {
        PlayerPrefs.SetInt(LEVEL_SAVE_KEY, SceneManager.GetActiveScene().buildIndex);
    }
}
