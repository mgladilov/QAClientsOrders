using QAClientsOrders.Data.DB;

using (AppDbContext db = new AppDbContext())
{
    db.Seed();
}