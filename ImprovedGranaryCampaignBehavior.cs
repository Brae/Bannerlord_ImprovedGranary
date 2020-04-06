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
            //CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));
        }

        public override void SyncData(IDataStore dataStore)
        {
            
        }

        protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
        {
            campaignGameSystemStarter.AddGameMenuOption("town", "town_granary", "Open granary", new GameMenuOption.OnConditionDelegate(ImprovedGranaryCampaignBehavior.town_menu_granary_on_condition), new GameMenuOption.OnConsequenceDelegate(this.town_menu_granary_on_consequence), false, 8, false);
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
            float newTotal = Settlement.CurrentSettlement.Town.FoodStocks + this.granaryRoster.TotalFood;
            float difference = 0;
            int upperLimit = Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();
            if (this.granaryRoster.TotalFood > 0)
            {
                if (newTotal > upperLimit)
                {
                    difference = upperLimit - Settlement.CurrentSettlement.Town.FoodStocks;
                    Settlement.CurrentSettlement.Town.FoodStocks = upperLimit;
                }
                else
                {
                    difference = this.granaryRoster.TotalFood;
                    Settlement.CurrentSettlement.Town.FoodStocks = newTotal;
                }
                InformationManager.DisplayMessage(new InformationMessage("Donated " + difference + " food to town granary"));
            }
            this.granaryRoster.RemoveAllItems();
        }
    }
 }
