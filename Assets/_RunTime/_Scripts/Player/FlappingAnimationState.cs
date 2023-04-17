using UnityEngine;

public class FlappingAnimationState : StateMachineBehaviour
{

    [Header("Flapping Parameters")]
    [Range(0.1f, 0.75f)]
    [SerializeField] private float durationFastFly = 0.5f;

    [Range(1f, 1.5f)]
    [SerializeField] private float fastFlyMultiplier = 1.25f;

    [Range(0.25f, 0.75f)]
    [SerializeField] private float normalFlyMultiplier = 0.5f;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //TODO: don't assume a component is assigned to a given object
        PlayerController player = animator.GetComponentInParent<PlayerController>();

        bool canFlyFast = !player.IsFalling && !player.IsDead &&
        player.LastFlappTime + durationFastFly > Time.time;

        var currentFlyMultiplier = canFlyFast ? fastFlyMultiplier : normalFlyMultiplier;
        animator.SetFloat(PlayerConstantAnimationsKeys.FlappMultiplier, currentFlyMultiplier);

    }

}
