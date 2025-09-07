using System;
using NUnit.Framework;
using UnityEngine;
using Systems.Combat;
using UniRx;
using View.World;

namespace Assets.Tests.EditMode
{
    public class CombatResolverTests
    {
        private class FakeDmg : IDamageable
        {
            public bool IsAlive { get; set; } = true;

            public IObservable<Unit> Died => died;

            private readonly UniRx.Subject<UniRx.Unit> died = new UniRx.Subject<UniRx.Unit>();
            public int DamageCalls { get; private set; }
            public int LastDamageAmount { get; private set; }
            public bool KillCalled { get; private set; }
            public void Damage(int amount) { DamageCalls++; LastDamageAmount = amount; }
            public void Kill() { KillCalled = true; IsAlive = false; died.OnNext(UniRx.Unit.Default); }
        }

        private class Info : ICombatInfo
        {
            public CombatTeam Team { get; set; } = CombatTeam.Neutral;
            public Transform Transform { get; set; }
            public bool IsGrounded { get; set; }
            public int ContactDamage { get; set; }
        }

        [Test]
        public void Resolve_NullInput_NoAction()
        {
            var sut = new CombatResolver();
            var aDmg = new FakeDmg();
            var bDmg = new FakeDmg();

            Assert.DoesNotThrow(() => sut.Resolve(null, aDmg, new Info { Transform = new GameObject().transform }, bDmg));
            Assert.AreEqual(0, aDmg.DamageCalls + bDmg.DamageCalls);
        }

        [Test]
        public void Resolve_DeathZone_KillsOther()
        {
            var sut = new CombatResolver();
            var dzGo = new GameObject("DeathZone");
            dzGo.AddComponent<BoxCollider2D>();
            var dz = dzGo.AddComponent<DeathZoneFacade>();

            var otherGo = new GameObject("Other");
            var other = new Info { Transform = otherGo.transform, ContactDamage = 1 };
            var otherDmg = new FakeDmg();

            sut.Resolve(dz, null, other, otherDmg);

            Assert.True(otherDmg.KillCalled);
        }

        [Test]
        public void Resolve_JumpOn_AAbove_DamagesB()
        {
            var sut = new CombatResolver();
            var aGo = new GameObject("A");
            var bGo = new GameObject("B");
            aGo.transform.position = new Vector3(0, 1.0f, 0);
            bGo.transform.position = new Vector3(0, 0.5f, 0);

            var a = new Info { Transform = aGo.transform, ContactDamage = 3 };
            var b = new Info { Transform = bGo.transform, ContactDamage = 4 };
            var aDmg = new FakeDmg();
            var bDmg = new FakeDmg();

            sut.Resolve(a, aDmg, b, bDmg);

            Assert.AreEqual(1, bDmg.DamageCalls);
            Assert.AreEqual(3, bDmg.LastDamageAmount);
            Assert.AreEqual(0, aDmg.DamageCalls);
        }

        [Test]
        public void Resolve_JumpOn_ALower_DamagesA()
        {
            var sut = new CombatResolver();
            var aGo = new GameObject("A");
            var bGo = new GameObject("B");
            aGo.transform.position = new Vector3(0, 0.2f, 0);
            bGo.transform.position = new Vector3(0, 0.6f, 0);

            var a = new Info { Transform = aGo.transform, ContactDamage = 3 };
            var b = new Info { Transform = bGo.transform, ContactDamage = 4 };
            var aDmg = new FakeDmg();
            var bDmg = new FakeDmg();

            sut.Resolve(a, aDmg, b, bDmg);

            Assert.AreEqual(1, aDmg.DamageCalls);
            Assert.AreEqual(4, aDmg.LastDamageAmount);
            Assert.AreEqual(0, bDmg.DamageCalls);
        }

        [Test]
        public void Resolve_NotAlive_NoDamage()
        {
            var sut = new CombatResolver();
            var aGo = new GameObject("A");
            var bGo = new GameObject("B");
            aGo.transform.position = new Vector3(0, 1.0f, 0);
            bGo.transform.position = new Vector3(0, 0.5f, 0);

            var a = new Info { Transform = aGo.transform, ContactDamage = 3 };
            var b = new Info { Transform = bGo.transform, ContactDamage = 4 };
            var aDmg = new FakeDmg { IsAlive = false };
            var bDmg = new FakeDmg();

            sut.Resolve(a, aDmg, b, bDmg);

            Assert.AreEqual(0, aDmg.DamageCalls);
            Assert.AreEqual(0, bDmg.DamageCalls);
        }
    }
}
