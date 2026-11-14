using System;
using System.Collections.Generic;
struct Purchase
{
    public string ProductName;
    public int Price;
}
class Customer
{
    public int CardNumber;
    public int Money;
    public int ProductsBought;
    public int MoneySpent;
}
class Product
{
    public string Name = "";
    public int Price;
    public int Quantity;
}
class Shop
{
    public int Profit = 0;
    public List<Product> Products = new List<Product>();
    public void AddProducts()
    {
        Products.Add(new Product { Name = "Хліб", Price = 30, Quantity = 20 });
        Products.Add(new Product { Name = "Молоко", Price = 45, Quantity = 15 });
        Products.Add(new Product { Name = "Сир", Price = 120, Quantity = 10 });
        Products.Add(new Product { Name = "Шоколад", Price = 60, Quantity = 25 });
        Products.Add(new Product { Name = "Сік", Price = 50, Quantity = 18 });
    }
    public void SellProducts(Customer customer, Random rnd)
    {
        while (customer.Money > 0)
        {
            List<Product> available = Products.FindAll(
                p => p.Quantity > 0 && p.Price <= customer.Money);
            if (available.Count == 0)
                break;
            Product product = available[rnd.Next(available.Count)];
            Purchase purchase = new Purchase
            {
                ProductName = product.Name,
                Price = product.Price
            };
            customer.Money -= purchase.Price;
            customer.MoneySpent += purchase.Price;
            customer.ProductsBought++;
            product.Quantity--;
            Profit += purchase.Price;
            Console.WriteLine(
                $"Карта {customer.CardNumber}: купив {product.Name}");
        }
    }
}
class Program
{
    static void Main()
    {
        Random rnd = new Random();
        Shop shop = new Shop();
        shop.AddProducts();
        Console.Write("Введіть кількість покупців: ");
        int count = Convert.ToInt32(Console.ReadLine());
        if (count <=0)
        {
            Console.WriteLine("Кількість покупців повинна бути більшою за нуль.");
            return;
        }
        List<Customer> customers = new List<Customer>();
        for (int i = 0; i < count; i++)
        {
            Customer customer = new Customer
            {
                CardNumber = rnd.Next(1, 1001),
                Money = rnd.Next(1000, 10001),
                ProductsBought = 0,
                MoneySpent = 0,
            };
            customers.Add(customer);
        }
        foreach (Customer customer in customers)
        {
            Console.WriteLine($"nПокупаць з картою {customer.CardNumber}");
            shop.SellProducts(customer, rnd);
        }
        Customer mostProducts = customers[0];
        Customer mostSpent = customers[0];
        foreach (Customer customer in customers)
        {
            if (customer.ProductsBought > mostProducts.ProductsBought)
                mostProducts = customer;
            if (customer.MoneySpent > mostSpent.MoneySpent)
                mostSpent = customer;
        }
        Console.WriteLine("\n========== РЕЗУЛЬТАТ ==========");
        Console.WriteLine($"Прибуток магазину: {shop.Profit}");
        Console.WriteLine($"Більше всього товарів купила карта: {mostProducts.CardNumber}");
        Console.WriteLine($"Кількість товарів: {mostProducts.ProductsBought}");
        Console.WriteLine($"Більше всього грошей витратила карта: {mostSpent.CardNumber}");
        Console.WriteLine($"Витрачено грошей: {mostSpent.MoneySpent}");
    }
}