using System.Collections.Generic;
using UnityEngine;

public class EndlessPipeGenerator : MonoBehaviour
{
    [SerializeField] private Transform pipeBasePrefab;
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameMode gameMode;

    private List<PipePair> currentPipeInScene = new List<PipePair>(10);
    private List<Ground> currentGroundInScene = new List<Ground>(10);
    [SerializeField] private Pool<PipePair> pipeObjPool;
    [SerializeField] private Pool<Ground> groundObjPool;


    [Header("Ground Parameter")]

    [Range(2, 5)]
    [SerializeField] private int maxGroundFrontOfPlayer = 2;

    [Range(2, 5)]
    [SerializeField] private int maxGroundBehindOfPlayer = 2;

    private float xCurrentGroundPosition = 0;
    private float xDistanceBetweenGround = 9f;

    [Space]

    [Header("Pipe Parameter")]

    [Range(2, 5)]
    [SerializeField] private int maxPipesInFrontOfPlayer = 2;
    [Range(2, 5)]
    [SerializeField] private int maxPipesBehindOfPlayer = 2;
    [SerializeField] private float xDistanceBetweenPipes = 10;
    private float currentPipePositionX = 7f;
    private const float yMaxTop = 2.5f;
    private const float yMaxBottom = -2.5f;

    private void Start() 
    {
        pipeObjPool.InitializePool();
        groundObjPool.InitializePool();
    }

    private void Update() 
    {
        if(!gameMode.IsGameStarted)
        {
            WaitingStartGame();
            return;
        }

        UpdateEndlessPipeGenerator();
    }

    private void WaitingStartGame()
    {
        CheckIfNeedAddGround();
        CheckDestroyGround();
        currentPipePositionX = playerController.transform.position.x + xDistanceBetweenPipes;
    }

    private void UpdateEndlessPipeGenerator()
    {
        CheckIfNeedAddFront();
        CheckDestroyPipe();
        CheckDestroyGround();

    }

    private void SpawnPipe()
    {
        Vector3 positionPipe = Vector3.zero;
        positionPipe.x = currentPipePositionX;
        positionPipe.y = Random.Range(yMaxBottom, yMaxTop);

        currentPipePositionX += xDistanceBetweenPipes;

        PipePair pipeObj = pipeObjPool.GetFromPool(positionPipe);
        currentPipeInScene.Add(pipeObj);
    }

    private void SpawnGround()
    {
        Vector3 positionGround = new Vector3(xCurrentGroundPosition, -6);
        Ground groundObj = groundObjPool.GetFromPool(positionGround);
        xCurrentGroundPosition += xDistanceBetweenGround;

        currentGroundInScene.Add(groundObj);
    }

    private void CheckDestroyPipe()
    {
        for (int i = currentPipeInScene.Count; i > 0; i--)
        {
            PipePair pipe = currentPipeInScene[i - 1];
            if (pipe.transform.position.x < playerController.transform.position.x - (xDistanceBetweenPipes * maxPipesBehindOfPlayer))
            {
                pipeObjPool.ReturnToPool(pipe);
                currentPipeInScene.RemoveAt(i - 1);
            }
        }
    }

    private void CheckDestroyGround()
    {
        for (int i = currentGroundInScene.Count; i > 0; i--)
        {
            Ground ground = currentGroundInScene[i - 1];
            if (ground.transform.position.x < playerController.transform.position.x - (xDistanceBetweenPipes * maxGroundBehindOfPlayer))
            {
                groundObjPool.ReturnToPool(ground);
                currentGroundInScene.RemoveAt(i - 1);
            }
        }
    }

    private void CheckIfNeedAddFront()
    {
        CheckIfNeedAddPipes();
        CheckIfNeedAddGround();
    }

    private void CheckIfNeedAddGround()
    {
        if (xCurrentGroundPosition < (xDistanceBetweenGround * maxGroundFrontOfPlayer) + playerController.transform.position.x)
        {
            SpawnGround();
        }
    }

    private void CheckIfNeedAddPipes()
    {
        if ((xDistanceBetweenPipes * maxPipesInFrontOfPlayer) + playerController.transform.position.x > currentPipePositionX)
        {
            SpawnPipe();
        }
    }

}
