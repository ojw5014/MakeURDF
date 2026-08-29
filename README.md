# MakeURDF

DH 파라미터로 로봇을 만들고 **URDF · USD · MJCF** 로 내보내는 윈도우 프로그램입니다.
그대로 **PyBullet · MuJoCo · Isaac Sim** 에서 씁니다.

STL 파일이나 3D 모델링 없이, 숫자 일곱 개짜리 줄을 몇 번 적는 것만으로 로봇 뼈대가 만들어집니다.

## 📖 처음이라면 — 입문 매뉴얼부터

### **▶ [양팔 로봇 스켈레톤 만들기](https://ojw5014.github.io/MakeURDF/)**

DH 열 줄로 양팔 로봇을 만들고, 관절을 돌리고, 역기구학까지 돌려보는 그림 위주 문서입니다.
그림 21장이 전부 실제 화면 캡처이고, 관절 회전과 IK 는 애니메이션으로 들어 있습니다.
브라우저에서 바로 열립니다.

> 오프라인으로 보시려면 `docs/index.html` 을 그대로 여십시오 — 그림이 파일 안에 들어 있어 단독으로 열립니다.

전체 기능 설명은 [`bin/doc/MakeUrdf_Manual.md`](bin/doc/MakeUrdf_Manual.md) 에 있습니다.

## 실행

빌드 없이 바로 쓰려면 `bin/Release/MakeUrdf.exe` 를 실행하십시오. (.NET Framework 4.0)

직접 빌드하려면 `MakeUrdf.sln` 을 Visual Studio 로 여십시오.
참조 DLL 은 `lib/` 에 동봉되어 있어 이 저장소만 클론해도 빌드됩니다.

## 무엇을 만드나

| 형식 | 파일 | 쓰는 곳 |
|------|------|---------|
| URDF | `.urdf` | PyBullet · ROS · Isaac Sim |
| USD  | `.usda` | Isaac Sim (직접 열기) |
| MJCF | `.xml`  | MuJoCo |

관절 한계 · 질량 · 관성 · mimic 커플링이 세 형식에 **같은 값으로** 실립니다.

## 폴더

```
Main*.cs            프로그램 소스 (C#, WinForms)
lib/                빌드에 필요한 참조 DLL
bin/Release/        실행 파일 일체
bin/doc/            전체 기능 매뉴얼
bin/python/         PyBullet · MuJoCo · Isaac Sim 뷰어/러너
docs/               입문 매뉴얼 (웹에 공개되는 폴더)
```

## 라이선스 / 문의

OpenJigWare — <https://github.com/ojw5014>
