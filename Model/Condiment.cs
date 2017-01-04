using System.ComponentModel;

namespace Model
{
    public enum CondimentType
    {
        [Description("Cream")]
        Cream,
        [Description("Sugar")]
        Sugar
    }
    public class Condiment
    {
        public CondimentType CondimentType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
