namespace adisyon.Models;

public class CreateOrder
{
    public int User_id { get; set; }
    public int Table_number { get; set; }
    public int Product_id { get; set; }
    public int Quantity { get; set; }
}
