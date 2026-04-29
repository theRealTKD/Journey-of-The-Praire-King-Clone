public class SpeedUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {
        player.movement.moveSpeed += 2f; 
    }
}

public class DamageUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {

            player.shooting.damageBoost *= 1.2f;
    }
}

public class FireRateUpgrade : IUpgradeVisitor
{
    public void Visit(playerControl player)
    {

        player.shooting.fireRate /= 1.2f;
    }
}