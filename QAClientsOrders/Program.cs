using QAClientsOrders;
using QAClientsOrders.Data.DB;
using QAClientsOrders.Helper;

using (AppDbContext db = new AppDbContext())
{
    db.Seed();
    
}

DisplayClientsTable clientsTable = new DisplayClientsTable();
DisplayOrdersTable ordersTable = new DisplayOrdersTable();
Console.WriteLine("Welcome to the client and order management system!");

while (true)
{
    Console.WriteLine("Please select a table:");
    Console.WriteLine("1. Clients");
    Console.WriteLine("2. Orders");
    Console.WriteLine("3. Exit");

    int choice = ConsoleReader<int>.Read("Your choice: ");

    switch (choice)
    {
        case 1:
            clientsTable.ManageClients();
            break;
        case 2:
            ordersTable.ManageOrders();
            break;
        case 3:
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
    