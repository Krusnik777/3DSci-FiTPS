using UnityEngine;

namespace SciFiTPS
{
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] private Entity[] m_entityPrefabs;
        [SerializeField] private CubeArea m_area;
        [SerializeField] private SpawnMode m_spawnMode;
        [SerializeField] private int m_numSpawns;
        [SerializeField] private float m_respawnTime;
        [SerializeField] private int m_spawnsTeamId;
        [SerializeField] private bool m_spawnAtStart;

        private float timer;

        private void Start()
        {
            if (m_spawnMode == SpawnMode.Start || m_spawnAtStart)
            {
                SpawnEntities();
            }

            timer = m_respawnTime;
        }

        private void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            if (m_spawnMode == SpawnMode.Loop && timer < 0)
            {
                SpawnEntities();

                timer = m_respawnTime;
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < m_numSpawns; i++)
            {
                int index = Random.Range(0, m_entityPrefabs.Length);

                GameObject e = Instantiate(m_entityPrefabs[index].gameObject);

                if (e.TryGetComponent(out Destructible dest))
                {
                    dest.SetTeam(m_spawnsTeamId);

                    if (dest is Drone)
                    {
                        dest.transform.GetComponent<AIDrone>().SetMovementArea(m_area);
                    }
                }

                e.transform.position = m_area.GetRandomInsideZone();
            }
        }
    }
}
