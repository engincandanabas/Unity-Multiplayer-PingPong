using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameManager.PlayerType playerType;

    [SerializeField] private float movementSpeed;

    private bool gameStarted = false;
    private GameManager.PlayerType thisPlayerType;

    private Vector2 screenBounds;
    private float playerHalfHeight;
    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        playerHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y / 2;
        screenBounds=Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));  
    }

    private void Update()
    {
        if (!gameStarted) return;


        if (thisPlayerType == playerType)
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
        gameStarted = true;
        thisPlayerType = GameManager.Instance.GetLocalPlayerType();
    }
}
