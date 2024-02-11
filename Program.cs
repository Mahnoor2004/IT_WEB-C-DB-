using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace BITF21M025_Assignment1
{
    internal class Program
    {
        private static List<string> voters = new List<string>(); 
        private static List<string> candidates = new List<string>();
        static void Main(string[] args)
        {
            VotingMachine votingMachine = new VotingMachine();

            while (true)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Welcome to Online Voting System");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("1. Add Voter");
                Console.WriteLine("2. Update Voter");
                Console.WriteLine("3. Delete Voter");
                Console.WriteLine("4. Display Voters");
                Console.WriteLine("5. Cast Vote");
                Console.WriteLine("6. Insert Candidate");
                Console.WriteLine("7. Update Candidate");
                Console.WriteLine("8. Display Candidates");
                Console.WriteLine("9. Delete Candidate");
                Console.WriteLine("10. Declare Winner");
                Console.WriteLine("11. Read Candidate");
                Console.WriteLine("12. EXIT");
                Console.WriteLine("Enter your choice from 1 to 10: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number from 1 to 10.");
                    continue;
                }
                switch (choice)
                {
                    case 1:
                       
                        votingMachine.AddVoter();
                        break;
                    case 2:
                        Console.WriteLine("Enter CNIC of the voter to update: ");
                        string cnicToUpdate = Console.ReadLine();
                        Console.WriteLine("Enter Voter details: ");
                        Console.WriteLine("Enter name: ");
                        string v_name = Console.ReadLine();
                        votingMachine.UpdateVoter((cnicToUpdate,v_name));
                        break;
                    case 3:
                        Console.WriteLine("Enter CNIC of the voter to delete: ");
                        string cnicToDelete = Console.ReadLine();
                        votingMachine.DeleteVoter(cnicToDelete);
                        break;
                    case 4:
                        votingMachine.DisplayVoters();
                        break;
                    case 5:
                        Console.WriteLine("Enter CNIC of the voter: ");
                        string voterCnic = Console.ReadLine();
                        Console.WriteLine("Enter the ID of the candidate: ");
                        string candidateId = Console.ReadLine();

                        if (int.TryParse(candidateId, out int id_C))
                        {
                            Candidate candidate = votingMachine.GetCandidateById(id_C);
                            if (candidate != null)
                            {
                                Voter voter = votingMachine.GetVoterById(voterCnic);
                                if (voter != null)
                                {
                                    if (votingMachine.CastVote(candidate, voter))
                                    {
                                        Console.WriteLine("Vote casted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Vote could not be casted.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Voter with the given CNIC not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Candidate with the given ID not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid candidate ID.");
                        }
                        break; 
                    case 6:
                        votingMachine.AddCandidate();
                        break;
                    case 7:
                        Console.Write("Enter the ID of Candidate: ");
                        string id_S = Console.ReadLine();
                        Console.WriteLine("Enter Candidate details: ");
                        Console.WriteLine("Enter name: ");
                        string c_name = Console.ReadLine();
                        Console.WriteLine("Enter selected party name: ");
                        string c_party = CommonTask.SelectParty();
                        if (int.TryParse(id_S, out int id))
                        {
                            votingMachine.UpdateCandidate((id,c_name,c_party));
                        }                       
                        break;
                    case 8:
                        votingMachine.DisplayCandidates();
                        break;
                    case 9:
                        Console.Write("Enter the ID of Candidate: ");
                        string id_S1 = Console.ReadLine();
                        if (int.TryParse(id_S1, out int ids))
                        {
                            votingMachine.DeleteCandidate(ids); ;
                        }
                        break;
                    case 10:
                        votingMachine.DeclareWinner();
                        break;
                    case 11:
                        Console.Write("Enter the ID of Candidate: ");
                        string id_c = Console.ReadLine();
                        if (int.TryParse(id_c, out int id1))
                        {
                            votingMachine.ReadCandidate(id1);
                        }
                        break;
                    case 12:
                        Console.WriteLine("Exiting....");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 10.");
                        break;
                }
            }
        }
    }
}