

using System;
using System.Numerics;

public class ItemEquip : Item
{
    public int EquipType { get; set; }
    public int Count { get; set; }
    public int MaxCount { get; set; }
    public int Lv { get; set; }
    public int LvMax { get; set; }
    public Vector3 Pos { get; set; }
    public ItemEquip()
    {
        Type = ItemType.Equip;
    }
}