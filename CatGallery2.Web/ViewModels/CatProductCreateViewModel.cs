using System.ComponentModel.DataAnnotations;
using CatGallery2.Application.Entities;
using CatGallery2.Web.ViewModels.Validation;

namespace CatGallery2.Web.ViewModels;

public sealed class CatProductCreateViewModel
{
    [Required(ErrorMessage = "Придумайте имя своему коту")]
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "Цена вашего котика должна быть больше 0")]
    public decimal Price { get; set; }
    public List<string> Tags { get; set; }
    [MaxFileCount(4)]
    public List<IFormFile> CatPhotos { get; set; } = new List<IFormFile>();

    public ICollection<TagProduct>? TagProducts { get; set; } = new List<TagProduct>();
}