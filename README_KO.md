# ExampleMod (STS2 튜토리얼 모드)

`ExampleMod`는 Slay the Spire 2 모딩을 위한 주석이 포함된 기능별 스타터 프로젝트입니다.

포함 내용:
- Linux/WSL 빌드/설치 스크립트 (`bash`)
- conda 환경 파일 (`environment.yml`)
- 디컴파일 참조 폴더 (선별된 서브셋 포함)
- 메인 메뉴 토글로 제어되는 5가지 튜토리얼 예제

## 사전 준비 사항

다음을 먼저 설치하세요:

1. Steam을 통해 Slay the Spire 2 설치.
2. Windows에서 작업하는 경우 WSL2 (Ubuntu).
3. Git.
4. Miniconda (권장).
5. Godot 4.5.1 CLI (`./scripts/bash/setup_godot_cli.sh`를 사용하는 경우 선택 사항).

공식 가이드:
- WSL 설치: [Microsoft WSL 설치 가이드](https://learn.microsoft.com/windows/wsl/install)
- Git 설치: [Git 설치 가이드](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
- Miniconda 설치: [Miniconda 설치 가이드](https://www.anaconda.com/docs/getting-started/miniconda/install)
- .NET 9 SDK (conda에서 관리하는 dotnet을 사용하지 않는 경우에만): [Linux에서 .NET](https://learn.microsoft.com/dotnet/core/install/linux)
- Godot 4.5.1 릴리스 다운로드 (godot 설정 스크립트를 사용하지 않는 경우에만): [Godot 4.5.1](https://github.com/godotengine/godot/releases/tag/4.5.1-stable)

참고:
- 이 레포의 conda 환경은 Python + dotnet을 자동으로 설치합니다.
- `./scripts/bash/setup_godot_cli.sh`는 로컬 Godot CLI 바이너리를 `.tools/`에 다운로드합니다.
- 로컬 `.env` 파일에 `STS2_INSTALL_DIR`을 설정해야 합니다 (아래 참조).

## 이 모드의 튜토리얼 예제

모든 게임플레이 예제는 기본적으로 비활성화되어 있으며, 메인 메뉴 버튼(`Example Mod`)에서 제어합니다.

1. 상단 바에 `Gamble` 버튼 추가 (배팅 금액 선택 포함)
2. `Burning Blood`의 전투 후 회복량을 6에서 10으로 변경
3. 아이언클래드 `Strike` 기본 데미지를 10으로 고정
4. 매 전투 시작 시 아이언클래드에게 힘(Strength) +1 부여
5. 모든 기능을 켜고 끌 수 있는 메인 메뉴 UI 추가

## 클론 위치

이 레포지토리는 어디든 클론할 수 있습니다.

예시:

```bash
mkdir -p ~/dev/sts2
cd ~/dev/sts2
git clone https://github.com/customjack/sts2_ExampleMod ExampleMod
cd ExampleMod
```

## .env 설정 (필수)

스크립트를 실행하기 전에 `.env.example`에서 로컬 `.env`를 생성해야 합니다.

한 번만 실행:

```bash
cp .env.example .env
```

그런 다음 `.env`를 편집하여 다음을 설정합니다:

```bash
STS2_INSTALL_DIR='C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2'
```

참고:
- `.env`는 로컬 전용이며 기본적으로 gitignore에 포함되어 있습니다.
- Windows 경로(`C:\...`) 또는 WSL 경로(`/mnt/c/...`) 모두 사용할 수 있습니다.
- `scripts/bash/`의 Bash 스크립트는 `.env`를 자동으로 로드합니다.

## 환경 설정

### 옵션 A: Conda (권장)

#### Linux/WSL

```bash
cd /path/to/ExampleMod
cp .env.example .env   # 한 번만, 이후 STS2_INSTALL_DIR 편집
./scripts/bash/setup_env.sh
source ./scripts/bash/activate_env.sh
```

### 옵션 B: 시스템 도구 사용

`사전 준비 사항` 섹션의 가이드를 참고한 후, 도구들이 `PATH`에 있는지 확인하세요.

## 빌드 / 패키징 / 설치

### Linux/WSL

```bash
./scripts/bash/build_and_stage.sh
./scripts/bash/setup_godot_cli.sh
./scripts/bash/make_pck.sh
./scripts/bash/install_to_game.sh
```

설치된 모드 출력 위치:
- `${STS2_INSTALL_DIR}/mods/ExampleMod/ExampleMod.dll`
- `${STS2_INSTALL_DIR}/mods/ExampleMod/ExampleMod.pck`

## 토글 메뉴 사용법

1. 모드를 활성화한 상태로 STS2를 실행합니다.
2. 메인 메뉴에서 `Example Mod`를 클릭합니다.
3. 기능 체크박스를 토글합니다.
4. 런을 시작하거나 다음 전투에서 활성화된 예제를 확인합니다.

참고:
- 토글 상태는 `user://example_mod_settings.json`에 저장됩니다.
- 이미 진행 중인 런의 오브젝트는 다음 관련 재빌드/전투까지 이전 값을 유지할 수 있습니다.

## 이 레포의 디컴파일 폴더

`decompile/`에는 이 예제들에 필요한 디컴파일된 게임 파일의 선별된 서브셋이 포함되어 있습니다.

로컬의 전체 디컴파일 덤프에서 새로고침하려면:

### Linux/WSL

```bash
./scripts/bash/sync_decompile_subset.sh
```

## 모드 매니페스트와 미리보기 이미지

`mod_manifest.json`을 편집하여 모드의 표시 방식을 변경합니다:

```json
{
  "pck_name": "ExampleMod",
  "name": "ExampleMod",
  "author": "local-dev",
  "description": "Tutorial mod with toggleable gameplay examples.",
  "version": "0.1.0"
}
```

참고:
- `pck_name`은 모드 패키지/기본 이름과 일치해야 합니다 (보통 DLL/PCK 이름과 동일).
- `name`, `author`, `description`, `version`은 표시용 메타데이터입니다.

미리보기 이미지:
- 프로젝트 루트의 `mod_image.png`를 사용합니다.
- 이 레포에는 이미 작동하는 `mod_image.png` 미리보기 이미지가 포함되어 있습니다.
- 모드 카드 이미지를 변경하려면 자신의 PNG로 교체하세요.

매니페스트나 이미지를 변경한 후 PCK를 다시 빌드합니다:

```bash
./scripts/bash/make_pck.sh
./scripts/bash/install_to_game.sh
```

## Godot 없이 로그 보기

포함된 웹 로그 뷰어를 실행합니다:

```bash
cd ./scripts/tools/log_viewer
./run.sh
```

그런 다음 다음 주소를 엽니다:

```text
http://127.0.0.1:8765
```

기본 로그 경로:
- `/mnt/c/Users/*/AppData/Roaming/SlayTheSpire2/logs/godot.log`에서 자동 감지

다음으로 오버라이드할 수 있습니다:

```bash
LOG_VIEWER_LOG_PATH="/mnt/c/Users/<사용자명>/AppData/Roaming/SlayTheSpire2/logs/godot.log" ./run.sh
```

## 선택적 디버깅

원격 디버그 실행 옵션:

```text
--remote-debug tcp://127.0.0.1:6007
```

그런 다음 Godot 에디터의 출력/원격 인스펙터에서 연결합니다.
