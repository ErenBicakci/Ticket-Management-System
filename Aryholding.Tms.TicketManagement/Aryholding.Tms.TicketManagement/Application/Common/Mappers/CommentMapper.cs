

using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Application.Common.Mappers
{
    public class CommentMapper
    {
        public static CommentResponseDto MapToResponseDto(Comment comment)
        {
            return new CommentResponseDto
            {
                TicketId = comment.TicketId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                Description = comment.Description,
                Id = comment.Id,
                Username = comment.User.UserName
            };
        }

        public static IEnumerable<CommentResponseDto> MapToResponseDtos(IEnumerable<Comment> comments)
        {
            return comments.Select(MapToResponseDto);
        }

    }
}
