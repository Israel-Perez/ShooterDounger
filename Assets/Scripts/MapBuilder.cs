using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap ground;
    public Tilemap walls;

    [Header("Tiles")]
    public TileBase[] floorTiles;      // varios pisos (con grietas, etc.)
    public TileBase wallTile;          // pared / borde general
    public TileBase wallFrontTile;     // opcional: ladrillo de frente (pared con piso debajo)
    public TileBase barrelTile;        // opcional

    [Header("Jugador")]
    public Transform elf;

    // # pared   . piso   B barril   E inicio del elfo   (espacio = vacio)
    [TextArea(12, 30)]
    public string layout =
@"##################        ##################
#................#        #................#
#..B.............#        #..B.............#
#................##########................#
#..........................................#
#.......E..................................#
#..........................................#
#................##########................#
#..............B.#        #..............B.#
#................#        #................#
############...###        ########...#######
           #...#                 #...#
           #...#                 #...#
           #...#                 #...#
           #...#                 #...#
    ########...###################...####
    #...................................#
    #.B...............................B.#
    #.................B.................#
    #.......##............##............#
    #.......##............##............#
    #...................................#
    #..............##...................#
    #.B.........B.....................B.#
    #...................................#
    #####################################";

    [ContextMenu("Build")]
    void Build()
    {
        string[] rows = layout.Replace("\r", "").Split('\n');
        int height = rows.Length;

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(ground, "Build map");
        UnityEditor.Undo.RecordObject(walls, "Build map");
#endif
        ground.ClearAllTiles();
        walls.ClearAllTiles();

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                char ch = rows[r][c];
                if (ch == ' ') continue;

                // la primera linea del texto es la fila de arriba
                var pos = new Vector3Int(c, height - 1 - r, 0);

                if (ch == '#')
                {
                    // si debajo hay piso, usa el ladrillo de frente
                    bool floorBelow = r + 1 < height && c < rows[r + 1].Length && rows[r + 1][c] != '#' && rows[r + 1][c] != ' ';
                    walls.SetTile(pos, (floorBelow && wallFrontTile != null) ? wallFrontTile : wallTile);
                }
                else
                {
                    ground.SetTile(pos, floorTiles[Random.Range(0, floorTiles.Length)]);

                    if (ch == 'B' && barrelTile != null)
                        walls.SetTile(pos, barrelTile);

                    if (ch == 'E' && elf != null)
                        elf.position = ground.GetCellCenterWorld(pos);
                }
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(ground);
        UnityEditor.EditorUtility.SetDirty(walls);
#endif
    }
}
