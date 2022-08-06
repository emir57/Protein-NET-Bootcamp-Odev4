using Core.Entity;
using WriteParameter.Attributes;

namespace Pagination.Entity.Concrete
{
    public class Person : IEntity
    {
        [IdColumn]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DeletedDate { get; set; }
        [IgnoreColumn]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
