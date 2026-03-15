using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Runs;

namespace ExampleMod.Automation;

/// <summary>
/// Serializes the current combat state into a JSON string for the MCP server.
/// </summary>
public static class CombatStateSerializer
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static string Serialize(CombatState combatState, IRunState runState, Player player)
    {
        var creature = player.Creature;
        var pcs = player.PlayerCombatState!;

        var state = new Dictionary<string, object?>
        {
            ["turn"] = combatState.RoundNumber,
            ["player"] = SerializePlayer(player, creature, pcs),
            ["enemies"] = SerializeEnemies(combatState)
        };

        return JsonSerializer.Serialize(state, JsonOpts);
    }

    private static Dictionary<string, object?> SerializePlayer(
        Player player, Creature creature, PlayerCombatState pcs)
    {
        var hand = new List<Dictionary<string, object?>>();
        int i = 0;
        foreach (var card in pcs.Hand.Cards)
        {
            hand.Add(new Dictionary<string, object?>
            {
                ["index"] = i,
                ["name"] = card.Title,
                ["type"] = card.GetType().BaseType?.Name ?? card.GetType().Name,
                ["energy_cost"] = card.EnergyCost.GetAmountToSpend(),
                ["target_type"] = card.TargetType.ToString(),
                ["can_play"] = card.CanPlay(),
                ["description"] = card.Description.GetFormattedText()
            });
            i++;
        }

        var powers = new List<Dictionary<string, object?>>();
        foreach (var power in creature.Powers)
        {
            powers.Add(new Dictionary<string, object?>
            {
                ["name"] = power.Title.GetFormattedText(),
                ["amount"] = power.Amount,
                ["type"] = power.GetType().Name
            });
        }

        var potions = new List<Dictionary<string, object?>>();
        for (int slot = 0; slot < player.PotionSlots.Count; slot++)
        {
            var potion = player.PotionSlots[slot];
            potions.Add(new Dictionary<string, object?>
            {
                ["slot"] = slot,
                ["name"] = potion?.Title.GetFormattedText(),
                ["target_type"] = potion?.TargetType.ToString()
            });
        }

        var relics = new List<string>();
        foreach (var relic in player.Relics)
        {
            relics.Add(relic.Title.GetFormattedText());
        }

        return new Dictionary<string, object?>
        {
            ["hp"] = creature.CurrentHp,
            ["max_hp"] = creature.MaxHp,
            ["block"] = creature.Block,
            ["energy"] = pcs.Energy,
            ["max_energy"] = pcs.MaxEnergy,
            ["gold"] = player.Gold,
            ["powers"] = powers,
            ["hand"] = hand,
            ["draw_pile_count"] = pcs.DrawPile.Cards.Count,
            ["discard_pile_count"] = pcs.DiscardPile.Cards.Count,
            ["exhaust_pile_count"] = pcs.ExhaustPile.Cards.Count,
            ["relics"] = relics,
            ["potions"] = potions
        };
    }

    private static List<Dictionary<string, object?>> SerializeEnemies(CombatState combatState)
    {
        var enemies = new List<Dictionary<string, object?>>();
        int idx = 0;

        foreach (var creature in combatState.HittableEnemies)
        {
            var intents = new List<Dictionary<string, object?>>();

            if (creature.Monster?.NextMove?.Intents != null)
            {
                var targets = combatState.GetOpponentsOf(creature);
                foreach (var intent in creature.Monster.NextMove.Intents)
                {
                    var intentData = new Dictionary<string, object?>
                    {
                        ["type"] = intent.IntentType.ToString()
                    };

                    if (intent is AttackIntent attackIntent)
                    {
                        try
                        {
                            intentData["damage"] = attackIntent.GetSingleDamage(targets, creature);
                            intentData["hits"] = attackIntent.Repeats > 0 ? attackIntent.Repeats : 1;
                        }
                        catch
                        {
                            intentData["damage"] = 0;
                            intentData["hits"] = 1;
                        }
                    }
                    else
                    {
                        intentData["damage"] = 0;
                        intentData["hits"] = 0;
                    }

                    intents.Add(intentData);
                }
            }

            var powers = new List<Dictionary<string, object?>>();
            foreach (var power in creature.Powers)
            {
                powers.Add(new Dictionary<string, object?>
                {
                    ["name"] = power.Title.GetFormattedText(),
                    ["amount"] = power.Amount,
                    ["type"] = power.GetType().Name
                });
            }

            enemies.Add(new Dictionary<string, object?>
            {
                ["index"] = idx,
                ["name"] = creature.Monster?.Title.GetFormattedText() ?? "Unknown",
                ["hp"] = creature.CurrentHp,
                ["max_hp"] = creature.MaxHp,
                ["block"] = creature.Block,
                ["powers"] = powers,
                ["intents"] = intents
            });

            idx++;
        }

        return enemies;
    }
}
