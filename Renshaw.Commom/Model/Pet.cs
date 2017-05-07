using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class Pet : IMultiUniqueEntity
    {
        public int UserId { get; set; }

        public int SourceId { get; set; }

        public long UniqueId { get; set; }

        public string Name { get; set; }

        public PetType PetType { get; set; }

        public int Level { get; set; }

        public int Star { get; set; }

        public int CurrentExp { get; set; }

        public PetBattleProperty BattleProperty { get; set; }

    }

    public class PetBattleProperty
    {
        public double HpMax { get; set; }

        public double MpMax { get; set; }

        public double Attack { get; set; }

        public double Defend { get; set; }

        public double CritRate { get; set; }

    }

    public enum PetType
    {
        /// <summary>
        /// 战士
        /// </summary>
        Soldier = 0,
        /// <summary>
        /// 盾兵
        /// </summary>
        Mauler = 1,
        /// <summary>
        /// 治疗
        /// </summary>
        Healer = 2,
        /// <summary>
        /// 弓箭手
        /// </summary>
        Archer = 3
    }
}
