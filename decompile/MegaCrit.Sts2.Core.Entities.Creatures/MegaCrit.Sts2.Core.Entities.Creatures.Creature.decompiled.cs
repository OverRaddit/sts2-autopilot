using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Entities.Creatures;

public class Creature
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <AfterAddedToRoom>d__108 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public Creature <>4__this;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			Creature creature = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0070;
				}
				if (creature.Side == CombatSide.Enemy)
				{
					val = creature.Monster.AfterAddedToRoom().GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AfterAddedToRoom>d__108>(ref val, ref this);
						return;
					}
					goto IL_0070;
				}
				goto end_IL_000e;
				IL_0070:
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <AfterTurnStart>d__135 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public int roundNumber;

		public CombatSide side;

		public Creature <>4__this;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			Creature creature = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0074;
				}
				if (roundNumber > 1 || side != CombatSide.Player)
				{
					val = creature.ClearBlock().GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AfterTurnStart>d__135>(ref val, ref this);
						return;
					}
					goto IL_0074;
				}
				goto end_IL_000e;
				IL_0074:
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <ClearBlock>d__138 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public Creature <>4__this;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			Creature creature = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0082;
				}
				if (!Hook.ShouldClearBlock(creature.CombatState, creature, out AbstractModel preventer))
				{
					val = Hook.AfterPreventingBlockClear(creature.CombatState, preventer, creature).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <ClearBlock>d__138>(ref val, ref this);
						return;
					}
					goto IL_0082;
				}
				creature.Block = 0;
				goto end_IL_000e;
				IL_0082:
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <TakeTurn>d__137 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public Creature <>4__this;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			Creature creature = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0090;
				}
				if (!creature.IsMonster || creature.Side != CombatSide.Enemy)
				{
					throw new InvalidOperationException("Only enemy monsters can take automated turns.");
				}
				if (!creature.Monster.SpawnedThisTurn)
				{
					val = creature.Monster.PerformMove().GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <TakeTurn>d__137>(ref val, ref this);
						return;
					}
					goto IL_0090;
				}
				goto end_IL_000e;
				IL_0090:
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
	private Action<int, int>? m_BlockChanged;

	[CompilerGenerated]
	private Action<int, int>? m_CurrentHpChanged;

	[CompilerGenerated]
	private Action<int, int>? m_MaxHpChanged;

	[CompilerGenerated]
	private Action<PowerModel>? m_PowerApplied;

	[CompilerGenerated]
	private Action<PowerModel, int, bool>? m_PowerIncreased;

	[CompilerGenerated]
	private Action<PowerModel, bool>? m_PowerDecreased;

	[CompilerGenerated]
	private Action<PowerModel>? m_PowerRemoved;

	[CompilerGenerated]
	private Action<Creature>? m_Died;

	[CompilerGenerated]
	private Action<Creature>? m_Revived;

	private int _block;

	private int _currentHp;

	private int _maxHp;

	private readonly List<PowerModel> _powers = new List<PowerModel>();

	private Player? _petOwner;

	public int Block
	{
		get
		{
			return _block;
		}
		private set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (value < 0)
			{
				throw new ArgumentException("Block must be positive", "value");
			}
			if (_block != value)
			{
				int block = _block;
				_block = value;
				this.BlockChanged?.Invoke(block, _block);
			}
		}
	}

	public int CurrentHp
	{
		get
		{
			return _currentHp;
		}
		private set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (value < 0)
			{
				throw new ArgumentException("Current HP must be positive", "value");
			}
			if (_currentHp != value)
			{
				int currentHp = _currentHp;
				_currentHp = value;
				this.CurrentHpChanged?.Invoke(currentHp, _currentHp);
			}
		}
	}

	public int MaxHp
	{
		get
		{
			return _maxHp;
		}
		private set
		{
			if (_maxHp != value)
			{
				int maxHp = _maxHp;
				_maxHp = value;
				this.MaxHpChanged?.Invoke(maxHp, _maxHp);
			}
		}
	}

	[field: CompilerGenerated]
	public int? MonsterMaxHpBeforeModification
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public uint? CombatId
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public MonsterModel? Monster
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public Player? Player
	{
		[CompilerGenerated]
		get;
	}

	public ModelId ModelId
	{
		get
		{
			if (!IsPlayer)
			{
				return Monster.Id;
			}
			return Player.Character.Id;
		}
	}

	[field: CompilerGenerated]
	public CombatSide Side
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public CombatState? CombatState
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public string Name
	{
		get
		{
			if (IsMonster)
			{
				return Monster.Title.GetFormattedText();
			}
			if (RunManager.Instance.IsSinglePlayerOrFakeMultiplayer)
			{
				return Player.Character.Title.GetFormattedText();
			}
			return PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, Player.NetId);
		}
	}

	public bool IsMonster => Monster != null;

	public bool IsPlayer => Player != null;

	[field: CompilerGenerated]
	public bool ShowsInfiniteHp
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public Player? PetOwner
	{
		get
		{
			return _petOwner;
		}
		set
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_petOwner != null)
			{
				throw new InvalidOperationException($"Pet {this} already has an owner.");
			}
			_petOwner = value;
		}
	}

	public bool IsPet => PetOwner != null;

	public System.Collections.Generic.IReadOnlyList<Creature> Pets => Player?.PlayerCombatState?.Pets ?? System.Array.Empty<Creature>();

	public bool IsAlive => CurrentHp > 0;

	public bool IsDead => !IsAlive;

	[field: CompilerGenerated]
	public string? SlotName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public System.Collections.Generic.IEnumerable<IHoverTip> HoverTips
	{
		get
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (!CombatManager.Instance.IsInProgress)
			{
				return System.Array.Empty<IHoverTip>();
			}
			List<IHoverTip> val = new List<IHoverTip>();
			if (IsMonster)
			{
				System.Collections.Generic.IEnumerator<AbstractIntent> enumerator = ((System.Collections.Generic.IEnumerable<AbstractIntent>)Monster.NextMove.Intents).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						AbstractIntent current = enumerator.Current;
						if (current.HasIntentTip)
						{
							val.Add((IHoverTip)current.GetHoverTip((System.Collections.Generic.IEnumerable<Creature>)CombatState.Allies, this));
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
			Enumerator<PowerModel> enumerator2 = _powers.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					PowerModel current2 = enumerator2.Current;
					System.Collections.Generic.IEnumerable<IHoverTip> hoverTips = current2.HoverTips;
					System.Collections.Generic.IEnumerator<IHoverTip> enumerator3 = hoverTips.GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator3).MoveNext())
						{
							IHoverTip current3 = enumerator3.Current;
							((System.Collections.Generic.ICollection<IHoverTip>)val).MegaTryAddingTip(current3);
						}
					}
					finally
					{
						((System.IDisposable)enumerator3)?.Dispose();
					}
				}
				return (System.Collections.Generic.IEnumerable<IHoverTip>)val;
			}
			finally
			{
				((System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			}
		}
	}

	public bool IsEnemy => Side == CombatSide.Enemy;

	public bool IsPrimaryEnemy
	{
		get
		{
			if (Side != CombatSide.Enemy)
			{
				return false;
			}
			return !IsSecondaryEnemy;
		}
	}

	public bool IsSecondaryEnemy
	{
		get
		{
			if (Side != CombatSide.Enemy)
			{
				return false;
			}
			return Enumerable.Any<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)Powers, (Func<PowerModel, bool>)((PowerModel p) => p.OwnerIsSecondaryEnemy));
		}
	}

	public bool IsHittable
	{
		get
		{
			if (IsDead)
			{
				return false;
			}
			if (!Hook.ShouldAllowHitting(CombatState, this))
			{
				return false;
			}
			return true;
		}
	}

	public bool CanReceivePowers
	{
		get
		{
			if (CombatState == null)
			{
				return false;
			}
			if (!Hook.ShouldAllowHitting(CombatState, this))
			{
				return false;
			}
			return true;
		}
	}

	public bool IsStunned => Monster?.NextMove.Id == "STUNNED";

	public System.Collections.Generic.IReadOnlyList<PowerModel> Powers => (System.Collections.Generic.IReadOnlyList<PowerModel>)_powers;

	public event Action<int, int>? BlockChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, int> val = this.m_BlockChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_BlockChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, int> val = this.m_BlockChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_BlockChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<int, int>? CurrentHpChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, int> val = this.m_CurrentHpChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_CurrentHpChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, int> val = this.m_CurrentHpChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_CurrentHpChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<int, int>? MaxHpChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, int> val = this.m_MaxHpChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_MaxHpChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, int> val = this.m_MaxHpChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_MaxHpChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PowerModel>? PowerApplied
	{
		[CompilerGenerated]
		add
		{
			Action<PowerModel> val = this.m_PowerApplied;
			Action<PowerModel> val2;
			do
			{
				val2 = val;
				Action<PowerModel> val3 = (Action<PowerModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel>>(ref this.m_PowerApplied, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PowerModel> val = this.m_PowerApplied;
			Action<PowerModel> val2;
			do
			{
				val2 = val;
				Action<PowerModel> val3 = (Action<PowerModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel>>(ref this.m_PowerApplied, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PowerModel, int, bool>? PowerIncreased
	{
		[CompilerGenerated]
		add
		{
			Action<PowerModel, int, bool> val = this.m_PowerIncreased;
			Action<PowerModel, int, bool> val2;
			do
			{
				val2 = val;
				Action<PowerModel, int, bool> val3 = (Action<PowerModel, int, bool>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel, int, bool>>(ref this.m_PowerIncreased, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PowerModel, int, bool> val = this.m_PowerIncreased;
			Action<PowerModel, int, bool> val2;
			do
			{
				val2 = val;
				Action<PowerModel, int, bool> val3 = (Action<PowerModel, int, bool>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel, int, bool>>(ref this.m_PowerIncreased, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PowerModel, bool>? PowerDecreased
	{
		[CompilerGenerated]
		add
		{
			Action<PowerModel, bool> val = this.m_PowerDecreased;
			Action<PowerModel, bool> val2;
			do
			{
				val2 = val;
				Action<PowerModel, bool> val3 = (Action<PowerModel, bool>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel, bool>>(ref this.m_PowerDecreased, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PowerModel, bool> val = this.m_PowerDecreased;
			Action<PowerModel, bool> val2;
			do
			{
				val2 = val;
				Action<PowerModel, bool> val3 = (Action<PowerModel, bool>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel, bool>>(ref this.m_PowerDecreased, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PowerModel>? PowerRemoved
	{
		[CompilerGenerated]
		add
		{
			Action<PowerModel> val = this.m_PowerRemoved;
			Action<PowerModel> val2;
			do
			{
				val2 = val;
				Action<PowerModel> val3 = (Action<PowerModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel>>(ref this.m_PowerRemoved, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PowerModel> val = this.m_PowerRemoved;
			Action<PowerModel> val2;
			do
			{
				val2 = val;
				Action<PowerModel> val3 = (Action<PowerModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PowerModel>>(ref this.m_PowerRemoved, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<Creature>? Died
	{
		[CompilerGenerated]
		add
		{
			Action<Creature> val = this.m_Died;
			Action<Creature> val2;
			do
			{
				val2 = val;
				Action<Creature> val3 = (Action<Creature>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<Creature>>(ref this.m_Died, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Creature> val = this.m_Died;
			Action<Creature> val2;
			do
			{
				val2 = val;
				Action<Creature> val3 = (Action<Creature>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<Creature>>(ref this.m_Died, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<Creature>? Revived
	{
		[CompilerGenerated]
		add
		{
			Action<Creature> val = this.m_Revived;
			Action<Creature> val2;
			do
			{
				val2 = val;
				Action<Creature> val3 = (Action<Creature>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<Creature>>(ref this.m_Revived, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Creature> val = this.m_Revived;
			Action<Creature> val2;
			do
			{
				val2 = val;
				Action<Creature> val3 = (Action<Creature>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<Creature>>(ref this.m_Revived, val3, val2);
			}
			while (val != val2);
		}
	}

	public Creature(MonsterModel monster, CombatSide side, string? slotName)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		monster.AssertMutable();
		int minInitialHp = monster.MinInitialHp;
		int maxInitialHp = monster.MaxInitialHp;
		if (minInitialHp > maxInitialHp)
		{
			throw new InvalidOperationException($"{monster.Id.Entry} has min HP {minInitialHp} greater than its max {maxInitialHp}!");
		}
		Monster = monster;
		Monster.Creature = this;
		SlotName = slotName;
		_maxHp = maxInitialHp;
		_currentHp = maxInitialHp;
		Side = side;
	}

	public Creature(Player player, int currentHp, int maxHp)
	{
		Player = player;
		_currentHp = currentHp;
		_maxHp = maxHp;
		Side = CombatSide.Player;
	}

	public void SetUniqueMonsterHpValue(System.Collections.Generic.IReadOnlyList<Creature> creaturesOnSide, Rng rng)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (Monster == null)
		{
			throw new InvalidOperationException("Can't set unique monster HP value for a player.");
		}
		int minInitialHp = Monster.MinInitialHp;
		int num = Monster.MaxInitialHp + 1;
		HashSet<int> val = Enumerable.ToHashSet<int>(Enumerable.Range(minInitialHp, num - minInitialHp));
		val.ExceptWith(Enumerable.Select<Creature, int>(Enumerable.Except<Creature>((System.Collections.Generic.IEnumerable<Creature>)creaturesOnSide, (System.Collections.Generic.IEnumerable<Creature>)new <>z__ReadOnlySingleElementList<Creature>(this)), (Func<Creature, int>)((Creature e) => e.MaxHp)));
		MonsterMaxHpBeforeModification = (_currentHp = (_maxHp = ((val.Count <= 0) ? rng.NextInt(minInitialHp, num) : rng.NextItem((System.Collections.Generic.IEnumerable<int>)val))));
	}

	public void ScaleMonsterHpForMultiplayer(EncounterModel? encounter, int playerCount, int actIndex)
	{
		if (playerCount != 1)
		{
			SetMaxHpInternal(ScaleHpForMultiplayer(decimal.op_Implicit(MaxHp), encounter, playerCount, actIndex));
			SetCurrentHpInternal(decimal.op_Implicit(MaxHp));
		}
	}

	public NCreatureVisuals? CreateVisuals()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (TestMode.IsOn)
		{
			return null;
		}
		if (Player != null)
		{
			return Player.Character.CreateVisuals();
		}
		if (Monster != null)
		{
			return Monster.CreateVisuals();
		}
		throw new InvalidOperationException("Creature and Monster should never both be null.");
	}

	[AsyncStateMachine(typeof(<AfterAddedToRoom>d__108))]
	public System.Threading.Tasks.Task AfterAddedToRoom()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<AfterAddedToRoom>d__108 <AfterAddedToRoom>d__ = default(<AfterAddedToRoom>d__108);
		<AfterAddedToRoom>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AfterAddedToRoom>d__.<>4__this = this;
		<AfterAddedToRoom>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <AfterAddedToRoom>d__.<>t__builder)).Start<<AfterAddedToRoom>d__108>(ref <AfterAddedToRoom>d__);
		return ((AsyncTaskMethodBuilder)(ref <AfterAddedToRoom>d__.<>t__builder)).Task;
	}

	public decimal DamageBlockInternal(decimal amount, ValueProp props)
	{
		decimal num = (((System.Enum)props).HasFlag((System.Enum)ValueProp.Unblockable) ? 0m : Math.Min(decimal.op_Implicit(Block), amount));
		Block -= (int)num;
		return num;
	}

	public DamageResult LoseHpInternal(decimal amount, ValueProp props)
	{
		bool flag = CurrentHp > 0 && amount >= decimal.op_Implicit(CurrentHp);
		int currentHp = CurrentHp;
		CurrentHp = Math.Max(CurrentHp - (int)amount, 0);
		return new DamageResult(this, props)
		{
			UnblockedDamage = currentHp - CurrentHp,
			WasTargetKilled = flag,
			OverkillDamage = (flag ? ((int)(-(decimal.op_Implicit(currentHp) - amount))) : 0)
		};
	}

	public void GainBlockInternal(decimal amount)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("amount must be positive. Use LoseBlock for block loss.");
		}
		Block = Math.Min(Block + (int)amount, 999);
	}

	public void LoseBlockInternal(decimal amount)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("amount must be positive. Use GainBlock for block gain.");
		}
		Block = Math.Max(Block - (int)amount, 0);
	}

	public void HealInternal(decimal amount)
	{
		bool isDead = IsDead;
		SetCurrentHpInternal(decimal.op_Implicit(CurrentHp) + amount);
		if (isDead && !IsDead)
		{
			Player?.ActivateHooks();
			this.Revived?.Invoke(this);
		}
	}

	public void SetCurrentHpInternal(decimal amount)
	{
		CurrentHp = (int)Math.Min(amount, decimal.op_Implicit(MaxHp));
	}

	public void SetMaxHpInternal(decimal amount)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("amount must be non-negative.");
		}
		MaxHp = (int)amount;
		CurrentHp = Math.Min(CurrentHp, MaxHp);
	}

	public void Reset()
	{
		RemoveAllPowersInternalExcept();
		Block = 0;
	}

	public void InvokeDiedEvent()
	{
		this.Died?.Invoke(this);
	}

	public void StunInternal(Func<System.Collections.Generic.IReadOnlyList<Creature>, System.Threading.Tasks.Task> stunMove, string? nextMoveId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (Monster == null)
		{
			throw new InvalidOperationException("Can't stun a player.");
		}
		if (CombatState != null && !IsDead)
		{
			if (string.IsNullOrEmpty(nextMoveId))
			{
				List<MonsterState> stateLog = Monster.MoveStateMachine.StateLog;
				nextMoveId = Enumerable.Last<MonsterState>((System.Collections.Generic.IEnumerable<MonsterState>)stateLog).Id;
			}
			MoveState state = new MoveState("STUNNED", stunMove, new StunIntent())
			{
				FollowUpStateId = nextMoveId,
				MustPerformOnceBeforeTransitioning = true
			};
			Monster.SetMoveImmediate(state);
		}
	}

	public void PrepareForNextTurn(System.Collections.Generic.IEnumerable<Creature> targets, bool rollNewMove = true)
	{
		Creature[] targets2 = Enumerable.ToArray<Creature>(targets);
		if (rollNewMove)
		{
			Monster.RollMove(targets2);
		}
		NCombatRoom.Instance?.GetCreatureNode(this)?.RefreshIntents();
	}

	public bool HasPower<T>() where T : PowerModel
	{
		return Enumerable.Any<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => p is T));
	}

	public bool HasPower(ModelId id)
	{
		return Enumerable.Any<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => p.Id == id));
	}

	public T? GetPower<T>() where T : PowerModel
	{
		return Enumerable.FirstOrDefault<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => p is T)) as T;
	}

	public PowerModel? GetPower(ModelId id)
	{
		return Enumerable.FirstOrDefault<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => p.Id == id));
	}

	public System.Collections.Generic.IEnumerable<T> GetPowerInstances<T>() where T : PowerModel
	{
		return Enumerable.OfType<T>((System.Collections.IEnumerable)_powers);
	}

	public PowerModel? GetPowerById(ModelId id)
	{
		return Enumerable.FirstOrDefault<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => p.Id == id));
	}

	public int GetPowerAmount<T>() where T : PowerModel
	{
		return GetPower<T>()?.Amount ?? 0;
	}

	public void ApplyPowerInternal(PowerModel power)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (power.Owner != this)
		{
			throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.ApplyInternal!");
		}
		if (!power.IsInstanced && Enumerable.Any<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => ((object)p).GetType() == ((object)power).GetType())))
		{
			throw new InvalidOperationException("Trying to add multiple instances of a non-instanced power to a creature.");
		}
		_powers.Add(power);
		this.PowerApplied?.Invoke(power);
	}

	public void InvokePowerModified(PowerModel power, int change, bool silent)
	{
		if (change > 0)
		{
			this.PowerIncreased?.Invoke(power, change, silent);
		}
		else if (((object)power.StackType/*cast due to .constrained prefix*/).Equals((object)PowerStackType.Counter) && power.AllowNegative && change < 0)
		{
			this.PowerIncreased?.Invoke(power, change, silent);
		}
		else
		{
			this.PowerDecreased?.Invoke(power, silent);
		}
	}

	public void RemovePowerInternal(PowerModel power)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (power.Owner != this)
		{
			throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.RemoveInternal!");
		}
		_powers.Remove(power);
		this.PowerRemoved?.Invoke(power);
	}

	public System.Collections.Generic.IEnumerable<PowerModel> RemoveAllPowersInternalExcept(System.Collections.Generic.IEnumerable<PowerModel>? except = null)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		List<PowerModel> val = Enumerable.ToList<PowerModel>(Enumerable.Except<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, except ?? System.Array.Empty<PowerModel>()));
		Enumerator<PowerModel> enumerator = val.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PowerModel current = enumerator.Current;
				current.RemoveInternal();
			}
			return (System.Collections.Generic.IEnumerable<PowerModel>)val;
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public System.Collections.Generic.IEnumerable<PowerModel> RemoveAllPowersAfterDeath()
	{
		return RemoveAllPowersInternalExcept(Enumerable.Where<PowerModel>((System.Collections.Generic.IEnumerable<PowerModel>)_powers, (Func<PowerModel, bool>)((PowerModel p) => !p.ShouldPowerBeRemovedAfterOwnerDeath() || !Hook.ShouldPowerBeRemovedOnDeath(p))));
	}

	public void BeforeTurnStart(int roundNumber, CombatSide side)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Enumerator<PowerModel> enumerator = _powers.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PowerModel current = enumerator.Current;
				current.AmountOnTurnStart = current.Amount;
			}
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	[AsyncStateMachine(typeof(<AfterTurnStart>d__135))]
	public System.Threading.Tasks.Task AfterTurnStart(int roundNumber, CombatSide side)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<AfterTurnStart>d__135 <AfterTurnStart>d__ = default(<AfterTurnStart>d__135);
		<AfterTurnStart>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AfterTurnStart>d__.<>4__this = this;
		<AfterTurnStart>d__.roundNumber = roundNumber;
		<AfterTurnStart>d__.side = side;
		<AfterTurnStart>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <AfterTurnStart>d__.<>t__builder)).Start<<AfterTurnStart>d__135>(ref <AfterTurnStart>d__);
		return ((AsyncTaskMethodBuilder)(ref <AfterTurnStart>d__.<>t__builder)).Task;
	}

	public void OnSideSwitch()
	{
		if (IsPlayer)
		{
			Player.OnSideSwitch();
		}
		else
		{
			Monster.OnSideSwitch();
		}
	}

	[AsyncStateMachine(typeof(<TakeTurn>d__137))]
	public System.Threading.Tasks.Task TakeTurn()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<TakeTurn>d__137 <TakeTurn>d__ = default(<TakeTurn>d__137);
		<TakeTurn>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<TakeTurn>d__.<>4__this = this;
		<TakeTurn>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <TakeTurn>d__.<>t__builder)).Start<<TakeTurn>d__137>(ref <TakeTurn>d__);
		return ((AsyncTaskMethodBuilder)(ref <TakeTurn>d__.<>t__builder)).Task;
	}

	[AsyncStateMachine(typeof(<ClearBlock>d__138))]
	private System.Threading.Tasks.Task ClearBlock()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<ClearBlock>d__138 <ClearBlock>d__ = default(<ClearBlock>d__138);
		<ClearBlock>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ClearBlock>d__.<>4__this = this;
		<ClearBlock>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <ClearBlock>d__.<>t__builder)).Start<<ClearBlock>d__138>(ref <ClearBlock>d__);
		return ((AsyncTaskMethodBuilder)(ref <ClearBlock>d__.<>t__builder)).Task;
	}

	public virtual string ToString()
	{
		return "Creature " + Name;
	}

	public double GetHpPercentRemaining()
	{
		return (double)_currentHp / (double)_maxHp;
	}

	public static decimal ScaleHpForMultiplayer(decimal hp, EncounterModel? encounter, int playerCount, int actIndex)
	{
		if (playerCount == 1)
		{
			return hp;
		}
		return hp * decimal.op_Implicit(playerCount) * MultiplayerScalingModel.GetMultiplayerScaling(encounter, actIndex);
	}
}
