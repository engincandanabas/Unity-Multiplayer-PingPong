using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance {  get; private set; }

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Button leaveLobbyButton;

    [Header("Animation Settings")]
    [SerializeField] private Ease ease;
    [SerializeField] private float animDuration;
    [SerializeField] private Transform childObject;
    private void Awake()
    {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
        });

    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        HideWithoutAnim();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }
    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            PlayerListSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<PlayerListSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }


        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        Show();
    }
    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    public void Hide()
    {
        childObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(ease).OnComplete(() =>
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        });

    }
    public void HideWithoutAnim()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Show()
    {
        if(!this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            childObject.transform.localScale = Vector3.zero;
            this.transform.GetChild(0).gameObject.SetActive(true);
            childObject.transform.DOScale(Vector3.one, 0.2f).SetEase(ease).SetDelay(0.25f);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

    }
}
