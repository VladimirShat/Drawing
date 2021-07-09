using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Texture2DManager
{
    public static Texture2D GenerateColorTexture2D(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        
        Color[] textureColors = new Color[texture.width * texture.height];

        for (int i = 0; i < textureColors.Length; i++)
        {
            textureColors[i] = color;
        }

        texture.SetPixels(textureColors);
        texture.Apply();

        return texture;
    }

    public static void SaveTextureAsPNG(Texture2D texture2D, string fileName)
    {
        var data = texture2D.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath + "/" + fileName + ".png", data);
        Debug.Log("saved");
    }
}
