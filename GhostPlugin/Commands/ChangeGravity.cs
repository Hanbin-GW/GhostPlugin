using System;
using CommandSystem;

namespace GhostPlugin.Commands
{
    public class ChangeGravity : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            throw new NotImplementedException();
        }

        public string Command { get; } = "Change Gravity";
        public string[] Aliases { get; } = new[] { "CG", "cg" };
        public string Description { get; } = "Change the Gravity";
    }
}