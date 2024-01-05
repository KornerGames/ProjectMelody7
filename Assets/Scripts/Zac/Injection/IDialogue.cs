namespace Zac
{
    public interface IDialogue
    {

        public interface ISetter
        {

            void Show(DialogueLineModel lineModel);
            void Hide();

        }

        public interface IGetter
        { 
        
        }

    }

}
