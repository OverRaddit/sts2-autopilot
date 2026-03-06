#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
SOURCE_ROOT="${PROJECT_ROOT}/../../decompile"
TARGET_ROOT="${PROJECT_ROOT}/decompile"

FILES=(
  "MegaCrit.Sts2.Core.Hooks/Hook.cs"
  "MegaCrit.Sts2.Core.Models.Cards/StrikeIronclad.cs"
  "MegaCrit.Sts2.Core.Models.Relics/BurningBlood.cs"
  "MegaCrit.Sts2.Core.Nodes.CommonUi/NTopBar.cs"
  "MegaCrit.Sts2.Core.Nodes.Screens.MainMenu/NMainMenu.cs"
  "MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen/NModInfoContainer.cs"
)

for rel in "${FILES[@]}"; do
  src="${SOURCE_ROOT}/${rel}"
  dst="${TARGET_ROOT}/${rel}"
  mkdir -p "$(dirname "${dst}")"
  cp -f "${src}" "${dst}"
  echo "Synced: ${rel}"
done
