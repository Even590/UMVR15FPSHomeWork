using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatorlPath : MonoBehaviour
{
    //WatPoint的半徑
    [SerializeField] float WayPointGizmosRadius = 0.4f;

    //取得下一個WayPoint的編號
    public int GetNextWayPointNumble(int WayPointNumber)
    {
        if(WayPointNumber  +1 > transform.childCount -1)
        {
            return 0;
        }
        return WayPointNumber +1;
    }

    //取得WayPoint的位置
    public Vector3 GetWayPointPosition(int WayPointNumber)
    {
        return transform.GetChild(WayPointNumber).position;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.green;
            int j = GetNextWayPointNumble(i);
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            Gizmos.DrawSphere(GetWayPointPosition(i),WayPointGizmosRadius);
        }
    }
}
