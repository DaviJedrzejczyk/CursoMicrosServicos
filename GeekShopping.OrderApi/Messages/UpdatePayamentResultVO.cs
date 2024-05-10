namespace GeekShopping.OrderApi.Messages
{
    public class UpdatePayamentResultVO
    {
        public long OrderID { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
