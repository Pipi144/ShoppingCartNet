namespace ShoppingCart.Models;

public class CartItem
{
    public Product Product {get; set;}
    public int Quantity {get; set;}
    
    public decimal TotalPrice {get; set;}

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        TotalPrice = (decimal)product.Price * quantity;
    }
    
    

    public void AdjustQuantity(int quantity)
    {
        Quantity = quantity;
        TotalPrice = (decimal)(Product.Price * Quantity);
    }
    
}