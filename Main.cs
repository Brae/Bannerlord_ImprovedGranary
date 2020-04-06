using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using ImprovedGranary.CampaignBehavior;

namespace ImprovedGranary
{
    public class Main : MBSubModuleBase
    {
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
}
