using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField] GameObject retryPanel;
    [SerializeField] GameObject levelCompletedPanel;
    [SerializeField] GameObject wonPanel;

    public bool gameStop = false;

    int totalLevel = 2;
    int currentLevel = 1;

    public void YouDied()
    {
        if (gameStop) return;
        gameStop = true;
        retryPanel.gameObject.SetActive(true);
    }

    public void levelCompleted()
    {
        if (gameStop) return;
        gameStop = true;
        levelCompletedPanel.gameObject.SetActive(true);
    }

    public void YouWon()
    {
        if (currentLevel < totalLevel)
        {
            levelCompleted();
            return;
        }
        if (gameStop) return;
        gameStop = true;
        wonPanel.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        gameStop = false;
        levelCompletedPanel.gameObject.SetActive(false);
        retryPanel.gameObject.SetActive(false);
        wonPanel.gameObject.SetActive(false);

        if (currentLevel < totalLevel)
        {
            SceneManager.LoadScene(currentLevel);
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
            SceneManager.LoadScene(0);
        }
    }
}
