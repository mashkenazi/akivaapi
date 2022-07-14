namespace akivaapi
{
    public class Customer
    {
        public string customerType { get; set; }
        public Company? company { get; set; }
        public Person? person { get; set; }
        public string? country { get; set; }
        public string managingPartnerId { get; set; }
        public string verticalCode { get; set; }
        public Contact[]? contacts { get; set; }

    }
}
