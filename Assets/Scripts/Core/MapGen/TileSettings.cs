using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.MapGen
{
    public class TileSettings
    {
        public byte[] NoSelfConnect { get; private set; }
        public ushort Weight { get; private set; }
        public byte Rotation { get; private set; }
        public int[] Hashes { get; private set; }
        public Dictionary<byte, List<TileSettings>> ConnectionRules { get; private set; }
        public Sprite Sprite { get; private set; }
        public bool Walkable { get; private set; }
        public short ObjectID { get; private set; }
        public float ObjectScale { get; private set; }

        public TileSettings(ushort weight, int[] hashes, Sprite sp, byte rotation, byte[] noSelfConnect,
            bool walkable, short objectID, float objectScale)
        {
            Weight = weight;
            Hashes = hashes;
            Sprite = sp;
            Rotation = rotation;
            Walkable = walkable;
            ObjectID = objectID;
            ObjectScale = objectScale;
            if (noSelfConnect != null)
            {
                NoSelfConnect = new byte[noSelfConnect.Length];
                for (int i = 0; i < noSelfConnect.Length; i++)
                {
                    byte dir = (byte)(noSelfConnect[i] + Rotation);
                    if (dir > 3) dir -= 4;
                    NoSelfConnect[i] = dir;
                }
            }
            ObjectID = objectID;
        }
        public void SetConnectionRules(Dictionary<byte, List<TileSettings>> connectionRules)
        {
            ConnectionRules = connectionRules;
        }

    }
}