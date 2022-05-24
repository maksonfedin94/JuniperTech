namespace Notes.Helpers
{
    public class TryResult<TResult>
    {
        public TryResult(bool operationSucceeded, TResult result = default(TResult))
        {
            OperationSucceeded = operationSucceeded;
            Value = result;
        }

        public bool OperationSucceeded { get; }

        public TResult Value { get; }
    }
}
