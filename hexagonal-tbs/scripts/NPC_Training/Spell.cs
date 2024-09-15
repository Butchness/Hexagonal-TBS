using System;

namespace NPC_Training
{
    public class Spell
    {
        public string Name { get; private set; } // Name of the spell
        public int ManaCost { get; private set; } // Cost to caster upon use
        public int Range { get; private set; } // Distance from caster the spell can be cast
        public int Duration { get; private set; } // The number of turns the spell effects the target (must be > 0)

        // Effects on the caster and the target: Effect[0] for the caster, Effect[1] for the target
        public AgentStats[] Effect { get; private set; } 

        public Spell(string name, int range, int duration, AgentStats casterEffect, AgentStats targetEffect)
        {
            Name = name;
            ManaCost = casterEffect.Mana;
            Range = range;
            Duration = duration; //0 is for single use like single heals and anything higher is for multiple turn effects like dots, buffs or debuffs
            Effect = new AgentStats[2]; // [0] is caster, [1] is target
            Effect[0] = casterEffect;
            Effect[1] = targetEffect;
        }

        // Check if the spell can be cast based on the caster's mana and range to the target
        public bool CanCast(AgentStats casterStats, int distanceToTarget)
        {
            return ((casterStats.Mana >= ManaCost) && (distanceToTarget <= Range));
        }

        // Apply the spell effect to the caster and the target
        public AgentStats ApplyEffect(AgentStats caster)
        {
            // Apply casting effect to caster (e.g., mana/resource cost reduction)
            caster.AdjustStats(
                Effect[0].Health, // Apply health adjustment (can be healing or damage)
                Effect[0].Mana,   // Apply mana adjustment (typically a cost for casting the spell)
                Effect[0].Speed,
                Effect[0].Armor,
                Effect[0].Fortitude,
                Effect[0].WeaponDamage,
                Effect[0].AttackRange);

            // Return the target effect to be applied to the target
            return Effect[1]; 
        }
    }
}
