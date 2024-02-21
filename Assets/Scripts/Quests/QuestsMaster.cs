using UnityEngine;

namespace SciFiTPS
{
    public class QuestsMaster : MonoSingleton<QuestsMaster>
    {
        private Quest[] quests;
        public Quest[] Quest => quests;

        public void ClearListsForAllQuests()
        {
            foreach (var quest in quests)
            {
                if (quest is QuestReachTrigger)
                {
                    (quest as QuestReachTrigger).ClearOwners();
                }

                if (quest is QuestKillDestructibles)
                {
                    (quest as QuestKillDestructibles).ClearTargetsList();
                }
            }
        }

        public void AssignOwnerForQuests(GameObject owner)
        {
            foreach (var quest in quests)
            {
                if (quest is QuestReachTrigger)
                {
                    (quest as QuestReachTrigger).AssignOwner(owner);
                }
            }
        }

        public void AssignTargetForKillQuest(QuestKillDestructibles assignedQuest, Destructible target)
        {
            foreach (var quest in quests)
            {
                if (quest is QuestKillDestructibles)
                {
                    var killQuest = quest as QuestKillDestructibles;

                    if (killQuest == assignedQuest)
                    {
                        killQuest.AssignTarget(target);
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            quests = GetComponentsInChildren<Quest>();
        }
    }
}
