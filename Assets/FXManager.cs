using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour {
    public GameObject[] FXArray;
    private Queue<GameObject>[] FXPool;
    private Dictionary<string, int> FXDictionary;
    public static FXManager _Instance;
	// Use this for initialization
	void Start () {
        Init();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Init()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this.gameObject);

        FXDictionary = new Dictionary<string, int>();
        FXPool = new Queue<GameObject>[FXArray.Length];
        for (int i = 0; i < FXPool.Length; i++)
        {
            FXPool[i] = new Queue<GameObject>();
            var fx = Instantiate(FXArray[i]);
            FXPool[i].Enqueue(fx);
            fx.SetActive(false);
            FXDictionary.Add(fx.name, i);
        }
    }
    public GameObject GetFX(string fxName)
    {
        int id=0;
        if(FXDictionary.TryGetValue(fxName, out id))
        {
            GameObject fx;
            if (FXPool[id].Count > 0)
            {
                fx = FXPool[id].Dequeue();
            }
            else
            {
                fx = Instantiate(FXArray[id]);
            }
            fx.SetActive(true);
            return fx;
        }
        return null;
    }

    public void RecoveryFX(GameObject fx)
    {
        int id = 0;
        if (FXDictionary.TryGetValue(fx.name, out id))
        {
            FXPool[id].Enqueue(fx);
        }
    }
}

