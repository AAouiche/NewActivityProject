namespace Domain.Validation
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        public static Result<T> SuccessResult(T value) => new Result<T> { Success = true, Value = value };
        public static Result<T> Failure(string error) => new Result<T> { Success = false, Error = error };
    }
}