namespace FluentValidation;

public abstract class HttpValidator<T> : JsonValidator<T>
{
    protected readonly HttpMethod HttpMethod;

    protected HttpValidator(string httpMethod)
    {
        HttpMethod = new HttpMethod(httpMethod);
    }

    public IConditionBuilder When(HttpMethod httpMethod, Action action)
        => When(x => HttpMethod == httpMethod, action);

    public IConditionBuilder Unless(HttpMethod httpMethod, Action action)
        => Unless(x => HttpMethod == httpMethod, action);
}
