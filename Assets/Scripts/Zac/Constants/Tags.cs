namespace Zac
{

    public enum Tags 
    {
        Player = 0,

        EnemyHostile = 100,
        EnemyNonHostile = 101,

        ItemHealing = 200,



    }

    public static class TagsUtil
    {

        public static bool IsEnemy(string tag)
            => (tag != null) &&
                (tag.Equals(Tags.EnemyHostile.ToString())
                || tag.Equals(Tags.EnemyNonHostile.ToString()));

    }

}