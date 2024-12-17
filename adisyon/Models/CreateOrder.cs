namespace adisyon.Models;

// Kullanıcıdan sipariş oluşturmak için gereken veri modeli
public class CreateOrder
{
    public int Table_number { get; set; }
    public int Product_id { get; set; }
    public int Quantity { get; set; }
}
