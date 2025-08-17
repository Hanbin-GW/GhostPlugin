GhostPlugin은 내가 제작한 대부분의 플러그인을 하나로 합친 SCP SL용 통합 플러그인입니다.
이 플러그인은 무료가 아니며, GhostServer에서 **기밀(Classified) 플러그인**으로 분류됩니다.

> [!IMPORTANT]
> 릴리즈에는 두 가지 버전이 있습니다: `GhostPlugin-Eng.dll`과 `GhostPlugin.dll`.
> **한국 서버 소유자**라면 `GhostPlugin.dll`을 사용해야 하며,
> **영문 유저**라면 `GhostPlugin-Eng.dll`을 사용하는 것을 강력히 추천합니다.

---

# 포함된 플러그인 목록:

* CustomRole (커스텀 역할)
* CustomItem (커스텀 아이템)
* 서버 전용 설정 시스템
* GhostPlugin의 커스텀 역할 능력
* 블랙아웃 모드
* 미니맵 (비활성화됨)

# GhostPlugin의 커스텀 역할

|                                             역할 이름                                             | ID |                 능력                 |        스폰 방식         |                             설명                             |
|:---------------------------------------------------------------------------------------------:|:--:|:----------------------------------:|:--------------------:|:----------------------------------------------------------:|
|                                            수석 과학자                                             | 1  |                 없음                 |      라운드 시작 즉시       |          SCP 제단의 총책임 연구원! 보유한 SCP 아이템을 이용해 탈출하라!           |
|     [팬텀](https://www.notion.so/Phantom-21c626d7af6a80cfae3bddf0df02e8ac?source=copy_link)     | 2  |          집중, 유령, 처치 시 회복           |    40% 확률로 카오스 스폰    |           카오스 반란군의 유령 저격팀. 수많은 MTF를 제거한 공포의 상징.            |
|                                              피험자                                              | 3  |               능동 위장                |      라운드 시작 즉시       |               제단의 인체 실험으로 특별한 능력을 얻은 실험체...!               |
|                                            엘리트 요원                                             | 4  |              처치 시 회복               |   80% 확률로 NTF로 리스폰   |                       MTF 소속의 엘리트 요원                       |
|                                            페도라 요원                                             | 5  |                 없음                 |    60% 확률로 카오스 스폰    |                      산성 무기를 가진 특수 요원                       |
|                                            죄수복 남자                                             | 6  |                 없음                 |   100% 확률로 NTF 스폰    |                  죄수복 담당관인 Agent Vetterang                  |
|                                            O5 관리자                                             | 7  |                 없음                 |   10% 확률로 라운드 시작 시   |            O5 관리자! 시설에서 탈출하고 MTF에게 구조 신호를 보내세요!            |
|                                          SCP 106 탱커                                           | 8  |                 없음                 |   50% 확률로 라운드 시작 시   |            구형 SCP-106. 추적 능력은 없지만 75% 피해 감소가 있음            |
|                                            생화학 경비                                             | 9  |                 없음                 |      라운드 시작 즉시       |                         생화학 무장 경비원                         |
|                                        SCP 049 영혼 약탈자                                         | 10 | Speedy096, 폭발, SCP106, Scp457, 오버킬 |      기부자 전용 역할       |               각종 SCP 능력을 사용하는 SCP 049의 각성 버전               |
|                                              거절됨                                              | 11 |                거절됨                 |         거절됨          |                                                            |
|                                          MTF 폭파 전문가                                           | 12 |                 없음                 |    40% 확률로 MTF 스폰    |                      폭발물을 다루는 MTF 요원                       |
|                                              거절됨                                              | 13 |                거절됨                 |         거절됨          |                                                            |
|                                            행운의 경비                                             | 14 |                 없음                 |      라운드 시작 즉시       |                     행운이 당신을 도울 것입니다..!                     |
| [총잡이 카오스](https://www.notion.so/Gunslinger-21c626d7af6a8014bcdac8ec01ecaa11?source=copy_link) | 15 |                오버킬                 |     무기를 잘 다루는 요원     |                                                            |
|                                              난쟁이                                              | 16 |                 없음                 |   D계급 인원의 아주 작은 버전   |                                                            |
|                                             잠입 요원                                             | 17 |             라운드 시작 즉시              |     적을 처치해 무장 해제     | D계급으로 위장한 카오스 스파이. 신분을 숨기고 제단을 무너뜨리세요! (키카드를 버리면 외형 변경 가능) |
|                                              집행자                                              | 18 |              처치 시 강화               |    30% 확률로 MTF 스폰    |                 강력한 근접 공격과 방어력을 지닌 전투 전문가                  |
|                                              전략가                                              | 19 |               향상된 시야               |    90% 확률로 카오스 스폰    |               다양한 전략으로 전장을 지배한 카오스 반란군의 지휘관                |
|                                          MTF  Medic                                           | 21 |                 없음                 |    55% 확률로 MTF 스폰    |                   부활키트 를 사용하여 아군을 부활하십시요                   |
|                                         Advanced  Ops                                         | 22 |                 없음                 |    80% 확률로 MTF 스폰    |                      고급 무기들을 가지고 있습니다                      |
|                                             병참장교                                              | 20 |                 없음                 |    80% 확률로 카오스 스폰    |                  탄약을 지속적으로 보급하는 후방 지원 병력                   |
|                                              헌터                                               | 23 |                 돌진                 |    50% 확률로 카오스 스폰    |                  특정 대상을 처치하는거에 특화되어 있습니다.                  |
|                                           EOD 좀비 병사                                           | 26 |               폭탄 저항                |     SCP049 부활 전용     |                     폭발에 저항력을 가진 좀비 병사                      |
|                                           Hugo Boss                                           | 24 |                 없음                 |   55% 확률로  MTF 스폰    |             많은 적을 분쇠시키시시요! 여러 고급무기들을 가지고있습니다.              |
|                                            중무장 카오스                                            | 39 |                 없음                 |    35% 확률로 카오스 스폰    |                      매우 중무장한 카오스 반란군                       |
|                                         탄도 SCP-049-2                                          | 42 |                 자폭                 |  80% 확률로 SCP049 부활   |                        사망 시 자폭하는 좀비                        |
|                                         난쟁이 SCP-049-2                                         | 43 |                 없음                 |  75% 확률로 SCP049 부활   |                        아주 작은 좀비 버전                         |
|                                             쇼크웨이브                                             | 44 |                충격파                 | 049에 의한 부활  90% (1명) |                      능력사용시 근처의 적을 기절                       |

### 증원군
|  역할 이름   |  ID |
|:--------:| :-: |
| 대형 요원 증원 |  57 |
|  증원 요원   |  58 |
|  증원 지휘관  |  59 |
|   C-요원   |  61 |
|  C-특수요원  |  62 |
|  C-지휘관   |  63 |
| 뱀의 손 감시관 |  71 |
| 뱀의 손 수호자 |  72 |
| 뱀의 손 병사  |  73 |

---
Development Context (SCP: Secret Laboratory Custom Multiplayer Modding)

In the Unity-based multiplayer game SCP: Secret Laboratory, I do not merely participate as a player—I engage as a developer who disassembles and reconstructs core systems to create new gameplay experiences.

Engine / Language: Unity (C#)

Key Technologies & Techniques:

(a) Utilized the CreateSchematic() method to dynamically spawn in-game objects (e.g., shields) in real time at the player’s aiming position.

(b) Integrated custom modules and self-developed methods to extend game logic.

(c) Injected Project MER (DLL-based modules) to manipulate server–client logic at runtime.

(d) Used HarmonyLib with IL injection to modify compiled bytecode for deeper control.

This type of work requires an advanced understanding of game engine architecture and real-time systems. My implementation includes:

(a) Raycasting – to detect gaze coordinates for precise object spawning.

(b) Prefab Instantiation – to dynamically place pre-designed assets into the scene.

(c) Physics Layers & Colliders – to handle real-time collisions and trigger-based behavior.

(d) Object Pooling – to minimize garbage collection and ensure memory efficiency.

(e) Multiplayer Synchronization – to align object state and behavior across server and clients.

This is a highly complex topic that blends game design with low-level systems control. I implemented it independently, without formal instruction.

Supporting media such as the yellow shield image, demo video, and my public service platform MAMBAB will be included in my online portfolio.
<img width="577" height="348" alt="KakaoTalk_Photo_2025-08-17-19-34-39" src="https://github.com/user-attachments/assets/40612760-afad-4f86-b0a2-706e77a48dd2" />
<img width="577" height="348" alt="image" src="https://github.com/user-attachments/assets/93da984b-11f9-4436-a311-aed80265bd8a" />
