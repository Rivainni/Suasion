using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] Animator animator;

    Vector2 movement;
    bool activeLock = false;

    bool faceLeft = false;
    bool faceUp = false;
    bool xStop = false;
    bool yStop = false;
    float offset = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (!activeLock)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetInteger("Horizontal", Mathf.RoundToInt(movement.x));
            animator.SetInteger("Vertical", Mathf.RoundToInt(movement.y));
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetInteger("Horizontal", 0);
            animator.SetInteger("Vertical", 0);
            animator.SetFloat("Speed", 0);
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
}