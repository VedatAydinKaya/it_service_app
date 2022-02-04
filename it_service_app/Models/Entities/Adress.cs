namespace it_service_app.Models.Entities
{
    public class Adress:BaseEntity
    {
        public string Line { get; set; }

        public string PostCode { get; set; }

        public string AdressType { get; set; }
    }

    public enum AdressTypes 
    {
        Fatura,
        Teslimat
    }
}
