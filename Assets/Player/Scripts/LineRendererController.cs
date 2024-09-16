using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] List<LineRenderer> lines = new(); // List<T> = dynamic size
    public void CreateLine(Transform startPoint, Transform endPoint)
    {
        // If a line exists
        if (lines.Count > 0)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                // At least need 2 point to create a line
                if (lines[i].positionCount >= 2)
                {
                    float startOffsetY = 4f; // 4f
                    float endOffsetY = 1.5f; // 1.5f
                    lines[i].SetPosition(0, startPoint.transform.position + Vector3.up * startOffsetY);
                    lines[i].SetPosition(1, endPoint.transform.position + Vector3.up * endOffsetY);
                }
                else Debug.Log("Need at least 2 positions");
            }
        }
        else Debug.Log("The line list is empty");
    }
}
