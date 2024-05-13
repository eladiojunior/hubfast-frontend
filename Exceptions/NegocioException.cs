namespace hubfast_frontend.Exceptions;

public class NegocioException: Exception
{
    public NegocioException(string mensagem) : base(mensagem) {}
}