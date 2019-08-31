using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
        Gizmos.color = Color.white;
        for (var i = 0; i < p_count - 1; ++i)
            Gizmos.DrawLine(p[i], p[i + 1]);

        // draw tangent
        p = new Vector3[tangent_count];
        for (var i = 0; i < tangent_count; ++i)
            p[i] = bezier((float)i / tangent_count, ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]);

        var helpV = getBestHelpV(ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]);
        for (var i = 0; i < tangent_count - 1; ++i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(p[i], 0.1f);
            var Y = bezierTangent((float)i / tangent_count, ref cp_world[0], ref cp_world[1], ref cp_world[2], ref cp_world[3]).normalized;
            Gizmos.DrawLine(p[i], p[i] + Y);

            Gizmos.color = Color.blue;
            var Z = Vector3.Cross(helpV, Y).normalized;
            Gizmos.DrawLine(p[i], p[i] + Z);

            Gizmos.color = Color.red;
            var X = Vector3.Cross(Y, Z);
            Gizmos.DrawLine(p[i], p[i] + X);
        }
    }

    Vector3 getOrthogonalVector(ref Vector3 v)
    {
        var v_orthogonal = Vector3.Cross(v, Vector3.up);
        if (v_orthogonal.magnitude > float.Epsilon)
            return v_orthogonal.normalized;

        v_orthogonal = Vector3.Cross(v, Vector3.right);
        if (v_orthogonal.magnitude > float.Epsilon)
            return v_orthogonal.normalized;

        v_orthogonal = Vector3.Cross(v, Vector3.forward);
        return v_orthogonal.normalized;
    }

    Vector3 getBestHelpV(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3)
    {
        var center0 = (p0 + p1) * 0.5f;
        var center1 = (p2 + p3) * 0.5f;

        var linkV = center1 - center0;

        var v0 = p1 - p0;
        var v0_orthogonal = Vector3.Cross(linkV, v0).normalized;

        var v1 = p2 - p3;
        var v1_orthogonal = Vector3.Cross(linkV, v1).normalized;

        //2個othogonal的中間值
        var helpV = 0.5f * (v0_orthogonal + v1_orthogonal);

        // 當p0==p1 而且 p2==p3
        //找出垂直此線段的向量
        if (helpV.magnitude < float.Epsilon)
            helpV = getOrthogonalVector(ref linkV);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(center0, center0 + linkV);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(center0, center0 + helpV);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(center0, center0 + v0_orthogonal);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center1, center1 + v1_orthogonal);

        return helpV;
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
