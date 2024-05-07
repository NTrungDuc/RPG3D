using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance { get { return instance; } }
    //infor
    private float maxHealth = 100;
    private float currentHealth;
    float attackRange = 5f;
    public PlayerState state;

    public Slider healthBar;
    private float staminaValue = 90;
    public Slider staminaBar;
    bool isUseStamina = false;
    bool isAttack = false;
    bool isSprint = false;
    //camera,movement
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Camera m_Camera;
    [SerializeField] CharacterController characterController;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float speed = 4f;

    private float RotationSpeed = 15;

    float mDesiredRotation = 0f;
    //skill
    [SerializeField] private ParticleSystem ultimateEffect;
    [SerializeField] private Transform posUltimate;
    [SerializeField] private Image UltimateSkill;
    float timeCDSkill = 10f;
    bool isUseSkill = false;
    //jump,gravity
    float mSpeedY = 0;
    float mGravity = -9.81f;
    [SerializeField] private float JumpSpeed = 4f;

    //Panel
    [SerializeField] private GameObject PanelInventory;
    bool isOpenInventory = false;


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
            Inventory();
            CDSkill();
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
        PlayerSprint();
        //restore stamina
        RestoreStamina();
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
            m_Animator.SetBool(Constant.ANIM_SPRINT, false);
        }


        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);

    }
    void RestoreStamina()
    {
        if(!isAttack && !isSprint)
        {
            isUseStamina = false;
        }
        else
        {
            isUseStamina = true;
        }
        if (!isUseStamina)
        {
            if (staminaValue < 90)
            {
                ModifyStamina(10);
            }
        }
    }
    public void PlayerSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (state == PlayerState.Moving)
            {
                if (staminaValue > 0)
                {
                    isSprint = true;
                    ModifyStamina(-10);

                    speed = 8;
                    m_Animator.SetBool(Constant.ANIM_SPRINT, true);
                }
                else
                {
                    speed = 4;
                    m_Animator.SetBool(Constant.ANIM_SPRINT, false);
                }
            }

        }
        else
        {
            isSprint = false;

            speed = 4;
            m_Animator.SetBool(Constant.ANIM_SPRINT, false);
        }
    }

    public void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (Input.GetMouseButton(0))
        {
            if (staminaValue > 0)
            {
                isAttack = true;
                ModifyStamina(-10);

                state = PlayerState.Attack;
                m_Animator.SetBool(Constant.ANIM_ATTACK, true);
                //target enemy
                TargetEnemy(nearestEnemy);
            }
            else
            {
                m_Animator.SetBool(Constant.ANIM_ATTACK, false);
            }
        }
        else
        {
            isAttack = false;
            m_Animator.SetBool(Constant.ANIM_ATTACK, false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            //use skill
            state = PlayerState.Attack;
            UltimateSkill.fillAmount = 1;
            if (!isUseSkill)
            {  
                StartCoroutine(EffectSKill(ultimateEffect, 4f, timeCDSkill));
            }
        }
    }
    void CDSkill()
    {
        if (isUseSkill)
        {
            UltimateSkill.fillAmount -= 1 / (timeCDSkill + 4) * Time.deltaTime;
            if (UltimateSkill.fillAmount <= 0)
            {
                UltimateSkill.fillAmount = 0;
            }
        }
    }
    IEnumerator EffectSKill(ParticleSystem skill, float time, float timeCD)
    {
        isUseSkill = true;
        skill.transform.position = posUltimate.position;
        skill.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        skill.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeCD);
        isUseSkill = false;
    }
    void TargetEnemy(GameObject target)
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance && distance <= attackRange)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
    }
    void ModifyStamina(float delta)
    {
        staminaValue += delta * Time.deltaTime;
        staminaValue = Mathf.Clamp(staminaValue, 0, 90);
        staminaBar.value = staminaValue;
    }
    public void UsePosion(float value)
    {
        if (currentHealth < 100)
        {
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100);
            healthBar.value = currentHealth;
            InventoryManager.Instance.refreshCountItem();
        }
        else
        {
            Debug.Log("Full HP");
        }
    }
    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isOpenInventory)
            {
                isOpenInventory = true;
                PanelInventory.SetActive(true);
                Cursor.visible = true;
                cinemachineFreeLook.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
            }
            else
            {
                isOpenInventory = false;
                PanelInventory.SetActive(false);
                Cursor.visible = false;
                cinemachineFreeLook.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
        }
    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            //die
            state = PlayerState.Die;
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
