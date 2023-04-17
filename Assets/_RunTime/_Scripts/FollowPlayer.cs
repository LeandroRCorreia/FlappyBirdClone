using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;

    void LateUpdate()
    {
        transform.position = new Vector3(playerPosition.position.x, transform.position.y, transform.position.z);  
    }

}
