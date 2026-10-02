using RpgRoguelikeRun.Entities;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class TraderTests
{
    // ================================================================
    // 1. ПОЗИТИВНЫЙ: AddGold увеличивает
    // ================================================================
    [Fact]
    public void AddGold_IncreasesGold()
    {
        var trader = new Trader("Ганс", gold: 100);

        trader.AddGold(50);

        Assert.Equal(150, trader.Gold);
    }

    // ================================================================
    // 2. НЕГАТИВНЫЙ: AddGold отрицательным — исключение
    // ================================================================
    [Fact]
    public void AddGold_NegativeAmount_ThrowsException()
    {
        var trader = new Trader("Ганс", gold: 100);

        Assert.Throws<ArgumentOutOfRangeException>(() => trader.AddGold(-10));
    }

    // ================================================================
    // 3. ГРАНИЧНЫЙ: LoseGold больше, чем есть — не уходит в минус
    // ================================================================
    [Fact]
    public void LoseGold_MoreThanAvailable_StopsAtZero()
    {
        var trader = new Trader("Ганс", gold: 30);

        int lost = trader.LoseGold(100);

        Assert.Equal(30, lost);
        Assert.Equal(0, trader.Gold);
    }

    // ================================================================
    // 4. Delay устанавливает IsDelayed и DaysDelayed
    // ================================================================
    [Fact]
    public void Delay_SetsDelayedFlagAndDays()
    {
        var trader = new Trader("Ганс");

        trader.Delay(days: 3);

        Assert.True(trader.IsDelayed);
        Assert.Equal(3, trader.DaysDelayed);
    }

    // ================================================================
    // 5. TryMove при задержке уменьшает DaysDelayed
    // ================================================================
    [Fact]
    public void TryMove_WhenDelayed_DecrementsDaysDelayed()
    {
        var trader = new Trader("Ганс");
        trader.Delay(days: 2);

        bool moved = trader.TryMove();

        Assert.False(moved);
        Assert.Equal(1, trader.DaysDelayed);
    }

    // ================================================================
    // 6. ГРАНИЧНЫЙ: TryMove после окончания задержки сбрасывает флаг
    // ================================================================
    [Fact]
    public void TryMove_LastDelayDay_ResetsDelayedFlag()
    {
        var trader = new Trader("Ганс");
        trader.Delay(days: 1);

        trader.TryMove();
        bool moved = trader.TryMove();

        Assert.False(trader.IsDelayed);
        Assert.True(moved);
    }
}