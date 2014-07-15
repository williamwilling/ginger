using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WilliamWilling.Ginger.Models
{
    public class BoardEntity : TableEntity
    {
        public BoardEntity()
        {
        }

        public BoardEntity(string game)
        {
            PartitionKey = game;
            RowKey = GenerateId();
            Version = 1;
            Contents = String.Empty;
        }

        public int Version
        {
            get;
            set;
        }

        public string Contents
        {
            get;
            set;
        }

        private string GenerateId()
        {
            char[] consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 's', 't', 'v', 'w', 'z' };
            char[] vowels = { 'a', 'e', 'i', 'o', 'u' };

            StringBuilder id = new StringBuilder(8);
            var random = new Random();

            for (int i = 0; i < 5; i++)
            {
                id.Append(consonants[random.Next(consonants.Length)]);
                id.Append(vowels[random.Next(vowels.Length)]);
            }

            return id.ToString();
        }
    }
}