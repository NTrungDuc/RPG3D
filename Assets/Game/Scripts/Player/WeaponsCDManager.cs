using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsCDManager : MonoBehaviour
{
    private static WeaponsCDManager instance;
    public static WeaponsCDManager Instance { get { return instance; } }
    [SerializeField] private List<float> weaponCooldowns;
    [SerializeField] private Image weaponCooldownImages;
    [SerializeField] private Transform posUltimate;
    [SerializeField] private List<ParticleSystem> ultimateEffects;
    [SerializeField] private GameObject UISkillCooldown;

    private List<float> weaponTimers;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        weaponTimers = new List<float>(new float[weaponCooldowns.Count]);
    }

    public void CDWeapons()
    {
        for (int i = 0; i < weaponTimers.Count; i++)
        {
            if (weaponTimers[i] > 0)
            {
                weaponTimers[i] -= Time.deltaTime;
                if (weaponTimers[i] < 0) weaponTimers[i] = 0;
                CDSkill(weaponCooldownImages, weaponTimers[i], weaponCooldowns[i]);
            }
        }
    }

    void CDSkill(Image cooldownImage, float timer, float cooldown)
    {
        cooldownImage.fillAmount = timer / cooldown;
    }

    public void UseWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponTimers.Count) return;

        if (weaponTimers[weaponIndex] <= 0)
        {
            weaponTimers[weaponIndex] = weaponCooldowns[weaponIndex];
            weaponCooldownImages.fillAmount = 1;
            StartCoroutine(EffectSkill(ultimateEffects[weaponIndex], 4f));
        }
    }

    IEnumerator EffectSkill(ParticleSystem skill, float time)
    {
        skill.transform.position = posUltimate.position;
        skill.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        skill.gameObject.SetActive(false);
    }
    public void UpdateUICooldown(Vector3 pos,int weapomImdex, bool updateCooldown)
    {
        UISkillCooldown.transform.localScale = pos;

        if (updateCooldown)
        {
            CDSkill(weaponCooldownImages, weaponTimers[weapomImdex], weaponCooldowns[weapomImdex]);
        }
    }
}
