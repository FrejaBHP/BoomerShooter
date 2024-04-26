public partial class WeaponInfo {
    public Ammotype AmmoType { get; private set; }
    public int UpState { get; private set; }
    public int DownState { get; private set; }
    public int ReadyState { get; private set; }
    public int AttackState { get; private set; }

    public WeaponInfo(Ammotype ammo, int wUpState, int wDownState, int wReadyState, int wAtkState) {

    }
}
