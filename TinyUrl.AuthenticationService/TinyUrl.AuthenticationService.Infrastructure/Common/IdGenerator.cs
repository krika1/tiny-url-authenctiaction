namespace TinyUrl.AuthenticationService.Infrastructure.Common
{
    public static class IdGenerator
    {
        public static int GeneratateUserId()
        {
            var guid = Guid.NewGuid().ToString();

            string numericString = new string(guid.Where(char.IsDigit).ToArray());

            if (numericString.Length > 9) 
            {
                numericString = numericString.Substring(0, 9);
            }

            return int.Parse(numericString);
        }
    }
}
