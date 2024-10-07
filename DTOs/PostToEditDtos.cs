namespace DotnetAPI.Models;
public class PostToEditDtos{
        public int PostId{get;set;}
        public string PostTitle{get;set;}
        public string PostContent{get;set;}

        public PostToEditDtos()
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