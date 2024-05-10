namespace GeekShopping.Email.Messages
{
    public class UpdatePayamentResultMessage
    {
        public long OrderID { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
