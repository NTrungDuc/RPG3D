using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    [SerializeField] private AudioSource footStepsWalk;
    [SerializeField] private AudioSource footStepsRun;
    [SerializeField] private AudioSource swordSlash;
    //sound boss totoise
    [SerializeField] public AudioSource attackTotoise_1;
    [SerializeField] public AudioSource attackTotoise_2;
    [SerializeField] public AudioSource attackTotoise_3;
    private void Awake()
    {
        instance = this;
    }
    public void soundWalk(bool isWalk)
    {
        footStepsWalk.enabled = isWalk;
    }
    public void soundRun(bool isRun)
    {
        footStepsRun.enabled = isRun;
    }
    public void soundSword(bool isAttack)
    {
        swordSlash.enabled = isAttack;
    }
    public IEnumerator TotoiseAttackAcis()
    {
        yield return new WaitForSeconds(3f);
        attackTotoise_2.Play();
    }
}
