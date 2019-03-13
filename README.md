# 2DGame

## Useful Links:
class jira:
http://ubuntucraft.dyndns.org:8080/secure/Dashboard.jspa

```csharp
/*
    Notes:
    Everything is an attribute for now, damage, health, strength, AC, anything with a value.
    Attributes have a base value and then some container for modifiers,
    Modifiers keep track of the source (i.e. item) they came from.

    Creatures are only defined by attributes for now (i.e. strength, intelligence...).
    Items are only defined by attribute modifiers for now. (i.e. +5 strength, +2% intelligence, +1 damage).

    For a creature to equip an item, both should exist first(i.e. make creature/make item, 
    then creature.equip(item)), just using some classic, simple aggregation.

    Enums:
        modifer type (percentile, flat)
        attribute type(damage, health, strength..)
 */

namespace Interfaces
{
    public interface Attribute {
        void AddModifier(AttributeModifier modifier);
        bool RemoveModifier(AttributeModifier modifier); // can use as: attribute.RemoveModifer(attribute.modifiers["..or something.."]) // but may not be useful
        bool RemoveAllModifiersFromSource(object source); // creature.equip(item) => creature's attribute is effected, and the attribute keeps track of where each mod came from
    }

    // this is simple, currently using an enum to distinguish percententile (+10%) and flat (+10)
    public interface AttributeModifier {
        /*
            float value
            object source

         */
    }
    public interface Creature {
        void Equip(Item item);
        void Unequip(Item item);
        void Attack(Creature target);
        bool IsDead(); // check if dead... might be useful
    }

    public interface Item {

        /* Constructors:
            Item(string name, int id)
            Item(List<Modifier> modifiers)
         */


        void AddModifier(Modifier modifier);
    }

    // I'm terrible at names, ObjectBuilder isn't very descriptive.... might split into CreatureBuilder, ItemBuilder.
    public interface ObjectBuilder {

        // these just look in the database, grab the components/data for the object,
        // then instantiate/return it.

        // just using name thats in the database for simple use. SQLite probably just
        // uses some linear search, not worrying about optimizing yet
        // can always overload for flexibility/optimization later.
        Creature getCreature(string name);
        Item getItem(string name);
    }

    public interface Data {
        void InsertCreature(Dictionary<object> creatureData);
        void InsertItem(Dictionary<object> itemData);

        Creature BuildCreature(string name);
        Item BuildItem(string name);

    }
}

```
