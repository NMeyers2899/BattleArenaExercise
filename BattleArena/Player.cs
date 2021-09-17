using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;

        public Item CurrentItem
        {
            get { return _currentItem; }
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items) : 
            base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.name = "Nothing";
        }

        /// <summary>
        /// Attempts to equip an item of an index given to us by the user.
        /// </summary>
        /// <param name="index"> The index which refers to an item. </param>
        /// <returns> If the user can equip the item or not. </returns>
        public bool TryEquipItem(int index)
        {
            // Checks to see if the index is out of bounds of our _items array. If it is...
            if(index >= _items.Length || index < 0)
            {
                // ...it returns false.
                return false;
            }

            // Sets the current item to the item at the index.
            _currentItem = _items[index];

            return true;
        }

        /// <summary>
        /// Sets the current item to nothing.
        /// </summary>
        /// <returns> Whether or not the player already had an item equipped. </returns>
        public bool TryUnequipItem()
        {
            // Checks to see if anything is equipped. If it is...
            if(_currentItem.name == "Nothing")
            {
                // ...it returns false.
                return false;
            }

            // Sets the item to nothing.
            _currentItem = new Item();
            _currentItem.name = "Nothing";

            return true;
        }
    }
}
