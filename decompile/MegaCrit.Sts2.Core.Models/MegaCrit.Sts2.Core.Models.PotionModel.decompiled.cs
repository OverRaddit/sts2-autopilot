using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models;

public abstract class PotionModel : AbstractModel
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <OnUseWrapper>d__72 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public PotionModel <>4__this;

		public PlayerChoiceContext choiceContext;

		public Creature target;

		private CombatState <combatState>5__2;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			PotionModel potionModel = <>4__this;
			try
			{
				TaskAwaiter val6;
				TaskAwaiter val5;
				TaskAwaiter val4;
				TaskAwaiter val3;
				TaskAwaiter val2;
				TaskAwaiter val;
				switch (num)
				{
				default:
					potionModel.RemoveBeforeUse();
					<combatState>5__2 = potionModel.Owner.Creature.CombatState;
					choiceContext.PushModel(potionModel);
					val6 = CombatManager.Instance.WaitForUnpause().GetAwaiter();
					if (!((TaskAwaiter)(ref val6)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val6;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val6, ref this);
						return;
					}
					goto IL_00ac;
				case 0:
					val6 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_00ac;
				case 1:
					val5 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_011e;
				case 2:
					val4 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_02e9;
				case 3:
					val3 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0353;
				case 4:
					val2 = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_03fe;
				case 5:
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						break;
					}
					IL_03fe:
					((TaskAwaiter)(ref val2)).GetResult();
					potionModel.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(potionModel.Owner.NetId).PotionUsed.Add(potionModel.Id);
					val = CombatManager.Instance.CheckForEmptyHand(choiceContext, potionModel.Owner).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 5);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val, ref this);
						return;
					}
					break;
					IL_00ac:
					((TaskAwaiter)(ref val6)).GetResult();
					val5 = Hook.BeforePotionUsed(potionModel.Owner.RunState, <combatState>5__2, potionModel, target).GetAwaiter();
					if (!((TaskAwaiter)(ref val5)).IsCompleted)
					{
						num = (<>1__state = 1);
						<>u__1 = val5;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val5, ref this);
						return;
					}
					goto IL_011e;
					IL_02f0:
					val3 = potionModel.OnUse(choiceContext, target).GetAwaiter();
					if (!((TaskAwaiter)(ref val3)).IsCompleted)
					{
						num = (<>1__state = 3);
						<>u__1 = val3;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val3, ref this);
						return;
					}
					goto IL_0353;
					IL_011e:
					((TaskAwaiter)(ref val5)).GetResult();
					if (TestMode.IsOff && <combatState>5__2 != null)
					{
						NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(potionModel.Owner.Creature);
						Vector2 val7 = Vector2.Zero;
						if (potionModel.TargetType.IsSingleTarget())
						{
							NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(target);
							val7 = creatureNode2.GetBottomOfHitbox();
						}
						else
						{
							System.Collections.Generic.IReadOnlyList<Creature> readOnlyList = (System.Collections.Generic.IReadOnlyList<Creature>)((potionModel.TargetType != TargetType.AllEnemies) ? Enumerable.ToList<Creature>(Enumerable.Where<Creature>((System.Collections.Generic.IEnumerable<Creature>)<combatState>5__2.GetCreaturesOnSide(CombatSide.Player), (Func<Creature, bool>)((Creature c) => c.IsHittable))) : Enumerable.ToList<Creature>(Enumerable.Where<Creature>((System.Collections.Generic.IEnumerable<Creature>)<combatState>5__2.GetCreaturesOnSide(CombatSide.Enemy), (Func<Creature, bool>)((Creature c) => c.IsHittable))));
							System.Collections.Generic.IEnumerator<Creature> enumerator = ((System.Collections.Generic.IEnumerable<Creature>)readOnlyList).GetEnumerator();
							try
							{
								while (((System.Collections.IEnumerator)enumerator).MoveNext())
								{
									Creature current = enumerator.Current;
									NCreature creatureNode3 = NCombatRoom.Instance.GetCreatureNode(current);
									val7 += creatureNode3.VfxSpawnPosition;
								}
							}
							finally
							{
								if (num < 0)
								{
									((System.IDisposable)enumerator)?.Dispose();
								}
							}
							val7 /= (float)((System.Collections.Generic.IReadOnlyCollection<Creature>)readOnlyList).Count;
						}
						NItemThrowVfx child = NItemThrowVfx.Create(creatureNode.VfxSpawnPosition, val7, potionModel.Image);
						((Node)(object)NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node?)(object)child);
						val4 = Cmd.Wait(0.5f).GetAwaiter();
						if (!((TaskAwaiter)(ref val4)).IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__1 = val4;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val4, ref this);
							return;
						}
						goto IL_02e9;
					}
					goto IL_02f0;
					IL_02e9:
					((TaskAwaiter)(ref val4)).GetResult();
					goto IL_02f0;
					IL_0353:
					((TaskAwaiter)(ref val3)).GetResult();
					potionModel.InvokeExecutionFinished();
					if (<combatState>5__2 != null && CombatManager.Instance.IsInProgress)
					{
						CombatManager.Instance.History.PotionUsed(<combatState>5__2, potionModel, target);
					}
					val2 = Hook.AfterPotionUsed(potionModel.Owner.RunState, <combatState>5__2, potionModel, target).GetAwaiter();
					if (!((TaskAwaiter)(ref val2)).IsCompleted)
					{
						num = (<>1__state = 4);
						<>u__1 = val2;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnUseWrapper>d__72>(ref val2, ref this);
						return;
					}
					goto IL_03fe;
				}
				((TaskAwaiter)(ref val)).GetResult();
				choiceContext.PopModel(potionModel);
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

	public const string locTable = "potions";

	[CompilerGenerated]
	private Action? m_BeforeUse;

	private Player? _owner;

	private DynamicVarSet? _dynamicVars;

	private PotionModel _canonicalInstance;

	public LocString Title => new LocString("potions", base.Id.Entry + ".title");

	public LocString Description => new LocString("potions", base.Id.Entry + ".description");

	public LocString SelectionScreenPrompt => new LocString("potions", base.Id.Entry + ".selectionScreenPrompt");

	public LocString StaticDescription => Description;

	public LocString DynamicDescription
	{
		get
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			LocString description = Description;
			DynamicVars.AddTo(description);
			string prefix = EnergyIconHelper.GetPrefix(this);
			description.Add("energyPrefix", EnergyIconHelper.GetPrefix(this));
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
				return description;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}
	}

	private string PackedImagePath => ImageHelper.GetImagePath("atlases/potion_atlas.sprites/" + base.Id.Entry.ToLowerInvariant() + ".tres");

	private string PackedOutlinePath => ImageHelper.GetImagePath("atlases/potion_outline_atlas.sprites/" + base.Id.Entry.ToLowerInvariant() + ".tres");

	public string ImagePath => PackedImagePath;

	public Texture2D Image => ResourceLoader.Load<Texture2D>(PackedImagePath, (string)null, (CacheMode)1);

	public string? OutlinePath
	{
		get
		{
			if (!ResourceLoader.Exists(PackedOutlinePath, ""))
			{
				return null;
			}
			return PackedOutlinePath;
		}
	}

	public Texture2D? Outline
	{
		get
		{
			if (OutlinePath == null)
			{
				return null;
			}
			return ResourceLoader.Load<Texture2D>(OutlinePath, (string)null, (CacheMode)1);
		}
	}

	public abstract PotionRarity Rarity { get; }

	public abstract PotionUsage Usage { get; }

	public abstract TargetType TargetType { get; }

	public PotionPoolModel Pool => Enumerable.First<PotionPoolModel>(ModelDb.AllPotionPools, (Func<PotionPoolModel, bool>)([CompilerGenerated] (PotionPoolModel p) => Enumerable.Contains<ModelId>(p.AllPotionIds, base.Id)));

	public Player Owner
	{
		get
		{
			AssertMutable();
			return _owner;
		}
		set
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			AssertMutable();
			if (_owner != null && _owner != value)
			{
				throw new InvalidOperationException("Cannot move potion " + base.Id.Entry + " from one owner to another");
			}
			_owner = value;
		}
	}

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

	[field: CompilerGenerated]
	public bool IsQueued
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public virtual bool CanBeGeneratedInCombat => true;

	public virtual bool PassesCustomUsabilityCheck => true;

	public HoverTip HoverTip
	{
		get
		{
			HoverTip result = new HoverTip(Title, DynamicDescription);
			result.SetCanonicalModel(CanonicalInstance);
			return result;
		}
	}

	public System.Collections.Generic.IEnumerable<IHoverTip> HoverTips => Enumerable.Concat<IHoverTip>((System.Collections.Generic.IEnumerable<IHoverTip>)new IHoverTip[1] { HoverTip }, ExtraHoverTips);

	public virtual System.Collections.Generic.IEnumerable<IHoverTip> ExtraHoverTips => System.Array.Empty<IHoverTip>();

	public PotionModel CanonicalInstance
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

	public override bool ShouldReceiveCombatHooks => true;

	[field: CompilerGenerated]
	public bool HasBeenRemovedFromState
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public event Action BeforeUse
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_BeforeUse;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_BeforeUse, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_BeforeUse;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_BeforeUse, val3, val2);
			}
			while (val != val2);
		}
	}

	public PotionModel ToMutable()
	{
		AssertCanonical();
		PotionModel potionModel = (PotionModel)MutableClone();
		potionModel.CanonicalInstance = this;
		return potionModel;
	}

	protected override void AfterCloned()
	{
		base.AfterCloned();
		HasBeenRemovedFromState = false;
		this.BeforeUse = null;
	}

	public void Discard()
	{
		Owner.DiscardPotionInternal(this);
		HasBeenRemovedFromState = true;
	}

	public void RemoveBeforeUse()
	{
		Owner.RemoveUsedPotionInternal(this);
		HasBeenRemovedFromState = true;
	}

	public void EnqueueManualUse(Creature? target)
	{
		AssertMutable();
		Action? obj = this.BeforeUse;
		if (obj != null)
		{
			obj.Invoke();
		}
		UsePotionAction action = new UsePotionAction(this, target, CombatManager.Instance.IsInProgress);
		IsQueued = true;
		RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(action);
	}

	[AsyncStateMachine(typeof(<OnUseWrapper>d__72))]
	public System.Threading.Tasks.Task OnUseWrapper(PlayerChoiceContext choiceContext, Creature? target)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<OnUseWrapper>d__72 <OnUseWrapper>d__ = default(<OnUseWrapper>d__72);
		<OnUseWrapper>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<OnUseWrapper>d__.<>4__this = this;
		<OnUseWrapper>d__.choiceContext = choiceContext;
		<OnUseWrapper>d__.target = target;
		<OnUseWrapper>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <OnUseWrapper>d__.<>t__builder)).Start<<OnUseWrapper>d__72>(ref <OnUseWrapper>d__);
		return ((AsyncTaskMethodBuilder)(ref <OnUseWrapper>d__.<>t__builder)).Task;
	}

	public void AfterUsageCanceled()
	{
		IsQueued = false;
	}

	protected virtual System.Threading.Tasks.Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
	{
		return System.Threading.Tasks.Task.CompletedTask;
	}

	public SerializablePotion ToSerializable(int slotIndex)
	{
		AssertMutable();
		return new SerializablePotion
		{
			Id = base.Id,
			SlotIndex = slotIndex
		};
	}

	public static PotionModel FromSerializable(SerializablePotion save)
	{
		return SaveUtil.PotionOrDeprecated(save.Id).ToMutable();
	}

	protected static void AssertValidForTargetedPotion([NotNull] Creature? target)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (target == null)
		{
			throw new ArgumentNullException("target", "Target must be present for targeted potions.");
		}
	}

	public bool CanThrowAtAlly()
	{
		if (TargetType == TargetType.AnyPlayer && ((System.Collections.Generic.IReadOnlyCollection<Player>)Owner.RunState.Players).Count > 1)
		{
			return CombatManager.Instance.IsInProgress;
		}
		return false;
	}
}
