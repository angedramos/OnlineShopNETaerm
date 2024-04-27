using Newtonsoft.Json;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;
using System.Net;
using System.Net.Mail;

namespace OnlineShopNET.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IUserRepository userRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        public async Task<CreateOrderDTO> PlaceOrder(int userId, List<ProductsListDTO> productlist)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException(Constant_Messages.USER_NOT_FOUND);
            }

            decimal totalPrice = 0;

            //Check If ProductList has any duplicate ProductID
            var uniqueData = productlist.GroupBy(item => item.productID).Select(group => group.First());

            foreach (var entry in uniqueData)
            {
                int productId = entry.productID;
                int quantity = entry.product_quantity;

                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    throw new ArgumentException(Constant_Messages.PRODUCT_NOT_FOUND);
                }
                if( quantity == 0 ) 
                {
                    throw new ArgumentException(Constant_Messages.QUANTITY_REQUIRED);
                }
                if(quantity > product.product_quantity)
                {
                    throw new ArgumentException(Constant_Messages.PRODUCT_LIMIT_EXCEED);
                }

                UpdateProductQuantity productQuantity = new UpdateProductQuantity
                {
                    productID = product.productId,
                    product_quantity = product.product_quantity - quantity
                };

                await _productRepository.UpdateQuantity(productQuantity);

                totalPrice += (product.product_price * quantity);
            }
            string json = JsonConvert.SerializeObject(uniqueData);

            Order2 order2 = new Order2
            {
                userID = user.user_id,
                orderDate = DateTime.UtcNow,
                purchase = json,
                totalPrice = totalPrice
            };
      
            // Save the order to the database
            bool saveOrder = await _orderRepository.CreateOrder(order2);
            try 
            { 
                if(saveOrder)
                {
                    await SendEmailAsync(user.email, user.username, totalPrice);
                    CreateOrderDTO finalOrder = new CreateOrderDTO
                    {
                        userID = user.user_id,
                        OrderDate = DateTime.UtcNow,
                        purchase = json,
                        OrderTotalPrice = totalPrice
                    };
                    return finalOrder;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constant_Messages.UNSUCCESSFUL_ORDER_CREATING + ex.Message);
            }
            return null;
        }

        public Task SendEmailAsync(string email, string user, decimal totalPrice)
        {
            var mail = Constant_Messages.STORE_EMAIL;
            var password = Constant_Messages.STORE_PASSWORD;
            var subject = Constant_Messages.STORE_SUBJECT;
            var message = $"User {user}, your purchase with a cost of {totalPrice} has been successfully paid off. \nThanks for your trust in OnlineShopNETAerm. \nHope to see you soon. ";

            SmtpClient client = new SmtpClient(Constant_Messages.SMTP_EMAIL);
            client.Port = 587; 
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mail, password);
            client.EnableSsl = true;

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
    }
}
