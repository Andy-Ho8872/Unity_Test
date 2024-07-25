using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void DestroyInAnimation(GameObject obj)
    {
        // Only delete the object(Clone) that exists in the scene
        Destroy(GameObject.Find($"{obj.name}(Clone)"));
    }
}
