using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, right, down, left };

public class DiggerDungeonGenerator : MonoBehaviour
{
    Room[] roomArray;
    public DiggerDungeonData dungeonData;

    public bool fillBackground = false;
    [Min(0)] public int outlineWidth;
    Vector2Int backgroundOffset;

    //Positions visited by th diggers
    List<Vector2Int> positionsVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMouvementMap = new Dictionary<Direction, Vector2Int>
    {
        { Direction.up, Vector2Int.up},
        { Direction.right, Vector2Int.right},
        { Direction.down, Vector2Int.down},
        { Direction.left, Vector2Int.left}
    };

    private void Awake()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        //Init seed
        SeedInitializer seedInitializer;
        if (TryGetComponent(out seedInitializer))
        {
            seedInitializer.InitSeed();
        }

        if (dungeonData != null)
        {
            Debug.Log("Generating dungeon with " + dungeonData.ToString());

            Vector2Int[] roomPositions = GenerateRoomPositions();

            //RoomArray creation
            roomArray = CreateRooms(roomPositions);

            roomArray[0].SetCategory(Room.RoomCategory.spawnRoom);
            GetFarthestRoomIndex(roomArray[0]).SetCategory(Room.RoomCategory.bossRoom);

            //Find a way to set different room variation : item, shop etc

            //Set layouts
            RandomizeLayout();

            //Tilemap generation
            TilemapGenerator tilemapGenerator = GetComponent<TilemapGenerator>();
            if (TryGetComponent(out tilemapGenerator))
            {
                if (fillBackground)
                {
                    tilemapGenerator.DrawTilemap(dungeonData, CreateBackground(roomPositions), backgroundOffset.x, backgroundOffset.y);
                }
                tilemapGenerator.DrawDungeon(dungeonData, roomArray);  //Separate this step into 3 => read texture based on given layout, get spawner placement and register it inside Room variable, draw room
            }
        }
    }

    public Vector2Int[] GenerateRoomPositions()
    {
        Digger[] diggers = new Digger[dungeonData.numberOfDiggers];
        Vector2Int startPos = Vector2Int.zero;
        positionsVisited.Add(startPos);
        for (int i = 0; i < diggers.Length; i++)
        {
            diggers[i] = new Digger(startPos);
        }

        int iterations = UnityEngine.Random.Range(dungeonData.minIterations, dungeonData.maxIterations);
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < diggers.Length; j++)
            {
                Vector2Int newPos = diggers[j].Move(directionMouvementMap);
                if (!positionsVisited.Contains(newPos)) positionsVisited.Add(newPos);
            }
        }
        return positionsVisited.ToArray();
    }


    public Room[] CreateRooms(Vector2Int[] positions)
    {
        //Pre initialisation
        Room[] rooms = new Room[positions.Length];

        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i] = new Room(positions[i]);
            rooms[i].SetCategory(Room.RoomCategory.defaultRoom);
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            int roomModel = 0;
            List<Room> connectedRooms = new List<Room>();

            //For each direction
            for (int j = 0; j < directionMouvementMap.Count; j++)
            {
                //Check for neighbor rooms inside the array
                for (int k = 0; k < rooms.Length; k++)
                {
                    //Bitmask the roomType to get the good one
                    //See : https://gamedevelopment.tutsplus.com/tutorials/how-to-use-tile-bitmasking-to-auto-tile-your-level-layouts--cms-25673
                    if (rooms[i].position + directionMouvementMap[(Direction)j] == rooms[k].position)
                    {
                        roomModel += (int)Mathf.Pow(2, j);
                        connectedRooms.Add(rooms[k]);
                    }
                }
            }
            rooms[i].roomModel = roomModel;
            rooms[i].connectedRooms = connectedRooms.ToArray();
        }
        return rooms;
    }

    

    public Color[,] CreateBackground(Vector2Int[] positions)
    {
        int xMin = 0;
        int xMax = 0;
        int yMin = 0;
        int yMax = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].x < xMin)
            {
                xMin = positions[i].x;
            }
            else if (positions[i].x > xMax)
            {
                xMax = positions[i].x;
            }

            if (positions[i].y < yMin)
            {
                yMin = positions[i].y;
            }
            else if (positions[i].y > yMax)
            {
                yMax = positions[i].y;
            }
        }

        xMin -= outlineWidth;
        xMax += outlineWidth;
        yMin -= outlineWidth;
        yMax += outlineWidth;

        int width = xMax - xMin + 1;
        int height = yMax - yMin + 1;

        backgroundOffset = new Vector2Int(xMin * dungeonData.roomSize.x, yMin * dungeonData.roomSize.y);

        Color[,] result = new Color[width * dungeonData.roomSize.x, height * dungeonData.roomSize.y];
        for (int y = 0; y < result.GetLength(1); y++)
        {
            for (int x = 0; x < result.GetLength(0); x++)
            {
                result[x, y] = Color.black;
            }
        }
        return result;
    }

    void RandomizeLayout() //A refaire pour virer le switch
    {
        for (int i = 0; i < roomArray.Length; i++)
        {
            switch (roomArray[i].roomCategory)
            {
                default:
                    break;
                case Room.RoomCategory.defaultRoom:
                    roomArray[i].roomLayout = Random.Range(dungeonData.defaultRooms.minLayout, dungeonData.defaultRooms.maxLayout);
                    break;
                case Room.RoomCategory.spawnRoom:
                    roomArray[i].roomLayout = Random.Range(dungeonData.spawnRooms.minLayout, dungeonData.spawnRooms.maxLayout);
                    break;
                case Room.RoomCategory.bossRoom:
                    roomArray[i].roomLayout = Random.Range(dungeonData.bossRooms.minLayout, dungeonData.bossRooms.maxLayout);
                    break;
                case Room.RoomCategory.itemRoom:
                    roomArray[i].roomLayout = Random.Range(dungeonData.itemRooms.minLayout, dungeonData.itemRooms.maxLayout);
                    break;
            }
        }
    }



    Room GetFarthestRoomIndex(Room startingRoom)
    {
        //Init
        List<Room> roomVisited = new List<Room>();
        roomVisited.Add(startingRoom);
        List<Room> roomToVisit = new List<Room>();
        for (int i = 0; i < startingRoom.connectedRooms.Length; i++)
        {
            roomToVisit.Add(startingRoom.connectedRooms[i]);
        }

        //Recursive stuff
        while(roomToVisit.Count!=0)
        {
            for (int i = 0; i < roomToVisit[0].connectedRooms.Length; i++)
            {
                if(!roomVisited.Contains(roomToVisit[0].connectedRooms[i]))
                {
                    roomToVisit.Add(roomToVisit[0].connectedRooms[i]);
                }
            }
            roomVisited.Add(roomToVisit[0]);
            roomToVisit.RemoveAt(0);
        }
        return roomVisited[roomVisited.Count - 1];
    }
}
