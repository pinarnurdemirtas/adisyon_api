namespace adisyon.Models;

public class Siparis
{
    public int Id { get; set; }
    public int table_number { get; set; }
    public int product_id { get; set; }
    public int quantity { get; set; }
    public string status { get; set; }
    public DateTime order_date { get; set; }

}