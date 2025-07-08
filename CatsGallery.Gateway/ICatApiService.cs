namespace CatsGallery.Gateway;
// todo Гейтвей - фигня, ненужная и вредная. Как минимум это протекшая абстракция (httpResponse) в слое application. У тебя должна быть сборка Infrastructure.CatApi с  CatService внутри
// todo третий слой - инфраструктурный, но из него не должны торчать детали (НАДО РАЗОБРАТЬСЯ ЧТО ЭТО)
// todo А у тебя бессовестно торчит HttpResponseMessage (ШО?)
public interface ICatApiService
{
    Task<HttpResponseMessage> GetRandomCatAsync();
    Task<HttpResponseMessage> GetCatByIdAsync(string id);
}