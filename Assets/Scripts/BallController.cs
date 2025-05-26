using System;
using Unity.Netcode;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance { get; private set; }

    public event EventHandler<OnPlayerScoreArgs> OnPlayerScore;
    public class OnPlayerScoreArgs : EventArgs
    {
        public GameManager.PlayerType scorePlayerType;
    }

    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxInitialAngle = 0.67f;
    [SerializeField] private float velocityMultiplier = 1.1f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private TrailRenderer trailRenderer;

    private bool canThrow=false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
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
        canThrow = true;
        InitialPush(this, null);
    }
    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinArgs args)
    {
        canThrow = false;
        ResetBall(this,null);
    }

    private void InitialPush(object sender, OnPlayerScoreArgs onPlayerScoreArgs)
    {
        if(!canThrow) return;

        this.Wait(2, () =>
            {
                Vector2 dir =(UnityEngine.Random.Range(0,2)==0) ? Vector2.left:Vector2.right;
                dir.y = UnityEngine.Random.Range(-maxInitialAngle, maxInitialAngle);
                rb.linearVelocity = dir * movementSpeed;
            });
    }
    private void ResetBall(object sender, OnPlayerScoreArgs onPlayerScoreArgs)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        this.transform.position = Vector2.zero;
        trailRenderer.enabled = false;
        this.Wait(0.25f, () =>
        {
            GetComponent<SpriteRenderer>().enabled = true;
            trailRenderer.enabled = true;
        });

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Score"))
        {
            SpawnVfxRpc(this.transform.position);

            GameManager.PlayerType playerType = (this.transform.position.x < 0) ? GameManager.PlayerType.PlayerRight : GameManager.PlayerType.PlayerLeft;
            OnPlayerScore?.Invoke(this, new OnPlayerScoreArgs
            {
                scorePlayerType = playerType
            });
            CinemachineShake.Instance.ShakeCamera();
            AudioManager.Instance.PlaySound("Wall");

            
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity *= velocityMultiplier;
            CinemachineShake.Instance.ShakeCamera();
            AudioManager.Instance.PlaySound("Score");

        }
    }
    [Rpc(SendTo.Server)]
    public void SpawnVfxRpc(Vector3 pos)
    {
        GameObject vfx = Instantiate(vfxPrefab, pos, Quaternion.identity);
        vfx.GetComponent<NetworkObject>().Spawn(true);
    }
}
