using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        var t = this.transform;

        var cp = new Vector3[4];
        for (var i = 0; i < 4; ++i)
        {
            Gizmos.color = Color.yellow;
            cp[i] = t.TransformPoint(points[i]);
            Gizmos.DrawSphere(cp[i], 0.1f);
        }

        int len = 100;
        var p = new Vector3[len];
        for (var i = 0; i < len; ++i)
        {
            p[i] = bezier((float)i / len, ref cp[0], ref cp[1], ref cp[2], ref cp[3]);
        }
        for (var i = 0; i < len - 1; ++i)
        {
            Gizmos.DrawLine(p[i], p[i + 1]);
        }


    }

    Vector3 bezier(float t,ref Vector3 P0,ref Vector3 P1, ref Vector3 P2, ref Vector3 P3) {
        float a=(1 - t);
        return P0 * a * a * a + P1 * 3 * t * a * a + P2 * 3 * t * t * a + P3 * t * t * t;
    }
   }
