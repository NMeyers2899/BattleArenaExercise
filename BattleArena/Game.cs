using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        NONE
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public int BoostType;
    }

    public class Game
    {
        private bool _gameOver = false;
        private int _currentScene = 0;
        private int _currentEnemyIndex = 0;
        private Player _player;
        private string _playerName;
        private Entity[] _enemies;
        private Entity _currentEnemy;
        private Item[] _offensiveInventory;
        private Item[] _defensiveInventory;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        private void Start()
        {
            _gameOver = false;

            _currentScene = 0;

            InitializeEnemies();

            InitializeItems();
        }

        /// <summary>
        /// Initalizes the items for the different classes
        /// </summary>
        public void InitializeItems()
        {
            // Defensive Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 20, BoostType = 1 };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 25, BoostType = 0 };

            // Offensive Items
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 20, BoostType = 1 };
            Item freshJays = new Item { Name = "Fresh J's", StatBoost = 10, BoostType = 0 };

            _defensiveInventory = new Item[] { bigWand, bigShield };
            _offensiveInventory = new Item[] { bigStick, freshJays };
         }

        /// <summary>
        /// Initalizes the enemies on start and restart of the game.
        /// </summary>
        private void InitializeEnemies()
        {
            _currentEnemyIndex = 0;

            // Initalizes the Stats for Little Dude.
            Entity littleDude = new Entity("A Little Dude", 30, 25, 15);

            // Initalizes the Stats for Big Dude.
            Entity bigDude = new Entity("A Big Dude", 35, 30, 20);

            // Initalizes the Stats for The Final Boss.
            Entity theFinalBoss = new Entity("Krazarackaradareda the World Eater", 45, 35, 20);

            // Initalizes the list of _enemies that will be fought in this order.
            _enemies = new Entity[] { littleDude, bigDude, theFinalBoss };

            _currentEnemy = _enemies[_currentEnemyIndex];

        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        private void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        private void End()
        {
            Console.WriteLine("Farewell " + _player.Name + "!");
        }

        /// <summary>
        /// Gets an input from the _player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <returns> The users input of a given choice. </returns>
        private int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputRecieved = -1;

            while(inputRecieved == -1)
            {
                // Print out all options.
                Console.WriteLine(description);
                for(int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                input = Console.ReadLine();

                // If a player typed an int...
                if(int.TryParse(input, out inputRecieved))
                {
                    // ...decrement the input and check if it's within bounds of the array.
                    inputRecieved--;
                    if(inputRecieved < 0 || inputRecieved >= options.Length)
                    {
                        // Sets inputRecieved to the default value.
                        inputRecieved = -1;
                        //Display error message.
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
                // If the user didn't type an int.
                else
                {
                    // Sets inputRecieved to the default value.
                    inputRecieved = -1;
                    //Display error message.
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }

                Console.Clear();
            }

            return inputRecieved;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        private void DisplayCurrentScene()
        {
            // Finds the current scene for...
            switch (_currentScene)
            {
                // ...naming their character.
                case 0:
                    GetPlayerName();
                    break;
                // ...character selection.
                case 1:
                    CharacterSelection();
                    break;
                // ...fighting _enemies.
                case 2:
                    Battle();
                    break;
                // ...asking the _player to restart the game.
                case 3:
                    DisplayMainMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the _player to start or quit the game
        /// </summary>
        private void DisplayMainMenu()
        {
            int choice = GetInput("Would you like to restart the game?", "Yes!", "No.");
            // Finds out whether the _player wishes to...
            switch(choice)
            {
                // ...restart the game.
                case 0:
                    _currentScene = 0;
                    InitializeEnemies();
                    break;
                // ...end the game.
                case 1:
                    _gameOver = true;
                    break;
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the _player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.Write("What is your name, adventurer? \n> ");
            _playerName = Console.ReadLine();
            Console.Clear();

            int choice = GetInput("Would you like to keep your name?", "Yes.", "No.");

            switch (choice)
            {
                case 0:
                    _currentScene++;
                    break;
                case 1:
                    break;
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = 0;

            // Checks to see if the _player kept their fighting style from another playthough.
             choice = GetInput("Which style of fighting do you align with?",
                "Brute Force!", "Defensive Tactics.");

            // Finds out whether the _player wants to...
            switch (choice)
            {
                // ...be a more physical fighter.
                case 0:
                    _player = new Player(_playerName, 100, 35, 10, _offensiveInventory);
                    break;
                // ...or rely on defense more.
                case 1:
                    _player = new Player(_playerName, 75, 20, 15, _defensiveInventory);
                    break;
            }

            _currentScene++;
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character"> The character that will have its stats shown </param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine(character.Name + "'s stats:");
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack: " + character.AttackPower);
            Console.WriteLine("Defense: " + character.DefensePower);
        }

       public void DisplayEquipItemMenu()
       {
            // Get the item index.
            int choice = GetInput("Select an item to equip.", _player.GetItemNames());

            // Equip the item of the given index.
            _player.TryEquipItem(choice);

            if (!_player.TryEquipItem(choice))
            {
                Console.WriteLine("You couldn't find that item in the bag.");
                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
            Console.ReadKey(true);
            Console.Clear();
       }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            // Gives updates on the _player and current enemy's stats.
            DisplayStats(_player);
            Console.WriteLine("");
            DisplayStats(_currentEnemy);
            Console.WriteLine("");

            int choice = GetInput(_currentEnemy.Name + " stands before you! What will you do?",
                "Attack!", "Equip Item." , "Remove Current Item");
            // Finds out if the _player wishes to...
            switch (choice)
            {
                // ...attack, dealing damage to the enemy. In turn taking damage from the enemy.
                case 0:
                    damageDealt = _player.Attack(_currentEnemy);
                    break;
                // ... dodge the enemy's attack, but deal no damage in return.
                case 1:
                    DisplayEquipItemMenu();
                    return;
            }

            damageDealt = _currentEnemy.Attack(_player);

            CheckBattleResults();
        }

        /// <summary>
        /// Checks to see if either the _player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            // If the _player is still alive and the enemy is dead, it moves on to the next fight.
            if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You defeated " + _currentEnemy.Name + "!");
                _currentEnemyIndex++;

                if(_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = 3;
                    Console.WriteLine("You are victorious!");
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
                Console.ReadKey(true);
                Console.Clear();
            }

            // If the _player is dead, it asks the _player if they wish to restart the game.
            if(_player.Health <= 0)
            {
                Console.WriteLine("You have been slain.");
                DisplayMainMenu();
            }
        }

        /// <summary>
        /// Takes in an array and a number given by the user and adds the number to the end of the array.
        /// </summary>
        /// <param name="arr"> The array of integers. </param>
        /// <param name="num"> The new number. </param>
        /// <returns> The new array with the number appended to the end. </returns>
        private int[] AppendToArray(int[] arr, int num)
        {
            // Creates a new array that will store the values of the old array and a new one.
            int[] appendedArray = new int[arr.Length + 1];

            // Iterates through the old array, giving the new array the values of the old one.
            for(int i = 0; i < arr.Length; i++)
            {
                appendedArray[i] = arr[i];
            }

            // Adds the number given by the user to the final spot in the new array.
            appendedArray[arr.Length] = num;

            // Returns the new array.
            return appendedArray;
        }

    }
}
