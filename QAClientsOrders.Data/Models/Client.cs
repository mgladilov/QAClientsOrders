namespace QAClientsOrders.Data.Models;

public class Client : BaseEntity
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string PhoneNum { get; set; }
    public int OrderAmount { get; set; }
    public DateTime DateAdd { get; set; }
}