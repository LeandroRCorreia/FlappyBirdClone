using UnityEngine;


public class PlayerInput : MonoBehaviour
{

    public bool Tap()
    {

#if UNITY_EDITOR_WIN
        if(Input.GetMouseButton(0))
        {
            return true;
        }
#endif

#if UNITY_ANDROID
        for(int i = 0; i < Input.touchCount; i++)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began )
            {
                return true;
            }
        }
#endif

        return false;
    }


}
