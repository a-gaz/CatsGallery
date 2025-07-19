using System.Collections.Concurrent;
using CatsGallery.Application.Entities;
using CatsGallery.Application.Interfaces.Repos;

namespace CatsGallery.Infrastructure.Repos;

public class UserRepo : IUserRepo
{
    public List<Cat> InitialCats { get; }
    public ConcurrentQueue<Cat> CatQueue { get; }
}