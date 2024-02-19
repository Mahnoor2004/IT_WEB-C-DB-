namespace QueenLocalDataHandling
{
    public class Order
    {
        private int orderID;
        private string cnic;
        private string name;
        private string phone;
        private string address;
        private string productID;
        private float price;
        private string productSize;

        public Order(int orderID, string cnic, string name, string phone, string address, string productID, float price)
        {
            this.orderID = orderID;
            this.cnic = cnic;
            this.name = name;
            this.phone = phone;
            this.address = address;
            this.productID = productID;
            this.price = price;
        }
        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public float Price
        {
            get { return price; }
            set { price = value; }
        }
        public string ProductSize
        {
            get { return productSize; }
            set { productSize = value; }
        }

        public string CNIC
        {
            get { return cnic; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("CNIC cannot be null or empty.");
                }
                cnic = value;
            }
        }

        public void SetProductSize(string size)
        {
            if (size == null)
            {
                throw new ArgumentNullException(nameof(size), "Product size cannot be null.");
            }
            productSize = size;
        }
    }
}
