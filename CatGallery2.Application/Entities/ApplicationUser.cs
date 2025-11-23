using Microsoft.AspNetCore.Identity;

namespace CatGallery2.Application.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public ICollection<WishlistCat>? WishlistCats { get; set; } = new List<WishlistCat>();
    public ICollection<CatProduct>? CatProducts { get; set; } = new List<CatProduct>();
}