using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlayerHandEvent : Photon.PunBehaviour {

    public SteamVR_Input_Sources m_Source;
    public SteamVR_Behaviour_Pose m_TrackedObject;
    
    public SteamVR_Action_Boolean m_Hand_Trigger;


    private bool IsCatching;
    private Rigidbody m_RigidBody;
    private Vector3 StartThrowPos, EndThrowPos;
    private FixedJoint m_CurrentCatchJoint;
    private Rigidbody m_CurrentCatchRigidbody;

    //Photon

    int handID = 0;
    int itemID = 0;

    // Use this for initialization
    void Start()
    {
        //定義目前的手
        m_RigidBody = GetComponent<Rigidbody>();
        
    }
	
	// Update is called once per frame
	void Update () {
        CatchItem();
        if (handID == 0)
        {
            handID = transform.GetComponent<PhotonView>().viewID;
        }
    }

    /// <summary>
    /// 抓取物品
    /// </summary>
    /// <param name="item">物品</param>
    private void CatchItem()
    {



        //如果按下Trigger則抓取物品  
        if (m_Hand_Trigger.GetStateUp(m_Source))
        {

            print(IsCatching);
            if (!IsCatching)
            {
                var cols = Physics.OverlapSphere(transform.position, .2f);
                print(cols.Length);
                Collider item = null;
                foreach (var col in cols)
                {
                    if (!col.transform.root.CompareTag("Item")) continue;
                    if (item == null)
                    {
                        item = col;
                    }
                    else
                    {
                        float disA = Vector3.Distance(transform.position, item.transform.position);
                        float disB = Vector3.Distance(transform.position, col.transform.position);
                        item = disA < disB ? item : col;
                    }
                }
                if (!item)
                    return;
                //增加關節至手掌
                itemID = item.GetComponent<PhotonView>().viewID;
                photonView.RPC("CatchItem_RPC", PhotonTargets.All,itemID,handID);
                /*m_CurrentCatchJoint = item.gameObject.AddComponent<FixedJoint>();
                //m_CurrentCatchJoint.transform.position = transform.position;
                //m_CurrentCatchJoint.transform.rotation = transform.rotation;
                m_CurrentCatchJoint.connectedBody = m_RigidBody;
                m_CurrentCatchRigidbody = item.GetComponent<Rigidbody>();
                m_CurrentCatchRigidbody.transform.SetParent(transform);
                m_CurrentCatchRigidbody.useGravity = false;
                //m_CurrentCatchRigidbody.isKinematic = true;*/

                IsCatching = true;
                //item.tag = "Catched";
            }
            else
                Throw();
        }


    }

    [PunRPC]
    void CatchItem_RPC(int itemID,int handID)
    {
        GameObject item = PhotonView.Find(itemID).gameObject;
        GameObject hand = PhotonView.Find(handID).gameObject;
        m_CurrentCatchJoint = item.gameObject.AddComponent<FixedJoint>();
        //m_CurrentCatchJoint.transform.position = transform.position;
        //m_CurrentCatchJoint.transform.rotation = transform.rotation;
        m_CurrentCatchJoint.connectedBody = hand.GetComponent<Rigidbody>();
        m_CurrentCatchRigidbody = item.GetComponent<Rigidbody>();
        m_CurrentCatchRigidbody.transform.SetParent(hand.transform);
        m_CurrentCatchRigidbody.useGravity = false;
        //m_CurrentCatchRigidbody.isKinematic = true;
        item.tag = "Catched";
    }


    private void Throw()
    {
        if (m_CurrentCatchJoint)
        {
            EndThrowPos = m_CurrentCatchJoint.transform.position;
           /* Destroy(m_CurrentCatchJoint);
            m_CurrentCatchJoint = null;*/
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
            Vector3 orignForce = origin.TransformVector(m_TrackedObject.GetVelocity());
            Vector3 orignAngularForce = origin.TransformVector(m_TrackedObject.GetAngularVelocity());
            photonView.RPC("ThrowItem_RPC", PhotonTargets.All, orignForce, orignAngularForce);
            /*m_CurrentCatchRigidbody.transform.SetParent(null);
            m_CurrentCatchRigidbody.useGravity = true;
            m_CurrentCatchRigidbody.velocity = origin.TransformVector(m_TrackedObject.GetVelocity());
            m_CurrentCatchRigidbody.angularVelocity = origin.TransformVector(m_TrackedObject.GetAngularVelocity());
            m_CurrentCatchRigidbody.isKinematic = false;
            m_CurrentCatchRigidbody.transform.tag = "Item";
            m_CurrentCatchRigidbody = null;*/
        }

        IsCatching = false;
        
    }

    [PunRPC]
    void ThrowItem_RPC(Vector3 orignForce , Vector3 orignAngularForce)
    {
        Destroy(m_CurrentCatchJoint);
        m_CurrentCatchJoint = null;

        m_CurrentCatchRigidbody.transform.SetParent(null);
        m_CurrentCatchRigidbody.useGravity = true;
        m_CurrentCatchRigidbody.velocity = orignForce;
        m_CurrentCatchRigidbody.angularVelocity = orignAngularForce;
        m_CurrentCatchRigidbody.isKinematic = false;
        m_CurrentCatchRigidbody.transform.tag = "Item";
        m_CurrentCatchRigidbody = null;
    }

}
