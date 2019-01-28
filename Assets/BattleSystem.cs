using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物傷害判斷及血量
/// </summary>
public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSlider;
    [SerializeField]
    private ParticleSystem playerBlood;

    public int Hp=200;
    private int MaxHp;

	void Start ()
    {
        var cols =  GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.gameObject.AddComponent<HurtDetector>();
        }
        HurtDetector. OnHurtEvent += OnHurt;
        MaxHp = Hp;
	}
	
	void Update ()
    {

	}
    private void OnHurt(Collision collision)
    {
        if (collision.transform.CompareTag("Item") || collision.transform.CompareTag("Catched"))
        {
            var force = collision.rigidbody.velocity.magnitude;
            print(force);
            if (force > 1f)
            {
                //扣血
                Hp -= (int)force;
                //播放特效
                var contacts= collision.contacts;
                foreach (var contact in contacts)
                {
                   var blood= Instantiate(playerBlood, contact.point, Quaternion.identity);
                    blood.transform.LookAt(transform);
                    blood.Play();                   
                }
                UpdateHealthSlider();
            }
        }
    }

    /// <summary>
    /// 更新血條
    /// </summary>
    private void UpdateHealthSlider()
    {
        playerSlider.transform.localScale= new Vector3( (Hp / MaxHp)*0.2f,0.01f,0.01f);
    }

}
