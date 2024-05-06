namespace Onlyoffice.Api.Models;

public class GroupsDao : MultiResponseDao<GroupDto> { }

public class GroupDao : SingleResponseDao<GroupDto> { }

public class GroupDto
{
      public string? Id { get; set; }
      public string? Name { get; set; }
      public string? Manager { get; set; }
}