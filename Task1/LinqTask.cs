using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
            => customers.Where(x => x.Orders.Count() > limit);

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        ) => customers.Select(x => (x, suppliers.Where(s => s.City == x.City)));

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            var suppliersByCountry = suppliers.GroupBy(x => x.City);
            return customers.Select(x => (x, suppliersByCountry.Where(sc => sc.Key == x.City).SelectMany(s => s)));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
            => customers.Where(x => x.Orders.Any(x => x.Total > limit));

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        ) => customers.Where(c => c.Orders.Length > 0).Select(x => (x, x.Orders.Min(x => x.OrderDate)));

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        ) => Linq4(customers).OrderBy(x => x.dateOfEntry).ThenBy(x => x.customer.Orders.Count()).ThenBy(x => x.customer.CompanyName);

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
            => customers.Where(x =>
            x.PostalCode.Any(c => !char.IsDigit(c))
            || string.IsNullOrEmpty(x.Region)
            || (!x.Phone.Contains('(') && !x.Phone.Contains(')')));

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */

            return products.GroupBy(x => x.Category)
                .Select(productsByCategory => new Linq7CategoryGroup()
                {
                    Category = productsByCategory.Key,
                    UnitsInStockGroup = productsByCategory.GroupBy(p => p.UnitsInStock).Select(productsByUnitsInStock => new Linq7UnitsInStockGroup()
                    {
                        UnitsInStock = productsByUnitsInStock.Key,
                        Prices = productsByUnitsInStock.Select(p => p.UnitPrice),
                    }),
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            var categories = new[] {0, cheap, middle, expensive};
            return categories[1..].Select(c => (c, products.Where(p => p.UnitPrice > categories[Array.IndexOf(categories, c) - 1] && p.UnitPrice <= c)));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        ) => customers.GroupBy(x => x.City)
                .Select(customersByCity => (customersByCity.Key, (int)Math.Round(customersByCity.Average(c => c.Orders.Sum(o => o.Total))), (int)customersByCity.Average(c => c.Orders.Count())));

        public static string Linq10(IEnumerable<Supplier> suppliers)
            => string.Concat(suppliers.Select(x => x.Country).Distinct().OrderBy(x => x.Length).ThenBy(x => x));
    }
}