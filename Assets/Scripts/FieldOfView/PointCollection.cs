/*
 * Naive sollution for testing, do not use
 */

using System.Collections.Generic;
using UnityEngine;

namespace FoV
{
    public class PointCollection : MonoBehaviour, IPointCollection
    {
        public int PointCount => GetPoints().Length;

        public Vector2[] GetPoints()
        {

            var list = new Vector2[transform.childCount];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = transform.GetChild(i).transform.position;
            }

            return list;
        }

        public Vector2[] GetPointsIn(Bounds bounds)
        {
            var list = new List<Vector2>();
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var p = transform.GetChild(i).transform.position;
                if (bounds.Contains(p))
                    list[i] = p;
            }

            return list.ToArray();
        }
    }
}