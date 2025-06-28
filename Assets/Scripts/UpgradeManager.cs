using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public const int C_ID_MAX_HP = 0;
    public const int C_ID_HEALING_FACTOR = 1;
    public const int C_ID_DAMAGE = 2;
    public const int C_ID_FIRING_RATE = 3;
    public const int C_ID_ALLY_WORTH = 4;
    public const int C_ID_PASSIVE = 5;

    [System.Serializable]
    public class Upgrade
    {
        public int id;
        public string title;
        public int rank;
        public int value;
        public int cost;

        public Upgrade(int id, string title, int cost, int value, int rank)
        {
            this.id = id;
            this.title = title;
            this.cost = cost;
            this.value = value;
            this.rank = rank;
        }

        public Upgrade(Upgrade u)
        {
            this.id = u.id;
            this.title = u.title;
            this.cost = u.cost;
            this.value = u.value;
            this.rank = u.rank;
        }
    }
    
    public List<Upgrade> available = new List<Upgrade>();
    public List<Upgrade> purchased = new List<Upgrade>();

    void Start()
    {
        ResetUpgrades();
    }

    public void ResetUpgrades()
    {
        available.Clear();
        purchased.Clear();

        // Health
        available.Add(new Upgrade(0, "Increase Health", 100, 100, 0));
        available.Add(new Upgrade(0, "Increase Health", 150, 150, 1));
        available.Add(new Upgrade(0, "Increase Health", 250, 200, 2));
        available.Add(new Upgrade(0, "Increase Health", 400, 250, 3));

        // Healing Factor
        available.Add(new Upgrade(1, "Healing Factor", 120, 10, 0));
        available.Add(new Upgrade(1, "Healing Factor", 180, 20, 1));
        available.Add(new Upgrade(1, "Healing Factor", 250, 40, 2));
        available.Add(new Upgrade(1, "Healing Factor", 350, 50, 3));

        // Damage
        available.Add(new Upgrade(2, "Increase Damage", 130, 10, 0));
        available.Add(new Upgrade(2, "Increase Damage", 200, 15, 1));
        available.Add(new Upgrade(2, "Increase Damage", 270, 25, 2));
        available.Add(new Upgrade(2, "Increase Damage", 350, 40, 3));

        /*
        // Firing Rate
        available.Add(new Upgrade(3, "Increase Firing Rate", 100, 0, 0));
        available.Add(new Upgrade(3, "Increase Firing Rate", 160, 15, 1));
        available.Add(new Upgrade(3, "Increase Firing Rate", 220, 30, 2));
        available.Add(new Upgrade(3, "Increase Firing Rate", 300, 50, 3));

        // Ally Support
        available.Add(new Upgrade(4, "Ally Support", 200, 1, 0));
        available.Add(new Upgrade(4, "Ally Support", 300, 2, 1));
        available.Add(new Upgrade(4, "Ally Support", 400, 3, 2));
        available.Add(new Upgrade(4, "Ally Support", 500, 4, 3));

        // Mining Efficiency
        available.Add(new Upgrade(5, "Mining Efficiency", 150, 1, 0));
        available.Add(new Upgrade(5, "Mining Efficiency", 220, 2, 1));
        available.Add(new Upgrade(5, "Mining Efficiency", 300, 3, 2));
        available.Add(new Upgrade(5, "Mining Efficiency", 400, 4, 3));
        */

        foreach (Upgrade u in available)
        {
            if(u.rank == 0)
            {
                ApplyUpgrade(u);
            }
        }
    }

    public void ApplyUpgrade(Upgrade u)
    {
        purchased.Add(new Upgrade(u));

        switch (u.id)
        {
            case C_ID_MAX_HP:
            GameManager.maxHP = u.value;
            break;
            case C_ID_HEALING_FACTOR:
            GameManager.healingFactor = u.value;
            break;
            case C_ID_DAMAGE:
            GameManager.bulletDamage = u.value;
            break;
            case C_ID_FIRING_RATE:
            // GameManager.maxHP = u.value;
            break;
            case C_ID_ALLY_WORTH:
            // GameManager.maxHP = u.value;
            break;
            case C_ID_PASSIVE:
            // GameManager.maxHP = u.value;
            break;
        }

        available.Remove(u);
    }

    public void PurchaseUpgrade(int upgradeId)
    {
        // TODO
        /*
        Upgrade upgrade = upgrades.Find(u => u.id == upgradeId);
        if (upgrade != null)
        {
            // Add logic to check player currency here
            upgrade.currentRank++;
            upgrade.cost += 50; // Example cost scaling
        }
        */
    }
}
