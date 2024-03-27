using System;
using System.Data.SqlClient;

namespace QueenData
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerCNIC { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerAddress { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public string ProductSize { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }

    public class OrderDAL
    {
        private string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=QueenLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void InsertOrder(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Orders (CustomerCNIC, CustomerName, CustomerContact, CustomerAddress, ProductCode, Price, ProductSize, ProductName, ProductQuantity) " +
                                   "VALUES (@CNIC, @Name, @Contact, @Address, @ProductCode, @Price, @Size, @ProductName, @ProductQuantity)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CNIC", order.CustomerCNIC);
                        command.Parameters.AddWithValue("@Name", order.CustomerName);
                        command.Parameters.AddWithValue("@Contact", order.CustomerContact);
                        command.Parameters.AddWithValue("@Address", order.CustomerAddress);
                        command.Parameters.AddWithValue("@ProductCode", order.ProductCode);
                        command.Parameters.AddWithValue("@Price", order.Price);
                        command.Parameters.AddWithValue("@Size", order.ProductSize);
                        command.Parameters.AddWithValue("@ProductName", order.ProductName);
                        command.Parameters.AddWithValue("@ProductQuantity", order.ProductQuantity);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting order: " + ex.Message);
            }
        }

        public void UpdateOrderAddress(string phone, string newAddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Orders SET CustomerAddress = @NewAddress WHERE CustomerContact = @Phone";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewAddress", newAddress);
                        command.Parameters.AddWithValue("@Phone", phone);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating order address: " + ex.Message);
            }
        }

        public void DeleteOrder(int orderID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderID);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting order: " + ex.Message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OrderDAL orderDAL = new OrderDAL();
            int choice;
            do
            {
                Console.WriteLine("1. Insert Order");
                Console.WriteLine("2. Update Customer's Address");
                Console.WriteLine("3. Delete Order");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid choice! Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Order Details:");
                        Order newOrder = new Order();
                        Console.Write("Customer CNIC: ");
                        newOrder.CustomerCNIC = Console.ReadLine();
                        Console.Write("Customer Name: ");
                        newOrder.CustomerName = Console.ReadLine();
                        Console.Write("Customer Contact: ");
                        newOrder.CustomerContact = Console.ReadLine();
                        Console.Write("Customer Address: ");
                        newOrder.CustomerAddress = Console.ReadLine();
                        Console.Write("Product Code: ");
                        newOrder.ProductCode = Console.ReadLine();
                        Console.Write("Price: ");
                        if (!decimal.TryParse(Console.ReadLine(), out newOrder.Price))
                        {
                            Console.WriteLine("Invalid price! Please enter a valid decimal number.");
                            continue;
                        }
                        Console.Write("Product Size: ");
                        newOrder.ProductSize = Console.ReadLine();
                        Console.Write("Product Name: ");
                        newOrder.ProductName = Console.ReadLine();
                        Console.Write("Product Quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out newOrder.ProductQuantity))
                        {
                            Console.WriteLine("Invalid quantity! Please enter a valid integer number.");
                            continue;
                        }
                        orderDAL.InsertOrder(newOrder);
                        Console.WriteLine("Order inserted successfully!");
                        break;
                    case 2:
                        Console.Write("Enter Customer Contact Number: ");
                        string phone = Console.ReadLine();
                        Console.Write("Enter New Address: ");
                        string newAddress = Console.ReadLine();
                        orderDAL.UpdateOrderAddress(phone, newAddress);
                        Console.WriteLine("Customer's address updated successfully!");
                        break;
                    case 3:
                        Console.Write("Enter Order ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int orderID))
                        {
                            Console.WriteLine("Invalid order ID! Please enter a valid integer number.");
                            continue;
                        }
                        orderDAL.DeleteOrder(orderID);
                        Console.WriteLine("Order deleted successfully!");
                        break;
                    case 4:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            } while (choice != 4);
        }
    }
}
