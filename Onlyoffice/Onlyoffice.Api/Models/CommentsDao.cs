namespace Onlyoffice.Api.Models;

public class CommentsDao : HttpResponseDaoBase
{
    public List<CommentDto> Response { get; set; } = [];
}

public class SingleCommentDao : HttpResponseDaoBase
{
    public CommentDto? Response { get; set; }
}

public class CommentDto
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
