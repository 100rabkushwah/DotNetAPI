namespace DotnetAPI.DTOs;
public class PostToAddDtos{
        public string PostTitle{get;set;}
        public string PostContent{get;set;}

        public PostToAddDtos()
        {
            if(PostTitle == null)
            {
                PostTitle = "";
            }
            if(PostContent == null)
            {
                PostContent = "";
            }
        }
}