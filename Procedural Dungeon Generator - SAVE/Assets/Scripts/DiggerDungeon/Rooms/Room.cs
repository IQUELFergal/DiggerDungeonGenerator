using UnityEngine;

public class Room
{
    public RoomCategory roomCategory;
    public Vector2Int position;
    [Range(0,15)] public int roomModel;
    [Min(0)] public int roomLayout;
    [HideInInspector] public Room[] connectedRooms;

    public Room(Vector2Int pos, int rModel = 0, int rLayout = 0, RoomCategory rCategory = RoomCategory.defaultRoom)
    {
        position = pos;
        roomModel = rModel;
        roomLayout = rLayout;
        roomCategory = rCategory;
    }

    public RoomCategory SetCategory(RoomCategory rCategory)
    {
        roomCategory = rCategory;
        return rCategory;
    }

    public enum RoomCategory { defaultRoom, spawnRoom, bossRoom, itemRoom};

    public Room[] SetConnectedRooms(Room[] cRooms)
    {
        connectedRooms = cRooms;
        return cRooms;
    }
}
