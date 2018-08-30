using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GunModel_S {

    private static GunModel_S instance = null;
    public static GunModel_S Instance
    {
        get
        {
            if(instance == null)
            {
                //instance = new GunModel_S();
                TextAsset ta = Resources.Load<TextAsset>("Json/static_weapon");
                instance = JsonUtility.FromJson<GunModel_S>(ta.text);
            }
            return instance;
        }
    }

    public GunModel_S() {  }
    
    [SerializeField]
    private List<GunRow> gunData;


    /// <summary>
    /// 通过id查询枪信息的方法
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GunRow GetGunInfo(int id)
    {
        for (int i = 0; i < gunData.Count; i++)
        {
            if (gunData[i].ID == id)
            {
                return gunData[i].GetGunRow(gunData[i]);
            }
        }
        return null;
    }
	
}
