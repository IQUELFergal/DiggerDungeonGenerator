using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap wallOverlayTilemap = null;
    public Tilemap wallTilemap = null;
    public Tilemap groundTilemap = null;
    public Tilemap voidTilemap = null;

    //public TileBase wallTile = null;
    //public TileBase floorTile = null;
    //public TileBase voidTile = null;

    //public Texture2D debugTexture = null;

    //public Vector2Int roomSize = new Vector2Int(16, 16);

    Dictionary<Color, TileBase> ColorToTile;

    void InitDictionary(DiggerDungeonData dungeonData)
    {
        ColorToTile = new Dictionary<Color, TileBase>()
        {
            { Color.clear, dungeonData.voidTile},
            { Color.white, dungeonData.groundTile},
            { Color.black, dungeonData.wallTile}
        };
    }

    /*TileBase GetTileFromColor(Color color)
    {
        return null;
    }*/


    /*[ContextMenu("Generate tilemap")]
    void GenerateTilemap(DungeonData dungeonData)
    {
        InitDictionary(dungeonData);

        Debug.Log("Clearing tilemap.");
        ClearTilemaps();

        Debug.Log("Drawing tilemap...");
        Color[] roomTilemap = MapTextureExtractor.GetTextureData(debugTexture, roomSize.x, roomSize.y, 16);
        DrawRoom(dungeonData, roomTilemap);
    }*/

    public void DrawTilemap(DiggerDungeonData dungeonData, Color[,] tilePos, int xOffset = 0, int yOffset = 0)
    {
        InitDictionary(dungeonData);
        //Debug.Log("Drawing tilemap...");
        for (int y = 0; y < tilePos.GetLength(1); y++)
        {
            for (int x = 0; x < tilePos.GetLength(0); x++)
            {
                //Error
                if (!ColorToTile.ContainsKey(tilePos[x, y]))
                {
                    Debug.LogError("Some tiles are not recognized");
                    return;
                }
                //Ground
                if (ColorToTile[tilePos[x, y]] == dungeonData.groundTile)
                {
                    groundTilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), ColorToTile[tilePos[x, y]]);
                    wallTilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), null);
                }
                //Walls
                else
                {
                    groundTilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), null);
                    wallTilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), ColorToTile[tilePos[x, y]]);
                }
            }
        }
    }

    //Draws a single dungeon room
    public void DrawRoom(DiggerDungeonData dungeonData, Color[] tilePos, int xOffset = 0, int yOffset = 0)
    {
        for (int i = 0; i < tilePos.Length; i++)
        {
            //Error
            if (!ColorToTile.ContainsKey(tilePos[i]))
            {
                Debug.LogError("Some tiles are not recognized");
                return;
            }
            //Ground
            if (ColorToTile[tilePos[i]] == dungeonData.groundTile)
            {
                groundTilemap.SetTile(new Vector3Int(i % dungeonData.roomSize.x + xOffset, i / dungeonData.roomSize.y + yOffset, 0), ColorToTile[tilePos[i]]);
                wallTilemap.SetTile(new Vector3Int(i % dungeonData.roomSize.x + xOffset, i / dungeonData.roomSize.y + yOffset, 0), null);
            }
            //Walls
            else
            {
                groundTilemap.SetTile(new Vector3Int(i % dungeonData.roomSize.x + xOffset, i / dungeonData.roomSize.y + yOffset, 0), null);
                wallTilemap.SetTile(new Vector3Int(i % dungeonData.roomSize.x + xOffset, i / dungeonData.roomSize.y + yOffset, 0), ColorToTile[tilePos[i]]);
            }
        }
    }

    //Calls the DrawRoom to draw the whole dungeon
    public void DrawDungeon(DiggerDungeonData dungeonData, Room[] rooms)
    {
        InitDictionary(dungeonData);
        foreach (var room in rooms)
        {
            Color[] roomTilemap;
            //Color[] roomTilemap = MapTextureExtractor.GetTextureData(debugTexture, roomSize.x, roomSize.y, roomSize.x * room.roomModel, roomSize.y * room.roomLayout);
            switch (room.roomCategory)
            {
                default:
                    Debug.LogError("No category corresponding to " + room.roomCategory);
                    return;

                case Room.RoomCategory.defaultRoom:
                    roomTilemap = MapTextureExtractor.GetTextureData(dungeonData.defaultRooms.roomTexture, dungeonData.roomSize.x, dungeonData.roomSize.y, dungeonData.roomSize.x * room.roomModel, dungeonData.roomSize.y * room.roomLayout);
                    break;

                case Room.RoomCategory.spawnRoom:
                    roomTilemap = MapTextureExtractor.GetTextureData(dungeonData.spawnRooms.roomTexture, dungeonData.roomSize.x, dungeonData.roomSize.y, dungeonData.roomSize.x * room.roomModel, dungeonData.roomSize.y * room.roomLayout);
                    break;

                case Room.RoomCategory.bossRoom:
                    roomTilemap = MapTextureExtractor.GetTextureData(dungeonData.bossRooms.roomTexture, dungeonData.roomSize.x, dungeonData.roomSize.y, dungeonData.roomSize.x * room.roomModel, dungeonData.roomSize.y * room.roomLayout);
                    break;

                case Room.RoomCategory.itemRoom:
                    roomTilemap = MapTextureExtractor.GetTextureData(dungeonData.itemRooms.roomTexture, dungeonData.roomSize.x, dungeonData.roomSize.y, dungeonData.roomSize.x * room.roomModel, dungeonData.roomSize.y * room.roomLayout);
                    break;

            }
            DrawRoom(dungeonData, roomTilemap, dungeonData.roomSize.x * room.position.x, dungeonData.roomSize.y * room.position.y);
        }
    }

    [ContextMenu("Get tilemap info")]
    void GetTilemapInformations()
    {
        Debug.Log("cellBounds :" + wallTilemap.cellBounds.ToString());
        Debug.Log("color :" + wallTilemap.color.ToString());
        Debug.Log("origin :" + wallTilemap.origin.ToString());
        Debug.Log("size :" + wallTilemap.size.ToString());
        Debug.Log("tileAnchor :" + wallTilemap.tileAnchor.ToString());
        Debug.Log("localBounds :" + wallTilemap.localBounds.ToString());
    }

    [ContextMenu("Clear Tilemap")]
    void ClearTilemaps()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
