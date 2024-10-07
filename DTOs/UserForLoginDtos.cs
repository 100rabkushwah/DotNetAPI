namespace DotnetAPI.DTOs{
    public  class UserForLoginDtos
    {
         public string? Email{get; set;}
         public string? Password{get; set;}
         public UserForLoginDtos()
         {
            if(Email == null)
                Email = "";

            if(Password == null)
                Password = "";
         }
    }
}