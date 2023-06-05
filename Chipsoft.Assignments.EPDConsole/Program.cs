
namespace Chipsoft.Assignments.EPDConsole
{
    public class Program
    {
        //Don't create EF migrations, use the reset db option
        //This deletes and recreates the db, this makes sure all tables exist

        #region FreeCodeForAssignment
        static void Main(string[] args)
        {
            ConsoleManager manager = new ConsoleManager();
            while (manager.ShowMenu())
            {
                //Continue
            }
        }

        #endregion
    }
}