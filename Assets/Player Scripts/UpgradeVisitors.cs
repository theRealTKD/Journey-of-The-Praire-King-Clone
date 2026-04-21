public class SpeedUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {
        player.moveSpeed += 2f;
    }
}
public class DamageUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {
        player.damageBoost *=1.2f;
    }
}
public class FireRateUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {
        player.fireRate /= 1.2f;
    }
}