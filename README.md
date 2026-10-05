# 🏰 Medieval Trader — Roguelike про средневекового торговца

Учебный проект на .NET 10. Игрок — торговец, который путешествует между
городами, покупает и продаёт товары, переживает события в дороге и
старается не обанкротиться.

## 🎮 Управление

| Клавиша | Действие |
|---------|----------|
| `WASD` / стрелки | Шаг / идти по дороге |
| `B` | Открыть рынок |
| `T` | Меню дорог (выход из локации) |
| `Esc` | Выход из меню / из игры |

## 🏗️ Архитектура

### Папки

- `Entities/` — сущности (Trader, Creature, Enemy) и события (`Events/`)
- `Items/` — товары, инвентарь, каталог, цены (`Pricing/`)
- `WorldLayer/` — мир, локации (`Locations/`), рынки (`Markets/`), дороги (`Roads/`)
- `Configuration/` — конфигурация мира (`WorldConfigurator`, `LocationPresets`)
- `Services/` — сервисы (TradeService, GameSetupFacade, TurnFacade, GameRandom)
- `UI/` — ввод/вывод (MarketMenu, TravelMenu, GameRenderer, InputHandler)
- `RpgRoguelikeRun.Tests/` — unit-тесты

### Применённые паттерны

| Паттерн | Где | Зачем |
|---------|-----|-------|
| **Singleton** | `GameManager` | Единая точка входа в игру |
| **Factory Method** | `MarketFactory`, `LocationFactory`, `RoadFactory`, `TraderFactory` | Создание объектов с разной логикой по типу |
| **Registry** | `LocationFactoryRegistry` | Реестр фабрик локаций |
| **Builder** | `ItemBuilder` | Пошаговая сборка `Item` с 6+ параметрами |
| **Prototype** | `Item.Clone()`, `ItemCatalog` | Клонирование товаров |
| **Decorator** | `PricePipeline`, `SafetyPipeline`, `SpeedPipeline` | Модификаторы цены/безопасности/скорости |
| **Adapter** | `SystemRandomAdapter`, `IRandomProvider` | Интеграция `System.Random` |
| **Facade** | `GameSetupFacade`, `TurnFacade` | Упрощение сложных операций |
| **Strategy** | `MarketDistribution` | Распределение количества и веса |

## 🧪 Тесты

```bash
dotnet test
Покрыты: Inventory, Item, Market, MarketLot, Trader, Road,
ItemBuilder, PriceModifiers, PricePipeline, SafetyPipeline,
SpeedPipeline, RandomAdapter, MarketFactory.
```
## 🚀 Запуск

dotnet run --project RpgRoguelikeRun

## 📝 Лицензия

Учебный проект. Свободное использование в образовательных целях.