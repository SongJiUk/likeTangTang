using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo
{
    /*


    1. 조이스틱 만들기
        1. 플레이어 이동 및 카메라도 같이 이동
        2. event를 이용하여 실시간 정보 바꾸기
    2. Util이나 Define을 만들어놓기
        1. Util( 자주 쓰는 함수들 제네릭으로 만들어놓으면 편함)
        2. Define(enum으로 종류 관리)
    3. 매니저로 정리해서 관리하기
        1. 큰 틀의 매니저를 만들어서 그 매니저에서 모든걸 관리
        
        GameManager -> 인게임 정보
        델리게이트를 사용하여 통신원활하게
        
    

    4. Addressable로 리소스 관리하기
        1. 로딩할때 한번에 불러올 리소스들 확인
    5. ObjectManager를 만들어서 오브젝트 관리하기
        1. 컨트롤러는 Base를 부모로 사용
        2. 상속관계를 이용해서 관리
        3. 데미지를 받는부분은 맞는쪽에서 관리하는게 좋음.
    6. poolManager로 재활용하기
        1. 유니티에서 제공하는 ObjectPool 사용
        2. 사용법은 (create, get, release, destory)
    7. DataManager를 만들어 xml or json으로 데이터 관리하기
        1. json이 직관적으론 좋긴한데 xml이 계층관계를 보는게 쉬움
        2. 뭘 할진 선택임
    8. 일단 각 매니저들과 컨트롤러를 어떤식으로 코딩해서 사용할지 생각해봐야될거같음.



    9. 젬 떨구기
        1. 
그 이후는

젬떨구기, 최적화, 스킬, Projectile(화살 같은것) 쏘는거, 맵, UI, 보스, AI등

일단 다 보고나서 다시 확인해봐야될듯

젬을 획득할때 플레이어에서 거리를 관리하기(그리드 방식으로)

ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

 그리드 방식( 직접 만들기 vs Unity에서 제공하는 Grid 사용하기)


WorldToCell -> 그리드에 나의 월드좌표를 건내주면 셀 번호를 돌려준다.

Add
Remove
Get
한방에 불러오는 코드(내 위치, Range)



ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

스킬

데이터( 스킬 데이터 관리가 가장 어려움)
하나에 가득 넣어버리면 관리가 힘들어진다.

스크립트 여러개 생성 vs 하나에 넣어서 생성

코루틴사용할때 isVaild체크 해야됌( null 체크하면 꼬일수도있음.)
Utils 쪽에 만들거나 Extension을 하나 만들어서 activeSelf 를 추가해서 검사해주는거임
굳이 안만들어서 사용해도됨 ( activeSelf를 &&으로 검사해주면 되니까) 근데 만들어주면 편함.



ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
NOTE : memoTree 설정 방법 
     Setting.Json

     "todo-tree.general.tags": [
        "BUG",
        "HACK",
        "FIXME",
        "TODO",
        "XXX",
        "[ ]",
        "[x]",
        "NOTE",
        "CHECKLIST",
        "WARNING"
    ],
    "todo-tree.highlights.customHighlight": {
        
       "CHECKLIST": {
      "background": "#20a904",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "check-circle-fill",
      "iconColour": "#20a904",
      "type": "text"
    },
    "NOTE": {
      "background": "#ff0404",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "star-fill",
      "iconColour": "#ffc404",
      "type": "text"
    },
    "TODO": {
      "background": "#b782f9",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "pin",
      "iconColour": "#b782f9"
    },
    "WARNING": {
      "background": "#ffc404",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "alert",
      "iconColour": "#ff9f04"
    },
    "[ ]": {
      "background": "#f87364",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "x",
      "iconColour": "#f87364",
      "type": "text"
    },
    "[x]": {
      "background": "#20a904",
      "foreground": "#ffffff",
      "gutterIcon": true,
      "icon": "check",
      "iconColour": "#20a904",
      "type": "text"
    }
  },
  "todo-tree.regex.regex": "((\\*|//|#|<!--|;|/\\*|^)\\s*($TAGS)|^\\s*- \\[ \\])", 추가


[x] 몬스터 생성 위치 정해주기
Uitls에 몬스터 생성 위치를 정해주는 함수를 하나 만들어서 사용하면 편함
[x] 평타 스킬 만들기
평타마다 스크립트를 넣어서, ColiisionEnter로 사용해줌
[x] 무한맵 만들기
카메라에 트리거를 달아서 카메라가 해당 타일을 벗어나면 해당 타일을 카메라의 방향으로 옮겨주면됌.


ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
[x] SkillController -> SkillBase(안에 스킬에 필요한것들 넣어놓기, owner, type, data, level, damage등)
[x] 스킬들 최적화해서 합치기(FireProjectile, EgoSword) => 스킬 통합
[x] EgoSword개선(물리 인의적응로 껏다 키는거 없앰, 파티클시스템을 이용.), 피직스 통합관리
[x] 반복스킬, 액티브스킬 나눠서 관리하기(Repeat, Sequence등)


[ ] 보스몬스터 대쉬 추가, 다시보고 확인하기.
[ ] UI 마무리

ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

만들어야할것들 
씬 : 로딩씬, 타이틀씬, 로비씬
팝업 : 씬에 맞는 팝업
데이터 : 데이터 정보(굳이 다 안하고 기본적인것만 하자.)

     */ 


     
}
