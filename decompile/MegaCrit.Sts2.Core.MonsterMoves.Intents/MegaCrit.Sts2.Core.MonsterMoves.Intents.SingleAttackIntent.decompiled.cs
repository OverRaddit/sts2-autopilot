using System;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class SingleAttackIntent : AttackIntent
{
	public override int Repeats => 1;

	protected override LocString IntentLabelFormat => new LocString("intents", "FORMAT_DAMAGE_SINGLE");

	public SingleAttackIntent(int damage)
	{
		base.DamageCalc = () => decimal.op_Implicit(damage);
	}

	public SingleAttackIntent(Func<decimal> damageCalc)
	{
		base.DamageCalc = damageCalc;
	}

	public override int GetTotalDamage(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		return GetSingleDamage(targets, owner);
	}

	public override LocString GetIntentLabel(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		LocString intentLabelFormat = IntentLabelFormat;
		float num = GetTotalDamage(targets, owner);
		intentLabelFormat.Add("Damage", decimal.op_Implicit((int)num));
		return intentLabelFormat;
	}
}
