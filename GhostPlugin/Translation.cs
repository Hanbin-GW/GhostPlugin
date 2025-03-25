using System.Collections.Generic;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
namespace GhostPlugin
{
    public class Translation : ITranslation
    {
        public Dictionary<string, string> CustomItemMessages { get; set; } = new Dictionary<string, string>()
        {
            {"en","You Recieve a Custom Item!"},
            {"kr","특수무기를 획득하셧 습니다."}
        };
    }
}