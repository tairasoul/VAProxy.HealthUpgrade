using BepInEx;
using Invector;
using UnityEngine;
using UpgradeFramework;

namespace HealthUpgrade
{
    internal class PluginInfo
    {
        public const string GUID = "tairasoul.healthupgrade";
        public const string Name = "HealthUpgrade";
        public const string Version = "1.0.1";
    }
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    internal class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;
        internal vHealthController healthController;
        internal bool init = false;
        void Awake()
        {
            Instance = this;
            Logger.LogInfo($"Plugin {PluginInfo.GUID} ({PluginInfo.Name}) version {PluginInfo.Version} loaded.");
        }

        int[] levelPrices = [100, 200, 300, 400, 500, 600];
        int[] refundPrices = [75, 150, 225, 300, 375, 450];

        int[] Upgraded(int level, int currentPrice)
        {
            bool increaseCurrent = false;
            if (healthController?.currentHealth == healthController?.maxHealth)
                increaseCurrent = true;
            if (healthController != null) healthController.maxHealth = level;
            if (increaseCurrent && healthController != null)
                healthController.currentHealth = level;
            return [levelPrices[level - 1], refundPrices[level - 1]];
        }

        int[] Downgraded(int level, int currentPrice)
        {
            if (healthController != null) healthController.maxHealth = level;
            if (healthController?.currentHealth > level && healthController != null)
                healthController.currentHealth = level;
            return [levelPrices[level - 1], refundPrices[level - 1]];
        }

        void Start() => Init();
        void OnDestroy() => Init();

        void Init()
        {
            if (init) return;
            init = true;
            GameObject upg = new("HealthUpgrade");
            DontDestroyOnLoad(upg);
            upg.AddComponent<vHealthControllerChecker>();
            Upgrade upgrade = new("Health", "healthupgrade.health", "Frame", "Toughness of Sen's frame.", Upgraded, Downgraded, 3, 1, 6, levelPrices[2], refundPrices[2]);
            Framework.AddUpgrade(upgrade);
        }
    }
}
