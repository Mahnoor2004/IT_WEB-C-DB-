using System;

namespace QueenLocalDataHandling
{
    class Program
    {
        static void Main()
        {
            OrderCRUD orderCRUD = new OrderCRUD();

            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Insert Order");
                Console.WriteLine("2. Display All Orders");
                Console.WriteLine("3. Update Address");
                Console.WriteLine("4. Delete Order");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        InsertOrder(orderCRUD);
                        break;
                    case "2":
                        DisplayAllOrders(orderCRUD);
                        break;
                    case "3":
                        UpdateAddress(orderCRUD);
                        break;
                    case "4":
                        DeleteOrder(orderCRUD);
                        break;
                    case "5":
                        Console.WriteLine("Exiting the program.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void InsertOrder(OrderCRUD orderCRUD)
        {
            Console.WriteLine("Insert Order:");
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            Console.Write("Enter Customer CNIC: ");
            string cnic = Console.ReadLine();

            Console.Write("Enter Customer Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Customer Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Customer Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Product ID: ");
            string productId = Console.ReadLine();

            Console.Write("Enter Price: ");
            float price = float.Parse(Console.ReadLine());

            Console.Write("Enter Product Size: ");
            string productSize = Console.ReadLine();
            Order newOrder = new Order(orderId, cnic, name, phone, address, productId, price);
            newOrder.SetProductSize(productSize);
            orderCRUD.InsertOrder(newOrder);

            Console.WriteLine("Order inserted successfully.");
        }
        static void DisplayAllOrders(OrderCRUD orderCRUD)
        {
            Console.WriteLine("Displaying All Orders:");
            orderCRUD.GetAllOrders();
        }

        static void UpdateAddress(OrderCRUD orderCRUD)
        {
            Console.WriteLine("Update Address:");
            Console.Write("Enter Customer Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter New Address: ");
            string newAddress = Console.ReadLine();

            orderCRUD.UpdateAddress(phone, newAddress);
        }

        static void DeleteOrder(OrderCRUD orderCRUD)
        {
            Console.WriteLine("Delete Order:");
            Console.Write("Enter Order ID to delete: ");
            int orderId = int.Parse(Console.ReadLine());
            orderCRUD.DeleteOrder(orderId);
        }
    }
}
