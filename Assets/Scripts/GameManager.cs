using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static BallController;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler OnScoreChanged;
    public event EventHandler<PlayerNamesArgs> OnUsernameGet;

    public event EventHandler<OnGameWinArgs> OnGameWin;
    public class OnGameWinArgs : EventArgs
    {
        public FixedString32Bytes playerName;
    }

    public GameObject playerLeft;
    public GameObject playerRight;

    public class PlayerNamesArgs : EventArgs
    {
        public FixedString32Bytes player1Name;
        public FixedString32Bytes player2Name;
    }

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
    private void OnDisable()
    {
        BallController.Instance.OnPlayerScore -= BallController_OnPlayerScore;
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
    [ServerRpc]
    private void AssignPlayerServerRpc()
    {
        Debug.Log("Assigning ownerships...");

        List<ulong> clientIds = new List<ulong>();
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            clientIds.Add(client.ClientId);
        }

        if (clientIds.Count >= 2)
        {
            playerLeft.GetComponent<NetworkObject>().ChangeOwnership(clientIds[0]);
            playerRight.GetComponent<NetworkObject>().ChangeOwnership(clientIds[1]);

            Debug.Log($"Left assigned to {clientIds[0]}, Right assigned to {clientIds[1]}");
        }
        else
        {
            Debug.LogWarning("Not enough players to assign ownership.");
        }
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

        if (playerLeftScore.Value == 3)
        {
            OnGameWin?.Invoke(this, new OnGameWinArgs { playerName = "PLAYER 1" });
        }
        else if(playerRightScore.Value == 3)
        {
            OnGameWin?.Invoke(this, new OnGameWinArgs { playerName = "PLAYER 2" });
        }

        OnScoreChanged?.Invoke(this, null);
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            // Start Game
            AssignPlayerServerRpc();
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
