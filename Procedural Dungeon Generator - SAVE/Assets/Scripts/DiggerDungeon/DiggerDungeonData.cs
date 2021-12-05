using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New DiggerDungeonData", menuName = "ScriptableObjects/DiggerDungeonData", order = 1)]
public class DiggerDungeonData : ScriptableObject
{
    [Header("Diggers")]
    public int numberOfDiggers = 1;

    [Header("Iterations")]
    public int minIterations = 1;
    public int maxIterations = 1;

    [Header("Tiles")]
    public TileBase groundTile = null;
    public TileBase voidTile = null;
    public TileBase wallTile = null;
    public TileBase wallOverlayTile = null;

    [Header("Room layouts")]
    public Vector2Int roomSize = Vector2Int.zero;
    public RoomLayouts defaultRooms;
    public RoomLayouts spawnRooms;
    public RoomLayouts bossRooms;
    public RoomLayouts itemRooms;

    [System.Serializable]
    public class RoomLayouts
    {
        [Min(0)] public int minLayout = 0;
        [Min(1)] public int maxLayout = 1;
        public Texture2D roomTexture = null;

        
    }

    public override string ToString() => this.name + " (" + numberOfDiggers+ " diggers), Size :" + roomSize.x + " x " + roomSize.y + ", Iteration range : (" + minIterations + "," + maxIterations + ")";

    private void OnValidate()
    {
        if (defaultRooms.minLayout >= defaultRooms.maxLayout) defaultRooms.maxLayout = defaultRooms.minLayout + 1;
        if (spawnRooms.minLayout >= spawnRooms.maxLayout) spawnRooms.maxLayout = spawnRooms.minLayout + 1;
        if (bossRooms.minLayout >= bossRooms.maxLayout) bossRooms.maxLayout = bossRooms.minLayout + 1;
        if (itemRooms.minLayout >= itemRooms.maxLayout) itemRooms.maxLayout = itemRooms.minLayout + 1;
    }
}
