using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace Zomo.Core.Common
{
    public abstract class ZomoPlugin
    {
        private readonly List<ModuleInfo> _registeredModules = new List<ModuleInfo>();
        public abstract string Name { get; }

        public abstract void Init();

        public virtual void Destroy()
        {
            foreach (var module in _registeredModules)
                ZomoApplication.Instance.CommandService.RemoveModuleAsync(module);
        }

        public async Task<bool> RegisterModule(Type module)
        {
            if (!module.IsClass)
                throw new Exception("The given type isn't a class.");

            if (!module.IsSubclassOf(typeof(ModuleBase<SocketCommandContext>)))
                throw new Exception("The given type isn't a ModuleBase<SocketCommandContext>");

            var res = await ZomoApplication.Instance.CommandService.AddModuleAsync(module,
                ZomoApplication.Instance.Services);
            _registeredModules.Add(res);

            return true;
        }
    }
}