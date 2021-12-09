namespace HSBM.EntityModel.CategoryMaster
{
    public class CategoryMasterRequest
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool IncludeIsDeleted { get; set; }
    }

}