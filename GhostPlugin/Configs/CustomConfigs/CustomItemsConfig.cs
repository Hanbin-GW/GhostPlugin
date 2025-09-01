using System.Collections.Generic;
using System.ComponentModel;
using GhostPlugin.Custom.Items.Armor;
using GhostPlugin.Custom.Items.Etc;
using GhostPlugin.Custom.Items.Firearms;
using GhostPlugin.Custom.Items.Grenades;
using GhostPlugin.Custom.Items.Keycard;
using GhostPlugin.Custom.Items.Medkit;
using GhostPlugin.Custom.Items.Perks;
using ProjectMER.Commands.Utility;

namespace GhostPlugin.Configs.CustomConfigs
{
    public class CustomItemsConfig
    {
        [Description("Enables Custom Items")]
        public bool IsEnabled { get; set; } = true;

        public List<HackingDevice> HackingDevices { get; private set; } = new List<HackingDevice>()
        {
            new HackingDevice()
        };
        public List<Mors> Morses { get; private set; } = new List<Mors>()
        {
            new Mors()
        };

        public List<ShockwaveGun> ShockwaveGuns { get; private set; } = new List<ShockwaveGun>()
        {
            new ShockwaveGun()
        };

        public List<GernadeLuncher> GernadeLunchers { get; private set; } = new List<GernadeLuncher>()
        {
            new GernadeLuncher()
        };
        public List<ReconBattleRife> FtacReacon { get; private set; } = new List<ReconBattleRife>()
        {
            new ReconBattleRife()
        };

        public List<PoisonGun> PoisonGuns { get; private set; } = new List<PoisonGun>()
        {
            new PoisonGun()
        };

        public List<ClusterGrenade> ClusterGrenades { get; private set; } = new List<ClusterGrenade>()
        {
            new ClusterGrenade()
        };

        public List<TripleFlashGrenade> TripleFlashGrenades { get; private set; } = new List<TripleFlashGrenade>()
        {
            new TripleFlashGrenade()
        };
        public List<StunGrenade> StunGrenades { get; private set; } = new List<StunGrenade>()
        {
            new StunGrenade()
        };

        public List<Stim> Stims { get; private set; } = new List<Stim>()
        {
            new Stim()
        };
        public List<BattleRage> BattleRages { get; private set; } = new List<BattleRage>()
        {
            new BattleRage()
        };
        public List<Svd> Svds { get; private set; } = new List<Svd>()
        {
            new Svd()
        };

        public List<EodPadding> EodPaddings { get; private set; } = new List<EodPadding>()
        {
            new EodPadding()
        };
        public List<SmokeGrenade> SmokeGrenades { get; private set; } = new List<SmokeGrenade>()
        {
            new SmokeGrenade()
        };
        public List<ExplosiveRoundRevolver> ExplosiveRoundRevolvers { get; private set; } = new List<ExplosiveRoundRevolver>()
        {
            new ExplosiveRoundRevolver()
        };

        public List<ParalyzeRife> ParalyzeRifes { get; private set; } = new List<ParalyzeRife>()
        {
            new ParalyzeRife()
        };
        public List<PlasmaEmitter> PlasmaEmitters { get; private set; } = new List<PlasmaEmitter>()
        {
            new PlasmaEmitter()
        };

        public List<PlasmaShockwaveEmitter> PlasmaShockwaveEmitters { get; private set; } = new List<PlasmaShockwaveEmitter>()
        {
            new PlasmaShockwaveEmitter() 
        };

        public List<PhotonCannon> PhotonCannons { get; private set; } = new List<PhotonCannon>()
        {
            new PhotonCannon()
        };

        public List<ArmorPlateKit> ArmorPlateKits { get; private set; } = new List<ArmorPlateKit>()
        {
            new ArmorPlateKit()
        };

        public List<ImpactGrenade> ImpactGrenades { get; private set; } = new List<ImpactGrenade>()
        {
            new ImpactGrenade()
        };
        public List<StickyGrenade> StickyGrenades { get; private set; } = new List<StickyGrenade>()
        {
            new StickyGrenade()
        };
        public List<PlasmaShotgun> PlasmaShotguns { get; private set; } = new List<PlasmaShotgun>()
        {
            new PlasmaShotgun()
        };

        public List<Bolter> Bolters { get; private set; } = new List<Bolter>()
        {
            new Bolter()
        };

        public List<AcidShooter> AcidShooters { get; private set; } = new List<AcidShooter>()
        {
            new AcidShooter()
        };

        public List<C4> C4s { get; private set; } = new List<C4>()
        {
            new C4()
        };
        public List<SpikeJailbird> SpikeJailbirds { get; private set; } = new List<SpikeJailbird>()
        {
            new SpikeJailbird()
        };

        public List<ReviveKit> ReviveKits { get; private set; } = new List<ReviveKit>()
        {
            new ReviveKit()
        };

        public List<PoisonGrenade> PoisonGrenades { get; private set; } = new List<PoisonGrenade>()
        {
            new PoisonGrenade()
        };

        public List<LaserCannon> LaserCannons { get; private set; } = new List<LaserCannon>()
        {
            new LaserCannon()
        };

        public List<Anti173> Anti173s { get; private set; } = new List<Anti173>()
        {
            new Anti173()
        };

        public List<Basilisk> Basilisks { get; private set; } = new List<Basilisk>()
        {
            new Basilisk()
        };

        public List<AmmoBox> AmmoBoxes { get; private set; } = new List<AmmoBox>()
        {
            new AmmoBox()
        };

        public List<TrophySystem> TrophySystems { get; private set; } = new List<TrophySystem>()
        {
            new TrophySystem()
        };
        public List<OverkillVest> OverkillVests { get; private set; } = new List<OverkillVest>()
        {
            new OverkillVest()
        };

        public List<PlasmaBlaster> PlasmaBlasters { get; private set; } = new List<PlasmaBlaster>()
        {
            new PlasmaBlaster()
        };

        public List<MachineGun> MachineGuns { get; private set; } = new List<MachineGun>()
        {
            new MachineGun()
        };

        public List<Riveter> Riveters { get; private set; } = new List<Riveter>()
        {
            new Riveter()
        };

        public List<LaserGun> LaserGuns { get; private set; } = new List<LaserGun>()
        {
            new LaserGun()
        };

        public List<Ballista> MorsReworks { get; private set; } = new List<Ballista>()
        {
            new Ballista()
        };

        public List<PortableEnergyShild> PortableEnergyShilds { get; private set; } = new List<PortableEnergyShild>()
        {
            new PortableEnergyShild()
        };

        public List<M16> M16s { get; private set; } = new List<M16>()
        {
            new M16()
        };

        public List<QuickfixPerk> QuickfixPerks { get; private set; } = new List<QuickfixPerk>()
        {
            new QuickfixPerk()
        };
        public List<FocusPerk> FocusPerks { get; private set; } = new List<FocusPerk>()
        {
            new FocusPerk()
        };

        public List<BoostOnKillPerk> BoostOnKillPerks { get; private set; } = new List<BoostOnKillPerk>()
        {
            new BoostOnKillPerk()
        };

        public List<MartydomPerk> MartydomPerks { get; private set; } = new List<MartydomPerk>()
        {
            new MartydomPerk()
        };

        public List<EngineerPerk> EngineerPerks { get; private set; } = new List<EngineerPerk>()
        {
            new EngineerPerk()
        };

        public List<OverkillPerk> OverkillPerks { get; private set; } = new List<OverkillPerk>()
        {
            new OverkillPerk()
        };
    }
}