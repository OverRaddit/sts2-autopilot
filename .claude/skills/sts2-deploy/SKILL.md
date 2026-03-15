---
name: sts2-deploy
description: STS2 모드 빌드 및 macOS 게임에 배포. "모드 빌드", "모드 배포", "STS2 deploy" 등의 요청에 사용.
user_invocable: true
---

# STS2 모드 빌드 & 배포

프로젝트 루트: `/Users/simgeon-u/Documents/dev/side-project/sts2_ExampleMod/`

## 기본 동작: 빌드 + 배포

```bash
cd /Users/simgeon-u/Documents/dev/side-project/sts2_ExampleMod
./scripts/bash/dev_deploy_macos.sh
```

이 스크립트가 하는 일:
1. `.env`에서 `STS2_INSTALL_DIR` 로드
2. `dotnet build` 실행 → `bin/Debug/net9.0/ExampleMod.dll` 생성
3. DLL, `mod_manifest.json`, `ExampleMod.pck`를 게임 mods 폴더에 복사
4. 배포 경로: `{STS2_INSTALL_DIR}/SlayTheSpire2.app/Contents/MacOS/mods/ExampleMod/`

빌드 결과를 사용자에게 보여주고, 게임 재시작이 필요함을 알려줄 것.

Release 빌드가 필요하면:
```bash
./scripts/bash/dev_deploy_macos.sh Release
```

## PCK 재빌드

mod_manifest.json, 이미지, 또는 Godot 리소스가 변경된 경우 PCK를 재빌드해야 한다.

```bash
./scripts/bash/make_pck.sh
```

> **주의**: 반드시 프로젝트 내 `.tools/godot/Godot.app` (4.5.x)으로 빌드해야 한다. brew Godot(4.6+)는 버전 불일치로 실패.

PCK 재빌드 후 다시 `dev_deploy_macos.sh`를 실행하여 배포.

## 로그 확인

게임 실행 후 모드 관련 에러 확인:

```bash
cat ~/Library/Application\ Support/Godot/app_userdata/Slay\ the\ Spire\ 2/logs/godot.log | tail -100
```

모드 로드 성공/실패, Harmony 패치 적용 여부 등을 확인할 수 있다.

## 트러블슈팅

- **빌드 실패 (DLL 참조)**: `.csproj`의 `Sts2DataDir`가 `SlayTheSpire2.app/Contents/Resources/data_sts2_macos_arm64`인지 확인
- **모드 미인식**: mods 폴더가 `.app/Contents/MacOS/mods/` 안에 있는지 확인
- **Harmony 크래시**: BetterSpire2의 ARM64 패치된 `0Harmony.dll`이 설치되어 있는지 확인
- **PCK 로드 실패**: Godot 4.5.x로 빌드했는지 확인
