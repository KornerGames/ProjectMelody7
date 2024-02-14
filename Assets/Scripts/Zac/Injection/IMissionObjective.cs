namespace Zac
{
    public interface IMissionObjective
    {

        public interface ISetter
        {

            void Show(DialogueLineModel lineModel);
            void Hide();

        }

        public interface IGetter
        {

            bool IsActive();

        }

    }

}
