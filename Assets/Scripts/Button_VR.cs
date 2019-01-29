using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button_VR : MonoBehaviour {
    [SerializeField]
    private UnityEvent OnTouch;
    
    private void OnTriggerEnter(Collider other)
    {
        print("touched");
        OnTouch.Invoke();
    }
}
