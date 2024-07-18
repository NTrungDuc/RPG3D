using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearGate : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private void OnDisable()
    {
        door.SetActive(true);
    }
}
