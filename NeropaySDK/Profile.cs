namespace PaymixSDK
{
    public class Profile
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string mobilePrefix { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string title { get; set; }
        public PaymixAddress residentialAddress { get; set; }
    }
}