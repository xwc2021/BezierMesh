using UnityEngine;
using UnityEditor;

namespace Mytool {
    [CustomEditor(typeof(BezierData))]
    public class DrawBezier : Editor
    {
        private void OnSceneViewGUI(SceneView sv)
        {
            BezierData be = target as BezierData;

            Tools.hidden = be.isEditMode;
            if (be.isEditMode)
            {
                var cnPoints =be.cnPoints;
                var t = be.transform;

                var segment_count = cnPoints.Count / 3;
                var center_index = 1;
                for (var i = 0; i < segment_count; ++i) {
                    //中間動(P1):一起動
                    var old_P1 = t.TransformPoint(cnPoints[center_index]);
                    var new_P1 =Handles.PositionHandle(old_P1, Quaternion.identity);
                    var diff_P1 = new_P1 - old_P1;
                    cnPoints[center_index] = t.InverseTransformPoint(new_P1);

                    var old_P0= diff_P1 + t.TransformPoint(cnPoints[center_index-1]);
                    var old_P2 = diff_P1 + t.TransformPoint(cnPoints[center_index + 1]);

                    //2端點動(P0,P2):另1個端點到中點的長度不變
                    var new_P0 = Handles.PositionHandle(old_P0, Quaternion.identity);
                    var new_P2 = Handles.PositionHandle(old_P2, Quaternion.identity);
                    if (old_P0 != new_P0)
                    {
                        var dir =(new_P1 - new_P0).normalized;
                        var old_length = (old_P2 - old_P1).magnitude;
                        new_P2 = new_P1 + dir * old_length;
                    }
                    else if (old_P2 != new_P2)
                    {
                        var dir = (new_P1 - new_P2).normalized;
                        var old_length = (old_P0 - old_P1).magnitude;
                        new_P0 = new_P1 + dir * old_length;
                    }

                    cnPoints[center_index-1] = t.InverseTransformPoint(new_P0);
                    cnPoints[center_index+1] = t.InverseTransformPoint(new_P2);

                    center_index += 3;
                }
                /*
                be.startPoint = t.InverseTransformPoint(Handles.PositionHandle(t.TransformPoint(be.startPoint), Quaternion.identity));
                be.endPoint = t.InverseTransformPoint(Handles.PositionHandle(t.TransformPoint(be.endPoint), Quaternion.identity));
                be.startTangent = t.InverseTransformPoint(Handles.PositionHandle(t.TransformPoint(be.startTangent), Quaternion.identity));
                be.endTangent = t.InverseTransformPoint(Handles.PositionHandle(t.TransformPoint(be.endTangent), Quaternion.identity));
                */
            }
        }

        void OnEnable()
        {
            //Debug.Log("OnEnable");
            SceneView.duringSceneGui += OnSceneViewGUI;
        }

        void OnDisable()
        {
            //Debug.Log("OnDisable");
            SceneView.duringSceneGui -= OnSceneViewGUI;
        }
    }
}
