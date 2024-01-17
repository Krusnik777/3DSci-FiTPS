using UnityEngine;

namespace SciFiTPS
{
    public class Pickup_FirstAidKit : TriggerInteractAction
    {
        protected override void OnEndAction(GameObject owner)
        {
            base.OnEndAction(owner);

            Destructible destructible = owner.transform.root.GetComponent<Destructible>();

            if (destructible != null)
            {
                destructible.FullHeal();
            }

            Destroy(gameObject);
        }
    }
}
