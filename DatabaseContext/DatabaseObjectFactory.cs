namespace DatabaseContext
{
    public delegate object DatabaseObjectFactory(Type queryType);

    public static class DatabaseObjectFactoryExtensions
    {
        public static T Create<T>(this DatabaseObjectFactory factory)
        {
            return (T)factory(typeof(T));
        }
    }
}