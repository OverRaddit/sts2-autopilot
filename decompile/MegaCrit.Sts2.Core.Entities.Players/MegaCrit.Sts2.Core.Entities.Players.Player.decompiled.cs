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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Entities.Players;

public class Player
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct <ReviveBeforeCombatEnd>d__161 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder <>t__builder;

		public Player <>4__this;

		private TaskAwaiter <>u__1;

		private void MoveNext()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			int num = <>1__state;
			Player player = <>4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = <>u__1;
					<>u__1 = default(TaskAwaiter);
					num = (<>1__state = -1);
					goto IL_007a;
				}
				if (player.Creature.IsDead)
				{
					val = CreatureCmd.Heal(player.Creature, 1m).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <ReviveBeforeCombatEnd>d__161>(ref val, ref this);
						return;
					}
					goto IL_007a;
				}
				goto end_IL_000e;
				IL_007a:
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

	public const int initialMaxPotionSlotCount = 3;

	[CompilerGenerated]
	private Action<RelicModel>? m_RelicObtained;

	[CompilerGenerated]
	private Action<RelicModel>? m_RelicRemoved;

	[CompilerGenerated]
	private Action<int>? m_MaxPotionCountChanged;

	[CompilerGenerated]
	private Action<PotionModel>? m_PotionProcured;

	[CompilerGenerated]
	private Action<PotionModel>? m_PotionDiscarded;

	[CompilerGenerated]
	private Action<PotionModel>? m_UsedPotionRemoved;

	[CompilerGenerated]
	private Action? m_AddPotionFailed;

	[CompilerGenerated]
	private Action? m_GoldChanged;

	private CardPile[]? _runPiles;

	private readonly List<RelicModel> _relics = new List<RelicModel>();

	private readonly List<PotionModel?> _potionSlots = new List<PotionModel>();

	private IRunState _runState = NullRunState.Instance;

	private int _gold;

	public int MaxPotionCount => _potionSlots.Count;

	[field: CompilerGenerated]
	public CharacterModel Character
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public Creature Creature
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public ulong NetId
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public PlayerRngSet PlayerRng
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public PlayerOddsSet PlayerOdds
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public RelicGrabBag RelicGrabBag
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public UnlockState UnlockState
	{
		[CompilerGenerated]
		get;
	}

	public IRunState RunState
	{
		get
		{
			return _runState;
		}
		set
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (!(_runState is NullRunState))
			{
				throw new InvalidOperationException("RunState has already been set.");
			}
			_runState = value;
		}
	}

	[field: CompilerGenerated]
	public bool IsActiveForHooks
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public PlayerCombatState? PlayerCombatState
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public ExtraPlayerFields ExtraFields
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = new ExtraPlayerFields();

	public System.Collections.Generic.IReadOnlyList<RelicModel> Relics => (System.Collections.Generic.IReadOnlyList<RelicModel>)_relics;

	public System.Collections.Generic.IReadOnlyList<PotionModel?> PotionSlots => (System.Collections.Generic.IReadOnlyList<PotionModel?>)_potionSlots;

	public System.Collections.Generic.IEnumerable<PotionModel> Potions => Enumerable.OfType<PotionModel>((System.Collections.IEnumerable)Enumerable.Where<PotionModel>((System.Collections.Generic.IEnumerable<PotionModel>)_potionSlots, (Func<PotionModel, bool>)((PotionModel p) => p != null)));

	public Creature? Osty => PlayerCombatState?.GetPet<Osty>();

	public bool IsOstyAlive => Osty?.IsAlive ?? false;

	public bool IsOstyMissing => !IsOstyAlive;

	public int Gold
	{
		get
		{
			return _gold;
		}
		set
		{
			if (value != Gold)
			{
				_gold = value;
				Action? obj = this.GoldChanged;
				if (obj != null)
				{
					obj.Invoke();
				}
			}
		}
	}

	[field: CompilerGenerated]
	public int MaxAscensionWhenRunStarted
	{
		[CompilerGenerated]
		get;
	}

	public bool HasOpenPotionSlots => Enumerable.Any<PotionModel>((System.Collections.Generic.IEnumerable<PotionModel>)_potionSlots, (Func<PotionModel, bool>)((PotionModel p) => p == null));

	[field: CompilerGenerated]
	public bool CanRemovePotions
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;

	private bool IsInventoryPopulated
	{
		get
		{
			if (!Enumerable.Any<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)Deck.Cards) && !Enumerable.Any<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Relics))
			{
				return Enumerable.Any<PotionModel>(Potions);
			}
			return true;
		}
	}

	[field: CompilerGenerated]
	public CardPile Deck
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Deck);

	[field: CompilerGenerated]
	public int MaxEnergy
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public List<ModelId> DiscoveredCards
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public List<ModelId> DiscoveredRelics
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public List<ModelId> DiscoveredPotions
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public List<ModelId> DiscoveredEnemies
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public List<string> DiscoveredEpochs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public int BaseOrbSlotCount
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public System.Collections.Generic.IEnumerable<CardPile> Piles
	{
		get
		{
			if (_runPiles == null)
			{
				_runPiles = new CardPile[1] { Deck };
			}
			return Enumerable.Concat<CardPile>((System.Collections.Generic.IEnumerable<CardPile>)(PlayerCombatState?.AllPiles ?? System.Array.Empty<CardPile>()), (System.Collections.Generic.IEnumerable<CardPile>)_runPiles);
		}
	}

	public event Action<RelicModel>? RelicObtained
	{
		[CompilerGenerated]
		add
		{
			Action<RelicModel> val = this.m_RelicObtained;
			Action<RelicModel> val2;
			do
			{
				val2 = val;
				Action<RelicModel> val3 = (Action<RelicModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<RelicModel>>(ref this.m_RelicObtained, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<RelicModel> val = this.m_RelicObtained;
			Action<RelicModel> val2;
			do
			{
				val2 = val;
				Action<RelicModel> val3 = (Action<RelicModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<RelicModel>>(ref this.m_RelicObtained, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<RelicModel>? RelicRemoved
	{
		[CompilerGenerated]
		add
		{
			Action<RelicModel> val = this.m_RelicRemoved;
			Action<RelicModel> val2;
			do
			{
				val2 = val;
				Action<RelicModel> val3 = (Action<RelicModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<RelicModel>>(ref this.m_RelicRemoved, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<RelicModel> val = this.m_RelicRemoved;
			Action<RelicModel> val2;
			do
			{
				val2 = val;
				Action<RelicModel> val3 = (Action<RelicModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<RelicModel>>(ref this.m_RelicRemoved, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<int>? MaxPotionCountChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int> val = this.m_MaxPotionCountChanged;
			Action<int> val2;
			do
			{
				val2 = val;
				Action<int> val3 = (Action<int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int>>(ref this.m_MaxPotionCountChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> val = this.m_MaxPotionCountChanged;
			Action<int> val2;
			do
			{
				val2 = val;
				Action<int> val3 = (Action<int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int>>(ref this.m_MaxPotionCountChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PotionModel>? PotionProcured
	{
		[CompilerGenerated]
		add
		{
			Action<PotionModel> val = this.m_PotionProcured;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_PotionProcured, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PotionModel> val = this.m_PotionProcured;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_PotionProcured, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PotionModel>? PotionDiscarded
	{
		[CompilerGenerated]
		add
		{
			Action<PotionModel> val = this.m_PotionDiscarded;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_PotionDiscarded, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PotionModel> val = this.m_PotionDiscarded;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_PotionDiscarded, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<PotionModel>? UsedPotionRemoved
	{
		[CompilerGenerated]
		add
		{
			Action<PotionModel> val = this.m_UsedPotionRemoved;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_UsedPotionRemoved, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PotionModel> val = this.m_UsedPotionRemoved;
			Action<PotionModel> val2;
			do
			{
				val2 = val;
				Action<PotionModel> val3 = (Action<PotionModel>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<PotionModel>>(ref this.m_UsedPotionRemoved, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action AddPotionFailed
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_AddPotionFailed;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_AddPotionFailed, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_AddPotionFailed;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_AddPotionFailed, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action GoldChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_GoldChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_GoldChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Action val = this.m_GoldChanged;
			Action val2;
			do
			{
				val2 = val;
				Action val3 = (Action)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action>(ref this.m_GoldChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public bool HasEventPet()
	{
		if (!Enumerable.Any<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Relics, (Func<RelicModel, bool>)((RelicModel r) => r.AddsPet)))
		{
			return Enumerable.Any<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)Deck.Cards, (Func<CardModel, bool>)((CardModel c) => c is ByrdonisEgg));
		}
		return true;
	}

	private Player(CharacterModel character, ulong netId, int currentHp, int maxHp, int maxEnergy, int gold, int potionSlotCount, int orbSlotCount, RelicGrabBag sharedRelicGrabBag, UnlockState unlockState, List<ModelId>? discoveredCards = null, List<ModelId>? discoveredEnemies = null, List<string>? discoveredEpochs = null, List<ModelId>? discoveredPotions = null, List<ModelId>? discoveredRelics = null)
	{
		RunState = NullRunState.Instance;
		Character = character;
		NetId = netId;
		Creature = new Creature(this, currentHp, maxHp);
		MaxEnergy = maxEnergy;
		Gold = gold;
		SetMaxPotionCountInternal(potionSlotCount);
		BaseOrbSlotCount = orbSlotCount;
		RelicGrabBag = sharedRelicGrabBag;
		UnlockState = unlockState;
		PlayerRng = new PlayerRngSet(0u);
		PlayerOdds = new PlayerOddsSet(PlayerRng);
		DiscoveredCards = discoveredCards ?? new List<ModelId>();
		DiscoveredEnemies = discoveredEnemies ?? new List<ModelId>();
		DiscoveredEpochs = discoveredEpochs ?? new List<string>();
		DiscoveredPotions = discoveredPotions ?? new List<ModelId>();
		DiscoveredRelics = discoveredRelics ?? new List<ModelId>();
		IsActiveForHooks = Creature.IsAlive;
		MaxAscensionWhenRunStarted = (SaveManager.Instance?.Progress.GetStatsForCharacter(Character.Id))?.MaxAscension ?? 0;
	}

	public static Player CreateForNewRun<T>(UnlockState unlockState, ulong netId) where T : CharacterModel
	{
		return CreateForNewRun(ModelDb.Character<T>(), unlockState, netId);
	}

	public static Player CreateForNewRun(CharacterModel character, UnlockState unlockState, ulong netId)
	{
		Player player = new Player(character, netId, character.StartingHp, character.StartingHp, character.MaxEnergy, character.StartingGold, 3, character.BaseOrbSlotCount, new RelicGrabBag(), unlockState);
		player.PopulateStartingInventory();
		return player;
	}

	public static Player FromSerializable(SerializablePlayer save)
	{
		Player player = new Player(ModelDb.GetById<CharacterModel>(save.CharacterId), save.NetId, save.CurrentHp, save.MaxHp, save.MaxEnergy, save.Gold, save.MaxPotionSlotCount, save.BaseOrbSlotCount, MegaCrit.Sts2.Core.Runs.RelicGrabBag.FromSerializable(save.RelicGrabBag), MegaCrit.Sts2.Core.Unlocks.UnlockState.FromSerializable(save.UnlockState), Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)save.DiscoveredCards), Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)save.DiscoveredEnemies), Enumerable.ToList<string>((System.Collections.Generic.IEnumerable<string>)save.DiscoveredEpochs), Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)save.DiscoveredPotions), Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)save.DiscoveredRelics));
		player.PlayerRng = PlayerRngSet.FromSerializable(save.Rng);
		player.PlayerOdds = PlayerOddsSet.FromSerializable(save.Odds, player.PlayerRng);
		player.ExtraFields = ExtraPlayerFields.FromSerializable(save.ExtraFields);
		player.LoadInventory(save);
		return player;
	}

	public void InitializeSeed(string seed)
	{
		PlayerRng = new PlayerRngSet((uint)((ulong)StringHelper.GetDeterministicHashCode(seed) + NetId));
		PlayerOdds = new PlayerOddsSet(PlayerRng);
	}

	private void PopulateStartingInventory()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (IsInventoryPopulated)
		{
			throw new InvalidOperationException("Inventory is already populated.");
		}
		if (!(RunState is NullRunState))
		{
			throw new InvalidOperationException("A player's starting inventory must be populated before being added to a run.");
		}
		PopulateStartingDeck();
		PopulateStartingRelics();
		System.Collections.Generic.IEnumerator<PotionModel> enumerator = Enumerable.Select<PotionModel, PotionModel>((System.Collections.Generic.IEnumerable<PotionModel>)Character.StartingPotions, (Func<PotionModel, PotionModel>)((PotionModel p) => p.ToMutable())).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				PotionModel current = enumerator.Current;
				AddPotionInternal(current);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	private void LoadInventory(SerializablePlayer save)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (IsInventoryPopulated)
		{
			throw new InvalidOperationException("Inventory is already populated.");
		}
		if (!(RunState is NullRunState))
		{
			throw new InvalidOperationException("A player's inventory must be loaded before being added to a run.");
		}
		PopulateDeck(Enumerable.Select<SerializableCard, CardModel>((System.Collections.Generic.IEnumerable<SerializableCard>)save.Deck, (Func<SerializableCard, CardModel>)CardModel.FromSerializable));
		LoadPotions(save.Potions);
		PopulateRelics(Enumerable.Select<SerializableRelic, RelicModel>((System.Collections.Generic.IEnumerable<SerializableRelic>)save.Relics, (Func<SerializableRelic, RelicModel>)RelicModel.FromSerializable));
	}

	public void PopulateRelicGrabBagIfNecessary(Rng rng)
	{
		if (!RelicGrabBag.IsPopulated)
		{
			RelicGrabBag.Populate(this, rng);
		}
	}

	public SerializablePlayer ToSerializable()
	{
		return new SerializablePlayer
		{
			CharacterId = Character.Id,
			CurrentHp = Creature.CurrentHp,
			MaxHp = Creature.MaxHp,
			MaxEnergy = MaxEnergy,
			MaxPotionSlotCount = MaxPotionCount,
			BaseOrbSlotCount = BaseOrbSlotCount,
			NetId = NetId,
			Gold = Gold,
			Rng = PlayerRng.ToSerializable(),
			Odds = PlayerOdds.ToSerializable(),
			RelicGrabBag = RelicGrabBag.ToSerializable(),
			Deck = Enumerable.ToList<SerializableCard>(Enumerable.Select<CardModel, SerializableCard>((System.Collections.Generic.IEnumerable<CardModel>)Deck.Cards, (Func<CardModel, SerializableCard>)((CardModel c) => c.ToSerializable()))),
			Relics = Enumerable.ToList<SerializableRelic>(Enumerable.Select<RelicModel, SerializableRelic>((System.Collections.Generic.IEnumerable<RelicModel>)Relics, (Func<RelicModel, SerializableRelic>)((RelicModel r) => r.ToSerializable()))),
			Potions = Enumerable.ToList<SerializablePotion>(Enumerable.OfType<SerializablePotion>((System.Collections.IEnumerable)Enumerable.Select<PotionModel, SerializablePotion>((System.Collections.Generic.IEnumerable<PotionModel>)PotionSlots, (Func<PotionModel, int, SerializablePotion>)((PotionModel p, int i) => p?.ToSerializable(i))))),
			ExtraFields = ExtraFields.ToSerializable(),
			UnlockState = UnlockState.ToSerializable(),
			DiscoveredCards = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)DiscoveredCards),
			DiscoveredEnemies = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)DiscoveredEnemies),
			DiscoveredEpochs = Enumerable.ToList<string>((System.Collections.Generic.IEnumerable<string>)DiscoveredEpochs),
			DiscoveredPotions = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)DiscoveredPotions),
			DiscoveredRelics = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)DiscoveredRelics)
		};
	}

	public void SyncWithSerializedPlayer(SerializablePlayer player)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		if (player.NetId != NetId)
		{
			throw new InvalidOperationException($"Tried to sync player that has net ID {NetId} with SerializablePlayer that has net ID {player.NetId}!");
		}
		if (player.CharacterId != Character.Id)
		{
			throw new InvalidOperationException($"Character changed for player {NetId}! This is not allowed");
		}
		Creature.SetMaxHpInternal(decimal.op_Implicit(player.MaxHp));
		Creature.SetCurrentHpInternal(decimal.op_Implicit(player.CurrentHp));
		MaxEnergy = player.MaxEnergy;
		Gold = player.Gold;
		SetMaxPotionCountInternal(player.MaxPotionSlotCount);
		Deck.Clear(silent: true);
		Enumerator<RelicModel> enumerator = Enumerable.ToList<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)_relics).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RelicModel current = enumerator.Current;
				RemoveRelicInternal(current, silent: true);
			}
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		Enumerator<PotionModel> enumerator2 = Enumerable.ToList<PotionModel>((System.Collections.Generic.IEnumerable<PotionModel>)_potionSlots).GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				PotionModel current2 = enumerator2.Current;
				if (current2 != null)
				{
					DiscardPotionInternal(current2, silent: true);
				}
			}
		}
		finally
		{
			((System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
		}
		PopulateDeck(Enumerable.Select<SerializableCard, CardModel>((System.Collections.Generic.IEnumerable<SerializableCard>)player.Deck, (Func<SerializableCard, CardModel>)([CompilerGenerated] (SerializableCard c) => RunState.LoadCard(c, this))), silent: true);
		PopulateRelics(Enumerable.Select<SerializableRelic, RelicModel>((System.Collections.Generic.IEnumerable<SerializableRelic>)player.Relics, (Func<SerializableRelic, RelicModel>)RelicModel.FromSerializable), silent: true);
		LoadPotions(player.Potions, silent: true);
		PlayerRng.LoadFromSerializable(player.Rng);
		PlayerOdds.LoadFromSerializable(player.Odds);
		RelicGrabBag.LoadFromSerializable(player.RelicGrabBag);
		DiscoveredCards = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)player.DiscoveredCards);
		DiscoveredEnemies = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)player.DiscoveredEnemies);
		DiscoveredEpochs = Enumerable.ToList<string>((System.Collections.Generic.IEnumerable<string>)player.DiscoveredEpochs);
		DiscoveredPotions = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)player.DiscoveredPotions);
		DiscoveredRelics = Enumerable.ToList<ModelId>((System.Collections.Generic.IEnumerable<ModelId>)player.DiscoveredRelics);
		IsActiveForHooks = Creature.IsAlive;
	}

	public void AddRelicInternal(RelicModel relic, int index = -1, bool silent = false)
	{
		relic.AssertMutable();
		relic.Owner = this;
		if (index == -1)
		{
			_relics.Add(relic);
		}
		else
		{
			_relics.Insert(index, relic);
		}
		if (relic != null && !relic.IsMelted && relic.ShouldFlashOnPlayer)
		{
			relic.Flashed += OnRelicFlashed;
		}
		if (!silent)
		{
			this.RelicObtained?.Invoke(relic);
		}
	}

	public void RemoveRelicInternal(RelicModel relic, bool silent = false)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (!_relics.Contains(relic))
		{
			throw new InvalidOperationException($"Player does not have relic {relic.Id}");
		}
		_relics.Remove(relic);
		relic.RemoveInternal();
		if (relic.ShouldFlashOnPlayer)
		{
			relic.Flashed -= OnRelicFlashed;
		}
		if (!silent)
		{
			this.RelicRemoved?.Invoke(relic);
		}
	}

	public void MeltRelicInternal(RelicModel relic)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		if (!relic.IsWax)
		{
			throw new InvalidOperationException($"{relic.Id} is not wax.");
		}
		if (relic.IsMelted)
		{
			throw new InvalidOperationException($"{relic.Id} is already melted.");
		}
		if (!_relics.Contains(relic))
		{
			throw new InvalidOperationException($"Player does not have relic {relic.Id}");
		}
		if (relic.ShouldFlashOnPlayer)
		{
			relic.Flashed -= OnRelicFlashed;
		}
		relic.IsMelted = true;
		relic.Status = RelicStatus.Disabled;
	}

	public T? GetRelic<T>() where T : RelicModel
	{
		return Enumerable.FirstOrDefault<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Relics, (Func<RelicModel, bool>)((RelicModel r) => r is T)) as T;
	}

	public RelicModel? GetRelicById(ModelId id)
	{
		return Enumerable.FirstOrDefault<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Relics, (Func<RelicModel, bool>)((RelicModel r) => r.Id == id));
	}

	public int GetPotionSlotIndex(PotionModel model)
	{
		return _potionSlots.IndexOf(model);
	}

	public PotionModel? GetPotionAtSlotIndex(int index)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (index < 0 || index >= _potionSlots.Count)
		{
			throw new IndexOutOfRangeException($"Index {index} is not a valid potion slot index! Player has {_potionSlots.Count} potion slots");
		}
		return _potionSlots[index];
	}

	public void AddToMaxPotionCount(int maxPotionCountIncrease)
	{
		SetMaxPotionCountInternal(_potionSlots.Count + maxPotionCountIncrease);
	}

	public void SubtractFromMaxPotionCount(int maxPotionCountDecrease)
	{
		SetMaxPotionCountInternal(_potionSlots.Count - maxPotionCountDecrease);
	}

	private void SetMaxPotionCountInternal(int newMaxPotionCount)
	{
		if (newMaxPotionCount > _potionSlots.Count)
		{
			for (int i = _potionSlots.Count; i < newMaxPotionCount; i++)
			{
				_potionSlots.Add((PotionModel)null);
			}
			this.MaxPotionCountChanged?.Invoke(MaxPotionCount);
		}
		else
		{
			if (newMaxPotionCount >= _potionSlots.Count)
			{
				return;
			}
			for (int num = _potionSlots.Count - 1; num >= newMaxPotionCount; num--)
			{
				if (_potionSlots[num] != null)
				{
					int num2 = _potionSlots.IndexOf((PotionModel)null);
					if (num2 < newMaxPotionCount)
					{
						_potionSlots[num2] = _potionSlots[num];
					}
					else
					{
						DiscardPotionInternal(_potionSlots[num]);
					}
				}
				_potionSlots.RemoveAt(num);
			}
			this.MaxPotionCountChanged?.Invoke(MaxPotionCount);
		}
	}

	public PotionProcureResult AddPotionInternal(PotionModel potion, int slotIndex = -1, bool silent = false)
	{
		potion.AssertMutable();
		PotionProcureResult potionProcureResult = new PotionProcureResult
		{
			potion = potion
		};
		if (slotIndex < 0)
		{
			slotIndex = _potionSlots.IndexOf((PotionModel)null);
		}
		if (slotIndex >= 0)
		{
			if (_potionSlots[slotIndex] != null)
			{
				Log.Warn($"Tried to add potion {potion} at slot index {slotIndex} which is already filled with potion {_potionSlots[slotIndex]}!");
				if (!silent)
				{
					Action? obj = this.AddPotionFailed;
					if (obj != null)
					{
						obj.Invoke();
					}
				}
				potionProcureResult.success = false;
				potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
				return potionProcureResult;
			}
			potion.Owner = this;
			_potionSlots[slotIndex] = potion;
			if (!silent)
			{
				this.PotionProcured?.Invoke(potion);
			}
			potionProcureResult.success = true;
		}
		else
		{
			if (!silent)
			{
				Action? obj2 = this.AddPotionFailed;
				if (obj2 != null)
				{
					obj2.Invoke();
				}
			}
			potionProcureResult.success = false;
			potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
		}
		return potionProcureResult;
	}

	public void DiscardPotionInternal(PotionModel potion, bool silent = false)
	{
		RemovePotionInternal(potion);
		if (!silent)
		{
			this.PotionDiscarded?.Invoke(potion);
		}
	}

	public void RemoveUsedPotionInternal(PotionModel potion)
	{
		RemovePotionInternal(potion);
		this.UsedPotionRemoved?.Invoke(potion);
	}

	private void RemovePotionInternal(PotionModel potion)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		int num = _potionSlots.IndexOf(potion);
		if (num < 0)
		{
			throw new InvalidOperationException($"Tried to remove potion you don't have: {potion.Id}");
		}
		_potionSlots[num] = null;
	}

	private void PopulateStartingDeck()
	{
		List<CardModel> val = new List<CardModel>();
		System.Collections.Generic.IEnumerator<CardModel> enumerator = Character.StartingDeck.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardModel current = enumerator.Current;
				CardModel cardModel = current.ToMutable();
				cardModel.FloorAddedToDeck = 1;
				val.Add(cardModel);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
		PopulateDeck((System.Collections.Generic.IEnumerable<CardModel>)val);
	}

	private void PopulateDeck(System.Collections.Generic.IEnumerable<CardModel> cards, bool silent = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (Enumerable.Any<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)Deck.Cards))
		{
			throw new InvalidOperationException("Deck has already been populated.");
		}
		System.Collections.Generic.IEnumerator<CardModel> enumerator = cards.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardModel current = enumerator.Current;
				Deck.AddInternal(current, -1, silent);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	private void PopulateStartingRelics()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		List<RelicModel> val = Enumerable.ToList<RelicModel>(Enumerable.Select<RelicModel, RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Character.StartingRelics, (Func<RelicModel, RelicModel>)((RelicModel r) => r.ToMutable())));
		Enumerator<RelicModel> enumerator = val.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				RelicModel current = enumerator.Current;
				current.FloorAddedToDeck = 1;
				SaveManager.Instance.MarkRelicAsSeen(current);
			}
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		PopulateRelics((System.Collections.Generic.IEnumerable<RelicModel>)val);
	}

	private void PopulateRelics(System.Collections.Generic.IEnumerable<RelicModel> relics, bool silent = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Enumerable.Any<RelicModel>((System.Collections.Generic.IEnumerable<RelicModel>)Relics))
		{
			throw new InvalidOperationException("Relics have already been populated.");
		}
		System.Collections.Generic.IEnumerator<RelicModel> enumerator = relics.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				RelicModel current = enumerator.Current;
				AddRelicInternal(current, -1, silent);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	private void LoadPotions(List<SerializablePotion> serializablePotions, bool silent = false)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Enumerable.Any<PotionModel>(Potions))
		{
			throw new InvalidOperationException("Potions have already been populated.");
		}
		Enumerator<SerializablePotion> enumerator = serializablePotions.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				SerializablePotion current = enumerator.Current;
				AddPotionInternal(PotionModel.FromSerializable(current), current.SlotIndex, silent);
			}
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public void ResetCombatState()
	{
		PlayerCombatState = new PlayerCombatState(this);
	}

	public void PopulateCombatState(Rng rng, CombatState state)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Enumerator<CardModel> enumerator = Enumerable.ToList<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)Deck.Cards).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CardModel current = enumerator.Current;
				CardModel cardModel = state.CloneCard(current);
				cardModel.DeckVersion = current;
				PlayerCombatState.DrawPile.AddInternal(cardModel);
			}
		}
		finally
		{
			((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		PlayerCombatState.DrawPile.RandomizeOrderInternal(this, rng, state);
	}

	[AsyncStateMachine(typeof(<ReviveBeforeCombatEnd>d__161))]
	public System.Threading.Tasks.Task ReviveBeforeCombatEnd()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		<ReviveBeforeCombatEnd>d__161 <ReviveBeforeCombatEnd>d__ = default(<ReviveBeforeCombatEnd>d__161);
		<ReviveBeforeCombatEnd>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ReviveBeforeCombatEnd>d__.<>4__this = this;
		<ReviveBeforeCombatEnd>d__.<>1__state = -1;
		((AsyncTaskMethodBuilder)(ref <ReviveBeforeCombatEnd>d__.<>t__builder)).Start<<ReviveBeforeCombatEnd>d__161>(ref <ReviveBeforeCombatEnd>d__);
		return ((AsyncTaskMethodBuilder)(ref <ReviveBeforeCombatEnd>d__.<>t__builder)).Task;
	}

	public void AfterCombatEnd()
	{
		Creature.RemoveAllPowersInternalExcept();
		PlayerCombatState?.AfterCombatEnd();
		Creature.LoseBlockInternal(decimal.op_Implicit(Creature.Block));
	}

	private void OnRelicFlashed(RelicModel relic, System.Collections.Generic.IEnumerable<Creature> targets)
	{
		SfxCmd.Play(relic.FlashSfx);
		System.Collections.Generic.IEnumerator<Creature> enumerator = targets.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				Creature current = enumerator.Current;
				((Node)(object)NCombatRoom.Instance?.CombatVfxContainer).AddChildSafely((Node?)(object)NRelicFlashVfx.Create(relic, current));
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	public void OnSideSwitch()
	{
	}

	public void DeactivateHooks()
	{
		IsActiveForHooks = false;
	}

	public void ActivateHooks()
	{
		IsActiveForHooks = true;
	}
}
