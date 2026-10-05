```mermaid
graph TD
    %% Root
    Root[RpgRoguelikeRun] --> Config[Configuration]
    Root --> Entities[Entities]
    Root --> Enums[Enums]
    Root --> Items[Items]
    Root --> Services[Services]
    Root --> UI[UI]
    Root --> World[WorldLayer]
    Root --> Manager[GameManager]
    Root --> Program[Program]

    %% Configuration Layer
    subgraph ConfigBlock [Configuration Layer]
        LocPresets[LocationPresets]
        WorldConfig[WorldConfigurator]
        LocPresets --> WorldConfig
    end

    %% Enums
    subgraph EnumsBlock [Enums]
        Difficulty[Difficulty]
        ItemCategory[ItemCategory]
        ItemQuality[ItemQuality]
        ItemRarity[ItemRarity]
        GameCommand[GameCommand]
        LocationType[LocationType]
        RoadQuality[RoadQuality]
    end
    Difficulty --> WorldConfig
    Difficulty --> GameManager

    %% Services Layer
    subgraph ServicesBlock [Services]
        subgraph RandomBlock [Services.Random]
            IRandom[IRandomProvider]
            SystemRandom[SystemRandomAdapter]
            SeededRandom[SeededRandomAdapter]
            GameRandom[GameRandom]

            IRandom --> SystemRandom
            IRandom --> SeededRandom
            GameRandom -- "delegates to" --> IRandom
            GameRandom -- "SetSeed()" --> SeededRandom
            GameRandom -- "Reset()" --> SystemRandom
        end

        SetupFacade[GameSetupFacade]
        TradeService[TradeService]
        TurnFacade[TurnFacade]

        SetupFacade -- "configures" --> WorldConfig
        SetupFacade -- "creates" --> WorldCS
        SetupFacade -- "creates" --> TraderFactory
        SetupFacade -- "adds" --> Trader

        TradeService -- "modifies" --> Trader
        TradeService -- "reads" --> Market
        TradeService -- "modifies" --> Inventory
        TradeService -- "creates" --> TradeRecord

        TurnFacade -- "controls" --> Trader
        TurnFacade -- "triggers" --> WorldCS
        TurnFacade -- "uses" --> GameRandom
    end

    %% World Layer (Detailed)
    subgraph WorldBlock [WorldLayer]
        WorldCS[World]
        WorldMap[WorldMap]
        PriceModifiers[PriceModifiers]
        
        subgraph LocationsBlock [Locations]
            Location[Location]
            LocationType[LocationType]
            MarketFactory[MarketFactory]
            MarketDistribution[MarketDistribution]
            DistributionPipeline[DistributionPipeline]
            
            subgraph LocationFactoryBlock [Locations.Factory]
                LocationFactory[LocationFactory (Abstract)]
                LocationFactoryRegistry[LocationFactoryRegistry]
                VillageFactory[VillageFactory]
                TownFactory[TownFactory]
                PortFactory[PortFactory]
                MineFactory[MineFactory]
                MonasteryFactory[MonasteryFactory]

                LocationFactory --> VillageFactory
                LocationFactory --> TownFactory
                LocationFactory --> PortFactory
                LocationFactory --> MineFactory
                LocationFactory --> MonasteryFactory

                LocationFactoryRegistry -- "registers" --> VillageFactory
                LocationFactoryRegistry -- "registers" --> TownFactory
                LocationFactoryRegistry -- "registers" --> PortFactory
                LocationFactoryRegistry -- "registers" --> MineFactory
                LocationFactoryRegistry -- "registers" --> MonasteryFactory

                VillageFactory -- "creates" --> Location
                TownFactory -- "creates" --> Location
                PortFactory -- "creates" --> Location
                MineFactory -- "creates" --> Location
                MonasteryFactory -- "creates" --> Location
            end

            subgraph MarketBlock [Market]
                Market[Market]
                MarketLot[MarketLot]
            end
        end
        
        subgraph RoadsBlock [Roads]
            Road[Road (Abstract)]
            RoyalHighway[RoyalHighway]
            AbandonedRoad[AbandonedRoad]
            ForestPath[ForestPath]
            RoadQuality[RoadQuality]
            
            subgraph SafetyBlock [Safety]
                ISafetyModifier[ISafetyModifier]
                BaseSafety[BaseSafety]
                SafetyDecorator[SafetyDecorator (Abstract)]
                QualitySafetyDecorator[QualitySafetyDecorator]
                SafetyPipeline[SafetyPipeline]

                ISafetyModifier --> BaseSafety
                ISafetyModifier --> SafetyDecorator
                SafetyDecorator --> QualitySafetyDecorator
                SafetyPipeline -- "creates chain" --> BaseSafety
                SafetyPipeline -- "wraps" --> QualitySafetyDecorator
            end

            subgraph RoadFactoryBlock [Roads.Factory]
                RoadFactory[RoadFactory (Abstract)]
                RoyalHighwayFactory[RoyalHighwayFactory]
                AbandonedRoadFactory[AbandonedRoadFactory]
                ForestPathFactory[ForestPathFactory]

                RoadFactory --> RoyalHighwayFactory
                RoadFactory --> AbandonedRoadFactory
                RoadFactory --> ForestPathFactory

                RoyalHighwayFactory -- "creates" --> RoyalHighway
                AbandonedRoadFactory -- "creates" --> AbandonedRoad
                ForestPathFactory -- "creates" --> ForestPath
            end

            Road --> RoyalHighway
            Road --> AbandonedRoad
            Road --> ForestPath
            Road -- "has" --> RoadQuality
            Road -- "uses" --> SafetyPipeline
        end

        %% Composition
        WorldCS -- "has a" --> WorldMap
        WorldCS -- "has many" --> Location
        WorldCS -- "has many" --> Road
        WorldCS -- "has a" --> Trader
        WorldCS -- "has many" --> Enemy
        WorldCS -- "registers" --> RoadEventFactory
        WorldCS -- "registers" --> RoadFactory

        %% Location Composition
        Location -- "has a" --> Market
        Location -- "has type" --> LocationType
        Market -- "has many" --> MarketLot
        MarketLot -- "contains" --> Item

        %% Market Factory
        MarketFactory -- "creates" --> Market
        MarketFactory -- "uses" --> ItemCatalog
        MarketFactory -- "uses" --> MarketDistribution
        MarketFactory -- "uses" --> PriceModifiers
        MarketFactory -- "uses" --> GameRandom
        MarketDistribution -- "uses" --> GameRandom
        DistributionPipeline -- "uses" --> GameRandom

        %% Dependencies
        WorldCS -- "uses" --> GameRandom
        WorldCS -- "uses" --> PriceModifiers
        PriceModifiers -- "depends on" --> LocationType
        PriceModifiers -- "depends on" --> ItemCategory
    end

    %% Items Layer (Simplified for context)
    subgraph ItemsBlock [Items]
        Item[Item]
        ItemStack[ItemStack]
        Inventory[Inventory]
        ItemBuilder[ItemBuilder]
        ItemCatalog[ItemCatalog]

        subgraph PricingBlock [Items.Pricing]
            IPriceModifier[IPriceModifier]
            BasePrice[BasePrice]
            PriceDecorator[PriceDecorator (Abstract)]
            QualityDecorator[QualityDecorator]
            RarityDecorator[RarityDecorator]
            FreshnessDecorator[FreshnessDecorator]
            PricePipeline[PricePipeline]

            IPriceModifier --> BasePrice
            IPriceModifier --> PriceDecorator
            PriceDecorator --> QualityDecorator
            PriceDecorator --> RarityDecorator
            PriceDecorator --> FreshnessDecorator
            PricePipeline -- "creates chain" --> BasePrice
            PricePipeline -- "wraps" --> QualityDecorator
            PricePipeline -- "wraps" --> RarityDecorator
            PricePipeline -- "wraps" --> FreshnessDecorator
        end

        ItemStack -- "contains" --> Item
        Inventory -- "has many" --> ItemStack
        ItemBuilder -- "builds" --> Item
        ItemCatalog -- "creates prototypes" --> ItemBuilder
        ItemCatalog -- "clones" --> Item
        Item -- "uses" --> PricePipeline
        Item -- "has" --> ItemCategory
        Item -- "has" --> ItemQuality
        Item -- "has" --> ItemRarity
        ItemCatalog -- "uses" --> GameRandom
    end

    %% Entities Layer
    subgraph EntitiesBlock [Entities]
        Creature[Creature]
        Enemy[Enemy]
        Trader[Trader]
        TradeRecord[TradeRecord]
        TraderFactory[TraderFactory]

        Creature --> Enemy
        Creature --> Trader
        Trader -- "has a" --> Inventory
        Trader -- "has many" --> TradeRecord
        Trader -- "located in" --> Location
        Trader -- "travels on" --> Road
        Trader -- "uses" --> TradeService
        TraderFactory -- "creates" --> Trader
        TraderFactory -- "reads" --> ItemCatalog
        TraderFactory -- "uses" --> GameRandom
        TradeRecord -- "refers to" --> Item
    end

    %% Events Layer
    subgraph EventsBlock [Entities.Events]
        RoadEvent[RoadEvent (Abstract)]
        BanditAmbush[BanditAmbush]
        HelpfulMerchant[HelpfulMerchant]
        Storm[Storm]

        RoadEvent --> BanditAmbush
        RoadEvent --> HelpfulMerchant
        RoadEvent --> Storm

        BanditAmbush -- "uses" --> SystemRandom
        BanditAmbush -- "modifies" --> Trader
        BanditAmbush -- "removes from" --> Inventory

        HelpfulMerchant -- "uses" --> GameRandom
        HelpfulMerchant -- "reads" --> ItemCatalog
        HelpfulMerchant -- "modifies" --> Trader
        HelpfulMerchant -- "adds to" --> Inventory

        Storm -- "uses" --> GameRandom
        Storm -- "modifies" --> Trader
    end

    %% Event Factory Layer
    subgraph EventFactoryBlock [Entities.Events.Factory]
        RoadEventFactory[RoadEventFactory (Abstract)]
        BanditFac[BanditAmbushFactory]
        MerchantFac[HelpfulMerchantFactory]
        StormFac[StormFactory]

        RoadEventFactory --> BanditFac
        RoadEventFactory --> MerchantFac
        RoadEventFactory --> StormFac

        BanditFac -- "creates" --> BanditAmbush
        MerchantFac -- "creates" --> HelpfulMerchant
        StormFac -- "creates" --> Storm
    end

    %% UI Layer
    subgraph UIBlock [UI]
        GameRenderer[GameRenderer]
        InputHandler[InputHandler]
        MarketMenu[MarketMenu]
        TravelMenu[TravelMenu]

        GameRenderer -- "renders" --> Trader
        GameRenderer -- "renders" --> WorldCS
        GameRenderer -- "renders" --> Location
        GameRenderer -- "renders" --> Road
        
        InputHandler -- "returns" --> GameCommand
        
        MarketMenu -- "reads" --> Market
        MarketMenu -- "reads" --> MarketLot
        MarketMenu -- "calls Buy/Sell" --> Trader
        
        TravelMenu -- "reads" --> Location
        TravelMenu -- "reads" --> Road
        TravelMenu -- "calls EnterRoad" --> Trader
    end

    %% Game Manager (Singleton + Facade)
    subgraph ManagerBlock [GameManager]
        GameManager[GameManager (Singleton)]
        GameManager -- "uses" --> SetupFacade
        GameManager -- "uses" --> TurnFacade
        GameManager -- "uses" --> InputHandler
        GameManager -- "uses" --> GameRenderer
        GameManager -- "uses" --> TravelMenu
        GameManager -- "uses" --> MarketMenu
        GameManager -- "uses" --> Difficulty
        GameManager -- "uses" --> Seed
        GameManager -- "creates" --> WorldCS
        GameManager -- "creates" --> Trader
    end

    %% Program (Entry Point)
    subgraph ProgramBlock [Program]
        Program[Program.cs]
        Program -- "starts" --> GameManager
    end

    %% Connections to World Configurator
    WorldConfig -- "Registers" --> BanditFac
    WorldConfig -- "Registers" --> MerchantFac
    WorldConfig -- "Registers" --> StormFac
    WorldConfig -- "Registers" --> RoadFactory
    WorldConfig -- "uses" --> LocationFactoryRegistry

    %% External Connections
    WorldConfig -- "Creates" --> Location
    WorldConfig -- "Creates" --> Road
    WorldConfig -- "Uses" --> GameRandom
    
    TurnFacade -- "triggers" --> WorldCS
    WorldCS -- "triggers" --> RoadEvent
    TradeService -- "modifies" --> Trader
    TradeService -- "modifies" --> Inventory
    WorldCS -- "uses" --> PriceModifiers

    %% Styling
    classDef config fill:#f9f,stroke:#333,stroke-width:2px;
    classDef world fill:#bbf,stroke:#333,stroke-width:2px;
    classDef services fill:#bfb,stroke:#333,stroke-width:2px;
    classDef entities fill:#fbb,stroke:#333,stroke-width:2px;
    classDef items fill:#ffb,stroke:#333,stroke-width:2px;
    classDef pricing fill:#fdd,stroke:#333,stroke-width:2px;
    classDef events fill:#fbf,stroke:#333,stroke-width:2px;
    classDef enums fill:#ddd,stroke:#333,stroke-width:1px;
    classDef random fill:#efe,stroke:#333,stroke-width:2px;
    classDef ui fill:#ffe,stroke:#333,stroke-width:2px;
    classDef roads fill:#ddf,stroke:#333,stroke-width:2px;
    classDef safety fill:#dfd,stroke:#333,stroke-width:2px;
    classDef roadfactory fill:#edd,stroke:#333,stroke-width:2px;
    classDef market fill:#fed,stroke:#333,stroke-width:2px;
    classDef locations fill:#eef,stroke:#333,stroke-width:2px;
    classDef locfactory fill:#def,stroke:#333,stroke-width:2px;
    classDef manager fill:#fcf,stroke:#333,stroke-width:3px;
    classDef program fill:#cfc,stroke:#333,stroke-width:3px;
    
    class LocPresets,WorldConfig config;
    class WorldCS,WorldMap,PriceModifiers world;
    class SetupFacade,TradeService,TurnFacade services;
    class Creature,Enemy,Trader,TradeRecord,TraderFactory entities;
    class Item,ItemStack,Inventory,ItemBuilder,ItemCatalog items;
    class IPriceModifier,BasePrice,PriceDecorator,QualityDecorator,RarityDecorator,FreshnessDecorator,PricePipeline pricing;
    class RoadEvent,BanditAmbush,HelpfulMerchant,Storm,BanditFac,MerchantFac,StormFac,RoadEventFactory events;
    class Difficulty,ItemCategory,ItemQuality,ItemRarity,GameCommand,LocationType,RoadQuality enums;
    class IRandom,SystemRandom,SeededRandom,GameRandom random;
    class GameRenderer,InputHandler,MarketMenu,TravelMenu ui;
    class RoyalHighway,AbandonedRoad,ForestPath roads;
    class ISafetyModifier,BaseSafety,SafetyDecorator,QualitySafetyDecorator,SafetyPipeline safety;
    class RoadFactory,RoyalHighwayFactory,AbandonedRoadFactory,ForestPathFactory roadfactory;
    class Market,MarketLot market;
    class Location,MarketFactory,MarketDistribution,DistributionPipeline locations;
    class LocationFactory,LocationFactoryRegistry,VillageFactory,TownFactory,PortFactory,MineFactory,MonasteryFactory locfactory;
    class GameManager manager;
    class Program program;

```