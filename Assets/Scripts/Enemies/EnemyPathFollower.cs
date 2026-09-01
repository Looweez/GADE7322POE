using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public float speed = 3f;
    private List<Vector3> pathWaypoints;
    private int currentWaypointIndex = 0;

    public void SetupPath(List<Vector3> newPath)
    {
        pathWaypoints = newPath;
        if (pathWaypoints.Count > 0)
        {
            transform.position = pathWaypoints[0];
        }
    }

    private void Update()
    {
        if (pathWaypoints == null || currentWaypointIndex >= pathWaypoints.Count) return;
        Vector3 target = pathWaypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }
}
