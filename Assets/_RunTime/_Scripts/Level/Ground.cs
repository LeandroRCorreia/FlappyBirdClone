using UnityEngine;

public class Ground : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other) 
    {
        PlayerAC playerAc = other.gameObject.GetComponentInChildren<PlayerAC>();

        if(playerAc != null)
        {
            playerAc.FrozenRotation();
        }
        
    }

}
