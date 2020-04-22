using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontalMove = 0f;
    public float runSpeed = 40f;
    
    private Rigidbody m_Rigidbody;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (Time.time > 2)
        {
            Move(runSpeed * Time.fixedDeltaTime);
        }
    }


    void Move(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
        m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }
    
    
}
