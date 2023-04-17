using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementParams", menuName = "FlappyBirdClone2.0/PlayerMovementParams", order = 0)]
public class PlayerMovementParams : ScriptableObject 
{
    [field:SerializeField] public float velocityY {get; private set; }
    [field:SerializeField] public float xVelocity { get; private set; }

}

