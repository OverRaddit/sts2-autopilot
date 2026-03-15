using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models;

public abstract class CardModel : AbstractModel
{
	private enum DescriptionPreviewType
	{
		None,
		Upgrade
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <MoveToResultPileWithoutPlaying>d__330 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public CardModel <>4__this;

		public PlayerChoiceContext choiceContext;

		private TaskAwaiter <>u__1;

		private TaskAwaiter<CardPileAddResult> <>u__2;

		private void MoveNext()
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel cardModel = <>4__this;
			try
			{
				TaskAwaiter val3;
				TaskAwaiter val2;
				TaskAwaiter<CardPileAddResult> val;
				switch (num)
				{
				default:
				{
					CardPile? pile = cardModel.Pile;
					if (pile != null && pile.Type == PileType.Play)
					{
						if (cardModel.IsDupe)
						{
							val3 = CardPileCmd.RemoveFromCombat(cardModel, isBeingPlayed: false).GetAwaiter();
							if (!((TaskAwaiter)(ref val3)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val3;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <MoveToResultPileWithoutPlaying>d__330>(ref val3, ref this);
								return;
							}
							goto IL_009d;
						}
						if (cardModel.ExhaustOnNextPlay || cardModel.Keywords.Contains(CardKeyword.Exhaust))
						{
							val2 = CardCmd.Exhaust(choiceContext, cardModel).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <MoveToResultPileWithoutPlaying>d__330>(ref val2, ref this);
								return;
							}
							goto IL_011b;
						}
						val = CardPileCmd.Add(cardModel, PileType.Discard).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__2 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CardPileAddResult>, <MoveToResultPileWithoutPlaying>d__330>(ref val, ref this);
							return;
						}
						break;
					}
					goto end_IL_000e;
				}
				case 0:
					val3 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_009d;
				case 1:
					val2 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_011b;
				case 2:
					{
						val = <>u__2;
						<>u__2 = default(TaskAwaiter<CardPileAddResult>);
						num = (<>1__state = -1);
						break;
					}
					IL_009d:
					((TaskAwaiter)(ref val3)).GetResult();
					goto end_IL_000e;
					IL_011b:
					((TaskAwaiter)(ref val2)).GetResult();
					goto end_IL_000e;
				}
				val.GetResult();
				end_IL_000e:;
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
				return;
			}
			<>1__state = -2;
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <OnPlayWrapper>d__327 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public CardModel <>4__this;

		public PlayerChoiceContext choiceContext;

		public Creature target;

		public bool isAutoPlay;

		public bool skipCardPileVisuals;

		public ResourceInfo resources;

		private CombatState <combatState>5__2;

		private PileType <resultPileType>5__3;

		private CardPilePosition <resultPilePosition>5__4;

		private int <playCount>5__5;

		private ulong <playStartTime>5__6;

		private TaskAwaiter <>u__1;

		private TaskAwaiter<CardPileAddResult> <>u__2;

		private System.Collections.Generic.IEnumerator<AbstractModel> <>7__wrap6;

		private int <i>5__8;

		private CardPlay <cardPlay>5__9;

		private AfflictionModel <affliction>5__10;

