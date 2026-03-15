using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public abstract class AttackIntent : AbstractIntent
{
	public override IntentType IntentType => IntentType.Attack;

	protected override string IntentPrefix => "ATTACK";

	[field: CompilerGenerated]
	public Func<decimal>? DamageCalc
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		protected set;
	}

	public virtual int Repeats => 0;

	protected override string SpritePath => "atlases/intent_atlas.sprites/intent_attack.tres";

	public override System.Collections.Generic.IEnumerable<string> AssetPaths
	{
		get
		{
			List<string> val = new List<string>();
			for (int i = 1; i <= 5; i++)
			{
				val.Add(ImageHelper.GetImagePath($"atlases/intent_atlas.sprites/attack/intent_attack_{i}.tres"));
			}
			return (System.Collections.Generic.IEnumerable<string>)val;
		}
	}

	public override Texture2D GetTexture(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		int totalDamage = GetTotalDamage(targets, owner);
		string text = "";
		text = ((totalDamage < 5) ? (text + "1") : ((totalDamage < 10) ? (text + "2") : ((totalDamage < 20) ? (text + "3") : ((totalDamage >= 40) ? (text + "5") : (text + "4")))));
		string imagePath = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_" + text + ".tres");
		return PreloadManager.Cache.GetTexture2D(imagePath);
	}

	public override string GetAnimation(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		string animation = base.GetAnimation(targets, owner);
		int totalDamage = GetTotalDamage(targets, owner);
		if (totalDamage < 5)
		{
			return animation + "_1";
		}
		if (totalDamage < 10)
		{
			return animation + "_2";
		}
		if (totalDamage < 20)
		{
			return animation + "_3";
		}
		if (totalDamage < 40)
		{
			return animation + "_4";
		}
		return animation + "_5";
	}

	protected override LocString GetIntentDescription(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		LocString intentDescription = base.GetIntentDescription(targets, owner);
		intentDescription.Add("Damage", decimal.op_Implicit(GetSingleDamage(targets, owner)));
		intentDescription.Add("Repeat", decimal.op_Implicit(Repeats));
		return intentDescription;
	}

	public abstract int GetTotalDamage(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner);

	public int GetSingleDamage(System.Collections.Generic.IEnumerable<Creature> targets, Creature owner)
	{
		decimal num = DamageCalc.Invoke();
		Player me = LocalContext.GetMe(owner.CombatState);
		if (me != null)
		{
			num = Hook.ModifyDamage(me.RunState, me.Creature.CombatState, me.Creature, owner, DamageCalc.Invoke(), ValueProp.Move, null, ModifyDamageHookType.All, CardPreviewMode.None, out System.Collections.Generic.IEnumerable<AbstractModel> _);
		}
		return Math.Max(0, (int)num);
	}
}
