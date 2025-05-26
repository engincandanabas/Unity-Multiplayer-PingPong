using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameManager.PlayerType playerType;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float xOffset;

    private bool canMove = false;
    private GameManager.PlayerType thisPlayerType;

    private Vector2 screenBounds;
    private float playerHalfHeight;
    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        ScreenSizeManager.Instance.OnSizeChanged += SetPlayerPosition;
        playerHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y / 2;

        SetPlayerPosition(this,null);
    }

    private void Update()
    {
        if (!canMove) return;


        if (IsOwner)
        {
            var input = Input.GetAxis("Vertical");
            var movementDir = input * movementSpeed * Time.deltaTime;

            float clampedY = Mathf.Clamp(transform.position.y+ movementDir, -screenBounds.y+playerHalfHeight, screenBounds.y-playerHalfHeight);
            Vector2 pos = transform.position;
            pos.y= clampedY;
            transform.position = pos;
        }
    }

    private void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        canMove = true;
        thisPlayerType = GameManager.Instance.GetLocalPlayerType();
    }
    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinArgs args)
    {
        canMove=false;
    }
    private void SetPlayerPosition(object sender, EventArgs eventArgs)
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        var pos = this.transform.position;
        if (xOffset < 0)
            pos.x = screenBounds.x + xOffset;
        else
            pos.x = -screenBounds.x + xOffset;
        this.transform.position = pos;
    }
}
