using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance { get { return instance; } }
    //infor
    private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    public float damagePlayer;
    //camera,movement
    [SerializeField] private Camera m_Camera;
    [SerializeField] CharacterController characterController;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float speed = 4f;
    
    private float RotationSpeed = 15;
    
    float mDesiredRotation = 0f;
    //jump,gravity
    float mSpeedY = 0;
    float mGravity = -9.81f;
    [SerializeField] private float JumpSpeed = 4f;
    //
    public PlayerState state;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (state != PlayerState.Die)
        {
            Movement();
            Attack();
        }
    }
    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(h, 0, v).normalized;

        Vector3 rotateMovement = Quaternion.Euler(0, m_Camera.transform.rotation.eulerAngles.y, 0) * movement;
        Vector3 verticalMovement = Vector3.up * mSpeedY;
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mSpeedY = JumpSpeed;
                m_Animator.SetBool(Constant.ANIM_JUMP, true);
            }
        }
        else
        {
            mSpeedY += mGravity * Time.deltaTime;
            m_Animator.SetBool(Constant.ANIM_JUMP, false);
        }
        characterController.Move((rotateMovement * speed + verticalMovement) * Time.deltaTime);
        if (rotateMovement.magnitude > 0)
        {
            mDesiredRotation = Mathf.Atan2(rotateMovement.x, rotateMovement.z) * Mathf.Rad2Deg;
            m_Animator.SetBool(Constant.ANIM_RUN, true);
            state = PlayerState.Moving;
        }
        else
        {
            m_Animator.SetBool(Constant.ANIM_RUN, false);
            state = PlayerState.Idle;
        }
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
        
    }
    private void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            state = PlayerState.Attack;
            m_Animator.SetBool(Constant.ANIM_ATTACK, true);
        }
        else
        {
            m_Animator.SetBool(Constant.ANIM_ATTACK, false);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            state = PlayerState.Attack;
            m_Animator.SetBool(Constant.ANIM_SKILL, true);
        }
        else
        {
            m_Animator.SetBool(Constant.ANIM_SKILL, false);
        }
    }
}
public enum PlayerState
{
    Idle,
    Moving,
    Attack,
    Die
}
