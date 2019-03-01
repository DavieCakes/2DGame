# Creature
+ id : int
+ name : string
+ Attributes : Dictionary<CreatureAttributes, CreatureStat>
+ inventory : List<Item>
+ EquipSlots : Dictionary<string, Item>
+ Abilities : List<Ability>
+ Actions : List<ActionStrategy>
+ Leveler : LevelingStrategy

+ Creature() : Creature
+ Equip(item : Item) : void
+ Unequip(item: Item) : void
+ CalculateStats() : void

# CreatureStat
+ BaseValue : float
- readonly statModifiers : List<StatModifier> 
+ readonly statModifiers : ReadOnlyCollection<StatModifier> 
- isDirty : bool = true
- _value : float

+ CreatureStat(baseValue : float) : CreatureStat
+ Value : float
+ AddModifier(StatModifier mod) : void
- CompareModifierOrder(StatModifier a, StatModifier b) : int
+ RemoveModifier(StatModifier mod) : bool
- CalculateFinalValue() : float
+ RemoveAllModifiersFromSource(object source) : bool

# StatModifier
+ readonly Value : float
+ readonly Type : StatModType
+ readonly attType : CreatureAttributeType
+ readonly Order : int
+ readonly Source : object

+ StatModifier(float value, StatModType type) 
+ StatModifier(float value, StatModType type, int order)
+ StatModifier(float value, StatModType type, int order, object source)
+ StatModifier(float value, StatModType type, CreatureAttributeType modType, object source)



# abstract TargetingAction : ActionStrategy
+ virtual void Perform(Creature from, Creature to)

# abstract AreaAction : ActionStrategy
+ virtual void Perform(Creature from)

# FireBall : AreaAction
+ override void Perform(Creature from)