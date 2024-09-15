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
            // Adjust current Health, but don't allow the max health to be changed unless it's intentional
            if (healthAdjustment < 0)
            {
                TakeDamage(Math.Abs(healthAdjustment)); // Treat negative healthAdjustment as damage
            }
            else
            {
                Heal(healthAdjustment); // Treat positive healthAdjustment as healing
            }

            // Adjust current Mana (use mana if it's negative, regain mana if it's positive)
            if (manaAdjustment < 0)
            {
                UseMana(Math.Abs(manaAdjustment)); // Use mana if adjustment is negative
            }
            else
            {
                RegainMana(manaAdjustment); // Regain mana if adjustment is positive
            }

            // Adjust other stats (like MaxHealth, MaxMana, Speed, Armor, etc.)
            Speed += speedAdjustment;
            Armor += armorAdjustment;
            Fortitude += fortitudeAdjustment;
            WeaponDamage += weaponDamageAdjustment;
            AttackRange += attackRangeAdjustment;

            // Ensure current health and mana don’t exceed the new maximum values
            Health = Math.Min(Health, MaxHealth);
            Mana = Math.Min(Mana, MaxMana);
        }

        // Add this extra method for spell use readability
        public void AdjustStats(AgentStats effectStats)
        {
            // Adjust health (current health adjustment without affecting max health)
            if (effectStats.Health < 0)
            {
                TakeDamage(Math.Abs(effectStats.Health)); // Apply damage if negative
            }
            else
            {
                Heal(effectStats.Health); // Apply healing if positive
            }

            // Adjust mana (current mana adjustment without affecting max mana)
            if (effectStats.Mana < 0)
            {
                UseMana(Math.Abs(effectStats.Mana)); // Spend mana if negative
            }
            else
            {
                RegainMana(effectStats.Mana); // Regain mana if positive
            }

            // Adjust other stats (like Speed, Armor, etc.)
            Speed += effectStats.Speed;
            Armor += effectStats.Armor;
            Fortitude += effectStats.Fortitude;
            WeaponDamage += effectStats.WeaponDamage;
            AttackRange += effectStats.AttackRange;

            // Ensure current health and mana don’t exceed their max values
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