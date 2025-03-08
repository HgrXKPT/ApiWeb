namespace WebApplication1.Utils
{
    public class Validacoes
    {
        public static void VerificarEspacoVazioOuNulo(string value, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(mensagem);
            }
        }
    }
}
