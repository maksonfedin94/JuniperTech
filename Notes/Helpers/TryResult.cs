namespace Notes.Helpers
{
    public static class TryResult
    {
        public static TryResult<TResult> Success<TResult>(TResult result)
        {
            return new TryResult<TResult>(true, result);
        }

        public static TryResult<TResult> Unsucceed<TResult>()
        {
            return new TryResult<TResult>(false, default(TResult));
        }
    }
}
