using Education.Data.Repositories.Abstract;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ICommentLikeRepository> _commentLikeRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<IContentRepository> _contentRepository;
        private readonly Lazy<IContentTagRepository> _contentTagRepository;
        private readonly Lazy<IRatingRepository> _ratingRepository;
        private readonly Lazy<ITagRepository> _tagRepository;
        private readonly Lazy<IContentUserRepository> _contentUserRepository;

        public RepositoryManager(AppDbContext repositoryContext)
        {
            _context = repositoryContext;
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
			_commentLikeRepository = new Lazy<ICommentLikeRepository>(() => new CommentLikeRepository(_context));   
			_commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(_context));
			_contentRepository = new Lazy<IContentRepository>(() => new ContentRepository(_context));
			_contentTagRepository = new Lazy<IContentTagRepository>(() => new ContentTagRepository(_context));
			_ratingRepository = new Lazy<IRatingRepository>(() => new RatingRepository(_context));
			_tagRepository = new Lazy<ITagRepository>(() => new TagRepository(_context));
			_contentUserRepository = new Lazy<IContentUserRepository>(() => new ContentUserRepository(_context));
		}

        public ICategoryRepository CategoryRepository => _categoryRepository.Value;
        public ICommentLikeRepository CommentLikeRepository => _commentLikeRepository.Value;
        public IContentRepository ContentRepository => _contentRepository.Value;
        public IRatingRepository RatingRepository => _ratingRepository.Value;
		public ICommentRepository CommentRepository => _commentRepository.Value;
		public ITagRepository TagRepository => _tagRepository.Value;
		public IContentTagRepository ContentTagRepository => _contentTagRepository.Value;
        public IContentUserRepository ContentUserRepository => _contentUserRepository.Value;

		public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
