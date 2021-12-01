namespace PizzaCore.Data.Entities {
  public class CartItem {
    public int Id { get; set; }
    public ProductSize ProductSize { get; set; }
    public int Quantity { get; set; }
  }
}
