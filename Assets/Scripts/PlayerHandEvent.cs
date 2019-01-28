using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlayerHandEvent : MonoBehaviour {

    public SteamVR_Input_Sources m_Source;
    public SteamVR_Behaviour_Pose m_TrackedObject;
    
    public SteamVR_Action_Boolean m_Hand_Trigger;


    private bool IsCatching;
    private Rigidbody m_RigidBody;
    private Vector3 StartThrowPos, EndThrowPos;
    private FixedJoint m_CurrentCatchJoint;
    private Rigidbody m_CurrentCatchRigidbody;

    // Use this for initialization
    void Start()
    {
        //定義目前的手
        m_RigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
    }
    private void OnTriggerStay(Collider other)
    {
        CatchItem(other);
    }

    /// <summary>
    /// 抓取物品
    /// </summary>
    /// <param name="item">物品</param>
    private void CatchItem(Collider item)
    {

        if (!item.CompareTag("Item")) return;


        //如果按下Trigger則抓取物品  //如果正在抓取則返回
        if (m_Hand_Trigger.GetStateUp(m_Source))
        {
            if (!IsCatching)
            {
                //增加關節至手掌
                m_CurrentCatchJoint = item.gameObject.AddComponent<FixedJoint>();
                //m_CurrentCatchJoint.transform.position = transform.position;
                //m_CurrentCatchJoint.transform.rotation = transform.rotation;
                m_CurrentCatchJoint.connectedBody = m_RigidBody;
                m_CurrentCatchRigidbody = item.GetComponent<Rigidbody>();
                m_CurrentCatchRigidbody.transform.SetParent(transform);
                m_CurrentCatchRigidbody.useGravity = false;

                IsCatching = true;
            }
            else
                Throw();
        }


    }
    private void Throw()
    {
        if (m_CurrentCatchJoint)
        {
            EndThrowPos = m_CurrentCatchJoint.transform.position;


            Destroy(m_CurrentCatchJoint);
            m_CurrentCatchRigidbody.transform.SetParent(null);
            m_CurrentCatchRigidbody.useGravity = true;
            m_CurrentCatchJoint = null;
        }
        if (m_CurrentCatchRigidbody)
        {
            Transform origin;
            if (m_TrackedObject.origin)
            {
                origin = m_TrackedObject.origin;
            }
            else
            {
                origin = m_TrackedObject.transform.parent;
            }

            m_CurrentCatchRigidbody.velocity = origin.TransformVector(m_TrackedObject.GetVelocity());
            m_CurrentCatchRigidbody.angularVelocity = origin.TransformVector(m_TrackedObject.GetAngularVelocity());
            m_CurrentCatchRigidbody = null;
        }

        IsCatching = false;
    }
}
