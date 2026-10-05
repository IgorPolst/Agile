# 🏰 Medieval Trader

> Консольный roguelike про средневекового торговца: путешествуй между городами, покупай и продавай товары, переживай события в дороге и не обанкроться.

![Build](https://img.shields.io/badge/build-passing-brightgreen)
![.NET](https://img.shields.io/badge/.NET-10.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📖 О игре

**Жанр:** Roguelike / Экономическая стратегия / Управление ресурсами  
**Платформа:** Windows, Linux, macOS (консоль)  
**Стек:** .NET 10, C#, xUnit

Ты — **Ганс**, странствующий торговец. Твоя цель — **выжить** и **разбогатеть**. Каждый день — это **выбор**: куда идти, что купить, что продать, рискнуть или отсидеться.

**Особенности:**
- 🗺️ **Процедурный мир** — 25 городов в 3 регионах, случайные дороги
- 💰 **Динамическая экономика** — цены зависят от локации, региона, качества, объёма
- 🎲 **События в дороге** — бандиты, штормы, солнечные дни, странствующие рыцари
- 📦 **Инвентарь** — товары портятся, качество снижается, цена падает
- ⚔️ **Риск** — бандиты отнимают золото и товар

---

## 🎮 Управление

| Клавиша | Действие |
|---------|----------|
| `W A S D` / стрелки | Шаг / идти по дороге |
| `B` | Открыть рынок (в городе) |
| `T` | Меню дорог (выйти из города) |
| `Esc` | Выход в главное меню / отмена |

---

## 🚀 Как запустить

### Вариант 1: Готовый билд (рекомендуется)

1. Скачай архив со [страницы Releases](https://github.com/ваш-логин/ваш-репозиторий/releases).
   - `MedievalTrader-win-x64.zip` — для Windows
   - `MedievalTrader-linux-x64.zip` — для Linux
2. Распакуй архив.
3. Запусти `MedievalTrader.exe` (Windows) или `./MedievalTrader` (Linux).
4. **Ничего дополнительно устанавливать не нужно** — .NET Runtime включён.

### Вариант 2: Сборка из исходников

**Требования:** .NET 10 SDK.

```bash
git clone https://github.com/ваш-логин/ваш-репозиторий.git
cd ваш-репозиторий
dotnet run --project RpgRoguelikeRun
```

## 🏗️ Архитектурные решения

Проект **намеренно** использует **разные паттерны** для **разных** задач. Ниже — **что** и **почему**.

### 🔒 Singleton — `GameManager`

**Файл:** `GameManager.cs`

**Зачем:** единая **точка входа** в игру. Нельзя создать **второй** `GameManager` — иначе будет **конфликт** настроек.

```csharp
private static GameManager? _instance;
public static GameManager Instance => _instance ??= new GameManager();
private GameManager() { }
```

## 🧪 Тесты

```bash
dotnet test
```
**Покрыто:**
1. Inventory — добавление, удаление, старение, capacity
2. Item — цена, порча, Prototype, описания
3. Market, MarketLot — спред, динамика цен
4. Trader — золото, задержка, события
5. Road — безопасность, скорость, TravelTime
6. ItemBuilder, PricePipeline, SafetyPipeline, SpeedPipeline
7. RandomAdapter, MarketFactory, GameState

## 🏭 Factory Method — создание объектов
**Файлы:** `WorldLayer/MarketFactory.cs`, `WorldLayer/Locations/Factory/*.cs`, `WorldLayer/Roads/Factory/*.cs`, `Entities/Events/Factory/*.cs`

**Зачем:** создание **сложных** объектов с **разной** логикой. **Добавить** новый тип — **новый** класс + строка в реестре. `GameManager` **не меняется.**

``` csharp
public abstract class RoadFactory
{
    public abstract Road CreateRoad(IRandomProvider random);
}
```

## 🎨 Decorator — модификаторы цены/безопасности/скорости
**Файлы:** `Items/Pricing/PricePipeline.cs`, `WorldLayer/Roads/Safety/SafetyPipeline.cs`, `WorldLayer/Roads/Speed/SpeedPipeline.cs`

**Зачем: цепочка** модификаторов. **Каждый — отдельный** класс. **Добавить** новый фактор (например, «скидка ярмарки») — **один** класс + **строка** в pipeline.

```bash
csharp
IPriceModifier chain = new BasePrice();
chain = new QualityDecorator(chain);
chain = new RarityDecorator(chain);
chain = new FreshnessDecorator(chain);
```

## 🧬 Prototype — Item.Clone()
**Файл:** `Items/Item.cs`

**Зачем:** клонирование товаров. `ItemCatalog` хранит **прототипы**, `Create()` возвращает **клон**. Изменение клона **не** трогает прототип.

```csharp
public object Clone()
{
    return new Item(Name, Category, Rarity, BaseCost, Quality, ShelfLifeDays)
    {
        DaysInStorage = DaysInStorage
    };
}
```

### 🔧 Builder — ItemBuilder

**Файл:** `Items/ItemBuilder.cs`

**Зачем:**`Item` имеет **6+** параметров. Fluent Interface **читается** как рецепт:

```csharp
var grain = new ItemBuilder()
    .WithName("Зерно")
    .WithCategory(ItemCategory.Raw)
    .WithBaseCost(3)
    .WithShelfLife(30)
    .Build();
```

## 🎭 Strategy — регионы

**Файлы:** `WorldLayer/Regions/*.cs`

**Зачем: каждый** регион — **своя** стратегия. Прибрежный — **рыба** чаще, горный — **соль** дешевле. Location держит `IRegionStrategy`, **спрашивает** её.

```csharp
public interface IRegionStrategy
{
    List<ItemCategory> GetCategories();
    double GetPriceMultiplier(Item item);
    int GetQualityShift(Item item);
    double GetSpawnWeight(Item item);
}
```
## 🔌 Adapter — IRandomProvider

**Файлы:** `Services/Random/*.cs`

**Зачем:** централизация **случайности** через **свой** интерфейс. `SystemRandomAdapter` подгоняет `System.Random`. `SeededRandomAdapter` — **воспроизводимость**.

```csharp
public interface IRandomProvider
{
    int Next(int min, int max);
    double NextDouble();
}
```

## 🏛️ Facade — GameSetupFacade, TurnFacade

**Файлы:** `Services/GameSetupFacade.cs`, `Services/TurnFacade.cs`

**Зачем:** **скрыть** сложный порядок вызовов. `GameSetupFacade.StartNewGame` — **один** метод вместо **пяти**.

```csharp
public static (World world, Trader trader) StartNewGame(
    int mapWidth, int mapHeight, Difficulty difficulty)
{
    var world = new World(mapWidth, mapHeight);
    WorldConfigurator.Configure(world, difficulty);
    var trader = TraderFactory.CreateStartingTrader();
    world.AddTrader(trader);
    trader.EnterLocation(world.Locations.First());
    return (world, trader);
}
```

## 🎭 State — состояния игры

**Файлы:** `Game/States/*.cs`

**Зачем: меню → игра → пауза → GameOver.** Вместо **флагов — классы.** `GameManager.Run` **тонкий**, логика — в состояниях.

```csharp
public interface IGameState
{
    string Name { get; }
    void Enter(GameContext context);
    bool HandleInput(GameContext context);
    void Render(GameContext context);
}
```

###**Конкретные состояния:**

- `MenuState` — стартовое меню

- `PlayingState` — основной геймплей

- `PauseState` — пауза по Escape

- `GameOverState` — конец игры со статистикой

## 📢 Observer — порча товаров

**Файлы:** `Items/Item.cs`, `Entities/Trader.cs`, `Items/Inventory.cs`

**Зачем:** `Item.OnSpoiled` — событие. `Trader` **подписывается** через `Inventory.OnItemAdded`. `Item` **не знает** про `Trader`. **Слабая** связность.

```csharp
public event Action<Item>? OnSpoiled;
// ...
OnSpoiled?.Invoke(this);
```

### Подписчики:

`Trader.HandleItemSpoiled` — пишет в журнал, показывает сообщение

## 📋 Registry — реестры фабрик
**Файлы:** `WorldLayer/Locations/Factory/LocationFactoryRegistry.cs`, `WorldLayer/Regions/RegionRegistry.cs`

**Зачем: единая** точка **доступа** к фабрикам по **ключу** (enum или строка). **Добавить** новый регион — **один** класс + **строка** в словаре.

```csharp
private static readonly Dictionary<string, IRegionStrategy> _regions = new()
{
    { "coastal",  new CoastalRegionStrategy() },
    { "forest",   new ForestRegionStrategy() },
    { "mountain", new MountainRegionStrategy() },
};
```
## 📁 Структура проекта

RpgRoguelikeRun/
├── Configuration/       — конфигурация мира (пресеты, difficulty)
├── Entities/            — торговец, враг, события
│   ├── Events/          — события в дороге
│   └── Factory/         — фабрики событий
├── Game/                — состояния игры (State)
│   └── States/
├── Items/               — товары, инвентарь, каталог
│   └── Pricing/         — Decorator для цены
├── Services/            — сервисы (TradeService, Facades, Random)
│   └── Random/
├── UI/                  — ввод/вывод (рендер, меню)
├── WorldLayer/          — мир, локации, рынки, дороги
│   ├── Locations/       — локации и фабрики
│   ├── Markets/         — рынки
│   ├── Regions/         — регионы (Strategy)
│   └── Roads/           — дороги
├── GameManager.cs
└── Program.cs

RpgRoguelikeRun.Tests/   — unit-тесты (xUnit)
RpgRoguelikeRun.Tests/   — unit-тесты (xUnit)

## 📝 Лицензия
MIT. Свободное использование в образовательных целях.

## 📌 Что здесь есть

| Раздел | Что внутри |
|--------|-----------|
| **Архитектурные решения** | **10 паттернов** — Singleton, Factory, Decorator, Prototype, Builder, Strategy, Adapter, Facade, State, Observer, Registry |
| **Структура проекта** | **Дерево** папок с **комментариями** |
| **История занятий** | **Таблица** — что **когда** делали |
| **Лицензия** | MIT |
| **Автор** | Шаблон |

---



