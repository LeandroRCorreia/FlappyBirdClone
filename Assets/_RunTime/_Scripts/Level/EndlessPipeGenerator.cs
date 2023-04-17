using System.Collections.Generic;
using UnityEngine;

public class EndlessPipeGenerator : MonoBehaviour
{
    [SerializeField] private Transform pipeBasePrefab;
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameMode gameMode;

    private List<GameObject> currentPipeInScene = new List<GameObject>();
    private List<GameObject> currentGroundInScene = new List<GameObject>();

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
    private float currentPipePosition = 7f;
    private const float yMaxTop = 2.5f;
    private const float yMaxBottom = -2.5f;

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
        currentPipePosition = playerController.transform.position.x + xDistanceBetweenPipes;
    }

    private void UpdateEndlessPipeGenerator()
    {
        CheckIfNeedAddFront();
        CheckDestroyPipe();
        CheckDestroyGround();

    }

    private void SpawnPipe()
    {
        var pipe = Instantiate(pipeBasePrefab, transform);
        pipe.position = new Vector3(currentPipePosition, Random.Range(yMaxBottom, yMaxTop));
        currentPipePosition += xDistanceBetweenPipes;
        currentPipeInScene.Add(pipe.gameObject);
    }

    private void SpawnGround()
    {
        var groundInstance = Instantiate(groundPrefab, transform);
        groundInstance.position = new Vector3(xCurrentGroundPosition, -6);
        xCurrentGroundPosition += xDistanceBetweenGround;
        currentGroundInScene.Add(groundInstance.gameObject);
    }

    private void CheckDestroyPipe()
    {
        for (int i = currentPipeInScene.Count; i > 0; i--)
        {
            var pipe = currentPipeInScene[i - 1].transform;
            if (pipe.position.x < playerController.transform.position.x - (xDistanceBetweenPipes * maxPipesBehindOfPlayer))
            {
                Destroy(pipe.gameObject);
                currentPipeInScene.RemoveAt(i - 1);
            }
        }
    }

    private void CheckDestroyGround()
    {
        for (int i = currentGroundInScene.Count; i > 0; i--)
        {
            var ground = currentGroundInScene[i - 1].transform;
            if (ground.position.x < playerController.transform.position.x - (xDistanceBetweenPipes * maxGroundBehindOfPlayer))
            {
                Destroy(ground.gameObject);
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
        if ((xDistanceBetweenPipes * maxPipesInFrontOfPlayer) + playerController.transform.position.x > currentPipePosition)
        {
            SpawnPipe();
        }
    }

}
