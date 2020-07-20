using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EFD.Data.EfCore;

namespace EFD
{
    public class CustomerDemo//farklı bir classta veri taşiycaz burada
    {
        public CustomerDemo()
        {
            Orders = new List<OrderDemo>();
        }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int OrderCount { get; set; }
        public List<OrderDemo> Orders { get; set; }//bir müşterinin birden fazla ordeı olabilir
    }
    public class OrderDemo//ilgili müşteri hangi siparişle ilişkili bilgisi var
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public List<ProductDemo> ProductDemos { get; set; }
    }

    public class ProductDemo
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new NorthwindContext())
            {
                var Customers = db.Customers
                .Where(i => i.Orders.Count() > 0)//herhangi bir müşterinin en az bir kaydı varsa gelir
                .Select(i => new CustomerDemo
                {
                    CustomerId = i.Id,
                    Name = i.FirstName,
                    OrderCount = i.Orders.Count(),
                    Orders = i.Orders.Select(a => new OrderDemo
                    {
                        OrderId = a.Id,
                        Total = (decimal)a.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),//başa decimal yazmsaam kızar
                        //bu kayıtlar orderdetaile gittiği için kayıtları orderdetaile gönderiyorum
                        ProductDemos=a.OrderDetails.Select(p=>new ProductDemo{
                            ProductId=(int)p.ProductId,
                            Name=p.Product.ProductName,
                        }).ToList(),
                    }).ToList()
                })
                .OrderBy(i => i.OrderCount)//artan azalan şeklinge gelsin sipariş veren oranı
                .ToList();
                foreach (var customer in Customers)
                {
                    Console.WriteLine(customer.CustomerId + " - " + customer.Name + " - " + customer.OrderCount);
                    foreach (var item in customer.Orders)
                    {
                        Console.WriteLine("Order id: " + item.OrderId + " Total: " + item.Total);
                    }
                    
                }
            }



        }
    }
}








