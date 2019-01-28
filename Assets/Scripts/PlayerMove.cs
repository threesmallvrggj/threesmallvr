using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMove : MonoBehaviour {
    public SteamVR_Action_Boolean TouchPad;
    public SteamVR_Action_Vector2 TouchPadAxis;
    public SteamVR_Input_Sources Sources;
    public Transform CameraTrans;

    public float Zoff;
    public float MoveSpeed=2f;
    private Animator m_Animator;
	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        SetUp();
        Move();
    }
    private void SetUp()
    {
        var x = CameraTrans.position.x;
        var z = CameraTrans.position.z;

        transform.position = new Vector3(x, transform.position.y, z - Zoff);

        float y = CameraTrans.rotation.eulerAngles.y;
        var eul = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eul.x,y,eul.z);
    }
    private void Move()
    {
        if (TouchPad.GetState(Sources))
        {
            var axis = TouchPadAxis.GetAxis(Sources);
            if (axis.x < 0 && axis.y < 0.5f && axis.y > -0.5f)
            {
                transform.root.position += -transform.right * MoveSpeed * Time.deltaTime;
            }
            if (axis.x > 0 && axis.y < 0.5f && axis.y > -0.5f)
            {
                transform.root.position += transform.right * MoveSpeed * Time.deltaTime;
            }
            if (axis.y > 0 && axis.x < 0.5f && axis.x > -0.5f)
            {
                transform.root.position += transform.forward * MoveSpeed * Time.deltaTime;
            }
            if (axis.y < 0 && axis.x < 0.5f && axis.x > -0.5f)
            {
                transform.root.position += -transform.forward * MoveSpeed * Time.deltaTime;
            }
            m_Animator.SetBool("Walk", true);
        }
        else
            m_Animator.SetBool("Walk", false);
    }
}
