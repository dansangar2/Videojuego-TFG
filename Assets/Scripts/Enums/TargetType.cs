namespace Enums
{
    /**<summary>
     It define the target type.
     <para>"Both" means that the target is being the enemies and/or playable characters.</para>
    </summary>*/
    public enum TargetType
    {
        Enemy,
        Group,
        Both
    }

    /**<summary>
     It define the target range.
     <para>"More Than One", "Random" and "RandomInOne" must be a integer as value of hit </para>
    </summary>*/
    public enum TargetRange
    {
        One,
        All,
        MoreThanOne,
        Random,
        Himself
    }
}