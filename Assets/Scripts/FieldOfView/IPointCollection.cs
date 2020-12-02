using UnityEngine;

namespace FoV
{
    public interface IPointCollection
    {
        Vector2[] GetPoints();
        int PointCount { get; }
        Vector2[] GetPointsIn(Bounds bounds);
    }
}