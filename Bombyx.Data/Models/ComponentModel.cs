namespace Bombyx.Data.Models
{
    public class ComponentModel
    {
        public string SortCode { get; set; }
        public string NameEnglish { get; set; }
        public string NameGerman { get; set; }
        public string NameFrench { get; set; }
        public decimal Density { get; set; }
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
        public decimal ThermalCond { get; set; }
        public decimal Thickness { get; set; }
        public int Layer { get; set; }
        public decimal Uvalue { get; set; }
        public decimal Resistance { get; set; }
    }
}
