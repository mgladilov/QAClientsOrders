using System.Data.Entity;
using QAClientsOrders.Data.DB;
using QAClientsOrders.Data.Models;
using QAClientsOrders.Helper;

namespace QAClientsOrders;

public class DisplayOrdersTable
{
    public void ManageOrders()
    {
        while (true)
        {
            Console.WriteLine("Please select an action:");
            Console.WriteLine("1. Add order");
            Console.WriteLine("2. Edit order");
            Console.WriteLine("3. Delete order");
            Console.WriteLine("4. Show orders by client");
            Console.WriteLine("5. Return");
            Console.WriteLine("6. Exit");

            int choice = ConsoleReader<int>.Read("Your choice: ");

            switch (choice)
            {
                case 1:
                    AddOrder();
                    break;
                case 2:
                    EditOrder();
                    break;
                case 3:
                    DeleteOrder();
                    break;
                case 4:
                    ShowOrdersByClient();
                    break;
                case 5:
                    return;
                case 6:
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    
    static void AddOrder()
    {
        using (var db = new AppDbContext())
        {
            int clientID = ConsoleReader<int>.Read("Client ID: ");

            var client = db.Clients.Find(clientID);

            if (client == null)
            {
                Console.WriteLine($"Client with ID {clientID} not found.");
                return;
            }

            string description = ConsoleReader<string>.Read("Description: ");
            float price = ConsoleReader<int>.Read("Price: ");
            DateTime orderDate = ConsoleReader<DateTime>.Read("Order date (dd/MM/yyyy): ");

            var order = new Order
            {
                Client = client,
                Description = description,
                OrderPrice = price,
                OrderDate = orderDate
            };

            db.Orders.Add(order);

            client.OrderAmount++;
            db.SaveChanges();

            Console.WriteLine("Order added successfully.");
        }
    }

    static void EditOrder()
    {
        using (var db = new AppDbContext())
        {
            int id = ConsoleReader<int>.Read("Order ID: ");

            var order = db.Orders.Find(id);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            string description = ConsoleReader<string>.Read("Description: ");
            float price = ConsoleReader<float>.Read("Price: ");
            DateTime orderDate = ConsoleReader<DateTime>.Read("Order date (dd/MM/yyyy): ");

            order.Description = description;
            order.OrderPrice = price;
            order.OrderDate = orderDate;

            db.SaveChanges();

            Console.WriteLine("Order updated successfully.");
        }
    }

    static void DeleteOrder()
    {
        using (var db = new AppDbContext())
        {
            int id = ConsoleReader<int>.Read("Order ID: ");

            var order = db.Orders.Find(id);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            db.Orders.Remove(order);

            var client = order.Client;
            client.OrderAmount--;

            db.SaveChanges();

            Console.WriteLine("Order deleted successfully.");
        }
    }

    static void ShowOrdersByClient()
    {
        using (var db = new AppDbContext())
        {
            int id = ConsoleReader<int>.Read("Client ID: ");

            var client = db.Clients.Find(id);

            if (client == null)
            {
                Console.WriteLine("Client not found.");
                return;
            }

            Console.WriteLine($"Client: {client.FirstName} {client.SecondName} ({client.PhoneNum})");
            Console.WriteLine($"Date added: {client.DateAdd:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Number of orders: {client.OrderAmount}");

            var orders = db.Orders.Include(o => o.Client).Where(o => o.ClientID == id).ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found for this client.");
            }
            else
            {
                Console.WriteLine("Orders:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"ID: {order.Id}");
                    Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine($"Description: {order.Description}");
                    Console.WriteLine($"Price: {order.OrderPrice:C}");
                    Console.WriteLine(
                        $"Close date: {(order.CloseDate == null ? "Not closed" : order.CloseDate.ToString("dd/MM/yyyy"))}");
                    Console.WriteLine($"Client: {order.Client.FirstName} {order.Client.SecondName}");
                    Console.WriteLine();
                }
            }
        }
    }
}