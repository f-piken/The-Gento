using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    
    public Canvas Canvas;

    [Header("GameOverPanel")]
    [SerializeField]
    private Canvas gameOverCanvas;

    [SerializeField]
    public GameObject panel1;
    public GameObject panel2;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(showGameOverPanel);
        GameManager.Instance.OnBackToMenu.AddListener(resetPanel);
    }

    private void resetPanel()
    {
        gameOverCanvas.gameObject.SetActive(!true);
    }

    private void showGameOverPanel(bool isWin)
    {
        MainMenuUI script1 = FindObjectOfType<MainMenuUI>();
        gameOverCanvas.gameObject.SetActive(true);

        if (isWin == true){
            panel1.SetActive(true);
            script1.status++;
        }else if (isWin == false){
            panel2.SetActive(true);
        }
    }
}