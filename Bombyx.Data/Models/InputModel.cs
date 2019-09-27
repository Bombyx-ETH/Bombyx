namespace Bombyx.Data.Models
{
    public class InputModel
    {
        public string SortCode { get; set; }
        public string ComponentCode { get; set; }
        public string NameEnglish { get; set; }
        public decimal UBP13Embodied { get; set; }
        public decimal UBP13EoL { get; set; }
        public decimal TotalEmbodied { get; set; }
        public decimal TotalEoL { get; set; }
        public decimal RenewableEmbodied { get; set; }
        public decimal RenewableEoL { get; set; }
        public decimal NonRenewableEmbodied { get; set; }
        public decimal NonRenewableEoL { get; set; }
        public decimal GHGEmbodied { get; set; }
        public decimal GHGEoL { get; set; }
        public decimal? Resistance { get; set; }
    }
}
