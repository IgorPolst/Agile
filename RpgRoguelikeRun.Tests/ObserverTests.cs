using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class ObserverTests
{
    private static Item MakeFish() =>
        new Item("Рыба", ItemCategory.Raw, ItemRarity.Common,
                 baseCost: 10, quality: ItemQuality.Common, shelfLifeDays: 3);

    // ================================================================
    // 1. Событие срабатывает при переходе в испорченное
    // ================================================================
    [Fact]
    public void OnSpoiled_TriggersWhenItemBecomesSpoiled()
    {
        var fish = MakeFish();
        bool triggered = false;
        fish.OnSpoiled += _ => triggered = true;

        fish.DaysInStorage = 3;

        Assert.True(triggered);
    }

    // ================================================================
    // 2. Событие НЕ срабатывает, если товар ещё свежий
    // ================================================================
    [Fact]
    public void OnSpoiled_DoesNotTriggerWhenItemFresh()
    {

        var fish = MakeFish();
        bool triggered = false;
        fish.OnSpoiled += _ => triggered = true;

        fish.DaysInStorage = 1;   // < 3 → свежий

        Assert.False(triggered);
    }

    // ================================================================
    // 3. Событие НЕ срабатывает повторно
    // ================================================================
    [Fact]
    public void OnSpoiled_TriggersOnlyOnce()
    {
        var fish = MakeFish();
        int count = 0;
        fish.OnSpoiled += _ => count++;

        fish.DaysInStorage = 3;
        fish.DaysInStorage = 5;
        fish.DaysInStorage = 10;

        // Assert
        Assert.Equal(1, count);
    }

    // ================================================================
    // 4. Непортящийся товар НЕ дёргает событие
    // ================================================================
    [Fact]
    public void OnSpoiled_DoesNotTriggerForNonPerishable()
    {
        var salt = new Item("Соль", ItemCategory.Raw, ItemRarity.Common,
                            baseCost: 5, shelfLifeDays: null);
        bool triggered = false;
        salt.OnSpoiled += _ => triggered = true;

        salt.DaysInStorage = 1000;

        Assert.False(triggered);
    }

    // ================================================================
    // 5. Trader получает уведомление через Inventory
    // ================================================================
    [Fact]
    public void Trader_ReceivesNotification_ThroughInventory()
    {
        var trader = new Entities.Trader("Ганс", gold: 100);
        var fish = MakeFish();
        trader.Inventory.Add(fish);

        fish.DaysInStorage = 3;

        Assert.Single(trader.SpoiledLog);
        Assert.NotNull(trader.LastSpoiledMessage);
    }

    // ================================================================
    // 6. Отписка работает — после удаления Trader не получает уведомления
    // ================================================================
    [Fact]
    public void Unsubscribe_StopsReceivingNotifications()
    {
        var trader = new Entities.Trader("Ганс", gold: 100);
        var fish = MakeFish();
        trader.Inventory.Add(fish);
        trader.Inventory.Remove(fish);
        fish.DaysInStorage = 3;

        Assert.Empty(trader.SpoiledLog);
    }
}