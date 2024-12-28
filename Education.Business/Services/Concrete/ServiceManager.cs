using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Services.Concrete;
using Education.Data.Repositories.Abstract;
using Education.Entity.Models;
using Microsoft.AspNetCore.Identity;

public class ServiceManager : IServiceManager
{
	private readonly Lazy<IContentService> _contentService;
	private readonly Lazy<ICategoryService> _categoryService;
	private readonly Lazy<ICommentService> _commentService;
	private readonly Lazy<ICommentLikeService> _commentLikeService;
	private readonly Lazy<IContentTagService> _contentTagService;
	private readonly Lazy<IContentUserService> _contentUserService;
	private readonly Lazy<IRatingService> _ratingService;
	private readonly Lazy<ITagService> _tagService;
	private readonly Lazy<IApplicationUserService> _ApplicationUserService;

	private readonly UserManager<ApplicationUser> _userManager;

	public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;

		_contentService = new Lazy<IContentService>(() => new ContentManager(repositoryManager, mapper));
		_categoryService = new Lazy<ICategoryService>(() => new CategoryManager(repositoryManager, mapper));
		_commentService = new Lazy<ICommentService>(() => new CommentManager(repositoryManager, mapper));
		_commentLikeService = new Lazy<ICommentLikeService>(() => new CommentLikeManager(repositoryManager, mapper));
		_contentTagService = new Lazy<IContentTagService>(() => new ContentTagManager(repositoryManager, mapper));
		_contentUserService = new Lazy<IContentUserService>(() => new ContentUserManager(repositoryManager, mapper));
		_ratingService = new Lazy<IRatingService>(() => new RatingManager(repositoryManager, mapper));
		_tagService = new Lazy<ITagService>(() => new TagManager(repositoryManager, mapper));
		_ApplicationUserService = new Lazy<IApplicationUserService>(() => new ApplicationUserManager(_userManager, mapper, repositoryManager));
	}

	public IContentService ContentService => _contentService.Value;
	public ICategoryService CategoryService => _categoryService.Value;
	public ICommentService CommentService => _commentService.Value;
	public ICommentLikeService CommentLikeService => _commentLikeService.Value;
	public IContentTagService ContentTagService => _contentTagService.Value;
	public IContentUserService ContentUserService => _contentUserService.Value;
	public IRatingService RatingService => _ratingService.Value;
	public ITagService TagService => _tagService.Value;
	public IApplicationUserService ApplicationUserService => _ApplicationUserService.Value;
}
