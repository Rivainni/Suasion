using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] Animator animator;

    Vector2 movement;
    bool isPlaying = false;
    bool activeLock = false;

    // Update is called once per frame
    void Update()
    {
        if (!activeLock)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if ((movement.x != 0 || movement.y != 0) && !isPlaying)
            {
                PlayFootsteps();
                isPlaying = true;
            }
            else if (isPlaying && (movement.x == 0 && movement.y == 0))
            {
                PauseFootsteps();
                isPlaying = false;
            }

            animator.SetInteger("Horizontal", Mathf.RoundToInt(movement.x));
            animator.SetInteger("Vertical", Mathf.RoundToInt(movement.y));
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetInteger("Horizontal", 0);
            animator.SetInteger("Vertical", 0);
            animator.SetFloat("Speed", 0);
            PauseFootsteps();
            isPlaying = false;
        }
    }

    void FixedUpdate()
    {
        if (!activeLock)
        {
            rBody.MovePosition(rBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void LockMovement()
    {
        activeLock = true;
    }

    public void UnlockMovement()
    {
        activeLock = false;
    }

    void PlayFootsteps()
    {
        AkSoundEngine.PostEvent("Player_Footsteps", gameObject);
    }

    void PauseFootsteps()
    {
        AkSoundEngine.ExecuteActionOnEvent("Player_Footsteps", AkActionOnEventType.AkActionOnEventType_Pause, gameObject);
    }
}