using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTextureExtractor
{
    public static Color[] GetTextureData(Texture2D texture, int widthOffset = 0, int heightOffset = 0)
    {
        return GetTextureData(texture, texture.width, texture.height, widthOffset, heightOffset);
    }

    public static Color[] GetTextureData(Texture2D texture, int width, int height, int widthOffset = 0, int heightOffset = 0)
    {
        Color[] result = new Color[width * height];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = texture.GetPixel((i % width) + widthOffset, (i / height) + heightOffset); //Mettre un switch pour plusieurs couleurs
        }
        return result;
    }

    static Color[] FillRectangle(Color color, int width, int height, int widthOffset = 0, int heightOffset = 0)
    {
        Color[] result = new Color[width * height];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = color;
        }
        return result;
    }
}
