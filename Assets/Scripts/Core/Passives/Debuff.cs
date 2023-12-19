using Assets.Scripts.Core.Data;

namespace Assets.Scripts.Core.Passives
{
    public abstract class Debuff
    {
        public uint ID { get; protected set; }
        public uint ImgId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public float Magnitude { get; protected set; }
        public float Duration { get; protected set; }

        public Debuff(DebuffData data)
        {
            ID = data.id;
            ImgId = data.imgId;
            Name = data.name;
            Description = data.description;
            Magnitude = data.magnitude;
            Duration = data.duration;
        }
    }
}