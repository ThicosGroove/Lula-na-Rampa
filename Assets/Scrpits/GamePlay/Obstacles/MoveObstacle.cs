
public class MoveObstacle : MoveBase
{
    protected override void DieBehaviour()
    {
        Destroy(this.gameObject);
    }

    protected override void MoveBehaviour()
    {
        
    }
}