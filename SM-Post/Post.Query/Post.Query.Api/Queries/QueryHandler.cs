using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class QueryHandler : IQueryHandler
{
    /*
     The QueryHandler class is the concrete colleague class (and the IqueryHandler interface the Colleague or Abstract Colleague) 
       that handles queries by invoking the relevant repository method to obtain one or more social media post records (with possibly comments) 
       from the read database, and once retrieved, maps it to a list of PostEntity that is returned to the controller by the mediator.

     Query Objects are often classes with no properties or fields, yet the name of a query object should always clearly express its intent, for example, FindAllPostAsync
     */

    private readonly IPostRepository _postRepository;

    public QueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostEntity>> HandleAsync(FindAllPostQuery query)
    {
        return await _postRepository.ListAllAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        var post = await _postRepository.GetByIdAsync(query.Id);
        return new List<PostEntity> { post };
    }


    public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
    {
        return await _postRepository.ListByAuthorAsync(query.Author);
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query)
    {
        return await _postRepository.ListWithCommentsAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
    {
        return await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
    }
}
