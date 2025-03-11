using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo
{
    /*


    1. 조이스틱 만들기
    1. 플레이어 이동 및 카메라도 같이 이동
    2. Updata문을 돌리지 말고 event를 이용하여 실시간 정보 바뀌기
2. Util이나 Define을 만들어놓기
    1. Util( 자주 쓰는 함수들 제네릭으로 만들어놓으면 편함)
    2. Define(enum으로 종류 관리)
3. 매니저로 정리해서 관리하기
    1. 큰 틀의 매니저를 만들어서 그 매니저에서 모든걸 관리
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

그 이후는

젬떨구기, 최적화, 스킬, Projectile(화살 같은것) 쏘는거, 맵, UI, 보스, AI등

일단 다 보고나서 다시 확인해봐야될듯

젬을 획득할때 플레이어에서 거리를 관리하기(그리드 방식으로)


    1

 그리드 방식( 직접 만들기 vs Unity에서 제공하는 Grid 사용하기)


WorldToCell -> 그리드에 나의 월드좌표를 건내주면 셀 번호를 돌려준다.

Add
Remove
Get
한방에 불러오는 코드(내 위치, Range)



------------

스킬

데이터( 스킬 데이터 관리가 가장 어려움)
하나에 가득 넣어버리면 관리가 힘들어진다.

스크립트 여러개 생성 vs 하나에 넣어서 생성

코루틴사용할때 isVaild체크 해야됌( null 체크하면 꼬일수도있음.)
Utils 쪽에 만들거나 Extension을 하나 만들어서 activeSelf 를 추가해서 검사해주는거임
굳이 안만들어서 사용해도됨 ( activeSelf를 &&으로 검사해주면 되니까) 근데 만들어주면 편함.

     */
}
