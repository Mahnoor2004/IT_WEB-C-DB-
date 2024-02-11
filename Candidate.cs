using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace BITF21M025_Assignment1
{
    class Candidate
    {

        private int candidateID;
        private string name;
        private string party;
        private int votes;
        private static Random rand = new Random();
        private static List<int> generatedIDs = new List<int>();
        public Candidate(string name, string party)
        {
            CandidateID = GenerateCandidateID();
            Name = name;
            Party = party;
            Votes = 0;
        }
        //public Candidate(int id, string name, string party)
        //{
        //    this.candidateID = id;
        //    this.name = name;
        //    this.party = party;
        //    this.votes = 0;
        //}
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }
        public int Votes { get; set; }

        // Parameterized constructor for deserialization
        [JsonConstructor]
        public Candidate(int candidateID, string name, string party, int votes)
        {
            this.CandidateID = candidateID;
            this.Name = name;
            this.Party = party;
            this.Votes = votes;
        }
        public int GenerateCandidateID()
        {
            int ID = 0;
            do
            {
                ID = rand.Next(1000, 100001);
            }
            while (generatedIDs.Contains(ID));
            generatedIDs.Add(ID);
            return ID;
        }
        public void setCandidateID(int v_id)
        {
            this.candidateID = v_id;
            Console.WriteLine($"set : {candidateID}");
        }

        public int GetCandidateID()
        {
            return candidateID;
        }
        //or
        //////////
        public int GetVotes()
        {
            return votes;
        }

        public void IncrementVotes()
        {
            votes++;
        }
        public override string ToString()
        {
            return $"Candidate ID: {CandidateID}, Name: {Name}, Party: {Party}";
        }
    }


}

