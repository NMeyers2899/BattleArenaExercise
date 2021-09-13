using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    // Test

    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver = false;
        bool keepName = false;
        int currentScene = 0;
        Character player;
        Character littleDude;
        Character bigDude;
        Character theFinalBoss;
        Character[] enemies;
        private Character currentEnemy;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            // Initalizes the Stats for Little Dude.
            littleDude.name = "A Little Dude";
            littleDude.health = 20;
            littleDude.attackPower = 20;
            littleDude.defensePower = 5;

            // Initalizes the Stats for Big Dude.
            bigDude.name = "A Big Dude";
            bigDude.health = 25;
            bigDude.attackPower = 30;
            bigDude.defensePower = 10;

            // Initalizes the Stats for The Final Boss.
            theFinalBoss.name = "Krazarackaraodareda the World Eater";
            theFinalBoss.health = 40;
            theFinalBoss.attackPower = 20;
            theFinalBoss.defensePower = 5;

            // Initalizes the list of enemies that will be fought in this order.
            enemies = new Character[] { littleDude, bigDude, theFinalBoss };
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Farewell " + player.name + "!");
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {
                //Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                }

                Console.Clear();
            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            // Finds the current scene for...
            switch (currentScene)
            {
                // ...character selection.
                case 0:
                    CharacterSelection();
                    currentScene++;
                    break;
                // ...fighting enemies.
                case 1:
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        currentEnemy = enemies[i];
                        Battle();
                    }
                    currentScene++;
                    break;
                // ...asking the player to restart the game.
                case 2:
                    DisplayMainMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            int choice = GetInput("Would you like to restart the game?", "Yes!", "No.");
            // Finds out whether the player wishes to...
            switch(choice)
            {
                // ...restart the game.
                case 1:
                    currentScene = 0;
                    break;
                // ...end the game.
                case 2:
                    gameOver = true;
                    break;
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            if (!keepName)
            {
                Console.Write("What is your name, adventurer? \n> ");
                player.name = Console.ReadLine();
                Console.Clear();
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = 0;

            GetPlayerName();

            if (!keepName)
            {
               choice = GetInput("Would you like to keep your name?", "Yes.", "No.");

                switch (choice)
                {
                    case 1:
                        keepName = true;
                        break;
                    case 2:
                        break;
                }
            }

            // Checks to see if the player kept their fighting style from another playthough.
             choice = GetInput(player.name + ", which style of fighting do you align with?",
                "Brute Force!", "Defensive Tactics.");

            // Finds out whether the player wants to...
            switch (choice)
            {
                // ...be a more physical fighter.
                case 1:
                    player.health = 100;
                    player.attackPower = 35;
                    player.defensePower = 10;
                    break;
                // ...or rely on defense more.
                case 2:
                    player.health = 75;
                    player.attackPower = 20;
                    player.defensePower = 15;
                    break;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine(character.name + "'s stats:");
            Console.WriteLine("Health: " + character.health);
            Console.WriteLine("Attack: " + character.attackPower);
            Console.WriteLine("Defense: " + character.defensePower);
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            if (attackPower - defensePower < 0)
            {
                return 0;
            }

            return attackPower - defensePower;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public void Attack(ref Character attacker, ref Character defender)
        {
            float damage = CalculateDamage(attacker.attackPower, defender.defensePower);
            Console.WriteLine(attacker.name + " deals " + damage + " to " + defender.name + "!");
            defender.health -= damage;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            // Checks to see if both the player and the enemy are still alive.
            while(player.health > 0 && currentEnemy.health > 0)
            {
                // Gives updates on the player and current enemy's stats.
                DisplayStats(player);
                Console.WriteLine("");
                DisplayStats(currentEnemy);
                Console.WriteLine("");

                int choice = GetInput(currentEnemy.name + " stands before you! What will you do?", 
                    "Attack!", "Dodge!");
                // Finds out if the player wishes to...
                switch (choice)
                {
                    // ...attack, dealing damage to the enemy. In turn taking damage from the enemy.
                    case 1:
                        Attack(ref player, ref currentEnemy);
                        Attack(ref currentEnemy, ref player);
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    // ... dodge the enemy's attack, but deal no damage in return.
                    case 2:
                        Console.WriteLine("You dodge " + currentEnemy.name + "!");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                }
            }

            CheckBattleResults();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            // If the player is still alive and the enemy is dead, it moves on to the next fight.
            if (player.health > 0 && currentEnemy.health <= 0)
            {
                Console.WriteLine("You defeated " + currentEnemy.name + "!");

                Console.ReadKey(true);
                Console.Clear();
            }

            // If the player is dead, it asks the player if they wish to restart the game.
            if(player.health <= 0)
            {
                Console.WriteLine("You have been slain.");
                DisplayMainMenu();
            }
        }

    }
}
