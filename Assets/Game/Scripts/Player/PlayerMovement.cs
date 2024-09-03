using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance { get { return instance; } }
    //information player
    private float maxHealth = 100;
    private float currentHealth;
    float attackRange = 5f;
    private Vector3 currentPos;
    public PlayerState state;
    [SerializeField] public LevelUp levelUp;
    public Slider healthBar;
    private float maxStamina = 90;
    private float staminaValue;
    public Slider staminaBar;
    //bool
    bool isUseStamina = false;
    bool isAttack = false;
    bool isSprint = false;
    bool isActiveShield = false;
    //shield
    [SerializeField] private ShieldUpgrade shield;
    [SerializeField] private Collider shieldCol;
    //camera,movement
    [SerializeField] private Camera m_Camera;
    [SerializeField] CharacterController characterController;
    [SerializeField] private Animator m_Animator;
    private float speed = 4f;
    float initialSpeed;

    private float RotationSpeed = 15;

    float mDesiredRotation = 0f;

    //jump,gravity
    float mSpeedY = 0;
    float mGravity = -9.81f;
    [SerializeField] private float JumpSpeed = 4f;

    //Panel
    [SerializeField] private GameObject PanelInventory;
    public bool isOpenInventory = false;
    //target obj
    [SerializeField] private LayerMask enemyLayer;
    //combo attack
    private int comboStep = 1;
    private float lastAttackTime = 0f;
    [SerializeField] private float comboDelay = 0.5f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(upgradeStats(0.1f));
        currentPos = transform.position;
        initialSpeed = speed;
    }
    private void Update()
    {
        if (state != PlayerState.Die)
        {

            Movement();
            Inventory();
            WeaponsCDManager.Instance.CDWeapons();
        }
        else
        {
            StartCoroutine(RespawnPlayer());
        }
    }
    private void Movement()
    {
        if (state == PlayerState.Attack || isOpenInventory)
        {
            return;
        }
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
        Defend(); //shield
        characterController.Move((rotateMovement * speed + verticalMovement) * Time.deltaTime);
        if (rotateMovement.magnitude > 0)
        {
            mDesiredRotation = Mathf.Atan2(rotateMovement.x, rotateMovement.z) * Mathf.Rad2Deg;
            m_Animator.SetBool(Constant.ANIM_RUN, true);

            state = PlayerState.Moving;
            //rotation
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
            //run
            PlayerSprint();
            if (!isSprint)
            {
                SoundManager.Instance.soundWalk(true);
            }
            else
            {
                SoundManager.Instance.soundWalk(false);
            }
        }
        else
        {
            m_Animator.SetBool(Constant.ANIM_RUN, false);

            state = PlayerState.Idle;
            m_Animator.SetBool(Constant.ANIM_SPRINT, false);
            SoundManager.Instance.soundWalk(false);
        }




        //restore stamina
        RestoreStamina();

    }
    void RestoreStamina()
    {
        if (!isAttack && !isSprint)
        {
            isUseStamina = false;
        }
        else
        {
            isUseStamina = true;
        }
        if (!isUseStamina)
        {
            if (staminaValue < maxStamina)
            {
                ModifyStamina(2);
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

                    speed = initialSpeed * 2;
                    m_Animator.SetBool(Constant.ANIM_SPRINT, true);
                    SoundManager.Instance.soundRun(true);
                }
                else
                {
                    speed = initialSpeed;
                    m_Animator.SetBool(Constant.ANIM_SPRINT, false);
                    SoundManager.Instance.soundRun(false);
                }
            }

        }
        else
        {
            isSprint = false;

            speed = initialSpeed;
            m_Animator.SetBool(Constant.ANIM_SPRINT, false);
            SoundManager.Instance.soundRun(false);
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButton(0))
        {            
            if (staminaValue > 0)
            {
                isAttack = true;
                ModifyStamina(-10);

                state = PlayerState.Attack;
                comboAttack();
                SoundManager.Instance.soundSword(true);
                //target enemy
                GameObject nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    TargetEnemy(nearestEnemy);
                }
            }
            else
            {
                resetCombo();
                SoundManager.Instance.soundSword(false);
            }
        }
        else
        {
            isAttack = false;
            state = PlayerState.Idle;
            resetCombo();
            SoundManager.Instance.soundSword(false);
        }

    }
    void comboAttack()
    {
        if(lastAttackTime > comboDelay)
        {
            lastAttackTime = 0;
            comboStep++;
        }
        lastAttackTime += Time.deltaTime;
        if (comboStep == 1)
        {
            m_Animator.SetBool(Constant.ANIM_ATTACK1, true);
        }
        else if (comboStep == 2)
        {
            m_Animator.SetBool(Constant.ANIM_ATTACK2, true);
        }
        else if (comboStep == 3)
        {
            m_Animator.SetBool(Constant.ANIM_ATTACK3, true);
        }
        else
        {            
            resetCombo();
        }
    }
    void resetCombo()
    {
        comboStep = 1;
        m_Animator.SetBool(Constant.ANIM_ATTACK1, false);
        m_Animator.SetBool(Constant.ANIM_ATTACK2, false);
        m_Animator.SetBool(Constant.ANIM_ATTACK3, false);
    }
    public void Defend()
    {
        if (!isSprint)
        {
            if (Input.GetMouseButton(1))
            {
                isActiveShield = true;
                m_Animator.SetBool(Constant.ANIM_DEF, true);
                speed /= initialSpeed;

                shieldCol.enabled = true;
                //playerCol.enabled = false;
            }
            else
            {
                isActiveShield = false;
                m_Animator.SetBool(Constant.ANIM_DEF, false);
                speed = initialSpeed;

                shieldCol.enabled = false;
                //playerCol.enabled = true;
            }
        }
    }
    public void useAbilities(int abilities)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            WeaponsCDManager.Instance.UseWeapon(abilities);
        }

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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < nearestDistance && distance <= attackRange)
            {
                nearestEnemy = hitCollider.gameObject;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
    }
    void ModifyStamina(float delta)
    {
        staminaValue += delta * Time.deltaTime;
        staminaValue = Mathf.Clamp(staminaValue, 0, maxStamina);
        staminaBar.value = staminaValue;
    }
    public void UsePosion(float value, int index)
    {
        switch (index)
        {
            case 0:
                currentHealth += value;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                healthBar.value = currentHealth;
                InventoryManager.Instance.refreshCountItem();
                break;
            case 1:
                staminaValue += value;
                staminaValue = Mathf.Clamp(staminaValue, 0, maxStamina);
                staminaBar.value = staminaValue;
                InventoryManager.Instance.refreshCountItem();
                break;
        }

    }
    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isOpenInventory)
            {
                PanelInventory.SetActive(true);
                LockCursor(true, CursorLockMode.None);
            }
            else
            {
                PanelInventory.SetActive(false);
                LockCursor(false, CursorLockMode.Locked);
            }
        }
    }
    public void LockCursor(bool activeCursor, CursorLockMode mode)
    {
        Cursor.visible = activeCursor;
        CameraShake.Instance.freeLookCamera.enabled = !activeCursor;
        Cursor.lockState = mode;
        isOpenInventory = activeCursor;
    }
    public void UpgradeStats(float healthLevelMultiplier, float staminaLevelMultiplier, int level)
    {
        int newMaxHealth = Mathf.RoundToInt(maxHealth * Mathf.Pow(healthLevelMultiplier, level));
        int newMaxStamina =  Mathf.RoundToInt(maxStamina * Mathf.Pow(staminaLevelMultiplier, level));
        maxHealth = newMaxHealth;
        maxStamina = newMaxStamina;
        StartCoroutine(upgradeStats(0));
        StartCoroutine(AnimLevelUp());
    }
    IEnumerator AnimLevelUp()
    {
        m_Animator.SetTrigger(Constant.ANIM_LEVELUP);
        yield return new WaitForSeconds(2.3f);
        m_Animator.SetTrigger(Constant.ANIM_IDLE);
    }
    IEnumerator upgradeStats(float time)
    {
        yield return new WaitForSeconds(time);
        currentHealth = maxHealth;
        healthBar.value = healthBar.maxValue = currentHealth;
        staminaValue = maxStamina;
        staminaBar.value = staminaBar.maxValue = staminaValue;
    }
    public void SaveDataPlayer(PlayerData data)
    {
        data.health = maxHealth;
        data.stamina = maxStamina;
        levelUp.SaveDataLevelUp(data);
        shield.SaveLevelShield(data);
    }
    public void LoadDataPlayer(PlayerData data)
    {
        maxHealth = data.health;
        maxStamina = data.stamina;
        levelUp.LoadDataLevelUp(data);
        shield.LoadLevelShield(data);
    }
    public void takeDamage(float damage)
    {
        if (isActiveShield)
        {
            currentHealth -= damage / shield.shieldValue;
        }
        else
        {
            currentHealth -= damage;
        }
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            //die
            state = PlayerState.Die;
            
        }
    }
    public IEnumerator RespawnPlayer()
    {
        m_Animator.SetTrigger(Constant.ANIM_DIE);
        //Debug.Log("die");
        yield return new WaitForSeconds(5f);
        state = PlayerState.Idle;
        m_Animator.SetTrigger(Constant.ANIM_IDLE);
        healthBar.value = currentHealth = maxHealth;
        staminaBar.value = staminaValue = maxStamina;
        transform.position = currentPos;
        yield return new WaitForSeconds(0.5f);
        m_Animator.ResetTrigger(Constant.ANIM_IDLE);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_RESPAWN))
        {
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
