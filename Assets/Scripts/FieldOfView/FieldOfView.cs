/*
 * Made by Bit Assembly
 * 
 * I don't fucking know what licence, you owe me a beer I guess.
 */

using System.Collections.Generic;
using UnityEngine;

namespace FoV
{
    public class FieldOfView : MonoBehaviour
    {
        Mesh mesh;

        public GridPointGenerator PointCollection;

        public LayerMask LosBlock;

        public float maxDistance = 10f;

        int vertCountMemory = 0;

        public List<Vector2> visiblePoints;

        // Start is called before the first frame update
        void Start()
        {
            mesh = new Mesh();

            var filter = GetComponent<MeshFilter>();

            // mesh.SetVertices(GetVertices());
            // mesh.triangles = GetTriangles();
            filter.mesh = mesh;
        }

        private void LateUpdate()
        {
            var verts = GetVertices();
            if (verts.Count != vertCountMemory)
            {
                if (vertCountMemory > verts.Count)
                    mesh.triangles = new int[] { };
                mesh.SetVertices(verts);
                vertCountMemory = verts.Count;
                mesh.triangles = GetTriangles(verts.Count - 1);
            }
            else
                mesh.SetVertices(verts);
        }

        int[] GetTriangles(int count)
        {
            var tris = new int[count * 3]; // 24 * 3 = 72
            for (int i = 0; i < count; i++)
            {
                tris[i * 3] = 0;
                tris[i * 3 + 1] = i + 1;
                tris[i * 3 + 2] = i + 2;
            }
            tris[count * 3 - 3] = 0;
            tris[count * 3 - 1] = 1;
            tris[count * 3 - 2] = count;
            return tris;
        }

        List<Vector3> GetVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(Vector3.zero);

            var points = PointCollection.GetPointsIn(new Bounds(transform.position, new Vector3(maxDistance * 2f, maxDistance * 2f, 1f)));
            for (int i = 0; i < points.Length; i++)
            {
                points[i] -= (Vector2)transform.position;
            }

            var list = new List<Vector2>(points);
            list.Sort(SortPoints);
            visiblePoints.Clear();
            foreach (var point in list)
            {
                float dx = point.y > 0 ? .002f : -.002f;
                float dy = point.x > 0 ? .002f : -.002f;

                Vector2 p1 = new Vector2(point.x - dx, point.y + dy);
                Vector2 p2 = new Vector2(point.x + dx, point.y - dy);


                var hit = Physics2D.Raycast(transform.position, p1, maxDistance, LosBlock);
                if (hit.distance + .1f > point.magnitude)
                    visiblePoints.Add(point + (Vector2)transform.position);
                if (hit)
                    vertices.Add(hit.point - (Vector2)transform.position);
                else
                    vertices.Add(p1.normalized * maxDistance);

                hit = Physics2D.Raycast(transform.position, p2, maxDistance, LosBlock);
                if (hit)
                    vertices.Add(hit.point - (Vector2)transform.position);
                else
                    vertices.Add(p2.normalized * maxDistance);
            }
            return vertices;
        }

        static int SortPoints(Vector2 a, Vector2 b)
        {
            if (a == b)
                return 0;

            return System.Math.Sign(Vector2.SignedAngle(Vector2.right, b) - Vector2.SignedAngle(Vector2.right, a));
        }

    }
}
