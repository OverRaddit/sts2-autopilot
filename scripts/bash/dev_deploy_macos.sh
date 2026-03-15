#!/usr/bin/env bash
# Build ExampleMod and deploy to macOS STS2 mods folder.
# Usage: ./scripts/bash/dev_deploy_macos.sh [Debug|Release]
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
# shellcheck disable=SC1091
source "${SCRIPT_DIR}/_load_env.sh"

MOD_BASENAME="${MOD_BASENAME:-ExampleMod}"
CONFIG="${1:-Debug}"

if [[ -z "${STS2_INSTALL_DIR:-}" ]]; then
  echo "STS2_INSTALL_DIR is not set. Create ${PROJECT_ROOT}/.env from .env.example first." >&2
  exit 1
fi

# macOS mods path: inside .app bundle
GAME_MOD_DIR="${STS2_INSTALL_DIR}/SlayTheSpire2.app/Contents/MacOS/mods/${MOD_BASENAME}"

# Ensure dotnet is available
if ! command -v dotnet >/dev/null 2>&1; then
  if [[ -x "$HOME/.dotnet/dotnet" ]]; then
    export PATH="$HOME/.dotnet:$PATH"
  else
    echo "dotnet not found. Install .NET SDK first." >&2
    exit 1
  fi
fi

# Build
echo "==> Building ${MOD_BASENAME} (${CONFIG})..."
dotnet build "${PROJECT_ROOT}/${MOD_BASENAME}.csproj" -c "${CONFIG}" -p:Sts2InstallDir="${STS2_INSTALL_DIR}" -v quiet

DLL="${PROJECT_ROOT}/bin/${CONFIG}/net9.0/${MOD_BASENAME}.dll"
if [[ ! -f "${DLL}" ]]; then
  echo "Build failed: ${DLL} not found." >&2
  exit 1
fi

# Deploy
mkdir -p "${GAME_MOD_DIR}"
cp -f "${DLL}" "${GAME_MOD_DIR}/"
cp -f "${PROJECT_ROOT}/mod_manifest.json" "${GAME_MOD_DIR}/"

if [[ -f "${PROJECT_ROOT}/${MOD_BASENAME}.pck" ]]; then
  cp -f "${PROJECT_ROOT}/${MOD_BASENAME}.pck" "${GAME_MOD_DIR}/"
fi

echo "==> Deployed to: ${GAME_MOD_DIR}"
ls -lh "${GAME_MOD_DIR}/"
echo ""
echo "Restart STS2 to apply changes."
