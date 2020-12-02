/*
 * Made by Bit Assembly
 * 
 * I don't fucking know what licence, you owe me a beer I guess.
 */

using UnityEngine;
using UnityEngine.Tilemaps;

namespace FoV
{
    public class FogOfWar : MonoBehaviour
    {
        public Tile solidTile;
        public Tilemap fog;
        public Tilemap ground;
        public FieldOfView FoV;
        public LayerMask blockLOS;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            foreach (var point in FoV.visiblePoints)
            {
                var remove = new Vector3Int[4];
                remove[0] = new Vector3Int((int)point.x, (int)point.y, 0);
                remove[1] = new Vector3Int((int)point.x - 1, (int)point.y, 0);
                remove[2] = new Vector3Int((int)point.x - 1, (int)point.y - 1, 0);
                remove[3] = new Vector3Int((int)point.x, (int)point.y - 1, 0);
                fog.SetTiles(remove, new TileBase[] { null, null, null, null });
            }

            int bx = Mathf.RoundToInt(FoV.transform.position.x);
            int by = Mathf.RoundToInt(FoV.transform.position.y);
            for (int x = bx - 20; x <= bx + 20; x++)
            {
                for (int y = by - 20; y <= by + 20; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    if (ground.GetTile(pos) == null)
                    {
                        if (fog.GetTile(pos) != null)
                        {
                            var hit = Physics2D.Linecast(FoV.transform.position, new Vector2(x + .5f, y + .5f), blockLOS);
                            if (!hit)
                                fog.SetTile(pos, null);
                        }
                    }
                }
            }
        }
    }
}