namespace Questao5.Domain.Enumerators
{
    public enum TipoMovimento
    {
        Credito='C', //Key 67
        Debito='D' //Key 68
    }

    public static class TipoMovimentoExtensions
    {
        public static string ToChar(this TipoMovimento tipo)
        {
            return ((char)tipo).ToString()[0].ToString();
        }
    }
}
