using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
namespace BITF21M025_Assignment1
{
    class VotingMachine
    {
        private static List<Candidate> candidates;
        private static List<Voter> voters;
        public Candidate GetCandidateById(int id)
        {
            foreach (Candidate candidate in candidates)
            {
                if (candidate.CandidateID == id)
                {
                    Console.WriteLine(candidate.CandidateID);
                    return candidate;
                }
            }
            return null; 
        }
        public Voter GetVoterById(string id)
        {
            foreach (Voter voter in voters)
            {
                if (voter.Cnic == id)
                {
                    return voter;
                }
            }
            return null;
        }
        public VotingMachine()
        {
            candidates = new List<Candidate>();
            voters = new List<Voter>();
        }
        public void AddVoter()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addVoter.txt");
            Console.WriteLine("Enter name: ");
            string v_name = Console.ReadLine();
            Console.WriteLine("Enter CNIC: ");
            string v_cnic = Console.ReadLine();
            string v_party = "";
            Voter v = new Voter(v_name, v_cnic, v_party);
            voters.Add(v);
            string output = JsonSerializer.Serialize(v);
            using (StreamWriter sw = new StreamWriter(filePath, append: true))
            {
                sw.WriteLine(output);
            }
            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO Voter ([VoterId], [Voter Name], [Selected Party]) VALUES (@VoterId, @VoterName, @SelectedPartyName)";
                command.Parameters.AddWithValue("@VoterId", v_cnic);
                command.Parameters.AddWithValue("@VoterName", v_name);
                command.Parameters.AddWithValue("@SelectedPartyName", v_party);
                command.ExecuteNonQuery();
            }
            CommonTask.CloseConnection(connection);
            Console.WriteLine("Voter added successfully");
        }
        public void UpdateVoter(params (string cnic, string v_name)[] voterDetails)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addVoter.txt");
            List<Voter> updatedVoters = new List<Voter>();
            bool anyUpdated = false;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(jsonData);

                    foreach (var detail in voterDetails)
                    {
                        if (voter.Cnic == detail.cnic)
                        {
                            anyUpdated = true;
                            voter.VoterName = detail.v_name;
                        }
                    }

                    updatedVoters.Add(voter);
                }
            }

            if (!anyUpdated)
            {
                Console.WriteLine("No voters were updated.");
                return;
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var voter in updatedVoters)
                {
                    string updatedJsonData = JsonSerializer.Serialize(voter);
                    sw.WriteLine(updatedJsonData);
                }
            }

            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "UPDATE Voter SET [Voter Name] = @VoterName WHERE [VoterId] = @Cnic";
                foreach (var detail in voterDetails)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@VoterName", detail.v_name);
                    command.Parameters.AddWithValue("@Cnic", detail.cnic);
                    command.ExecuteNonQuery();
                }
            }
            CommonTask.CloseConnection(connection);

            Console.WriteLine("Voters updated successfully");
        }

        public void AddCandidate()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addCandidate.txt");
            Console.WriteLine("Enter name: ");
            string v_name = Console.ReadLine();

            Console.WriteLine("Enter party name: ");
            string v_party = CommonTask.SelectParty();
            Candidate c = new Candidate(v_name, v_party);
            candidates.Add(c);
            int v_id = c.CandidateID;
            //Candidate c = new Candidate(v_id, v_name, v_party);
            //v_id = c.GenerateCandidateID();
            //c.Name=v_name;
            //c.CandidateID=v_id;
            //candidates.Add(c);
            foreach (Candidate v in candidates)
            {
                Console.WriteLine(v.CandidateID);
            }
            string output = JsonSerializer.Serialize(c);

            using (StreamWriter sw = new StreamWriter(filePath, append: true))
            {
                sw.WriteLine(output);
            }

            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO Candidate ([Candidate Id], [Name], [Party],[Votes]) VALUES (@candidateID, @name, @party,@votes)";
                command.Parameters.AddWithValue("@candidateID", v_id);
                command.Parameters.AddWithValue("@name", v_name);
                command.Parameters.AddWithValue("@party", v_party);
                command.Parameters.AddWithValue("@votes", 0);
                command.ExecuteNonQuery();
            }
            CommonTask.CloseConnection(connection);
            Console.WriteLine("Candidate added successfully");
        }

        public void DisplayCandidates()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addCandidate.txt");
            Console.WriteLine("Candidate Details:");
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(jsonData);
                    Console.WriteLine($"ID: {candidate.CandidateID}");
                    Console.WriteLine($"Name: {candidate.Name}");
                    Console.WriteLine($"Selected Party: {candidate.Party}");
                    int votes = candidate.GetVotes();
                    Console.WriteLine($"Votes: {votes}");
                    Console.WriteLine("------------------------------------");

                }
            }

        }
        public void UpdateCandidate(params (int id, string c_name, string c_party)[] candidateDetails)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addCandidate.txt");
            List<Candidate> updatedCandidates = new List<Candidate>();
            bool anyUpdated = false;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(jsonData);

                    foreach (var detail in candidateDetails)
                    {
                        if (candidate.CandidateID == detail.id)
                        {
                            anyUpdated = true;
                            candidate.Name = detail.c_name;
                            if (!string.IsNullOrEmpty(detail.c_party))
                            {
                                candidate.Party = detail.c_party;
                            }
                        }
                    }

                    updatedCandidates.Add(candidate);
                }
            }

            if (!anyUpdated)
            {
                Console.WriteLine("No candidates were updated.");
                return;
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var candidate in updatedCandidates)
                {
                    string updatedJsonData = JsonSerializer.Serialize(candidate);
                    sw.WriteLine(updatedJsonData);
                }
            }

            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "UPDATE Candidate SET [Name]= @name, [Party]=@party WHERE [Candidate Id]=@candidateID";

                foreach (var detail in candidateDetails)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@name", detail.c_name);
                    if (!string.IsNullOrEmpty(detail.c_party)) 
                    {
                        command.Parameters.AddWithValue("@party", detail.c_party);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@party", DBNull.Value); 
                    }
                    command.Parameters.AddWithValue("@candidateID", detail.id);
                    command.ExecuteNonQuery();
                }
            }
            CommonTask.CloseConnection(connection);

            Console.WriteLine("Candidates updated successfully");
        }

        //public void UpdateCandidate(params (int id, string c_name, string c_party)[] candidateDetails)
        //{
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    string filePath = Path.Combine(currentDirectory, "addCandidate.txt");
        //    List<Candidate> updatedCandidates = new List<Candidate>();
        //    bool anyUpdated = false;

        //    using (StreamReader sr = new StreamReader(filePath))
        //    {
        //        string jsonData;
        //        while ((jsonData = sr.ReadLine()) != null)
        //        {
        //            Candidate candidate = JsonSerializer.Deserialize<Candidate>(jsonData);

        //            foreach (var detail in candidateDetails)
        //            {
        //                if (candidate.CandidateID == detail.id)
        //                {
        //                    anyUpdated = true;
        //                    candidate.Name = detail.c_name;
        //                    candidate.Party = detail.c_party;
        //                }
        //            }

        //            updatedCandidates.Add(candidate);
        //        }
        //    }

        //    if (!anyUpdated)
        //    {
        //        Console.WriteLine("No candidates were updated.");
        //        return;
        //    }

        //    using (StreamWriter sw = new StreamWriter(filePath))
        //    {
        //        foreach (var candidate in updatedCandidates)
        //        {
        //            string updatedJsonData = JsonSerializer.Serialize(candidate);
        //            sw.WriteLine(updatedJsonData);
        //        }
        //    }

        //    SqlConnection connection = CommonTask.GetConnection();
        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        command.Connection = connection;
        //        command.CommandText = "UPDATE Candidate SET [Name]= @name, [Party]=@party WHERE [Candidate Id]=@candidateID";

        //        foreach (var detail in candidateDetails)
        //        {
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@name", detail.c_name);
        //            command.Parameters.AddWithValue("@party", detail.c_party);
        //            command.Parameters.AddWithValue("@candidateID", detail.id);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    CommonTask.CloseConnection(connection);

        //    Console.WriteLine("Candidates updated successfully");
        //}

        public void DeleteCandidate(int id)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addCandidate.txt");
            Candidate candidateToDelete = null;
            List<string> lines = new List<string>();
            bool found = false;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(jsonData);
                    if (candidate.CandidateID == id)
                    {
                        found = true;
                        candidateToDelete = candidate;
                    }
                    else
                    {
                        lines.Add(jsonData);
                    }
                }
            }
            if (!found)
            {
                Console.WriteLine("Candidate with the given ID not found.");
                return;
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }
            }
            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "DELETE FROM Candidate WHERE [Candidate Id]=@candidateID";
                command.Parameters.AddWithValue("@candidateID", id);
                command.ExecuteNonQuery();
            }
            CommonTask.CloseConnection(connection);
            Console.WriteLine("Candidate deleted successfully");
        }
        public void DisplayVoters()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addVoter.txt");
            Console.WriteLine("Voter Details:");
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(jsonData);

                    Console.WriteLine($"Name: {voter.VoterName}");
                    Console.WriteLine($"CNIC: {voter.Cnic}");
                    Console.WriteLine($"Selected Party: {voter.SelectedPartyName}");
                    Console.WriteLine("------------------------------------");

                }
            }
        }
        public void DeleteVoter(string cnic)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "addVoter.txt");
            Voter voterToDelete = null;
            List<string> lines = new List<string>();
            bool found = false;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonData;
                while ((jsonData = sr.ReadLine()) != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(jsonData);
                    if (voter.Cnic == cnic)
                    {
                        found = true;
                        voterToDelete = voter;
                    }
                    else
                    {
                        lines.Add(jsonData);
                    }
                }
            }
            if (!found)
            {
                Console.WriteLine("Voter with the given ID not found.");
                return;
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }
            }
            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "DELETE FROM Voter WHERE [VoterId]=@cnic";
                command.Parameters.AddWithValue("@cnic", cnic);
                command.ExecuteNonQuery();
            }
            CommonTask.CloseConnection(connection);
            Console.WriteLine("Voter deleted successfully");
        }
        public void DeclareWinner()
        {
            if (candidates.Count == 0)
            {
                Console.WriteLine("No candidates available.");
                return;
            }
            Candidate winner = candidates[0];
            foreach (var candidate in candidates)
            {
                if (candidate.GetVotes() > winner.GetVotes())
                {
                    winner = candidate;
                }
            }
            Console.WriteLine("Winner Details:");
            Console.WriteLine($"Candidate ID: {winner.CandidateID}");
            Console.WriteLine($"Name: {winner.Name}");
            Console.WriteLine($"Party: {winner.Party}");
            Console.WriteLine($"Votes: {winner.GetVotes()}");
        }
        public bool CastVote(Candidate c, Voter v)
        {
            if (candidates.Count == 0)
            {
                Console.WriteLine("No candidates available.");
                return false;
            }
            if (!v.HasVoted())
            {
                v.SelectedPartyName = c.Party;
                c.Votes++; 
                UpdateCandidate((c.CandidateID, c.Name, c.Party));
                UpdateVoter((v.Cnic, v.VoterName));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReadCandidate(int id)
        {
            SqlConnection connection = CommonTask.GetConnection();
            using (SqlCommand command = new SqlCommand("SELECT * FROM Candidate WHERE [Candidate Id] = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int candidateId = (int)reader["Candidate Id"];
                    string name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : (string)reader["Name"];
                    string party = reader.IsDBNull(reader.GetOrdinal("Party")) ? null : (string)reader["Party"];
                    int votes = (int)reader["Votes"];

                    Candidate candidate = new Candidate(candidateId, name, party, votes);

                    // Print candidate details to console
                    Console.WriteLine($"Candidate ID: {candidate.CandidateID}");
                    Console.WriteLine($"Name: {candidate.Name}");
                    Console.WriteLine($"Party: {candidate.Party}");
                    Console.WriteLine($"Votes: {candidate.Votes}");
                }
            }
            CommonTask.CloseConnection(connection);
        }

    }
}
