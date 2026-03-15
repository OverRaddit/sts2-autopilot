using System;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class MultiAttackIntent : AttackIntent
{
	private readonly int _repeat;

	private readonly Func<int>? _repeatCalc;

	protected override LocString IntentLabelFormat => new LocString("intents", "FORMAT_DAMAGE_MULTI");

	public override int Repeats => _repeatCalc?.Invoke() ?? _repeat;

	public MultiAttackIntent(int damage, int repeat)
	{
		base.DamageCalc = () => decimal.op_Implicit(damage);
		_repeat = repeat;
	}

	public MultiAttackIntent(int damage, Func<int> repeatCalc)
	{
		base.DamageCalc = () => decimal.op_Implicit(damage);
		_repeatCalc = repeatCalc;
	}

	public override int GetTotalDamage(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		return GetSingleDamage(targets, owner) * Repeats;
	}

	public override LocString GetIntentLabel(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		LocString intentLabelFormat = IntentLabelFormat;
		float num = GetSingleDamage(targets, owner);
		intentLabelFormat.Add("Damage", decimal.op_Implicit((int)num));
		intentLabelFormat.Add("Repeat", decimal.op_Implicit(Repeats));
		return intentLabelFormat;
	}
}
