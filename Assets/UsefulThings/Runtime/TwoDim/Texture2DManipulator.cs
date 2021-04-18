using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UsefulThings.TwoDim
{
    public class Texture2DManipulator
    {
        private Texture2D mainTexture;

        private Texture2DManipulator()
        {
        }
        
        public Texture2DManipulator PutTextureOn(Texture2D textureToPaste, int xPos, int yPos)
        {
            for (int i = 0; i < textureToPaste.width; i++)
            {
                for (int j = 0; j < textureToPaste.height; j++)
                {
                    Color pixel = textureToPaste.GetPixel(i, j);

                    if (pixel.a > 0)
                    {
                        int targetXPos = i + xPos;
                        int targetYPos = j + yPos;
                    
                        if (targetXPos >= 0 && targetXPos < mainTexture.width 
                                            && targetYPos >= 0 && targetYPos < mainTexture.height)
                        {
                            mainTexture.SetPixel(targetXPos, targetYPos, pixel);
                        }
                    }
                }
            }
            return this;
        }

        public Texture2DManipulator Combine(params Texture2D[] textures)
        {
            return Combine((IEnumerable<Texture2D>) textures);
        }

        public Texture2DManipulator Combine(IEnumerable<Texture2D> textures)
        {
            IEnumerable<Texture2D> allTextures = textures.Prepend(mainTexture);
            Vector2Int maxSize = GetMaxSize(allTextures);
            Color32[][] textureColors = allTextures.Select(tex => tex.GetPixels32()).ToArray();
            Color32[] newColors = new Color32[maxSize.x * maxSize.y];

            for (int i = 0; i < newColors.Length; i++)
            {
                foreach (Color32[] colors in textureColors)
                {
                    if (i < colors.Length)
                    {
                        Color32 color = colors[i];
                    
                        if (color.a > 0)
                        {
                            newColors[i] = color;
                            // Force solid colors, ignore transparency
                            newColors[i].a = 255;
                        }
                    }
                }
            }
            
            mainTexture = new Texture2D(maxSize.x, maxSize.y);
            mainTexture.SetPixels32(newColors);
            return this;
        }

        public Texture2D ApplyChanges()
        {
            mainTexture.Apply();
            return mainTexture;
        }

        public static Texture2DManipulator From(Texture2D texture)
        {
            Texture2DManipulator manipulator = new Texture2DManipulator();
            manipulator.mainTexture = texture;
            return manipulator;
        }

        public static Texture2DManipulator Copy(Texture2D texture)
        {
            Texture2DManipulator manipulator = new Texture2DManipulator();
            Texture2D copyTexture = new Texture2D(texture.width, texture.height);
            copyTexture.SetPixels32(texture.GetPixels32());
            manipulator.mainTexture = copyTexture;
            return manipulator;
        }

        public static Texture2DManipulator Empty(int width = 1, int height = 1)
        {
            Texture2DManipulator manipulator = new Texture2DManipulator();
            manipulator.mainTexture = new Texture2D(width, height);
            return manipulator;
        }

        public static Texture2D CreateColoredTexture(Color color, int width = 1, int height = 1)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            var colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            
            texture.SetPixels(colors);
            return texture;
        }
        
        public static Vector2Int GetMaxSize(params Texture2D[] textures)
        {
            return GetMaxSize((IEnumerable<Texture2D>) textures);
        }
        
        public static Vector2Int GetMaxSize(IEnumerable<Texture2D> textures)
        {
            Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);

            foreach (Texture2D texture in textures)
            {
                max.x = Math.Max(max.x, texture.width);
                max.y = Math.Max(max.y, texture.height);
            }

            return max;
        }

        public static uint GetColor32ManhattanDistance(Color32 color)
        {
            return (uint) color.r + color.g + color.b;
        }
    }
}