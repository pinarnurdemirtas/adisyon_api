namespace adisyon.Models;

public class OrderCash
{
    public int Cash_id { get; set; }
    public int Order_id { get; set; }
    public int Product_id { get; set; }
    public int Quantity { get; set; }
    public decimal Product_price { get; set; }
    public decimal Total_price { get; set; }
    public string Status { get; set; }
    public DateTime Order_date { get; set; }
}


