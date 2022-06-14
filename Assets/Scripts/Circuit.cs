using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public GameObject[] wayPoints;

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }
        
    private void DrawGizmos(bool selected)
    {
        if (!selected) return;
        if(wayPoints.Length > 1)
        {
            Vector3 prev = wayPoints[0].transform.position;
            for (int i = 1; i < wayPoints.Length; i++)
            {
                Vector3 next = wayPoints[i].transform.position;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
            Gizmos.DrawLine(prev, wayPoints[0].transform.position);
        }
    }
}
