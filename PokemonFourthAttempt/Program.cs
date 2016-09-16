using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonFourthAttempt
{
    public delegate void PokemonFaint(object sender, PokemonFaintEventArgs e);
    public delegate void TrainerOutOfPokemon(object sender, EventArgs e);

    public class PokemonFaintEventArgs : EventArgs
    {
        
    }

    class Program
    {
        static void Main()
        {
            Arena arena = new Arena();
            arena.ExchangeAttacks();
            arena.ExchangeAttacks();
        }
    }

    class Pokemon
    {
        public event PokemonFaint OnFaint;

        private int hp;

        public Pokemon(int hp)
        {
            this.hp = hp;
        }

        public void TakeDamage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                OnFaint(this, new PokemonFaintEventArgs());
            }
        }

        public int HP => this.hp;
    }

    class Trainer
    {
        private Pokemon[] pokemonArr;
        private int currentIndex;
        private string name;

        public event TrainerOutOfPokemon OnTrainerLoss;

        public Trainer(string name, Pokemon[] pokemonArr)
        {
            this.name = name;
            this.pokemonArr = pokemonArr;
            foreach (Pokemon pokemon in this.pokemonArr)
            {
                pokemon.OnFaint += new PokemonFaint(SwitchPokemon);
            }
        }

        protected void SwitchPokemon(object sender, PokemonFaintEventArgs e)
        {
            Pokemon senderPokemon = (Pokemon)sender;
            //	The statement below needs to be examined further for edification purposes
            //senderPokemon.hp = 0;

            if (this.currentIndex == pokemonArr.Length - 1)
            {
                OnTrainerLoss(this, new EventArgs());
            }

            this.currentIndex++;
        }

        public Pokemon CurrentPokemon => this.pokemonArr[currentIndex];
        public int CurrentIndex => this.currentIndex;

        public override string ToString()
        {
            return this.name;
        }
    }

    class Arena
    {
        public Trainer ash;
        public Trainer gary;

        public Arena()
        {
            this.ash = new Trainer("Ash", new Pokemon[] { new Pokemon(50), new Pokemon(50) });
            this.gary = new Trainer("Gary", new Pokemon[] { new Pokemon(50), new Pokemon(50) });

            this.ash.OnTrainerLoss += new TrainerOutOfPokemon(TrainerLossHandler);
            this.gary.OnTrainerLoss += new TrainerOutOfPokemon(TrainerLossHandler);
        }

        public void ExchangeAttacks()
        {
            this.ash.CurrentPokemon.TakeDamage(30);
            Console.WriteLine($"Gary's pokemon is {this.gary.CurrentIndex}");
            this.gary.CurrentPokemon.TakeDamage(50);
            Console.WriteLine($"Gary's pokemon is {this.gary.CurrentIndex}");
        }

        protected void TrainerLossHandler(object sender, EventArgs e)
        {
            Trainer loser = (Trainer)sender;

            Console.WriteLine("cls");
            Console.WriteLine($"{loser} Lost");
            Console.Read();
            Environment.Exit(0);
        }
    }




}
