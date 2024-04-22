using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 基类
public class Topping : MonoBehaviour
{
    public virtual void Initialize() { }
}

// 第一种子类
public class JamTopping : Topping
{
    public JamType jamType;

    public override void Initialize()
    {
        jamType = (JamType)Random.Range(0, 3);
        gameObject.name = jamType.ToString() + " Jam";
    }
}

// 第二种子类
public class NutTopping : Topping
{
    public int nutsCount;

    public override void Initialize()
    {
        nutsCount = Random.Range(2, 6);
        gameObject.name = "Nuts (" + nutsCount + ")";
    }
}

// 第三种子类
public class CrustTopping : Topping
{
    public CrustType crustType;

    public override void Initialize()
    {
        crustType = (CrustType)Random.Range(0, 2);
        gameObject.name = crustType.ToString() + " Crust";
    }
}

