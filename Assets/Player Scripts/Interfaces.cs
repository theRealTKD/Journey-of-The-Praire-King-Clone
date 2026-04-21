public interface IUpgradeVisitor {
    void Visit(playerControl player); 
}

public interface IAcceptUpgrades {
    void Accept(IUpgradeVisitor visitor);
}