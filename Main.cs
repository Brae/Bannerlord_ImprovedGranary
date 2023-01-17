using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using ImprovedGranary.CampaignBehavior;
using HarmonyLib;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Localization;
using System;

namespace ImprovedGranary
{
    public class Main : MBSubModuleBase
    {
        // Handle registration of the Harmony patch to change total capacity of the granary
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony harmony = new Harmony("Harmony_ImprovedGranary");
            harmony.PatchAll();
        }

        // Register the new settlement behaviour to enable adding food to the granary
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            Campaign campaign = game.GameType as Campaign;
            bool flag = campaign == null;
            if (!flag)
            {
                CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
                gameInitializer.AddBehavior(new ImprovedGranaryCampaignBehavior());
            }
        }
    }

    [HarmonyPatch(typeof(DefaultBuildingTypes), "InitializeAll")]
    class ImprovedGranaryHarmonyPatch
    {
        public static void PostFix(ref BuildingType _____buildingSettlementGranary)
        {
            _____buildingSettlementGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=aK23T43P}Keeps stockpiles of food so that the settlement has more food supply. Each level increases the local food supply.", null), new int[]
            {
                1000,
                1500,
                2000
            }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
            {
                new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Foodstock, 200f, 500f, 1000f)
            }, 0);
        }
    }
}
