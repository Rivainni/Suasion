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

    // Update is called once per frame
    void Update()
    {
        if (!activeLock)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        rBody.MovePosition(rBody.position + movement * moveSpeed * Time.fixedDeltaTime);
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