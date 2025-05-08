using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static BallController;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler OnScoreChanged;

    private NetworkVariable<int> playerLeftScore = new NetworkVariable<int>();
    private NetworkVariable<int> playerRightScore = new NetworkVariable<int>();

    public enum PlayerType
    {
        None,
        PlayerLeft,
        PlayerRight
    }
    public PlayerType localPlayerType;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        BallController.Instance.OnPlayerScore += BallController_OnPlayerScore;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("Network spawned : " + NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            localPlayerType = PlayerType.PlayerLeft;
        }
        else
        {
            localPlayerType = PlayerType.PlayerRight;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }

        playerLeftScore.OnValueChanged += (int prevScore, int newScore) =>
        {
            OnScoreChanged?.Invoke(this, EventArgs.Empty);
        };
        playerRightScore.OnValueChanged += (int prevScore, int newScore) =>
        {
            OnScoreChanged?.Invoke(this, EventArgs.Empty);
        };
    }
    private void BallController_OnPlayerScore(object sender, OnPlayerScoreArgs eventArgs)
    {
        if (eventArgs.scorePlayerType == PlayerType.PlayerLeft)
        {
            playerLeftScore.Value++;
        }
        else
        {
            playerRightScore.Value++;
        }
        Debug.Log("Player1Score : " + playerLeftScore.Value);
        Debug.Log("Player2Score : " + playerRightScore.Value);

        OnScoreChanged?.Invoke(this,null);
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 1)
        {
            // Start Game
            TriggerOnGameStartedRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerOnGameStartedRpc()
    {
        OnGameStarted?.Invoke(this, null);
    }
    public PlayerType GetLocalPlayerType()
    {
        return localPlayerType;
    }
    public void GetScores(out int player1Score, out int player2Score)
    {
        player1Score = this.playerLeftScore.Value;
        player2Score = this.playerRightScore.Value;
    }
}
