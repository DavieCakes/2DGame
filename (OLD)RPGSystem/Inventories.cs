using System.Collections.Generic;
using Items;

namespace Inventories {
    public class Inventory : List<Item>
    {
        public Inventory()
        {
        }

        public Inventory(IEnumerable<Item> collection) : base(collection)
        {
        }

        public Inventory(int capacity) : base(capacity)
        {
        }
    }

    public class Test {
        public Test() {
            Inventory i = new Inventory();
            Item item = new Item("name", 1);
            i.Add(item); // success
        }
    }
}