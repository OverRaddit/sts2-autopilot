using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Players;

public class PlayerCombatState
{
	private readonly Player _player;

	private readonly List<Creature> _pets = new List<Creature>();

	private CardPile[]? _piles;

	private int _energy;

	private int _stars;

	[CompilerGenerated]
	private Action<int, int>? m_EnergyChanged;

	[CompilerGenerated]
	private Action<int, int>? m_StarsChanged;

	public System.Collections.Generic.IReadOnlyList<Creature> Pets => (System.Collections.Generic.IReadOnlyList<Creature>)_pets;

	[field: CompilerGenerated]
	public CardPile Hand
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Hand);

	[field: CompilerGenerated]
	public CardPile DrawPile
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Draw);

	[field: CompilerGenerated]
	public CardPile DiscardPile
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Discard);

	[field: CompilerGenerated]
	public CardPile ExhaustPile
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Exhaust);

	[field: CompilerGenerated]
	public CardPile PlayPile
	{
		[CompilerGenerated]
		get;
	} = new CardPile(PileType.Play);

	public System.Collections.Generic.IReadOnlyList<CardPile> AllPiles
	{
		get
		{
			if (_piles == null)
			{
				_piles = new CardPile[5] { Hand, DrawPile, DiscardPile, ExhaustPile, PlayPile };
			}
			return _piles;
		}
	}

	public System.Collections.Generic.IEnumerable<CardModel> AllCards => Enumerable.SelectMany<CardPile, CardModel>((System.Collections.Generic.IEnumerable<CardPile>)AllPiles, (Func<CardPile, System.Collections.Generic.IEnumerable<CardModel>>)((CardPile p) => (System.Collections.Generic.IEnumerable<CardModel>)p.Cards));

	public int Energy
	{
		get
		{
			return _energy;
		}
		set
		{
			if (_energy != value)
			{
				int energy = _energy;
				_energy = value;
				this.EnergyChanged?.Invoke(energy, _energy);
			}
		}
	}

	public int MaxEnergy => (int)Hook.ModifyMaxEnergy(_player.Creature.CombatState, _player, decimal.op_Implicit(_player.MaxEnergy));

	public int Stars
	{
		get
		{
			return _stars;
		}
		set
		{
			if (_stars != value)
			{
				int stars = _stars;
				_stars = value;
				CombatManager.Instance.History.StarsModified(_player.Creature.CombatState, _stars - stars, _player);
				this.StarsChanged?.Invoke(stars, _stars);
			}
		}
	}

	[field: CompilerGenerated]
	public OrbQueue OrbQueue
	{
		[CompilerGenerated]
		get;
	}

	public event Action<int, int>? EnergyChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, int> val = this.m_EnergyChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_EnergyChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, int> val = this.m_EnergyChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_EnergyChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public event Action<int, int>? StarsChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, int> val = this.m_StarsChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_StarsChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, int> val = this.m_StarsChanged;
			Action<int, int> val2;
			do
			{
				val2 = val;
				Action<int, int> val3 = (Action<int, int>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
				val = Interlocked.CompareExchange<Action<int, int>>(ref this.m_StarsChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public PlayerCombatState(Player player)
	{
		_player = player;
		CombatManager.Instance.StateTracker.Subscribe(this);
		System.Collections.Generic.IEnumerator<CardPile> enumerator = ((System.Collections.Generic.IEnumerable<CardPile>)AllPiles).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardPile current = enumerator.Current;
				CombatManager.Instance.StateTracker.Subscribe(current);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
		OrbQueue = new OrbQueue(player);
		OrbQueue.Clear();
		OrbQueue.AddCapacity(player.BaseOrbSlotCount);
	}

	public void AfterCombatEnd()
	{
		CombatManager.Instance.StateTracker.Unsubscribe(this);
		System.Collections.Generic.IEnumerator<CardPile> enumerator = ((System.Collections.Generic.IEnumerable<CardPile>)AllPiles).GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardPile current = enumerator.Current;
				current.Clear();
				CombatManager.Instance.StateTracker.Unsubscribe(current);
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
		_pets.Clear();
	}

	public void ResetEnergy()
	{
		Energy = MaxEnergy;
	}

	public void AddMaxEnergyToCurrent()
	{
		Energy += MaxEnergy;
	}

	public void LoseEnergy(decimal amount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("Must not be negative.", "amount");
		}
		Energy = (int)Math.Max(decimal.op_Implicit(Energy) - amount, 0m);
	}

	public void GainEnergy(decimal amount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("Must not be negative.", "amount");
		}
		Energy = (int)Math.Max(decimal.op_Implicit(Energy) + amount, 0m);
	}

	public bool HasEnoughResourcesFor(CardModel card, out UnplayableReason reason)
	{
		int num = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
		int num2 = Math.Max(0, card.GetStarCostWithModifiers());
		if (num > Energy && card.CombatState != null && Hook.ShouldPayExcessEnergyCostWithStars(card.CombatState, _player))
		{
			num2 += (num - Energy) * 2;
			num = Energy;
		}
		reason = UnplayableReason.None;
		if (num > Energy)
		{
			reason |= UnplayableReason.EnergyCostTooHigh;
		}
		if (num2 > Stars)
		{
			reason |= UnplayableReason.StarCostTooHigh;
		}
		return reason == UnplayableReason.None;
	}

	public void LoseStars(decimal amount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("Must not be negative.", "amount");
		}
		Stars = (int)Math.Max(decimal.op_Implicit(Stars) - amount, 0m);
	}

	public void GainStars(decimal amount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (amount < 0m)
		{
			throw new ArgumentException("Must not be negative.", "amount");
		}
		Stars = (int)Math.Max(decimal.op_Implicit(Stars) + amount, 0m);
	}

	public void AddPetInternal(Creature pet)
	{
		pet.Monster.AssertMutable();
		if (!_pets.Contains(pet))
		{
			if (pet.PetOwner != _player)
			{
				pet.PetOwner = _player;
			}
			pet.Died += OnPetDied;
			_pets.Add(pet);
		}
	}

	public Creature? GetPet<T>() where T : MonsterModel
	{
		return Enumerable.FirstOrDefault<Creature>((System.Collections.Generic.IEnumerable<Creature>)Pets, (Func<Creature, bool>)((Creature p) => p.Monster is T));
	}

	public void RecalculateCardValues()
	{
		System.Collections.Generic.IEnumerator<CardModel> enumerator = AllCards.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardModel current = enumerator.Current;
				current.Enchantment?.RecalculateValues();
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	public void EndOfTurnCleanup()
	{
		System.Collections.Generic.IEnumerator<CardModel> enumerator = AllCards.GetEnumerator();
		try
		{
			while (((System.Collections.IEnumerator)enumerator).MoveNext())
			{
				CardModel current = enumerator.Current;
				current.EndOfTurnCleanup();
			}
		}
		finally
		{
			((System.IDisposable)enumerator)?.Dispose();
		}
	}

	public bool HasCardsToPlay()
	{
		return Enumerable.Any<CardModel>((System.Collections.Generic.IEnumerable<CardModel>)Hand.Cards, (Func<CardModel, bool>)((CardModel c) => c.CanPlay()));
	}

	private void OnPetDied(Creature pet)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!_pets.Contains(pet))
		{
			throw new InvalidOperationException("Player does not have pet " + pet.Name);
		}
		if (Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(pet.CombatState, pet))
		{
			pet.Died -= OnPetDied;
			_pets.Remove(pet);
		}
	}
}
