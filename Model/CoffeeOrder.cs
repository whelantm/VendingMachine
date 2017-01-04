using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class CoffeeOrder
    {
        public Coffee Coffee { get; set; }
        public IDictionary<CondimentType, Condiment> Condiments { get; set; }

        public CoffeeOrder()
        {
            Condiments = new Dictionary<CondimentType, Condiment>();
        }
        public decimal Price()
        {
            decimal price = Coffee?.Price ?? 0;
            foreach(var condiment in Condiments)
            {
                price += condiment.Value?.Price ?? 0;
            }
            return price;
        }

        public override string ToString()
        {
            StringBuilder stb = new StringBuilder();
            stb.AppendFormat("Coffee: {0} ", Coffee.Size.ToString());
            foreach (var c in Condiments)
            {
                stb.AppendFormat("{0} {1} ", c.Value.CondimentType.ToString(), c.Value.Quantity);
            }

            return stb.ToString();
        }
    }
}
