using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockCursor : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
