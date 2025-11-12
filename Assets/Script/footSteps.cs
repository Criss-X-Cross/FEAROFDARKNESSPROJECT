using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class footSteps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource footstepsSound, sprintSound;
    public CharacterController characterController; // optional: assign in inspector to detect grounding
    public float walkSpeed;
    public float sprintSpeed;

    private bool isJumping = false;

    void Start()
    {
        if (footstepsSound != null) footstepsSound.enabled = false;
        if (sprintSound != null) sprintSound.enabled = false;
    }

    void Update()
    {
        // Update jumping state
        if (characterController != null)
        {
            // Use CharacterController to detect airborne state
            if (!characterController.isGrounded)
            {
                if (!isJumping)
                {
                    isJumping = true;
                    StopFootstepSounds();
                }
            }
            else
            {
                isJumping = false;
            }
        }
        else
        {
            // to detect jump start/end
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                StopFootstepSounds();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }

        // If jumping, ensure footstep sounds remain stopped
        if (isJumping)
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
            return;
        }

        // Movement logic | Sprinting vs Walking
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //footstepsSound.pitch = 1.5f;
                footstepsSound.enabled = false;
                sprintSound.enabled = true;
                //sprintSound.volume = 1f;
                footstepsSound.pitch = Mathf.MoveTowards(footstepsSound.pitch, sprintSpeed, Time.deltaTime * 1f);
                Debug.Log("Sprinting");
            }
            else
            {
                //footstepsSound.pitch = 1f;
                //footstepsSound.volume = 1f;
                footstepsSound.enabled = true;
                sprintSound.enabled = false;
                footstepsSound.pitch = Mathf.MoveTowards(footstepsSound.pitch, walkSpeed, Time.deltaTime * 1f);
                Debug.Log("Walking");
            }
        }
        else
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
        }
    }

    private void StopFootstepSounds()
    {
        if (footstepsSound != null)
        {
            footstepsSound.Stop();
            footstepsSound.enabled = false;
        }
        if (sprintSound != null)
        {
            sprintSound.Stop();
            sprintSound.enabled = false;
        }
    }
}
