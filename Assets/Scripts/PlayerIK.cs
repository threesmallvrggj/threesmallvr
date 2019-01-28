using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour {
    public Transform RightHand, LeftHand;
    public Transform Sword, Shield;
    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand.position);
        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand.position);

        Vector3 lef = LeftHand.rotation.eulerAngles;
        Vector3 rig = RightHand.rotation.eulerAngles;

        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Euler(lef));
        animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.Euler(rig));
    }
}
