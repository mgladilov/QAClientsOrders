namespace QAClientsOrders.Data.Models;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public int ClientID { get; set; }
    public string Description { get; set; }
    public float OrderPrice { get; set; }
    public DateTime CloseDate { get; set; }
    
    public virtual Client Client { get; set; }
}