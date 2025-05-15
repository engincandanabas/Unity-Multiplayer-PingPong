using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LobbyManager;

public class CreateLobbyUI : MonoBehaviour
{
    public static CreateLobbyUI Instance { get; private set; }

    [SerializeField] private TMP_InputField lobbyNameField;
    [SerializeField] private Toggle lobbyVisibilityToggle;
    [SerializeField] private TMP_Dropdown lobbyMaxPlayerDropDown;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button createButton;

    [Header("Animation Settings")]
    [SerializeField] private Ease ease;
    [SerializeField] private float animDuration;
    [SerializeField] private Transform childObject;

    private void Awake()
    {
        Instance = this;
        HideWithoutAnim();
    }

    private void Start()
    {
        lobbyNameField.onValueChanged.AddListener(LobbyNameField_OnValueChanged);
        createButton.onClick.AddListener(CreateLobby);
        exitButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Hide();
        LobbyListUI.Instance.Show();
    }
    private void CreateLobby()
    {
        Hide();
        LobbyManager.Instance.CreateLobby(
                lobbyNameField.text,
                lobbyMaxPlayerDropDown.value,
                !lobbyVisibilityToggle.isOn
            );
    }
    private void LobbyNameField_OnValueChanged(string value)
    {
        if (value.Length > 1)
            createButton.interactable = true;
        else
            createButton.interactable = false;
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
        childObject.transform.localScale = Vector3.zero;
        this.transform.GetChild(0).gameObject.SetActive(true);
        childObject.transform.DOScale(Vector3.one, 0.2f).SetEase(ease).SetDelay(0.25f);

    }
}
