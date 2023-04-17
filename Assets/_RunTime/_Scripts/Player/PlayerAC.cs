using System.Collections;
using UnityEngine;

public static class PlayerConstantAnimationsKeys
{
    public const string IsNotFlapping = "IsNotFlapping";
    public const string FlappMultiplier  = "FlappMultiplier";
}

public class PlayerAC : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameMode gameMode;

    [Space]

    [Header("Flying animation")]    
    [SerializeField] private float zRotSpeed = 5f;
    [SerializeField] private float maxRotation = 0.35f;
    [SerializeField] private float minRotation = -0.7f;
    [Range(0.15f, 0.5f)]
    [SerializeField] private float lenghtPingPongEffect = 0.5f;
    [Range(0.15f, 0.5f)]
    [SerializeField] private float ySpeedPingpongEffect = 0.375f;

    private void Start() 
    {
        StartCoroutine(ProcessPingPongAnimCoro());
    }

    private void LateUpdate()
    {
        ProcessBirdRotation();
        animator.SetBool(PlayerConstantAnimationsKeys.IsNotFlapping, playerController.IsFalling);
    }

    private void PingPongAnim()
    {
        var yPosition = Mathf.PingPong(Time.time + ySpeedPingpongEffect * Time.deltaTime, lenghtPingPongEffect);
        transform.localPosition = new Vector3(transform.localPosition.x, yPosition);
    }
    
    private IEnumerator ProcessPingPongAnimCoro()
    {

        while(!gameMode.IsGameStarted)
        {
            PingPongAnim();
            yield return null;
        }
        transform.localPosition = Vector3.zero;

    }

    private void ProcessBirdRotation()
    {
        BirdRotation();
        LimitBirdRotation();
    }

    private void BirdRotation()
    {
        transform.Rotate(playerController.IsFalling ?
        Vector3.back :
        Vector3.forward, zRotSpeed * Time.deltaTime);
    }

    private void LimitBirdRotation()
    {
        if(transform.rotation.z >= maxRotation)
        {   
            transform.eulerAngles = new Vector3(0,0, 25);
        }   
        else if(transform.rotation.z <= minRotation)
        {
            transform.eulerAngles = new Vector3(0,0, -90);
        }
    }

    public void FrozenRotation()
    {
        zRotSpeed = 0;
    }
    
}
