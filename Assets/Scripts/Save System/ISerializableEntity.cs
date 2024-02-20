using UnityEngine;

namespace SciFiTPS
{
    public interface ISerializableEntity
    {
        public long EntityId { get; }

        public bool IsSerializable();
        public string SerializeState();
        public void DeserializeState(string state);
    }
}
