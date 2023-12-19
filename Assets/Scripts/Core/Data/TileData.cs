namespace Assets.Scripts.Core.Data
{
    public class TileData
    {
        public ushort index;
        public ushort weight;
        public byte? rotations;
        public byte[] noselfconnect;
        public bool? walkable;
        public byte[] connection;
        public short? objectID;
        public float? objectScale;
    }
}