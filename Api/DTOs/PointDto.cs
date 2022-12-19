namespace Api.DTOs
{
    public class PointDto
    {
        public ObjectIdDto _Id { get; set; }

        public bool Approved { get; set; }
       
        public ObjectIdDto User_Id { get; set; }

        public int Point { get; set; }
        public string Prize { get; set; }
    }
}
