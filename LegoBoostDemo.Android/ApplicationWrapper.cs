namespace LegoBoostDemo.Droid
{
    public class ApplicationWrapper
    {
        private static App instance;

        public static App Application
        {
            get
            {
                return instance ??= new App(new AndroidInitializer());
            }
        }
    }
}