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
    public class BlueSteamTruckItem : WorldObjectItem<BlueSteamTruckObject>
    {
        public override LocString DisplayName        { get { return Localizer.DoStr("Blue Steam Truck"); } }
        public override LocString DisplayDescription { get { return Localizer.DoStr("A blue truck that runs on steam."); } }
    }

    [RequiresSkill(typeof(MechanicsSkill), 0)] 
    public class BlueSteamTruckRecipe : Recipe
    {
        public BlueSteamTruckRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<BlueSteamTruckItem>(),
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
                new CraftingElement<LeatherHideItem>(typeof(MechanicsSkill), 20, MechanicsSkill.MultiplicativeStrategy, typeof(MechanicsLavishResourcesTalent))
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(BlueSteamTruckRecipe), Item.Get<BlueSteamTruckItem>().UILink(), 25, typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent));    

            this.Initialize(Localizer.DoStr("Blue Steam Truck"), typeof(BlueSteamTruckRecipe));
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
    public partial class BlueSteamTruckObject : PhysicsWorldObject, IRepresentsItem
    {
        static BlueSteamTruckObject()
        {
            WorldObject.AddOccupancy<BlueSteamTruckObject>(new List<BlockOccupancy>(0));
        }

        public override LocString DisplayName { get { return Localizer.DoStr("Blue Steam Truck"); } }
        public Type RepresentedItemType { get { return typeof(BlueSteamTruckItem); } }

        private static Type[] fuelTypeList = new Type[]
        {
            typeof(LogItem),
typeof(LumberItem),
typeof(CharcoalItem),
typeof(ArrowItem),
typeof(BoardItem),
typeof(CoalItem)
        };

        private BlueSteamTruckObject() { }

        protected override void Initialize()
        {
            base.Initialize();
            
            this.GetComponent<PublicStorageComponent>().Initialize(24, 5000000);           
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTypeList);           
            this.GetComponent<FuelConsumptionComponent>().Initialize(25);    
            this.GetComponent<AirPollutionComponent>().Initialize(0.03f);            
            this.GetComponent<VehicleComponent>().Initialize(18, 2, 2);
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2,2,3));  
        }
    }
}