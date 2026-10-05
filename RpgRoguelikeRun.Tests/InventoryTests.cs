using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class InventoryTests
{
    // ---------- ARRANGE helper ----------
    private static Item MakeItem(string name = "Зерно", int cost = 3)
        => new Item(name, ItemCategory.Raw, ItemRarity.Common, cost);

    // ================================================================
    // 1. ПОЗИТИВНЫЙ: добавление товара увеличивает Count
    // ================================================================
    [Fact]
    public void Add_NewItem_IncreasesCount()
    {
        var inventory = new Inventory(capacity: 10);
        var item = MakeItem();

        bool added = inventory.Add(item, quantity: 3);

        Assert.True(added);
        Assert.Equal(3, inventory.Count);
    }

    // ================================================================
    // 2. ГРАНИЧНЫЙ: нельзя превысить Capacity
    // ================================================================
    [Fact]
    public void Add_MoreThanCapacity_ReturnsFalse()
    {
        var inventory = new Inventory(capacity: 5);
        var item = MakeItem();

        bool added = inventory.Add(item, quantity: 10);

        Assert.False(added);
        Assert.Equal(0, inventory.Count);
    }

    // ================================================================
    // 3. ПОЗИТИВНЫЙ: удаление уменьшает Count
    // ================================================================
    [Fact]
    public void Remove_ExistingItem_DecreasesCount()
    {
        var inventory = new Inventory(capacity: 10);
        var item = MakeItem();
        inventory.Add(item, quantity: 5);

        bool removed = inventory.Remove(item, quantity: 2);

        Assert.True(removed);
        Assert.Equal(3, inventory.Count);
    }

    // ================================================================
    // 4. НЕГАТИВНЫЙ: нельзя удалить больше, чем есть
    // ================================================================
    [Fact]
    public void Remove_MoreThanAvailable_ReturnsFalse()
    {
        var inventory = new Inventory(capacity: 10);
        var item = MakeItem();
        inventory.Add(item, quantity: 2);

        bool removed = inventory.Remove(item, quantity: 5);

        Assert.False(removed);
        Assert.Equal(2, inventory.Count);   // ничего не изменилось
    }

    // ================================================================
    // 5. ГРАНИЧНЫЙ: AgeItems увеличивает DaysInStorage
    // ================================================================
    [Fact]
    public void AgeItems_IncreasesDaysInStorage()
    {
        var inventory = new Inventory(capacity: 10);
        var item = MakeItem();
        inventory.Add(item, quantity: 1);

        inventory.AgeItems(days: 3);

        Assert.Equal(3, item.DaysInStorage);
    }

    // ================================================================
    // 6. ГРАНИЧНЫЙ: испорченный товар дешевеет
    // ================================================================
    [Fact]
    public void AgeItems_OverShelfLife_ItemBecomesSpoiled()
    {
        var inventory = new Inventory(capacity: 10);
        var item = new Item("Зерно", ItemCategory.Raw, ItemRarity.Common, baseCost: 10,
                            quality: ItemQuality.Common, shelfLifeDays: 5);
        inventory.Add(item, quantity: 1);

        inventory.AgeItems(days: 6);

        Assert.True(item.IsSpoiled);
        Assert.Equal(2, item.CurrentCost);
    }

    // ================================================================
    // 7. ГРАНИЧНЫЙ: CountOf несуществующего товара = 0
    // ================================================================
    [Fact]
    public void CountOf_NonExistingItem_ReturnsZero()
    {
        var inventory = new Inventory(capacity: 10);
        var item = MakeItem();
        int count = inventory.CountOf(item);
        Assert.Equal(0, count);
    }

    // ================================================================
    // 8. НЕГАТИВНЫЙ: HasSpaceFor(0) — валидно?
    // ================================================================
    [Fact]
    public void HasSpaceFor_ZeroQuantity_ReturnsTrue()
    {
        var inventory = new Inventory(capacity: 1);
        inventory.Add(MakeItem(), quantity: 1);
        bool hasSpace = inventory.HasSpaceFor(0);
        Assert.True(hasSpace);
    }
}