namespace Eco.Mods.TechTree
{
    // [DoNotLocalize]
    using System;
    using System.Collections.Generic;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Components.VehicleModules;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    
    [Serialized]
    [Weight(25000)]  
    public class GoldSteamTruckItem : WorldObjectItem<GoldSteamTruckObject>
    {
        public override LocString DisplayName        { get { return Localizer.DoStr("Gold Steam Truck"); } }
        public override LocString DisplayDescription { get { return Localizer.DoStr("A gold truck that runs on steam. Show your richness to the world !"); } }
    }

    [RequiresSkill(typeof(MechanicsSkill), 0)] 
    public class GoldSteamTruckRecipe : Recipe
    {
        public GoldSteamTruckRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<GoldSteamTruckItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<PortableSteamEngineItem>(1),
                new CraftingElement<IronWheelItem>(4),
                new CraftingElement<IronAxleItem>(1), 
                new CraftingElement<IronPlateItem>(typeof(MechanicsSkill), 30, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent)),
                new CraftingElement<IronPipeItem>(typeof(MechanicsSkill), 20, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent)),
                new CraftingElement<ScrewsItem>(typeof(MechanicsSkill), 40, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent)),
                new CraftingElement<LumberItem>(typeof(MechanicsSkill), 30, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent)),
                new CraftingElement<LeatherHideItem>(typeof(MechanicsSkill), 20, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent)),
				new CraftingElement<GoldIngotItem>(50)
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(GoldSteamTruckRecipe), Item.Get<GoldSteamTruckItem>().UILink(), 25, typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent));    

            this.Initialize(Localizer.DoStr("Gold Steam Truck"), typeof(GoldSteamTruckRecipe));
            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }

    [Serialized]
    [RequireComponent(typeof(StandaloneAuthComponent))]
    [RequireComponent(typeof(FuelSupplyComponent))]              
    [RequireComponent(typeof(FuelConsumptionComponent))]         
    [RequireComponent(typeof(PublicStorageComponent))]      
    [RequireComponent(typeof(MovableLinkComponent))]        
    [RequireComponent(typeof(AirPollutionComponent))]       
    [RequireComponent(typeof(VehicleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]   
    [RequireComponent(typeof(TailingsReportComponent))]     
    public partial class GoldSteamTruckObject : PhysicsWorldObject, IRepresentsItem
    {
        static GoldSteamTruckObject()
        {
            WorldObject.AddOccupancy<GoldSteamTruckObject>(new List<BlockOccupancy>(0));
        }

        public override LocString DisplayName { get { return Localizer.DoStr("Gold Steam Truck"); } }
        public Type RepresentedItemType { get { return typeof(GoldSteamTruckItem); } }

        private static Type[] fuelTypeList = new Type[]
        {
            typeof(LogItem),
typeof(LumberItem),
typeof(CharcoalItem),
typeof(ArrowItem),
typeof(BoardItem),
typeof(CoalItem)
        };

        private GoldSteamTruckObject() { }

        protected override void Initialize()
        {
            base.Initialize();
            
            this.GetComponent<PublicStorageComponent>().Initialize(24, 5000000);           
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTypeList);           
            this.GetComponent<FuelConsumptionComponent>().Initialize(50);    
            this.GetComponent<AirPollutionComponent>().Initialize(0.06f);            
            this.GetComponent<VehicleComponent>().Initialize(23, 2, 2);
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2,2,3));  
        }
    }
}