		private void MoveNext()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0661: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_079c: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0860: Unknown result type (might be due to invalid IL or missing references)
			//IL_0865: Unknown result type (might be due to invalid IL or missing references)
			//IL_086d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0903: Unknown result type (might be due to invalid IL or missing references)
			//IL_0967: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0974: Unknown result type (might be due to invalid IL or missing references)
			//IL_09db: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0648: Unknown result type (might be due to invalid IL or missing references)
			//IL_0766: Unknown result type (might be due to invalid IL or missing references)
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_081d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0823: Unknown result type (might be due to invalid IL or missing references)
			//IL_082a: Unknown result type (might be due to invalid IL or missing references)
			//IL_082f: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0781: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0845: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_08db: Unknown result type (might be due to invalid IL or missing references)
			//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0931: Unknown result type (might be due to invalid IL or missing references)
			//IL_0936: Unknown result type (might be due to invalid IL or missing references)
			//IL_094c: Unknown result type (might be due to invalid IL or missing references)
			//IL_094e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c2: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel cardModel = <>4__this;
			try
			{
				TaskAwaiter val17;
				TaskAwaiter<CardPileAddResult> val16;
				TaskAwaiter val15;
				TaskAwaiter val14;
				TaskAwaiter val13;
				TaskAwaiter val12;
				TaskAwaiter val11;
				TaskAwaiter val10;
				TaskAwaiter val9;
				TaskAwaiter val8;
				TaskAwaiter val7;
				TaskAwaiter val6;
				TaskAwaiter val5;
				TaskAwaiter val4;
				TaskAwaiter val3;
				TaskAwaiter<CardPileAddResult> val2;
				TaskAwaiter val;
				ValueTuple<PileType, CardPilePosition> val18;
				System.Collections.Generic.IEnumerable<AbstractModel> modifiers;
				CardPile? pile;
				switch (num)
				{
				default:
					<combatState>5__2 = cardModel.CombatState;
					choiceContext.PushModel(cardModel);
					val17 = CombatManager.Instance.WaitForUnpause().GetAwaiter();
					if (!((TaskAwaiter)(ref val17)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val17;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val17, ref this);
						return;
					}
					goto IL_00cf;
				case 0:
					val17 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_00cf;
				case 1:
					val16 = <>u__2;
					<>u__2 = default(TaskAwaiter<CardPileAddResult>);
					num = (<>1__state = -1);
					goto IL_014d;
				case 2:
					val15 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_01cb;
				case 3:
					val14 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_022b;
				case 4:
				{
					try
					{
						if (num != 4)
						{
							goto IL_02fb;
						}
						TaskAwaiter val19 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_02f4;
						IL_02f4:
						((TaskAwaiter)(ref val19)).GetResult();
						goto IL_02fb;
						IL_02fb:
						if (((System.Collections.IEnumerator)<>7__wrap6).MoveNext())
						{
							AbstractModel current = <>7__wrap6.Current;
							val19 = current.AfterModifyingCardPlayResultPileOrPosition(cardModel, <resultPileType>5__3, <resultPilePosition>5__4).GetAwaiter();
							if (!((TaskAwaiter)(ref val19)).IsCompleted)
							{
								num = (<>1__state = 4);
								<>u__1 = val19;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val19, ref this);
								return;
							}
							goto IL_02f4;
						}
					}
					finally
					{
						if (num < 0 && <>7__wrap6 != null)
						{
							((System.IDisposable)<>7__wrap6).Dispose();
						}
					}
					<>7__wrap6 = null;
					<playCount>5__5 = cardModel.GetEnchantedReplayCount() + 1;
					<playCount>5__5 = Hook.ModifyCardPlayCount(<combatState>5__2, cardModel, <playCount>5__5, target, out List<AbstractModel> modifyingModels);
					val13 = Hook.AfterModifyingCardPlayCount(<combatState>5__2, cardModel, (System.Collections.Generic.IEnumerable<AbstractModel>)modifyingModels).GetAwaiter();
					if (!((TaskAwaiter)(ref val13)).IsCompleted)
					{
						num = (<>1__state = 5);
						<>u__1 = val13;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val13, ref this);
						return;
					}
					goto IL_03b8;
				}
				case 5:
					val13 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_03b8;
				case 6:
					val12 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0436;
				case 7:
					val11 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_04b6;
				case 8:
					val10 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0579;
				case 9:
					val9 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_05ff;
				case 10:
					val8 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_067d;
				case 11:
					val7 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_070f;
				case 12:
					val6 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_07b8;
				case 13:
					val5 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_087c;
				case 14:
					val4 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0912;
				case 15:
					val3 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0983;
				case 16:
					val2 = <>u__2;
					<>u__2 = default(TaskAwaiter<CardPileAddResult>);
					num = (<>1__state = -1);
					goto IL_09f7;
				case 17:
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						break;
					}
					IL_07bf:
					<cardPlay>5__9 = null;
					<i>5__8++;
					goto IL_07d8;
					IL_00cf:
					((TaskAwaiter)(ref val17)).GetResult();
					cardModel.CurrentTarget = target;
					if (isAutoPlay)
					{
						val16 = CardPileCmd.Add(cardModel, PileType.Play, CardPilePosition.Bottom, null, skipCardPileVisuals).GetAwaiter();
						if (!val16.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val16;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CardPileAddResult>, <OnPlayWrapper>d__327>(ref val16, ref this);
							return;
						}
						goto IL_014d;
					}
					val14 = CardPileCmd.AddDuringManualCardPlay(cardModel).GetAwaiter();
					if (!((TaskAwaiter)(ref val14)).IsCompleted)
					{
						num = (<>1__state = 3);
						<>u__1 = val14;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val14, ref this);
						return;
					}
					goto IL_022b;
					IL_070f:
					((TaskAwaiter)(ref val7)).GetResult();
					<affliction>5__10.InvokeExecutionFinished();
					<affliction>5__10 = null;
					goto IL_0728;
					IL_05ff:
					((TaskAwaiter)(ref val9)).GetResult();
					cardModel.InvokeExecutionFinished();
					if (cardModel.Enchantment != null)
					{
						val8 = cardModel.Enchantment.OnPlay(choiceContext, <cardPlay>5__9).GetAwaiter();
						if (!((TaskAwaiter)(ref val8)).IsCompleted)
						{
							num = (<>1__state = 10);
							<>u__1 = val8;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val8, ref this);
							return;
						}
						goto IL_067d;
					}
					goto IL_068f;
					IL_014d:
					val16.GetResult();
					if (!skipCardPileVisuals)
					{
						val15 = Cmd.CustomScaledWait(0.25f, 0.35f).GetAwaiter();
						if (!((TaskAwaiter)(ref val15)).IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__1 = val15;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val15, ref this);
							return;
						}
						goto IL_01cb;
					}
					goto IL_0232;
					IL_0983:
					((TaskAwaiter)(ref val3)).GetResult();
					goto IL_09ff;
					IL_0579:
					((TaskAwaiter)(ref val10)).GetResult();
					CombatManager.Instance.History.CardPlayStarted(<combatState>5__2, <cardPlay>5__9);
					val9 = cardModel.OnPlay(choiceContext, <cardPlay>5__9).GetAwaiter();
					if (!((TaskAwaiter)(ref val9)).IsCompleted)
					{
						num = (<>1__state = 9);
						<>u__1 = val9;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val9, ref this);
						return;
					}
					goto IL_05ff;
					IL_01cb:
					((TaskAwaiter)(ref val15)).GetResult();
					goto IL_0232;
					IL_09f7:
					val2.GetResult();
					goto IL_09ff;
					IL_0728:
					CombatManager.Instance.History.CardPlayFinished(<combatState>5__2, <cardPlay>5__9);
					if (CombatManager.Instance.IsInProgress)
					{
						val6 = Hook.AfterCardPlayed(<combatState>5__2, choiceContext, <cardPlay>5__9).GetAwaiter();
						if (!((TaskAwaiter)(ref val6)).IsCompleted)
						{
							num = (<>1__state = 12);
							<>u__1 = val6;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val6, ref this);
							return;
						}
						goto IL_07b8;
					}
					goto IL_07bf;
					IL_022b:
					((TaskAwaiter)(ref val14)).GetResult();
					goto IL_0232;
					IL_0232:
					val18 = Hook.ModifyCardPlayResultPileTypeAndPosition(<combatState>5__2, cardModel, isAutoPlay, resources, cardModel.GetResultPileType(), CardPilePosition.Bottom, out modifiers);
					<resultPileType>5__3 = val18.Item1;
					<resultPilePosition>5__4 = val18.Item2;
					<>7__wrap6 = modifiers.GetEnumerator();
					goto case 4;
					IL_067d:
					((TaskAwaiter)(ref val8)).GetResult();
					cardModel.Enchantment.InvokeExecutionFinished();
					goto IL_068f;
					IL_0912:
					((TaskAwaiter)(ref val4)).GetResult();
					goto IL_09ff;
					IL_03b8:
					((TaskAwaiter)(ref val13)).GetResult();
					<playStartTime>5__6 = Time.GetTicksMsec();
					<i>5__8 = 0;
					goto IL_07d8;
					IL_07d8:
					if (<i>5__8 < <playCount>5__5)
					{
						if (cardModel.Type == CardType.Power)
						{
							val12 = cardModel.PlayPowerCardFlyVfx().GetAwaiter();
							if (!((TaskAwaiter)(ref val12)).IsCompleted)
							{
								num = (<>1__state = 6);
								<>u__1 = val12;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val12, ref this);
								return;
							}
							goto IL_0436;
						}
						if (<i>5__8 > 0)
						{
							NCard nCard = NCard.FindOnTable(cardModel);
							if (nCard != null)
							{
								val11 = nCard.AnimMultiCardPlay().GetAwaiter();
								if (!((TaskAwaiter)(ref val11)).IsCompleted)
								{
									num = (<>1__state = 7);
									<>u__1 = val11;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val11, ref this);
									return;
								}
								goto IL_04b6;
							}
						}
						goto IL_04bd;
					}
					if (!skipCardPileVisuals)
					{
						float num2 = (float)(Time.GetTicksMsec() - <playStartTime>5__6) / 1000f;
						val5 = Cmd.CustomScaledWait(0.15f - num2, 0.3f - num2).GetAwaiter();
						if (!((TaskAwaiter)(ref val5)).IsCompleted)
						{
							num = (<>1__state = 13);
							<>u__1 = val5;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val5, ref this);
							return;
						}
						goto IL_087c;
					}
					goto IL_0883;
					IL_09ff:
					val = CombatManager.Instance.CheckForEmptyHand(choiceContext, cardModel.Owner).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 17);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val, ref this);
						return;
					}
					break;
					IL_068f:
					if (cardModel.Affliction != null)
					{
						<affliction>5__10 = cardModel.Affliction;
						val7 = <affliction>5__10.OnPlay(choiceContext, target).GetAwaiter();
						if (!((TaskAwaiter)(ref val7)).IsCompleted)
						{
							num = (<>1__state = 11);
							<>u__1 = val7;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val7, ref this);
							return;
						}
						goto IL_070f;
					}
					goto IL_0728;
					IL_0436:
					((TaskAwaiter)(ref val12)).GetResult();
					goto IL_04bd;
					IL_087c:
					((TaskAwaiter)(ref val5)).GetResult();
					goto IL_0883;
					IL_0883:
					pile = cardModel.Pile;
					if (pile != null && pile.Type == PileType.Play)
					{
						PileType pileType = <resultPileType>5__3;
						if (pileType != PileType.None)
						{
							if (pileType == PileType.Exhaust)
							{
								val3 = CardCmd.Exhaust(choiceContext, cardModel, causedByEthereal: false, skipCardPileVisuals).GetAwaiter();
								if (!((TaskAwaiter)(ref val3)).IsCompleted)
								{
									num = (<>1__state = 15);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val3, ref this);
									return;
								}
								goto IL_0983;
							}
							val2 = CardPileCmd.Add(cardModel, <resultPileType>5__3, <resultPilePosition>5__4, null, skipCardPileVisuals).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 16);
								<>u__2 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CardPileAddResult>, <OnPlayWrapper>d__327>(ref val2, ref this);
								return;
							}
							goto IL_09f7;
						}
						val4 = CardPileCmd.RemoveFromCombat(cardModel, isBeingPlayed: true, skipCardPileVisuals).GetAwaiter();
						if (!((TaskAwaiter)(ref val4)).IsCompleted)
						{
							num = (<>1__state = 14);
							<>u__1 = val4;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val4, ref this);
							return;
						}
						goto IL_0912;
					}
					goto IL_09ff;
					IL_04b6:
					((TaskAwaiter)(ref val11)).GetResult();
					goto IL_04bd;
					IL_04bd:
					<cardPlay>5__9 = new CardPlay
					{
						Card = cardModel,
						Target = target,
						ResultPile = <resultPileType>5__3,
						Resources = resources,
						IsAutoPlay = isAutoPlay,
						PlayIndex = <i>5__8,
						PlayCount = <playCount>5__5
					};
					val10 = Hook.BeforeCardPlayed(<combatState>5__2, <cardPlay>5__9).GetAwaiter();
					if (!((TaskAwaiter)(ref val10)).IsCompleted)
					{
						num = (<>1__state = 8);
						<>u__1 = val10;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnPlayWrapper>d__327>(ref val10, ref this);
						return;
					}
					goto IL_0579;
					IL_07b8:
					((TaskAwaiter)(ref val6)).GetResult();
					goto IL_07bf;
				}
				((TaskAwaiter)(ref val)).GetResult();
				if (cardModel.EnergyCost.AfterCardPlayedCleanup())
				{
					Action? obj = cardModel.EnergyCostChanged;
					if (obj != null)
					{
						obj.Invoke();
					}
				}
				if (cardModel._temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>)((TemporaryCardCost c) => c.ClearsWhenCardIsPlayed)) > 0)
				{
					Action? obj2 = cardModel.StarCostChanged;
					if (obj2 != null)
					{
						obj2.Invoke();
					}
				}
				cardModel.CurrentTarget = null;
				Action? obj3 = cardModel.Played;
				if (obj3 != null)
				{
					obj3.Invoke();
				}
				choiceContext.PopModel(cardModel);
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				<combatState>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
				return;
			}
			<>1__state = -2;
			<combatState>5__2 = null;
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <PlayPowerCardFlyVfx>d__328 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public CardModel <>4__this;

		private NCard <node>5__2;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel card = <>4__this;
			try
			{
				TaskAwaiter val;
				TaskAwaiter val3;
				if (num != 0)
				{
					if (num == 1)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0279;
					}
					<node>5__2 = NCard.FindOnTable(card);
					bool flag = false;
					if (<node>5__2 != null)
					{
						System.Collections.Generic.IEnumerator<NCardFlyPowerVfx> enumerator = Enumerable.OfType<NCardFlyPowerVfx>((System.Collections.IEnumerable)((Node)NCombatRoom.Instance.CombatVfxContainer).GetChildren(false)).GetEnumerator();
						try
						{
							while (((System.Collections.IEnumerator)enumerator).MoveNext())
							{
								NCardFlyPowerVfx current = enumerator.Current;
								if (current.CardNode == <node>5__2)
								{
									flag = true;
									break;
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((System.IDisposable)enumerator)?.Dispose();
							}
						}
					}
					if (!(<node>5__2 == null || flag))
					{
						goto IL_01cc;
					}
					<node>5__2 = NCard.Create(card);
					if (<node>5__2 != null)
					{
						Tween val2 = ((Node)<node>5__2).CreateTween();
						val2.Parallel().TweenProperty((GodotObject)(object)<node>5__2, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One * 1f), 0.10000000149011612).From(Variant.op_Implicit(Vector2.Zero))
							.SetEase((EaseType)1)
							.SetTrans((TransitionType)7);
						((Node)(object)NCombatRoom.Instance?.CombatVfxContainer).AddChildSafely((Node?)(object)<node>5__2);
						((Control)<node>5__2).GlobalPosition = PileType.Play.GetTargetPosition(<node>5__2);
						<node>5__2.UpdateVisuals(PileType.Play, CardPreviewMode.Normal);
					}
					val3 = Cmd.CustomScaledWait(0.1f, 0.8f).GetAwaiter();
					if (!((TaskAwaiter)(ref val3)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val3;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <PlayPowerCardFlyVfx>d__328>(ref val3, ref this);
						return;
					}
				}
				else
				{
					val3 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
				}
				((TaskAwaiter)(ref val3)).GetResult();
				goto IL_01cc;
				IL_0279:
				((TaskAwaiter)(ref val)).GetResult();
				goto end_IL_000e;
				IL_01cc:
				if (<node>5__2 != null)
				{
					NCardFlyPowerVfx nCardFlyPowerVfx = NCardFlyPowerVfx.Create(<node>5__2);
					((Node)(object)NCombatRoom.Instance?.CombatVfxContainer).AddChildSafely((Node?)(object)nCardFlyPowerVfx);
					TaskHelper.RunSafely(nCardFlyPowerVfx.PlayAnim());
					float duration = nCardFlyPowerVfx.GetDuration();
					val = Cmd.CustomScaledWait(duration * 0.2f, duration).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 1);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <PlayPowerCardFlyVfx>d__328>(ref val, ref this);
						return;
					}
					goto IL_0279;
				}
				end_IL_000e:;
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				<node>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
				return;
			}
			<>1__state = -2;
			<node>5__2 = null;
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <SpendEnergy>d__325 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public CardModel <>4__this;

		public int amount;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel cardModel = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num != 0)
				{
					if (!cardModel.IsDupe && cardModel.EnergyCost.CostsX)
					{
						cardModel.EnergyCost.CapturedXValue = amount;
					}
					if (amount > 0)
					{
						CombatManager.Instance.History.EnergySpent(cardModel.CombatState, amount, cardModel.Owner);
						cardModel.Owner.PlayerCombatState.LoseEnergy(decimal.op_Implicit(Math.Max(0, amount)));
					}
					val = Hook.AfterEnergySpent(cardModel.CombatState, cardModel, amount).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <SpendEnergy>d__325>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
				}
				((TaskAwaiter)(ref val)).GetResult();
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
				return;
			}
			<>1__state = -2;
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <SpendResources>d__324 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder<ValueTuple<int, int>> <>t__builder;

		public CardModel <>4__this;

		private int <energyToSpend>5__2;

		private int <starsToSpend>5__3;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel cardModel = <>4__this;
			ValueTuple<int, int> result;
			try
			{
				TaskAwaiter val;
				TaskAwaiter val2;
				if (num != 0)
				{
					if (num == 1)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0147;
					}
					int energy = cardModel.Owner.PlayerCombatState.Energy;
					<energyToSpend>5__2 = cardModel.EnergyCost.GetAmountToSpend();
					<starsToSpend>5__3 = Math.Max(0, cardModel.GetStarCostWithModifiers());
					if (<energyToSpend>5__2 > energy && Hook.ShouldPayExcessEnergyCostWithStars(cardModel.CombatState, cardModel.Owner))
					{
						<starsToSpend>5__3 += (<energyToSpend>5__2 - energy) * 2;
						<energyToSpend>5__2 = energy;
					}
					val2 = cardModel.SpendEnergy(<energyToSpend>5__2).GetAwaiter();
					if (!((TaskAwaiter)(ref val2)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val2;
						<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SpendResources>d__324>(ref val2, ref this);
						return;
					}
				}
				else
				{
					val2 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
				}
				((TaskAwaiter)(ref val2)).GetResult();
				val = cardModel.SpendStars(<starsToSpend>5__3).GetAwaiter();
				if (!((TaskAwaiter)(ref val)).IsCompleted)
				{
					num = (<>1__state = 1);
					<>u__1 = val;
					<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SpendResources>d__324>(ref val, ref this);
					return;
				}
				goto IL_0147;
				IL_0147:
				((TaskAwaiter)(ref val)).GetResult();
				result = new ValueTuple<int, int>(<energyToSpend>5__2, <starsToSpend>5__3);
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				<>t__builder.SetException(exception);
				return;
			}
			<>1__state = -2;
			<>t__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			<>t__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <SpendStars>d__326 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public CardModel <>4__this;

		public int amount;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			CardModel cardModel = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_00bb;
				}
				if (!cardModel.IsDupe)
				{
					cardModel.LastStarsSpent = amount;
				}
				if (amount > 0)
				{
					cardModel.Owner.PlayerCombatState.LoseStars(decimal.op_Implicit(amount));
					val = Hook.AfterStarsSpent(cardModel.Owner.Creature.CombatState, amount, cardModel.Owner).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <SpendStars>d__326>(ref val, ref this);
						return;
					}
					goto IL_00bb;
				}
				goto end_IL_000e;
				IL_00bb:
				((TaskAwaiter)(ref val)).GetResult();
				end_IL_000e:;
			}
			catch (System.Exception exception)
			{
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
				return;
			}
			<>1__state = -2;
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
		}
	}

	[CompilerGenerated]
	private Action? m_AfflictionChanged;

	[CompilerGenerated]
	private Action? m_EnchantmentChanged;

	[CompilerGenerated]
	private Action? m_EnergyCostChanged;

	[CompilerGenerated]
	private Action? m_KeywordsChanged;

	[CompilerGenerated]
	private Action? m_ReplayCountChanged;

	[CompilerGenerated]
	private Action? m_Played;

	[CompilerGenerated]
	private Action? m_Drawn;

	[CompilerGenerated]
	private Action? m_StarCostChanged;

	[CompilerGenerated]
	private Action? m_Upgraded;

	[CompilerGenerated]
	private Action? m_Forged;

	private LocString? _titleLocString;

	private CardPoolModel? _pool;

	private Player? _owner;

	private CardEnergyCost? _energyCost;

	private int _baseReplayCount;

	private bool _starCostSet;

	private int _baseStarCost;

	private bool _wasStarCostJustUpgraded;

	private List<TemporaryCardCost> _temporaryStarCosts = new List<TemporaryCardCost>();

	private int _lastStarsSpent;

	private HashSet<CardKeyword>? _keywords;

	private HashSet<CardTag>? _tags;

	private DynamicVarSet? _dynamicVars;

	private bool _exhaustOnNextPlay;

	private bool _hasSingleTurnRetain;

	private bool _hasSingleTurnSly;

	private CardModel? _cloneOf;

	private bool _isDupe;

	private int _currentUpgradeLevel;

	private CardUpgradePreviewType _upgradePreviewType;

	private bool _isEnchantmentPreview;

	private int? _floorAddedToDeck;

	private Creature? _currentTarget;

	private CardModel? _deckVersion;

	private CardModel? _canonicalInstance;

	public LocString TitleLocString => _titleLocString ?? (_titleLocString = new LocString("cards", base.Id.Entry + ".title"));

	public string Title
	{
		get
		{
			LocString titleLocString = TitleLocString;
			if (!IsUpgraded)
			{
				return titleLocString.GetFormattedText();
			}
			if (MaxUpgradeLevel > 1)
			{
				return $"{titleLocString.GetFormattedText()}+{CurrentUpgradeLevel}";
			}
			return titleLocString.GetFormattedText() + "+";
		}
	}

	public LocString Description => new LocString("cards", base.Id.Entry + ".description");

	protected LocString SelectionScreenPrompt
	{
		get
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			LocString locString = new LocString("cards", base.Id.Entry + ".selectionScreenPrompt");
			if (!locString.Exists())
			{
				throw new InvalidOperationException($"No selection screen prompt for {base.Id}.");
			}
			DynamicVars.AddTo(locString);
			return locString;
		}
	}

	public virtual string PortraitPath => ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{Pool.Title.ToLowerInvariant()}/{base.Id.Entry.ToLowerInvariant()}.tres");

	public virtual string BetaPortraitPath => ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{Pool.Title.ToLowerInvariant()}/beta/{base.Id.Entry.ToLowerInvariant()}.tres");

	public static string MissingPortraitPath => ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");

	private string PortraitPngPath => ImageHelper.GetImagePath($"packed/card_portraits/{Pool.Title.ToLowerInvariant()}/{base.Id.Entry.ToLowerInvariant()}.png");

	private string BetaPortraitPngPath => ImageHelper.GetImagePath($"packed/card_portraits/{Pool.Title.ToLowerInvariant()}/beta/{base.Id.Entry.ToLowerInvariant()}.png");

	public bool HasPortrait => ResourceLoader.Exists(PortraitPngPath, "");

	public bool HasBetaPortrait => ResourceLoader.Exists(BetaPortraitPngPath, "");

	public Texture2D Portrait => ResourceLoader.Load<Texture2D>(PortraitPath, (string)null, (CacheMode)1);

	private string FramePath
	{
		get
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			CardType cardType;
			switch (Type)
			{
			case CardType.None:
			case CardType.Status:
			case CardType.Curse:
				cardType = CardType.Skill;
				break;
			case CardType.Attack:
			case CardType.Skill:
			case CardType.Power:
			case CardType.Quest:
				cardType = Type;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (Rarity != CardRarity.Ancient)
			{
				return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_frame_" + ((object)cardType/*cast due to .constrained prefix*/).ToString().ToLowerInvariant() + "_s.tres");
			}
			return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
		}
	}

	public Texture2D Frame => ResourceLoader.Load<Texture2D>(FramePath, (string)null, (CacheMode)1);

	private string PortraitBorderPath
	{
		get
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			CardType cardType;
			switch (Type)
			{
			case CardType.None:
			case CardType.Status:
			case CardType.Curse:
			case CardType.Quest:
				cardType = CardType.Skill;
				break;
			case CardType.Attack:
			case CardType.Skill:
			case CardType.Power:
				cardType = Type;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_portrait_border_" + ((object)cardType/*cast due to .constrained prefix*/).ToString().ToLowerInvariant() + "_s.tres");
		}
	}

	private string AncientTextBgPath
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (Rarity != CardRarity.Ancient)
			{
				throw new InvalidOperationException("This card is not an ancient card.");
			}
			CardType cardType;
			switch (Type)
			{
			case CardType.None:
			case CardType.Status:
			case CardType.Curse:
				cardType = CardType.Skill;
				break;
			case CardType.Attack:
			case CardType.Skill:
			case CardType.Power:
			case CardType.Quest:
				cardType = Type;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return ImageHelper.GetImagePath("atlases/compressed.sprites/card_template/ancient_card_text_bg_" + ((object)cardType/*cast due to .constrained prefix*/).ToString().ToLowerInvariant() + ".tres");
		}
	}

	public Texture2D AncientTextBg => ResourceLoader.Load<Texture2D>(AncientTextBgPath, (string)null, (CacheMode)1);

	public Texture2D PortraitBorder => ResourceLoader.Load<Texture2D>(PortraitBorderPath, (string)null, (CacheMode)1);

	private string EnergyIconPath => VisualCardPool.EnergyIconPath;

	public Texture2D EnergyIcon => ResourceLoader.Load<Texture2D>(EnergyIconPath, (string)null, (CacheMode)1);

	protected IHoverTip EnergyHoverTip => HoverTipFactory.ForEnergy(this);

	private string BannerTexturePath
	{
		get
		{
			if (Rarity != CardRarity.Ancient)
			{
				return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner.tres");
			}
			return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner_ancient_s.tres");
		}
	}

	public Texture2D BannerTexture => ResourceLoader.Load<Texture2D>(BannerTexturePath, (string)null, (CacheMode)1);

	private string BannerMaterialPath => Rarity switch
	{
		CardRarity.Uncommon => "res://materials/cards/banners/card_banner_uncommon_mat.tres", 
		CardRarity.Rare => "res://materials/cards/banners/card_banner_rare_mat.tres", 
		CardRarity.Curse => "res://materials/cards/banners/card_banner_curse_mat.tres", 
		CardRarity.Status => "res://materials/cards/banners/card_banner_status_mat.tres", 
		CardRarity.Event => "res://materials/cards/banners/card_banner_event_mat.tres", 
		CardRarity.Quest => "res://materials/cards/banners/card_banner_quest_mat.tres", 
		CardRarity.Ancient => "res://materials/cards/banners/card_banner_ancient_mat.tres", 
		_ => "res://materials/cards/banners/card_banner_common_mat.tres", 
	};

	public Material BannerMaterial => PreloadManager.Cache.GetMaterial(BannerMaterialPath);

	public Material FrameMaterial => VisualCardPool.FrameMaterial;

	[field: CompilerGenerated]
	public virtual CardType Type
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public virtual CardRarity Rarity
	{
		[CompilerGenerated]
		get;
	}

	public virtual CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.None;

	public virtual CardPoolModel Pool
	{
		get
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			if (_pool != null)
			{
				return _pool;
			}
			_pool = Enumerable.FirstOrDefault<CardPoolModel>(ModelDb.AllCardPools, (Func<CardPoolModel, bool>)([CompilerGenerated] (CardPoolModel pool) => Enumerable.Contains<ModelId>(pool.AllCardIds, base.Id)));
			if (_pool != null)
			{
				return _pool;
			}
			if (Enumerable.Contains<ModelId>(ModelDb.CardPool<MockCardPool>().AllCardIds, base.Id))
			{
				_pool = ModelDb.CardPool<MockCardPool>();
				return _pool;
			}
			throw new InvalidProgramException($"Card {this} is not in any card pool!");
		}
	}

	public virtual CardPoolModel VisualCardPool => Pool;

	public Player Owner
	{
		get
		{
			AssertMutable();
			return _owner;
		}
		set
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			AssertMutable();
			if (_owner != null && value != null)
			{
				throw new InvalidOperationException("Card " + base.Id.Entry + " already has an owner.");
			}
			_owner = value;
		}
	}

	public CardPile? Pile
	{
		get
		{
			Player? owner = _owner;
			if (owner == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<CardPile>(owner.Piles, (Func<CardPile, bool>)([CompilerGenerated] (CardPile p) => Enumerable.Contains<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)p.Cards, this)));
		}
	}

	[field: CompilerGenerated]
	protected virtual int CanonicalEnergyCost
	{
		[CompilerGenerated]
		get;
	}

	protected virtual bool HasEnergyCostX => false;

	public CardEnergyCost EnergyCost
	{
		get
		{
			if (_energyCost == null)
			{
				_energyCost = new CardEnergyCost(this, CanonicalEnergyCost, HasEnergyCostX);
			}
			return _energyCost;
		}
	}

	public int BaseReplayCount
	{
		get
		{
			return _baseReplayCount;
		}
		set
		{
			AssertMutable();
			_baseReplayCount = value;
			Action? obj = this.ReplayCountChanged;
			if (obj != null)
			{
				obj.Invoke();
			}
		}
	}

	public virtual int CanonicalStarCost => -1;

	public int BaseStarCost
	{
		get
		{
			if (!base.IsMutable)
			{
				return CanonicalStarCost;
			}
			if (!_starCostSet)
			{
				_baseStarCost = CanonicalStarCost;
				_starCostSet = true;
			}
			return _baseStarCost;
		}
		private set
		{
			AssertMutable();
			if (!HasStarCostX)
			{
				_baseStarCost = value;
				_starCostSet = true;
			}
			Action? obj = this.StarCostChanged;
			if (obj != null)
			{
				obj.Invoke();
			}
		}
	}

	public bool WasStarCostJustUpgraded => _wasStarCostJustUpgraded;

	public TemporaryCardCost? TemporaryStarCost => Enumerable.LastOrDefault<TemporaryCardCost>((System.Collections.Generic.IEnumerable<TemporaryCardCost>)_temporaryStarCosts);

	public virtual int CurrentStarCost
	{
		get
		{
			int? num = Enumerable.LastOrDefault<TemporaryCardCost>((System.Collections.Generic.IEnumerable<TemporaryCardCost>)_temporaryStarCosts)?.Cost;
			if (num.HasValue)
			{
				if (num == 0 && BaseStarCost < 0)
				{
					return BaseStarCost;
				}
				return num.Value;
			}
			return BaseStarCost;
		}
	}

	public virtual bool HasStarCostX => false;

	public int LastStarsSpent
	{
		get
		{
			return _lastStarsSpent;
		}
		set
		{
			AssertMutable();
			_lastStarsSpent = value;
		}
	}

	[field: CompilerGenerated]
	public virtual TargetType TargetType
	{
		[CompilerGenerated]
		get;
	}

	public virtual System.Collections.Generic.IEnumerable<CardKeyword> CanonicalKeywords => System.Array.Empty<CardKeyword>();

	public IReadOnlySet<CardKeyword> Keywords
	{
		get
		{
			if (_keywords != null)
			{
				return (IReadOnlySet<CardKeyword>)(object)_keywords;
			}
			_keywords = new HashSet<CardKeyword>();
			_keywords.UnionWith(CanonicalKeywords);
			return (IReadOnlySet<CardKeyword>)(object)_keywords;
		}
	}

	public virtual System.Collections.Generic.IEnumerable<CardTag> Tags => (System.Collections.Generic.IEnumerable<CardTag>)(_tags ?? (_tags = CanonicalTags));

	protected virtual HashSet<CardTag> CanonicalTags => new HashSet<CardTag>();

	public DynamicVarSet DynamicVars
	{
		get
		{
			if (_dynamicVars != null)
			{
				return _dynamicVars;
			}
			_dynamicVars = new DynamicVarSet(CanonicalVars);
			_dynamicVars.InitializeWithOwner(this);
			return _dynamicVars;
		}
	}

	protected virtual System.Collections.Generic.IEnumerable<DynamicVar> CanonicalVars => System.Array.Empty<DynamicVar>();

	public bool ExhaustOnNextPlay
	{
		get
		{
			return _exhaustOnNextPlay;
		}
		set
		{
			AssertMutable();
			_exhaustOnNextPlay = value;
		}
	}

	private bool HasSingleTurnRetain
	{
		get
		{
			return _hasSingleTurnRetain;
		}
		set
		{
			AssertMutable();
			_hasSingleTurnRetain = value;
		}
	}

	public bool ShouldRetainThisTurn
	{
		get
		{
			if (!Keywords.Contains(CardKeyword.Retain))
			{
				return HasSingleTurnRetain;
			}
			return true;
		}
	}

	private bool HasSingleTurnSly
	{
		get
		{
			return _hasSingleTurnSly;
		}
		set
		{
			AssertMutable();
			_hasSingleTurnSly = value;
		}
	}

	public bool IsSlyThisTurn
	{
		get
		{
			if (!Keywords.Contains(CardKeyword.Sly))
			{
				return HasSingleTurnSly;
			}
			return true;
		}
	}

	[field: CompilerGenerated]
	public EnchantmentModel? Enchantment
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public AfflictionModel? Affliction
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public virtual bool CanBeGeneratedInCombat => true;

	public virtual bool CanBeGeneratedByModifiers => true;

	public virtual OrbEvokeType OrbEvokeType => OrbEvokeType.None;

	public virtual bool GainsBlock => false;

	public virtual bool IsBasicStrikeOrDefend
	{
		get
		{
			if (Rarity != CardRarity.Basic)
			{
				return false;
			}
			if (Enumerable.Contains<CardTag>(Tags, CardTag.Strike))
			{
				return true;
			}
			if (Enumerable.Contains<CardTag>(Tags, CardTag.Defend))
			{
				return true;
			}
			return false;
		}
	}

	public CardModel? CloneOf => _cloneOf;

	public bool IsClone => CloneOf != null;

	public CardModel? DupeOf
	{
		get
		{
			if (!IsDupe)
			{
				return null;
			}
			return CloneOf;
		}
	}

	public bool IsDupe
	{
		get
		{
			return _isDupe;
		}
		private set
		{
			AssertMutable();
			_isDupe = value;
		}
	}

	public bool IsRemovable => !Keywords.Contains(CardKeyword.Eternal);

	public bool IsTransformable
	{
		get
		{
			if (!IsRemovable)
			{
				CardPile pile = Pile;
				return pile == null || pile.Type != PileType.Deck;
			}
			return true;
		}
	}

	public bool IsInCombat
	{
		get
		{
			if (base.IsMutable)
			{
				return Pile?.IsCombatPile ?? false;
			}
			return false;
		}
	}

	public int CurrentUpgradeLevel
	{
		get
		{
			return _currentUpgradeLevel;
		}
		private set
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			AssertMutable();
			if (value > MaxUpgradeLevel)
			{
				throw new InvalidOperationException($"{base.Id} cannot be upgraded past its MaxUpgradeLevel.");
			}
			_currentUpgradeLevel = value;
		}
	}

	public virtual int MaxUpgradeLevel => 1;

	public bool IsUpgraded => CurrentUpgradeLevel > 0;

	public bool IsUpgradable
	{
		get
		{
			if (CurrentUpgradeLevel >= MaxUpgradeLevel)
			{
				return false;
			}
			return true;
		}
	}

	public CardUpgradePreviewType UpgradePreviewType
	{
		get
		{
			return _upgradePreviewType;
		}
		set
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			AssertMutable();
			if (!value.IsPreview() && _upgradePreviewType.IsPreview())
			{
				throw new InvalidOperationException("A card cannot go to from being upgrade preview. Consider making a new card model instead.");
			}
			_upgradePreviewType = value;
		}
	}

	protected virtual bool IsPlayable => true;

	[field: CompilerGenerated]
	public bool ShouldShowInCardLibrary
	{
		[CompilerGenerated]
		get;
	}

	public bool ShouldGlowGold
	{
		get
		{
			if (!ShouldGlowGoldInternal)
			{
				return Enchantment?.ShouldGlowGold ?? false;
			}
			return true;
		}
	}

	public bool ShouldGlowRed
	{
		get
		{
			if (!ShouldGlowRedInternal)
			{
				return Enchantment?.ShouldGlowRed ?? false;
			}
			return true;
		}
	}

	protected virtual bool ShouldGlowGoldInternal => false;

	protected virtual bool ShouldGlowRedInternal => false;

	public bool IsEnchantmentPreview
	{
		get
		{
			return _isEnchantmentPreview;
		}
		set
		{
			AssertMutable();
			_isEnchantmentPreview = value;
		}
	}

	public virtual bool HasBuiltInOverlay => false;

	public string OverlayPath => SceneHelper.GetScenePath("cards/overlays/" + base.Id.Entry.ToLowerInvariant());

	public int? FloorAddedToDeck
	{
		get
		{
			return _floorAddedToDeck;
		}
		set
		{
			AssertMutable();
			_floorAddedToDeck = value;
		}
	}

	public Creature? CurrentTarget
	{
		get
		{
			return _currentTarget;
		}
		private set
		{
			AssertMutable();
			_currentTarget = value;
		}
	}

	public CardModel? DeckVersion
	{
		get
		{
			return _deckVersion;
		}
		set
		{
			AssertMutable();
			_deckVersion = value;
		}
	}

	[field: CompilerGenerated]
	public bool HasBeenRemovedFromState
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	protected virtual System.Collections.Generic.IEnumerable<IHoverTip> ExtraHoverTips => System.Array.Empty<IHoverTip>();

	public System.Collections.Generic.IEnumerable<IHoverTip> HoverTips
	{
		get
		{
			List<IHoverTip> val = Enumerable.ToList<IHoverTip>(ExtraHoverTips);
			if (Enchantment != null)
			{
				val.AddRange(Enchantment.HoverTips);
			}
			if (Affliction != null)
			{
				val.AddRange(Affliction.HoverTips);
			}
			int enchantedReplayCount = GetEnchantedReplayCount();
			if (enchantedReplayCount > 0)
			{
				val.Add(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, new DynamicVar("Times", decimal.op_Implicit(enchantedReplayCount))));
			}
			if (OrbEvokeType != OrbEvokeType.None)
			{
				val.Add(HoverTipFactory.Static(StaticHoverTip.Evoke));
			}
			if (GainsBlock)
			{
				val.Add(HoverTipFactory.Static(StaticHoverTip.Block));
			}
			System.Collections.Generic.IEnumerator<CardKeyword> enumerator = ((System.Collections.Generic.IEnumerable<CardKeyword>)Keywords).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					CardKeyword current = enumerator.Current;
					val.Add(HoverTipFactory.FromKeyword(current));
					if (current == CardKeyword.Ethereal)
					{
						val.Add(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			return Enumerable.Distinct<IHoverTip>((System.Collections.Generic.IEnumerable<IHoverTip>)val);
		}
	}

	public CardModel CanonicalInstance
	{
		get
		{
			if (!base.IsMutable)
			{
				return this;
			}
			return _canonicalInstance;
		}
		private set
		{
			AssertMutable();
			_canonicalInstance = value;
		}
	}

	public IRunState? RunState => _owner?.RunState;

	public CombatState? CombatState
	{
		get
		{
			CardPile pile = Pile;
			if ((pile != null && pile.IsCombatPile) || UpgradePreviewType == CardUpgradePreviewType.Combat)
			{
				return _owner?.Creature.CombatState;
			}
			return null;
		}
	}

	public ICardScope? CardScope
	{
		get
		{
			ICardScope combatState = CombatState;
			object obj = combatState;
			if (obj == null)
			{
				combatState = _owner?.Creature.CombatState;
				obj = combatState ?? RunState;
			}
			return (ICardScope?)obj;
		}
	}

	public virtual bool HasTurnEndInHandEffect => false;

	public override bool ShouldReceiveCombatHooks => Pile?.IsCombatPile ?? false;

	public virtual System.Collections.Generic.IEnumerable<string> AllPortraitPaths => new <>z__ReadOnlySingleElementList<string>(PortraitPath);

	public System.Collections.Generic.IEnumerable<string> RunAssetPaths => ExtraRunAssetPaths;

	protected virtual System.Collections.Generic.IEnumerable<string> ExtraRunAssetPaths => System.Array.Empty<string>();

	public event Action AfflictionChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_AfflictionChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_AfflictionChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_AfflictionChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_AfflictionChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action EnchantmentChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_EnchantmentChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_EnchantmentChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_EnchantmentChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_EnchantmentChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action EnergyCostChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_EnergyCostChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_EnergyCostChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_EnergyCostChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_EnergyCostChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action KeywordsChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_KeywordsChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_KeywordsChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_KeywordsChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_KeywordsChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action ReplayCountChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_ReplayCountChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_ReplayCountChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_ReplayCountChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_ReplayCountChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action Played
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Played;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Played, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Played;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Played, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action Drawn
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Drawn;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Drawn, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Drawn;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Drawn, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action StarCostChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_StarCostChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_StarCostChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_StarCostChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_StarCostChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action Upgraded
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Upgraded;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Upgraded, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Upgraded;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Upgraded, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action Forged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Forged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Forged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_Forged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_Forged, val3, val2);
			}
			while (val != val2);
		}
	}

	protected CardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true)
	{
		CanonicalEnergyCost = canonicalEnergyCost;
		Type = type;
		Rarity = rarity;
		TargetType = targetType;
		ShouldShowInCardLibrary = shouldShowInCardLibrary;
	}

	protected void MockSetEnergyCost(CardEnergyCost cost)
	{
		_energyCost = cost;
	}

	public void InvokeEnergyCostChanged()
	{
		Action? obj = this.EnergyCostChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public int ResolveEnergyXValue()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (!EnergyCost.CostsX)
		{
			throw new InvalidOperationException("This card does not have an X-cost.");
		}
		return Hook.ModifyXValue(CombatState, this, EnergyCost.CapturedXValue);
	}

	public int GetEnchantedReplayCount()
	{
		return Enchantment?.EnchantPlayCount(BaseReplayCount) ?? BaseReplayCount;
	}

	public int ResolveStarXValue()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!HasStarCostX)
		{
			throw new InvalidOperationException("This card does not have an X-cost.");
		}
		return Hook.ModifyXValue(CombatState, this, LastStarsSpent);
	}

	public Control CreateOverlay()
	{
		return PreloadManager.Cache.GetScene(OverlayPath).Instantiate<Control>((GenEditState)0);
	}

	public CardModel ToMutable()
	{
		AssertCanonical();
		return (CardModel)MutableClone();
	}

	protected override void DeepCloneFields()
	{
		HashSet<CardKeyword> val = new HashSet<CardKeyword>();
		System.Collections.Generic.IEnumerator<CardKeyword> enumerator = ((System.Collections.Generic.IEnumerable<CardKeyword>)Keywords).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardKeyword current = enumerator.Current;
				val.Add(current);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
		_keywords = val;
		_dynamicVars = DynamicVars.Clone(this);
		_energyCost = _energyCost?.Clone(this);
		_temporaryStarCosts = Enumerable.ToList<TemporaryCardCost>((System.Collections.Generic.IEnumerable<TemporaryCardCost>)_temporaryStarCosts);
		if (Enchantment != null)
		{
			EnchantmentModel enchantmentModel = (EnchantmentModel)Enchantment.ClonePreservingMutability();
			Enchantment = null;
			EnchantInternal(enchantmentModel, decimal.op_Implicit(enchantmentModel.Amount));
		}
		if (Affliction != null)
		{
			AfflictionModel afflictionModel = (AfflictionModel)Affliction.ClonePreservingMutability();
			Affliction = null;
			AfflictInternal(afflictionModel, decimal.op_Implicit(afflictionModel.Amount));
		}
	}

	protected override void AfterCloned()
	{
		base.AfterCloned();
		if (_canonicalInstance == null)
		{
			_canonicalInstance = ModelDb.GetById<CardModel>(base.Id);
		}
		CurrentTarget = null;
		DeckVersion = null;
		HasBeenRemovedFromState = false;
		this.AfflictionChanged = null;
		this.Drawn = null;
		this.EnchantmentChanged = null;
		this.EnergyCostChanged = null;
		this.Forged = null;
		this.KeywordsChanged = null;
		this.Played = null;
		this.ReplayCountChanged = null;
		this.StarCostChanged = null;
		this.Upgraded = null;
	}

	public virtual void AfterCreated()
	{
	}

	protected virtual void AfterDeserialized()
	{
	}

	protected void NeverEverCallThisOutsideOfTests_ClearOwner()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (TestMode.IsOff)
		{
			throw new InvalidOperationException("You monster!");
		}
		_owner = null;
	}

	public void SetToFreeThisTurn()
	{
		EnergyCost.SetThisTurnOrUntilPlayed(0);
		SetStarCostThisTurn(0);
	}

	public void SetToFreeThisCombat()
	{
		EnergyCost.SetThisCombat(0);
		SetStarCostThisCombat(0);
	}

	public void SetStarCostUntilPlayed(int cost)
	{
		AddTemporaryStarCost(TemporaryCardCost.UntilPlayed(cost));
	}

	public void SetStarCostThisTurn(int cost)
	{
		AddTemporaryStarCost(TemporaryCardCost.ThisTurn(cost));
	}

	public void SetStarCostThisCombat(int cost)
	{
		AddTemporaryStarCost(TemporaryCardCost.ThisCombat(cost));
	}

	public int GetStarCostThisCombat()
	{
		return Enumerable.FirstOrDefault<TemporaryCardCost>((System.Collections.Generic.IEnumerable<TemporaryCardCost>)_temporaryStarCosts, (Func<TemporaryCardCost, bool>)((TemporaryCardCost cost) => cost != null && !cost.ClearsWhenTurnEnds && !cost.ClearsWhenCardIsPlayed))?.Cost ?? BaseStarCost;
	}

	private void AddTemporaryStarCost(TemporaryCardCost cost)
	{
		AssertMutable();
		_temporaryStarCosts.Add(cost);
		Action? obj = this.StarCostChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	protected void UpgradeStarCostBy(int addend)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (HasStarCostX)
		{
			throw new InvalidOperationException("UpgradeStarCostBy called on " + base.Id.Entry + " which has star cost X.");
		}
		if (addend == 0)
		{
			return;
		}
		int baseStarCost = BaseStarCost;
		BaseStarCost += addend;
		_wasStarCostJustUpgraded = true;
		if (BaseStarCost < baseStarCost)
		{
			_temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>)([CompilerGenerated] (TemporaryCardCost c) => c.Cost > BaseStarCost));
		}
	}

	public void AddKeyword(CardKeyword keyword)
	{
		AssertMutable();
		_keywords.Add(keyword);
		Action? obj = this.KeywordsChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public void RemoveKeyword(CardKeyword keyword)
	{
		AssertMutable();
		_keywords.Remove(keyword);
		Action? obj = this.KeywordsChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public void GiveSingleTurnRetain()
	{
		HasSingleTurnRetain = true;
	}

	public void GiveSingleTurnSly()
	{
		HasSingleTurnSly = true;
	}

	public string GetDescriptionForPile(PileType pileType, Creature? target = null)
	{
		return GetDescriptionForPile(pileType, DescriptionPreviewType.None, target);
	}

	public string GetDescriptionForUpgradePreview()
	{
		return GetDescriptionForPile(PileType.None, DescriptionPreviewType.Upgrade);
	}

	private string GetDescriptionForPile(PileType pileType, DescriptionPreviewType previewType, Creature? target = null)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		LocString description = Description;
		DynamicVars.AddTo(description);
		AddExtraArgsToDescription(description);
		UpgradeDisplay upgradeDisplay = ((previewType == DescriptionPreviewType.Upgrade) ? UpgradeDisplay.UpgradePreview : (IsUpgraded ? UpgradeDisplay.Upgraded : UpgradeDisplay.Normal));
		description.Add(new IfUpgradedVar(upgradeDisplay));
		bool flag = ((pileType == PileType.Hand || pileType == PileType.Play) ? true : false);
		bool variable = flag;
		description.Add("OnTable", variable);
		bool variable2 = CombatManager.Instance.IsInProgress && (Pile?.IsCombatPile ?? pileType.IsCombatPile());
		description.Add("InCombat", variable2);
		description.Add("IsTargeting", target != null);
		string prefix = EnergyIconHelper.GetPrefix(this);
		description.Add("energyPrefix", prefix);
		description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
		System.Collections.Generic.IEnumerator<KeyValuePair<string, object>> enumerator = ((System.Collections.Generic.IEnumerable<KeyValuePair<string, object>>)description.Variables).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				if (enumerator.Current.Value is EnergyVar energyVar)
				{
					energyVar.ColorPrefix = prefix;
				}
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
		int num = 1;
		List<string> obj = new List<string>(num);
		CollectionsMarshal.SetCount<string>(obj, num);
		System.Span<string> span = CollectionsMarshal.AsSpan<string>(obj);
		int num2 = 0;
		span[num2] = description.GetFormattedText();
		List<string> val = obj;
		LocString locString = Enchantment?.DynamicExtraCardText;
		if (locString != null)
		{
			val.Add("[purple]" + locString.GetFormattedText() + "[/purple]");
		}
		LocString locString2 = Affliction?.DynamicExtraCardText;
		if (locString2 != null)
		{
			val.Add("[purple]" + locString2.GetFormattedText() + "[/purple]");
		}
		CardKeyword[] beforeDescription = CardKeywordOrder.beforeDescription;
		foreach (CardKeyword cardKeyword in beforeDescription)
		{
			if (cardKeyword switch
			{
				CardKeyword.Sly => IsSlyThisTurn, 
				CardKeyword.Retain => ShouldRetainThisTurn, 
				_ => Keywords.Contains(cardKeyword), 
			})
			{
				val.Insert(0, cardKeyword.GetCardText());
			}
		}
		int enchantedReplayCount = GetEnchantedReplayCount();
		if (enchantedReplayCount > 0)
		{
			LocString locString3 = new LocString("static_hover_tips", "REPLAY.extraText");
			locString3.Add("Times", decimal.op_Implicit(enchantedReplayCount));
			val.Add(locString3.GetFormattedText() ?? "");
		}
		System.Collections.Generic.IEnumerator<CardKeyword> enumerator2 = Enumerable.Intersect<CardKeyword>((System.Collections.Generic.IEnumerable<CardKeyword>)CardKeywordOrder.afterDescription, (System.Collections.Generic.IEnumerable<CardKeyword>)Keywords).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator2).MoveNext())
			{
				CardKeyword current = enumerator2.Current;
				val.Add(current.GetCardText());
			}
		}
		finally
		{
			((System.IDisposable)enumerator2)?.Dispose();
		}
		return string.Join<string>('\n', Enumerable.Where<string>((System.Collections.Generic.IEnumerable<string>)val, (Func<string, bool>)((string l) => !string.IsNullOrEmpty(l))));
	}

	public void UpdateDynamicVarPreview(CardPreviewMode previewMode, Creature? target, DynamicVarSet dynamicVarSet)
	{
		if (RunState == null && CombatState == null)
		{
			return;
		}
		bool flag = CombatState != null;
		bool flag2 = flag;
		if (flag2)
		{
			bool flag3;
			switch (Pile?.Type)
			{
			case PileType.Hand:
			case PileType.Play:
				flag3 = true;
				break;
			default:
				flag3 = false;
				break;
			}
			flag2 = flag3 || UpgradePreviewType == CardUpgradePreviewType.Combat;
		}
		bool runGlobalHooks = flag2;
		System.Collections.Generic.IEnumerator<DynamicVar> enumerator = dynamicVarSet.Values.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				DynamicVar current = enumerator.Current;
				current.UpdateCardPreview(this, previewMode, target, runGlobalHooks);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	public void EnchantInternal(EnchantmentModel enchantment, decimal amount)
	{
		AssertMutable();
		enchantment.AssertMutable();
		Enchantment = enchantment;
		Enchantment.ApplyInternal(this, amount);
		Action? obj = this.EnchantmentChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public void AfflictInternal(AfflictionModel affliction, decimal amount)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		AssertMutable();
		affliction.AssertMutable();
		if (Affliction != null)
		{
			throw new InvalidOperationException($"Attempted to afflict card {this} that was already afflicted! This is not allowed");
		}
		Affliction = affliction;
		Affliction.Card = this;
		Affliction.Amount = (int)amount;
		Action? obj = this.AfflictionChanged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public void ClearEnchantmentInternal()
	{
		if (Enchantment != null)
		{
			AssertMutable();
			Enchantment.ClearInternal();
			Enchantment = null;
			Action? obj = this.EnchantmentChanged;
			if (obj != null)
			{
				obj.Invoke();
			}
		}
	}

	public void ClearAfflictionInternal()
	{
		AssertMutable();
		if (Affliction != null)
		{
			Affliction.ClearInternal();
			Affliction = null;
			Owner.PlayerCombatState.RecalculateCardValues();
			Action? obj = this.AfflictionChanged;
			if (obj != null)
			{
				obj.Invoke();
			}
		}
	}

	protected virtual void AddExtraArgsToDescription(LocString description)
	{
	}

	public int GetStarCostWithModifiers()
	{
		if (HasStarCostX)
		{
			return Owner.PlayerCombatState?.Stars ?? 0;
		}
		CardPile pile = Pile;
		if (pile != null && pile.IsCombatPile)
		{
			return (int)Hook.ModifyStarCost(CombatState, this, decimal.op_Implicit(CurrentStarCost));
		}
		return CurrentStarCost;
	}

	public bool CostsEnergyOrStars(bool includeGlobalModifiers)
	{
		if (includeGlobalModifiers)
		{
			if (!EnergyCost.CostsX && EnergyCost.GetWithModifiers(CostModifiers.All) > 0)
			{
				return true;
			}
			if (!HasStarCostX && GetStarCostWithModifiers() > 0)
			{
				return true;
			}
		}
		else if (EnergyCost.GetWithModifiers(CostModifiers.Local) > 0 || CurrentStarCost > 0)
		{
			return true;
		}
		return false;
	}

	public void RemoveFromCurrentPile()
	{
		AssertMutable();
		Pile?.RemoveInternal(this);
	}

	public void RemoveFromState()
	{
		RemoveFromCurrentPile();
		HasBeenRemovedFromState = true;
	}

	public void EndOfTurnCleanup()
	{
		ExhaustOnNextPlay = false;
		HasSingleTurnRetain = false;
		HasSingleTurnSly = false;
		if (EnergyCost.EndOfTurnCleanup())
		{
			Action? obj = this.EnergyCostChanged;
			if (obj != null)
			{
				obj.Invoke();
			}
		}
		if (_temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>)((TemporaryCardCost c) => c.ClearsWhenTurnEnds)) > 0)
		{
			Action? obj2 = this.StarCostChanged;
			if (obj2 != null)
			{
				obj2.Invoke();
			}
		}
	}

	public virtual void AfterTransformedFrom()
	{
	}

	public virtual void AfterTransformedTo()
	{
	}

	public void AfterForged()
	{
		Action? obj = this.Forged;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	protected virtual System.Threading.Tasks.Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		return System.Threading.Tasks.Task.CompletedTask;
	}

	public virtual System.Threading.Tasks.Task OnEnqueuePlayVfx(Creature? target)
	{
		return System.Threading.Tasks.Task.CompletedTask;
	}

	protected virtual void OnUpgrade()
	{
	}

	public virtual System.Threading.Tasks.Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
	{
		return System.Threading.Tasks.Task.CompletedTask;
	}

	public bool CanPlayTargeting(Creature? target)
	{
		if (!IsValidTarget(target))
		{
			return false;
		}
		return CanPlay();
	}

	public bool CanPlay()
	{
		UnplayableReason reason;
		AbstractModel preventer;
		return CanPlay(out reason, out preventer);
	}

	public bool CanPlay(out UnplayableReason reason, out AbstractModel? preventer)
	{
		reason = UnplayableReason.None;
		CombatState combatState = CombatState ?? _owner?.Creature.CombatState;
		if (combatState == null || Owner.PlayerCombatState == null)
		{
			preventer = null;
			return false;
		}
		if (Keywords.Contains(CardKeyword.Unplayable))
		{
			reason |= UnplayableReason.HasUnplayableKeyword;
		}
		if (!Owner.PlayerCombatState.HasEnoughResourcesFor(this, out var reason2))
		{
			reason |= reason2;
		}
		if (TargetType == TargetType.AnyAlly && Enumerable.Count<Creature>((System.Collections.Generic.IEnumerable<Creature>)combatState.PlayerCreatures, (Func<Creature, bool>)((Creature c) => c.IsAlive)) <= 1)
		{
			reason |= UnplayableReason.NoLivingAllies;
		}
		if (!Hook.ShouldPlay(combatState, this, out preventer, AutoPlayType.None))
		{
			reason |= UnplayableReason.BlockedByHook;
		}
		if (!IsPlayable)
		{
			reason |= UnplayableReason.BlockedByCardLogic;
		}
		return reason == UnplayableReason.None;
	}

	public bool IsValidTarget(Creature? target)
	{
		if (target == null)
		{
			if (TargetType != TargetType.AnyEnemy)
			{
				return TargetType != TargetType.AnyAlly;
			}
			return false;
		}
		if (!target.IsAlive)
		{
			return false;
		}
		if (TargetType == TargetType.AnyEnemy)
		{
			return target.Side != Owner.Creature.Side;
		}
		if (TargetType == TargetType.AnyAlly)
		{
			return target.Side == Owner.Creature.Side;
		}
		return false;
	}

	public bool TryManualPlay(Creature? target)
	{
		if (CanPlayTargeting(target))
		{
			EnqueueManualPlay(target);
			return true;
		}
		return false;
	}

	private void EnqueueManualPlay(Creature? target)
	{
		TaskHelper.RunSafely(OnEnqueuePlayVfx(target));
		RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new PlayCardAction(this, target));
	}

	[AsyncStateMachine(typeof(<SpendResources>d__324))]
	public async System.Threading.Tasks.Task<ValueTuple<int, int>> SpendResources()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		int energy = Owner.PlayerCombatState.Energy;
		int energyToSpend = EnergyCost.GetAmountToSpend();
		int starsToSpend = Math.Max(0, GetStarCostWithModifiers());
		if (energyToSpend > energy && Hook.ShouldPayExcessEnergyCostWithStars(CombatState, Owner))
		{
			starsToSpend += (energyToSpend - energy) * 2;
			energyToSpend = energy;
		}
		await SpendEnergy(energyToSpend);
		await SpendStars(starsToSpend);
		return new ValueTuple<int, int>(energyToSpend, starsToSpend);
	}

	[AsyncStateMachine(typeof(<SpendEnergy>d__325))]
	private System.Threading.Tasks.Task SpendEnergy(int amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<SpendEnergy>d__325 <SpendEnergy>d__ = default(<SpendEnergy>d__325);
		<SpendEnergy>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SpendEnergy>d__.<>4__this = this;
		<SpendEnergy>d__.amount = amount;
		<SpendEnergy>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <SpendEnergy>d__.<>t__builder)).Start<<SpendEnergy>d__325>(ref <SpendEnergy>d__);
		return ((AsyncTaskMethodBuilder)(ref <SpendEnergy>d__.<>t__builder)).Task;
	}

	[AsyncStateMachine(typeof(<SpendStars>d__326))]
	private System.Threading.Tasks.Task SpendStars(int amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<SpendStars>d__326 <SpendStars>d__ = default(<SpendStars>d__326);
		<SpendStars>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SpendStars>d__.<>4__this = this;
		<SpendStars>d__.amount = amount;
		<SpendStars>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <SpendStars>d__.<>t__builder)).Start<<SpendStars>d__326>(ref <SpendStars>d__);
		return ((AsyncTaskMethodBuilder)(ref <SpendStars>d__.<>t__builder)).Task;
	}

	[AsyncStateMachine(typeof(<OnPlayWrapper>d__327))]
	public System.Threading.Tasks.Task OnPlayWrapper(PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<OnPlayWrapper>d__327 <OnPlayWrapper>d__ = default(<OnPlayWrapper>d__327);
		<OnPlayWrapper>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<OnPlayWrapper>d__.<>4__this = this;
		<OnPlayWrapper>d__.choiceContext = choiceContext;
		<OnPlayWrapper>d__.target = target;
		<OnPlayWrapper>d__.isAutoPlay = isAutoPlay;
		<OnPlayWrapper>d__.resources = resources;
		<OnPlayWrapper>d__.skipCardPileVisuals = skipCardPileVisuals;
		<OnPlayWrapper>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <OnPlayWrapper>d__.<>t__builder)).Start<<OnPlayWrapper>d__327>(ref <OnPlayWrapper>d__);
		return ((AsyncTaskMethodBuilder)(ref <OnPlayWrapper>d__.<>t__builder)).Task;
	}

	[AsyncStateMachine(typeof(<PlayPowerCardFlyVfx>d__328))]
	private System.Threading.Tasks.Task PlayPowerCardFlyVfx()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<PlayPowerCardFlyVfx>d__328 <PlayPowerCardFlyVfx>d__ = default(<PlayPowerCardFlyVfx>d__328);
		<PlayPowerCardFlyVfx>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<PlayPowerCardFlyVfx>d__.<>4__this = this;
		<PlayPowerCardFlyVfx>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <PlayPowerCardFlyVfx>d__.<>t__builder)).Start<<PlayPowerCardFlyVfx>d__328>(ref <PlayPowerCardFlyVfx>d__);
		return ((AsyncTaskMethodBuilder)(ref <PlayPowerCardFlyVfx>d__.<>t__builder)).Task;
	}

	protected virtual PileType GetResultPileType()
	{
		if (IsDupe || Type == CardType.Power)
		{
			return PileType.None;
		}
		if (ExhaustOnNextPlay || Keywords.Contains(CardKeyword.Exhaust))
		{
			return PileType.Exhaust;
		}
		return PileType.Discard;
	}

	[AsyncStateMachine(typeof(<MoveToResultPileWithoutPlaying>d__330))]
	public System.Threading.Tasks.Task MoveToResultPileWithoutPlaying(PlayerChoiceContext choiceContext)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<MoveToResultPileWithoutPlaying>d__330 <MoveToResultPileWithoutPlaying>d__ = default(<MoveToResultPileWithoutPlaying>d__330);
		<MoveToResultPileWithoutPlaying>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<MoveToResultPileWithoutPlaying>d__.<>4__this = this;
		<MoveToResultPileWithoutPlaying>d__.choiceContext = choiceContext;
		<MoveToResultPileWithoutPlaying>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <MoveToResultPileWithoutPlaying>d__.<>t__builder)).Start<<MoveToResultPileWithoutPlaying>d__330>(ref <MoveToResultPileWithoutPlaying>d__);
		return ((AsyncTaskMethodBuilder)(ref <MoveToResultPileWithoutPlaying>d__.<>t__builder)).Task;
	}

	public void UpgradeInternal()
	{
		AssertMutable();
		CurrentUpgradeLevel++;
		OnUpgrade();
		DynamicVars.RecalculateForUpgradeOrEnchant();
		Action? obj = this.Upgraded;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public void FinalizeUpgradeInternal()
	{
		DynamicVars.FinalizeUpgrade();
		EnergyCost.FinalizeUpgrade();
		_wasStarCostJustUpgraded = false;
	}

	public void DowngradeInternal()
	{
		AssertMutable();
		CurrentUpgradeLevel = 0;
		CardModel cardModel = ModelDb.GetById<CardModel>(base.Id).ToMutable();
		_dynamicVars = cardModel.DynamicVars.Clone(this);
		EnergyCost.ResetForDowngrade();
		_baseStarCost = cardModel.CanonicalStarCost;
		_keywords = Enumerable.ToHashSet<CardKeyword>((System.Collections.Generic.IEnumerable<CardKeyword>)cardModel.Keywords);
		AfterDowngraded();
		Enchantment?.ModifyCard();
		Affliction?.AfterApplied();
		Action? obj = this.Upgraded;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	protected virtual void AfterDowngraded()
	{
	}

	public void InvokeDrawn()
	{
		Action? obj = this.Drawn;
		if (obj != null)
		{
			obj.Invoke();
		}
	}

	public CardModel CreateClone()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (Pile != null && !Pile.Type.IsCombatPile())
		{
			throw new InvalidOperationException("Cannot create a clone of a card that is not in a combat pile.");
		}
		AssertMutable();
		CardModel cardModel = CardScope.CloneCard(this);
		cardModel._cloneOf = this;
		return cardModel;
	}

	public CardModel CreateDupe()
	{
		if (IsDupe)
		{
			return DupeOf.CreateDupe();
		}
		AssertMutable();
		CardModel cardModel = CreateClone();
		cardModel.IsDupe = true;
		cardModel.RemoveKeyword(CardKeyword.Exhaust);
		return cardModel;
	}

	public SerializableCard ToSerializable()
	{
		AssertMutable();
		return new SerializableCard
		{
			Id = base.Id,
			CurrentUpgradeLevel = CurrentUpgradeLevel,
			Props = SavedProperties.From(this),
			Enchantment = Enchantment?.ToSerializable(),
			FloorAddedToDeck = FloorAddedToDeck
		};
	}

	public static CardModel FromSerializable(SerializableCard save)
	{
		CardModel cardModel = SaveUtil.CardOrDeprecated(save.Id).ToMutable();
		save.Props?.Fill(cardModel);
		if (save.FloorAddedToDeck.HasValue)
		{
			cardModel.FloorAddedToDeck = save.FloorAddedToDeck;
		}
		cardModel.AfterDeserialized();
		if (!(cardModel is DeprecatedCard))
		{
			if (save.Enchantment != null)
			{
				cardModel.EnchantInternal(EnchantmentModel.FromSerializable(save.Enchantment), decimal.op_Implicit(save.Enchantment.Amount));
				cardModel.Enchantment.ModifyCard();
				cardModel.FinalizeUpgradeInternal();
			}
			for (int i = 0; i < save.CurrentUpgradeLevel; i++)
			{
				cardModel.UpgradeInternal();
				cardModel.FinalizeUpgradeInternal();
			}
		}
		return cardModel;
	}

	public override int CompareTo(AbstractModel? other)
	{
		if (this == other)
		{
			return 0;
		}
		if (other == null)
		{
			return 1;
		}
		int num = base.CompareTo(other);
		if (num != 0)
		{
			return num;
		}
		CardModel cardModel = (CardModel)other;
		int num2 = CurrentUpgradeLevel.CompareTo(cardModel.CurrentUpgradeLevel);
		if (num2 != 0)
		{
			return num2;
		}
		return 0;
	}
}
