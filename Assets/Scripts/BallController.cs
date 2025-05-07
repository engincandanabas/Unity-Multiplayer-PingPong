using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance { get; private set; }

    public event EventHandler<OnPlayerScoreArgs> OnPlayerScore;
    public class OnPlayerScoreArgs : EventArgs
    {
        public GameManager.PlayerType scorePlayerType;
    }

    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxInitialAngle = 0.67f;
    [SerializeField] private float velocityMultiplier = 1.1f;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        OnPlayerScore += ResetBall;
        OnPlayerScore += InitialPush;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= GameManager_OnGameStarted;
        OnPlayerScore -= ResetBall;
        OnPlayerScore -= InitialPush;
    }
    private void GameManager_OnGameStarted(object sender, EventArgs eventArgs)
    {
        InitialPush(this, null);
    }

    private void InitialPush(object sender, OnPlayerScoreArgs onPlayerScoreArgs)
    {
        this.Wait(2, () =>
            {
                Vector2 dir = Vector2.left;
                dir.y = UnityEngine.Random.Range(-maxInitialAngle, maxInitialAngle);
                rb.linearVelocity = dir * movementSpeed;
            });
    }
    private void ResetBall(object sender, OnPlayerScoreArgs onPlayerScoreArgs)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        this.transform.position = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = true;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Score"))
        {
            GameManager.PlayerType playerType = (this.transform.position.x < 0) ? GameManager.PlayerType.PlayerRight : GameManager.PlayerType.PlayerLeft;
            OnPlayerScore?.Invoke(this, new OnPlayerScoreArgs
            {
                scorePlayerType = playerType
            });
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity *= velocityMultiplier;
        }
    }
}
