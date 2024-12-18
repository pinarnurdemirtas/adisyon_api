namespace adisyon.Models;

public class Orders
{
    public int Order_id { get; set; }
    public int User_id { get; set; }
    public int Table_number { get; set; }
    public int Product_id { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
    public DateTime Order_date { get; set; }
    public string Product_name { get; set; }

}