using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldMap;

var world = new World(width: 32, height: 16);
var trader = new Trader("Ганс");

var inventory = new Inventory(capacity: 20);
inventory.Add(new Item("Соль", cost: 3, category: "Raw", rarity: "Common"), 10);
inventory.Add(new Item("Шёлк", cost: 25, category: "Luxury", rarity: "Rare"), 2);

world.AddTrader(trader);
world.AddEnemy(new Enemy());

Console.WriteLine($"Мир {world.Map.Width}x{world.Map.Height}, торговец: {trader.Name}, золото: {trader.Gold}");