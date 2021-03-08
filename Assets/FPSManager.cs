using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
    }
}
