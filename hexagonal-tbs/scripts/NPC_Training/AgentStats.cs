using System;
using NPC_Training;

namespace NPC_Training{
    public class AgentStats
    {
        //Fluid Stats
        public int Health {get; private set;}
        public int Mana {get; private set;}
        public int Speed {get; private set;}
        public int MaxMana {get; private set;}
        public int MaxHealth {get; private set;}
        public int Armor {get; private set;}
        public int Fortitude {get; private set;}
        public int WeaponDamage {get; private set;}
        public int AttackRange {get; private set;}

        public AgentStats(int maxHealth, int maxMana, int speed, int armor, int fortitude, int weaponDamage, int attackRange)
        {
            MaxHealth = maxHealth;
            Health = maxHealth; // Start with full health
            MaxMana = maxMana;
            Mana = maxMana; // Start with full mana
            Speed = speed;
            Armor = armor;
            Fortitude = fortitude;
            WeaponDamage = weaponDamage;
            AttackRange = attackRange;
        }

        //Used to deal damage to the agent
        //This should be facilitated by the training manager
        public void TakeDamage(int damage){
            int reducedDamage = Math.Max(damage/Armor, 0);
            Health = Math.Max(Health - reducedDamage,0); //Limit minimum HP to 0
        }

        //Method to restore HP
        //This should be facilitated by the training manager
        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth); //Limit maximum HP to MaxHealth
        }

        // Method to spend mana
        //This should be facilitated by the training manager
        public bool UseMana(int amount)
        {
            if (Mana >= amount)
            {
                Mana -= amount;
                return true;
            }
            return false; // Not enough mana
        }

        // Method to regain mana
        //This should be facilitated by the training manager
        public void RegainMana(int amount)
        {
            Mana = Math.Min(Mana + amount, MaxMana); // Ensure mana doesn’t go above max mana
        }

        // Method to increase or decrease stats, e.g. through buffs or debuffs
    //This should be facilitated by the training manager
    public void AdjustStats(int healthAdjustment, int manaAdjustment, int speedAdjustment, int armorAdjustment, int fortitudeAdjustment, int weaponDamageAdjustment, int attackRangeAdjustment)
    {
        MaxHealth += healthAdjustment;
        MaxMana += manaAdjustment;
        Speed += speedAdjustment;
        Armor += armorAdjustment;
        Fortitude += fortitudeAdjustment;
        WeaponDamage += weaponDamageAdjustment;
        AttackRange += attackRangeAdjustment;
        
        // Ensure the current health and mana don’t exceed the new maximum values
        Health = Math.Min(Health, MaxHealth);
        Mana = Math.Min(Mana, MaxMana);
    }

    // To reset the stats (for example, at the end of a round or event)
    //This should be facilitated by the training manager
    public void ResetStats()
    {
        Health = MaxHealth;
        Mana = MaxMana;
    } 
}
}