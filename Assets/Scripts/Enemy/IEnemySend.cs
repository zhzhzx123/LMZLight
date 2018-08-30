

public interface IEnemySend
{
    void SwitchState(int state);

    void SetIDandNum(int id, int num);

    void Hurt(float hurt);

    void DestroyThis();
}
