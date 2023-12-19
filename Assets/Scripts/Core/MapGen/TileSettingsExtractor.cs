using Assets.Scripts.Core.Data;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts.Core.MapGen
{
    public static class TileSettingsExtractor
    {
        public static TileSettings[] Extract(TileData[] data, Sprite[] sprites)
        {
            var result = new List<TileSettings>();
            for (int i = 0; i < data.Length; i++)
            {
                byte rotations = 0;
                if (data[i].rotations.HasValue)
                {
                    rotations = data[i].rotations.Value;
                }
                var sprite = sprites[data[i].index];
                for (byte j = 0; j < rotations + 1; j++)
                {
                    result.Add(new TileSettings(
                    data[i].weight,
                    GetDirectionHashes(data[i].connection, j),
                    sprite,
                    j,
                    data[i].noselfconnect,
                    data[i].walkable.HasValue ? data[i].walkable.Value : true,
                    data[i].objectID.HasValue ? data[i].objectID.Value : (short)-1,
                    data[i].objectScale.HasValue ? data[i].objectScale.Value : 1f));
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].SetConnectionRules(GetConnections(result[i], result));
            }
            return result.ToArray();
        }
        private static int[] GetDirectionHashes(byte[] connection, byte rotation)
        {
            int[] hashes = new int[4];

            if (connection != null)
            {
                for (byte i = 0; i < hashes.Length; i++)
                {
                    var dir = i + rotation;
                    if (dir > 3)
                        dir -= 4;

                    hashes[i] = connection[dir];
                }
            }
            
            return hashes;
        }
        /*private static int[] GetDirectionHashes(Sprite sprite, byte rotation)
        {
            int[] hashes = new int[4];
            for (byte i = 0; i < hashes.Length; i++)
            {
                hashes[i] = GetColorAtDirection(i, sprite, rotation).GetHashCode();
            }
            return hashes;
        }*/
        /*private static Color GetColorAtDirection(byte dir, Sprite sprite, byte rotation)
        {
            dir += rotation;
            if (dir > 3)
                dir -= 4;

            int xOffset = (int)sprite.rect.x;
            int yOffset = (int)sprite.rect.y;
            int midWidth = (int)sprite.rect.width / 2;
            int midHeight = (int)sprite.rect.height / 2;
            switch (dir)
            {
                case 0:
                    return sprite.texture.GetPixel(xOffset + midWidth, yOffset + (int)sprite.rect.height - 1);//u
                case 1:
                    return sprite.texture.GetPixel(xOffset + (int)sprite.rect.width - 1, yOffset + midHeight);//r
                case 2:
                    return sprite.texture.GetPixel(xOffset + midWidth, yOffset);//d
                case 3:
                    return sprite.texture.GetPixel(xOffset, yOffset + midHeight);//l
                default:
                    return Color.black;
            }
        }*/
        private static Dictionary<byte, List<TileSettings>> GetConnections(TileSettings current, List<TileSettings> all)
        {
            var result = new Dictionary<byte, List<TileSettings>>
            {
                { 0, new List<TileSettings>() },
                { 1, new List<TileSettings>() },
                { 2, new List<TileSettings>() },
                { 3, new List<TileSettings>() }
            };
            var currentHash = current.Hashes;
            for (ushort i = 0; i < all.Count; i++)
            {
                var allHash = all[i].Hashes;
                foreach (var pair in result)
                {
                    if (current == all[i] && current.NoSelfConnect != null && current.NoSelfConnect.Contains(pair.Key))
                        continue;

                    if (currentHash[pair.Key] == allHash[GetOpposite(pair.Key)])
                        result[pair.Key].Add(all[i]);
                }
            }
            return result;
        }
        private static int GetOpposite(int dir)
        {
            switch (dir)
            {
                case 0: return 2;
                case 1: return 3;
                case 2: return 0;
                case 3: return 1;
                default: return 0;
            }
        }
    }
}