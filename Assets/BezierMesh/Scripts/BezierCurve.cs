using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;
    [Range(5, 20)]
    public int tangent_count = 10;

    void OnDrawGizmos()
    {
        var t = this.transform;

        // to world
        var cp_world = new Vector3[4];
        for (var i = 0; i < 4; ++i)
        {
            Gizmos.color = Color.yellow;
            cp_world[i] = t.TransformPoint(points[i]);
            Gizmos.DrawSphere(cp_world[i], 0.1f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cp_world[0], cp_world[1]);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(cp_world[2], cp_world[3]);

        // draw bezier
        var p_count = 100;
        var p = new Vector3[p_count];
        for (var i = 0; i < p_count; ++i)
            p[i] = bezier((float)i / p_count, ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]);
        Gizmos.color = Color.red;
        for (var i = 0; i < p_count - 1; ++i)
            Gizmos.DrawLine(p[i], p[i + 1]);

        // draw tangent
        p = new Vector3[tangent_count];
        for (var i = 0; i < tangent_count; ++i)
            p[i] = bezier((float)i / tangent_count, ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]);

        var helpV = Vector3.up;
        var helpV2 = Vector3.right;
        for (var i = 0; i < tangent_count - 1; ++i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(p[i], 0.1f);
            var Y = bezierTangent((float)i / tangent_count, ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]).normalized;
            Gizmos.DrawLine(p[i], p[i] + Y);

            Gizmos.color = Color.blue;
            var Z = Vector3.Cross(helpV, Y).normalized;
            if(Z==Vector3.zero)
                Z = Vector3.Cross(helpV2, Y).normalized;
            Gizmos.DrawLine(p[i], p[i] +Z);

            Gizmos.color = Color.red;
            var X = Vector3.Cross(Y, Z);
            Gizmos.DrawLine(p[i], p[i] + X);
        }


    }

    Vector3 bezier(float t, ref Vector3 P0, ref Vector3 P1, ref Vector3 P2, ref Vector3 P3)
    {
        float a = (1 - t);
        return P0 * a * a * a + P1 * 3 * t * a * a + P2 * 3 * t * t * a + P3 * t * t * t;
    }

    Vector3 bezierTangent(float t, ref Vector3 P0, ref Vector3 P1, ref Vector3 P2, ref Vector3 P3)
    {
        float t2 = t * t;
        return P0 * (-3 * t2 + 6 * t - 3) + P1 * (9 * t2 - 12 * t + 3) + P2 * (-9 * t2 + 6 * t) + P3 * (3 * t2);
    }
}
