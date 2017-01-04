using System.ComponentModel;

namespace Model
{
    public enum CupSize
    {
        [Description("Small")]
        Small,
        [Description("Medium")]
        Medium,
        [Description("Large")]
        Large
    }
    public class Coffee
    {
        public CupSize Size { get; set; }
        public decimal Price { get; set; }
    }
}
