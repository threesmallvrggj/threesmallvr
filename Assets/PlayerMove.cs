using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public Transform CameraTrans;
    public float Zoff;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SetUp();

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
}
