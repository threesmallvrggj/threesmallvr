using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HurtDetector : MonoBehaviour
{
    public static Action<Collision> OnHurtEvent;


    private void OnCollisionEnter(Collision collision)
    {
        OnHurtEvent(collision);
    }

}
