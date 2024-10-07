namespace DotnetAPI.DTOs{
    public class UserForLoginConfirmationDtos
    {
         public byte[] PasswordHash{get; set;}
         public byte[] PasswordSalt{get; set;}
         public UserForLoginConfirmationDtos()
         {
            if(PasswordHash == null)
                PasswordHash = new byte[0];

            if(PasswordSalt == null)
                 PasswordSalt = new byte[0];
         }
    }
}