using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
namespace BITF21M025_Assignment1
{
    internal class CommonTask
    {
        private static readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        private static List<string> partyNames;
        static CommonTask()
        {
            partyNames = new List<string>
            {
                "Party A",
                "Party B",
                "Party C"
                // Add more party names as needed
            };
        }
        public static string SelectParty()
        {
            Console.WriteLine("List of Party Names:");
            for (int i = 0; i < partyNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {partyNames[i]}");
            }

            // Prompt user to enter the index of the selected party
            Console.WriteLine("Enter the number corresponding to the party:");
            int partyIndex;
            while (!int.TryParse(Console.ReadLine(), out partyIndex) || partyIndex < 1 || partyIndex > partyNames.Count)
            {
                Console.WriteLine("Invalid input. Please enter a valid party number.");
            }

            // Get the selected party name based on the index
            string selectedPartyName = partyNames[partyIndex - 1];

            return selectedPartyName;
        }
    public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        public static void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
