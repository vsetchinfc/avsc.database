namespace AVSC.Database.Exceptions
{
    public class NotSupportedDatabaseException : System.Exception
    {
        public NotSupportedDatabaseException () { }

        public NotSupportedDatabaseException (string message) : base(message) { }

        public NotSupportedDatabaseException (string message, System.Exception inner) : base(message, inner) { } 

        protected NotSupportedDatabaseException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}