using System;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
//price can also be taken as float (better practice)
namespace QueenLocalDataHandling
{
    public class OrderCRUD
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QueensDB;Integrated Security=True";
        public void InsertOrder(Order newOrder)
        {
            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder", "The order object cannot be null");
            }
            if (string.IsNullOrWhiteSpace(newOrder.CNIC))
            {
                throw new ArgumentNullException("CustomerCNIC", "Customer CNIC is required");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Orders ([Order Id], [Customer CNIC], [Customer Name], [Customer Phone], [Customer Address], [Product ID], [Price], [Product Size]) VALUES (@OrderID, @CustomerCNIC, @CustomerName, @CustomerPhone, @CustomerAddress, @ProductID, @Price, @ProductSize)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", newOrder.OrderID);
                    command.Parameters.AddWithValue("@CustomerCNIC", newOrder.CNIC);
                    command.Parameters.AddWithValue("@CustomerName", newOrder.Name);
                    command.Parameters.AddWithValue("@CustomerPhone", newOrder.Phone);
                    command.Parameters.AddWithValue("@CustomerAddress", newOrder.Address);
                    command.Parameters.AddWithValue("@ProductID", newOrder.ProductID);
                    command.Parameters.AddWithValue("@Price", newOrder.Price);
                    command.Parameters.AddWithValue("@ProductSize", newOrder.ProductSize);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void GetAllOrders()
        {
            string query = "SELECT * FROM Orders";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int orderId = (int)reader["Order Id"];
                        string cnic = reader["Customer CNIC"].ToString();
                        string name = reader["Customer Name"].ToString();
                        string phone = reader["Customer Phone"].ToString();
                        string address = reader["Customer Address"].ToString();
                        string productId = reader["Product ID"].ToString();
                        int price = (int)reader["Price"];
                        string productSize = reader["Product Size"].ToString();


                        Console.WriteLine($"Order ID: {orderId}");
                        Console.WriteLine($"Customer CNIC: {cnic}");
                        Console.WriteLine($"Customer Name: {name}");
                        Console.WriteLine($"Customer Phone: {phone}");
                        Console.WriteLine($"Customer Address: {address}");
                        Console.WriteLine($"Product ID: {productId}");
                        Console.WriteLine($"Price: {price}");
                        Console.WriteLine($"Product Size: {productSize}");
                        Console.WriteLine("---------------------------------------------");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void UpdateAddress(string phone, string newAddress)
        {
            string updateCommandText = "UPDATE Orders SET [Customer Address] = @NewAddress WHERE [Customer Phone] = @Phone";
            //string updateCommandText = "UPDATE Orders SET [Customer Address] = '" + newAddress + "' WHERE [Customer Phone] = '" + phone + "'";
            //comment out the parameterized values
            //SQL Injection (Concatenated Query)
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateCommandText, connection))
                {
                    command.Parameters.AddWithValue("@NewAddress", newAddress);
                    command.Parameters.AddWithValue("@Phone", phone);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Address updated successfully for phone number {phone}");
                    }
                    else
                    {
                        Console.WriteLine($"No order found with phone number {phone}");
                    }
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            string deleteCommandText = "DELETE FROM Orders WHERE [Order Id] = @OrderID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteCommandText, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Order with ID {orderId} deleted successfully");
                    }
                    else
                    {
                        Console.WriteLine($"No order found with ID {orderId}");
                    }
                }
            }
        }
    }
}

