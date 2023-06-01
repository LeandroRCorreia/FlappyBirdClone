using UnityEngine;

public class PipePair : MonoBehaviour, IPooledObject
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


}
