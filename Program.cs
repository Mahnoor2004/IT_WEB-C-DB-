using System;
using System.Data.SqlClient;
using System.Data;

namespace bitf21m025_web3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QueenDB;Integrated Security=True";
            string query = "Select * from Orders";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand selectCmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = selectCmd;
            DataTable order = new DataTable();
            adapter.Fill(order);
            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Modify Order");
                Console.WriteLine("2. Add Order");
                Console.WriteLine("3. Remove Order");
                Console.WriteLine("4. Commit Changes ");
                Console.WriteLine("5. Exit");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        ModifyOrder(order);
                        break;

                    case 2:
                        AddOrder(order);
                        break;

                    case 3:
                        RemoveOrder(order);
                        break;

                    case 4:
                        CommitChanges(adapter, order);
                        Console.WriteLine("Changes commited to database.");
                        break;

                    case 5:
                        Console.WriteLine("Exiting....");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                        break;
                }
            }
        }

        static void ModifyOrder(DataTable order)
        {
            Console.WriteLine("Enter OrderID to modify details:");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Invalid input. Please enter a valid OrderID.");
                return;
            }

            DataRow[] foundRows = order.Select($"OrderID = {orderId}");

            if (foundRows.Length == 0)
            {
                Console.WriteLine($"No order found with OrderID: {orderId}");
                return;
            }

            Console.WriteLine("Enter new quantity:");
            if (!int.TryParse(Console.ReadLine(), out int newQuantity))
            {
                Console.WriteLine("Invalid input. Please enter a valid quantity.");
                return;
            }

            foundRows[0]["ProductQuantity"] = newQuantity;
            Console.WriteLine("Order details modified successfully.");
        }
        //add Order
        static void AddOrder(DataTable order)
        {
            DataRow newOrderRow = order.NewRow();

            Console.WriteLine("Enter ProductName:");
            newOrderRow["ProductName"] = Console.ReadLine();

            Console.WriteLine("Enter ProductCode:");
            newOrderRow["ProductCode"] = Console.ReadLine();

            Console.WriteLine("Enter ProductSize:");
            newOrderRow["ProductSize"] = Console.ReadLine();

            Console.WriteLine("Enter CustomerAddress:");
            newOrderRow["CustomerAddress"] = Console.ReadLine();

            Console.WriteLine("Enter CustomerContact:");
            newOrderRow["CustomerContact"] = Console.ReadLine();

            Console.WriteLine("Enter ProductQuantity:");
            int quantity;
            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid input. Please enter a valid quantity.");
                return;
            }
            newOrderRow["ProductQuantity"] = quantity;

            Console.WriteLine("Enter Price:");
            int price;
            if (!int.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Invalid input. Please enter a valid price.");
                return;
            }
            newOrderRow["Price"] = price;

            Console.WriteLine("Enter CustomerID:");
            newOrderRow["CustomerID"] = Console.ReadLine();

            Console.WriteLine("Enter CustomerName:");
            newOrderRow["CustomerName"] = Console.ReadLine();

            order.Rows.Add(newOrderRow);
            Console.WriteLine("New order added successfully.");
        }

        //remove Order
        static void RemoveOrder(DataTable order)
        {
            Console.WriteLine("Enter OrderID to remove:");
            int orderId;
            if (!int.TryParse(Console.ReadLine(), out orderId))
            {
                Console.WriteLine("Invalid input. Please enter a valid OrderID.");
                return;
            }
            DataRow[] foundRows = order.Select($"OrderID = {orderId}");
            if (foundRows.Length == 0)
            {
                Console.WriteLine($"No order found with OrderID: {orderId}");
                return;
            }
            foundRows[0].Delete();
            Console.WriteLine("Order removed successfully.");
        }
        //Commit Changes
        static void CommitChanges(SqlDataAdapter dataAdapter, DataTable order)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Update(order);
        }

    }
}








