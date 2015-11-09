namespace Nop.Plugin.Payments.Mellat.Models
{
    public class InstallmentInfoModel
    {
        public int ProductId { get; set; }
        public decimal FinallPrice { get; set; }
        public long PrePay { get; set; }
        public int Months { get; set; }

    }
}
