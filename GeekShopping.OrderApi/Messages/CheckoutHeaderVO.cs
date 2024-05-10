﻿using GeekShopping.MessageBus;

namespace GeekShopping.OrderApi.Messages
{
    public class CheckoutHeaderVO : BaseMessage
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string CouponCode { get; set; }

        public decimal PurchaseAmount { get; set; }

        public decimal DiscountTotal { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateTime { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpiryYear { get; set; }

        public int CartTotalItens { get; set; }
        public IEnumerable<CartDetailVO> CartDetails { get; set; }
    }
}