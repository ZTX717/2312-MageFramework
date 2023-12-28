

public class Item
{
    public int Id { get; set; }
    public ItemType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


public enum ItemType
{
    Weapon,
    Armor,
    Equip
}