using AutoMapper;
using Education.Entity.DTOs.ApplicationUserDTO;
using Education.Entity.DTOs.CategoryDTO;
using Education.Entity.DTOs.CommentDTO;
using Education.Entity.DTOs.CommentLikeDTO;
using Education.Entity.DTOs.ContentDTO;
using Education.Entity.DTOs.ContentTagDTO;
using Education.Entity.DTOs.ContentUserDTO;
using Education.Entity.DTOs.RaitingDTO;
using Education.Entity.DTOs.TagDTO;
using Education.Entity.Models;

namespace Education.Business.MappingProfiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Category mappings
			CreateMap<Category, CategoryResponseDto>().ReverseMap();
			CreateMap<CategoryRequestDto, Category>();

			// Comment mappings
			CreateMap<Comment, CommentResponseDto>().ReverseMap();
			CreateMap<CommentRequestDto, Comment>();

			// CommentLike mappings
			CreateMap<CommentLike, CommentLikeResponseDto>().ReverseMap();
			CreateMap<CommentLikeRequestDto, CommentLike>();

			// Content mappings
			CreateMap<Content, ContentResponseDto>().ReverseMap();
			CreateMap<ContentRequestDto, Content>();

			// ContentTag mappings
			CreateMap<ContentTag, ContentTagResponseDto>().ReverseMap();
			CreateMap<ContentTagRequestDto, ContentTag>();

			// ContentUser mappings
			CreateMap<ContentUser, ContentUserResponseDto>().ReverseMap();
			CreateMap<ContentUserRequestDto, ContentUser>();

			// Rating mappings
			CreateMap<Rating, RatingResponseDto>().ReverseMap();
			CreateMap<RatingRequestDto, Rating>();

			// Tag mappings
			CreateMap<Tag, TagResponseDto>().ReverseMap();
			CreateMap<TagRequestDto, Tag>();	

			// ApplicationUser mappings
			CreateMap<ApplicationUser, ApplicationUserResponseDto>().ReverseMap();
			CreateMap<ApplicationUserRequestDto, ApplicationUser>();

			// CategoryStatistics mappings
			CreateMap<CategoryStatistics, CategoryStatisticsResponseDto>().ReverseMap();

		}
	}
}
