using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ImprovedGranary.CampaignBehavior
{
    internal class ImprovedGranaryCampaignBehavior : CampaignBehaviorBase
    {
        public ItemRoster granaryRoster = new ItemRoster();
        public override void RegisterEvents()
        {
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));            
        }

        public override void SyncData(IDataStore dataStore)
        {
            
        }

        protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
        {
            InformationManager.DisplayMessage(new InformationMessage("Loaded Improved Granary mod"));
            campaignGameSystemStarter.AddGameMenuOption("town", "town_granary", "Open granary", new GameMenuOption.OnConditionDelegate(ImprovedGranaryCampaignBehavior.town_menu_granary_on_condition), new GameMenuOption.OnConsequenceDelegate(this.town_menu_granary_on_consequence), false, -1, false);
        }
        
        private static bool town_menu_granary_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;
            return Settlement.CurrentSettlement.OwnerClan == Clan.PlayerClan;
        }

        private void town_menu_granary_on_consequence(MenuCallbackArgs args)
        {
            LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
            InventoryManager.OpenScreenAsTrade(this.granaryRoster, Settlement.CurrentSettlement.GetComponent<Town>(), InventoryManager.InventoryCategoryType.Goods, new InventoryManager.DoneLogicExtrasDelegate(this.doneStashDelegate));
            //InventoryManager.OpenScreenAsStash(new ItemRoster());            
        }

        private void doneStashDelegate()
        {
            Settlement.CurrentSettlement.Town.FoodStocks = Settlement.CurrentSettlement.Town.FoodStocks + this.granaryRoster.TotalFood;
        }
    }
 }
