``` mermaid
classDiagram
    direction LR

    %% ===================== CORE =====================
    namespace Core {
        class GameManager {
            <<Singleton>>
            -_instance: GameManager
            +Instance: GameManager
            +MapWidth: int
            +MapHeight: int
            +Difficulty: Difficulty
            -_world: World
            -_trader: Trader
            -_gameStopped: bool
            -_random: Random
            +Run()
            -Init()
            -ConfigureEventFactories()
            -ConfigureRoadFactories()
            -CreateStartingMarket() Market
            -GiveStartingGoods(trader)
            -HandleInput()
            -DoTurn()
            -Update()
            -Render()
            -OpenMarketMenu()
            -BuyMenu()
            -SellMenu()
        }

        class Difficulty {
            <<enumeration>>
            Easy
            Normal
            Hard
        }
    }

    %% ===================== ENTITIES =====================
    namespace Entities {
        class Creature {
            <<Abstract>>
            +Name: string
            +Gold: int
            +Move()
        }

        class Trader {
            +Inventory: Inventory
            +IsDelayed: bool
            +DaysDelayed: int
            +CurrentRoad: Road
            +CurrentMarket: Market
            +LastEvent: RoadEvent
            +EventLog: List~RoadEvent~
            +TryMove() bool
            +EnterRoad(road)
            +LeaveRoad()
            +EnterMarket(market)
            +LeaveMarket()
            +AddGold(n)
            +LoseGold(n) int
            +Delay(days)
            +RegisterEvent(ev)
            +Buy(item, qty, price) bool
            +Sell(item, qty, price) bool
        }

        class Enemy {
            +Move()
            +Trade()
        }
    }

    %% ===================== EVENTS =====================
    namespace Events {
        class RoadEvent {
            <<Abstract>>
            +Title: string
            +TriggersStormDamage: bool
            +IsHostile: bool
            +IsFriendly: bool
            +Trigger(trader)
        }

        class BanditAmbush {
            +Damage: int
            +DamageVariance: double
            +IsHostile: bool
            +Trigger(trader)
        }

        class HelpfulMerchant {
            +Bonus: int
            +BonusVariance: double
            +IsFriendly: bool
            +Trigger(trader)
        }

        class Storm {
            +DelayDays: int
            +TriggersStormDamage: bool
            +Trigger(trader)
        }

        class RoadEventFactory {
            <<Abstract>>
            +CreateEvent() RoadEvent
            +ProducesHostile: bool
            +ProducesFriendly: bool
            +ProducesNeutral: bool
        }

        class BanditAmbushFactory {
            +CreateEvent() RoadEvent
            +ProducesHostile: bool
        }

        class HelpfulMerchantFactory {
            +CreateEvent() RoadEvent
            +ProducesFriendly: bool
        }

        class StormFactory {
            +CreateEvent() RoadEvent
            +ProducesNeutral: bool
        }
    }

    %% ===================== WORLD =====================
    namespace WorldLayer {
        class World {
            +Map: WorldMap
            +Trader: Trader
            +Enemies: List~Enemy~
            +EventFactories: List~RoadEventFactory~
            +RoadFactories: List~RoadFactory~
            +Roads: List~Road~
            +AddTrader(trader)
            +AddEnemy(enemy)
            +RegisterEventFactory(f)
            +RegisterRoadFactory(f)
            +CreateRandomRoad() Road
            +GenerateRoads(n)
            +TriggerRandomRoadEvent(trader, road) RoadEvent
            -PickEventForRoad(road) RoadEvent
            -ApplyEventSideEffects(ev, road)
            +TryPayToll(road, trader) bool
        }

        class WorldMap {
            +Width: int
            +Height: int
        }

        class Market {
            +Lots: List~MarketLot~
            +BuybackRate: double
            +AddLot(item, qty, price)
            +TryTakeFromMarket(item, qty, out price) bool
            +GetSellPrice(item) int
        }

        class MarketLot {
            +Item: Item
            +Quantity: int
            +PricePerUnit: int
        }
    }

    %% ===================== ROADS =====================
    namespace Roads {
        class Road {
            <<Abstract>>
            +Name: string
            +Length: int
            +Quality: RoadQuality
            +TollCost: int
            +Safety: double
            +EffectiveSafety: double
            +BanditChance: double
            +FriendlyChance: double
            +SpeedMultiplier: double
            +TravelTime: int
            +ApplyStorm()
            +Repair()
        }

        class RoyalHighway
        class AbandonedRoad
        class ForestPath

        class RoadFactory {
            <<Abstract>>
            #BaseLength: int
            #LengthVariance: double
            +CreateRoad(random) Road
            #RollLength(random) int
        }

        class RoyalHighwayFactory
        class AbandonedRoadFactory
        class ForestPathFactory

        class RoadQuality {
            <<enumeration>>
            Paved
            Dirt
            Muddy
            Overgrown
        }
    }

    %% ===================== ITEMS =====================
    namespace Items {
        class Item {
            +Name: string
            +Category: ItemCategory
            +Rarity: ItemRarity
            +BaseCost: int
            +Quality: ItemQuality
            +ShelfLifeDays: int?
            +DaysInStorage: int
            +IsPerishable: bool
            +IsSpoiled: bool
            +CurrentCost: int
            +Clone() object
        }

        class ItemBuilder {
            +WithName(n)
            +WithCategory(c)
            +WithRarity(r)
            +WithBaseCost(c)
            +WithQuality(q)
            +WithShelfLife(d)
            +WithoutShelfLife()
            +Build() Item
        }

        class ItemCatalog {
            <<static>>
            +Create(key) Item
            +Keys: IEnumerable~string~
        }

        class ItemStack {
            +Item: Item
            +Quantity: int
            +TotalCost: int
        }

        class Inventory {
            +Capacity: int
            +Stacks: List~ItemStack~
            +Count: int
            +HasSpaceFor(n) bool
            +Add(item, qty) bool
            +Remove(item, qty) bool
            +CountOf(item) int
        }

        class ItemCategory {
            <<enumeration>>
            Raw
            Crafted
            Luxury
            Contraband
            Livestock
        }

        class ItemQuality {
            <<enumeration>>
            Poor
            Common
            Good
            Excellent
        }

        class ItemRarity {
            <<enumeration>>
            Common
            Uncommon
            Rare
            Legendary
        }
    }

    %% ===================== RELATIONS =====================
    GameManager --> World
    GameManager --> Trader
    GameManager --> Market

    Trader --|> Creature
    Enemy --|> Creature
    Trader --> Inventory
    Trader --> Market : CurrentMarket
    Trader --> Road : CurrentRoad
    Trader --> RoadEvent : LastEvent

    World *-- WorldMap
    World *-- Market
    World o-- Road
    World o-- RoadEventFactory
    World o-- RoadFactory

    Market *-- MarketLot
    MarketLot --> Item

    Inventory *-- ItemStack
    ItemStack --> Item
    Item ..> ItemCategory
    Item ..> ItemRarity
    Item ..> ItemQuality

    ItemCatalog ..> ItemBuilder : uses
    ItemCatalog ..> Item : creates clones

    Road <|-- RoyalHighway
    Road <|-- AbandonedRoad
    Road <|-- ForestPath
    Road ..> RoadQuality

    RoadFactory <|-- RoyalHighwayFactory
    RoadFactory <|-- AbandonedRoadFactory
    RoadFactory <|-- ForestPathFactory

    RoadEvent <|-- BanditAmbush
    RoadEvent <|-- HelpfulMerchant
    RoadEvent <|-- Storm

    RoadEventFactory <|-- BanditAmbushFactory
    RoadEventFactory <|-- HelpfulMerchantFactory
    RoadEventFactory <|-- StormFactory

```