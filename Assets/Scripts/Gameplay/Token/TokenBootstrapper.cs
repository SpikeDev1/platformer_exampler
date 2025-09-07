using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Gameplay.Token
{
    public class TokenBootstrapper : IInitializable
    {
        private readonly DiContainer container;

        public TokenBootstrapper(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize()
        {
            var tokens = Object.FindObjectsByType<TokenView>(FindObjectsInactive.Include, FindObjectsSortMode.None);


            for (var i = 0; i < tokens.Length; i++)
            {
                var go = tokens[i].gameObject;

                var ctx = go.GetComponent<GameObjectContext>();
                if (ctx == null) ctx = go.AddComponent<GameObjectContext>();

                var installer = go.GetComponent<TokenInstaller>();
                if (installer == null) installer = go.AddComponent<TokenInstaller>();

                List<MonoInstaller> listInstallers = null;

                if (ctx.Installers == null) listInstallers = new List<MonoInstaller>();

                if (!ctx.Installers.Contains(installer))
                {
                    if (listInstallers == null)
                    {
                        listInstallers = ctx.Installers.ToList();
                    }

                    listInstallers.Add(installer);
                    ctx.Installers = listInstallers;
                }

                ctx.Construct(container);
            }
        }
    }
}