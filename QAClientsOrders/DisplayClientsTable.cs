using Microsoft.EntityFrameworkCore;
using QAClientsOrders.Data.DB;
using QAClientsOrders.Data.Models;
using QAClientsOrders.Helper;

namespace QAClientsOrders;

public class DisplayClientsTable
{
    public void ManageClients()
    {
        while (true)
        {
            Console.WriteLine("Please select an action:");
            Console.WriteLine("1. Add client");
            Console.WriteLine("2. Edit client");
            Console.WriteLine("3. Delete client");
            Console.WriteLine("4. Show client's orders");
            Console.WriteLine("5. Return");
            Console.WriteLine("6. Exit");
            int choice = ConsoleReader<int>.Read("Your choice: ");

            switch (choice)
            {
                case 1:
                    AddClient();
                    break;
                case 2:
                    EditClient();
                    break;
                case 3:
                    DeleteClient();
                    break;
                case 4:
                    ShowClientOrders();
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
    
    static void AddClient()
    {
        using (var db = new AppDbContext())
        {
            string firstName = ConsoleReader<string>.Read("First name: ");
            string secondName = ConsoleReader<string>.Read("Second name: ");
            string phoneNum = ConsoleReader<string>.Read("Phone number: ");

            var client = new Client
            {
                FirstName = firstName,
                SecondName = secondName,
                PhoneNum = phoneNum,
                DateAdd = DateTime.Now,
                OrderAmount = 0
            };

            db.Clients.Add(client);
            db.SaveChanges();

            Console.WriteLine("Client added successfully.");
        }
    }
    
    static void EditClient()
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

            string firstName = ConsoleReader<string>.Read("First name: ");
            string secondName = ConsoleReader<string>.Read("Second name: ");
            string phoneNum = ConsoleReader<string>.Read("Phone number: ");

            client.FirstName = firstName;
            client.SecondName = secondName;
            client.PhoneNum = phoneNum;

            db.SaveChanges();

            Console.WriteLine("Client updated successfully.");
        }
    }

    static void DeleteClient()
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

            Console.WriteLine(
                $"Are you sure you want to delete the client '{client.FirstName} {client.SecondName}' and all their orders? (Y/N)");
            string answer = Console.ReadLine().ToUpper();

            if (answer == "Y")
            {
                db.Clients.Remove(client);
                db.SaveChanges();

                Console.WriteLine("Client deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
    }
    
    static void ShowClientOrders()
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
                    Console.WriteLine($"Close date: {(order.CloseDate == null ? "Not closed" : order.CloseDate.ToString("dd/MM/yyyy"))}");
                    Console.WriteLine($"Client: {order.Client.FirstName} {order.Client.SecondName}");
                    Console.WriteLine();
                }
            }
        }
    }
}