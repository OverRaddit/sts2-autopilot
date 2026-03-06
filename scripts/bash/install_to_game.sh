#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
MOD_BASENAME="${MOD_BASENAME:-ExampleMod}"
DIST_DIR="${PROJECT_ROOT}/dist/${MOD_BASENAME}"
GAME_MOD_DIR="${1:-${PROJECT_ROOT}/../../../mods/${MOD_BASENAME}}"

if [[ ! -f "${DIST_DIR}/${MOD_BASENAME}.dll" ]]; then
  echo "Missing ${DIST_DIR}/${MOD_BASENAME}.dll. Run scripts/bash/build_and_stage.sh first." >&2
  exit 1
fi

mkdir -p "${GAME_MOD_DIR}"
if ! cp -f "${DIST_DIR}/${MOD_BASENAME}.dll" "${GAME_MOD_DIR}/${MOD_BASENAME}.dll"; then
  echo "Failed to copy ${MOD_BASENAME}.dll. It is likely locked by a running game process." >&2
  exit 1
fi

if [[ -f "${DIST_DIR}/${MOD_BASENAME}.pck" ]]; then
  if ! cp -f "${DIST_DIR}/${MOD_BASENAME}.pck" "${GAME_MOD_DIR}/${MOD_BASENAME}.pck"; then
    echo "Failed to copy ${MOD_BASENAME}.pck. Close Slay the Spire 2 and rerun this script." >&2
    exit 1
  fi
else
  echo "Warning: ${MOD_BASENAME}.pck not found in dist; DLL only was installed."
fi

echo "Installed to: ${GAME_MOD_DIR}"
ls -la "${GAME_MOD_DIR}"
