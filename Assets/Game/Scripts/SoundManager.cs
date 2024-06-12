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
}
