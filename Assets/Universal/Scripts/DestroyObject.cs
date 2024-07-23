using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void DestroyInAnimation(GameObject obj) 
    {
        Destroy(obj, 0.5f);
    }
}
