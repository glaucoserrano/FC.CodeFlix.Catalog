using FC.Codeflix.Catalog.Application.UseCases.Category.Commom;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput,CategoryModelOutput>
{

}
