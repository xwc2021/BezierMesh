using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Mytool
{
    public class BezierData : MonoBehaviour
    {
        public List<Vector3> cnPoints;
        public bool isEditMode = false;
        public GameObject prefab;
        public Transform place_holder;
        public int lineSegmentIndex;

        void OnValidate()
        {
            updateMaterialProperty();
        }

        public void updateMaterialProperty() {
            var bezier_count = cnPoints.Count / 3 - 1;
            var start_index = 1;
            var light_dir_world = new Vector3(0, 1, 0);
            var light_dir_local = transform.InverseTransformVector(light_dir_world);
            for (var i = 0; i < bezier_count; ++i)
            {
                Vector3 P0 = cnPoints[start_index];
                Vector3 P1 = cnPoints[start_index + 1];
                Vector3 P2 = cnPoints[start_index + 2];
                Vector3 P3 = cnPoints[start_index + 3];
                var helpV = BezierCurve.getBestHelpV(ref P0, ref P1, ref P2, ref P3);

                var propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetVector("_P0", P0);
                propertyBlock.SetVector("_P1", P1);
                propertyBlock.SetVector("_P2", P2);
                propertyBlock.SetVector("_P3", P3);
                propertyBlock.SetVector("_helpV", helpV);
                propertyBlock.SetVector("_lightDir", light_dir_local);
                var obj = place_holder.GetChild(i).gameObject;
                var mesh_render = obj.GetComponent<MeshRenderer>();
                mesh_render.SetPropertyBlock(propertyBlock);

                start_index += 3;
            }
        }

        public void addMesh()
        {
            // 清空子元素(如果之前生成過的話)
            var child_count = place_holder.childCount;
            for (var i = 0; i < child_count; ++i)
                DestroyImmediate(place_holder.GetChild(0).gameObject);

            // 生成
            var bezier_count = cnPoints.Count / 3 - 1;
            for (var i = 0; i < bezier_count; ++i)
                GameObject.Instantiate<GameObject>(prefab, transform.position, transform.rotation, place_holder);

            updateMaterialProperty();
        }

        public void addLineSegment()
        {
            if (cnPoints.Count == 0)
            {
                cnPoints.AddRange(new Vector3[3] { new Vector3(0, 0, -1), new Vector3(0, 0, 0), new Vector3(0, 0, 1) });
            }
            else
            {
                var P0 = cnPoints[cnPoints.Count - 3] + new Vector3(5, 0, 0);
                var P1 = cnPoints[cnPoints.Count - 2] + new Vector3(5, 0, 0);
                var P2 = cnPoints[cnPoints.Count - 1] + new Vector3(5, 0, 0);
                cnPoints.AddRange(new Vector3[3] { P0, P1, P2 });
            }
        }

        private void OnDrawGizmos()
        {
            // cnPoints : 012 345 678 abc 
            // beziers : 1234 4567 78ab
            var bezier_count = cnPoints.Count / 3 - 1;


            var cnPoints_world = cnPoints.ToArray();

            //local to world
            for (var i = 0; i < cnPoints_world.Length; ++i)
                cnPoints_world[i] = transform.TransformPoint(cnPoints_world[i]);

            //draw bezier
            var start_index = 1;
            for (var i = 0; i < bezier_count; ++i)
            {
                Handles.DrawBezier(cnPoints_world[start_index],
               cnPoints_world[start_index + 3],
               cnPoints_world[start_index + 1],
               cnPoints_world[start_index + 2], Color.yellow, null, 2f);

                start_index += 3;
            }

            //draw point
            for (var i = 0; i < cnPoints.Count; ++i)
            {
                Gizmos.DrawSphere(cnPoints_world[i], 0.1f);
            }

            //draw line segment
            var line_segment_count = cnPoints.Count / 3;
            var line_start_index = 0;
            for (var i = 0; i < line_segment_count; ++i)
            {
                Gizmos.DrawLine(cnPoints_world[line_start_index], cnPoints_world[line_start_index + 2]);
                line_start_index += 3;
            }
        }
    }
}
