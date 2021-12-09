namespace HSBM.EntityModel.AmenityMaster
{
    public class AmenityMasterRequest
    {
        public int Id { get; set; }
        public string AmenityName { get; set; }
        public bool IncludeIsDeleted { get; set; }
    }

}