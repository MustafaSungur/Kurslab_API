

namespace Education.Data.Repositories.Abstract
{
	public interface IRepositoryManager
	{
		ICategoryRepository CategoryRepository { get; }
		ICommentLikeRepository CommentLikeRepository { get; }
		IContentRepository ContentRepository { get; }
		IRatingRepository RatingRepository { get; }
		ICommentRepository CommentRepository { get; }
		ITagRepository TagRepository { get; }
		IContentTagRepository ContentTagRepository { get; }
		IContentUserRepository ContentUserRepository { get; }

		Task SaveAsync();
	}
}
