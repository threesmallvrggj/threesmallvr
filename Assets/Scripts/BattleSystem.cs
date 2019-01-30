using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物傷害判斷及血量
/// </summary>
public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject HpBar;
    [SerializeField]
    private ParticleSystem playerBlood;

    private Animator m_Animator;

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

        m_Animator = GetComponent<Animator>();

        MaxHp = Hp;
	}
	
	void Update ()
    {

	}

    /// <summary>
    /// 當受傷
    /// </summary>
    /// <param name="collision">碰撞物</param>
    private void OnHurt(Collision collision)
    {
        
        if (collision.transform.CompareTag("Item") || collision.transform.CompareTag("Catched"))
        {
            var force = collision.rigidbody.velocity.magnitude;
            print(force);
            if (force > 5f)
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
                UpdateHealthBar();

                //死亡
                if (Hp <= 0)
                {
                    Destroy(HpBar.gameObject);
                    m_Animator.enabled = false;
                    transform.tag = "Item";
                }
            }
        }
    }

    /// <summary>
    /// 更新血條
    /// </summary>
    private void UpdateHealthBar()
    {
        float x = HpBar.transform.localScale.x;
        float y = HpBar.transform.localScale.y;
        float z = HpBar.transform.localScale.z;

        HpBar.transform.localScale= new Vector3( ((float)Hp / MaxHp)*x,y,z);
    }

}
