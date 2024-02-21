using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace SciFiTPS
{
    public class SceneSerializer : MonoBehaviour
    {
        [System.Serializable]
        public class SceneObjectState
        {
            public int SceneId;
            public long EntityId;
            public string State;
        }

        public static string FileName = "Test.dat";

        [SerializeField] private PrefabsDataBase m_prefabsDataBase;

        public void SaveScene()
        {
            SaveToFile(FileName);
        }

        public void LoadScene()
        {
            LoadFromFile(FileName);
        }

        public void DeleteSave()
        {
            if (File.Exists(Application.persistentDataPath + "/" + FileName))
            {
                File.Delete(Application.persistentDataPath + "/" + FileName);
            }
        }

        private void SaveToFile(string filePath)
        {
            List<SceneObjectState> savedObjects = new List<SceneObjectState>();

            foreach (var entity in FindObjectsOfType<Entity>())
            {
                ISerializableEntity serializableEntity = entity as ISerializableEntity;

                if (serializableEntity == null) continue;

                if (!serializableEntity.IsSerializable()) continue;

                SceneObjectState s = new SceneObjectState();

                s.EntityId = serializableEntity.EntityId;
                s.State = serializableEntity.SerializeState();

                savedObjects.Add(s);
            }

            if (savedObjects.Count == 0) 
            {
                Debug.Log("Save failed! No saved objects!");
                return; 
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filePath);

            bf.Serialize(file, savedObjects);

            file.Close();

            Debug.Log("Scene saved! Path file: " + Application.persistentDataPath + "/" + filePath);
        }

        private void LoadFromFile(string filePath)
        {
            Player.Instance.NullInstance();

            QuestsMaster.Instance.ClearListsForAllQuests();

            foreach (var entity in FindObjectsOfType<Entity>())
            {
                Destroy(entity.gameObject);
            }

            List<SceneObjectState> loadedObjects = new List<SceneObjectState>();

            if (File.Exists(Application.persistentDataPath + "/" + filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + filePath, FileMode.Open);

                loadedObjects = (List<SceneObjectState>) bf.Deserialize(file);
                file.Close();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                return;
            }

            foreach (var v in loadedObjects)
            {
                if (m_prefabsDataBase.IsPlayerId(v.EntityId))
                {
                    GameObject p = m_prefabsDataBase.CreatePlayer();

                    p.GetComponent<ISerializableEntity>().DeserializeState(v.State);

                    QuestsMaster.Instance.AssignOwnerForQuests(p);

                    loadedObjects.Remove(v);
                    break;
                }
            }

            foreach (var v in loadedObjects)
            {
                GameObject g = m_prefabsDataBase.CreateEntityFromId(v.EntityId);

                g.GetComponent<ISerializableEntity>().DeserializeState(v.State);

                if (g.GetComponent<Vehicle>() != null)
                {
                    QuestsMaster.Instance.AssignOwnerForQuests(g);
                }

                if (g.TryGetComponent(out Destructible dest))
                {
                    if (dest.AssignedKillQuest != null)
                        QuestsMaster.Instance.AssignTargetForKillQuest(dest.AssignedKillQuest, dest);
                }
            }

            Debug.Log("Scene loaded! Path file: " + Application.persistentDataPath + "/" + filePath);
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveScene();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                LoadScene();
            }*/
        }
    }
}
