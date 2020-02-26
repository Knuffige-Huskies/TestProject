using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPC : MonoBehaviour
{
    [SerializeField] protected Camera m_CharacterCamera; //camera of the character

    private CharacterController m_Controller; //reference to the character controller
    [SerializeField] private Transform m_GroundCheck; //ground check object of the player
    [SerializeField] private LayerMask m_GroundMask; //mask that tells what layers are ground

    [SerializeField] private float m_CharacterBaseSpeed = 12f; //speed of the player without buffs applied
    [SerializeField] private float m_CharacterSpeedMultiplier { get; set; } = 1f; //multiplier for the character speed
    [SerializeField] private float m_Gravity = -9.81f; //gravity of the map
    [SerializeField] private float m_JumpHeight = 2f; //jump height of the player
    [SerializeField] private float m_GroundDistance = 0.4f; //distance when player no longer stands on the ground
    [SerializeField] private float m_MouseSensitivity = 100f; //sensitivity of the mouse
    [SerializeField] private float m_CowerHeight = 0.8f; //(OUTDATED) height of player when cowering 
    private float m_CharacterHeight; //height of the player

    private float m_XRotation = 0f; //x rotation of the player
    private Vector3 m_Velocity; //current velocity of the player
    private bool m_IsGrounded; //if the player stands on the ground

    /// <summary>
    /// Start is called before the first frame update
    /// Init Character Controller, height and lock cursor
    /// </summary>
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //m_CharacterHeight = m_Controller.transform.localScale.y;
    }

    /// <summary>
    /// Called every update step
    /// Run all allowed types of looking, moving, ...
    /// </summary>
    private void Update()
    {
        LookAround();
        CheckIfGrounded();
        Walk();
        Jump();
        //Cower();
    }

    /// <summary>
    /// Turn the character if the mouse is moved. 
    /// </summary>
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;
        m_XRotation -= mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);
        m_CharacterCamera.transform.localRotation = Quaternion.Euler(m_XRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Check if the character touches the ground.
    /// </summary>
    private void CheckIfGrounded()
    {
        m_IsGrounded = Physics.CheckSphere(m_GroundCheck.position, m_GroundDistance, m_GroundMask);
        // Reset velocity (down) when already on ground
        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -2f;
        }
    }

    /// <summary>
    /// Move character if player uses wasd.
    /// </summary>
    private void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        m_Controller.Move(move * m_CharacterSpeedMultiplier * m_CharacterBaseSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Jump on 'space' click.
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
        }
        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_Controller.Move(m_Velocity * Time.deltaTime);
    }

    /*
    /// <summary>
    /// (OUTDATED)
    /// Start cowering when left shift button is pressed, stop cowering on release.
    /// </summary>
    private void Cower()
    {
        if (Input.GetButtonDown("Cower"))
        {
            Vector3 size = m_Controller.transform.localScale;
            size.y = size.y * m_CowerHeight;
            m_Controller.transform.localScale = size;
            m_CharacterSpeedMultiplier /= 2;
        }
        if (Input.GetButtonUp("Cower"))
        {
            Vector3 size = m_Controller.transform.localScale;
            size.y = m_CharacterHeight;
            m_Controller.transform.localScale = size;
            m_CharacterSpeedMultiplier *= 2;
        }
    }
    */
}
