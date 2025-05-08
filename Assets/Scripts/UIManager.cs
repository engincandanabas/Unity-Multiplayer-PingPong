using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
    }
    private void GameManager_OnGameStarted(object sender, EventArgs args)
    {
        connectingUI.SetActive(false);
        gamePlayUI.SetActive(true);
    }
    private void GameManager_OnScoreChanged(object sender, EventArgs args)
    {
        GameManager.Instance.GetScores(out int player1Score, out int player2Score);
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }
}
