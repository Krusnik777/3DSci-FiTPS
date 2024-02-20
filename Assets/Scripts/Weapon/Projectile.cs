using UnityEngine;

namespace SciFiTPS
{
    public class Projectile : Entity
    {
        [SerializeField] protected float m_velocity;
        [SerializeField] protected float m_lifeTime;
        [SerializeField] protected int m_damage;
        [SerializeField] protected ImpactEffectHandler m_impactEffectHandler;

        protected float timer;

        protected Destructible m_parent;
        public Destructible Parent => m_parent;

        protected virtual void Update()
        {
            float stepLength = Time.deltaTime * m_velocity;
            Vector3 step = transform.forward * stepLength;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, stepLength/*, 1, QueryTriggerInteraction.Ignore*/))
            {
                if (!hit.collider.isTrigger)
                {
                    Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                    if (dest != null && dest != m_parent)
                    {
                        dest.ApplyDamage(this, m_damage);
                    }
                    OnProjectileLifeEnd(hit.collider, hit.point, hit.normal);
                }
            }

            timer += Time.deltaTime;

            if (timer > m_lifeTime) Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, step.z);
        }

        protected void OnProjectileLifeEnd(Collider col, Vector3 pos, Vector3 normal)
        {
            if (m_impactEffectHandler != null)
            {
                var impactEffect = Instantiate(m_impactEffectHandler.GetImpactPrefab(col), pos, Quaternion.LookRotation(normal));
                impactEffect.transform.SetParent(col.transform);
                
                /*
                if (col.GetComponent<Surface>() != null)
                {
                    impactEffect.UpdateType(col.GetComponent<Surface>().Type);
                }*/
            }

            Destroy(gameObject);
        }

        public void SetParentShooter (Destructible parent)
        {
            m_parent = parent;
        }
    }
}
