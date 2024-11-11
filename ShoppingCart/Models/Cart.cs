namespace ShoppingCart.Models;

public class Cart
{
    
    public List<CartItem> Items { get; set; }
    public decimal Total { get; set; }

    public Cart(List<CartItem>? items)
    {
        if (items != null)
        {
            Items = items;
            Total = items.Sum(item => item.TotalPrice);
        }
        else
        {
            Items = new List<CartItem>();
        }
    }

    public void RemoveCartItem(CartItem item)
    {
        Items.Remove(item);
        Total -= item.TotalPrice;
    }
    
    public void AddToCart(CartItem item)
    {
        // check if the item is already in cart or not
        var existingItem = Items.FirstOrDefault(cartItem => item.Product.ProductId == cartItem.Product.ProductId);
        
        // if not=> add the item and update total
        if (existingItem == null)
        {
            Items.Add(item);
            Total += item.TotalPrice;
        }
        //if yes update total and the item by itself
        else
        {
            // recalculate the total
            var tempTotal= Total - existingItem.TotalPrice;
            existingItem.AdjustQuantity(existingItem.Quantity+=item.Quantity);
            Total = tempTotal + existingItem.TotalPrice;
        }
        
    }
    
    public void AdjustCartQuantity(CartItem item, int quantity)
    {
        var prevTotal = Total - item.TotalPrice;
        // check if the item is already in cart or not
        var existingItem = Items.FirstOrDefault(cartItem => item.Product.ProductId == cartItem.Product.ProductId);
        // update when found item
        if (existingItem != null)
        {
             // recalculate the total
            var tempTotal= Total - existingItem.TotalPrice;
            existingItem.AdjustQuantity(quantity);
            Total = tempTotal + existingItem.TotalPrice;
        }
    }
}