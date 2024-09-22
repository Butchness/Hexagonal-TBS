using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using NPC_Training;

namespace NPC_Training
{
    public class SpellManager
    {
        public List<Spell> Spells {get; private set; }
        private const int SpellSize = 1;

        public SpellManager(Dictionary<string, Spell> spellBook)
        {
            Spells = new List<Spell>();
            SelectRandSpells(spellBook);
        }

        public void SelectRandSpells(Dictionary<string, Spell> spellBook)
        {
            Spells.Clear();

            if(SpellSize > spellBook.Count){throw new Exception("Spellbook is not large enouge!");}

            if (spellBook.Count == 0)
            {
                throw new Exception("Spellbook is empty! Unable to select any spells.");
            }

            Random random = new Random(DateTime.Now.Millisecond);
            List<Spell> availableSpells = new List<Spell>(spellBook.Values); // Convert spellbook values to a list

            // Select SpellSize unique spells
            while (Spells.Count < SpellSize)
            {
                int randomIndex = random.Next(availableSpells.Count); // Select a random spell index
                Spell selectedSpell = availableSpells[randomIndex];

                if (!Spells.Contains(selectedSpell)) // Ensure no duplicates
                {
                    Spells.Add(selectedSpell);
                }

                // Remove the spell from the availableSpells list to prevent selecting it again
                availableSpells.RemoveAt(randomIndex);
            }
        }
    }
}