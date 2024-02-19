using Cardmngr.Domain.Entities;
using Cardmngr.Shared.DomainExtensions;
using FluentAssertions;

namespace Cardmngr.Shared.Tests.SubtaskTests
{
    public class SubtaskExtensionsTests
    {
        [Fact]
        public void NeedUpdate_ShouldReturnEmpty_WhenIsEmpty()
        {
            // Assign
            List<Subtask> first = [];
            List<Subtask> second = [new Subtask()];

            // Act
            var result = first.NeedUpdate(second);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void NeedUpdate_ShouldReturnEmpty_WhenOtherEmpty()
        {
            // Assign
            List<Subtask> first = [new Subtask()];
            List<Subtask> second = [];

            // Act
            var result = first.NeedUpdate(second);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void NeedUpdate_ShouldReturnEmpty_WhenNoSameIdsSubtask()
        {
            // Assign
            List<Subtask> first = [new Subtask { Id = 1 }];
            List<Subtask> second = [new Subtask { Id = 2 }];

            // Act
            var result = first.NeedUpdate(second);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void NeedUpdate_ShouldReturnEmpty_WhenSubtasksEquals()
        {
            // Assign
            List<Subtask> first = [new Subtask { Id = 1, Title = "test" }];
            List<Subtask> second = [new Subtask { Id = 1, Title = "test" }];

            // Act
            var result = first.NeedUpdate(second);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void NeedUpdate_ShouldReturnValues_ThatNeedUpdate()
        {
            // Assign
            List<Subtask> first = [new Subtask { Id = 1, Title = "test" }, new Subtask()];
            List<Subtask> second = [new Subtask { Id = 1, Title = "test1" }];

            // Act
            var result = first.NeedUpdate(second);

            // Assert
            result.Should().ContainSingle();

            result.Single().Should().Be(second[0]);
        }
    }
}