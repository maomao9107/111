using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.Class;
using Newtonsoft.Json;

namespace FollowBot
{
    public class FollowBotSettings : JsonSettings
    {
        private static FollowBotSettings _instance;
        public static FollowBotSettings Instance => _instance ?? (_instance = new FollowBotSettings());

        private FollowBotSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name, "FollowBot.json"))
        {
            if (DefensiveSkills == null)
                DefensiveSkills = SetupDefaultDefensiveSkills();
            if (Flasks == null)
                Flasks = SetupDefaultFlasks();
        }

        private bool _ignoreHiddenAuras;

        [DefaultValue(false)]
        public bool IgnoreHiddenAuras
        {
            get { return _ignoreHiddenAuras; }
            set
            { _ignoreHiddenAuras = value; NotifyPropertyChanged(() => IgnoreHiddenAuras); }
        }
        
        private string _acceptedManualInviteNames;
        private int _followDistance;
        private int _maxfollowDistance;
        private int _maxCombatDistance;
        private int _maxLootDistance;

        #region Party Role
        private bool _shouldKill;
        private bool _shouldLoot;
        #endregion

        #region Defence
        private ObservableCollection<DefensiveSkillsClass> _defensiveSkills;
        private ObservableCollection<FlasksClass> _flasks;
        #endregion

        #region Gems
        private bool _gemDebugStatements;
        private bool _levelAllGems;
        private bool _levelOffhandOnly;
        private ObservableCollection<string> _globalNameIgnoreList;

        #endregion

        [DefaultValue("")]
        public string InviteWhiteList
        {
            get { return _acceptedManualInviteNames; }
            set
            { _acceptedManualInviteNames = value; NotifyPropertyChanged(() => InviteWhiteList); }
        }
        [DefaultValue(15)]
        public int FollowDistance
        {
            get { return _followDistance; }
            set
            { _followDistance = value; NotifyPropertyChanged(() => FollowDistance); }
        }
        [DefaultValue(25)]
        public int MaxFollowDistance
        {
            get { return _maxfollowDistance; }
            set
            { _maxfollowDistance = value; NotifyPropertyChanged(() => MaxFollowDistance); }
        }
        [DefaultValue(40)]
        public int MaxCombatDistance
        {
            get { return _maxCombatDistance; }
            set
            { _maxCombatDistance = value; NotifyPropertyChanged(() => MaxCombatDistance); }
        }
        [DefaultValue(40)]
        public int MaxLootDistance
        {
            get { return _maxLootDistance; }
            set
            { _maxLootDistance = value; NotifyPropertyChanged(() => MaxLootDistance); }
        }
        #region Party Role
        [DefaultValue(true)]
        public bool ShouldKill
        {
            get { return _shouldKill; }
            set
            { _shouldKill = value; NotifyPropertyChanged(() => ShouldKill); }
        }
        [DefaultValue(false)]
        public bool ShouldLoot
        {
            get { return _shouldLoot; }
            set
            { _shouldLoot = value; NotifyPropertyChanged(() => ShouldLoot); }
        }
        #endregion

        #region Defence Skills
        public ObservableCollection<DefensiveSkillsClass> DefensiveSkills
        {
            get => _defensiveSkills ;//?? (_defensiveSkills = new ObservableCollection<DefensiveSkillsClass>());
            set
            {
                _defensiveSkills = value;
                NotifyPropertyChanged(() => DefensiveSkills);
            }
        }

        #endregion

        #region Flasks
        public ObservableCollection<FlasksClass> Flasks
        {
            get => _flasks ;//?? (_flasks = new ObservableCollection<FlasksClass>());
            set
            {
                _flasks = value;
                NotifyPropertyChanged(() => Flasks);
            }
        }
        private ObservableCollection<DefensiveSkillsClass> SetupDefaultDefensiveSkills()
        {
            ObservableCollection<DefensiveSkillsClass> skills = new ObservableCollection<DefensiveSkillsClass>();

            skills.Add(new DefensiveSkillsClass(false, "Vaal Molten Shell", false, 0, 0,false));
            skills.Add(new DefensiveSkillsClass(false, "Vaal Discipline", false, 0, 0, false));
            skills.Add(new DefensiveSkillsClass(false, "Molten Shell", false, 0, 0, false));
            skills.Add(new DefensiveSkillsClass(false, "Steelskin", false, 0, 0, false));
            return skills;
        }
        private ObservableCollection<FlasksClass> SetupDefaultFlasks()
        {
            ObservableCollection<FlasksClass> flasks = new ObservableCollection<FlasksClass>();

            flasks.Add(new FlasksClass(false, 1, false, false, 0));
            flasks.Add(new FlasksClass(false, 2, false, false, 0));
            flasks.Add(new FlasksClass(false, 3, false, false, 0));
            flasks.Add(new FlasksClass(false, 4, false, false, 0));
            flasks.Add(new FlasksClass(false, 5, false, false, 0));
            return flasks;
        }

        #endregion

        #region SkillGems
        /// <summary>
		/// Should the plugin log debug statements?
		/// </summary>
		[DefaultValue(false)]
        public bool GemDebugStatements
        {
            get
            {
                return _gemDebugStatements;
            }
            set
            {
                if (value.Equals(_gemDebugStatements))
                {
                    return;
                }
                _gemDebugStatements = value;
                NotifyPropertyChanged(() => GemDebugStatements);
            }
        }
        [DefaultValue(false)]
        public bool LevelOffhandOnly
        {
            get
            {
                return _levelOffhandOnly;
            }
            set
            {
                if (value.Equals(_levelOffhandOnly))
                {
                    return;
                }
                _levelOffhandOnly = value;
                NotifyPropertyChanged(() => LevelOffhandOnly);
            }
        }
        [DefaultValue(false)]
        public bool LevelAllGems
        {
            get
            {
                return _levelAllGems;
            }
            set
            {
                if (value.Equals(_levelAllGems))
                {
                    return;
                }
                _levelAllGems = value;
                NotifyPropertyChanged(() => LevelAllGems);
            }
        }
        /// <summary>
		/// A list of skillgem names to ignore from leveling.
		/// </summary>
		public ObservableCollection<string> GlobalNameIgnoreList
        {
            get
            {
                return _globalNameIgnoreList ?? (_globalNameIgnoreList = new ObservableCollection<string>());
            }
            set
            {
                if (value.Equals(_globalNameIgnoreList))
                {
                    return;
                }
                _globalNameIgnoreList = value;
                NotifyPropertyChanged(() => GlobalNameIgnoreList);
            }
        }

        /// <summary>
		/// A list of SkillGemEntry for the user's skillgems.
		/// </summary>
		[JsonIgnore]
        public ObservableCollection<SkillGemEntry> UserSkillGemsInOffHands
        {
            get
            {
                //using (LokiPoe.AcquireFrame())
                //{
                ObservableCollection<SkillGemEntry> skillGemEntries = new ObservableCollection<SkillGemEntry>();

                if (!LokiPoe.IsInGame)
                {
                    return skillGemEntries;
                }

                foreach (Inventory inv in UsableOffInventories)
                {
                    foreach (Item item in inv.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.Components.SocketsComponent == null)
                        {
                            continue;
                        }

                        for (int idx = 0; idx < item.SocketedGems.Length; idx++)
                        {
                            Item gem = item.SocketedGems[idx];
                            if (gem == null)
                            {
                                continue;
                            }

                            skillGemEntries.Add(new SkillGemEntry(gem.Name, inv.PageSlot, idx));
                        }
                    }
                }
                return skillGemEntries;
                //}
            }
        }
        /// <summary>
        /// A list of SkillGemEntry for the user's skillgems.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<SkillGemEntry> UserSkillGems
        {
            get
            {
                //using (LokiPoe.AcquireFrame())
                //{
                ObservableCollection<SkillGemEntry> skillGemEntries = new ObservableCollection<SkillGemEntry>();

                if (!LokiPoe.IsInGame)
                {
                    return skillGemEntries;
                }

                foreach (Inventory inv in UsableInventories)
                {
                    foreach (Item item in inv.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.Components.SocketsComponent == null)
                        {
                            continue;
                        }

                        for (int idx = 0; idx < item.SocketedGems.Length; idx++)
                        {
                            Item gem = item.SocketedGems[idx];
                            if (gem == null)
                            {
                                continue;
                            }

                            skillGemEntries.Add(new SkillGemEntry(gem.Name, inv.PageSlot, idx));
                        }
                    }
                }
                return skillGemEntries;
                //}
            }
        }
        public void UpdateGlobalNameIgnoreList()
        {
            NotifyPropertyChanged(() => GlobalNameIgnoreList);
        }
        public class SkillGemEntry
        {
            public string Name;
            public InventorySlot InventorySlot;
            public int SocketIndex;

            public string SerializationString { get; private set; }

            public SkillGemEntry(string name, InventorySlot slot, int socketIndex)
            {
                Name = name;
                InventorySlot = slot;
                SocketIndex = socketIndex;
                SerializationString = string.Format("{0} [{1}: {2}]", Name, InventorySlot, SocketIndex);
            }

            public Item InventoryItem
            {
                get
                {
                    return UsableInventories.Where(ui => ui.PageSlot == InventorySlot)
                        .Select(ui => ui.Items.FirstOrDefault())
                        .FirstOrDefault();
                }
            }

            public Item SkillGem
            {
                get
                {
                    Item item = InventoryItem;
                    if (item == null || item.Components.SocketsComponent == null)
                    {
                        return null;
                    }

                    Item sg = item.SocketedGems[SocketIndex];
                    if (sg == null)
                    {
                        return null;
                    }

                    if (sg.Name != Name)
                    {
                        return null;
                    }

                    return sg;
                }
            }
        }
        private static IEnumerable<Inventory> UsableInventories => new[]
        {
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.LeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.RightHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffLeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffRightHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Head),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Chest),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Gloves),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Boots),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.LeftRing),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.RightRing),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Neck)
        };
        private static IEnumerable<Inventory> UsableOffInventories => new[]
        {
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffLeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffRightHand),
        };
        #endregion
    }
}

    