﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button_VR : MonoBehaviour {

    [SerializeField]
    private UnityEvent OnTouch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        OnTouch.Invoke();
        print("Touched");
    }
}
