namespace Onlyoffice.Api.Models;

public class CommentsDao : MultiResponseDao<CommentDto> { }

public class SingleCommentDao : SingleResponseDao<CommentDto> { }

public class CommentDto : IEntityDto<string>
{
    public string Id { get; set; } = "";
    public UserDto? CreatedBy { get; set; }
    public string? Text { get; set; }
    public string? ParentId { get; set; }
    public bool Inactive { get; set; }
    public bool CanEdit { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public class CommentUpdateData
{
    public string? Content { get; set; }
    public string? ParentId { get; set; }
}
