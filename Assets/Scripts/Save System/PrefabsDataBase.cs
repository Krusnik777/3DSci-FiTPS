using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    [CreateAssetMenu]
    public class PrefabsDataBase : ScriptableObject
    {
        public Entity PlayerPrefab;
        public List<Entity> AllPrefabs;

        public bool IsPlayerId(long id)
        {
            return id == (PlayerPrefab as ISerializableEntity).EntityId;
        }

        public GameObject CreatePlayer()
        {
            return Instantiate(PlayerPrefab.gameObject);
        }

        public GameObject CreateEntityFromId(long id)
        {
            foreach(var entity in AllPrefabs)
            {
                if (!(entity is ISerializableEntity)) continue;

                if ((entity as ISerializableEntity).EntityId == id)
                {
                    return Instantiate(entity.gameObject);
                }
            }

            return null;
        }
    }
}
