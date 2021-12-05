using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{

    // SOURCE : https://www.youtube.com/watch?v=xYOG8kH2tF8

    public int width;
    public int height;

    [Range(0,100)]public int randomFillPercent;

    public int smoothIterations = 5;

    int[,] map;

    private void Start()
    {
        GenerateCave();
    }

    private void GenerateCave()
    {
        Debug.Log("Generating dungeon...");

        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothIterations; i++)
        {
            SmoothMap();
        }

        TilemapGenerator gen = GetComponent<TilemapGenerator>();
        //gen.DrawTilemap(map, width, height);                                 /!\ REFAIRE DRAWTILEMAP POUR QU'ELLE MATCH AVEC UN DUNGEONDATA
    }

    void RandomFillMap()
    {
        /*for (int i = 0; i < map.Length; i++)
        {
            map[i] = (Random.Range(0, 100) < randomFillPercent ? 1 : 0);
        }*/
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = (Random.Range(0, 100) < randomFillPercent ? 1 : 0);
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if(neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else wallCount++;
            }
        }
        return wallCount;
    }

    int[] Convert2DTo1D(int[,] array2D)
    {
        int[] result = new int[array2D.Length];
        int i = 0;
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                result[i++] = array2D[x, y];
            }
        }
        return result;
    }
}
