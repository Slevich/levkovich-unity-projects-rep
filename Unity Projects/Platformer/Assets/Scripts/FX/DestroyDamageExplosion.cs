using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDamageExplosion : MonoBehaviour
{
    //Метод самоуничтожает объект. Может использоваться в конце анимации для удаления объекта со сцены.
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
