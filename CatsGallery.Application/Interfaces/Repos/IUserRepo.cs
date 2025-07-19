using System.Collections.Concurrent;
using CatsGallery.Application.Entities;

namespace CatsGallery.Application.Interfaces.Repos;

public interface IUserRepo
{
    List<Cat> InitialCats { get; } 
    ConcurrentQueue<Cat> CatQueue { get; }
}