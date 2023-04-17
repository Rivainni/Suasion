using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] Animator animator;

    Vector2 movement;
    bool isPlaying = false;
    bool activeLock = false;
    float oldX;
    float oldY;

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

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x == 0 && movement.y == 0)
            {
                animator.SetFloat("OldH", oldX);
                animator.SetFloat("OldV", oldY);
            }
            else
            {
                oldX = movement.x;
                oldY = movement.y;
            }
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Speed", 0);
            animator.SetFloat("OldH", oldX);
            animator.SetFloat("OldV", oldY);
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

    void PlayFootsteps()
    {
        AkSoundEngine.PostEvent("Player_Footsteps", gameObject);
    }

    void PauseFootsteps()
    {
        AkSoundEngine.ExecuteActionOnEvent("Player_Footsteps", AkActionOnEventType.AkActionOnEventType_Pause, gameObject);
    }

    public void LockMovement()
    {
        activeLock = true;
    }

    public void UnlockMovement()
    {
        activeLock = false;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerPosition = rBody.position;
    }

    public void LoadData(GameData gameData)
    {
        rBody.position = gameData.playerPosition;
    }
}