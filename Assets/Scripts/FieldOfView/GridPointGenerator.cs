/*
 * Made by Bit Assembly
 * 
 * I don't fucking know what licence, you owe me a beer I guess.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FoV
{
    public class GridPointGenerator : MonoBehaviour, IPointCollection
    {
        public Tilemap tileMap;

        Vector2[] points;

        bool changed = true;

        public int PointCount => points.Length;

        void Start()
        {
#if UNITY_EDITOR
            Tilemap.tilemapTileChanged += Tilemap_tilemapTileChanged;
#endif
            // make our own event to hook up with a map changing socket to react to changes in gametime.
        }

        private void Tilemap_tilemapTileChanged(Tilemap arg1, Tilemap.SyncTile[] arg2)
        {
            changed = true;
            // make smarter, local changes only
        }

        void CalculateAll()
        {
            var list = new List<Vector2>();

            var bounds = tileMap.cellBounds;
            for (int x = bounds.min.x; x < bounds.max.x + 1; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y + 1; y++)
                {
                    if (HasPointAt(new Vector3Int(x, y, 0)))
                        list.Add(new Vector2(x + 1, y + 1));
                }
            }
            points = list.ToArray();
        }


        bool HasPointAt(Vector3Int tile)
        {
            bool a, b, c, d;
            a = tileMap.GetTile(tile) != null;
            b = tileMap.GetTile(tile + Vector3Int.right) != null;
            c = tileMap.GetTile(tile + Vector3Int.up) != null;
            d = tileMap.GetTile(tile + new Vector3Int(1, 1, 0)) != null;

            int count = 0;
            if (a)
                count++;
            if (b)
                count++;
            if (c)
                count++;
            if (d)
                count++;

            if (count == 0 || count == 4)
                return false;
            return true;
        }


        public Vector2[] GetPoints()
        {
            if (points == null || changed)
                CalculateAll();
            var list = new Vector2[points.Length];
            System.Array.Copy(points, list, list.Length);
            return list;

        }

        void OnDrawGizmosSelected()
        {
            if (points == null)
                return;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, .1f);
            }
        }

        public Vector2[] GetPointsIn(Bounds bounds)
        {
            if (points == null || changed)
                CalculateAll();
            var list = new List<Vector2>();
            for (int i = 0; i < points.Length; i++)
            {
                if (bounds.Contains(points[i]))
                    list.Add(points[i]);
            }

            return list.ToArray();
        }
    }
}