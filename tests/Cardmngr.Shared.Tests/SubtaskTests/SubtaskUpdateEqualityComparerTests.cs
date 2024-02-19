using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Helpers;
using FluentAssertions;

namespace Cardmngr.Shared.Tests.SubtaskTests
{
    public class SubtaskUpdateEqualityComparerTests
    {
        private readonly IEqualityComparer<Subtask> comparer = new SubtaskUpdateEqualityComparer();

        [Fact]
        public void Equals_ShouldReturnFalse_WhenTitleDiff()
        {
            var x = new Subtask { Title = "x" };
            var y = new Subtask { Title = "y" };

            comparer.Equals(x, y).Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenResponsibleDiff()
        {
            var x = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };
            var y = new Subtask { Title = "x", Responsible = new UserInfo { Id = "1" } };

            comparer.Equals(x, y).Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenEquals()
        {
            var x = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };
            var y = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };

            comparer.Equals(x, y).Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOneIsNull()
        {
            Subtask x = null!;
            var y = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };

            comparer.Equals(x, y).Should().BeFalse();
            comparer.Equals(y, x).Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenIdsDiff()
        {
            var x = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" }, Id = 1 };
            var y = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };

            comparer.Equals(x, y).Should().BeFalse();
        }

        [Fact]
        public void Equals_HaveNoEffect_WithOtherProps()
        {
            var x = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" }, Description = "abc" };
            var y = new Subtask { Title = "x", Responsible = new UserInfo { Id = "2" } };

            comparer.Equals(x, y).Should().BeTrue();
        }
    }
}
