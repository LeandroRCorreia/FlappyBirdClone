using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [field: SerializeField] public PlayerMovementParams playerMovement {get; set;}  

    [SerializeField] private float maxVelocityY;
    [SerializeField] private float flappTime;
    private float lastFlappTime = 0;

    [Space]

    [Header("Audio")]

    [SerializeField] private AudioClip flappSound;
    private AudioSource audioSource;

    private float lastFlapInitialPositionY = -10;
    public bool IsFalling => lastFlapInitialPositionY >= transform.position.y;

    public bool StartGameTrigger { get; private set; }
    
    public Rigidbody2D Rb => rb;

    public bool IsDead {get; private set;}

    public bool DeathZone => transform.position.y > 8f;  

    public AudioSource AudioSource => audioSource != null ? audioSource : audioSource = GetComponent<AudioSource>();

    public bool CanFlapp => IsDead || lastFlappTime +  flappTime >= Time.time;

    public float LastFlappTime => lastFlappTime;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();    
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start() 
    {
        lastFlapInitialPositionY = -10;    
        StartGameTrigger = false;
        IsDead = false;
    }

    private void Update()
    {
        if(IsDead) return;
        HandleInput();
    }

    private void FixedUpdate() 
    {
        xApplyMovement();
    }

    private void LateUpdate() 
    {
        LimiterFlappVelocity();
    }

    private void xApplyMovement()
    {
        transform.position = new Vector2(transform.position.x + playerMovement.xVelocity * Time.fixedDeltaTime, transform.position.y);
    }

    private void HandleInput()
    {   
        if (playerInput.Tap())
        {
            Flapp();
        }
    }

    public void Flapp()
    {
        if(CanFlapp) return;

        lastFlappTime = Time.time;
        StopImmediately();
        lastFlapInitialPositionY = transform.position.y;
        StartGameTrigger = true;
        AudioUtility.PlaySFX(AudioSource, flappSound);
        rb.AddForce(Vector2.up * playerMovement.velocityY, ForceMode2D.Impulse);
    }

    private void LimiterFlappVelocity()
    {
        if (rb.velocity.y > maxVelocityY)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityY);
        }
        else if (rb.velocity.y < -maxVelocityY)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocityY);
        }

    }

    private void StopImmediately()
    {
        rb.velocity = Vector2.zero;
    }

    public void Death()
    {
        StopImmediately();
        IsDead = true;
    }

}
