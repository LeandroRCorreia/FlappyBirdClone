using UnityEngine;

public class Ground : MonoBehaviour, IPooledObject
{
    public void OnGetFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnInstantiate()
    {
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.TryGetComponent<PlayerAC>(out PlayerAC playerAc))
        {
            playerAc.FrozenRotation();
        }
        
    }

}
