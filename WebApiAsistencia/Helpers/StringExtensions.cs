namespace WebApiAsistencia.Helpers
{
    public static class StringExtensions
    {
        public static string CapitalizarPrimeraLetra(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}
