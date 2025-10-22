using System.ComponentModel;
using GhostPlugin.Custom.Roles.Chaos;
using GhostPlugin.Custom.Roles.ClassD;
using GhostPlugin.Custom.Roles.Foundation;
using GhostPlugin.Custom.Roles.Scps;
using GhostPlugin.Custom.Roles.Scientist;


namespace GhostPlugin.Configs.CustomConfigs
{
    using System.Collections.Generic;
    public class CustomRolesConfig
    {
        [Description("Enables Custom Items")]
        public bool IsEnabled { get; set; } = true;
        public List<ChiefScientist> ChiefScientists { get; set; } = new()
        {
            new ChiefScientist()
        };
        public List<D_Alpha> DAlphas { get; set; } = new()
        {
            new D_Alpha()
        };

        public List<Tanker106> Tanker106S { get; set; } = new()
        {
            new Tanker106()
        };

        public List<Viper> Vipers { get; set; } = new()
        {
            new Viper()
        };

        public List<Jailbirdman> Jailbirdmans { get; set; } = new()
        {
            new Jailbirdman()
        };

        public List<LuckyGuard> LuckyGuards { get; set; } = new()
        {
            new LuckyGuard()
        };

        public List<Gunslinger> Gunslingers { get; set; } = new()
        {
            new Gunslinger()
        };
        public List<O5Administrator> Administrators { get; set; } = new()
        {
            new O5Administrator()
        };

        public List<CiPhantom> CiPhantoms { get; set; } = new()
        {
            new CiPhantom()
        };

        public List<FedoraAgent> FedoraAgents { get; set; } = new()
        {
            new FedoraAgent()
        };

        public List<Elite> Elites { get; set; } = new()
        {
            new Elite()
        };

        public List<JuggernautChaos> JuggernautChaosList { get; set; } = new()
        {
            new JuggernautChaos()
        };

        public List<Scp682> Scp682s { get; set; } = new()
        {
            new Scp682()
        };

        public List<SoleStealer049> SoleStealer049s { get; set; } = new()
        {
            new SoleStealer049()
        };

        public List<Scp049AP> Scp049Aps { get; set; } = new()
        {
            new Scp049AP()
        };
        public List<Demolitionist> Demolitionists { get; set; } = new()
        {
            new Demolitionist()
        };

        public List<Dwarf> Dwarves { get; set; } = new()
        {
            new Dwarf()
        };

        public List<SpyAgent> SpyAgents { get; set; } = new()
        {
            new SpyAgent()
        };

        public List<Enforcer> Enforcers { get; set; } = new()
        {
            new Enforcer()
        };

        public List<Strategist> Strategists { get; set; } = new()
        {
            new Strategist()
        };

        public List<Quartermaster> Quartermasters { get; set; } = new()
        {
            new Quartermaster()
        };

        public List<Medic> Medics { get; set; } = new()
        {
            new Medic()
        };

        public List<AdvancedMTF> AdvancedMtfs { get; set; } = new()
        {
            new AdvancedMTF()
        };
        public List<Hunter> Hunters { get; set; } = new()
        {
            new Hunter()
        };

        public List<HugoBoss> HugoBosses { get; set; } = new()
        {
            new HugoBoss()
        };
        public List<Tracker> Trackers { get; set; } = new List<Tracker>()
        {
            new Tracker()
        };
        public List<Director> Directors { get; set; } = new List<Director>()
        {
            new Director()
        };

        public List<Commando> Commandos { get; set; } = new List<Commando>()
        {
            new Commando()
        };
        public List<DwarfZombie> DwarfZombies { get; set; } = new()
        {
            new DwarfZombie()
        };
        public List<ExplosiveZombie> ExplosiveZombies { get; set; } = new()
        {
            new ExplosiveZombie()
        };

        public List<EodSoldierZombie> EodSoldierZombies { get; set; } = new()
        {
            new EodSoldierZombie()
        };

        public List<ShockWaveZombie> ShockWaveZombies { get; set; } = new()
        {
            new ShockWaveZombie()
        };

        public List<ReinforceZombie> ReinforceZombies { get; set; } = new()
        {
            new ReinforceZombie()
        };
    }
}